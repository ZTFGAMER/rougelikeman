namespace BestHTTP.SignalR
{
    using BestHTTP;
    using System;
    using System.Runtime.CompilerServices;

    public delegate void OnPrepareRequestDelegate(Connection connection, HTTPRequest req, RequestTypes type);
}

