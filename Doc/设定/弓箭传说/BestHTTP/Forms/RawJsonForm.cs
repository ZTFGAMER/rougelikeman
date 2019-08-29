namespace BestHTTP.Forms
{
    using BestHTTP;
    using BestHTTP.JSON;
    using System;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;

    public sealed class RawJsonForm : HTTPFormBase
    {
        private byte[] CachedData;
        [CompilerGenerated]
        private static Func<HTTPFieldData, string> <>f__am$cache0;
        [CompilerGenerated]
        private static Func<HTTPFieldData, string> <>f__am$cache1;

        public override byte[] GetData()
        {
            if ((this.CachedData != null) && !base.IsChanged)
            {
                return this.CachedData;
            }
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = x => x.Name;
            }
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = x => x.Text;
            }
            string s = Json.Encode(base.Fields.ToDictionary<HTTPFieldData, string, string>(<>f__am$cache0, <>f__am$cache1));
            base.IsChanged = false;
            return (this.CachedData = Encoding.UTF8.GetBytes(s));
        }

        public override void PrepareRequest(HTTPRequest request)
        {
            request.SetHeader("Content-Type", "application/json");
        }
    }
}

