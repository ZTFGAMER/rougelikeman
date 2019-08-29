namespace Dxx.Net
{
    using System;
    using System.Collections.Generic;

    public class NetConfig
    {
        public const string HttpPath = "https://api-archero.habby.mobi:12020";
        public const string HttpIAPPath = "https://api-archero.habby.mobi:12020/IAP_Verification";
        public static Dictionary<int, string> mIPs;
        public const ushort NetVersion = 1;
        public const ushort AppVersion = 2;
        public const string AppVersionName_Android = "1.0.3";
        public const string AppVersionName_IOS = "1.0.3";

        static NetConfig()
        {
            Dictionary<int, string> dictionary = new Dictionary<int, string> {
                { 
                    0,
                    "35.181.18.186"
                },
                { 
                    1,
                    "18.138.14.129"
                }
            };
            mIPs = dictionary;
        }

        public static string GetIP(int random)
        {
            string str = string.Empty;
            if (!mIPs.TryGetValue(random, out str))
            {
                return mIPs[0];
            }
            return str;
        }

        public static string GetPath(ushort sendcode, string ip)
        {
            string str = "https://api-archero.habby.mobi:12020";
            if (sendcode == 15)
            {
                str = "https://api-archero.habby.mobi:12020/IAP_Verification";
            }
            if (!string.IsNullOrEmpty(ip))
            {
                return str.Replace("api-archero.habby.mobi", ip);
            }
            return "https://api-archero.habby.mobi:12020";
        }

        public static string RandomIP() => 
            GetIP(GameLogic.Random(0, 2));
    }
}

