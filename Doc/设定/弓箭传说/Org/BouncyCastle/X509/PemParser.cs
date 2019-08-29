namespace Org.BouncyCastle.X509
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Utilities;
    using Org.BouncyCastle.Utilities.Encoders;
    using System;
    using System.IO;
    using System.Text;

    internal class PemParser
    {
        private readonly string _header1;
        private readonly string _header2;
        private readonly string _footer1;
        private readonly string _footer2;

        internal PemParser(string type)
        {
            this._header1 = "-----BEGIN " + type + "-----";
            this._header2 = "-----BEGIN X509 " + type + "-----";
            this._footer1 = "-----END " + type + "-----";
            this._footer2 = "-----END X509 " + type + "-----";
        }

        private string ReadLine(Stream inStream)
        {
            int num;
            StringBuilder builder = new StringBuilder();
            do
            {
                while ((((num = inStream.ReadByte()) != 13) && (num != 10)) && (num >= 0))
                {
                    if (num != 13)
                    {
                        builder.Append((char) num);
                    }
                }
            }
            while ((num >= 0) && (builder.Length == 0));
            if (num < 0)
            {
                return null;
            }
            return builder.ToString();
        }

        internal Asn1Sequence ReadPemObject(Stream inStream)
        {
            string str;
            StringBuilder builder = new StringBuilder();
            while ((str = this.ReadLine(inStream)) != null)
            {
                if (Platform.StartsWith(str, this._header1) || Platform.StartsWith(str, this._header2))
                {
                    break;
                }
            }
            while ((str = this.ReadLine(inStream)) != null)
            {
                if (Platform.StartsWith(str, this._footer1) || Platform.StartsWith(str, this._footer2))
                {
                    break;
                }
                builder.Append(str);
            }
            if (builder.Length == 0)
            {
                return null;
            }
            Asn1Object obj2 = Asn1Object.FromByteArray(Base64.Decode(builder.ToString()));
            if (!(obj2 is Asn1Sequence))
            {
                throw new IOException("malformed PEM data encountered");
            }
            return (Asn1Sequence) obj2;
        }
    }
}

