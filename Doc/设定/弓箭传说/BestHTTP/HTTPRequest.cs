namespace BestHTTP
{
    using BestHTTP.Authentication;
    using BestHTTP.Cookies;
    using BestHTTP.Extensions;
    using BestHTTP.Forms;
    using BestHTTP.Logger;
    using Org.BouncyCastle.Crypto.Tls;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Threading;
    using UnityEngine;

    public sealed class HTTPRequest : IEnumerator, IEnumerator<HTTPRequest>, IDisposable
    {
        public static readonly byte[] EOL = new byte[] { 13, 10 };
        public static readonly string[] MethodNames;
        public static int UploadChunkSize;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private System.Uri <Uri>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private HTTPMethods <MethodType>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private byte[] <RawData>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Stream <UploadStream>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <DisposeUploadStream>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <UseUploadStreamLength>k__BackingField;
        public OnUploadProgressDelegate OnUploadProgress;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private OnRequestFinishedDelegate <Callback>k__BackingField;
        public OnDownloadProgressDelegate OnProgress;
        public OnRequestFinishedDelegate OnUpgraded;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <DisableRetry>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsRedirected>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private System.Uri <RedirectUri>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private HTTPResponse <Response>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private HTTPResponse <ProxyResponse>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private System.Exception <Exception>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private object <Tag>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private BestHTTP.Authentication.Credentials <Credentials>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private HTTPProxy <Proxy>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <MaxRedirects>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <UseAlternateSSL>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsCookiesEnabled>k__BackingField;
        private List<Cookie> customCookies;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private HTTPFormUsage <FormUsage>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private HTTPRequestStates <State>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <RedirectCount>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TimeSpan <ConnectTimeout>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TimeSpan <Timeout>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <EnableTimoutForStreaming>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <EnableSafeReadOnUnknownContentLength>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Priority>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ICertificateVerifyer <CustomCertificateVerifyer>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IClientCredentialsProvider <CustomClientCredentialsProvider>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private SupportedProtocols <ProtocolHandler>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <TryToMinimizeTCPLatency>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private long <Downloaded>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private long <DownloadLength>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <DownloadProgressChanged>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private long <Uploaded>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private long <UploadLength>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <UploadProgressChanged>k__BackingField;
        private bool isKeepAlive;
        private bool disableCache;
        private bool cacheOnly;
        private int streamFragmentSize;
        private bool useStreaming;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Dictionary<string, List<string>> <Headers>k__BackingField;
        private HTTPFormBase FieldCollector;
        private HTTPFormBase FormImpl;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event Func<HTTPRequest, X509Certificate, X509Chain, bool> CustomCertificationValidator;

        public event OnBeforeHeaderSendDelegate OnBeforeHeaderSend;

        public event OnBeforeRedirectionDelegate OnBeforeRedirection;

        static HTTPRequest()
        {
            string[] textArray1 = new string[7];
            HTTPMethods get = HTTPMethods.Get;
            textArray1[0] = get.ToString().ToUpper();
            HTTPMethods head = HTTPMethods.Head;
            textArray1[1] = head.ToString().ToUpper();
            HTTPMethods post = HTTPMethods.Post;
            textArray1[2] = post.ToString().ToUpper();
            HTTPMethods put = HTTPMethods.Put;
            textArray1[3] = put.ToString().ToUpper();
            HTTPMethods delete = HTTPMethods.Delete;
            textArray1[4] = delete.ToString().ToUpper();
            HTTPMethods patch = HTTPMethods.Patch;
            textArray1[5] = patch.ToString().ToUpper();
            HTTPMethods merge = HTTPMethods.Merge;
            textArray1[6] = merge.ToString().ToUpper();
            MethodNames = textArray1;
            UploadChunkSize = 0x800;
        }

        public HTTPRequest(System.Uri uri) : this(uri, HTTPMethods.Get, HTTPManager.KeepAliveDefaultValue, HTTPManager.IsCachingDisabled, null)
        {
        }

        public HTTPRequest(System.Uri uri, HTTPMethods methodType) : this(uri, methodType, HTTPManager.KeepAliveDefaultValue, HTTPManager.IsCachingDisabled || (methodType != HTTPMethods.Get), null)
        {
        }

        public HTTPRequest(System.Uri uri, OnRequestFinishedDelegate callback) : this(uri, HTTPMethods.Get, HTTPManager.KeepAliveDefaultValue, HTTPManager.IsCachingDisabled, callback)
        {
        }

        public HTTPRequest(System.Uri uri, HTTPMethods methodType, OnRequestFinishedDelegate callback) : this(uri, methodType, HTTPManager.KeepAliveDefaultValue, HTTPManager.IsCachingDisabled || (methodType != HTTPMethods.Get), callback)
        {
        }

        public HTTPRequest(System.Uri uri, bool isKeepAlive, OnRequestFinishedDelegate callback) : this(uri, HTTPMethods.Get, isKeepAlive, HTTPManager.IsCachingDisabled, callback)
        {
        }

        public HTTPRequest(System.Uri uri, HTTPMethods methodType, bool isKeepAlive, OnRequestFinishedDelegate callback) : this(uri, methodType, isKeepAlive, HTTPManager.IsCachingDisabled || (methodType != HTTPMethods.Get), callback)
        {
        }

        public HTTPRequest(System.Uri uri, bool isKeepAlive, bool disableCache, OnRequestFinishedDelegate callback) : this(uri, HTTPMethods.Get, isKeepAlive, disableCache, callback)
        {
        }

        public HTTPRequest(System.Uri uri, HTTPMethods methodType, bool isKeepAlive, bool disableCache, OnRequestFinishedDelegate callback)
        {
            this.Uri = uri;
            this.MethodType = methodType;
            this.IsKeepAlive = isKeepAlive;
            this.DisableCache = disableCache;
            this.Callback = callback;
            this.StreamFragmentSize = 0x1000;
            this.DisableRetry = methodType != HTTPMethods.Get;
            this.MaxRedirects = 0x7fffffff;
            this.RedirectCount = 0;
            this.IsCookiesEnabled = HTTPManager.IsCookiesEnabled;
            long num = 0L;
            this.DownloadLength = num;
            this.Downloaded = num;
            this.DownloadProgressChanged = false;
            this.State = HTTPRequestStates.Initial;
            this.ConnectTimeout = HTTPManager.ConnectTimeout;
            this.Timeout = HTTPManager.RequestTimeout;
            this.EnableTimoutForStreaming = false;
            this.EnableSafeReadOnUnknownContentLength = true;
            this.Proxy = HTTPManager.Proxy;
            this.UseUploadStreamLength = true;
            this.DisposeUploadStream = true;
            this.CustomCertificateVerifyer = HTTPManager.DefaultCertificateVerifyer;
            this.CustomClientCredentialsProvider = HTTPManager.DefaultClientCredentialsProvider;
            this.UseAlternateSSL = HTTPManager.UseAlternateSSLDefaultValue;
            this.CustomCertificationValidator += HTTPManager.DefaultCertificationValidator;
            this.TryToMinimizeTCPLatency = HTTPManager.TryToMinimizeTCPLatency;
        }

        public void Abort()
        {
            if (!Monitor.TryEnter(HTTPManager.Locker, TimeSpan.FromMilliseconds(100.0)))
            {
                throw new System.Exception("Wasn't able to acquire a thread lock. Abort failed!");
            }
            try
            {
                if (this.State >= HTTPRequestStates.Finished)
                {
                    HTTPManager.Logger.Warning("HTTPRequest", $"Abort - Already in a state({this.State.ToString()}) where no Abort required!");
                }
                else
                {
                    ConnectionBase connectionWith = HTTPManager.GetConnectionWith(this);
                    if (connectionWith == null)
                    {
                        if (!HTTPManager.RemoveFromQueue(this))
                        {
                            HTTPManager.Logger.Warning("HTTPRequest", "Abort - No active connection found with this request! (The request may already finished?)");
                        }
                        this.State = HTTPRequestStates.Aborted;
                        this.CallCallback();
                    }
                    else
                    {
                        if ((this.Response != null) && this.Response.IsStreamed)
                        {
                            this.Response.Dispose();
                        }
                        connectionWith.Abort(HTTPConnectionStates.AbortRequested);
                    }
                }
            }
            finally
            {
                Monitor.Exit(HTTPManager.Locker);
            }
        }

        public void AddBinaryData(string fieldName, byte[] content)
        {
            this.AddBinaryData(fieldName, content, null, null);
        }

        public void AddBinaryData(string fieldName, byte[] content, string fileName)
        {
            this.AddBinaryData(fieldName, content, fileName, null);
        }

        public void AddBinaryData(string fieldName, byte[] content, string fileName, string mimeType)
        {
            if (this.FieldCollector == null)
            {
                this.FieldCollector = new HTTPFormBase();
            }
            this.FieldCollector.AddBinaryData(fieldName, content, fileName, mimeType);
        }

        public void AddField(string fieldName, string value)
        {
            this.AddField(fieldName, value, Encoding.UTF8);
        }

        public void AddField(string fieldName, string value, Encoding e)
        {
            if (this.FieldCollector == null)
            {
                this.FieldCollector = new HTTPFormBase();
            }
            this.FieldCollector.AddField(fieldName, value, e);
        }

        public void AddHeader(string name, string value)
        {
            if (this.Headers == null)
            {
                this.Headers = new Dictionary<string, List<string>>();
            }
            if (!this.Headers.TryGetValue(name, out List<string> list))
            {
                this.Headers.Add(name, list = new List<string>(1));
            }
            list.Add(value);
        }

        internal void CallCallback()
        {
            try
            {
                if (this.Callback != null)
                {
                    this.Callback(this, this.Response);
                }
            }
            catch (System.Exception exception)
            {
                HTTPManager.Logger.Exception("HTTPRequest", "CallCallback", exception);
            }
        }

        internal bool CallCustomCertificationValidator(X509Certificate cert, X509Chain chain)
        {
            if (this.CustomCertificationValidator != null)
            {
                return this.CustomCertificationValidator(this, cert, chain);
            }
            return true;
        }

        internal bool CallOnBeforeRedirection(System.Uri redirectUri)
        {
            if (this.onBeforeRedirection != null)
            {
                return this.onBeforeRedirection(this, this.Response, redirectUri);
            }
            return true;
        }

        public void Clear()
        {
            this.ClearForm();
            this.RemoveHeaders();
            this.IsRedirected = false;
            this.RedirectCount = 0;
            long num = 0L;
            this.DownloadLength = num;
            this.Downloaded = num;
        }

        public void ClearForm()
        {
            this.FormImpl = null;
            this.FieldCollector = null;
        }

        public void Dispose()
        {
        }

        public string DumpHeaders()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                this.SendHeaders(stream);
                return stream.ToArray().AsciiToString();
            }
        }

        public void EnumerateHeaders(OnHeaderEnumerationDelegate callback)
        {
            this.EnumerateHeaders(callback, false);
        }

        public void EnumerateHeaders(OnHeaderEnumerationDelegate callback, bool callBeforeSendCallback)
        {
            if (!this.HasHeader("Host"))
            {
                this.SetHeader("Host", this.CurrentUri.Authority);
            }
            if (this.IsRedirected && !this.HasHeader("Referer"))
            {
                this.AddHeader("Referer", this.Uri.ToString());
            }
            if (!this.HasHeader("Accept-Encoding"))
            {
                this.AddHeader("Accept-Encoding", "gzip, identity");
            }
            if (this.HasProxy && !this.HasHeader("Proxy-Connection"))
            {
                this.AddHeader("Proxy-Connection", !this.IsKeepAlive ? "Close" : "Keep-Alive");
            }
            if (!this.HasHeader("Connection"))
            {
                this.AddHeader("Connection", !this.IsKeepAlive ? "Close, TE" : "Keep-Alive, TE");
            }
            if (!this.HasHeader("TE"))
            {
                this.AddHeader("TE", "identity");
            }
            if (!this.HasHeader("User-Agent"))
            {
                this.AddHeader("User-Agent", "BestHTTP");
            }
            long uploadStreamLength = -1L;
            if (this.UploadStream == null)
            {
                byte[] entityBody = this.GetEntityBody();
                uploadStreamLength = (entityBody == null) ? ((long) 0) : ((long) entityBody.Length);
                if ((this.RawData == null) && ((this.FormImpl != null) || ((this.FieldCollector != null) && !this.FieldCollector.IsEmpty)))
                {
                    this.SelectFormImplementation();
                    if (this.FormImpl != null)
                    {
                        this.FormImpl.PrepareRequest(this);
                    }
                }
            }
            else
            {
                uploadStreamLength = this.UploadStreamLength;
                if (uploadStreamLength == -1L)
                {
                    this.SetHeader("Transfer-Encoding", "Chunked");
                }
                if (!this.HasHeader("Content-Type"))
                {
                    this.SetHeader("Content-Type", "application/octet-stream");
                }
            }
            if ((uploadStreamLength > 0L) && !this.HasHeader("Content-Length"))
            {
                this.SetHeader("Content-Length", uploadStreamLength.ToString());
            }
            if (this.HasProxy && (this.Proxy.Credentials != null))
            {
                switch (this.Proxy.Credentials.Type)
                {
                    case AuthenticationTypes.Basic:
                        this.SetHeader("Proxy-Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(this.Proxy.Credentials.UserName + ":" + this.Proxy.Credentials.Password)));
                        break;

                    case AuthenticationTypes.Unknown:
                    case AuthenticationTypes.Digest:
                    {
                        Digest digest = DigestStore.Get(this.Proxy.Address);
                        if (digest != null)
                        {
                            string str = digest.GenerateResponseHeader(this, this.Proxy.Credentials);
                            if (!string.IsNullOrEmpty(str))
                            {
                                this.SetHeader("Proxy-Authorization", str);
                            }
                        }
                        break;
                    }
                }
            }
            if (this.Credentials != null)
            {
                switch (this.Credentials.Type)
                {
                    case AuthenticationTypes.Basic:
                        this.SetHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(this.Credentials.UserName + ":" + this.Credentials.Password)));
                        break;

                    case AuthenticationTypes.Unknown:
                    case AuthenticationTypes.Digest:
                    {
                        Digest digest2 = DigestStore.Get(this.CurrentUri);
                        if (digest2 != null)
                        {
                            string str2 = digest2.GenerateResponseHeader(this, this.Credentials);
                            if (!string.IsNullOrEmpty(str2))
                            {
                                this.SetHeader("Authorization", str2);
                            }
                        }
                        break;
                    }
                }
            }
            List<Cookie> customCookies = !this.IsCookiesEnabled ? null : CookieJar.Get(this.CurrentUri);
            if ((customCookies == null) || (customCookies.Count == 0))
            {
                customCookies = this.customCookies;
            }
            else if (this.customCookies != null)
            {
                for (int i = 0; i < this.customCookies.Count; i++)
                {
                    <EnumerateHeaders>c__AnonStorey0 storey = new <EnumerateHeaders>c__AnonStorey0 {
                        customCookie = this.customCookies[i]
                    };
                    int num3 = customCookies.FindIndex(new Predicate<Cookie>(storey.<>m__0));
                    if (num3 >= 0)
                    {
                        customCookies[num3] = storey.customCookie;
                    }
                    else
                    {
                        customCookies.Add(storey.customCookie);
                    }
                }
            }
            if ((customCookies != null) && (customCookies.Count > 0))
            {
                bool flag = true;
                string str3 = string.Empty;
                bool flag2 = HTTPProtocolFactory.IsSecureProtocol(this.CurrentUri);
                foreach (Cookie cookie in customCookies)
                {
                    if (!cookie.IsSecure || (cookie.IsSecure && flag2))
                    {
                        if (!flag)
                        {
                            str3 = str3 + "; ";
                        }
                        else
                        {
                            flag = false;
                        }
                        str3 = str3 + cookie.ToString();
                        cookie.LastAccess = DateTime.UtcNow;
                    }
                }
                if (!string.IsNullOrEmpty(str3))
                {
                    this.SetHeader("Cookie", str3);
                }
            }
            if (callBeforeSendCallback && (this._onBeforeHeaderSend != null))
            {
                try
                {
                    this._onBeforeHeaderSend(this);
                }
                catch (System.Exception exception)
                {
                    HTTPManager.Logger.Exception("HTTPRequest", "OnBeforeHeaderSend", exception);
                }
            }
            if ((callback != null) && (this.Headers != null))
            {
                foreach (KeyValuePair<string, List<string>> pair in this.Headers)
                {
                    callback(pair.Key, pair.Value);
                }
            }
        }

        internal void FinishStreaming()
        {
            if ((this.Response != null) && this.UseStreaming)
            {
                this.Response.FinishStreaming();
            }
        }

        internal byte[] GetEntityBody()
        {
            if (this.RawData != null)
            {
                return this.RawData;
            }
            if ((this.FormImpl != null) || ((this.FieldCollector != null) && !this.FieldCollector.IsEmpty))
            {
                this.SelectFormImplementation();
                if (this.FormImpl != null)
                {
                    return this.FormImpl.GetData();
                }
            }
            return null;
        }

        public string GetFirstHeaderValue(string name)
        {
            if (this.Headers != null)
            {
                List<string> list = null;
                if (this.Headers.TryGetValue(name, out list) && (list.Count > 0))
                {
                    return list[0];
                }
            }
            return null;
        }

        public List<string> GetHeaderValues(string name)
        {
            if (this.Headers != null)
            {
                List<string> list = null;
                if (this.Headers.TryGetValue(name, out list) && (list.Count > 0))
                {
                    return list;
                }
            }
            return null;
        }

        public bool HasHeader(string name) => 
            ((this.Headers != null) && this.Headers.ContainsKey(name));

        public bool MoveNext() => 
            (this.State < HTTPRequestStates.Finished);

        internal void Prepare()
        {
            if (this.FormUsage == HTTPFormUsage.Unity)
            {
                this.SelectFormImplementation();
            }
        }

        public bool RemoveHeader(string name) => 
            this.Headers?.Remove(name);

        public void RemoveHeaders()
        {
            if (this.Headers != null)
            {
                this.Headers.Clear();
            }
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        private HTTPFormBase SelectFormImplementation()
        {
            if (this.FormImpl != null)
            {
                return this.FormImpl;
            }
            if (this.FieldCollector == null)
            {
                return null;
            }
            switch (this.FormUsage)
            {
                case HTTPFormUsage.Automatic:
                    if (!this.FieldCollector.HasBinary && !this.FieldCollector.HasLongValue)
                    {
                        break;
                    }
                    goto Label_007F;

                case HTTPFormUsage.UrlEncoded:
                    break;

                case HTTPFormUsage.Multipart:
                    goto Label_007F;

                case HTTPFormUsage.RawJSon:
                    this.FormImpl = new RawJsonForm();
                    goto Label_00AF;

                case HTTPFormUsage.Unity:
                    this.FormImpl = new UnityForm();
                    goto Label_00AF;

                default:
                    goto Label_00AF;
            }
            this.FormImpl = new HTTPUrlEncodedForm();
            goto Label_00AF;
        Label_007F:
            this.FormImpl = new HTTPMultiPartForm();
        Label_00AF:
            this.FormImpl.CopyFrom(this.FieldCollector);
            return this.FormImpl;
        }

        public HTTPRequest Send() => 
            HTTPManager.SendRequest(this);

        private void SendHeaders(Stream stream)
        {
            <SendHeaders>c__AnonStorey1 storey = new <SendHeaders>c__AnonStorey1 {
                stream = stream
            };
            this.EnumerateHeaders(new OnHeaderEnumerationDelegate(storey.<>m__0), true);
        }

        internal void SendOutTo(Stream stream)
        {
            try
            {
                string str = (!this.HasProxy || !this.Proxy.SendWholeUri) ? this.CurrentUri.GetRequestPathAndQueryURL() : this.CurrentUri.OriginalString;
                string str2 = $"{MethodNames[(int) this.MethodType]} {str} HTTP/1.1";
                if (HTTPManager.Logger.Level <= Loglevels.Information)
                {
                    HTTPManager.Logger.Information("HTTPRequest", $"Sending request: '{str2}'");
                }
                stream.WriteArray(str2.GetASCIIBytes());
                stream.WriteArray(EOL);
                this.SendHeaders(stream);
                stream.WriteArray(EOL);
                if (this.UploadStream != null)
                {
                    stream.Flush();
                }
                byte[] rawData = this.RawData;
                if ((rawData == null) && (this.FormImpl != null))
                {
                    rawData = this.FormImpl.GetData();
                }
                if ((rawData != null) || (this.UploadStream != null))
                {
                    Stream uploadStream = this.UploadStream;
                    if (uploadStream == null)
                    {
                        uploadStream = new MemoryStream(rawData, 0, rawData.Length);
                        this.UploadLength = rawData.Length;
                    }
                    else
                    {
                        this.UploadLength = !this.UseUploadStreamLength ? -1L : this.UploadStreamLength;
                    }
                    this.Uploaded = 0L;
                    byte[] buffer2 = new byte[UploadChunkSize];
                    int count = 0;
                    while ((count = uploadStream.Read(buffer2, 0, buffer2.Length)) > 0)
                    {
                        if (!this.UseUploadStreamLength)
                        {
                            stream.WriteArray(count.ToString("X").GetASCIIBytes());
                            stream.WriteArray(EOL);
                        }
                        stream.Write(buffer2, 0, count);
                        if (!this.UseUploadStreamLength)
                        {
                            stream.WriteArray(EOL);
                        }
                        stream.Flush();
                        this.Uploaded += count;
                        this.UploadProgressChanged = true;
                    }
                    if (!this.UseUploadStreamLength)
                    {
                        stream.WriteArray("0".GetASCIIBytes());
                        stream.WriteArray(EOL);
                        stream.WriteArray(EOL);
                    }
                    stream.Flush();
                    if ((this.UploadStream == null) && (uploadStream != null))
                    {
                        uploadStream.Dispose();
                    }
                }
                else
                {
                    stream.Flush();
                }
                HTTPManager.Logger.Information("HTTPRequest", "'" + str2 + "' sent out");
            }
            finally
            {
                if ((this.UploadStream != null) && this.DisposeUploadStream)
                {
                    this.UploadStream.Dispose();
                }
            }
        }

        public void SetFields(WWWForm wwwForm)
        {
            this.FormUsage = HTTPFormUsage.Unity;
            this.FormImpl = new UnityForm(wwwForm);
        }

        public void SetForm(HTTPFormBase form)
        {
            this.FormImpl = form;
        }

        public void SetHeader(string name, string value)
        {
            if (this.Headers == null)
            {
                this.Headers = new Dictionary<string, List<string>>();
            }
            if (!this.Headers.TryGetValue(name, out List<string> list))
            {
                this.Headers.Add(name, list = new List<string>(1));
            }
            list.Clear();
            list.Add(value);
        }

        public void SetRangeHeader(int firstBytePos)
        {
            this.SetHeader("Range", $"bytes={firstBytePos}-");
        }

        public void SetRangeHeader(int firstBytePos, int lastBytePos)
        {
            this.SetHeader("Range", $"bytes={firstBytePos}-{lastBytePos}");
        }

        internal void UpgradeCallback()
        {
            if ((this.Response != null) && this.Response.IsUpgraded)
            {
                try
                {
                    if (this.OnUpgraded != null)
                    {
                        this.OnUpgraded(this, this.Response);
                    }
                }
                catch (System.Exception exception)
                {
                    HTTPManager.Logger.Exception("HTTPRequest", "UpgradeCallback", exception);
                }
            }
        }

        HTTPRequest IEnumerator<HTTPRequest>.Current =>
            this;

        public System.Uri Uri { get; private set; }

        public HTTPMethods MethodType { get; set; }

        public byte[] RawData { get; set; }

        public Stream UploadStream { get; set; }

        public bool DisposeUploadStream { get; set; }

        public bool UseUploadStreamLength { get; set; }

        public bool IsKeepAlive
        {
            get => 
                this.isKeepAlive;
            set
            {
                if (this.State == HTTPRequestStates.Processing)
                {
                    throw new NotSupportedException("Changing the IsKeepAlive property while processing the request is not supported.");
                }
                this.isKeepAlive = value;
            }
        }

        public bool DisableCache
        {
            get => 
                this.disableCache;
            set
            {
                if (this.State == HTTPRequestStates.Processing)
                {
                    throw new NotSupportedException("Changing the DisableCache property while processing the request is not supported.");
                }
                this.disableCache = value;
            }
        }

        public bool CacheOnly
        {
            get => 
                this.cacheOnly;
            set
            {
                if (this.State == HTTPRequestStates.Processing)
                {
                    throw new NotSupportedException("Changing the CacheOnly property while processing the request is not supported.");
                }
                this.cacheOnly = value;
            }
        }

        public bool UseStreaming
        {
            get => 
                this.useStreaming;
            set
            {
                if (this.State == HTTPRequestStates.Processing)
                {
                    throw new NotSupportedException("Changing the UseStreaming property while processing the request is not supported.");
                }
                this.useStreaming = value;
            }
        }

        public int StreamFragmentSize
        {
            get => 
                this.streamFragmentSize;
            set
            {
                if (this.State == HTTPRequestStates.Processing)
                {
                    throw new NotSupportedException("Changing the StreamFragmentSize property while processing the request is not supported.");
                }
                if (value < 1)
                {
                    throw new ArgumentException("StreamFragmentSize must be at least 1.");
                }
                this.streamFragmentSize = value;
            }
        }

        public OnRequestFinishedDelegate Callback { get; set; }

        public bool DisableRetry { get; set; }

        public bool IsRedirected { get; internal set; }

        public System.Uri RedirectUri { get; internal set; }

        public System.Uri CurrentUri =>
            (!this.IsRedirected ? this.Uri : this.RedirectUri);

        public HTTPResponse Response { get; internal set; }

        public HTTPResponse ProxyResponse { get; internal set; }

        public System.Exception Exception { get; internal set; }

        public object Tag { get; set; }

        public BestHTTP.Authentication.Credentials Credentials { get; set; }

        public bool HasProxy =>
            (this.Proxy != null);

        public HTTPProxy Proxy { get; set; }

        public int MaxRedirects { get; set; }

        public bool UseAlternateSSL { get; set; }

        public bool IsCookiesEnabled { get; set; }

        public List<Cookie> Cookies
        {
            get
            {
                if (this.customCookies == null)
                {
                    this.customCookies = new List<Cookie>();
                }
                return this.customCookies;
            }
            set => 
                (this.customCookies = value);
        }

        public HTTPFormUsage FormUsage { get; set; }

        public HTTPRequestStates State { get; internal set; }

        public int RedirectCount { get; internal set; }

        public TimeSpan ConnectTimeout { get; set; }

        public TimeSpan Timeout { get; set; }

        public bool EnableTimoutForStreaming { get; set; }

        public bool EnableSafeReadOnUnknownContentLength { get; set; }

        public int Priority { get; set; }

        public ICertificateVerifyer CustomCertificateVerifyer { get; set; }

        public IClientCredentialsProvider CustomClientCredentialsProvider { get; set; }

        public SupportedProtocols ProtocolHandler { get; set; }

        public bool TryToMinimizeTCPLatency { get; set; }

        internal long Downloaded { get; set; }

        internal long DownloadLength { get; set; }

        internal bool DownloadProgressChanged { get; set; }

        internal long UploadStreamLength
        {
            get
            {
                if ((this.UploadStream == null) || !this.UseUploadStreamLength)
                {
                    return -1L;
                }
                try
                {
                    return this.UploadStream.Length;
                }
                catch
                {
                    return -1L;
                }
            }
        }

        internal long Uploaded { get; set; }

        internal long UploadLength { get; set; }

        internal bool UploadProgressChanged { get; set; }

        private Dictionary<string, List<string>> Headers { get; set; }

        public object Current =>
            null;

        [CompilerGenerated]
        private sealed class <EnumerateHeaders>c__AnonStorey0
        {
            internal Cookie customCookie;

            internal bool <>m__0(Cookie c) => 
                c.Name.Equals(this.customCookie.Name);
        }

        [CompilerGenerated]
        private sealed class <SendHeaders>c__AnonStorey1
        {
            internal Stream stream;

            internal void <>m__0(string header, List<string> values)
            {
                if (!string.IsNullOrEmpty(header) && (values != null))
                {
                    byte[] aSCIIBytes = (header + ": ").GetASCIIBytes();
                    for (int i = 0; i < values.Count; i++)
                    {
                        if (string.IsNullOrEmpty(values[i]))
                        {
                            HTTPManager.Logger.Warning("HTTPRequest", $"Null/empty value for header: {header}");
                        }
                        else
                        {
                            this.stream.WriteArray(aSCIIBytes);
                            this.stream.WriteArray(values[i].GetASCIIBytes());
                            this.stream.WriteArray(HTTPRequest.EOL);
                        }
                    }
                }
            }
        }
    }
}

