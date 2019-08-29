namespace BestHTTP.SignalR.Transports
{
    using BestHTTP;
    using BestHTTP.Forms;
    using BestHTTP.SignalR;
    using BestHTTP.SignalR.Messages;
    using System;
    using System.Collections.Generic;

    public abstract class PostSendTransportBase : TransportBase
    {
        protected List<HTTPRequest> sendRequestQueue;

        public PostSendTransportBase(string name, Connection con) : base(name, con)
        {
            this.sendRequestQueue = new List<HTTPRequest>();
        }

        private void OnSendRequestFinished(HTTPRequest req, HTTPResponse resp)
        {
            this.sendRequestQueue.Remove(req);
            string str = string.Empty;
            switch (req.State)
            {
                case HTTPRequestStates.Finished:
                    if (!resp.IsSuccess)
                    {
                        str = $"Send - Request Finished Successfully, but the server sent an error. Status Code: {resp.StatusCode}-{resp.Message} Message: {resp.DataAsText}";
                        break;
                    }
                    HTTPManager.Logger.Information("Transport - " + base.Name, "Send - Request Finished Successfully! " + resp.DataAsText);
                    if (!string.IsNullOrEmpty(resp.DataAsText))
                    {
                        IServerMessage msg = TransportBase.Parse(base.Connection.JsonEncoder, resp.DataAsText);
                        if (msg != null)
                        {
                            base.Connection.OnMessage(msg);
                        }
                    }
                    break;

                case HTTPRequestStates.Error:
                    str = "Send - Request Finished with Error! " + ((req.Exception == null) ? "No Exception" : (req.Exception.Message + "\n" + req.Exception.StackTrace));
                    break;

                case HTTPRequestStates.Aborted:
                    str = "Send - Request Aborted!";
                    break;

                case HTTPRequestStates.ConnectionTimedOut:
                    str = "Send - Connection Timed Out!";
                    break;

                case HTTPRequestStates.TimedOut:
                    str = "Send - Processing the request Timed Out!";
                    break;
            }
            if (!string.IsNullOrEmpty(str))
            {
                base.Connection.Error(str);
            }
        }

        protected override void SendImpl(string json)
        {
            HTTPRequest req = new HTTPRequest(base.Connection.BuildUri(RequestTypes.Send, this), HTTPMethods.Post, true, true, new OnRequestFinishedDelegate(this.OnSendRequestFinished)) {
                FormUsage = HTTPFormUsage.UrlEncoded
            };
            req.AddField("data", json);
            base.Connection.PrepareRequest(req, RequestTypes.Send);
            req.Priority = -1;
            req.Send();
            this.sendRequestQueue.Add(req);
        }
    }
}

