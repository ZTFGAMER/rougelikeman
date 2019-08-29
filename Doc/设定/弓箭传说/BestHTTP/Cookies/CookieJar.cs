namespace BestHTTP.Cookies
{
    using BestHTTP;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public static class CookieJar
    {
        private const int Version = 1;
        private static List<Cookie> Cookies = new List<Cookie>();
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static string <CookieFolder>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static string <LibraryPath>k__BackingField;
        private static object Locker = new object();
        private static bool _isSavingSupported;
        private static bool IsSupportCheckDone;
        private static bool Loaded;

        public static void Clear()
        {
            object locker = Locker;
            lock (locker)
            {
                Load();
                Cookies.Clear();
            }
        }

        public static void Clear(string domain)
        {
            object locker = Locker;
            lock (locker)
            {
                Load();
                int index = 0;
                while (index < Cookies.Count)
                {
                    Cookie cookie = Cookies[index];
                    if (!cookie.WillExpireInTheFuture() || (cookie.Domain.IndexOf(domain) != -1))
                    {
                        Cookies.RemoveAt(index);
                    }
                    else
                    {
                        index++;
                    }
                }
            }
        }

        public static void Clear(TimeSpan olderThan)
        {
            object locker = Locker;
            lock (locker)
            {
                Load();
                int index = 0;
                while (index < Cookies.Count)
                {
                    Cookie cookie = Cookies[index];
                    if (!cookie.WillExpireInTheFuture() || ((cookie.Date + olderThan) < DateTime.UtcNow))
                    {
                        Cookies.RemoveAt(index);
                    }
                    else
                    {
                        index++;
                    }
                }
            }
        }

        private static Cookie Find(Cookie cookie, out int idx)
        {
            for (int i = 0; i < Cookies.Count; i++)
            {
                Cookie cookie2 = Cookies[i];
                if (cookie2.Equals(cookie))
                {
                    idx = i;
                    return cookie2;
                }
            }
            idx = -1;
            return null;
        }

        public static List<Cookie> Get(Uri uri)
        {
            object locker = Locker;
            lock (locker)
            {
                Load();
                List<Cookie> list = null;
                for (int i = 0; i < Cookies.Count; i++)
                {
                    Cookie item = Cookies[i];
                    if ((item.WillExpireInTheFuture() && (uri.Host.IndexOf(item.Domain) != -1)) && uri.AbsolutePath.StartsWith(item.Path))
                    {
                        if (list == null)
                        {
                            list = new List<Cookie>();
                        }
                        list.Add(item);
                    }
                }
                return list;
            }
        }

        public static List<Cookie> GetAll()
        {
            object locker = Locker;
            lock (locker)
            {
                Load();
                return Cookies;
            }
        }

        internal static void Load()
        {
            if (IsSavingSupported)
            {
                object locker = Locker;
                lock (locker)
                {
                    if (!Loaded)
                    {
                        SetupFolder();
                        try
                        {
                            Cookies.Clear();
                            if (!Directory.Exists(CookieFolder))
                            {
                                Directory.CreateDirectory(CookieFolder);
                            }
                            if (File.Exists(LibraryPath))
                            {
                                using (FileStream stream = new FileStream(LibraryPath, FileMode.Open))
                                {
                                    using (BinaryReader reader = new BinaryReader(stream))
                                    {
                                        reader.ReadInt32();
                                        int num = reader.ReadInt32();
                                        for (int i = 0; i < num; i++)
                                        {
                                            Cookie item = new Cookie();
                                            item.LoadFrom(reader);
                                            if (item.WillExpireInTheFuture())
                                            {
                                                Cookies.Add(item);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        catch
                        {
                            Cookies.Clear();
                        }
                        finally
                        {
                            Loaded = true;
                        }
                    }
                }
            }
        }

        internal static void Maintain()
        {
            object locker = Locker;
            lock (locker)
            {
                try
                {
                    uint num = 0;
                    TimeSpan span = TimeSpan.FromDays(7.0);
                    int index = 0;
                    while (index < Cookies.Count)
                    {
                        Cookie cookie = Cookies[index];
                        if (!cookie.WillExpireInTheFuture() || ((cookie.LastAccess + span) < DateTime.UtcNow))
                        {
                            Cookies.RemoveAt(index);
                        }
                        else
                        {
                            if (!cookie.IsSession)
                            {
                                num += cookie.GuessSize();
                            }
                            index++;
                        }
                    }
                    if (num > HTTPManager.CookieJarSize)
                    {
                        Cookies.Sort();
                        while ((num > HTTPManager.CookieJarSize) && (Cookies.Count > 0))
                        {
                            Cookie cookie2 = Cookies[0];
                            Cookies.RemoveAt(0);
                            num -= cookie2.GuessSize();
                        }
                    }
                }
                catch
                {
                }
            }
        }

        internal static void Persist()
        {
            if (IsSavingSupported)
            {
                object locker = Locker;
                lock (locker)
                {
                    if (Loaded)
                    {
                        try
                        {
                            Maintain();
                            if (!Directory.Exists(CookieFolder))
                            {
                                Directory.CreateDirectory(CookieFolder);
                            }
                            using (FileStream stream = new FileStream(LibraryPath, FileMode.Create))
                            {
                                using (BinaryWriter writer = new BinaryWriter(stream))
                                {
                                    writer.Write(1);
                                    int num = 0;
                                    foreach (Cookie cookie in Cookies)
                                    {
                                        if (!cookie.IsSession)
                                        {
                                            num++;
                                        }
                                    }
                                    writer.Write(num);
                                    foreach (Cookie cookie2 in Cookies)
                                    {
                                        if (!cookie2.IsSession)
                                        {
                                            cookie2.SaveTo(writer);
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
            }
        }

        public static void Remove(Uri uri, string name)
        {
            object locker = Locker;
            lock (locker)
            {
                Load();
                int index = 0;
                while (index < Cookies.Count)
                {
                    Cookie cookie = Cookies[index];
                    if (cookie.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && (uri.Host.IndexOf(cookie.Domain) != -1))
                    {
                        Cookies.RemoveAt(index);
                    }
                    else
                    {
                        index++;
                    }
                }
            }
        }

        public static void Set(Cookie cookie)
        {
            object locker = Locker;
            lock (locker)
            {
                Load();
                Find(cookie, out int num);
                if (num >= 0)
                {
                    Cookies[num] = cookie;
                }
                else
                {
                    Cookies.Add(cookie);
                }
            }
        }

        internal static void Set(HTTPResponse response)
        {
            if (response != null)
            {
                object locker = Locker;
                lock (locker)
                {
                    try
                    {
                        Maintain();
                        List<Cookie> list = new List<Cookie>();
                        List<string> headerValues = response.GetHeaderValues("set-cookie");
                        if (headerValues != null)
                        {
                            foreach (string str in headerValues)
                            {
                                try
                                {
                                    Cookie cookie = Cookie.Parse(str, response.baseRequest.CurrentUri);
                                    if (cookie != null)
                                    {
                                        Cookie cookie2 = Find(cookie, out int num);
                                        if (!string.IsNullOrEmpty(cookie.Value) && cookie.WillExpireInTheFuture())
                                        {
                                            if (cookie2 == null)
                                            {
                                                Cookies.Add(cookie);
                                                list.Add(cookie);
                                            }
                                            else
                                            {
                                                cookie.Date = cookie2.Date;
                                                Cookies[num] = cookie;
                                                list.Add(cookie);
                                            }
                                        }
                                        else if (num != -1)
                                        {
                                            Cookies.RemoveAt(num);
                                        }
                                    }
                                }
                                catch
                                {
                                }
                            }
                            response.Cookies = list;
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

        public static void Set(Uri uri, Cookie cookie)
        {
            Set(cookie);
        }

        internal static void SetupFolder()
        {
            if (IsSavingSupported)
            {
                try
                {
                    if (string.IsNullOrEmpty(CookieFolder) || string.IsNullOrEmpty(LibraryPath))
                    {
                        CookieFolder = Path.Combine(HTTPManager.GetRootCacheFolder(), "Cookies");
                        LibraryPath = Path.Combine(CookieFolder, "Library");
                    }
                }
                catch
                {
                }
            }
        }

        public static bool IsSavingSupported
        {
            get
            {
                if (!IsSupportCheckDone)
                {
                    try
                    {
                        File.Exists(HTTPManager.GetRootCacheFolder());
                        _isSavingSupported = true;
                    }
                    catch
                    {
                        _isSavingSupported = false;
                        HTTPManager.Logger.Warning("CookieJar", "Cookie saving and loading disabled!");
                    }
                    finally
                    {
                        IsSupportCheckDone = true;
                    }
                }
                return _isSavingSupported;
            }
        }

        private static string CookieFolder
        {
            [CompilerGenerated]
            get => 
                <CookieFolder>k__BackingField;
            [CompilerGenerated]
            set => 
                (<CookieFolder>k__BackingField = value);
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
    }
}

