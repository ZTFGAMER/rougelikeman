namespace BestHTTP.SignalR.Messages
{
    using BestHTTP.SignalR.Hubs;
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct ClientMessage
    {
        public readonly BestHTTP.SignalR.Hubs.Hub Hub;
        public readonly string Method;
        public readonly object[] Args;
        public readonly ulong CallIdx;
        public readonly OnMethodResultDelegate ResultCallback;
        public readonly OnMethodFailedDelegate ResultErrorCallback;
        public readonly OnMethodProgressDelegate ProgressCallback;
        public ClientMessage(BestHTTP.SignalR.Hubs.Hub hub, string method, object[] args, ulong callIdx, OnMethodResultDelegate resultCallback, OnMethodFailedDelegate resultErrorCallback, OnMethodProgressDelegate progressCallback)
        {
            this.Hub = hub;
            this.Method = method;
            this.Args = args;
            this.CallIdx = callIdx;
            this.ResultCallback = resultCallback;
            this.ResultErrorCallback = resultErrorCallback;
            this.ProgressCallback = progressCallback;
        }
    }
}

