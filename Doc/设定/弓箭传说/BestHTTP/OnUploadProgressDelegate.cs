namespace BestHTTP
{
    using System;
    using System.Runtime.CompilerServices;

    public delegate void OnUploadProgressDelegate(HTTPRequest originalRequest, long uploaded, long uploadLength);
}

