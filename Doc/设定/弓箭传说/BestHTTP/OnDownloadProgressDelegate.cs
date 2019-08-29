namespace BestHTTP
{
    using System;
    using System.Runtime.CompilerServices;

    public delegate void OnDownloadProgressDelegate(HTTPRequest originalRequest, long downloaded, long downloadLength);
}

