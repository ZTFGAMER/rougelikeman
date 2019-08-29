namespace BestHTTP
{
    using BestHTTP.Caching;
    using BestHTTP.Cookies;
    using BestHTTP.Extensions;
    using BestHTTP.Logger;
    using BestHTTP.Statistics;
    using Org.BouncyCastle.Crypto.Tls;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using UnityEngine;

    public static class HTTPManager
    {
        private static byte maxConnectionPerServer;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static bool <KeepAliveDefaultValue>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static bool <IsCachingDisabled>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static TimeSpan <MaxConnectionIdleTime>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static bool <IsCookiesEnabled>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static uint <CookieJarSize>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static bool <EnablePrivateBrowsing>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static TimeSpan <ConnectTimeout>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static TimeSpan <RequestTimeout>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static Func<string> <RootCacheFolderProvider>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static HTTPProxy <Proxy>k__BackingField;
        private static HeartbeatManager heartbeats;
        private static ILogger logger;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ICertificateVerifyer <DefaultCertificateVerifyer>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static IClientCredentialsProvider <DefaultClientCredentialsProvider>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static bool <UseAlternateSSLDefaultValue>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static Func<HTTPRequest, X509Certificate, X509Chain, bool> <DefaultCertificationValidator>k__BackingField;
        public static bool TryToMinimizeTCPLatency = false;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static int <MaxPathLength>k__BackingField;
        private static Dictionary<string, List<ConnectionBase>> Connections = new Dictionary<string, List<ConnectionBase>>();
        private static List<ConnectionBase> ActiveConnections = new List<ConnectionBase>();
        private static List<ConnectionBase> FreeConnections = new List<ConnectionBase>();
        private static List<ConnectionBase> RecycledConnections = new List<ConnectionBase>();
        private static List<HTTPRequest> RequestQueue = new List<HTTPRequest>();
        private static bool IsCallingCallbacks;
        internal static object Locker = new object();
        [CompilerGenerated]
        private static HTTPConnectionRecycledDelegate <>f__mg$cache0;
        [CompilerGenerated]
        private static Predicate<HTTPRequest> <>f__am$cache0;
        [CompilerGenerated]
        private static Comparison<HTTPRequest> <>f__am$cache1;

        static HTTPManager()
        {
            MaxConnectionPerServer = 4;
            KeepAliveDefaultValue = true;
            MaxPathLength = 0xff;
            MaxConnectionIdleTime = TimeSpan.FromSeconds(20.0);
            IsCookiesEnabled = true;
            CookieJarSize = 0xa00000;
            EnablePrivateBrowsing = false;
            ConnectTimeout = TimeSpan.FromSeconds(20.0);
            RequestTimeout = TimeSpan.FromSeconds(60.0);
            logger = new DefaultLogger();
            DefaultCertificateVerifyer = null;
            UseAlternateSSLDefaultValue = true;
        }

        private static bool CanProcessFromQueue()
        {
            for (int i = 0; i < RequestQueue.Count; i++)
            {
                if (FindOrCreateFreeConnection(RequestQueue[i]) != null)
                {
                    return true;
                }
            }
            return false;
        }

        private static ConnectionBase CreateConnection(HTTPRequest request, string serverUrl)
        {
            if (request.CurrentUri.IsFile && (Application.platform != RuntimePlatform.WebGLPlayer))
            {
                return new FileConnection(serverUrl);
            }
            return new HTTPConnection(serverUrl);
        }

        private static ConnectionBase FindOrCreateFreeConnection(HTTPRequest request)
        {
            ConnectionBase base2 = null;
            string keyForRequest = GetKeyForRequest(request);
            if (Connections.TryGetValue(keyForRequest, out List<ConnectionBase> list))
            {
                int num = 0;
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].IsActive)
                    {
                        num++;
                    }
                }
                if (num <= MaxConnectionPerServer)
                {
                    for (int j = 0; (j < list.Count) && (base2 == null); j++)
                    {
                        ConnectionBase base3 = list[j];
                        if (((base3 != null) && base3.IsFree) && ((!base3.HasProxy || (base3.LastProcessedUri == null)) || base3.LastProcessedUri.Host.Equals(request.CurrentUri.Host, StringComparison.OrdinalIgnoreCase)))
                        {
                            base2 = base3;
                        }
                    }
                }
            }
            else
            {
                Connections.Add(keyForRequest, list = new List<ConnectionBase>(MaxConnectionPerServer));
            }
            if (base2 == null)
            {
                if (list.Count >= MaxConnectionPerServer)
                {
                    return null;
                }
                list.Add(base2 = CreateConnection(request, keyForRequest));
            }
            return base2;
        }

        internal static ConnectionBase GetConnectionWith(HTTPRequest request)
        {
            object locker = Locker;
            lock (locker)
            {
                for (int i = 0; i < ActiveConnections.Count; i++)
                {
                    ConnectionBase base2 = ActiveConnections[i];
                    if (base2.CurrentRequest == request)
                    {
                        return base2;
                    }
                }
                return null;
            }
        }

        public static GeneralStatistics GetGeneralStatistics(StatisticsQueryFlags queryFlags)
        {
            GeneralStatistics statistics = new GeneralStatistics {
                QueryFlags = queryFlags
            };
            if (((byte) (queryFlags & StatisticsQueryFlags.Connections)) != 0)
            {
                int num = 0;
                foreach (KeyValuePair<string, List<ConnectionBase>> pair in Connections)
                {
                    if (pair.Value != null)
                    {
                        num += pair.Value.Count;
                    }
                }
                statistics.Connections = num;
                statistics.ActiveConnections = ActiveConnections.Count;
                statistics.FreeConnections = FreeConnections.Count;
                statistics.RecycledConnections = RecycledConnections.Count;
                statistics.RequestsInQueue = RequestQueue.Count;
            }
            if (((byte) (queryFlags & StatisticsQueryFlags.Cache)) != 0)
            {
                statistics.CacheEntityCount = HTTPCacheService.GetCacheEntityCount();
                statistics.CacheSize = HTTPCacheService.GetCacheSize();
            }
            if (((byte) (queryFlags & StatisticsQueryFlags.Cookies)) != 0)
            {
                List<Cookie> all = CookieJar.GetAll();
                statistics.CookieCount = all.Count;
                uint num2 = 0;
                for (int i = 0; i < all.Count; i++)
                {
                    num2 += all[i].GuessSize();
                }
                statistics.CookieJarSize = num2;
            }
            return statistics;
        }

        private static string GetKeyForRequest(HTTPRequest request)
        {
            if (request.CurrentUri.IsFile)
            {
                return request.CurrentUri.ToString();
            }
            return (((request.Proxy == null) ? string.Empty : new UriBuilder(request.Proxy.Address.Scheme, request.Proxy.Address.Host, request.Proxy.Address.Port).Uri.ToString()) + new UriBuilder(request.CurrentUri.Scheme, request.CurrentUri.Host, request.CurrentUri.Port).Uri.ToString());
        }

        internal static string GetRootCacheFolder()
        {
            try
            {
                if (RootCacheFolderProvider != null)
                {
                    return RootCacheFolderProvider();
                }
            }
            catch (Exception exception)
            {
                Logger.Exception("HTTPManager", "GetRootCacheFolder", exception);
            }
            return Application.persistentDataPath;
        }

        private static void OnConnectionRecylced(ConnectionBase conn)
        {
            object recycledConnections = RecycledConnections;
            lock (recycledConnections)
            {
                RecycledConnections.Add(conn);
            }
        }

        public static void OnQuit()
        {
            object locker = Locker;
            lock (locker)
            {
                HTTPCacheService.SaveLibrary();
                HTTPRequest[] requestArray = RequestQueue.ToArray();
                RequestQueue.Clear();
                foreach (HTTPRequest request in requestArray)
                {
                    try
                    {
                        request.Abort();
                    }
                    catch
                    {
                    }
                }
                foreach (KeyValuePair<string, List<ConnectionBase>> pair in Connections)
                {
                    foreach (ConnectionBase base2 in pair.Value)
                    {
                        try
                        {
                            if (base2.CurrentRequest != null)
                            {
                                base2.CurrentRequest.State = HTTPRequestStates.Aborted;
                            }
                            base2.Abort(HTTPConnectionStates.Closed);
                            base2.Dispose();
                        }
                        catch
                        {
                        }
                    }
                    pair.Value.Clear();
                }
                Connections.Clear();
                OnUpdate();
            }
        }

        public static void OnUpdate()
        {
            if (Monitor.TryEnter(Locker))
            {
                try
                {
                    IsCallingCallbacks = true;
                    try
                    {
                        for (int i = 0; i < ActiveConnections.Count; i++)
                        {
                            IProtocol response;
                            ConnectionBase conn = ActiveConnections[i];
                            switch (conn.State)
                            {
                                case HTTPConnectionStates.Processing:
                                    conn.HandleProgressCallback();
                                    if ((conn.CurrentRequest.UseStreaming && (conn.CurrentRequest.Response != null)) && conn.CurrentRequest.Response.HasStreamedFragments())
                                    {
                                        conn.HandleCallback();
                                    }
                                    try
                                    {
                                        if (((!conn.CurrentRequest.UseStreaming && (conn.CurrentRequest.UploadStream == null)) || conn.CurrentRequest.EnableTimoutForStreaming) && ((DateTime.UtcNow - conn.StartTime) > conn.CurrentRequest.Timeout))
                                        {
                                            conn.Abort(HTTPConnectionStates.TimedOut);
                                        }
                                    }
                                    catch (OverflowException)
                                    {
                                        Logger.Warning("HTTPManager", "TimeSpan overflow");
                                    }
                                    break;

                                case HTTPConnectionStates.Redirected:
                                    SendRequest(conn.CurrentRequest);
                                    RecycleConnection(conn);
                                    break;

                                case HTTPConnectionStates.Upgraded:
                                    conn.HandleCallback();
                                    break;

                                case HTTPConnectionStates.WaitForProtocolShutdown:
                                    response = conn.CurrentRequest.Response as IProtocol;
                                    if (response != null)
                                    {
                                        response.HandleEvents();
                                    }
                                    if ((response == null) || response.IsClosed)
                                    {
                                        conn.HandleCallback();
                                        conn.Dispose();
                                        RecycleConnection(conn);
                                    }
                                    break;

                                case HTTPConnectionStates.WaitForRecycle:
                                    conn.CurrentRequest.FinishStreaming();
                                    conn.HandleCallback();
                                    RecycleConnection(conn);
                                    break;

                                case HTTPConnectionStates.Free:
                                    RecycleConnection(conn);
                                    break;

                                case HTTPConnectionStates.AbortRequested:
                                    response = conn.CurrentRequest.Response as IProtocol;
                                    if (response != null)
                                    {
                                        response.HandleEvents();
                                        if (response.IsClosed)
                                        {
                                            conn.HandleCallback();
                                            conn.Dispose();
                                            RecycleConnection(conn);
                                        }
                                    }
                                    break;

                                case HTTPConnectionStates.TimedOut:
                                    try
                                    {
                                        if ((DateTime.UtcNow - conn.TimedOutStart) > TimeSpan.FromMilliseconds(500.0))
                                        {
                                            Logger.Information("HTTPManager", "Hard aborting connection because of a long waiting TimedOut state");
                                            conn.CurrentRequest.Response = null;
                                            conn.CurrentRequest.State = HTTPRequestStates.TimedOut;
                                            conn.HandleCallback();
                                            RecycleConnection(conn);
                                        }
                                    }
                                    catch (OverflowException)
                                    {
                                        Logger.Warning("HTTPManager", "TimeSpan overflow");
                                    }
                                    break;

                                case HTTPConnectionStates.Closed:
                                    conn.CurrentRequest.FinishStreaming();
                                    conn.HandleCallback();
                                    RecycleConnection(conn);
                                    break;
                            }
                        }
                    }
                    finally
                    {
                        IsCallingCallbacks = false;
                    }
                    if (Monitor.TryEnter(RecycledConnections))
                    {
                        try
                        {
                            if (RecycledConnections.Count > 0)
                            {
                                for (int i = 0; i < RecycledConnections.Count; i++)
                                {
                                    ConnectionBase item = RecycledConnections[i];
                                    if (item.IsFree)
                                    {
                                        ActiveConnections.Remove(item);
                                        FreeConnections.Add(item);
                                    }
                                }
                                RecycledConnections.Clear();
                            }
                        }
                        finally
                        {
                            Monitor.Exit(RecycledConnections);
                        }
                    }
                    if (FreeConnections.Count > 0)
                    {
                        for (int i = 0; i < FreeConnections.Count; i++)
                        {
                            ConnectionBase item = FreeConnections[i];
                            if (item.IsRemovable)
                            {
                                List<ConnectionBase> list = null;
                                if (Connections.TryGetValue(item.ServerAddress, out list))
                                {
                                    list.Remove(item);
                                }
                                item.Dispose();
                                FreeConnections.RemoveAt(i);
                                i--;
                            }
                        }
                    }
                    if (CanProcessFromQueue())
                    {
                        if (<>f__am$cache0 == null)
                        {
                            <>f__am$cache0 = req => req.Priority != 0;
                        }
                        if (RequestQueue.Find(<>f__am$cache0) != null)
                        {
                            if (<>f__am$cache1 == null)
                            {
                                <>f__am$cache1 = (req1, req2) => req1.Priority - req2.Priority;
                            }
                            RequestQueue.Sort(<>f__am$cache1);
                        }
                        HTTPRequest[] requestArray = RequestQueue.ToArray();
                        RequestQueue.Clear();
                        for (int i = 0; i < requestArray.Length; i++)
                        {
                            SendRequest(requestArray[i]);
                        }
                    }
                }
                finally
                {
                    Monitor.Exit(Locker);
                }
            }
            if (heartbeats != null)
            {
                heartbeats.Update();
            }
        }

        private static void RecycleConnection(ConnectionBase conn)
        {
            if (<>f__mg$cache0 == null)
            {
                <>f__mg$cache0 = new HTTPConnectionRecycledDelegate(HTTPManager.OnConnectionRecylced);
            }
            conn.Recycle(<>f__mg$cache0);
        }

        internal static bool RemoveFromQueue(HTTPRequest request) => 
            RequestQueue.Remove(request);

        public static HTTPRequest SendRequest(HTTPRequest request)
        {
            object locker = Locker;
            lock (locker)
            {
                Setup();
                if (IsCallingCallbacks)
                {
                    request.State = HTTPRequestStates.Queued;
                    RequestQueue.Add(request);
                }
                else
                {
                    SendRequestImpl(request);
                }
                return request;
            }
        }

        public static HTTPRequest SendRequest(string url, OnRequestFinishedDelegate callback) => 
            SendRequest(new HTTPRequest(new Uri(url), HTTPMethods.Get, callback));

        public static HTTPRequest SendRequest(string url, HTTPMethods methodType, OnRequestFinishedDelegate callback) => 
            SendRequest(new HTTPRequest(new Uri(url), methodType, callback));

        public static HTTPRequest SendRequest(string url, HTTPMethods methodType, bool isKeepAlive, OnRequestFinishedDelegate callback) => 
            SendRequest(new HTTPRequest(new Uri(url), methodType, isKeepAlive, callback));

        public static HTTPRequest SendRequest(string url, HTTPMethods methodType, bool isKeepAlive, bool disableCache, OnRequestFinishedDelegate callback) => 
            SendRequest(new HTTPRequest(new Uri(url), methodType, isKeepAlive, disableCache, callback));

        private static void SendRequestImpl(HTTPRequest request)
        {
            <SendRequestImpl>c__AnonStorey0 storey = new <SendRequestImpl>c__AnonStorey0 {
                conn = FindOrCreateFreeConnection(request)
            };
            if (storey.conn != null)
            {
                if (ActiveConnections.Find(new Predicate<ConnectionBase>(storey.<>m__0)) == null)
                {
                    ActiveConnections.Add(storey.conn);
                }
                FreeConnections.Remove(storey.conn);
                request.State = HTTPRequestStates.Processing;
                request.Prepare();
                storey.conn.Process(request);
            }
            else
            {
                request.State = HTTPRequestStates.Queued;
                RequestQueue.Add(request);
            }
        }

        public static void Setup()
        {
            HTTPUpdateDelegator.CheckInstance();
            HTTPCacheService.CheckSetup();
            CookieJar.SetupFolder();
        }

        public static byte MaxConnectionPerServer
        {
            get => 
                maxConnectionPerServer;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("MaxConnectionPerServer must be greater than 0!");
                }
                maxConnectionPerServer = value;
            }
        }

        public static bool KeepAliveDefaultValue
        {
            [CompilerGenerated]
            get => 
                <KeepAliveDefaultValue>k__BackingField;
            [CompilerGenerated]
            set => 
                (<KeepAliveDefaultValue>k__BackingField = value);
        }

        public static bool IsCachingDisabled
        {
            [CompilerGenerated]
            get => 
                <IsCachingDisabled>k__BackingField;
            [CompilerGenerated]
            set => 
                (<IsCachingDisabled>k__BackingField = value);
        }

        public static TimeSpan MaxConnectionIdleTime
        {
            [CompilerGenerated]
            get => 
                <MaxConnectionIdleTime>k__BackingField;
            [CompilerGenerated]
            set => 
                (<MaxConnectionIdleTime>k__BackingField = value);
        }

        public static bool IsCookiesEnabled
        {
            [CompilerGenerated]
            get => 
                <IsCookiesEnabled>k__BackingField;
            [CompilerGenerated]
            set => 
                (<IsCookiesEnabled>k__BackingField = value);
        }

        public static uint CookieJarSize
        {
            [CompilerGenerated]
            get => 
                <CookieJarSize>k__BackingField;
            [CompilerGenerated]
            set => 
                (<CookieJarSize>k__BackingField = value);
        }

        public static bool EnablePrivateBrowsing
        {
            [CompilerGenerated]
            get => 
                <EnablePrivateBrowsing>k__BackingField;
            [CompilerGenerated]
            set => 
                (<EnablePrivateBrowsing>k__BackingField = value);
        }

        public static TimeSpan ConnectTimeout
        {
            [CompilerGenerated]
            get => 
                <ConnectTimeout>k__BackingField;
            [CompilerGenerated]
            set => 
                (<ConnectTimeout>k__BackingField = value);
        }

        public static TimeSpan RequestTimeout
        {
            [CompilerGenerated]
            get => 
                <RequestTimeout>k__BackingField;
            [CompilerGenerated]
            set => 
                (<RequestTimeout>k__BackingField = value);
        }

        public static Func<string> RootCacheFolderProvider
        {
            [CompilerGenerated]
            get => 
                <RootCacheFolderProvider>k__BackingField;
            [CompilerGenerated]
            set => 
                (<RootCacheFolderProvider>k__BackingField = value);
        }

        public static HTTPProxy Proxy
        {
            [CompilerGenerated]
            get => 
                <Proxy>k__BackingField;
            [CompilerGenerated]
            set => 
                (<Proxy>k__BackingField = value);
        }

        public static HeartbeatManager Heartbeats
        {
            get
            {
                if (heartbeats == null)
                {
                    heartbeats = new HeartbeatManager();
                }
                return heartbeats;
            }
        }

        public static ILogger Logger
        {
            get
            {
                if (logger == null)
                {
                    logger = new DefaultLogger();
                    logger.Level = Loglevels.None;
                }
                return logger;
            }
            set => 
                (logger = value);
        }

        public static ICertificateVerifyer DefaultCertificateVerifyer
        {
            [CompilerGenerated]
            get => 
                <DefaultCertificateVerifyer>k__BackingField;
            [CompilerGenerated]
            set => 
                (<DefaultCertificateVerifyer>k__BackingField = value);
        }

        public static IClientCredentialsProvider DefaultClientCredentialsProvider
        {
            [CompilerGenerated]
            get => 
                <DefaultClientCredentialsProvider>k__BackingField;
            [CompilerGenerated]
            set => 
                (<DefaultClientCredentialsProvider>k__BackingField = value);
        }

        public static bool UseAlternateSSLDefaultValue
        {
            [CompilerGenerated]
            get => 
                <UseAlternateSSLDefaultValue>k__BackingField;
            [CompilerGenerated]
            set => 
                (<UseAlternateSSLDefaultValue>k__BackingField = value);
        }

        public static Func<HTTPRequest, X509Certificate, X509Chain, bool> DefaultCertificationValidator
        {
            [CompilerGenerated]
            get => 
                <DefaultCertificationValidator>k__BackingField;
            [CompilerGenerated]
            set => 
                (<DefaultCertificationValidator>k__BackingField = value);
        }

        internal static int MaxPathLength
        {
            [CompilerGenerated]
            get => 
                <MaxPathLength>k__BackingField;
            [CompilerGenerated]
            set => 
                (<MaxPathLength>k__BackingField = value);
        }

        [CompilerGenerated]
        private sealed class <SendRequestImpl>c__AnonStorey0
        {
            internal ConnectionBase conn;

            internal bool <>m__0(ConnectionBase c) => 
                (c == this.conn);
        }
    }
}

