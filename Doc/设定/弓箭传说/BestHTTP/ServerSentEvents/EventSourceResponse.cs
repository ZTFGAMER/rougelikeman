namespace BestHTTP.ServerSentEvents
{
    using BestHTTP;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;

    public sealed class EventSourceResponse : HTTPResponse, IProtocol
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsClosed>k__BackingField;
        public Action<EventSourceResponse, Message> OnMessage;
        public Action<EventSourceResponse> OnClosed;
        private object FrameLock;
        private byte[] LineBuffer;
        private int LineBufferPos;
        private Message CurrentMessage;
        private List<Message> CompletedMessages;

        public EventSourceResponse(HTTPRequest request, Stream stream, bool isStreamed, bool isFromCache) : base(request, stream, isStreamed, isFromCache)
        {
            this.FrameLock = new object();
            this.LineBuffer = new byte[0x400];
            this.CompletedMessages = new List<Message>();
            base.IsClosedManually = true;
        }

        void IProtocol.HandleEvents()
        {
            object frameLock = this.FrameLock;
            lock (frameLock)
            {
                if (this.CompletedMessages.Count > 0)
                {
                    if (this.OnMessage != null)
                    {
                        for (int i = 0; i < this.CompletedMessages.Count; i++)
                        {
                            try
                            {
                                this.OnMessage(this, this.CompletedMessages[i]);
                            }
                            catch (Exception exception)
                            {
                                HTTPManager.Logger.Exception("EventSourceMessage", "HandleEvents - OnMessage", exception);
                            }
                        }
                    }
                    this.CompletedMessages.Clear();
                }
            }
            if (this.IsClosed)
            {
                this.CompletedMessages.Clear();
                if (this.OnClosed != null)
                {
                    try
                    {
                        this.OnClosed(this);
                    }
                    catch (Exception exception2)
                    {
                        HTTPManager.Logger.Exception("EventSourceMessage", "HandleEvents - OnClosed", exception2);
                    }
                    finally
                    {
                        this.OnClosed = null;
                    }
                }
            }
        }

        public void FeedData(byte[] buffer, int count)
        {
            int num;
            if (count == -1)
            {
                count = buffer.Length;
            }
            if (count == 0)
            {
                return;
            }
            int sourceIndex = 0;
        Label_0015:
            num = -1;
            int num3 = 1;
            for (int i = sourceIndex; (i < count) && (num == -1); i++)
            {
                if (buffer[i] == 13)
                {
                    if (((i + 1) < count) && (buffer[i + 1] == 10))
                    {
                        num3 = 2;
                    }
                    num = i;
                }
                else if (buffer[i] == 10)
                {
                    num = i;
                }
            }
            int num5 = (num != -1) ? num : count;
            if (this.LineBuffer.Length < (this.LineBufferPos + (num5 - sourceIndex)))
            {
                Array.Resize<byte>(ref this.LineBuffer, this.LineBufferPos + (num5 - sourceIndex));
            }
            Array.Copy(buffer, sourceIndex, this.LineBuffer, this.LineBufferPos, num5 - sourceIndex);
            this.LineBufferPos += num5 - sourceIndex;
            if (num != -1)
            {
                this.ParseLine(this.LineBuffer, this.LineBufferPos);
                this.LineBufferPos = 0;
                sourceIndex = num + num3;
                if ((num != -1) && (sourceIndex < count))
                {
                    goto Label_0015;
                }
            }
        }

        private void ParseLine(byte[] buffer, int count)
        {
            if (count == 0)
            {
                if (this.CurrentMessage != null)
                {
                    object frameLock = this.FrameLock;
                    lock (frameLock)
                    {
                        this.CompletedMessages.Add(this.CurrentMessage);
                    }
                    this.CurrentMessage = null;
                }
            }
            else if (buffer[0] != 0x3a)
            {
                string str;
                string str2;
                int num = -1;
                for (int i = 0; (i < count) && (num == -1); i++)
                {
                    if (buffer[i] == 0x3a)
                    {
                        num = i;
                    }
                }
                if (num != -1)
                {
                    str = Encoding.UTF8.GetString(buffer, 0, num);
                    if (((num + 1) < count) && (buffer[num + 1] == 0x20))
                    {
                        num++;
                    }
                    num++;
                    if (num >= count)
                    {
                        return;
                    }
                    str2 = Encoding.UTF8.GetString(buffer, num, count - num);
                }
                else
                {
                    str = Encoding.UTF8.GetString(buffer, 0, count);
                    str2 = string.Empty;
                }
                if (this.CurrentMessage == null)
                {
                    this.CurrentMessage = new Message();
                }
                if (str != null)
                {
                    if (str == "id")
                    {
                        this.CurrentMessage.Id = str2;
                    }
                    else if (str == "event")
                    {
                        this.CurrentMessage.Event = str2;
                    }
                    else if (str == "data")
                    {
                        if (this.CurrentMessage.Data != null)
                        {
                            this.CurrentMessage.Data = this.CurrentMessage.Data + Environment.NewLine;
                        }
                        this.CurrentMessage.Data = this.CurrentMessage.Data + str2;
                    }
                    else if ((str == "retry") && int.TryParse(str2, out int num3))
                    {
                        this.CurrentMessage.Retry = TimeSpan.FromMilliseconds((double) num3);
                    }
                }
            }
        }

        private void ReadChunked(Stream stream)
        {
            int newSize = base.ReadChunkLength(stream);
            byte[] array = new byte[newSize];
            while (newSize != 0)
            {
                if (array.Length < newSize)
                {
                    Array.Resize<byte>(ref array, newSize);
                }
                int offset = 0;
                do
                {
                    int num3 = stream.Read(array, offset, newSize - offset);
                    if (num3 == 0)
                    {
                        throw new Exception("The remote server closed the connection unexpectedly!");
                    }
                    offset += num3;
                }
                while (offset < newSize);
                this.FeedData(array, offset);
                HTTPResponse.ReadTo(stream, 10);
                newSize = base.ReadChunkLength(stream);
            }
            base.ReadHeaders(stream);
        }

        private void ReadRaw(Stream stream, long contentLength)
        {
            int num;
            byte[] buffer = new byte[0x400];
            do
            {
                num = stream.Read(buffer, 0, buffer.Length);
                this.FeedData(buffer, num);
            }
            while (num > 0);
        }

        internal override bool Receive(int forceReadRawContentLength = -1, bool readPayloadData = true)
        {
            bool flag = base.Receive(forceReadRawContentLength, false);
            string firstHeaderValue = base.GetFirstHeaderValue("content-type");
            base.IsUpgraded = ((flag && (base.StatusCode == 200)) && !string.IsNullOrEmpty(firstHeaderValue)) && firstHeaderValue.ToLower().StartsWith("text/event-stream");
            if (!base.IsUpgraded)
            {
                base.ReadPayload(forceReadRawContentLength);
            }
            return flag;
        }

        private void ReceiveThreadFunc(object param)
        {
            try
            {
                if (base.HasHeaderWithValue("transfer-encoding", "chunked"))
                {
                    this.ReadChunked(base.Stream);
                }
                else
                {
                    this.ReadRaw(base.Stream, -1L);
                }
            }
            catch (ThreadAbortException)
            {
                base.baseRequest.State = HTTPRequestStates.Aborted;
            }
            catch (Exception exception)
            {
                if (HTTPUpdateDelegator.IsCreated)
                {
                    base.baseRequest.Exception = exception;
                    base.baseRequest.State = HTTPRequestStates.Error;
                }
                else
                {
                    base.baseRequest.State = HTTPRequestStates.Aborted;
                }
            }
            finally
            {
                this.IsClosed = true;
            }
        }

        internal void StartReceive()
        {
            if (base.IsUpgraded)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.ReceiveThreadFunc));
            }
        }

        public bool IsClosed { get; private set; }
    }
}

