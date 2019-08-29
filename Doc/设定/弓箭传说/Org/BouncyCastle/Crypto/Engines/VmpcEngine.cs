namespace Org.BouncyCastle.Crypto.Engines
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using System;

    public class VmpcEngine : IStreamCipher
    {
        protected byte n;
        protected byte[] P;
        protected byte s;
        protected byte[] workingIV;
        protected byte[] workingKey;

        public virtual void Init(bool forEncryption, ICipherParameters parameters)
        {
            if (!(parameters is ParametersWithIV))
            {
                throw new ArgumentException("VMPC Init parameters must include an IV");
            }
            ParametersWithIV hiv = (ParametersWithIV) parameters;
            if (!(hiv.Parameters is KeyParameter))
            {
                throw new ArgumentException("VMPC Init parameters must include a key");
            }
            KeyParameter parameter = (KeyParameter) hiv.Parameters;
            this.workingIV = hiv.GetIV();
            if (((this.workingIV == null) || (this.workingIV.Length < 1)) || (this.workingIV.Length > 0x300))
            {
                throw new ArgumentException("VMPC requires 1 to 768 bytes of IV");
            }
            this.workingKey = parameter.GetKey();
            this.InitKey(this.workingKey, this.workingIV);
        }

        protected virtual void InitKey(byte[] keyBytes, byte[] ivBytes)
        {
            this.s = 0;
            this.P = new byte[0x100];
            for (int i = 0; i < 0x100; i++)
            {
                this.P[i] = (byte) i;
            }
            for (int j = 0; j < 0x300; j++)
            {
                this.s = this.P[((this.s + this.P[j & 0xff]) + keyBytes[j % keyBytes.Length]) & 0xff];
                byte num3 = this.P[j & 0xff];
                this.P[j & 0xff] = this.P[this.s & 0xff];
                this.P[this.s & 0xff] = num3;
            }
            for (int k = 0; k < 0x300; k++)
            {
                this.s = this.P[((this.s + this.P[k & 0xff]) + ivBytes[k % ivBytes.Length]) & 0xff];
                byte num5 = this.P[k & 0xff];
                this.P[k & 0xff] = this.P[this.s & 0xff];
                this.P[this.s & 0xff] = num5;
            }
            this.n = 0;
        }

        public virtual void ProcessBytes(byte[] input, int inOff, int len, byte[] output, int outOff)
        {
            Check.DataLength(input, inOff, len, "input buffer too short");
            Check.OutputLength(output, outOff, len, "output buffer too short");
            for (int i = 0; i < len; i++)
            {
                this.s = this.P[(this.s + this.P[this.n & 0xff]) & 0xff];
                byte num2 = this.P[(this.P[this.P[this.s & 0xff] & 0xff] + 1) & 0xff];
                byte num3 = this.P[this.n & 0xff];
                this.P[this.n & 0xff] = this.P[this.s & 0xff];
                this.P[this.s & 0xff] = num3;
                this.n = (byte) ((this.n + 1) & 0xff);
                output[i + outOff] = (byte) (input[i + inOff] ^ num2);
            }
        }

        public virtual void Reset()
        {
            this.InitKey(this.workingKey, this.workingIV);
        }

        public virtual byte ReturnByte(byte input)
        {
            this.s = this.P[(this.s + this.P[this.n & 0xff]) & 0xff];
            byte num = this.P[(this.P[this.P[this.s & 0xff] & 0xff] + 1) & 0xff];
            byte num2 = this.P[this.n & 0xff];
            this.P[this.n & 0xff] = this.P[this.s & 0xff];
            this.P[this.s & 0xff] = num2;
            this.n = (byte) ((this.n + 1) & 0xff);
            return (byte) (input ^ num);
        }

        public virtual string AlgorithmName =>
            "VMPC";
    }
}

