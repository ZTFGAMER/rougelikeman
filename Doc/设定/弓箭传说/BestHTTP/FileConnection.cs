namespace BestHTTP
{
    using System;
    using System.IO;

    internal sealed class FileConnection : ConnectionBase
    {
        public FileConnection(string serverAddress) : base(serverAddress)
        {
        }

        internal override void Abort(HTTPConnectionStates newState)
        {
            base.State = newState;
            if (base.State == HTTPConnectionStates.TimedOut)
            {
                base.TimedOutStart = DateTime.UtcNow;
            }
            throw new NotImplementedException();
        }

        protected override void ThreadFunc(object param)
        {
            try
            {
                using (FileStream stream = new FileStream(base.CurrentRequest.CurrentUri.LocalPath, FileMode.Open, FileAccess.Read))
                {
                    Stream[] streams = new Stream[] { new MemoryStream(), stream };
                    using (StreamList list = new StreamList(streams))
                    {
                        list.Write("HTTP/1.1 200 Ok\r\n");
                        list.Write("Content-Type: application/octet-stream\r\n");
                        list.Write("Content-Length: " + stream.Length.ToString() + "\r\n");
                        list.Write("\r\n");
                        list.Seek(0L, SeekOrigin.Begin);
                        base.CurrentRequest.Response = new HTTPResponse(base.CurrentRequest, list, base.CurrentRequest.UseStreaming, false);
                        if (!base.CurrentRequest.Response.Receive(-1, true))
                        {
                            base.CurrentRequest.Response = null;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                if (base.CurrentRequest != null)
                {
                    base.CurrentRequest.Response = null;
                    HTTPConnectionStates state = base.State;
                    if (state != HTTPConnectionStates.AbortRequested)
                    {
                        if (state == HTTPConnectionStates.TimedOut)
                        {
                            goto Label_0139;
                        }
                        goto Label_014A;
                    }
                    base.CurrentRequest.State = HTTPRequestStates.Aborted;
                }
                return;
            Label_0139:
                base.CurrentRequest.State = HTTPRequestStates.TimedOut;
                return;
            Label_014A:
                base.CurrentRequest.Exception = exception;
                base.CurrentRequest.State = HTTPRequestStates.Error;
            }
            finally
            {
                base.State = HTTPConnectionStates.Closed;
                if (base.CurrentRequest.State == HTTPRequestStates.Processing)
                {
                    if (base.CurrentRequest.Response != null)
                    {
                        base.CurrentRequest.State = HTTPRequestStates.Finished;
                    }
                    else
                    {
                        base.CurrentRequest.State = HTTPRequestStates.Error;
                    }
                }
            }
        }
    }
}

