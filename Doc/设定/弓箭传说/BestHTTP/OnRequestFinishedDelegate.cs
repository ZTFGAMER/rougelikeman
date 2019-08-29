namespace BestHTTP
{
    using System;
    using System.Runtime.CompilerServices;

    public delegate void OnRequestFinishedDelegate(HTTPRequest originalRequest, HTTPResponse response);
}

