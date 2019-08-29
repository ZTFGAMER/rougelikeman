namespace BestHTTP.Forms
{
    using BestHTTP;
    using System;
    using System.Text;

    public sealed class HTTPUrlEncodedForm : HTTPFormBase
    {
        private const int EscapeTreshold = 0x100;
        private byte[] CachedData;

        public static string EscapeString(string originalString)
        {
            if (originalString.Length < 0x100)
            {
                return Uri.EscapeDataString(originalString);
            }
            int capacity = originalString.Length / 0x100;
            StringBuilder builder = new StringBuilder(capacity);
            for (int i = 0; i <= capacity; i++)
            {
                builder.Append((i >= capacity) ? Uri.EscapeDataString(originalString.Substring(0x100 * i)) : Uri.EscapeDataString(originalString.Substring(0x100 * i, 0x100)));
            }
            return builder.ToString();
        }

        public override byte[] GetData()
        {
            if ((this.CachedData != null) && !base.IsChanged)
            {
                return this.CachedData;
            }
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < base.Fields.Count; i++)
            {
                HTTPFieldData data = base.Fields[i];
                if (i > 0)
                {
                    builder.Append("&");
                }
                builder.Append(EscapeString(data.Name));
                builder.Append("=");
                if (!string.IsNullOrEmpty(data.Text) || (data.Binary == null))
                {
                    builder.Append(EscapeString(data.Text));
                }
                else
                {
                    builder.Append(EscapeString(Encoding.UTF8.GetString(data.Binary, 0, data.Binary.Length)));
                }
            }
            base.IsChanged = false;
            return (this.CachedData = Encoding.UTF8.GetBytes(builder.ToString()));
        }

        public override void PrepareRequest(HTTPRequest request)
        {
            request.SetHeader("Content-Type", "application/x-www-form-urlencoded");
        }
    }
}

