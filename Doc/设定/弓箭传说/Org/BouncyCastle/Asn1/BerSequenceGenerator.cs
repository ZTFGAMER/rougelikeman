namespace Org.BouncyCastle.Asn1
{
    using System;
    using System.IO;

    public class BerSequenceGenerator : BerGenerator
    {
        public BerSequenceGenerator(Stream outStream) : base(outStream)
        {
            base.WriteBerHeader(0x30);
        }

        public BerSequenceGenerator(Stream outStream, int tagNo, bool isExplicit) : base(outStream, tagNo, isExplicit)
        {
            base.WriteBerHeader(0x30);
        }
    }
}

