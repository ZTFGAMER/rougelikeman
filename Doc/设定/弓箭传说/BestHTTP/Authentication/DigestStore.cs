namespace BestHTTP.Authentication
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    internal static class DigestStore
    {
        private static Dictionary<string, Digest> Digests = new Dictionary<string, Digest>();
        private static object Locker = new object();
        private static string[] SupportedAlgorithms = new string[] { "digest", "basic" };

        public static string FindBest(List<string> authHeaders)
        {
            if ((authHeaders != null) && (authHeaders.Count != 0))
            {
                List<string> list = new List<string>(authHeaders.Count);
                for (int i = 0; i < authHeaders.Count; i++)
                {
                    list.Add(authHeaders[i].ToLower());
                }
                <FindBest>c__AnonStorey0 storey = new <FindBest>c__AnonStorey0 {
                    i = 0
                };
                while (storey.i < SupportedAlgorithms.Length)
                {
                    int num2 = list.FindIndex(new Predicate<string>(storey.<>m__0));
                    if (num2 != -1)
                    {
                        return authHeaders[num2];
                    }
                    storey.i++;
                }
            }
            return string.Empty;
        }

        internal static Digest Get(Uri uri)
        {
            object locker = Locker;
            lock (locker)
            {
                Digest digest = null;
                if (Digests.TryGetValue(uri.Host, out digest) && !digest.IsUriProtected(uri))
                {
                    return null;
                }
                return digest;
            }
        }

        public static Digest GetOrCreate(Uri uri)
        {
            object locker = Locker;
            lock (locker)
            {
                Digest digest = null;
                if (!Digests.TryGetValue(uri.Host, out digest))
                {
                    Digests.Add(uri.Host, digest = new Digest(uri));
                }
                return digest;
            }
        }

        public static void Remove(Uri uri)
        {
            object locker = Locker;
            lock (locker)
            {
                Digests.Remove(uri.Host);
            }
        }

        [CompilerGenerated]
        private sealed class <FindBest>c__AnonStorey0
        {
            internal int i;

            internal bool <>m__0(string header) => 
                header.StartsWith(DigestStore.SupportedAlgorithms[this.i]);
        }
    }
}

