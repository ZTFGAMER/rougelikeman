namespace BestHTTP
{
    using BestHTTP.Authentication;
    using BestHTTP.Caching;
    using BestHTTP.Cookies;
    using BestHTTP.Extensions;
    using BestHTTP.Logger;
    using BestHTTP.PlatformSupport.TcpClient.General;
    using Org.BouncyCastle.Crypto.Tls;
    using Org.BouncyCastle.Security;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Security;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Threading;

    internal sealed class HTTPConnection : ConnectionBase
    {
        private TcpClient Client;
        private System.IO.Stream Stream;
        private KeepAliveHeader KeepAlive;

        internal HTTPConnection(string serverAddress) : base(serverAddress)
        {
        }

        internal override void Abort(HTTPConnectionStates newState)
        {
            base.State = newState;
            if (base.State == HTTPConnectionStates.TimedOut)
            {
                base.TimedOutStart = DateTime.UtcNow;
            }
            if (this.Stream != null)
            {
                this.Stream.Dispose();
            }
        }

        private void Close()
        {
            this.KeepAlive = null;
            base.LastProcessedUri = null;
            if (this.Client != null)
            {
                try
                {
                    this.Client.Close();
                }
                catch
                {
                }
                finally
                {
                    this.Stream = null;
                    this.Client = null;
                }
            }
        }

        private void Connect()
        {
            Uri uri = !base.CurrentRequest.HasProxy ? base.CurrentRequest.CurrentUri : base.CurrentRequest.Proxy.Address;
            if (this.Client == null)
            {
                this.Client = new TcpClient();
            }
            if (!this.Client.Connected)
            {
                this.Client.ConnectTimeout = base.CurrentRequest.ConnectTimeout;
                if (HTTPManager.Logger.Level == Loglevels.All)
                {
                    HTTPManager.Logger.Verbose("HTTPConnection", $"'{base.CurrentRequest.CurrentUri.ToString()}' - Connecting to {uri.Host}:{uri.Port.ToString()}");
                }
                this.Client.Connect(uri.Host, uri.Port);
                if (HTTPManager.Logger.Level <= Loglevels.Information)
                {
                    HTTPManager.Logger.Information("HTTPConnection", "Connected to " + uri.Host + ":" + uri.Port.ToString());
                }
            }
            else if (HTTPManager.Logger.Level <= Loglevels.Information)
            {
                HTTPManager.Logger.Information("HTTPConnection", "Already connected to " + uri.Host + ":" + uri.Port.ToString());
            }
            base.StartTime = DateTime.UtcNow;
            if (this.Stream == null)
            {
                bool flag = HTTPProtocolFactory.IsSecureProtocol(base.CurrentRequest.CurrentUri);
                this.Stream = this.Client.GetStream();
                if (base.HasProxy && (!base.Proxy.IsTransparent || (flag && base.Proxy.NonTransparentForHTTPS)))
                {
                    bool flag2;
                    BinaryWriter stream = new BinaryWriter(this.Stream);
                    do
                    {
                        flag2 = false;
                        string str = $"CONNECT {base.CurrentRequest.CurrentUri.Host}:{base.CurrentRequest.CurrentUri.Port} HTTP/1.1";
                        HTTPManager.Logger.Information("HTTPConnection", "Sending " + str);
                        stream.SendAsASCII(str);
                        stream.Write(HTTPRequest.EOL);
                        stream.SendAsASCII("Proxy-Connection: Keep-Alive");
                        stream.Write(HTTPRequest.EOL);
                        stream.SendAsASCII("Connection: Keep-Alive");
                        stream.Write(HTTPRequest.EOL);
                        stream.SendAsASCII($"Host: {base.CurrentRequest.CurrentUri.Host}:{base.CurrentRequest.CurrentUri.Port}");
                        stream.Write(HTTPRequest.EOL);
                        if (base.HasProxy && (base.Proxy.Credentials != null))
                        {
                            switch (base.Proxy.Credentials.Type)
                            {
                                case AuthenticationTypes.Basic:
                                    stream.Write($"Proxy-Authorization: {("Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(base.Proxy.Credentials.UserName + ":" + base.Proxy.Credentials.Password)))}".GetASCIIBytes());
                                    stream.Write(HTTPRequest.EOL);
                                    break;

                                case AuthenticationTypes.Unknown:
                                case AuthenticationTypes.Digest:
                                {
                                    Digest digest = DigestStore.Get(base.Proxy.Address);
                                    if (digest != null)
                                    {
                                        string str2 = digest.GenerateResponseHeader(base.CurrentRequest, base.Proxy.Credentials);
                                        if (!string.IsNullOrEmpty(str2))
                                        {
                                            stream.Write($"Proxy-Authorization: {str2}".GetASCIIBytes());
                                            stream.Write(HTTPRequest.EOL);
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                        stream.Write(HTTPRequest.EOL);
                        stream.Flush();
                        base.CurrentRequest.ProxyResponse = new HTTPResponse(base.CurrentRequest, this.Stream, false, false);
                        if (!base.CurrentRequest.ProxyResponse.Receive(-1, true))
                        {
                            throw new Exception("Connection to the Proxy Server failed!");
                        }
                        if (HTTPManager.Logger.Level <= Loglevels.Information)
                        {
                            object[] objArray1 = new object[] { "Proxy returned - status code: ", base.CurrentRequest.ProxyResponse.StatusCode, " message: ", base.CurrentRequest.ProxyResponse.Message };
                            HTTPManager.Logger.Information("HTTPConnection", string.Concat(objArray1));
                        }
                        if (base.CurrentRequest.ProxyResponse.StatusCode == 0x197)
                        {
                            string str3 = DigestStore.FindBest(base.CurrentRequest.ProxyResponse.GetHeaderValues("proxy-authenticate"));
                            if (!string.IsNullOrEmpty(str3))
                            {
                                Digest orCreate = DigestStore.GetOrCreate(base.Proxy.Address);
                                orCreate.ParseChallange(str3);
                                if (((base.Proxy.Credentials != null) && orCreate.IsUriProtected(base.Proxy.Address)) && (!base.CurrentRequest.HasHeader("Proxy-Authorization") || orCreate.Stale))
                                {
                                    flag2 = true;
                                }
                            }
                        }
                        else if (!base.CurrentRequest.ProxyResponse.IsSuccess)
                        {
                            throw new Exception($"Proxy returned Status Code: "{base.CurrentRequest.ProxyResponse.StatusCode}", Message: "{base.CurrentRequest.ProxyResponse.Message}" and Response: {base.CurrentRequest.ProxyResponse.DataAsText}");
                        }
                    }
                    while (flag2);
                }
                if (flag)
                {
                    if (base.CurrentRequest.UseAlternateSSL)
                    {
                        TlsClientProtocol protocol = new TlsClientProtocol(this.Client.GetStream(), new SecureRandom());
                        List<string> hostNames = new List<string>(1) {
                            base.CurrentRequest.CurrentUri.Host
                        };
                        protocol.Connect(new LegacyTlsClient(base.CurrentRequest.CurrentUri, (base.CurrentRequest.CustomCertificateVerifyer != null) ? base.CurrentRequest.CustomCertificateVerifyer : new AlwaysValidVerifyer(), base.CurrentRequest.CustomClientCredentialsProvider, hostNames));
                        this.Stream = protocol.Stream;
                    }
                    else
                    {
                        SslStream stream = new SslStream(this.Client.GetStream(), false, (sender, cert, chain, errors) => base.CurrentRequest.CallCustomCertificationValidator(cert, chain));
                        if (!stream.IsAuthenticated)
                        {
                            stream.AuthenticateAsClient(base.CurrentRequest.CurrentUri.Host);
                        }
                        this.Stream = stream;
                    }
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            this.Close();
            base.Dispose(disposing);
        }

        private Uri GetRedirectUri(string location)
        {
            Uri uri = null;
            try
            {
                uri = new Uri(location);
                if (uri.IsFile || (uri.AbsolutePath == location))
                {
                    uri = null;
                }
            }
            catch (UriFormatException)
            {
                uri = null;
            }
            if (uri == null)
            {
                Uri uri2 = base.CurrentRequest.Uri;
                UriBuilder builder = new UriBuilder(uri2.Scheme, uri2.Host, uri2.Port, location);
                uri = builder.Uri;
            }
            return uri;
        }

        private bool LoadFromCache(Uri uri)
        {
            if (HTTPManager.Logger.Level == Loglevels.All)
            {
                HTTPManager.Logger.Verbose("HTTPConnection", $"{base.CurrentRequest.CurrentUri.ToString()} - LoadFromCache for Uri: {uri.ToString()}");
            }
            HTTPCacheFileInfo entity = HTTPCacheService.GetEntity(uri);
            if (entity == null)
            {
                HTTPManager.Logger.Warning("HTTPConnection", $"{base.CurrentRequest.CurrentUri.ToString()} - LoadFromCache for Uri: {uri.ToString()} - Cached entity not found!");
                return false;
            }
            base.CurrentRequest.Response.CacheFileInfo = entity;
            using (System.IO.Stream stream = entity.GetBodyStream(out int num))
            {
                if (stream == null)
                {
                    return false;
                }
                if (!base.CurrentRequest.Response.HasHeader("content-length"))
                {
                    List<string> list = new List<string>(1) {
                        num.ToString()
                    };
                    base.CurrentRequest.Response.Headers.Add("content-length", list);
                }
                base.CurrentRequest.Response.IsFromCache = true;
                if (!base.CurrentRequest.CacheOnly)
                {
                    base.CurrentRequest.Response.ReadRaw(stream, (long) num);
                }
            }
            return true;
        }

        private bool Receive()
        {
            SupportedProtocols protocol = (base.CurrentRequest.ProtocolHandler != SupportedProtocols.Unknown) ? base.CurrentRequest.ProtocolHandler : HTTPProtocolFactory.GetProtocolFromUri(base.CurrentRequest.CurrentUri);
            if (HTTPManager.Logger.Level == Loglevels.All)
            {
                HTTPManager.Logger.Verbose("HTTPConnection", $"{base.CurrentRequest.CurrentUri.ToString()} - Receive - protocol: {protocol.ToString()}");
            }
            base.CurrentRequest.Response = HTTPProtocolFactory.Get(protocol, base.CurrentRequest, this.Stream, base.CurrentRequest.UseStreaming, false);
            if (!base.CurrentRequest.Response.Receive(-1, true))
            {
                if (HTTPManager.Logger.Level == Loglevels.All)
                {
                    HTTPManager.Logger.Verbose("HTTPConnection", $"{base.CurrentRequest.CurrentUri.ToString()} - Receive - Failed! Response will be null, returning with false.");
                }
                base.CurrentRequest.Response = null;
                return false;
            }
            if ((base.CurrentRequest.Response.StatusCode == 0x130) && !base.CurrentRequest.DisableCache)
            {
                if (base.CurrentRequest.IsRedirected)
                {
                    if (!this.LoadFromCache(base.CurrentRequest.RedirectUri))
                    {
                        this.LoadFromCache(base.CurrentRequest.Uri);
                    }
                }
                else
                {
                    this.LoadFromCache(base.CurrentRequest.Uri);
                }
            }
            if (HTTPManager.Logger.Level == Loglevels.All)
            {
                HTTPManager.Logger.Verbose("HTTPConnection", $"{base.CurrentRequest.CurrentUri.ToString()} - Receive - Finished Successfully!");
            }
            return true;
        }

        protected override void ThreadFunc(object param)
        {
            bool flag = false;
            bool flag2 = false;
            RetryCauses none = RetryCauses.None;
            try
            {
                if (!base.HasProxy && base.CurrentRequest.HasProxy)
                {
                    base.Proxy = base.CurrentRequest.Proxy;
                }
                if (!this.TryLoadAllFromCache())
                {
                    if ((this.Client != null) && !this.Client.IsConnected())
                    {
                        this.Close();
                    }
                    do
                    {
                        if (none == RetryCauses.Reconnect)
                        {
                            this.Close();
                            Thread.Sleep(100);
                        }
                        base.LastProcessedUri = base.CurrentRequest.CurrentUri;
                        none = RetryCauses.None;
                        this.Connect();
                        if (base.State == HTTPConnectionStates.AbortRequested)
                        {
                            throw new Exception("AbortRequested");
                        }
                        if (!base.CurrentRequest.DisableCache)
                        {
                            HTTPCacheService.SetHeaders(base.CurrentRequest);
                        }
                        bool flag3 = false;
                        try
                        {
                            this.Client.NoDelay = base.CurrentRequest.TryToMinimizeTCPLatency;
                            base.CurrentRequest.SendOutTo(this.Stream);
                            flag3 = true;
                        }
                        catch (Exception exception)
                        {
                            this.Close();
                            if ((base.State == HTTPConnectionStates.TimedOut) || (base.State == HTTPConnectionStates.AbortRequested))
                            {
                                throw new Exception("AbortRequested");
                            }
                            if (flag || base.CurrentRequest.DisableRetry)
                            {
                                throw exception;
                            }
                            flag = true;
                            none = RetryCauses.Reconnect;
                        }
                        if (flag3)
                        {
                            bool flag4 = this.Receive();
                            if ((base.State == HTTPConnectionStates.TimedOut) || (base.State == HTTPConnectionStates.AbortRequested))
                            {
                                throw new Exception("AbortRequested");
                            }
                            if ((!flag4 && !flag) && !base.CurrentRequest.DisableRetry)
                            {
                                flag = true;
                                none = RetryCauses.Reconnect;
                            }
                            if (base.CurrentRequest.Response != null)
                            {
                                if (base.CurrentRequest.IsCookiesEnabled)
                                {
                                    CookieJar.Set(base.CurrentRequest.Response);
                                }
                                switch (base.CurrentRequest.Response.StatusCode)
                                {
                                    case 0x12d:
                                    case 0x12e:
                                    case 0x133:
                                    case 0x134:
                                        if (base.CurrentRequest.RedirectCount < base.CurrentRequest.MaxRedirects)
                                        {
                                            HTTPRequest currentRequest = base.CurrentRequest;
                                            currentRequest.RedirectCount++;
                                            string firstHeaderValue = base.CurrentRequest.Response.GetFirstHeaderValue("location");
                                            if (string.IsNullOrEmpty(firstHeaderValue))
                                            {
                                                throw new MissingFieldException($"Got redirect status({base.CurrentRequest.Response.StatusCode.ToString()}) without 'location' header!");
                                            }
                                            Uri redirectUri = this.GetRedirectUri(firstHeaderValue);
                                            if (HTTPManager.Logger.Level == Loglevels.All)
                                            {
                                                HTTPManager.Logger.Verbose("HTTPConnection", string.Format("{0} - Redirected to Location: '{1}' redirectUri: '{1}'", base.CurrentRequest.CurrentUri.ToString(), firstHeaderValue, redirectUri));
                                            }
                                            if (!base.CurrentRequest.CallOnBeforeRedirection(redirectUri))
                                            {
                                                HTTPManager.Logger.Information("HTTPConnection", "OnBeforeRedirection returned False");
                                            }
                                            else
                                            {
                                                base.CurrentRequest.RemoveHeader("Host");
                                                base.CurrentRequest.SetHeader("Referer", base.CurrentRequest.CurrentUri.ToString());
                                                base.CurrentRequest.RedirectUri = redirectUri;
                                                base.CurrentRequest.Response = null;
                                                bool flag5 = true;
                                                base.CurrentRequest.IsRedirected = flag5;
                                                flag2 = flag5;
                                            }
                                        }
                                        break;

                                    case 0x191:
                                    {
                                        string str = DigestStore.FindBest(base.CurrentRequest.Response.GetHeaderValues("www-authenticate"));
                                        if (!string.IsNullOrEmpty(str))
                                        {
                                            Digest orCreate = DigestStore.GetOrCreate(base.CurrentRequest.CurrentUri);
                                            orCreate.ParseChallange(str);
                                            if (((base.CurrentRequest.Credentials != null) && orCreate.IsUriProtected(base.CurrentRequest.CurrentUri)) && (!base.CurrentRequest.HasHeader("Authorization") || orCreate.Stale))
                                            {
                                                none = RetryCauses.Authenticate;
                                            }
                                        }
                                        break;
                                    }
                                    case 0x197:
                                        if (base.CurrentRequest.HasProxy)
                                        {
                                            string str2 = DigestStore.FindBest(base.CurrentRequest.Response.GetHeaderValues("proxy-authenticate"));
                                            if (!string.IsNullOrEmpty(str2))
                                            {
                                                Digest orCreate = DigestStore.GetOrCreate(base.CurrentRequest.Proxy.Address);
                                                orCreate.ParseChallange(str2);
                                                if (((base.CurrentRequest.Proxy.Credentials != null) && orCreate.IsUriProtected(base.CurrentRequest.Proxy.Address)) && (!base.CurrentRequest.HasHeader("Proxy-Authorization") || orCreate.Stale))
                                                {
                                                    none = RetryCauses.ProxyAuthenticate;
                                                }
                                            }
                                        }
                                        break;
                                }
                                this.TryStoreInCache();
                                if ((base.CurrentRequest.Response == null) || !base.CurrentRequest.Response.IsClosedManually)
                                {
                                    bool flag6 = (base.CurrentRequest.Response == null) || base.CurrentRequest.Response.HasHeaderWithValue("connection", "close");
                                    bool flag7 = !base.CurrentRequest.IsKeepAlive;
                                    if (flag6 || flag7)
                                    {
                                        this.Close();
                                    }
                                    else if (base.CurrentRequest.Response != null)
                                    {
                                        List<string> headerValues = base.CurrentRequest.Response.GetHeaderValues("keep-alive");
                                        if ((headerValues != null) && (headerValues.Count > 0))
                                        {
                                            if (this.KeepAlive == null)
                                            {
                                                this.KeepAlive = new KeepAliveHeader();
                                            }
                                            this.KeepAlive.Parse(headerValues);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    while (none != RetryCauses.None);
                }
            }
            catch (TimeoutException exception2)
            {
                base.CurrentRequest.Response = null;
                base.CurrentRequest.Exception = exception2;
                base.CurrentRequest.State = HTTPRequestStates.ConnectionTimedOut;
                this.Close();
            }
            catch (Exception exception3)
            {
                if (base.CurrentRequest != null)
                {
                    if (base.CurrentRequest.UseStreaming)
                    {
                        HTTPCacheService.DeleteEntity(base.CurrentRequest.CurrentUri, true);
                    }
                    base.CurrentRequest.Response = null;
                    switch (base.State)
                    {
                        case HTTPConnectionStates.AbortRequested:
                        case HTTPConnectionStates.Closed:
                            base.CurrentRequest.State = HTTPRequestStates.Aborted;
                            goto Label_0685;

                        case HTTPConnectionStates.TimedOut:
                            base.CurrentRequest.State = HTTPRequestStates.TimedOut;
                            goto Label_0685;
                    }
                    base.CurrentRequest.Exception = exception3;
                    base.CurrentRequest.State = HTTPRequestStates.Error;
                }
            Label_0685:
                this.Close();
            }
            finally
            {
                if (base.CurrentRequest != null)
                {
                    object locker = HTTPManager.Locker;
                    lock (locker)
                    {
                        if (((base.CurrentRequest != null) && (base.CurrentRequest.Response != null)) && base.CurrentRequest.Response.IsUpgraded)
                        {
                            base.State = HTTPConnectionStates.Upgraded;
                        }
                        else
                        {
                            base.State = !flag2 ? ((this.Client != null) ? HTTPConnectionStates.WaitForRecycle : HTTPConnectionStates.Closed) : HTTPConnectionStates.Redirected;
                        }
                        if ((base.CurrentRequest.State == HTTPRequestStates.Processing) && ((base.State == HTTPConnectionStates.Closed) || (base.State == HTTPConnectionStates.WaitForRecycle)))
                        {
                            if (base.CurrentRequest.Response != null)
                            {
                                base.CurrentRequest.State = HTTPRequestStates.Finished;
                            }
                            else
                            {
                                base.CurrentRequest.Exception = new Exception($"Remote server closed the connection before sending response header! Previous request state: {base.CurrentRequest.State.ToString()}. Connection state: {base.State.ToString()}");
                                base.CurrentRequest.State = HTTPRequestStates.Error;
                            }
                        }
                        if (base.CurrentRequest.State == HTTPRequestStates.ConnectionTimedOut)
                        {
                            base.State = HTTPConnectionStates.Closed;
                        }
                        base.LastProcessTime = DateTime.UtcNow;
                        if (base.OnConnectionRecycled != null)
                        {
                            base.RecycleNow();
                        }
                    }
                    HTTPCacheService.SaveLibrary();
                    CookieJar.Persist();
                }
            }
        }

        private bool TryLoadAllFromCache()
        {
            if (!base.CurrentRequest.DisableCache && HTTPCacheService.IsSupported)
            {
                try
                {
                    if (HTTPCacheService.IsCachedEntityExpiresInTheFuture(base.CurrentRequest))
                    {
                        if (HTTPManager.Logger.Level == Loglevels.All)
                        {
                            HTTPManager.Logger.Verbose("HTTPConnection", $"{base.CurrentRequest.CurrentUri.ToString()} - TryLoadAllFromCache - whole response loading from cache");
                        }
                        base.CurrentRequest.Response = HTTPCacheService.GetFullResponse(base.CurrentRequest);
                        if (base.CurrentRequest.Response != null)
                        {
                            return true;
                        }
                    }
                }
                catch
                {
                    HTTPCacheService.DeleteEntity(base.CurrentRequest.CurrentUri, true);
                }
            }
            return false;
        }

        private void TryStoreInCache()
        {
            if (((!base.CurrentRequest.UseStreaming && !base.CurrentRequest.DisableCache) && ((base.CurrentRequest.Response != null) && HTTPCacheService.IsSupported)) && HTTPCacheService.IsCacheble(base.CurrentRequest.CurrentUri, base.CurrentRequest.MethodType, base.CurrentRequest.Response))
            {
                if (base.CurrentRequest.IsRedirected)
                {
                    HTTPCacheService.Store(base.CurrentRequest.Uri, base.CurrentRequest.MethodType, base.CurrentRequest.Response);
                }
                else
                {
                    HTTPCacheService.Store(base.CurrentRequest.CurrentUri, base.CurrentRequest.MethodType, base.CurrentRequest.Response);
                }
            }
        }

        public override bool IsRemovable =>
            (base.IsRemovable || ((base.IsFree && (this.KeepAlive != null)) && ((DateTime.UtcNow - base.LastProcessTime) >= this.KeepAlive.TimeOut)));
    }
}

