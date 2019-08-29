namespace Org.BouncyCastle.Crypto.Tls
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Utilities;
    using System;

    public class Ssl3Mac : IMac
    {
        private const byte IPAD_BYTE = 0x36;
        private const byte OPAD_BYTE = 0x5c;
        internal static readonly byte[] IPAD = GenPad(0x36, 0x30);
        internal static readonly byte[] OPAD = GenPad(0x5c, 0x30);
        private readonly IDigest digest;
        private readonly int padLength;
        private byte[] secret;

        public Ssl3Mac(IDigest digest)
        {
            this.digest = digest;
            if (digest.GetDigestSize() == 20)
            {
                this.padLength = 40;
            }
            else
            {
                this.padLength = 0x30;
            }
        }

        public virtual void BlockUpdate(byte[] input, int inOff, int len)
        {
            this.digest.BlockUpdate(input, inOff, len);
        }

        public virtual int DoFinal(byte[] output, int outOff)
        {
            byte[] buffer = new byte[this.digest.GetDigestSize()];
            this.digest.DoFinal(buffer, 0);
            this.digest.BlockUpdate(this.secret, 0, this.secret.Length);
            this.digest.BlockUpdate(OPAD, 0, this.padLength);
            this.digest.BlockUpdate(buffer, 0, buffer.Length);
            int num = this.digest.DoFinal(output, outOff);
            this.Reset();
            return num;
        }

        private static byte[] GenPad(byte b, int count)
        {
            byte[] buf = new byte[count];
            Arrays.Fill(buf, b);
            return buf;
        }

        public virtual int GetMacSize() => 
            this.digest.GetDigestSize();

        public virtual void Init(ICipherParameters parameters)
        {
            this.secret = Arrays.Clone(((KeyParameter) parameters).GetKey());
            this.Reset();
        }

        public virtual void Reset()
        {
            this.digest.Reset();
            this.digest.BlockUpdate(this.secret, 0, this.secret.Length);
            this.digest.BlockUpdate(IPAD, 0, this.padLength);
        }

        public virtual void Update(byte input)
        {
            this.digest.Update(input);
        }

        public virtual string AlgorithmName =>
            (this.digest.AlgorithmName + "/SSL3MAC");
    }
}

