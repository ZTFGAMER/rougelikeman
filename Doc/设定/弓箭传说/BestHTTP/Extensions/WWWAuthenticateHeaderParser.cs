namespace BestHTTP.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public sealed class WWWAuthenticateHeaderParser : KeyValuePairList
    {
        [CompilerGenerated]
        private static Func<char, bool> <>f__am$cache0;

        public WWWAuthenticateHeaderParser(string headerValue)
        {
            base.Values = this.ParseQuotedHeader(headerValue);
        }

        private List<HeaderValue> ParseQuotedHeader(string str)
        {
            List<HeaderValue> list = new List<HeaderValue>();
            if (str != null)
            {
                int pos = 0;
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = ch => !char.IsWhiteSpace(ch) && !char.IsControl(ch);
                }
                string key = str.Read(ref pos, <>f__am$cache0, true).TrimAndLower();
                list.Add(new HeaderValue(key));
                while (pos < str.Length)
                {
                    HeaderValue item = new HeaderValue(str.Read(ref pos, '=', true).TrimAndLower());
                    str.SkipWhiteSpace(ref pos);
                    item.Value = str.ReadPossibleQuotedText(ref pos);
                    list.Add(item);
                }
            }
            return list;
        }
    }
}

