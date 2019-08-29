namespace BestHTTP.Cookies
{
    using BestHTTP;
    using BestHTTP.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public sealed class Cookie : IComparable<Cookie>, IEquatable<Cookie>
    {
        private const int Version = 1;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Name>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Value>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime <Date>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime <LastAccess>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime <Expires>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private long <MaxAge>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsSession>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Domain>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Path>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsSecure>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsHttpOnly>k__BackingField;
        [CompilerGenerated]
        private static Func<char, bool> <>f__am$cache0;

        internal Cookie()
        {
            this.IsSession = true;
            this.MaxAge = -1L;
            this.LastAccess = DateTime.UtcNow;
        }

        public Cookie(string name, string value) : this(name, value, "/", string.Empty)
        {
        }

        public Cookie(string name, string value, string path) : this(name, value, path, string.Empty)
        {
        }

        public Cookie(string name, string value, string path, string domain) : this()
        {
            this.Name = name;
            this.Value = value;
            this.Path = path;
            this.Domain = domain;
        }

        public Cookie(Uri uri, string name, string value, DateTime expires, bool isSession = true) : this(name, value, uri.AbsolutePath, uri.Host)
        {
            this.Expires = expires;
            this.IsSession = isSession;
            this.Date = DateTime.UtcNow;
        }

        public Cookie(Uri uri, string name, string value, long maxAge = -1L, bool isSession = true) : this(name, value, uri.AbsolutePath, uri.Host)
        {
            this.MaxAge = maxAge;
            this.IsSession = isSession;
            this.Date = DateTime.UtcNow;
        }

        public int CompareTo(Cookie other) => 
            this.LastAccess.CompareTo(other.LastAccess);

        public bool Equals(Cookie cookie)
        {
            if (cookie == null)
            {
                return false;
            }
            return (object.ReferenceEquals(this, cookie) || ((this.Name.Equals(cookie.Name, StringComparison.Ordinal) && (((this.Domain == null) && (cookie.Domain == null)) || this.Domain.Equals(cookie.Domain, StringComparison.Ordinal))) && (((this.Path == null) && (cookie.Path == null)) || this.Path.Equals(cookie.Path, StringComparison.Ordinal))));
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            return this.Equals(obj as Cookie);
        }

        public override int GetHashCode() => 
            this.ToString().GetHashCode();

        public uint GuessSize() => 
            ((uint) (((((((this.Name == null) ? 0 : (this.Name.Length * 2)) + ((this.Value == null) ? 0 : (this.Value.Length * 2))) + ((this.Domain == null) ? 0 : (this.Domain.Length * 2))) + ((this.Path == null) ? 0 : (this.Path.Length * 2))) + 0x20) + 3));

        internal void LoadFrom(BinaryReader stream)
        {
            stream.ReadInt32();
            this.Name = stream.ReadString();
            this.Value = stream.ReadString();
            this.Date = DateTime.FromBinary(stream.ReadInt64());
            this.LastAccess = DateTime.FromBinary(stream.ReadInt64());
            this.Expires = DateTime.FromBinary(stream.ReadInt64());
            this.MaxAge = stream.ReadInt64();
            this.IsSession = stream.ReadBoolean();
            this.Domain = stream.ReadString();
            this.Path = stream.ReadString();
            this.IsSecure = stream.ReadBoolean();
            this.IsHttpOnly = stream.ReadBoolean();
        }

        public static Cookie Parse(string header, Uri defaultDomain)
        {
            Cookie cookie = new Cookie();
            try
            {
                foreach (HeaderValue value2 in ParseCookieHeader(header))
                {
                    switch (value2.Key.ToLowerInvariant())
                    {
                        case "path":
                        {
                            cookie.Path = (!string.IsNullOrEmpty(value2.Value) && value2.Value.StartsWith("/")) ? value2.Value : "/";
                            continue;
                        }
                        case "domain":
                        {
                            if (string.IsNullOrEmpty(value2.Value))
                            {
                                return null;
                            }
                            cookie.Domain = !value2.Value.StartsWith(".") ? value2.Value : value2.Value.Substring(1);
                            continue;
                        }
                        case "expires":
                        {
                            cookie.Expires = value2.Value.ToDateTime(DateTime.FromBinary(0L));
                            cookie.IsSession = false;
                            continue;
                        }
                        case "max-age":
                        {
                            cookie.MaxAge = value2.Value.ToInt64(-1L);
                            cookie.IsSession = false;
                            continue;
                        }
                        case "secure":
                        {
                            cookie.IsSecure = true;
                            continue;
                        }
                        case "httponly":
                        {
                            cookie.IsHttpOnly = true;
                            continue;
                        }
                    }
                    cookie.Name = value2.Key;
                    cookie.Value = value2.Value;
                }
                if (HTTPManager.EnablePrivateBrowsing)
                {
                    cookie.IsSession = true;
                }
                if (string.IsNullOrEmpty(cookie.Domain))
                {
                    cookie.Domain = defaultDomain.Host;
                }
                if (string.IsNullOrEmpty(cookie.Path))
                {
                    cookie.Path = defaultDomain.AbsolutePath;
                }
                DateTime utcNow = DateTime.UtcNow;
                cookie.LastAccess = utcNow;
                cookie.Date = utcNow;
            }
            catch
            {
            }
            return cookie;
        }

        private static List<HeaderValue> ParseCookieHeader(string str)
        {
            List<HeaderValue> list = new List<HeaderValue>();
            if (str != null)
            {
                int pos = 0;
                while (pos < str.Length)
                {
                    if (<>f__am$cache0 == null)
                    {
                        <>f__am$cache0 = ch => (ch != '=') && (ch != ';');
                    }
                    HeaderValue item = new HeaderValue(str.Read(ref pos, <>f__am$cache0, true).Trim());
                    if ((pos < str.Length) && (str[pos - 1] == '='))
                    {
                        item.Value = ReadValue(str, ref pos);
                    }
                    list.Add(item);
                }
            }
            return list;
        }

        private static string ReadValue(string str, ref int pos)
        {
            string str2 = string.Empty;
            if (str == null)
            {
                return str2;
            }
            return str.Read(ref pos, ';', true);
        }

        internal void SaveTo(BinaryWriter stream)
        {
            stream.Write(1);
            if (this.Name == null)
            {
            }
            stream.Write(string.Empty);
            if (this.Value == null)
            {
            }
            stream.Write(string.Empty);
            stream.Write(this.Date.ToBinary());
            stream.Write(this.LastAccess.ToBinary());
            stream.Write(this.Expires.ToBinary());
            stream.Write(this.MaxAge);
            stream.Write(this.IsSession);
            if (this.Domain == null)
            {
            }
            stream.Write(string.Empty);
            if (this.Path == null)
            {
            }
            stream.Write(string.Empty);
            stream.Write(this.IsSecure);
            stream.Write(this.IsHttpOnly);
        }

        public override string ToString() => 
            (this.Name + "=" + this.Value);

        public bool WillExpireInTheFuture() => 
            (this.IsSession || ((this.MaxAge == -1L) ? (this.Expires > DateTime.UtcNow) : (Math.Max(0L, (long) (DateTime.UtcNow - this.Date).TotalSeconds) < this.MaxAge)));

        public string Name { get; private set; }

        public string Value { get; private set; }

        public DateTime Date { get; internal set; }

        public DateTime LastAccess { get; set; }

        public DateTime Expires { get; private set; }

        public long MaxAge { get; private set; }

        public bool IsSession { get; private set; }

        public string Domain { get; private set; }

        public string Path { get; private set; }

        public bool IsSecure { get; private set; }

        public bool IsHttpOnly { get; private set; }
    }
}

