namespace Org.BouncyCastle.Utilities.IO.Pem
{
    using Org.BouncyCastle.Utilities;
    using Org.BouncyCastle.Utilities.Encoders;
    using System;
    using System.Collections;
    using System.IO;
    using System.Text;

    public class PemReader
    {
        private const string BeginString = "-----BEGIN ";
        private const string EndString = "-----END ";
        private readonly TextReader reader;

        public PemReader(TextReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }
            this.reader = reader;
        }

        private PemObject LoadObject(string type)
        {
            string str2;
            string str = "-----END " + type;
            IList headers = Platform.CreateArrayList();
            StringBuilder builder = new StringBuilder();
            while (((str2 = this.reader.ReadLine()) != null) && (Platform.IndexOf(str2, str) == -1))
            {
                int index = str2.IndexOf(':');
                if (index == -1)
                {
                    builder.Append(str2.Trim());
                }
                else
                {
                    string source = str2.Substring(0, index).Trim();
                    if (Platform.StartsWith(source, "X-"))
                    {
                        source = source.Substring(2);
                    }
                    string val = str2.Substring(index + 1).Trim();
                    headers.Add(new PemHeader(source, val));
                }
            }
            if (str2 == null)
            {
                throw new IOException(str + " not found");
            }
            if ((builder.Length % 4) != 0)
            {
                throw new IOException("base64 data appears to be truncated");
            }
            return new PemObject(type, headers, Base64.Decode(builder.ToString()));
        }

        public PemObject ReadPemObject()
        {
            string source = this.reader.ReadLine();
            if ((source != null) && Platform.StartsWith(source, "-----BEGIN "))
            {
                source = source.Substring("-----BEGIN ".Length);
                int index = source.IndexOf('-');
                string type = source.Substring(0, index);
                if (index > 0)
                {
                    return this.LoadObject(type);
                }
            }
            return null;
        }

        public TextReader Reader =>
            this.reader;
    }
}

