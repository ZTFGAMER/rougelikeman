namespace BestHTTP.ServerSentEvents
{
    using BestHTTP;
    using BestHTTP.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class EventSource : IHeartbeat
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private System.Uri <Uri>k__BackingField;
        private States _state;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TimeSpan <ReconnectionTime>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <LastEventId>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private HTTPRequest <InternalRequest>k__BackingField;
        private Dictionary<string, OnEventDelegate> EventTable;
        private byte RetryCount;
        private DateTime RetryCalled;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event OnGeneralEventDelegate OnClosed;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event OnErrorDelegate OnError;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event OnMessageDelegate OnMessage;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event OnGeneralEventDelegate OnOpen;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event OnRetryDelegate OnRetry;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event OnStateChangedDelegate OnStateChanged;

        public EventSource(System.Uri uri)
        {
            this.Uri = uri;
            this.ReconnectionTime = TimeSpan.FromMilliseconds(2000.0);
            this.InternalRequest = new HTTPRequest(this.Uri, HTTPMethods.Get, true, true, new OnRequestFinishedDelegate(this.OnRequestFinished));
            this.InternalRequest.SetHeader("Accept", "text/event-stream");
            this.InternalRequest.SetHeader("Cache-Control", "no-cache");
            this.InternalRequest.SetHeader("Accept-Encoding", "identity");
            this.InternalRequest.ProtocolHandler = SupportedProtocols.ServerSentEvents;
            this.InternalRequest.OnUpgraded = new OnRequestFinishedDelegate(this.OnUpgraded);
            this.InternalRequest.DisableRetry = true;
        }

        void IHeartbeat.OnHeartbeatUpdate(TimeSpan dif)
        {
            if (this.State != States.Retrying)
            {
                HTTPManager.Heartbeats.Unsubscribe(this);
            }
            else if ((DateTime.UtcNow - this.RetryCalled) >= this.ReconnectionTime)
            {
                this.Open();
                if (this.State != States.Connecting)
                {
                    this.SetClosed("OnHeartbeatUpdate");
                }
                HTTPManager.Heartbeats.Unsubscribe(this);
            }
        }

        private void CallOnError(string error, string msg)
        {
            if (this.OnError != null)
            {
                try
                {
                    this.OnError(this, error);
                }
                catch (Exception exception)
                {
                    HTTPManager.Logger.Exception("EventSource", msg + " - OnError", exception);
                }
            }
        }

        private bool CallOnRetry()
        {
            if (this.OnRetry != null)
            {
                try
                {
                    return this.OnRetry(this);
                }
                catch (Exception exception)
                {
                    HTTPManager.Logger.Exception("EventSource", "CallOnRetry", exception);
                }
            }
            return true;
        }

        public void Close()
        {
            if ((this.State != States.Closing) && (this.State != States.Closed))
            {
                this.State = States.Closing;
                if (this.InternalRequest != null)
                {
                    this.InternalRequest.Abort();
                }
                else
                {
                    this.State = States.Closed;
                }
            }
        }

        public void Off(string eventName)
        {
            if ((eventName != null) && (this.EventTable != null))
            {
                this.EventTable.Remove(eventName);
            }
        }

        public void On(string eventName, OnEventDelegate action)
        {
            if (this.EventTable == null)
            {
                this.EventTable = new Dictionary<string, OnEventDelegate>();
            }
            this.EventTable[eventName] = action;
        }

        private void OnMessageReceived(EventSourceResponse resp, Message message)
        {
            if (this.State < States.Closing)
            {
                if (message.Id != null)
                {
                    this.LastEventId = message.Id;
                }
                if (message.Retry.TotalMilliseconds > 0.0)
                {
                    this.ReconnectionTime = message.Retry;
                }
                if (!string.IsNullOrEmpty(message.Data))
                {
                    if (this.OnMessage != null)
                    {
                        try
                        {
                            this.OnMessage(this, message);
                        }
                        catch (Exception exception)
                        {
                            HTTPManager.Logger.Exception("EventSource", "OnMessageReceived - OnMessage", exception);
                        }
                    }
                    if (((this.EventTable != null) && !string.IsNullOrEmpty(message.Event)) && (this.EventTable.TryGetValue(message.Event, out OnEventDelegate delegate2) && (delegate2 != null)))
                    {
                        try
                        {
                            delegate2(this, message);
                        }
                        catch (Exception exception2)
                        {
                            HTTPManager.Logger.Exception("EventSource", "OnMessageReceived - action", exception2);
                        }
                    }
                }
            }
        }

        private void OnRequestFinished(HTTPRequest req, HTTPResponse resp)
        {
            if (this.State != States.Closed)
            {
                if ((this.State == States.Closing) || (req.State == HTTPRequestStates.Aborted))
                {
                    this.SetClosed("OnRequestFinished");
                }
                else
                {
                    string str = string.Empty;
                    bool flag = true;
                    switch (req.State)
                    {
                        case HTTPRequestStates.Processing:
                            flag = !resp.HasHeader("content-length");
                            break;

                        case HTTPRequestStates.Finished:
                            if ((resp.StatusCode == 200) && !resp.HasHeaderWithValue("content-type", "text/event-stream"))
                            {
                                str = "No Content-Type header with value 'text/event-stream' present.";
                                flag = false;
                            }
                            if (((flag && (resp.StatusCode != 500)) && ((resp.StatusCode != 0x1f6) && (resp.StatusCode != 0x1f7))) && (resp.StatusCode != 0x1f8))
                            {
                                flag = false;
                                str = $"Request Finished Successfully, but the server sent an error. Status Code: {resp.StatusCode}-{resp.Message} Message: {resp.DataAsText}";
                            }
                            break;

                        case HTTPRequestStates.Error:
                            str = "Request Finished with Error! " + ((req.Exception == null) ? "No Exception" : (req.Exception.Message + "\n" + req.Exception.StackTrace));
                            break;

                        case HTTPRequestStates.Aborted:
                            str = "OnRequestFinished - Aborted without request. EventSource's State: " + this.State;
                            break;

                        case HTTPRequestStates.ConnectionTimedOut:
                            str = "Connection Timed Out!";
                            break;

                        case HTTPRequestStates.TimedOut:
                            str = "Processing the request Timed Out!";
                            break;
                    }
                    if (this.State < States.Closing)
                    {
                        if (!string.IsNullOrEmpty(str))
                        {
                            this.CallOnError(str, "OnRequestFinished");
                        }
                        if (flag)
                        {
                            this.Retry();
                        }
                        else
                        {
                            this.SetClosed("OnRequestFinished");
                        }
                    }
                    else
                    {
                        this.SetClosed("OnRequestFinished");
                    }
                }
            }
        }

        private void OnUpgraded(HTTPRequest originalRequest, HTTPResponse response)
        {
            EventSourceResponse response2 = response as EventSourceResponse;
            if (response2 == null)
            {
                this.CallOnError("Not an EventSourceResponse!", "OnUpgraded");
            }
            else
            {
                if (this.OnOpen != null)
                {
                    try
                    {
                        this.OnOpen(this);
                    }
                    catch (Exception exception)
                    {
                        HTTPManager.Logger.Exception("EventSource", "OnOpen", exception);
                    }
                }
                response2.OnMessage = (Action<EventSourceResponse, Message>) Delegate.Combine(response2.OnMessage, new Action<EventSourceResponse, Message>(this.OnMessageReceived));
                response2.StartReceive();
                this.RetryCount = 0;
                this.State = States.Open;
            }
        }

        public void Open()
        {
            if (((this.State == States.Initial) || (this.State == States.Retrying)) || (this.State == States.Closed))
            {
                this.State = States.Connecting;
                if (!string.IsNullOrEmpty(this.LastEventId))
                {
                    this.InternalRequest.SetHeader("Last-Event-ID", this.LastEventId);
                }
                this.InternalRequest.Send();
            }
        }

        private void Retry()
        {
            if ((this.RetryCount > 0) || !this.CallOnRetry())
            {
                this.SetClosed("Retry");
            }
            else
            {
                this.RetryCount = (byte) (this.RetryCount + 1);
                this.RetryCalled = DateTime.UtcNow;
                HTTPManager.Heartbeats.Subscribe(this);
                this.State = States.Retrying;
            }
        }

        private void SetClosed(string msg)
        {
            this.State = States.Closed;
            if (this.OnClosed != null)
            {
                try
                {
                    this.OnClosed(this);
                }
                catch (Exception exception)
                {
                    HTTPManager.Logger.Exception("EventSource", msg + " - OnClosed", exception);
                }
            }
        }

        public System.Uri Uri { get; private set; }

        public States State
        {
            get => 
                this._state;
            private set
            {
                States oldState = this._state;
                this._state = value;
                if (this.OnStateChanged != null)
                {
                    try
                    {
                        this.OnStateChanged(this, oldState, this._state);
                    }
                    catch (Exception exception)
                    {
                        HTTPManager.Logger.Exception("EventSource", "OnStateChanged", exception);
                    }
                }
            }
        }

        public TimeSpan ReconnectionTime { get; set; }

        public string LastEventId { get; private set; }

        public HTTPRequest InternalRequest { get; private set; }
    }
}

