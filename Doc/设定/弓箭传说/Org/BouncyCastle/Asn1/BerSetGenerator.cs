namespace Org.BouncyCastle.Asn1
{
    using System;
    using System.IO;

    public class BerSetGenerator : BerGenerator
    {
        public BerSetGenerator(Stream outStream) : base(outStream)
        {
            base.WriteBerHeader(0x31);
        }

        public BerSetGenerator(Stream outStream, int tagNo, bool isExplicit) : base(outStream, tagNo, isExplicit)
        {
            base.WriteBerHeader(0x31);
        }
    }
}

