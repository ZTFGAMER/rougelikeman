namespace Org.BouncyCastle.Asn1
{
    using System;
    using System.IO;

    public class DerSetGenerator : DerGenerator
    {
        private readonly MemoryStream _bOut;

        public DerSetGenerator(Stream outStream) : base(outStream)
        {
            this._bOut = new MemoryStream();
        }

        public DerSetGenerator(Stream outStream, int tagNo, bool isExplicit) : base(outStream, tagNo, isExplicit)
        {
            this._bOut = new MemoryStream();
        }

        public override void AddObject(Asn1Encodable obj)
        {
            new DerOutputStream(this._bOut).WriteObject(obj);
        }

        public override void Close()
        {
            base.WriteDerEncoded(0x31, this._bOut.ToArray());
        }

        public override Stream GetRawOutputStream() => 
            this._bOut;
    }
}

