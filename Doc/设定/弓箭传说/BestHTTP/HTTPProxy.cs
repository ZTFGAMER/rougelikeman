namespace BestHTTP
{
    using BestHTTP.Authentication;
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public sealed class HTTPProxy
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Uri <Address>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private BestHTTP.Authentication.Credentials <Credentials>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsTransparent>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <SendWholeUri>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <NonTransparentForHTTPS>k__BackingField;

        public HTTPProxy(Uri address) : this(address, null, false)
        {
        }

        public HTTPProxy(Uri address, BestHTTP.Authentication.Credentials credentials) : this(address, credentials, false)
        {
        }

        public HTTPProxy(Uri address, BestHTTP.Authentication.Credentials credentials, bool isTransparent) : this(address, credentials, isTransparent, true)
        {
        }

        public HTTPProxy(Uri address, BestHTTP.Authentication.Credentials credentials, bool isTransparent, bool sendWholeUri) : this(address, credentials, isTransparent, true, true)
        {
        }

        public HTTPProxy(Uri address, BestHTTP.Authentication.Credentials credentials, bool isTransparent, bool sendWholeUri, bool nonTransparentForHTTPS)
        {
            this.Address = address;
            this.Credentials = credentials;
            this.IsTransparent = isTransparent;
            this.SendWholeUri = sendWholeUri;
            this.NonTransparentForHTTPS = nonTransparentForHTTPS;
        }

        public Uri Address { get; set; }

        public BestHTTP.Authentication.Credentials Credentials { get; set; }

        public bool IsTransparent { get; set; }

        public bool SendWholeUri { get; set; }

        public bool NonTransparentForHTTPS { get; set; }
    }
}

