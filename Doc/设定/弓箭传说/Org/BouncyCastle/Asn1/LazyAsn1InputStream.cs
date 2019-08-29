namespace Org.BouncyCastle.Asn1
{
    using System;
    using System.IO;

    public class LazyAsn1InputStream : Asn1InputStream
    {
        public LazyAsn1InputStream(byte[] input) : base(input)
        {
        }

        public LazyAsn1InputStream(Stream inputStream) : base(inputStream)
        {
        }

        internal override DerSequence CreateDerSequence(DefiniteLengthInputStream dIn) => 
            new LazyDerSequence(dIn.ToArray());

        internal override DerSet CreateDerSet(DefiniteLengthInputStream dIn) => 
            new LazyDerSet(dIn.ToArray());
    }
}

