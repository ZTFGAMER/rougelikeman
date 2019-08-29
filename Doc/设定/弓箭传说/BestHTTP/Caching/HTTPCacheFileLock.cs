namespace BestHTTP.Caching
{
    using System;
    using System.Collections.Generic;

    internal sealed class HTTPCacheFileLock
    {
        private static Dictionary<Uri, object> FileLocks = new Dictionary<Uri, object>();
        private static object SyncRoot = new object();

        internal static object Acquire(Uri uri)
        {
            object syncRoot = SyncRoot;
            lock (syncRoot)
            {
                if (!FileLocks.TryGetValue(uri, out object obj3))
                {
                    FileLocks.Add(uri, obj3 = new object());
                }
                return obj3;
            }
        }

        internal static void Clear()
        {
            object syncRoot = SyncRoot;
            lock (syncRoot)
            {
                FileLocks.Clear();
            }
        }

        internal static void Remove(Uri uri)
        {
            object syncRoot = SyncRoot;
            lock (syncRoot)
            {
                if (FileLocks.ContainsKey(uri))
                {
                    FileLocks.Remove(uri);
                }
            }
        }
    }
}

