namespace Org.BouncyCastle.Crypto.Digests
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Utilities;
    using System;

    public class ShakeDigest : KeccakDigest, IXof, IDigest
    {
        public ShakeDigest() : this(0x80)
        {
        }

        public ShakeDigest(ShakeDigest source) : base(source)
        {
        }

        public ShakeDigest(int bitLength) : base(CheckBitLength(bitLength))
        {
        }

        private static int CheckBitLength(int bitLength)
        {
            if ((bitLength != 0x80) && (bitLength != 0x100))
            {
                throw new ArgumentException(bitLength + " not supported for SHAKE", "bitLength");
            }
            return bitLength;
        }

        public override IMemoable Copy() => 
            new ShakeDigest(this);

        public override int DoFinal(byte[] output, int outOff) => 
            this.DoFinal(output, outOff, this.GetDigestSize());

        public virtual int DoFinal(byte[] output, int outOff, int outLen)
        {
            this.DoOutput(output, outOff, outLen);
            this.Reset();
            return outLen;
        }

        protected override int DoFinal(byte[] output, int outOff, byte partialByte, int partialBits) => 
            this.DoFinal(output, outOff, this.GetDigestSize(), partialByte, partialBits);

        protected virtual int DoFinal(byte[] output, int outOff, int outLen, byte partialByte, int partialBits)
        {
            if ((partialBits < 0) || (partialBits > 7))
            {
                throw new ArgumentException("must be in the range [0,7]", "partialBits");
            }
            int num = (partialByte & ((((int) 1) << partialBits) - 1)) | (((int) 15) << partialBits);
            int num2 = partialBits + 4;
            if (num2 >= 8)
            {
                base.oneByte[0] = (byte) num;
                this.Absorb(base.oneByte, 0, 8L);
                num2 -= 8;
                num = num >> 8;
            }
            if (num2 > 0)
            {
                base.oneByte[0] = (byte) num;
                this.Absorb(base.oneByte, 0, (long) num2);
            }
            this.Squeeze(output, outOff, outLen * 8L);
            this.Reset();
            return outLen;
        }

        public virtual int DoOutput(byte[] output, int outOff, int outLen)
        {
            if (!base.squeezing)
            {
                byte[] data = new byte[] { 15 };
                this.Absorb(data, 0, 4L);
            }
            this.Squeeze(output, outOff, outLen * 8L);
            return outLen;
        }

        public override string AlgorithmName =>
            ("SHAKE" + base.fixedOutputLength);
    }
}

