namespace Org.BouncyCastle.Asn1
{
    using Org.BouncyCastle.Utilities.IO;
    using System;
    using System.IO;

    public abstract class DerGenerator : Asn1Generator
    {
        private bool _tagged;
        private bool _isExplicit;
        private int _tagNo;

        protected DerGenerator(Stream outStream) : base(outStream)
        {
        }

        protected DerGenerator(Stream outStream, int tagNo, bool isExplicit) : base(outStream)
        {
            this._tagged = true;
            this._isExplicit = isExplicit;
            this._tagNo = tagNo;
        }

        internal void WriteDerEncoded(int tag, byte[] bytes)
        {
            if (this._tagged)
            {
                int num = this._tagNo | 0x80;
                if (this._isExplicit)
                {
                    int num2 = (this._tagNo | 0x20) | 0x80;
                    MemoryStream outStream = new MemoryStream();
                    WriteDerEncoded(outStream, tag, bytes);
                    WriteDerEncoded(base.Out, num2, outStream.ToArray());
                }
                else
                {
                    if ((tag & 0x20) != 0)
                    {
                        num |= 0x20;
                    }
                    WriteDerEncoded(base.Out, num, bytes);
                }
            }
            else
            {
                WriteDerEncoded(base.Out, tag, bytes);
            }
        }

        internal static void WriteDerEncoded(Stream outStream, int tag, byte[] bytes)
        {
            outStream.WriteByte((byte) tag);
            WriteLength(outStream, bytes.Length);
            outStream.Write(bytes, 0, bytes.Length);
        }

        internal static void WriteDerEncoded(Stream outStr, int tag, Stream inStr)
        {
            WriteDerEncoded(outStr, tag, Streams.ReadAll(inStr));
        }

        private static void WriteLength(Stream outStr, int length)
        {
            if (length > 0x7f)
            {
                int num = 1;
                int num2 = length;
                while ((num2 = num2 >> 8) != 0)
                {
                    num++;
                }
                outStr.WriteByte((byte) (num | 0x80));
                for (int i = (num - 1) * 8; i >= 0; i -= 8)
                {
                    outStr.WriteByte((byte) (length >> i));
                }
            }
            else
            {
                outStr.WriteByte((byte) length);
            }
        }
    }
}

