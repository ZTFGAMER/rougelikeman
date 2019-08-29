namespace Org.BouncyCastle.Crypto.Digests
{
    using Org.BouncyCastle.Crypto.Utilities;
    using Org.BouncyCastle.Utilities;
    using System;

    public class Sha384Digest : LongDigest
    {
        private const int DigestLength = 0x30;

        public Sha384Digest()
        {
        }

        public Sha384Digest(Sha384Digest t) : base(t)
        {
        }

        public override IMemoable Copy() => 
            new Sha384Digest(this);

        public override int DoFinal(byte[] output, int outOff)
        {
            base.Finish();
            Pack.UInt64_To_BE(base.H1, output, outOff);
            Pack.UInt64_To_BE(base.H2, output, outOff + 8);
            Pack.UInt64_To_BE(base.H3, output, outOff + 0x10);
            Pack.UInt64_To_BE(base.H4, output, outOff + 0x18);
            Pack.UInt64_To_BE(base.H5, output, outOff + 0x20);
            Pack.UInt64_To_BE(base.H6, output, outOff + 40);
            this.Reset();
            return 0x30;
        }

        public override int GetDigestSize() => 
            0x30;

        public override void Reset()
        {
            base.Reset();
            base.H1 = 14_680_500_436_340_154_072L;
            base.H2 = 0x629a292a367cd507L;
            base.H3 = 10_473_403_895_298_186_519L;
            base.H4 = 0x152fecd8f70e5939L;
            base.H5 = 0x67332667ffc00b31L;
            base.H6 = 10_282_925_794_625_328_401L;
            base.H7 = 15_784_041_429_090_275_239L;
            base.H8 = 0x47b5481dbefa4fa4L;
        }

        public override void Reset(IMemoable other)
        {
            Sha384Digest t = (Sha384Digest) other;
            base.CopyIn(t);
        }

        public override string AlgorithmName =>
            "SHA-384";
    }
}

