namespace BestHTTP.Caching
{
    using BestHTTP;
    using BestHTTP.Logger;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;

    public static class HTTPCacheService
    {
        private const int LibraryVersion = 2;
        private static bool isSupported;
        private static bool IsSupportCheckDone;
        private static Dictionary<Uri, HTTPCacheFileInfo> library;
        private static Dictionary<ulong, HTTPCacheFileInfo> UsedIndexes = new Dictionary<ulong, HTTPCacheFileInfo>();
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static string <CacheFolder>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static string <LibraryPath>k__BackingField;
        private static bool InClearThread;
        private static bool InMaintainenceThread;
        private static ulong NextNameIDX = 1L;
        [CompilerGenerated]
        private static Predicate<string> <>f__am$cache0;
        [CompilerGenerated]
        private static Predicate<string> <>f__am$cache1;
        [CompilerGenerated]
        private static WaitCallback <>f__am$cache2;

        public static void BeginClear()
        {
            if (IsSupported && !InClearThread)
            {
                InClearThread = true;
                SetupCacheFolder();
                if (<>f__am$cache2 == null)
                {
                    <>f__am$cache2 = param => ClearImpl(param);
                }
                ThreadPool.QueueUserWorkItem(<>f__am$cache2);
            }
        }

        public static void BeginMaintainence(HTTPCacheMaintananceParams maintananceParam)
        {
            <BeginMaintainence>c__AnonStorey0 storey = new <BeginMaintainence>c__AnonStorey0 {
                maintananceParam = maintananceParam
            };
            if (storey.maintananceParam == null)
            {
                throw new ArgumentNullException("maintananceParams == null");
            }
            if (IsSupported && !InMaintainenceThread)
            {
                InMaintainenceThread = true;
                SetupCacheFolder();
                ThreadPool.QueueUserWorkItem(new WaitCallback(storey.<>m__0));
            }
        }

        internal static void CheckSetup()
        {
            if (IsSupported)
            {
                try
                {
                    SetupCacheFolder();
                    LoadLibrary();
                }
                catch
                {
                }
            }
        }

        private static void ClearImpl(object param)
        {
            if (IsSupported)
            {
                try
                {
                    string[] files = Directory.GetFiles(CacheFolder);
                    for (int i = 0; i < files.Length; i++)
                    {
                        try
                        {
                            File.Delete(files[i]);
                        }
                        catch
                        {
                        }
                    }
                }
                finally
                {
                    UsedIndexes.Clear();
                    library.Clear();
                    NextNameIDX = 1L;
                    SaveLibrary();
                    InClearThread = false;
                }
            }
        }

        internal static bool DeleteEntity(Uri uri, bool removeFromLibrary = true)
        {
            bool flag4;
            if (!IsSupported)
            {
                return false;
            }
            object obj3 = HTTPCacheFileLock.Acquire(uri);
            lock (obj3)
            {
                try
                {
                    object library = Library;
                    lock (library)
                    {
                        bool flag3 = Library.TryGetValue(uri, out HTTPCacheFileInfo info);
                        if (flag3)
                        {
                            info.Delete();
                        }
                        if (flag3 && removeFromLibrary)
                        {
                            Library.Remove(uri);
                            UsedIndexes.Remove(info.MappedNameIDX);
                        }
                        flag4 = true;
                    }
                }
                finally
                {
                }
            }
            return flag4;
        }

