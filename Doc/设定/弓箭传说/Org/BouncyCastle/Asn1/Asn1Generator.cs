namespace Org.BouncyCastle.Asn1
{
    using System;
    using System.IO;

    public abstract class Asn1Generator
    {
        private Stream _out;

        protected Asn1Generator(Stream outStream)
        {
            this._out = outStream;
        }

        public abstract void AddObject(Asn1Encodable obj);
        public abstract void Close();
        public abstract Stream GetRawOutputStream();

        protected Stream Out =>
            this._out;
    }
}

