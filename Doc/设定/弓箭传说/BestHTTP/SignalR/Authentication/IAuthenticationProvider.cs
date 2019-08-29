namespace BestHTTP.SignalR.Authentication
{
    using BestHTTP;
    using BestHTTP.SignalR;
    using System;

    public interface IAuthenticationProvider
    {
        event OnAuthenticationFailedDelegate OnAuthenticationFailed;

        event OnAuthenticationSuccededDelegate OnAuthenticationSucceded;

        void PrepareRequest(HTTPRequest request, RequestTypes type);
        void StartAuthentication();

        bool IsPreAuthRequired { get; }
    }
}

