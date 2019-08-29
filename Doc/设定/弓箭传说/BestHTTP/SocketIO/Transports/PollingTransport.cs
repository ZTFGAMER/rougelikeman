namespace BestHTTP.SocketIO.Transports
{
    using BestHTTP;
    using BestHTTP.Logger;
    using BestHTTP.SocketIO;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Text;

    internal sealed class PollingTransport : ITransport
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TransportStates <State>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private SocketManager <Manager>k__BackingField;
        private HTTPRequest LastRequest;
        private HTTPRequest PollRequest;
        private Packet PacketWithAttachment;
        private List<Packet> lonelyPacketList = new List<Packet>(1);

        public PollingTransport(SocketManager manager)
        {
            this.Manager = manager;
        }

        public void Close()
        {
            if (this.State != TransportStates.Closed)
            {
                this.State = TransportStates.Closed;
            }
        }

        private void OnPacket(Packet packet)
        {
            if ((packet.AttachmentCount != 0) && !packet.HasAllAttachment)
            {
                this.PacketWithAttachment = packet;
            }
            else
            {
                switch (packet.TransportEvent)
                {
                    case TransportEventTypes.Open:
                        if (this.State != TransportStates.Opening)
                        {
                            HTTPManager.Logger.Warning("PollingTransport", "Received 'Open' packet while state is '" + this.State.ToString() + "'");
                        }
                        else
                        {
                            this.State = TransportStates.Open;
                        }
                        break;

                    case TransportEventTypes.Message:
                        if (packet.SocketIOEvent == SocketIOEventTypes.Connect)
                        {
                            this.State = TransportStates.Open;
                        }
                        break;
                }
                ((IManager) this.Manager).OnPacket(packet);
            }
        }

        private void OnPollRequestFinished(HTTPRequest req, HTTPResponse resp)
        {
            this.PollRequest = null;
            if (this.State != TransportStates.Closed)
            {
                string str = null;
                switch (req.State)
                {
                    case HTTPRequestStates.Finished:
                        if (HTTPManager.Logger.Level <= Loglevels.All)
                        {
                            HTTPManager.Logger.Verbose("PollingTransport", "OnPollRequestFinished: " + resp.DataAsText);
                        }
                        if (resp.IsSuccess)
                        {
                            this.ParseResponse(resp);
                        }
                        else
                        {
                            str = $"Polling - Request finished Successfully, but the server sent an error. Status Code: {resp.StatusCode}-{resp.Message} Message: {resp.DataAsText} Uri: {req.CurrentUri}";
                        }
                        break;

                    case HTTPRequestStates.Error:
                        str = (req.Exception == null) ? "No Exception" : (req.Exception.Message + "\n" + req.Exception.StackTrace);
                        break;

                    case HTTPRequestStates.Aborted:
                        str = $"Polling - Request({req.CurrentUri}) Aborted!";
                        break;

                    case HTTPRequestStates.ConnectionTimedOut:
                        str = $"Polling - Connection Timed Out! Uri: {req.CurrentUri}";
                        break;

                    case HTTPRequestStates.TimedOut:
                        str = $"Polling - Processing the request({req.CurrentUri}) Timed Out!";
                        break;
                }
                if (!string.IsNullOrEmpty(str))
                {
                    ((IManager) this.Manager).OnTransportError(this, str);
                }
            }
        }

        private void OnRequestFinished(HTTPRequest req, HTTPResponse resp)
        {
            this.LastRequest = null;
            if (this.State != TransportStates.Closed)
            {
                string str = null;
                switch (req.State)
                {
                    case HTTPRequestStates.Finished:
                        if (HTTPManager.Logger.Level <= Loglevels.All)
                        {
                            HTTPManager.Logger.Verbose("PollingTransport", "OnRequestFinished: " + resp.DataAsText);
                        }
                        if (resp.IsSuccess)
                        {
                            if (req.MethodType != HTTPMethods.Post)
                            {
                                this.ParseResponse(resp);
                            }
                        }
                        else
                        {
                            str = $"Polling - Request finished Successfully, but the server sent an error. Status Code: {resp.StatusCode}-{resp.Message} Message: {resp.DataAsText} Uri: {req.CurrentUri}";
                        }
                        break;

                    case HTTPRequestStates.Error:
                        str = (req.Exception == null) ? "No Exception" : (req.Exception.Message + "\n" + req.Exception.StackTrace);
                        break;

                    case HTTPRequestStates.Aborted:
                        str = $"Polling - Request({req.CurrentUri}) Aborted!";
                        break;

                    case HTTPRequestStates.ConnectionTimedOut:
                        str = $"Polling - Connection Timed Out! Uri: {req.CurrentUri}";
                        break;

                    case HTTPRequestStates.TimedOut:
                        str = $"Polling - Processing the request({req.CurrentUri}) Timed Out!";
                        break;
                }
                if (!string.IsNullOrEmpty(str))
                {
                    ((IManager) this.Manager).OnTransportError(this, str);
                }
            }
        }

        public void Open()
        {
            ulong num2;
            string format = "{0}?EIO={1}&transport=polling&t={2}-{3}{5}";
            if (this.Manager.Handshake != null)
            {
                format = format + "&sid={4}";
            }
            bool flag = !this.Manager.Options.QueryParamsOnlyForHandshake || (this.Manager.Options.QueryParamsOnlyForHandshake && (this.Manager.Handshake == null));
            object[] args = new object[6];
            args[0] = this.Manager.Uri.ToString();
            args[1] = 4;
            args[2] = this.Manager.Timestamp.ToString();
            SocketManager manager = this.Manager;
            manager.RequestCounter = (num2 = manager.RequestCounter) + ((ulong) 1L);
            args[3] = num2.ToString();
            args[4] = (this.Manager.Handshake == null) ? string.Empty : this.Manager.Handshake.Sid;
            args[5] = !flag ? string.Empty : this.Manager.Options.BuildQueryParams();
            new HTTPRequest(new Uri(string.Format(format, args)), new OnRequestFinishedDelegate(this.OnRequestFinished)) { 
                DisableCache = true,
                DisableRetry = true
            }.Send();
            this.State = TransportStates.Opening;
        }

        private void ParseResponse(HTTPResponse resp)
        {
            try
            {
                if (((resp != null) && (resp.Data != null)) && (resp.Data.Length >= 1))
                {
                    int num2;
                    for (int i = 0; i < resp.Data.Length; i += num2)
                    {
                        PayloadTypes text = PayloadTypes.Text;
                        num2 = 0;
                        if (resp.Data[i] < 0x30)
                        {
                            text = (PayloadTypes) resp.Data[i++];
                            for (byte j = resp.Data[i++]; j != 0xff; j = resp.Data[i++])
                            {
                                num2 = (num2 * 10) + j;
                            }
                        }
                        else
                        {
                            for (byte j = resp.Data[i++]; j != 0x3a; j = resp.Data[i++])
                            {
                                num2 = (num2 * 10) + (j - 0x30);
                            }
                        }
                        Packet packetWithAttachment = null;
                        switch (text)
                        {
                            case PayloadTypes.Text:
                                packetWithAttachment = new Packet(Encoding.UTF8.GetString(resp.Data, i, num2));
                                break;

                            case PayloadTypes.Binary:
                                if (this.PacketWithAttachment != null)
                                {
                                    i++;
                                    num2--;
                                    byte[] destinationArray = new byte[num2];
                                    Array.Copy(resp.Data, i, destinationArray, 0, num2);
                                    this.PacketWithAttachment.AddAttachmentFromServer(destinationArray, true);
                                    if (this.PacketWithAttachment.HasAllAttachment)
                                    {
                                        packetWithAttachment = this.PacketWithAttachment;
                                        this.PacketWithAttachment = null;
                                    }
                                }
                                break;
                        }
                        if (packetWithAttachment != null)
                        {
                            try
                            {
                                this.OnPacket(packetWithAttachment);
                            }
                            catch (Exception exception)
                            {
                                HTTPManager.Logger.Exception("PollingTransport", "ParseResponse - OnPacket", exception);
                                ((IManager) this.Manager).EmitError(SocketIOErrors.Internal, exception.Message + " " + exception.StackTrace);
                            }
                        }
                    }
                }
            }
            catch (Exception exception2)
            {
                ((IManager) this.Manager).EmitError(SocketIOErrors.Internal, exception2.Message + " " + exception2.StackTrace);
                HTTPManager.Logger.Exception("PollingTransport", "ParseResponse", exception2);
            }
        }

        public void Poll()
        {
            if ((this.PollRequest == null) && (this.State != TransportStates.Paused))
            {
                ulong num2;
                object[] args = new object[6];
                args[0] = this.Manager.Uri.ToString();
                args[1] = 4;
                args[2] = this.Manager.Timestamp.ToString();
                SocketManager manager = this.Manager;
                manager.RequestCounter = (num2 = manager.RequestCounter) + ((ulong) 1L);
                args[3] = num2.ToString();
                args[4] = this.Manager.Handshake.Sid;
                args[5] = this.Manager.Options.QueryParamsOnlyForHandshake ? string.Empty : this.Manager.Options.BuildQueryParams();
                this.PollRequest = new HTTPRequest(new Uri(string.Format("{0}?EIO={1}&transport=polling&t={2}-{3}&sid={4}{5}", args)), HTTPMethods.Get, new OnRequestFinishedDelegate(this.OnPollRequestFinished));
                this.PollRequest.DisableCache = true;
                this.PollRequest.DisableRetry = true;
                this.PollRequest.Send();
            }
        }

        public void Send(Packet packet)
        {
            try
            {
                this.lonelyPacketList.Add(packet);
                this.Send(this.lonelyPacketList);
            }
            finally
            {
                this.lonelyPacketList.Clear();
            }
        }

        public void Send(List<Packet> packets)
        {
            if ((this.State == TransportStates.Opening) || (this.State == TransportStates.Open))
            {
                ulong num3;
                if (this.IsRequestInProgress)
                {
                    throw new Exception("Sending packets are still in progress!");
                }
                byte[] array = null;
                try
                {
                    array = packets[0].EncodeBinary();
                    for (int i = 1; i < packets.Count; i++)
                    {
                        byte[] sourceArray = packets[i].EncodeBinary();
                        Array.Resize<byte>(ref array, array.Length + sourceArray.Length);
                        Array.Copy(sourceArray, 0, array, array.Length - sourceArray.Length, sourceArray.Length);
                    }
                    packets.Clear();
                }
                catch (Exception exception)
                {
                    ((IManager) this.Manager).EmitError(SocketIOErrors.Internal, exception.Message + " " + exception.StackTrace);
                    return;
                }
                object[] args = new object[6];
                args[0] = this.Manager.Uri.ToString();
                args[1] = 4;
                args[2] = this.Manager.Timestamp.ToString();
                SocketManager manager = this.Manager;
                manager.RequestCounter = (num3 = manager.RequestCounter) + ((ulong) 1L);
                args[3] = num3.ToString();
                args[4] = this.Manager.Handshake.Sid;
                args[5] = this.Manager.Options.QueryParamsOnlyForHandshake ? string.Empty : this.Manager.Options.BuildQueryParams();
                this.LastRequest = new HTTPRequest(new Uri(string.Format("{0}?EIO={1}&transport=polling&t={2}-{3}&sid={4}{5}", args)), HTTPMethods.Post, new OnRequestFinishedDelegate(this.OnRequestFinished));
                this.LastRequest.DisableCache = true;
                this.LastRequest.SetHeader("Content-Type", "application/octet-stream");
                this.LastRequest.RawData = array;
                this.LastRequest.Send();
            }
        }

        public TransportTypes Type =>
            TransportTypes.Polling;

        public TransportStates State { get; private set; }

        public SocketManager Manager { get; private set; }

        public bool IsRequestInProgress =>
            (this.LastRequest != null);

        public bool IsPollingInProgress =>
            (this.PollRequest != null);

        private enum PayloadTypes : byte
        {
            Text = 0,
            Binary = 1
        }
    }
}

