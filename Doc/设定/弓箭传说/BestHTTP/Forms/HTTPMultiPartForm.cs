namespace BestHTTP.Forms
{
    using BestHTTP;
    using BestHTTP.Extensions;
    using System;
    using System.IO;

    public sealed class HTTPMultiPartForm : HTTPFormBase
    {
        private string Boundary;
        private byte[] CachedData;

        public HTTPMultiPartForm()
        {
            this.Boundary = this.GetHashCode().ToString("X");
        }

        public override byte[] GetData()
        {
            if (this.CachedData != null)
            {
                return this.CachedData;
            }
            using (MemoryStream stream = new MemoryStream())
            {
                for (int i = 0; i < base.Fields.Count; i++)
                {
                    HTTPFieldData data = base.Fields[i];
                    stream.WriteLine("--" + this.Boundary);
                    stream.WriteLine("Content-Disposition: form-data; name=\"" + data.Name + "\"" + (string.IsNullOrEmpty(data.FileName) ? string.Empty : ("; filename=\"" + data.FileName + "\"")));
                    if (!string.IsNullOrEmpty(data.MimeType))
                    {
                        stream.WriteLine("Content-Type: " + data.MimeType);
                    }
                    stream.WriteLine("Content-Length: " + data.Payload.Length.ToString());
                    stream.WriteLine();
                    stream.Write(data.Payload, 0, data.Payload.Length);
                    stream.Write(HTTPRequest.EOL, 0, HTTPRequest.EOL.Length);
                }
                stream.WriteLine("--" + this.Boundary + "--");
                base.IsChanged = false;
                return (this.CachedData = stream.ToArray());
            }
        }

        public override void PrepareRequest(HTTPRequest request)
        {
            request.SetHeader("Content-Type", "multipart/form-data; boundary=\"" + this.Boundary + "\"");
        }
    }
}

