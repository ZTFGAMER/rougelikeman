namespace BestHTTP.Extensions
{
    using BestHTTP;
    using System;
    using System.Collections.Generic;

    public sealed class HeaderParser : KeyValuePairList
    {
        public HeaderParser(string headerStr)
        {
            base.Values = this.Parse(headerStr);
        }

        private List<HeaderValue> Parse(string headerStr)
        {
            List<HeaderValue> list = new List<HeaderValue>();
            int pos = 0;
            try
            {
                while (pos < headerStr.Length)
                {
                    HeaderValue item = new HeaderValue();
                    item.Parse(headerStr, ref pos);
                    list.Add(item);
                }
            }
            catch (Exception exception)
            {
                HTTPManager.Logger.Exception("HeaderParser - Parse", headerStr, exception);
            }
            return list;
        }
    }
}

