namespace BestHTTP.SignalR
{
    using BestHTTP;
    using BestHTTP.JSON;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public sealed class NegotiationData
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Url>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <WebSocketServerUrl>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <ConnectionToken>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <ConnectionId>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TimeSpan? <KeepAliveTimeout>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TimeSpan <DisconnectTimeout>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TimeSpan <ConnectionTimeout>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <TryWebSockets>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <ProtocolVersion>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TimeSpan <TransportConnectTimeout>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TimeSpan <LongPollDelay>k__BackingField;
        public Action<NegotiationData> OnReceived;
        public Action<NegotiationData, string> OnError;
        private HTTPRequest NegotiationRequest;
        private IConnection Connection;

        public NegotiationData(BestHTTP.SignalR.Connection connection)
        {
            this.Connection = connection;
        }

        public void Abort()
        {
            if (this.NegotiationRequest != null)
            {
                this.OnReceived = null;
                this.OnError = null;
                this.NegotiationRequest.Abort();
            }
        }

        private static object Get(Dictionary<string, object> from, string key)
        {
            if (!from.TryGetValue(key, out object obj2))
            {
                throw new Exception($"Can't get {key} from Negotiation data!");
            }
            return obj2;
        }

        private static double GetDouble(Dictionary<string, object> from, string key) => 
            ((double) Get(from, key));

        private static int GetInt(Dictionary<string, object> from, string key) => 
            ((int) ((double) Get(from, key)));

        private static string GetString(Dictionary<string, object> from, string key) => 
            (Get(from, key) as string);

        private static List<string> GetStringList(Dictionary<string, object> from, string key)
        {
            List<object> list = Get(from, key) as List<object>;
            List<string> list2 = new List<string>(list.Count);
            for (int i = 0; i < list.Count; i++)
            {
                string item = list[i] as string;
                if (item != null)
                {
                    list2.Add(item);
                }
            }
            return list2;
        }

        private void OnNegotiationRequestFinished(HTTPRequest req, HTTPResponse resp)
        {
            this.NegotiationRequest = null;
            HTTPRequestStates state = req.State;
            if (state == HTTPRequestStates.Finished)
            {
                if (resp.IsSuccess)
                {
                    HTTPManager.Logger.Information("NegotiationData", "Negotiation data arrived: " + resp.DataAsText);
                    int index = resp.DataAsText.IndexOf("{");
                    if (index < 0)
                    {
                        this.RaiseOnError("Invalid negotiation text: " + resp.DataAsText);
                    }
                    else if (this.Parse(resp.DataAsText.Substring(index)) == null)
                    {
                        this.RaiseOnError("Parsing Negotiation data failed: " + resp.DataAsText);
                    }
                    else if (this.OnReceived != null)
                    {
                        this.OnReceived(this);
                        this.OnReceived = null;
                    }
                }
                else
                {
                    this.RaiseOnError($"Negotiation request finished Successfully, but the server sent an error. Status Code: {resp.StatusCode}-{resp.Message} Message: {resp.DataAsText} Uri: {req.CurrentUri}");
                }
            }
            else if (state == HTTPRequestStates.Error)
            {
                this.RaiseOnError((req.Exception == null) ? string.Empty : (req.Exception.Message + " " + req.Exception.StackTrace));
            }
            else
            {
                this.RaiseOnError(req.State.ToString());
            }
        }

        private NegotiationData Parse(string str)
        {
            bool success = false;
            Dictionary<string, object> from = Json.Decode(str, ref success) as Dictionary<string, object>;
            if (!success)
            {
                return null;
            }
            try
            {
                this.Url = GetString(from, "Url");
                if (from.ContainsKey("webSocketServerUrl"))
                {
                    this.WebSocketServerUrl = GetString(from, "webSocketServerUrl");
                }
                this.ConnectionToken = Uri.EscapeDataString(GetString(from, "ConnectionToken"));
                this.ConnectionId = GetString(from, "ConnectionId");
                if (from.ContainsKey("KeepAliveTimeout"))
                {
                    this.KeepAliveTimeout = new TimeSpan?(TimeSpan.FromSeconds(GetDouble(from, "KeepAliveTimeout")));
                }
                this.DisconnectTimeout = TimeSpan.FromSeconds(GetDouble(from, "DisconnectTimeout"));
                if (from.ContainsKey("ConnectionTimeout"))
                {
                    this.ConnectionTimeout = TimeSpan.FromSeconds(GetDouble(from, "ConnectionTimeout"));
                }
                else
                {
                    this.ConnectionTimeout = TimeSpan.FromSeconds(120.0);
                }
                this.TryWebSockets = (bool) Get(from, "TryWebSockets");
                this.ProtocolVersion = GetString(from, "ProtocolVersion");
                this.TransportConnectTimeout = TimeSpan.FromSeconds(GetDouble(from, "TransportConnectTimeout"));
                if (from.ContainsKey("LongPollDelay"))
                {
                    this.LongPollDelay = TimeSpan.FromSeconds(GetDouble(from, "LongPollDelay"));
                }
            }
            catch (Exception exception)
            {
                HTTPManager.Logger.Exception("NegotiationData", "Parse", exception);
                return null;
            }
            return this;
        }

        private void RaiseOnError(string err)
        {
            HTTPManager.Logger.Error("NegotiationData", "Negotiation request failed with error: " + err);
            if (this.OnError != null)
            {
                this.OnError(this, err);
                this.OnError = null;
            }
        }

        public void Start()
        {
            this.NegotiationRequest = new HTTPRequest(this.Connection.BuildUri(RequestTypes.Negotiate), HTTPMethods.Get, true, true, new OnRequestFinishedDelegate(this.OnNegotiationRequestFinished));
            this.Connection.PrepareRequest(this.NegotiationRequest, RequestTypes.Negotiate);
            this.NegotiationRequest.Send();
            HTTPManager.Logger.Information("NegotiationData", "Negotiation request sent");
        }

        public string Url { get; private set; }

        public string WebSocketServerUrl { get; private set; }

        public string ConnectionToken { get; private set; }

        public string ConnectionId { get; private set; }

        public TimeSpan? KeepAliveTimeout { get; private set; }

        public TimeSpan DisconnectTimeout { get; private set; }

        public TimeSpan ConnectionTimeout { get; private set; }

        public bool TryWebSockets { get; private set; }

        public string ProtocolVersion { get; private set; }

        public TimeSpan TransportConnectTimeout { get; private set; }

        public TimeSpan LongPollDelay { get; private set; }
    }
}

