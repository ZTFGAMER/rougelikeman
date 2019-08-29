namespace Org.BouncyCastle.Crypto.Macs
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Utilities;
    using System;

    public class HMac : IMac
    {
        private const byte IPAD = 0x36;
        private const byte OPAD = 0x5c;
        private readonly IDigest digest;
        private readonly int digestSize;
        private readonly int blockLength;
        private IMemoable ipadState;
        private IMemoable opadState;
        private readonly byte[] inputPad;
        private readonly byte[] outputBuf;

        public HMac(IDigest digest)
        {
            this.digest = digest;
            this.digestSize = digest.GetDigestSize();
            this.blockLength = digest.GetByteLength();
            this.inputPad = new byte[this.blockLength];
            this.outputBuf = new byte[this.blockLength + this.digestSize];
        }

        public virtual void BlockUpdate(byte[] input, int inOff, int len)
        {
            this.digest.BlockUpdate(input, inOff, len);
        }

        public virtual int DoFinal(byte[] output, int outOff)
        {
            this.digest.DoFinal(this.outputBuf, this.blockLength);
            if (this.opadState != null)
            {
                ((IMemoable) this.digest).Reset(this.opadState);
                this.digest.BlockUpdate(this.outputBuf, this.blockLength, this.digest.GetDigestSize());
            }
            else
            {
                this.digest.BlockUpdate(this.outputBuf, 0, this.outputBuf.Length);
            }
            int num = this.digest.DoFinal(output, outOff);
            Array.Clear(this.outputBuf, this.blockLength, this.digestSize);
            if (this.ipadState != null)
            {
                ((IMemoable) this.digest).Reset(this.ipadState);
                return num;
            }
            this.digest.BlockUpdate(this.inputPad, 0, this.inputPad.Length);
            return num;
        }

        public virtual int GetMacSize() => 
            this.digestSize;

        public virtual IDigest GetUnderlyingDigest() => 
            this.digest;

        public virtual void Init(ICipherParameters parameters)
        {
            this.digest.Reset();
            byte[] key = ((KeyParameter) parameters).GetKey();
            int length = key.Length;
            if (length > this.blockLength)
            {
                this.digest.BlockUpdate(key, 0, length);
                this.digest.DoFinal(this.inputPad, 0);
                length = this.digestSize;
            }
            else
            {
                Array.Copy(key, 0, this.inputPad, 0, length);
            }
            Array.Clear(this.inputPad, length, this.blockLength - length);
            Array.Copy(this.inputPad, 0, this.outputBuf, 0, this.blockLength);
            XorPad(this.inputPad, this.blockLength, 0x36);
            XorPad(this.outputBuf, this.blockLength, 0x5c);
            if (this.digest is IMemoable)
            {
                this.opadState = ((IMemoable) this.digest).Copy();
                ((IDigest) this.opadState).BlockUpdate(this.outputBuf, 0, this.blockLength);
            }
            this.digest.BlockUpdate(this.inputPad, 0, this.inputPad.Length);
            if (this.digest is IMemoable)
            {
                this.ipadState = ((IMemoable) this.digest).Copy();
            }
        }

        public virtual void Reset()
        {
            this.digest.Reset();
            this.digest.BlockUpdate(this.inputPad, 0, this.inputPad.Length);
        }

        public virtual void Update(byte input)
        {
            this.digest.Update(input);
        }

        private static void XorPad(byte[] pad, int len, byte n)
        {
            for (int i = 0; i < len; i++)
            {
                pad[i] = (byte) (pad[i] ^ n);
            }
        }

        public virtual string AlgorithmName =>
            (this.digest.AlgorithmName + "/HMAC");
    }
}

