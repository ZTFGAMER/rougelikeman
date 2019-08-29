namespace BestHTTP.SignalR.Authentication
{
    using BestHTTP;
    using BestHTTP.Cookies;
    using BestHTTP.SignalR;
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public sealed class SampleCookieAuthentication : IAuthenticationProvider
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Uri <AuthUri>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <UserName>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Password>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <UserRoles>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsPreAuthRequired>k__BackingField;
        private HTTPRequest AuthRequest;
        private BestHTTP.Cookies.Cookie Cookie;
        [CompilerGenerated]
        private static Predicate<BestHTTP.Cookies.Cookie> <>f__am$cache0;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event OnAuthenticationFailedDelegate OnAuthenticationFailed;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event OnAuthenticationSuccededDelegate OnAuthenticationSucceded;

        public SampleCookieAuthentication(Uri authUri, string user, string passwd, string roles)
        {
            this.AuthUri = authUri;
            this.UserName = user;
            this.Password = passwd;
            this.UserRoles = roles;
            this.IsPreAuthRequired = true;
        }

        private void OnAuthRequestFinished(HTTPRequest req, HTTPResponse resp)
        {
            this.AuthRequest = null;
            string reason = string.Empty;
            switch (req.State)
            {
                case HTTPRequestStates.Finished:
                    if (!resp.IsSuccess)
                    {
                        HTTPManager.Logger.Warning("CookieAuthentication", reason = $"Request Finished Successfully, but the server sent an error. Status Code: {resp.StatusCode}-{resp.Message} Message: {resp.DataAsText}");
                        break;
                    }
                    if (resp.Cookies != null)
                    {
                    }
                    this.Cookie = (<>f__am$cache0 != null) ? null : resp.Cookies.Find(<>f__am$cache0);
                    if (this.Cookie == null)
                    {
                        HTTPManager.Logger.Warning("CookieAuthentication", reason = "Auth. Cookie NOT found!");
                        break;
                    }
                    HTTPManager.Logger.Information("CookieAuthentication", "Auth. Cookie found!");
                    if (this.OnAuthenticationSucceded != null)
                    {
                        this.OnAuthenticationSucceded(this);
                    }
                    return;

                case HTTPRequestStates.Error:
                    HTTPManager.Logger.Warning("CookieAuthentication", reason = "Request Finished with Error! " + ((req.Exception == null) ? "No Exception" : (req.Exception.Message + "\n" + req.Exception.StackTrace)));
                    break;

                case HTTPRequestStates.Aborted:
                    HTTPManager.Logger.Warning("CookieAuthentication", reason = "Request Aborted!");
                    break;

                case HTTPRequestStates.ConnectionTimedOut:
                    HTTPManager.Logger.Error("CookieAuthentication", reason = "Connection Timed Out!");
                    break;

                case HTTPRequestStates.TimedOut:
                    HTTPManager.Logger.Error("CookieAuthentication", reason = "Processing the request Timed Out!");
                    break;
            }
            if (this.OnAuthenticationFailed != null)
            {
                this.OnAuthenticationFailed(this, reason);
            }
        }

        public void PrepareRequest(HTTPRequest request, RequestTypes type)
        {
            request.Cookies.Add(this.Cookie);
        }

        public void StartAuthentication()
        {
            this.AuthRequest = new HTTPRequest(this.AuthUri, HTTPMethods.Post, new OnRequestFinishedDelegate(this.OnAuthRequestFinished));
            this.AuthRequest.AddField("userName", this.UserName);
            this.AuthRequest.AddField("Password", this.Password);
            this.AuthRequest.AddField("roles", this.UserRoles);
            this.AuthRequest.Send();
        }

        public Uri AuthUri { get; private set; }

        public string UserName { get; private set; }

        public string Password { get; private set; }

        public string UserRoles { get; private set; }

        public bool IsPreAuthRequired { get; private set; }
    }
}

