namespace BestHTTP.SignalR.Hubs
{
    using BestHTTP;
    using BestHTTP.SignalR;
    using BestHTTP.SignalR.Messages;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;

    public class Hub : IHub
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Connection <BestHTTP.SignalR.Hubs.IHub.Connection>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Name>k__BackingField;
        private Dictionary<string, object> state;
        private Dictionary<ulong, ClientMessage> SentMessages;
        private Dictionary<string, OnMethodCallCallbackDelegate> MethodTable;
        private StringBuilder builder;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event OnMethodCallDelegate OnMethodCall;

        public Hub(string name) : this(name, null)
        {
        }

        public Hub(string name, Connection manager)
        {
            this.SentMessages = new Dictionary<ulong, ClientMessage>();
            this.MethodTable = new Dictionary<string, OnMethodCallCallbackDelegate>();
            this.builder = new StringBuilder();
            this.Name = name;
            ((IHub) this).Connection = manager;
        }

        bool IHub.Call(ClientMessage msg)
        {
            IHub hub = this;
            object syncRoot = hub.Connection.SyncRoot;
            lock (syncRoot)
            {
                if (!hub.Connection.SendJson(this.BuildMessage(msg)))
                {
                    return false;
                }
                this.SentMessages.Add(msg.CallIdx, msg);
            }
            return true;
        }

        void IHub.Close()
        {
            this.SentMessages.Clear();
        }

        bool IHub.HasSentMessageId(ulong id) => 
            this.SentMessages.ContainsKey(id);

        void IHub.OnMessage(IServerMessage msg)
        {
            ulong invocationId = (msg as IHubMessage).InvocationId;
            if (!this.SentMessages.TryGetValue(invocationId, out ClientMessage message))
            {
                HTTPManager.Logger.Warning("Hub - " + this.Name, "OnMessage - Sent message not found with id: " + invocationId.ToString());
            }
            else
            {
                MessageTypes type = msg.Type;
                if (type == MessageTypes.Result)
                {
                    ResultMessage result = msg as ResultMessage;
                    this.MergeState(result.State);
                    if (message.ResultCallback != null)
                    {
                        try
                        {
                            message.ResultCallback(this, message, result);
                        }
                        catch (Exception exception)
                        {
                            HTTPManager.Logger.Exception("Hub " + this.Name, "IHub.OnMessage - ResultCallback", exception);
                        }
                    }
                    this.SentMessages.Remove(invocationId);
                }
                else if (type == MessageTypes.Failure)
                {
                    FailureMessage error = msg as FailureMessage;
                    this.MergeState(error.State);
                    if (message.ResultErrorCallback != null)
                    {
                        try
                        {
                            message.ResultErrorCallback(this, message, error);
                        }
                        catch (Exception exception2)
                        {
                            HTTPManager.Logger.Exception("Hub " + this.Name, "IHub.OnMessage - ResultErrorCallback", exception2);
                        }
                    }
                    this.SentMessages.Remove(invocationId);
                }
                else if ((type == MessageTypes.Progress) && (message.ProgressCallback != null))
                {
                    try
                    {
                        message.ProgressCallback(this, message, msg as ProgressMessage);
                    }
                    catch (Exception exception3)
                    {
                        HTTPManager.Logger.Exception("Hub " + this.Name, "IHub.OnMessage - ProgressCallback", exception3);
                    }
                }
            }
        }

        void IHub.OnMethod(MethodCallMessage msg)
        {
            this.MergeState(msg.State);
            if (this.OnMethodCall != null)
            {
                try
                {
                    this.OnMethodCall(this, msg.Method, msg.Arguments);
                }
                catch (Exception exception)
                {
                    HTTPManager.Logger.Exception("Hub - " + this.Name, "IHub.OnMethod - OnMethodCall", exception);
                }
            }
            if (this.MethodTable.TryGetValue(msg.Method, out OnMethodCallCallbackDelegate delegate2) && (delegate2 != null))
            {
                try
                {
                    delegate2(this, msg);
                }
                catch (Exception exception2)
                {
                    HTTPManager.Logger.Exception("Hub - " + this.Name, "IHub.OnMethod - callback", exception2);
                }
            }
            else
            {
                HTTPManager.Logger.Warning("Hub - " + this.Name, $"[Client] {this.Name}.{msg.Method} (args: {msg.Arguments.Length})");
            }
        }

        private string BuildMessage(ClientMessage msg)
        {
            string str2;
            try
            {
                this.builder.Append("{\"H\":\"");
                this.builder.Append(this.Name);
                this.builder.Append("\",\"M\":\"");
                this.builder.Append(msg.Method);
                this.builder.Append("\",\"A\":");
                string str = string.Empty;
                if ((msg.Args != null) && (msg.Args.Length > 0))
                {
                    str = ((IHub) this).Connection.JsonEncoder.Encode(msg.Args);
                }
                else
                {
                    str = "[]";
                }
                this.builder.Append(str);
                this.builder.Append(",\"I\":\"");
                this.builder.Append(msg.CallIdx.ToString());
                this.builder.Append("\"");
                if ((msg.Hub.state != null) && (msg.Hub.state.Count > 0))
                {
                    this.builder.Append(",\"S\":");
                    str = ((IHub) this).Connection.JsonEncoder.Encode(msg.Hub.state);
                    this.builder.Append(str);
                }
                this.builder.Append("}");
                str2 = this.builder.ToString();
            }
            catch (Exception exception)
            {
                HTTPManager.Logger.Exception("Hub - " + this.Name, "Send", exception);
                str2 = null;
            }
            finally
            {
                this.builder.Length = 0;
            }
            return str2;
        }

        public bool Call(string method, params object[] args) => 
            this.Call(method, null, null, null, args);

        public bool Call(string method, OnMethodResultDelegate onResult, params object[] args) => 
            this.Call(method, onResult, null, null, args);

        public bool Call(string method, OnMethodResultDelegate onResult, OnMethodFailedDelegate onResultError, params object[] args) => 
            this.Call(method, onResult, onResultError, null, args);

        public bool Call(string method, OnMethodResultDelegate onResult, OnMethodProgressDelegate onProgress, params object[] args) => 
            this.Call(method, onResult, null, onProgress, args);

        public bool Call(string method, OnMethodResultDelegate onResult, OnMethodFailedDelegate onResultError, OnMethodProgressDelegate onProgress, params object[] args)
        {
            IHub hub = this;
            object syncRoot = hub.Connection.SyncRoot;
            lock (syncRoot)
            {
                ulong num;
                Connection connection = hub.Connection;
                connection.ClientMessageCounter = connection.ClientMessageCounter % ulong.MaxValue;
                Connection connection2 = hub.Connection;
                connection2.ClientMessageCounter = (num = connection2.ClientMessageCounter) + ((ulong) 1L);
                return hub.Call(new ClientMessage(this, method, args, num, onResult, onResultError, onProgress));
            }
        }

        private void MergeState(IDictionary<string, object> state)
        {
            if ((state != null) && (state.Count > 0))
            {
                foreach (KeyValuePair<string, object> pair in state)
                {
                    this.State[pair.Key] = pair.Value;
                }
            }
        }

        public void Off(string method)
        {
            this.MethodTable[method] = null;
        }

        public void On(string method, OnMethodCallCallbackDelegate callback)
        {
            this.MethodTable[method] = callback;
        }

        Connection IHub.Connection { get; set; }

        public string Name { get; private set; }

        public Dictionary<string, object> State
        {
            get
            {
                if (this.state == null)
                {
                    this.state = new Dictionary<string, object>();
                }
                return this.state;
            }
        }
    }
}

