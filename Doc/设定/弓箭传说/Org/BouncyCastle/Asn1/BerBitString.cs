namespace Org.BouncyCastle.Asn1
{
    using System;

    public class BerBitString : DerBitString
    {
        public BerBitString(byte[] data) : base(data)
        {
        }

        public BerBitString(Asn1Encodable obj) : base(obj)
        {
        }

        public BerBitString(int namedBits) : base(namedBits)
        {
        }

        public BerBitString(byte[] data, int padBits) : base(data, padBits)
        {
        }

        internal override void Encode(DerOutputStream derOut)
        {
            if ((derOut is Asn1OutputStream) || (derOut is BerOutputStream))
            {
                derOut.WriteEncoded(3, (byte) base.mPadBits, base.mData);
            }
            else
            {
                base.Encode(derOut);
            }
        }
    }
}

