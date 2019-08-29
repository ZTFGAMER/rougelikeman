namespace BestHTTP
{
    using System;

    internal enum RetryCauses
    {
        None,
        Reconnect,
        Authenticate,
        ProxyAuthenticate
    }
}

