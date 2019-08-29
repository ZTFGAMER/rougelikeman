namespace BestHTTP.Authentication
{
    using BestHTTP;
    using BestHTTP.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Text;

    internal sealed class Digest
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private System.Uri <Uri>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AuthenticationTypes <Type>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Realm>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <Stale>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Nonce>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Opaque>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Algorithm>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<string> <ProtectedUris>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <QualityOfProtections>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <NonceCount>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <HA1Sess>k__BackingField;
        [CompilerGenerated]
        private static Dictionary<string, int> <>f__switch$map0;

        internal Digest(System.Uri uri)
        {
            this.Uri = uri;
            this.Algorithm = "md5";
        }

        public string GenerateResponseHeader(HTTPRequest request, Credentials credentials)
        {
            try
            {
                AuthenticationTypes type = this.Type;
                if (type != AuthenticationTypes.Basic)
                {
                    if (type == AuthenticationTypes.Digest)
                    {
                        goto Label_004F;
                    }
                    goto Label_03D9;
                }
                return ("Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes($"{credentials.UserName}:{credentials.Password}")));
            Label_004F:
                this.NonceCount++;
                string str2 = string.Empty;
                string str3 = new Random(request.GetHashCode()).Next(-2147483648, 0x7fffffff).ToString("X8");
                string str4 = this.NonceCount.ToString("X8");
                switch (this.Algorithm.TrimAndLower())
                {
                    case "md5":
                        str2 = $"{credentials.UserName}:{this.Realm}:{credentials.Password}".CalculateMD5Hash();
                        break;

                    case "md5-sess":
                        if (string.IsNullOrEmpty(this.HA1Sess))
                        {
                            this.HA1Sess = $"{credentials.UserName}:{this.Realm}:{credentials.Password}:{this.Nonce}:{str4}".CalculateMD5Hash();
                        }
                        str2 = this.HA1Sess;
                        break;

                    default:
                        return string.Empty;
                }
                string str6 = string.Empty;
                string str7 = (this.QualityOfProtections == null) ? null : this.QualityOfProtections.TrimAndLower();
                if (str7 == null)
                {
                    string str8 = (request.MethodType.ToString().ToUpper() + ":" + request.CurrentUri.GetRequestPathAndQueryURL()).CalculateMD5Hash();
                    str6 = $"{str2}:{this.Nonce}:{str8}".CalculateMD5Hash();
                }
                else if (str7.Contains("auth-int"))
                {
                    str7 = "auth-int";
                    byte[] entityBody = request.GetEntityBody();
                    if (entityBody == null)
                    {
                        entityBody = string.Empty.GetASCIIBytes();
                    }
                    string str9 = $"{request.MethodType.ToString().ToUpper()}:{request.CurrentUri.GetRequestPathAndQueryURL()}:{entityBody.CalculateMD5Hash()}".CalculateMD5Hash();
                    str6 = $"{str2}:{this.Nonce}:{str4}:{str3}:{str7}:{str9}".CalculateMD5Hash();
                }
                else if (str7.Contains("auth"))
                {
                    str7 = "auth";
                    string str10 = (request.MethodType.ToString().ToUpper() + ":" + request.CurrentUri.GetRequestPathAndQueryURL()).CalculateMD5Hash();
                    str6 = $"{str2}:{this.Nonce}:{str4}:{str3}:{str7}:{str10}".CalculateMD5Hash();
                }
                else
                {
                    return string.Empty;
                }
                string str11 = $"Digest username="{credentials.UserName}", realm="{this.Realm}", nonce="{this.Nonce}", uri="{request.Uri.GetRequestPathAndQueryURL()}", cnonce="{str3}", response="{str6}"";
                if (str7 != null)
                {
                    str11 = str11 + (", qop=\"" + str7 + "\", nc=" + str4);
                }
                if (!string.IsNullOrEmpty(this.Opaque))
                {
                    str11 = str11 + ", opaque=\"" + this.Opaque + "\"";
                }
                return str11;
            }
            catch
            {
            }
        Label_03D9:
            return string.Empty;
        }

        public bool IsUriProtected(System.Uri uri)
        {
            if (string.CompareOrdinal(uri.Host, this.Uri.Host) != 0)
            {
                return false;
            }
            string str = uri.ToString();
            if ((this.ProtectedUris != null) && (this.ProtectedUris.Count > 0))
            {
                for (int i = 0; i < this.ProtectedUris.Count; i++)
                {
                    if (str.Contains(this.ProtectedUris[i]))
                    {
                        return true;
                    }
                }
            }
            return true;
        }

        public void ParseChallange(string header)
        {
            this.Type = AuthenticationTypes.Unknown;
            this.Stale = false;
            this.Opaque = null;
            this.HA1Sess = null;
            this.NonceCount = 0;
            this.QualityOfProtections = null;
            if (this.ProtectedUris != null)
            {
                this.ProtectedUris.Clear();
            }
            WWWAuthenticateHeaderParser parser = new WWWAuthenticateHeaderParser(header);
            foreach (HeaderValue value2 in parser.Values)
            {
                string key = value2.Key;
                if (key != null)
                {
                    if (<>f__switch$map0 == null)
                    {
                        Dictionary<string, int> dictionary = new Dictionary<string, int>(9) {
                            { 
                                "basic",
                                0
                            },
                            { 
                                "digest",
                                1
                            },
                            { 
                                "realm",
                                2
                            },
                            { 
                                "domain",
                                3
                            },
                            { 
                                "nonce",
                                4
                            },
                            { 
                                "qop",
                                5
                            },
                            { 
                                "stale",
                                6
                            },
                            { 
                                "opaque",
                                7
                            },
                            { 
                                "algorithm",
                                8
                            }
                        };
                        <>f__switch$map0 = dictionary;
                    }
                    if (<>f__switch$map0.TryGetValue(key, out int num))
                    {
                        switch (num)
                        {
                            case 0:
                                this.Type = AuthenticationTypes.Basic;
                                break;

                            case 1:
                                this.Type = AuthenticationTypes.Digest;
                                break;

                            case 2:
                                this.Realm = value2.Value;
                                break;

                            case 3:
                                goto Label_0167;

                            case 4:
                                this.Nonce = value2.Value;
                                break;

                            case 5:
                                this.QualityOfProtections = value2.Value;
                                break;

                            case 6:
                                this.Stale = bool.Parse(value2.Value);
                                break;

                            case 7:
                                this.Opaque = value2.Value;
                                break;

                            case 8:
                                this.Algorithm = value2.Value;
                                break;
                        }
                    }
                }
                continue;
            Label_0167:
                if (!string.IsNullOrEmpty(value2.Value) && (value2.Value.Length != 0))
                {
                    if (this.ProtectedUris == null)
                    {
                        this.ProtectedUris = new List<string>();
                    }
                    int pos = 0;
                    string item = value2.Value.Read(ref pos, ' ', true);
                    do
                    {
                        this.ProtectedUris.Add(item);
                        item = value2.Value.Read(ref pos, ' ', true);
                    }
                    while (pos < value2.Value.Length);
                }
            }
        }

        public System.Uri Uri { get; private set; }

        public AuthenticationTypes Type { get; private set; }

        public string Realm { get; private set; }

        public bool Stale { get; private set; }

        private string Nonce { get; set; }

        private string Opaque { get; set; }

        private string Algorithm { get; set; }

        public List<string> ProtectedUris { get; private set; }

        private string QualityOfProtections { get; set; }

        private int NonceCount { get; set; }

        private string HA1Sess { get; set; }
    }
}

