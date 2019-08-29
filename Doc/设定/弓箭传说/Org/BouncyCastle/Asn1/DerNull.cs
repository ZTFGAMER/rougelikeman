namespace Org.BouncyCastle.Asn1
{
    using System;

    public class DerNull : Asn1Null
    {
        public static readonly DerNull Instance = new DerNull(0);
        private byte[] zeroBytes;

        [Obsolete("Use static Instance object")]
        public DerNull()
        {
            this.zeroBytes = new byte[0];
        }

        protected internal DerNull(int dummy)
        {
            this.zeroBytes = new byte[0];
        }

        protected override bool Asn1Equals(Asn1Object asn1Object) => 
            (asn1Object is DerNull);

        protected override int Asn1GetHashCode() => 
            -1;

        internal override void Encode(DerOutputStream derOut)
        {
            derOut.WriteEncoded(5, this.zeroBytes);
        }
    }
}

