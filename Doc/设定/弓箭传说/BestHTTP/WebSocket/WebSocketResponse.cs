namespace BestHTTP.WebSocket
{
    using BestHTTP;
    using BestHTTP.Extensions;
    using BestHTTP.Logger;
    using BestHTTP.WebSocket.Frames;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;

    public sealed class WebSocketResponse : HTTPResponse, IHeartbeat, IProtocol
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private BestHTTP.WebSocket.WebSocket <WebSocket>k__BackingField;
        public Action<WebSocketResponse, string> OnText;
        public Action<WebSocketResponse, byte[]> OnBinary;
        public Action<WebSocketResponse, WebSocketFrameReader> OnIncompleteFrame;
        public Action<WebSocketResponse, ushort, string> OnClosed;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TimeSpan <PingFrequnecy>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ushort <MaxFragmentSize>k__BackingField;
        private int _bufferedAmount;
        private List<WebSocketFrameReader> IncompleteFrames;
        private List<WebSocketFrameReader> CompletedFrames;
        private WebSocketFrameReader CloseFrame;
        private object FrameLock;
        private object SendLock;
        private List<WebSocketFrame> unsentFrames;
        private AutoResetEvent newFrameSignal;
        private volatile bool sendThreadCreated;
        private volatile bool closeSent;
        private volatile bool closed;
        private DateTime lastPing;

        internal WebSocketResponse(HTTPRequest request, Stream stream, bool isStreamed, bool isFromCache) : base(request, stream, isStreamed, isFromCache)
        {
            this.IncompleteFrames = new List<WebSocketFrameReader>();
            this.CompletedFrames = new List<WebSocketFrameReader>();
            this.FrameLock = new object();
            this.SendLock = new object();
            this.unsentFrames = new List<WebSocketFrame>();
            this.newFrameSignal = new AutoResetEvent(false);
            this.lastPing = DateTime.MinValue;
            base.IsClosedManually = true;
            this.closed = false;
            this.MaxFragmentSize = 0x7fff;
        }

        void IHeartbeat.OnHeartbeatUpdate(TimeSpan dif)
        {
            if (this.lastPing == DateTime.MinValue)
            {
                this.lastPing = DateTime.UtcNow;
            }
            else if ((DateTime.UtcNow - this.lastPing) >= this.PingFrequnecy)
            {
                try
                {
                    this.Send(new WebSocketFrame(this.WebSocket, WebSocketFrameTypes.Ping, Encoding.UTF8.GetBytes(string.Empty)));
                }
                catch
                {
                    this.closed = true;
                    HTTPManager.Heartbeats.Unsubscribe(this);
                }
                this.lastPing = DateTime.UtcNow;
            }
        }

        void IProtocol.HandleEvents()
        {
            object frameLock = this.FrameLock;
            lock (frameLock)
            {
                for (int i = 0; i < this.CompletedFrames.Count; i++)
                {
                    WebSocketFrameReader reader = this.CompletedFrames[i];
                    try
                    {
                        switch (reader.Type)
                        {
                            case WebSocketFrameTypes.Continuation:
                                break;

                            case WebSocketFrameTypes.Text:
                                goto Label_0066;

                            case WebSocketFrameTypes.Binary:
                                goto Label_0098;

                            default:
                            {
                                continue;
                            }
                        }
                    Label_0049:
                        if (this.OnIncompleteFrame != null)
                        {
                            this.OnIncompleteFrame(this, reader);
                        }
                        continue;
                    Label_0066:
                        if (!reader.IsFinal)
                        {
                            goto Label_0049;
                        }
                        if (this.OnText != null)
                        {
                            this.OnText(this, reader.DataAsText);
                        }
                        continue;
                    Label_0098:
                        if (!reader.IsFinal)
                        {
                            goto Label_0049;
                        }
                        if (this.OnBinary != null)
                        {
                            this.OnBinary(this, reader.Data);
                        }
                    }
                    catch (Exception exception)
                    {
                        HTTPManager.Logger.Exception("WebSocketResponse", "HandleEvents", exception);
                    }
                }
                this.CompletedFrames.Clear();
            }
            if ((this.IsClosed && (this.OnClosed != null)) && (base.baseRequest.State == HTTPRequestStates.Processing))
            {
                try
                {
                    ushort num2 = 0;
                    string str = string.Empty;
                    if (((this.CloseFrame != null) && (this.CloseFrame.Data != null)) && (this.CloseFrame.Data.Length >= 2))
                    {
                        if (BitConverter.IsLittleEndian)
                        {
                            Array.Reverse(this.CloseFrame.Data, 0, 2);
                        }
                        num2 = BitConverter.ToUInt16(this.CloseFrame.Data, 0);
                        if (this.CloseFrame.Data.Length > 2)
                        {
                            str = Encoding.UTF8.GetString(this.CloseFrame.Data, 2, this.CloseFrame.Data.Length - 2);
                        }
                    }
                    this.OnClosed(this, num2, str);
                }
                catch (Exception exception2)
                {
                    HTTPManager.Logger.Exception("WebSocketResponse", "HandleEvents - OnClosed", exception2);
                }
            }
        }

        public void Close()
        {
            this.Close(0x3e8, "Bye!");
        }

        public void Close(ushort code, string msg)
        {
            if (!this.closed)
            {
                this.Send(new WebSocketFrame(this.WebSocket, WebSocketFrameTypes.ConnectionClose, BestHTTP.WebSocket.WebSocket.EncodeCloseData(code, msg)));
            }
        }

        internal void CloseStream()
        {
            ConnectionBase connectionWith = HTTPManager.GetConnectionWith(base.baseRequest);
            if (connectionWith != null)
            {
                connectionWith.Abort(HTTPConnectionStates.Closed);
            }
        }

        private void ReceiveThreadFunc(object param)
        {
            try
            {
                while (!this.closed)
                {
                    try
                    {
                        object obj3;
                        WebSocketFrameReader item = new WebSocketFrameReader();
                        item.Read(base.Stream);
                        if (item.HasMask)
                        {
                            this.Close(0x3ea, "Protocol Error: masked frame received from server!");
                        }
                        else if (!item.IsFinal)
                        {
                            if (this.OnIncompleteFrame == null)
                            {
                                this.IncompleteFrames.Add(item);
                            }
                            else
                            {
                                object obj2 = this.FrameLock;
                                lock (obj2)
                                {
                                    this.CompletedFrames.Add(item);
                                }
                            }
                        }
                        else
                        {
                            switch (item.Type)
                            {
                                case WebSocketFrameTypes.Continuation:
                                    if (this.OnIncompleteFrame != null)
                                    {
                                        goto Label_00E3;
                                    }
                                    item.Assemble(this.IncompleteFrames);
                                    this.IncompleteFrames.Clear();
                                    goto Label_0119;

                                case WebSocketFrameTypes.Text:
                                case WebSocketFrameTypes.Binary:
                                    goto Label_0119;

                                case WebSocketFrameTypes.ConnectionClose:
                                    this.CloseFrame = item;
                                    if (!this.closeSent)
                                    {
                                        this.Send(new WebSocketFrame(this.WebSocket, WebSocketFrameTypes.ConnectionClose, null));
                                    }
                                    this.closed = true;
                                    break;

                                case WebSocketFrameTypes.Ping:
                                    goto Label_015B;
                            }
                        }
                        continue;
                    Label_00E3:
                        obj3 = this.FrameLock;
                        lock (obj3)
                        {
                            this.CompletedFrames.Add(item);
                        }
                        continue;
                    Label_0119:
                        item.DecodeWithExtensions(this.WebSocket);
                        object frameLock = this.FrameLock;
                        lock (frameLock)
                        {
                            this.CompletedFrames.Add(item);
                        }
                        continue;
                    Label_015B:
                        if (!this.closeSent && !this.closed)
                        {
                            this.Send(new WebSocketFrame(this.WebSocket, WebSocketFrameTypes.Pong, item.Data));
                        }
                        continue;
                    }
                    catch (ThreadAbortException)
                    {
                        this.IncompleteFrames.Clear();
                        base.baseRequest.State = HTTPRequestStates.Aborted;
                        this.closed = true;
                        this.newFrameSignal.Set();
                        continue;
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
                        this.closed = true;
                        this.newFrameSignal.Set();
                        continue;
                    }
                }
            }
            finally
            {
                HTTPManager.Heartbeats.Unsubscribe(this);
            }
        }

        public void Send(string message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message must not be null!");
            }
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            this.Send(new WebSocketFrame(this.WebSocket, WebSocketFrameTypes.Text, bytes));
        }

        public void Send(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data must not be null!");
            }
            WebSocketFrame frame = new WebSocketFrame(this.WebSocket, WebSocketFrameTypes.Binary, data);
            if ((frame.Data != null) && (frame.Data.Length > this.MaxFragmentSize))
            {
                WebSocketFrame[] frameArray = frame.Fragment(this.MaxFragmentSize);
                object sendLock = this.SendLock;
                lock (sendLock)
                {
                    this.Send(frame);
                    if (frameArray != null)
                    {
                        for (int i = 0; i < frameArray.Length; i++)
                        {
                            this.Send(frameArray[i]);
                        }
                    }
                }
            }
            else
            {
                this.Send(frame);
            }
        }

        public void Send(WebSocketFrame frame)
        {
            if (frame == null)
            {
                throw new ArgumentNullException("frame is null!");
            }
            if (!this.closed)
            {
                object sendLock = this.SendLock;
                lock (sendLock)
                {
                    this.unsentFrames.Add(frame);
                    Interlocked.Add(ref this._bufferedAmount, (frame.Data == null) ? 0 : frame.Data.Length);
                    if (!this.sendThreadCreated)
                    {
                        ThreadPool.QueueUserWorkItem(new WaitCallback(this.SendThreadFunc));
                        this.sendThreadCreated = true;
                    }
                }
                HTTPManager.Logger.Information("WebSocketResponse", "Signaling SendThread!");
                this.newFrameSignal.Set();
            }
        }

        public void Send(byte[] data, ulong offset, ulong count)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data must not be null!");
            }
            if ((offset + count) > data.Length)
            {
                throw new ArgumentOutOfRangeException("offset + count >= data.Length");
            }
            WebSocketFrame frame = new WebSocketFrame(this.WebSocket, WebSocketFrameTypes.Binary, data, offset, count, true, true);
            if ((frame.Data != null) && (frame.Data.Length > this.MaxFragmentSize))
            {
                WebSocketFrame[] frameArray = frame.Fragment(this.MaxFragmentSize);
                object sendLock = this.SendLock;
                lock (sendLock)
                {
                    this.Send(frame);
                    if (frameArray != null)
                    {
                        for (int i = 0; i < frameArray.Length; i++)
                        {
                            this.Send(frameArray[i]);
                        }
                    }
                }
            }
            else
            {
                this.Send(frame);
            }
        }

        private void SendThreadFunc(object param)
        {
            List<WebSocketFrame> list = new List<WebSocketFrame>();
            try
            {
                while (!this.closed && !this.closeSent)
                {
                    if (HTTPManager.Logger.Level <= Loglevels.Information)
                    {
                        HTTPManager.Logger.Information("WebSocketResponse", "SendThread - Waiting...");
                    }
                    this.newFrameSignal.WaitOne();
                    try
                    {
                        object sendLock = this.SendLock;
                        lock (sendLock)
                        {
                            for (int i = this.unsentFrames.Count - 1; i >= 0; i--)
                            {
                                list.Add(this.unsentFrames[i]);
                            }
                            this.unsentFrames.Clear();
                        }
                        if (HTTPManager.Logger.Level <= Loglevels.Information)
                        {
                            HTTPManager.Logger.Information("WebSocketResponse", "SendThread - Wait is over, " + list.Count.ToString() + " new frames!");
                        }
                        while (list.Count > 0)
                        {
                            WebSocketFrame frame = list[list.Count - 1];
                            list.RemoveAt(list.Count - 1);
                            if (!this.closeSent)
                            {
                                byte[] buffer = frame.Get();
                                base.Stream.Write(buffer, 0, buffer.Length);
                                base.Stream.Flush();
                                if (frame.Type == WebSocketFrameTypes.ConnectionClose)
                                {
                                    this.closeSent = true;
                                }
                            }
                            Interlocked.Add(ref this._bufferedAmount, -frame.Data.Length);
                        }
                        continue;
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
                        this.closed = true;
                        continue;
                    }
                }
            }
            finally
            {
                this.sendThreadCreated = false;
                HTTPManager.Logger.Information("WebSocketResponse", "SendThread - Closed!");
            }
        }

        public void StartPinging(int frequency)
        {
            if (frequency < 100)
            {
                throw new ArgumentException("frequency must be at least 100 milliseconds!");
            }
            this.PingFrequnecy = TimeSpan.FromMilliseconds((double) frequency);
            HTTPManager.Heartbeats.Subscribe(this);
        }

        internal void StartReceive()
        {
            if (base.IsUpgraded)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.ReceiveThreadFunc));
            }
        }

        public BestHTTP.WebSocket.WebSocket WebSocket { get; internal set; }

        public bool IsClosed =>
            this.closed;

        public TimeSpan PingFrequnecy { get; private set; }

        public ushort MaxFragmentSize { get; private set; }

        public int BufferedAmount =>
            this._bufferedAmount;
    }
}

