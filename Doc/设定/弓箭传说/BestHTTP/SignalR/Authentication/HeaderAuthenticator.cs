namespace BestHTTP.SignalR.Authentication
{
    using BestHTTP;
    using BestHTTP.SignalR;
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;

    internal class HeaderAuthenticator : IAuthenticationProvider
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <User>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Roles>k__BackingField;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event OnAuthenticationFailedDelegate OnAuthenticationFailed;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event OnAuthenticationSuccededDelegate OnAuthenticationSucceded;

        public HeaderAuthenticator(string user, string roles)
        {
            this.User = user;
            this.Roles = roles;
        }

        public void PrepareRequest(HTTPRequest request, RequestTypes type)
        {
            request.SetHeader("username", this.User);
            request.SetHeader("roles", this.Roles);
        }

        public void StartAuthentication()
        {
        }

        public string User { get; private set; }

        public string Roles { get; private set; }

        public bool IsPreAuthRequired =>
            false;
    }
}