        private static void DeleteUnusedFiles()
        {
            if (IsSupported)
            {
                CheckSetup();
                string[] files = Directory.GetFiles(CacheFolder);
                for (int i = 0; i < files.Length; i++)
                {
                    try
                    {
                        string fileName = Path.GetFileName(files[i]);
                        ulong result = 0L;
                        bool flag = false;
                        if (ulong.TryParse(fileName, NumberStyles.AllowHexSpecifier, null, out result))
                        {
                            object library = Library;
                            lock (library)
                            {
                                flag = !UsedIndexes.ContainsKey(result);
                            }
                        }
                        else
                        {
                            flag = true;
                        }
                        if (flag)
                        {
                            File.Delete(files[i]);
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

        public static int GetCacheEntityCount()
        {
            if (!IsSupported)
            {
                return 0;
            }
            CheckSetup();
            object library = Library;
            lock (library)
            {
                return Library.Count;
            }
        }

        public static ulong GetCacheSize()
        {
            ulong num = 0L;
            if (IsSupported)
            {
                CheckSetup();
                object library = Library;
                lock (library)
                {
                    foreach (KeyValuePair<Uri, HTTPCacheFileInfo> pair in Library)
                    {
                        if (pair.Value.BodyLength > 0)
                        {
                            num += pair.Value.BodyLength;
                        }
                    }
                }
            }
            return num;
        }

        internal static HTTPCacheFileInfo GetEntity(Uri uri)
        {
            if (!IsSupported)
            {
                return null;
            }
            HTTPCacheFileInfo info = null;
            object library = Library;
            lock (library)
            {
                Library.TryGetValue(uri, out info);
            }
            return info;
        }

        internal static HTTPResponse GetFullResponse(HTTPRequest request)
        {
            if (IsSupported)
            {
                object library = Library;
                lock (library)
                {
                    if (Library.TryGetValue(request.CurrentUri, out HTTPCacheFileInfo info))
                    {
                        return info.ReadResponseTo(request);
                    }
                }
            }
            return null;
        }

        internal static ulong GetNameIdx()
        {
            object library = Library;
            lock (library)
            {
                ulong nextNameIDX = NextNameIDX;
                do
                {
                    NextNameIDX = (NextNameIDX += ((ulong) 1L)) % ulong.MaxValue;
                }
                while (UsedIndexes.ContainsKey(NextNameIDX));
                return nextNameIDX;
            }
        }

        internal static bool HasEntity(Uri uri)
        {
            if (!IsSupported)
            {
                return false;
            }
            object library = Library;
            lock (library)
            {
                return Library.ContainsKey(uri);
            }
        }

        internal static bool IsCacheble(Uri uri, HTTPMethods method, HTTPResponse response)
        {
            if (!IsSupported)
            {
                return false;
            }
            if (method != HTTPMethods.Get)
            {
                return false;
            }
            if (response == null)
            {
                return false;
            }
            if ((response.StatusCode < 200) || (response.StatusCode >= 400))
            {
                return false;
            }
            List<string> headerValues = response.GetHeaderValues("cache-control");
            if (headerValues != null)
            {
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = delegate (string headerValue) {
                        string str = headerValue.ToLower();
                        return str.Contains("no-store") || str.Contains("no-cache");
                    };
                }
                if (headerValues.Exists(<>f__am$cache0))
                {
                    return false;
                }
            }
            List<string> list2 = response.GetHeaderValues("pragma");
            if (list2 != null)
            {
                if (<>f__am$cache1 == null)
                {
                    <>f__am$cache1 = delegate (string headerValue) {
                        string str = headerValue.ToLower();
                        return str.Contains("no-store") || str.Contains("no-cache");
                    };
                }
                if (list2.Exists(<>f__am$cache1))
                {
                    return false;
                }
            }
            if (response.GetHeaderValues("content-range") != null)
            {
                return false;
            }
            return true;
        }

        internal static bool IsCachedEntityExpiresInTheFuture(HTTPRequest request)
        {
            if (IsSupported)
            {
                object library = Library;
                lock (library)
                {
                    if (Library.TryGetValue(request.CurrentUri, out HTTPCacheFileInfo info))
                    {
                        return info.WillExpireInTheFuture();
                    }
                }
            }
            return false;
        }

        private static void LoadLibrary()
        {
            if ((HTTPCacheService.library == null) && IsSupported)
            {
                HTTPCacheService.library = new Dictionary<Uri, HTTPCacheFileInfo>(new UriComparer());
                if (!File.Exists(LibraryPath))
                {
                    DeleteUnusedFiles();
                }
                else
                {
                    try
                    {
                        int num;
                        object library = HTTPCacheService.library;
                        lock (library)
                        {
                            using (FileStream stream = new FileStream(LibraryPath, FileMode.Open))
                            {
                                using (BinaryReader reader = new BinaryReader(stream))
                                {
                                    num = reader.ReadInt32();
                                    if (num > 1)
                                    {
                                        NextNameIDX = reader.ReadUInt64();
                                    }
                                    int num2 = reader.ReadInt32();
                                    for (int i = 0; i < num2; i++)
                                    {
                                        Uri uri = new Uri(reader.ReadString());
                                        HTTPCacheFileInfo info = new HTTPCacheFileInfo(uri, reader, num);
                                        if (info.IsExists())
                                        {
                                            HTTPCacheService.library.Add(uri, info);
                                            if (num > 1)
                                            {
                                                UsedIndexes.Add(info.MappedNameIDX, info);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (num == 1)
                        {
                            BeginClear();
                        }
                        else
                        {
                            DeleteUnusedFiles();
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

        internal static Stream PrepareStreamed(Uri uri, HTTPResponse response)
        {
            Stream saveStream;
            if (!IsSupported)
            {
                return null;
            }
            object library = Library;
            lock (library)
            {
                if (!Library.TryGetValue(uri, out HTTPCacheFileInfo info))
                {
                    Library.Add(uri, info = new HTTPCacheFileInfo(uri));
                    UsedIndexes.Add(info.MappedNameIDX, info);
                }
                try
                {
                    saveStream = info.GetSaveStream(response);
                }
                catch
                {
                    DeleteEntity(uri, true);
                    throw;
                }
            }
            return saveStream;
        }

        internal static void SaveLibrary()
        {
            if ((HTTPCacheService.library != null) && IsSupported)
            {
                try
                {
                    object library = Library;
                    lock (library)
                    {
                        using (FileStream stream = new FileStream(LibraryPath, FileMode.Create))
                        {
                            using (BinaryWriter writer = new BinaryWriter(stream))
                            {
                                writer.Write(2);
                                writer.Write(NextNameIDX);
                                writer.Write(Library.Count);
                                foreach (KeyValuePair<Uri, HTTPCacheFileInfo> pair in Library)
                                {
                                    writer.Write(pair.Key.ToString());
                                    pair.Value.SaveTo(writer);
                                }
                            }
                        }
                    }
                }
                catch
                {
                }
            }
        }

        internal static void SetBodyLength(Uri uri, int bodyLength)
        {
            if (IsSupported)
            {
                object library = Library;
                lock (library)
                {
                    if (Library.TryGetValue(uri, out HTTPCacheFileInfo info))
                    {
                        info.BodyLength = bodyLength;
                    }
                    else
                    {
                        Library.Add(uri, info = new HTTPCacheFileInfo(uri, DateTime.UtcNow, bodyLength));
                        UsedIndexes.Add(info.MappedNameIDX, info);
                    }
                }
            }
        }

        internal static void SetHeaders(HTTPRequest request)
        {
            if (IsSupported)
            {
                object library = Library;
                lock (library)
                {
                    if (Library.TryGetValue(request.CurrentUri, out HTTPCacheFileInfo info))
                    {
                        info.SetUpRevalidationHeaders(request);
                    }
                }
            }
        }

        internal static void SetupCacheFolder()
        {
            if (IsSupported)
            {
                try
                {
                    if (string.IsNullOrEmpty(CacheFolder) || string.IsNullOrEmpty(LibraryPath))
                    {
                        CacheFolder = Path.Combine(HTTPManager.GetRootCacheFolder(), "HTTPCache");
                        if (!Directory.Exists(CacheFolder))
                        {
                            Directory.CreateDirectory(CacheFolder);
                        }
                        LibraryPath = Path.Combine(HTTPManager.GetRootCacheFolder(), "Library");
                    }
                }
                catch
                {
                    isSupported = false;
                    HTTPManager.Logger.Warning("HTTPCacheService", "Cache Service Disabled!");
                }
            }
        }

        internal static HTTPCacheFileInfo Store(Uri uri, HTTPMethods method, HTTPResponse response)
        {
            if (((response == null) || (response.Data == null)) || (response.Data.Length == 0))
            {
                return null;
            }
            if (!IsSupported)
            {
                return null;
            }
            HTTPCacheFileInfo info = null;
            object library = Library;
            lock (library)
            {
                if (!Library.TryGetValue(uri, out info))
                {
                    Library.Add(uri, info = new HTTPCacheFileInfo(uri));
                    UsedIndexes.Add(info.MappedNameIDX, info);
                }
                try
                {
                    info.Store(response);
                    if (HTTPManager.Logger.Level == Loglevels.All)
                    {
                        HTTPManager.Logger.Verbose("HTTPCacheService", $"{uri.ToString()} - Saved to cache");
                    }
                }
                catch
                {
                    DeleteEntity(uri, true);
                    throw;
                }
            }
            return info;
        }

        public static bool IsSupported
        {
            get
            {
                if (!IsSupportCheckDone)
                {
                    try
                    {
                        File.Exists(HTTPManager.GetRootCacheFolder());
                        isSupported = true;
                    }
                    catch
                    {
                        isSupported = false;
                        HTTPManager.Logger.Warning("HTTPCacheService", "Cache Service Disabled!");
                    }
                    finally
                    {
                        IsSupportCheckDone = true;
                    }
                }
                return isSupported;
            }
        }

        private static Dictionary<Uri, HTTPCacheFileInfo> Library
        {
            get
            {
                LoadLibrary();
                return library;
            }
        }

        internal static string CacheFolder
        {
            [CompilerGenerated]
            get => 
                <CacheFolder>k__BackingField;
            [CompilerGenerated]
            private set => 
                (<CacheFolder>k__BackingField = value);
        }

        private static string LibraryPath
        {
            [CompilerGenerated]
            get => 
                <LibraryPath>k__BackingField;
            [CompilerGenerated]
            set => 
                (<LibraryPath>k__BackingField = value);
        }

        [CompilerGenerated]
        private sealed class <BeginMaintainence>c__AnonStorey0
        {
            internal HTTPCacheMaintananceParams maintananceParam;

            internal void <>m__0(object param)
            {
                try
                {
                    object library = HTTPCacheService.Library;
                    lock (library)
                    {
                        DateTime time = DateTime.UtcNow - this.maintananceParam.DeleteOlder;
                        List<HTTPCacheFileInfo> list = new List<HTTPCacheFileInfo>();
                        foreach (KeyValuePair<Uri, HTTPCacheFileInfo> pair in HTTPCacheService.Library)
                        {
                            if ((pair.Value.LastAccess < time) && HTTPCacheService.DeleteEntity(pair.Key, false))
                            {
                                list.Add(pair.Value);
                            }
                        }
                        for (int i = 0; i < list.Count; i++)
                        {
                            HTTPCacheService.Library.Remove(list[i].Uri);
                            HTTPCacheService.UsedIndexes.Remove(list[i].MappedNameIDX);
                        }
                        list.Clear();
                        ulong cacheSize = HTTPCacheService.GetCacheSize();
                        if (cacheSize > this.maintananceParam.MaxCacheSize)
                        {
                            List<HTTPCacheFileInfo> list2 = new List<HTTPCacheFileInfo>(HTTPCacheService.library.Count);
                            foreach (KeyValuePair<Uri, HTTPCacheFileInfo> pair2 in HTTPCacheService.library)
                            {
                                list2.Add(pair2.Value);
                            }
                            list2.Sort();
                            int num3 = 0;
                            while ((cacheSize >= this.maintananceParam.MaxCacheSize) && (num3 < list2.Count))
                            {
                                try
                                {
                                    HTTPCacheFileInfo info = list2[num3];
                                    ulong bodyLength = (ulong) info.BodyLength;
                                    HTTPCacheService.DeleteEntity(info.Uri, true);
                                    cacheSize -= bodyLength;
                                    continue;
                                }
                                catch
                                {
                                    continue;
                                }
                                finally
                                {
                                    num3++;
                                }
                            }
                        }
                    }
                }
                finally
                {
                    HTTPCacheService.SaveLibrary();
                    HTTPCacheService.InMaintainenceThread = false;
                }
            }
        }
    }
}

