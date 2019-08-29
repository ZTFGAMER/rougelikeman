namespace BestHTTP.SignalR.Authentication
{
    using System;
    using System.Runtime.CompilerServices;

    public delegate void OnAuthenticationFailedDelegate(IAuthenticationProvider provider, string reason);
}

