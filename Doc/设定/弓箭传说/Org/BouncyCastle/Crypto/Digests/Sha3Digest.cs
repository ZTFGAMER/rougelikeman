namespace Org.BouncyCastle.Crypto.Digests
{
    using Org.BouncyCastle.Utilities;
    using System;

    public class Sha3Digest : KeccakDigest
    {
        public Sha3Digest() : this(0x100)
        {
        }

        public Sha3Digest(Sha3Digest source) : base(source)
        {
        }

        public Sha3Digest(int bitLength) : base(CheckBitLength(bitLength))
        {
        }

        private static int CheckBitLength(int bitLength)
        {
            if (((bitLength != 0xe0) && (bitLength != 0x100)) && ((bitLength != 0x180) && (bitLength != 0x200)))
            {
                throw new ArgumentException(bitLength + " not supported for SHA-3", "bitLength");
            }
            return bitLength;
        }

        public override IMemoable Copy() => 
            new Sha3Digest(this);

        public override int DoFinal(byte[] output, int outOff)
        {
            byte[] data = new byte[] { 2 };
            this.Absorb(data, 0, 2L);
            return base.DoFinal(output, outOff);
        }

        protected override int DoFinal(byte[] output, int outOff, byte partialByte, int partialBits)
        {
            if ((partialBits < 0) || (partialBits > 7))
            {
                throw new ArgumentException("must be in the range [0,7]", "partialBits");
            }
            int num = (partialByte & ((((int) 1) << partialBits) - 1)) | (((int) 2) << partialBits);
            int num2 = partialBits + 2;
            if (num2 >= 8)
            {
                base.oneByte[0] = (byte) num;
                this.Absorb(base.oneByte, 0, 8L);
                num2 -= 8;
                num = num >> 8;
            }
            return base.DoFinal(output, outOff, (byte) num, num2);
        }

        public override string AlgorithmName =>
            ("SHA3-" + base.fixedOutputLength);
    }
}

