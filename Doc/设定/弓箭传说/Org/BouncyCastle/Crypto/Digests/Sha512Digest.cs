namespace Org.BouncyCastle.Crypto.Digests
{
    using Org.BouncyCastle.Crypto.Utilities;
    using Org.BouncyCastle.Utilities;
    using System;

    public class Sha512Digest : LongDigest
    {
        private const int DigestLength = 0x40;

        public Sha512Digest()
        {
        }

        public Sha512Digest(Sha512Digest t) : base(t)
        {
        }

        public override IMemoable Copy() => 
            new Sha512Digest(this);

        public override int DoFinal(byte[] output, int outOff)
        {
            base.Finish();
            Pack.UInt64_To_BE(base.H1, output, outOff);
            Pack.UInt64_To_BE(base.H2, output, outOff + 8);
            Pack.UInt64_To_BE(base.H3, output, outOff + 0x10);
            Pack.UInt64_To_BE(base.H4, output, outOff + 0x18);
            Pack.UInt64_To_BE(base.H5, output, outOff + 0x20);
            Pack.UInt64_To_BE(base.H6, output, outOff + 40);
            Pack.UInt64_To_BE(base.H7, output, outOff + 0x30);
            Pack.UInt64_To_BE(base.H8, output, outOff + 0x38);
            this.Reset();
            return 0x40;
        }

        public override int GetDigestSize() => 
            0x40;

        public override void Reset()
        {
            base.Reset();
            base.H1 = 0x6a09e667f3bcc908L;
            base.H2 = 13_503_953_896_175_478_587L;
            base.H3 = 0x3c6ef372fe94f82bL;
            base.H4 = 11_912_009_170_470_909_681L;
            base.H5 = 0x510e527fade682d1L;
            base.H6 = 11_170_449_401_992_604_703L;
            base.H7 = 0x1f83d9abfb41bd6bL;
            base.H8 = 0x5be0cd19137e2179L;
        }

        public override void Reset(IMemoable other)
        {
            Sha512Digest t = (Sha512Digest) other;
            base.CopyIn(t);
        }

        public override string AlgorithmName =>
            "SHA-512";
    }
}

