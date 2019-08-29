namespace BestHTTP
{
    using System;
    using System.Runtime.CompilerServices;

    public delegate bool OnBeforeRedirectionDelegate(HTTPRequest originalRequest, HTTPResponse response, Uri redirectUri);
}

