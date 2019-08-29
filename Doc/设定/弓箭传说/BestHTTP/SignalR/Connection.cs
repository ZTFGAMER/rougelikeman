namespace BestHTTP.SignalR
{
    using BestHTTP;
    using BestHTTP.Extensions;
    using BestHTTP.JSON;
    using BestHTTP.Logger;
    using BestHTTP.SignalR.Authentication;
    using BestHTTP.SignalR.Hubs;
    using BestHTTP.SignalR.JsonEncoders;
    using BestHTTP.SignalR.Messages;
    using BestHTTP.SignalR.Transports;
    using PlatformSupport.Collections.ObjectModel;
    using PlatformSupport.Collections.Specialized;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;

    public sealed class Connection : IHeartbeat, IConnection
    {
        public static IJsonEncoder DefaultEncoder = new DefaultJsonEncoder();
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private System.Uri <Uri>k__BackingField;
        private ConnectionStates _state;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private NegotiationData <NegotiationResult>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Hub[] <Hubs>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TransportBase <Transport>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ProtocolVersions <Protocol>k__BackingField;
        private ObservableDictionary<string, string> additionalQueryParams;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <QueryParamsOnlyForHandshake>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IJsonEncoder <JsonEncoder>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IAuthenticationProvider <AuthenticationProvider>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TimeSpan <PingInterval>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TimeSpan <ReconnectDelay>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private OnPrepareRequestDelegate <RequestPreparator>k__BackingField;
        internal object SyncRoot;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ulong <ClientMessageCounter>k__BackingField;
        private readonly string[] ClientProtocols;
        private ulong RequestCounter;
        private MultiMessage LastReceivedMessage;
        private string GroupsToken;
        private List<IServerMessage> BufferedMessages;
        private DateTime LastMessageReceivedAt;
        private DateTime ReconnectStartedAt;
        private DateTime ReconnectDelayStartedAt;
        private bool ReconnectStarted;
        private DateTime LastPingSentAt;
        private HTTPRequest PingRequest;
        private DateTime? TransportConnectionStartedAt;
        private StringBuilder queryBuilder;
        private string BuiltConnectionData;
        private string BuiltQueryParams;
        private SupportedProtocols NextProtocolToTry;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event OnClosedDelegate OnClosed;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event OnConnectedDelegate OnConnected;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event OnErrorDelegate OnError;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event OnNonHubMessageDelegate OnNonHubMessage;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event OnConnectedDelegate OnReconnected;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event OnConnectedDelegate OnReconnecting;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event BestHTTP.SignalR.OnStateChanged OnStateChanged;

        public Connection(System.Uri uri)
        {
            this.SyncRoot = new object();
            this.ClientProtocols = new string[] { "1.3", "1.4", "1.5" };
            this.queryBuilder = new StringBuilder();
            this.State = ConnectionStates.Initial;
            this.Uri = uri;
            this.JsonEncoder = DefaultEncoder;
            this.PingInterval = TimeSpan.FromMinutes(5.0);
            this.Protocol = ProtocolVersions.Protocol_2_2;
            this.ReconnectDelay = TimeSpan.FromSeconds(5.0);
        }

        public Connection(System.Uri uri, params Hub[] hubs) : this(uri)
        {
            this.Hubs = hubs;
            if (hubs != null)
            {
                for (int i = 0; i < hubs.Length; i++)
                {
                    ((IHub) hubs[i]).Connection = this;
                }
            }
        }

        public Connection(System.Uri uri, params string[] hubNames) : this(uri)
        {
            if ((hubNames != null) && (hubNames.Length > 0))
            {
                this.Hubs = new Hub[hubNames.Length];
                for (int i = 0; i < hubNames.Length; i++)
                {
                    this.Hubs[i] = new Hub(hubNames[i], this);
                }
            }
        }

        private void AdditionalQueryParams_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.BuiltQueryParams = null;
        }

        void IHeartbeat.OnHeartbeatUpdate(TimeSpan dif)
        {
            ConnectionStates state = this.State;
            if (state == ConnectionStates.Connected)
            {
                if ((this.Transport.SupportsKeepAlive && this.NegotiationResult.KeepAliveTimeout.HasValue) && ((DateTime.UtcNow - this.LastMessageReceivedAt) >= this.NegotiationResult.KeepAliveTimeout))
                {
                    this.Reconnect();
                }
                if ((this.PingRequest == null) && ((DateTime.UtcNow - this.LastPingSentAt) >= this.PingInterval))
                {
                    this.Ping();
                }
            }
            else if (state == ConnectionStates.Reconnecting)
            {
                if ((DateTime.UtcNow - this.ReconnectStartedAt) >= this.NegotiationResult.DisconnectTimeout)
                {
                    HTTPManager.Logger.Warning("SignalR Connection", "OnHeartbeatUpdate - Failed to reconnect in the given time!");
                    this.Close();
                }
                else if ((DateTime.UtcNow - this.ReconnectDelayStartedAt) >= this.ReconnectDelay)
                {
                    if (HTTPManager.Logger.Level <= Loglevels.Warning)
                    {
                        string[] textArray1 = new string[] { this.ReconnectStarted.ToString(), " ", this.ReconnectStartedAt.ToString(), " ", this.NegotiationResult.DisconnectTimeout.ToString() };
                        HTTPManager.Logger.Warning("SignalR Connection", string.Concat(textArray1));
                    }
                    this.Reconnect();
                }
            }
            else if (this.TransportConnectionStartedAt.HasValue)
            {
                DateTime? transportConnectionStartedAt = this.TransportConnectionStartedAt;
                if ((!transportConnectionStartedAt.HasValue ? ((TimeSpan?) null) : new TimeSpan?(((TimeSpan) DateTime.UtcNow) - transportConnectionStartedAt.GetValueOrDefault())) >= this.NegotiationResult.TransportConnectTimeout)
                {
                    HTTPManager.Logger.Warning("SignalR Connection", "OnHeartbeatUpdate - Transport failed to connect in the given time!");
                    ((IConnection) this).Error("Transport failed to connect in the given time!");
                }
            }
        }

        System.Uri IConnection.BuildUri(RequestTypes type) => 
            ((IConnection) this).BuildUri(type, null);

        System.Uri IConnection.BuildUri(RequestTypes type, TransportBase transport)
        {
            object syncRoot = this.SyncRoot;
            lock (syncRoot)
            {
                ulong num3;
                this.queryBuilder.Length = 0;
                UriBuilder builder = new UriBuilder(this.Uri);
                if (!builder.Path.EndsWith("/"))
                {
                    builder.Path = builder.Path + "/";
                }
                this.RequestCounter = this.RequestCounter % ulong.MaxValue;
                switch (type)
                {
                    case RequestTypes.Negotiate:
                        builder.Path = builder.Path + "negotiate";
                        break;

                    case RequestTypes.Connect:
                        if ((transport != null) && (transport.Type == TransportTypes.WebSocket))
                        {
                            builder.Scheme = !HTTPProtocolFactory.IsSecureProtocol(this.Uri) ? "ws" : "wss";
                        }
                        builder.Path = builder.Path + "connect";
                        break;

                    case RequestTypes.Start:
                        builder.Path = builder.Path + "start";
                        break;

                    case RequestTypes.Poll:
                        builder.Path = builder.Path + "poll";
                        if (this.LastReceivedMessage != null)
                        {
                            this.queryBuilder.Append("messageId=");
                            this.queryBuilder.Append(this.LastReceivedMessage.MessageId);
                        }
                        break;

                    case RequestTypes.Send:
                        builder.Path = builder.Path + "send";
                        break;

                    case RequestTypes.Reconnect:
                        if ((transport != null) && (transport.Type == TransportTypes.WebSocket))
                        {
                            builder.Scheme = !HTTPProtocolFactory.IsSecureProtocol(this.Uri) ? "ws" : "wss";
                        }
                        builder.Path = builder.Path + "reconnect";
                        if (this.LastReceivedMessage != null)
                        {
                            this.queryBuilder.Append("messageId=");
                            this.queryBuilder.Append(this.LastReceivedMessage.MessageId);
                        }
                        if (!string.IsNullOrEmpty(this.GroupsToken))
                        {
                            if (this.queryBuilder.Length > 0)
                            {
                                this.queryBuilder.Append("&");
                            }
                            this.queryBuilder.Append("groupsToken=");
                            this.queryBuilder.Append(this.GroupsToken);
                        }
                        break;

                    case RequestTypes.Abort:
                        builder.Path = builder.Path + "abort";
                        break;

                    case RequestTypes.Ping:
                        ulong num;
                        builder.Path = builder.Path + "ping";
                        this.queryBuilder.Append("&tid=");
                        this.RequestCounter = (num = this.RequestCounter) + ((ulong) 1L);
                        this.queryBuilder.Append(num.ToString());
                        this.queryBuilder.Append("&_=");
                        this.queryBuilder.Append(this.Timestamp.ToString());
                        goto Label_046C;
                }
                if (this.queryBuilder.Length > 0)
                {
                    this.queryBuilder.Append("&");
                }
                this.queryBuilder.Append("tid=");
                this.RequestCounter = (num3 = this.RequestCounter) + ((ulong) 1L);
                this.queryBuilder.Append(num3.ToString());
                this.queryBuilder.Append("&_=");
                this.queryBuilder.Append(this.Timestamp.ToString());
                if (transport != null)
                {
                    this.queryBuilder.Append("&transport=");
                    this.queryBuilder.Append(transport.Name);
                }
                this.queryBuilder.Append("&clientProtocol=");
                this.queryBuilder.Append(this.ClientProtocols[(int) this.Protocol]);
                if ((this.NegotiationResult != null) && !string.IsNullOrEmpty(this.NegotiationResult.ConnectionToken))
                {
                    this.queryBuilder.Append("&connectionToken=");
                    this.queryBuilder.Append(this.NegotiationResult.ConnectionToken);
                }
                if ((this.Hubs != null) && (this.Hubs.Length > 0))
                {
                    this.queryBuilder.Append("&connectionData=");
                    this.queryBuilder.Append(this.ConnectionData);
                }
            Label_046C:
                if ((this.AdditionalQueryParams != null) && (this.AdditionalQueryParams.Count > 0))
                {
                    this.queryBuilder.Append(this.QueryParams);
                }
                builder.Query = this.queryBuilder.ToString();
                this.queryBuilder.Length = 0;
                return builder.Uri;
            }
        }

        void IConnection.Error(string reason)
        {
            if (this.State != ConnectionStates.Closed)
            {
                HTTPManager.Logger.Error("SignalR Connection", reason);
                this.ReconnectStarted = false;
                if (this.OnError != null)
                {
                    this.OnError(this, reason);
                }
                if ((this.State == ConnectionStates.Connected) || (this.State == ConnectionStates.Reconnecting))
                {
                    this.ReconnectDelayStartedAt = DateTime.UtcNow;
                    if (this.State != ConnectionStates.Reconnecting)
                    {
                        this.ReconnectStartedAt = DateTime.UtcNow;
                    }
                }
                else if ((this.State != ConnectionStates.Connecting) || !this.TryFallbackTransport())
                {
                    this.Close();
                }
            }
        }

        void IConnection.OnMessage(IServerMessage msg)
        {
            if (this.State != ConnectionStates.Closed)
            {
                if (this.State == ConnectionStates.Connecting)
                {
                    if (this.BufferedMessages == null)
                    {
                        this.BufferedMessages = new List<IServerMessage>();
                    }
                    this.BufferedMessages.Add(msg);
                }
                else
                {
                    Hub hub;
                    this.LastMessageReceivedAt = DateTime.UtcNow;
                    switch (msg.Type)
                    {
                        case MessageTypes.KeepAlive:
                            break;

                        case MessageTypes.Data:
                            if (this.OnNonHubMessage != null)
                            {
                                this.OnNonHubMessage(this, (msg as DataMessage).Data);
                            }
                            break;

                        case MessageTypes.Multiple:
                            this.LastReceivedMessage = msg as MultiMessage;
                            if (this.LastReceivedMessage.IsInitialization)
                            {
                                HTTPManager.Logger.Information("SignalR Connection", "OnMessage - Init");
                            }
                            if (this.LastReceivedMessage.GroupsToken != null)
                            {
                                this.GroupsToken = this.LastReceivedMessage.GroupsToken;
                            }
                            if (this.LastReceivedMessage.ShouldReconnect)
                            {
                                HTTPManager.Logger.Information("SignalR Connection", "OnMessage - Should Reconnect");
                                this.Reconnect();
                            }
                            if (this.LastReceivedMessage.Data != null)
                            {
                                for (int i = 0; i < this.LastReceivedMessage.Data.Count; i++)
                                {
                                    ((IConnection) this).OnMessage(this.LastReceivedMessage.Data[i]);
                                }
                            }
                            break;

                        case MessageTypes.Result:
                        case MessageTypes.Failure:
                        case MessageTypes.Progress:
                        {
                            ulong invocationId = (msg as IHubMessage).InvocationId;
                            hub = this.FindHub(invocationId);
                            if (hub == null)
                            {
                                HTTPManager.Logger.Warning("SignalR Connection", $"No Hub found for Progress message! Id: {invocationId.ToString()}");
                                break;
                            }
                            ((IHub) hub).OnMessage(msg);
                            break;
                        }
                        case MessageTypes.MethodCall:
                        {
                            MethodCallMessage message = msg as MethodCallMessage;
                            hub = this[message.Hub];
                            if (hub == null)
                            {
                                HTTPManager.Logger.Warning("SignalR Connection", $"Hub "{message.Hub}" not found!");
                                break;
                            }
                            ((IHub) hub).OnMethod(message);
                            break;
                        }
                        default:
                            HTTPManager.Logger.Warning("SignalR Connection", "Unknown message type received: " + msg.Type.ToString());
                            break;
                    }
                }
            }
        }

        string IConnection.ParseResponse(string responseStr)
        {
            Dictionary<string, object> dictionary = Json.Decode(responseStr) as Dictionary<string, object>;
            if (dictionary == null)
            {
                ((IConnection) this).Error("Failed to parse Start response: " + responseStr);
                return string.Empty;
            }
            if (dictionary.TryGetValue("Response", out object obj2) && (obj2 != null))
            {
                return obj2.ToString();
            }
            ((IConnection) this).Error("No 'Response' key found in response: " + responseStr);
            return string.Empty;
        }

        HTTPRequest IConnection.PrepareRequest(HTTPRequest req, RequestTypes type)
        {
            if ((req != null) && (this.AuthenticationProvider != null))
            {
                this.AuthenticationProvider.PrepareRequest(req, type);
            }
            if (this.RequestPreparator != null)
            {
                this.RequestPreparator(this, req, type);
            }
            return req;
        }

        void IConnection.TransportAborted()
        {
            this.Close();
        }

        void IConnection.TransportReconnected()
        {
            if (this.State == ConnectionStates.Reconnecting)
            {
                HTTPManager.Logger.Information("SignalR Connection", "Transport Reconnected");
                this.InitOnStart();
                if (this.OnReconnected != null)
                {
                    try
                    {
                        this.OnReconnected(this);
                    }
                    catch (Exception exception)
                    {
                        HTTPManager.Logger.Exception("SignalR Connection", "OnReconnected", exception);
                    }
                }
            }
        }

        void IConnection.TransportStarted()
        {
            if (this.State == ConnectionStates.Connecting)
            {
                this.InitOnStart();
                if (this.OnConnected != null)
                {
                    try
                    {
                        this.OnConnected(this);
                    }
                    catch (Exception exception)
                    {
                        HTTPManager.Logger.Exception("SignalR Connection", "OnOpened", exception);
                    }
                }
                if (this.BufferedMessages != null)
                {
                    for (int i = 0; i < this.BufferedMessages.Count; i++)
                    {
                        ((IConnection) this).OnMessage(this.BufferedMessages[i]);
                    }
                    this.BufferedMessages.Clear();
                    this.BufferedMessages = null;
                }
            }
        }

        public void Close()
        {
            if (this.State != ConnectionStates.Closed)
            {
                this.State = ConnectionStates.Closed;
                this.ReconnectStarted = false;
                this.TransportConnectionStartedAt = null;
                if (this.Transport != null)
                {
                    this.Transport.Abort();
                    this.Transport = null;
                }
                this.NegotiationResult = null;
                HTTPManager.Heartbeats.Unsubscribe(this);
                this.LastReceivedMessage = null;
                if (this.Hubs != null)
                {
                    for (int i = 0; i < this.Hubs.Length; i++)
                    {
                        ((IHub) this.Hubs[i]).Close();
                    }
                }
                if (this.BufferedMessages != null)
                {
                    this.BufferedMessages.Clear();
                    this.BufferedMessages = null;
                }
                if (this.OnClosed != null)
                {
                    try
                    {
                        this.OnClosed(this);
                    }
                    catch (Exception exception)
                    {
                        HTTPManager.Logger.Exception("SignalR Connection", "OnClosed", exception);
                    }
                }
            }
        }

        private Hub FindHub(ulong msgId)
        {
            if (this.Hubs != null)
            {
                for (int i = 0; i < this.Hubs.Length; i++)
                {
                    if (((IHub) this.Hubs[i]).HasSentMessageId(msgId))
                    {
                        return this.Hubs[i];
                    }
                }
            }
            return null;
        }

        private void InitOnStart()
        {
            this.State = ConnectionStates.Connected;
            this.ReconnectStarted = false;
            this.TransportConnectionStartedAt = null;
            this.LastPingSentAt = DateTime.UtcNow;
            this.LastMessageReceivedAt = DateTime.UtcNow;
            HTTPManager.Heartbeats.Subscribe(this);
        }

        private void OnAuthenticationFailed(IAuthenticationProvider provider, string reason)
        {
            provider.OnAuthenticationFailed -= new OnAuthenticationFailedDelegate(this.OnAuthenticationFailed);
            ((IConnection) this).Error(reason);
        }

        private void OnAuthenticationSucceded(IAuthenticationProvider provider)
        {
            provider.OnAuthenticationSucceded -= new OnAuthenticationSuccededDelegate(this.OnAuthenticationSucceded);
            this.StartImpl();
        }

        private void OnNegotiationDataReceived(NegotiationData data)
        {
            int num = -1;
            for (int i = 0; (i < this.ClientProtocols.Length) && (num == -1); i++)
            {
                if (data.ProtocolVersion == this.ClientProtocols[i])
                {
                    num = i;
                }
            }
            if (num == -1)
            {
                num = 2;
                HTTPManager.Logger.Warning("SignalR Connection", "Unknown protocol version: " + data.ProtocolVersion);
            }
            this.Protocol = (ProtocolVersions) ((byte) num);
            if (data.TryWebSockets)
            {
                this.Transport = new WebSocketTransport(this);
                this.NextProtocolToTry = SupportedProtocols.ServerSentEvents;
            }
            else
            {
                this.Transport = new ServerSentEventsTransport(this);
                this.NextProtocolToTry = SupportedProtocols.HTTP;
            }
            this.State = ConnectionStates.Connecting;
            this.TransportConnectionStartedAt = new DateTime?(DateTime.UtcNow);
            this.Transport.Connect();
        }

        private void OnNegotiationError(NegotiationData data, string error)
        {
            ((IConnection) this).Error(error);
        }

        private void OnPingRequestFinished(HTTPRequest req, HTTPResponse resp)
        {
            this.PingRequest = null;
            string str = string.Empty;
            switch (req.State)
            {
                case HTTPRequestStates.Finished:
                {
                    if (!resp.IsSuccess)
                    {
                        str = $"Ping - Request Finished Successfully, but the server sent an error. Status Code: {resp.StatusCode}-{resp.Message} Message: {resp.DataAsText}";
                        break;
                    }
                    string str2 = ((IConnection) this).ParseResponse(resp.DataAsText);
                    if (str2 == "pong")
                    {
                        HTTPManager.Logger.Information("SignalR Connection", "Pong received.");
                        break;
                    }
                    str = "Wrong answer for ping request: " + str2;
                    break;
                }
                case HTTPRequestStates.Error:
                    str = "Ping - Request Finished with Error! " + ((req.Exception == null) ? "No Exception" : (req.Exception.Message + "\n" + req.Exception.StackTrace));
                    break;

                case HTTPRequestStates.ConnectionTimedOut:
                    str = "Ping - Connection Timed Out!";
                    break;

                case HTTPRequestStates.TimedOut:
                    str = "Ping - Processing the request Timed Out!";
                    break;
            }
            if (!string.IsNullOrEmpty(str))
            {
                ((IConnection) this).Error(str);
            }
        }

        public void Open()
        {
            if ((this.State == ConnectionStates.Initial) || (this.State == ConnectionStates.Closed))
            {
                if ((this.AuthenticationProvider != null) && this.AuthenticationProvider.IsPreAuthRequired)
                {
                    this.State = ConnectionStates.Authenticating;
                    this.AuthenticationProvider.OnAuthenticationSucceded += new OnAuthenticationSuccededDelegate(this.OnAuthenticationSucceded);
                    this.AuthenticationProvider.OnAuthenticationFailed += new OnAuthenticationFailedDelegate(this.OnAuthenticationFailed);
                    this.AuthenticationProvider.StartAuthentication();
                }
                else
                {
                    this.StartImpl();
                }
            }
        }

        private void Ping()
        {
            HTTPManager.Logger.Information("SignalR Connection", "Sending Ping request.");
            this.PingRequest = new HTTPRequest(((IConnection) this).BuildUri(RequestTypes.Ping), new OnRequestFinishedDelegate(this.OnPingRequestFinished));
            this.PingRequest.ConnectTimeout = this.PingInterval;
            ((IConnection) this).PrepareRequest(this.PingRequest, RequestTypes.Ping);
            this.PingRequest.Send();
            this.LastPingSentAt = DateTime.UtcNow;
        }

        public void Reconnect()
        {
            if (!this.ReconnectStarted)
            {
                this.ReconnectStarted = true;
                if (this.State != ConnectionStates.Reconnecting)
                {
                    this.ReconnectStartedAt = DateTime.UtcNow;
                }
                this.State = ConnectionStates.Reconnecting;
                HTTPManager.Logger.Warning("SignalR Connection", "Reconnecting");
                this.Transport.Reconnect();
                if (this.PingRequest != null)
                {
                    this.PingRequest.Abort();
                }
                if (this.OnReconnecting != null)
                {
                    try
                    {
                        this.OnReconnecting(this);
                    }
                    catch (Exception exception)
                    {
                        HTTPManager.Logger.Exception("SignalR Connection", "OnReconnecting", exception);
                    }
                }
            }
        }

        public bool Send(object arg)
        {
            if (arg == null)
            {
                throw new ArgumentNullException("arg");
            }
            object syncRoot = this.SyncRoot;
            lock (syncRoot)
            {
                if (this.State != ConnectionStates.Connected)
                {
                    return false;
                }
                string str = this.JsonEncoder.Encode(arg);
                if (string.IsNullOrEmpty(str))
                {
                    HTTPManager.Logger.Error("SignalR Connection", "Failed to JSon encode the given argument. Please try to use an advanced JSon encoder(check the documentation how you can do it).");
                }
                else
                {
                    this.Transport.Send(str);
                }
            }
            return true;
        }

        public bool SendJson(string json)
        {
            if (json == null)
            {
                throw new ArgumentNullException("json");
            }
            object syncRoot = this.SyncRoot;
            lock (syncRoot)
            {
                if (this.State != ConnectionStates.Connected)
                {
                    return false;
                }
                this.Transport.Send(json);
            }
            return true;
        }

        private void StartImpl()
        {
            this.State = ConnectionStates.Negotiating;
            this.NegotiationResult = new NegotiationData(this);
            this.NegotiationResult.OnReceived = new Action<NegotiationData>(this.OnNegotiationDataReceived);
            this.NegotiationResult.OnError = new Action<NegotiationData, string>(this.OnNegotiationError);
            this.NegotiationResult.Start();
        }

        private bool TryFallbackTransport()
        {
            if (this.State != ConnectionStates.Connecting)
            {
                return false;
            }
            if (this.BufferedMessages != null)
            {
                this.BufferedMessages.Clear();
            }
            this.Transport.Stop();
            this.Transport = null;
            switch (this.NextProtocolToTry)
            {
                case SupportedProtocols.Unknown:
                    return false;

                case SupportedProtocols.HTTP:
                    this.Transport = new PollingTransport(this);
                    this.NextProtocolToTry = SupportedProtocols.Unknown;
                    break;

                case SupportedProtocols.WebSocket:
                    this.Transport = new WebSocketTransport(this);
                    break;

                case SupportedProtocols.ServerSentEvents:
                    this.Transport = new ServerSentEventsTransport(this);
                    this.NextProtocolToTry = SupportedProtocols.HTTP;
                    break;
            }
            this.TransportConnectionStartedAt = new DateTime?(DateTime.UtcNow);
            this.Transport.Connect();
            if (this.PingRequest != null)
            {
                this.PingRequest.Abort();
            }
            return true;
        }

        public System.Uri Uri { get; private set; }

        public ConnectionStates State
        {
            get => 
                this._state;
            private set
            {
                ConnectionStates oldState = this._state;
                this._state = value;
                if (this.OnStateChanged != null)
                {
                    this.OnStateChanged(this, oldState, this._state);
                }
            }
        }

        public NegotiationData NegotiationResult { get; private set; }

        public Hub[] Hubs { get; private set; }

        public TransportBase Transport { get; private set; }

        public ProtocolVersions Protocol { get; private set; }

        public ObservableDictionary<string, string> AdditionalQueryParams
        {
            get => 
                this.additionalQueryParams;
            set
            {
                if (this.additionalQueryParams != null)
                {
                    this.additionalQueryParams.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.AdditionalQueryParams_CollectionChanged);
                }
                this.additionalQueryParams = value;
                this.BuiltQueryParams = null;
                if (value != null)
                {
                    value.CollectionChanged += new NotifyCollectionChangedEventHandler(this.AdditionalQueryParams_CollectionChanged);
                }
            }
        }

        public bool QueryParamsOnlyForHandshake { get; set; }

        public IJsonEncoder JsonEncoder { get; set; }

        public IAuthenticationProvider AuthenticationProvider { get; set; }

        public TimeSpan PingInterval { get; set; }

        public TimeSpan ReconnectDelay { get; set; }

        public OnPrepareRequestDelegate RequestPreparator { get; set; }

        public Hub this[int idx] =>
            this.Hubs[idx];

        public Hub this[string hubName]
        {
            get
            {
                for (int i = 0; i < this.Hubs.Length; i++)
                {
                    Hub hub = this.Hubs[i];
                    if (hub.Name.Equals(hubName, StringComparison.OrdinalIgnoreCase))
                    {
                        return hub;
                    }
                }
                return null;
            }
        }

        internal ulong ClientMessageCounter { get; set; }

        private uint Timestamp =>
            ((uint) DateTime.UtcNow.Subtract(new DateTime(0x7b2, 1, 1)).Ticks);

        private string ConnectionData
        {
            get
            {
                if (!string.IsNullOrEmpty(this.BuiltConnectionData))
                {
                    return this.BuiltConnectionData;
                }
                StringBuilder builder = new StringBuilder("[", this.Hubs.Length * 4);
                if (this.Hubs != null)
                {
                    for (int i = 0; i < this.Hubs.Length; i++)
                    {
                        builder.Append("{\"Name\":\"");
                        builder.Append(this.Hubs[i].Name);
                        builder.Append("\"}");
                        if (i < (this.Hubs.Length - 1))
                        {
                            builder.Append(",");
                        }
                    }
                }
                builder.Append("]");
                return (this.BuiltConnectionData = System.Uri.EscapeUriString(builder.ToString()));
            }
        }

        private string QueryParams
        {
            get
            {
                if ((this.AdditionalQueryParams == null) || (this.AdditionalQueryParams.Count == 0))
                {
                    return string.Empty;
                }
                if (!string.IsNullOrEmpty(this.BuiltQueryParams))
                {
                    return this.BuiltQueryParams;
                }
                StringBuilder builder = new StringBuilder(this.AdditionalQueryParams.Count * 4);
                foreach (KeyValuePair<string, string> pair in this.AdditionalQueryParams)
                {
                    builder.Append("&");
                    builder.Append(pair.Key);
                    if (!string.IsNullOrEmpty(pair.Value))
                    {
                        builder.Append("=");
                        builder.Append(System.Uri.EscapeDataString(pair.Value));
                    }
                }
                return (this.BuiltQueryParams = builder.ToString());
            }
        }
    }
}

