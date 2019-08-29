namespace BestHTTP.WebSocket
{
    using BestHTTP;
    using BestHTTP.Decompression.Zlib;
    using BestHTTP.Extensions;
    using BestHTTP.WebSocket.Extensions;
    using BestHTTP.WebSocket.Frames;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;

    public sealed class WebSocket
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <StartPingThread>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <PingFrequency>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private HTTPRequest <InternalRequest>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IExtension[] <Extensions>k__BackingField;
        public OnWebSocketOpenDelegate OnOpen;
        public OnWebSocketMessageDelegate OnMessage;
        public OnWebSocketBinaryDelegate OnBinary;
        public OnWebSocketClosedDelegate OnClosed;
        public OnWebSocketErrorDelegate OnError;
        public OnWebSocketErrorDescriptionDelegate OnErrorDesc;
        public OnWebSocketIncompleteFrameDelegate OnIncompleteFrame;
        private bool requestSent;
        private WebSocketResponse webSocket;

        public WebSocket(Uri uri) : this(uri, string.Empty, string.Empty, Array.Empty<IExtension>())
        {
            this.Extensions = new IExtension[] { new PerMessageCompression(CompressionLevel.Default, false, false, 15, 15, 5) };
        }

        public WebSocket(Uri uri, string origin, string protocol, params IExtension[] extensions)
        {
            this.PingFrequency = 0x3e8;
            if (uri.Port == -1)
            {
                string[] textArray1 = new string[] { uri.Scheme, "://", uri.Host, ":", !uri.Scheme.Equals("wss", StringComparison.OrdinalIgnoreCase) ? "80" : "443", uri.GetRequestPathAndQueryURL() };
                uri = new Uri(string.Concat(textArray1));
            }
            this.InternalRequest = new HTTPRequest(uri, new OnRequestFinishedDelegate(this.OnInternalRequestCallback));
            this.InternalRequest.OnUpgraded = new OnRequestFinishedDelegate(this.OnInternalRequestUpgraded);
            if (uri.Port != 80)
            {
                this.InternalRequest.SetHeader("Host", uri.Host + ":" + uri.Port);
            }
            else
            {
                this.InternalRequest.SetHeader("Host", uri.Host);
            }
            this.InternalRequest.SetHeader("Upgrade", "websocket");
            this.InternalRequest.SetHeader("Connection", "keep-alive, Upgrade");
            object[] from = new object[] { this, this.InternalRequest, uri, new object() };
            this.InternalRequest.SetHeader("Sec-WebSocket-Key", this.GetSecKey(from));
            if (!string.IsNullOrEmpty(origin))
            {
                this.InternalRequest.SetHeader("Origin", origin);
            }
            this.InternalRequest.SetHeader("Sec-WebSocket-Version", "13");
            if (!string.IsNullOrEmpty(protocol))
            {
                this.InternalRequest.SetHeader("Sec-WebSocket-Protocol", protocol);
            }
            this.InternalRequest.SetHeader("Cache-Control", "no-cache");
            this.InternalRequest.SetHeader("Pragma", "no-cache");
            this.Extensions = extensions;
            this.InternalRequest.DisableCache = true;
            if (HTTPManager.Proxy != null)
            {
                this.InternalRequest.Proxy = new HTTPProxy(HTTPManager.Proxy.Address, HTTPManager.Proxy.Credentials, false, false, HTTPManager.Proxy.NonTransparentForHTTPS);
            }
        }

        public void Close()
        {
            if (this.IsOpen)
            {
                this.webSocket.Close();
                this.InternalRequest.OnProgress = null;
                this.InternalRequest.Callback = null;
                this.InternalRequest.Abort();
            }
        }

        public void Close(ushort code, string message)
        {
            if (this.IsOpen)
            {
                this.webSocket.Close(code, message);
            }
        }

        public static byte[] EncodeCloseData(ushort code, string message)
        {
            int byteCount = Encoding.UTF8.GetByteCount(message);
            using (MemoryStream stream = new MemoryStream(2 + byteCount))
            {
                byte[] bytes = BitConverter.GetBytes(code);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(bytes, 0, bytes.Length);
                }
                stream.Write(bytes, 0, bytes.Length);
                bytes = Encoding.UTF8.GetBytes(message);
                stream.Write(bytes, 0, bytes.Length);
                return stream.ToArray();
            }
        }

        private string GetSecKey(object[] from)
        {
            byte[] inArray = new byte[0x10];
            int num = 0;
            for (int i = 0; i < from.Length; i++)
            {
                byte[] bytes = BitConverter.GetBytes(from[i].GetHashCode());
                for (int j = 0; (j < bytes.Length) && (num < inArray.Length); j++)
                {
                    inArray[num++] = bytes[j];
                }
            }
            return Convert.ToBase64String(inArray);
        }

        private void OnInternalRequestCallback(HTTPRequest req, HTTPResponse resp)
        {
            string reason = string.Empty;
            switch (req.State)
            {
                case HTTPRequestStates.Finished:
                    if (!resp.IsSuccess && (resp.StatusCode != 0x65))
                    {
                        reason = $"Request Finished Successfully, but the server sent an error. Status Code: {resp.StatusCode}-{resp.Message} Message: {resp.DataAsText}";
                        break;
                    }
                    HTTPManager.Logger.Information("WebSocket", $"Request finished. Status Code: {resp.StatusCode.ToString()} Message: {resp.Message}");
                    return;

                case HTTPRequestStates.Error:
                    reason = "Request Finished with Error! " + ((req.Exception == null) ? string.Empty : ("Exception: " + req.Exception.Message + req.Exception.StackTrace));
                    break;

                case HTTPRequestStates.Aborted:
                    reason = "Request Aborted!";
                    break;

                case HTTPRequestStates.ConnectionTimedOut:
                    reason = "Connection Timed Out!";
                    break;

                case HTTPRequestStates.TimedOut:
                    reason = "Processing the request Timed Out!";
                    break;

                default:
                    return;
            }
            if (this.OnError != null)
            {
                this.OnError(this, req.Exception);
            }
            if (this.OnErrorDesc != null)
            {
                this.OnErrorDesc(this, reason);
            }
            if ((this.OnError == null) && (this.OnErrorDesc == null))
            {
            }
            if ((!req.IsKeepAlive && (resp != null)) && (resp is WebSocketResponse))
            {
                (resp as WebSocketResponse).CloseStream();
            }
        }

        private void OnInternalRequestUpgraded(HTTPRequest req, HTTPResponse resp)
        {
            this.webSocket = resp as WebSocketResponse;
            if (this.webSocket == null)
            {
                if (this.OnError != null)
                {
                    this.OnError(this, req.Exception);
                }
                if (this.OnErrorDesc != null)
                {
                    string reason = string.Empty;
                    if (req.Exception != null)
                    {
                        reason = req.Exception.Message + " " + req.Exception.StackTrace;
                    }
                    this.OnErrorDesc(this, reason);
                }
            }
            else
            {
                this.webSocket.WebSocket = this;
                if (this.Extensions != null)
                {
                    for (int i = 0; i < this.Extensions.Length; i++)
                    {
                        IExtension extension = this.Extensions[i];
                        try
                        {
                            if ((extension != null) && !extension.ParseNegotiation(this.webSocket))
                            {
                                this.Extensions[i] = null;
                            }
                        }
                        catch (Exception exception)
                        {
                            HTTPManager.Logger.Exception("WebSocket", "ParseNegotiation", exception);
                            this.Extensions[i] = null;
                        }
                    }
                }
                if (this.OnOpen != null)
                {
                    try
                    {
                        this.OnOpen(this);
                    }
                    catch (Exception exception2)
                    {
                        HTTPManager.Logger.Exception("WebSocket", "OnOpen", exception2);
                    }
                }
                this.webSocket.OnText = delegate (WebSocketResponse ws, string msg) {
                    if (this.OnMessage != null)
                    {
                        this.OnMessage(this, msg);
                    }
                };
                this.webSocket.OnBinary = delegate (WebSocketResponse ws, byte[] bin) {
                    if (this.OnBinary != null)
                    {
                        this.OnBinary(this, bin);
                    }
                };
                this.webSocket.OnClosed = delegate (WebSocketResponse ws, ushort code, string msg) {
                    if (this.OnClosed != null)
                    {
                        this.OnClosed(this, code, msg);
                    }
                };
                if (this.OnIncompleteFrame != null)
                {
                    this.webSocket.OnIncompleteFrame = delegate (WebSocketResponse ws, WebSocketFrameReader frame) {
                        if (this.OnIncompleteFrame != null)
                        {
                            this.OnIncompleteFrame(this, frame);
                        }
                    };
                }
                if (this.StartPingThread)
                {
                    this.webSocket.StartPinging(Math.Max(this.PingFrequency, 100));
                }
                this.webSocket.StartReceive();
            }
        }

        public void Open()
        {
            if (this.requestSent)
            {
                throw new InvalidOperationException("Open already called! You can't reuse this WebSocket instance!");
            }
            if (this.Extensions != null)
            {
                try
                {
                    for (int i = 0; i < this.Extensions.Length; i++)
                    {
                        IExtension extension = this.Extensions[i];
                        if (extension != null)
                        {
                            extension.AddNegotiation(this.InternalRequest);
                        }
                    }
                }
                catch (Exception exception)
                {
                    HTTPManager.Logger.Exception("WebSocket", "Open", exception);
                }
            }
            this.InternalRequest.Send();
            this.requestSent = true;
        }

        public void Send(WebSocketFrame frame)
        {
            if (this.IsOpen)
            {
                this.webSocket.Send(frame);
            }
        }

        public void Send(string message)
        {
            if (this.IsOpen)
            {
                this.webSocket.Send(message);
            }
        }

        public void Send(byte[] buffer)
        {
            if (this.IsOpen)
            {
                this.webSocket.Send(buffer);
            }
        }

        public void Send(byte[] buffer, ulong offset, ulong count)
        {
            if (this.IsOpen)
            {
                this.webSocket.Send(buffer, offset, count);
            }
        }

        public bool IsOpen =>
            ((this.webSocket != null) && !this.webSocket.IsClosed);

        public int BufferedAmount =>
            this.webSocket.BufferedAmount;

        public bool StartPingThread { get; set; }

        public int PingFrequency { get; set; }

        public HTTPRequest InternalRequest { get; private set; }

        public IExtension[] Extensions { get; private set; }
    }
}

