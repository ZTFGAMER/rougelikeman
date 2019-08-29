namespace BestHTTP
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;

    internal abstract class ConnectionBase : IDisposable
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <ServerAddress>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private HTTPConnectionStates <State>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private HTTPRequest <CurrentRequest>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime <StartTime>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime <TimedOutStart>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private HTTPProxy <Proxy>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Uri <LastProcessedUri>k__BackingField;
        protected DateTime LastProcessTime;
        protected HTTPConnectionRecycledDelegate OnConnectionRecycled;
        private bool IsThreaded;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsDisposed>k__BackingField;

        public ConnectionBase(string serverAddress) : this(serverAddress, true)
        {
        }

        public ConnectionBase(string serverAddress, bool threaded)
        {
            this.ServerAddress = serverAddress;
            this.State = HTTPConnectionStates.Initial;
            this.LastProcessTime = DateTime.UtcNow;
            this.IsThreaded = threaded;
        }

        internal abstract void Abort(HTTPConnectionStates hTTPConnectionStates);
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            this.IsDisposed = true;
        }

        internal void HandleCallback()
        {
            try
            {
                this.HandleProgressCallback();
                if (this.State == HTTPConnectionStates.Upgraded)
                {
                    if (((this.CurrentRequest != null) && (this.CurrentRequest.Response != null)) && this.CurrentRequest.Response.IsUpgraded)
                    {
                        this.CurrentRequest.UpgradeCallback();
                    }
                    this.State = HTTPConnectionStates.WaitForProtocolShutdown;
                }
                else
                {
                    this.CurrentRequest.CallCallback();
                }
            }
            catch (Exception exception)
            {
                HTTPManager.Logger.Exception("ConnectionBase", "HandleCallback", exception);
            }
        }

        internal void HandleProgressCallback()
        {
            if ((this.CurrentRequest.OnProgress != null) && this.CurrentRequest.DownloadProgressChanged)
            {
                try
                {
                    this.CurrentRequest.OnProgress(this.CurrentRequest, this.CurrentRequest.Downloaded, this.CurrentRequest.DownloadLength);
                }
                catch (Exception exception)
                {
                    HTTPManager.Logger.Exception("ConnectionBase", "HandleProgressCallback - OnProgress", exception);
                }
                this.CurrentRequest.DownloadProgressChanged = false;
            }
            if ((this.CurrentRequest.OnUploadProgress != null) && this.CurrentRequest.UploadProgressChanged)
            {
                try
                {
                    this.CurrentRequest.OnUploadProgress(this.CurrentRequest, this.CurrentRequest.Uploaded, this.CurrentRequest.UploadLength);
                }
                catch (Exception exception2)
                {
                    HTTPManager.Logger.Exception("ConnectionBase", "HandleProgressCallback - OnUploadProgress", exception2);
                }
                this.CurrentRequest.UploadProgressChanged = false;
            }
        }

        internal void Process(HTTPRequest request)
        {
            if (this.State == HTTPConnectionStates.Processing)
            {
                throw new Exception("Connection already processing a request!");
            }
            this.StartTime = DateTime.MaxValue;
            this.State = HTTPConnectionStates.Processing;
            this.CurrentRequest = request;
            if (this.IsThreaded)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.ThreadFunc));
            }
            else
            {
                this.ThreadFunc(null);
            }
        }

        internal void Recycle(HTTPConnectionRecycledDelegate onConnectionRecycled)
        {
            this.OnConnectionRecycled = onConnectionRecycled;
            if (((this.State <= HTTPConnectionStates.Initial) || (this.State >= HTTPConnectionStates.WaitForProtocolShutdown)) || (this.State == HTTPConnectionStates.Redirected))
            {
                this.RecycleNow();
            }
        }

        protected void RecycleNow()
        {
            if ((this.State == HTTPConnectionStates.TimedOut) || (this.State == HTTPConnectionStates.Closed))
            {
                this.LastProcessTime = DateTime.MinValue;
            }
            this.State = HTTPConnectionStates.Free;
            this.CurrentRequest = null;
            if (this.OnConnectionRecycled != null)
            {
                this.OnConnectionRecycled(this);
                this.OnConnectionRecycled = null;
            }
        }

        protected virtual void ThreadFunc(object param)
        {
        }

        public string ServerAddress { get; protected set; }

        public HTTPConnectionStates State { get; protected set; }

        public bool IsFree =>
            ((this.State == HTTPConnectionStates.Initial) || (this.State == HTTPConnectionStates.Free));

        public bool IsActive =>
            ((this.State > HTTPConnectionStates.Initial) && (this.State < HTTPConnectionStates.Free));

        public HTTPRequest CurrentRequest { get; protected set; }

        public virtual bool IsRemovable =>
            (this.IsFree && ((DateTime.UtcNow - this.LastProcessTime) > HTTPManager.MaxConnectionIdleTime));

        public DateTime StartTime { get; protected set; }

        public DateTime TimedOutStart { get; protected set; }

        protected HTTPProxy Proxy { get; set; }

        public bool HasProxy =>
            (this.Proxy != null);

        public Uri LastProcessedUri { get; protected set; }

        protected bool IsDisposed { get; private set; }
    }
}

