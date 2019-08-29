namespace Org.BouncyCastle.Asn1
{
    using Org.BouncyCastle.Utilities.IO;
    using System;
    using System.IO;

    public class BerGenerator : Asn1Generator
    {
        private bool _tagged;
        private bool _isExplicit;
        private int _tagNo;

        protected BerGenerator(Stream outStream) : base(outStream)
        {
        }

        public BerGenerator(Stream outStream, int tagNo, bool isExplicit) : base(outStream)
        {
            this._tagged = true;
            this._isExplicit = isExplicit;
            this._tagNo = tagNo;
        }

        public override void AddObject(Asn1Encodable obj)
        {
            new BerOutputStream(base.Out).WriteObject(obj);
        }

        public override void Close()
        {
            this.WriteBerEnd();
        }

        public override Stream GetRawOutputStream() => 
            base.Out;

        protected void WriteBerBody(Stream contentStream)
        {
            Streams.PipeAll(contentStream, base.Out);
        }

        protected void WriteBerEnd()
        {
            base.Out.WriteByte(0);
            base.Out.WriteByte(0);
            if (this._tagged && this._isExplicit)
            {
                base.Out.WriteByte(0);
                base.Out.WriteByte(0);
            }
        }

        protected void WriteBerHeader(int tag)
        {
            if (this._tagged)
            {
                int num = this._tagNo | 0x80;
                if (this._isExplicit)
                {
                    this.WriteHdr(num | 0x20);
                    this.WriteHdr(tag);
                }
                else if ((tag & 0x20) != 0)
                {
                    this.WriteHdr(num | 0x20);
                }
                else
                {
                    this.WriteHdr(num);
                }
            }
            else
            {
                this.WriteHdr(tag);
            }
        }

        private void WriteHdr(int tag)
        {
            base.Out.WriteByte((byte) tag);
            base.Out.WriteByte(0x80);
        }
    }
}

