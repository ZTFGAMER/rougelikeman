namespace Org.BouncyCastle.Crypto.Macs
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using System;

    public class VmpcMac : IMac
    {
        private byte g;
        private byte n;
        private byte[] P;
        private byte s;
        private byte[] T;
        private byte[] workingIV;
        private byte[] workingKey;
        private byte x1;
        private byte x2;
        private byte x3;
        private byte x4;

        public virtual void BlockUpdate(byte[] input, int inOff, int len)
        {
            if ((inOff + len) > input.Length)
            {
                throw new DataLengthException("input buffer too short");
            }
            for (int i = 0; i < len; i++)
            {
                this.Update(input[inOff + i]);
            }
        }

        public virtual int DoFinal(byte[] output, int outOff)
        {
            for (int i = 1; i < 0x19; i++)
            {
                this.s = this.P[(this.s + this.P[this.n & 0xff]) & 0xff];
                this.x4 = this.P[((this.x4 + this.x3) + i) & 0xff];
                this.x3 = this.P[((this.x3 + this.x2) + i) & 0xff];
                this.x2 = this.P[((this.x2 + this.x1) + i) & 0xff];
                this.x1 = this.P[((this.x1 + this.s) + i) & 0xff];
                this.T[this.g & 0x1f] = (byte) (this.T[this.g & 0x1f] ^ this.x1);
                this.T[(this.g + 1) & 0x1f] = (byte) (this.T[(this.g + 1) & 0x1f] ^ this.x2);
                this.T[(this.g + 2) & 0x1f] = (byte) (this.T[(this.g + 2) & 0x1f] ^ this.x3);
                this.T[(this.g + 3) & 0x1f] = (byte) (this.T[(this.g + 3) & 0x1f] ^ this.x4);
                this.g = (byte) ((this.g + 4) & 0x1f);
                byte num2 = this.P[this.n & 0xff];
                this.P[this.n & 0xff] = this.P[this.s & 0xff];
                this.P[this.s & 0xff] = num2;
                this.n = (byte) ((this.n + 1) & 0xff);
            }
            for (int j = 0; j < 0x300; j++)
            {
                this.s = this.P[((this.s + this.P[j & 0xff]) + this.T[j & 0x1f]) & 0xff];
                byte num4 = this.P[j & 0xff];
                this.P[j & 0xff] = this.P[this.s & 0xff];
                this.P[this.s & 0xff] = num4;
            }
            byte[] sourceArray = new byte[20];
            for (int k = 0; k < 20; k++)
            {
                this.s = this.P[(this.s + this.P[k & 0xff]) & 0xff];
                sourceArray[k] = this.P[(this.P[this.P[this.s & 0xff] & 0xff] + 1) & 0xff];
                byte num6 = this.P[k & 0xff];
                this.P[k & 0xff] = this.P[this.s & 0xff];
                this.P[this.s & 0xff] = num6;
            }
            Array.Copy(sourceArray, 0, output, outOff, sourceArray.Length);
            this.Reset();
            return sourceArray.Length;
        }

        public virtual int GetMacSize() => 
            20;

        public virtual void Init(ICipherParameters parameters)
        {
            if (!(parameters is ParametersWithIV))
            {
                throw new ArgumentException("VMPC-MAC Init parameters must include an IV", "parameters");
            }
            ParametersWithIV hiv = (ParametersWithIV) parameters;
            KeyParameter parameter = (KeyParameter) hiv.Parameters;
            if (!(hiv.Parameters is KeyParameter))
            {
                throw new ArgumentException("VMPC-MAC Init parameters must include a key", "parameters");
            }
            this.workingIV = hiv.GetIV();
            if (((this.workingIV == null) || (this.workingIV.Length < 1)) || (this.workingIV.Length > 0x300))
            {
                throw new ArgumentException("VMPC-MAC requires 1 to 768 bytes of IV", "parameters");
            }
            this.workingKey = parameter.GetKey();
            this.Reset();
        }

        private void initKey(byte[] keyBytes, byte[] ivBytes)
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

        public virtual void Reset()
        {
            this.initKey(this.workingKey, this.workingIV);
            this.g = this.x1 = this.x2 = this.x3 = this.x4 = (byte) (this.n = 0);
            this.T = new byte[0x20];
            for (int i = 0; i < 0x20; i++)
            {
                this.T[i] = 0;
            }
        }

        public virtual void Update(byte input)
        {
            this.s = this.P[(this.s + this.P[this.n & 0xff]) & 0xff];
            byte num = (byte) (input ^ this.P[(this.P[this.P[this.s & 0xff] & 0xff] + 1) & 0xff]);
            this.x4 = this.P[(this.x4 + this.x3) & 0xff];
            this.x3 = this.P[(this.x3 + this.x2) & 0xff];
            this.x2 = this.P[(this.x2 + this.x1) & 0xff];
            this.x1 = this.P[((this.x1 + this.s) + num) & 0xff];
            this.T[this.g & 0x1f] = (byte) (this.T[this.g & 0x1f] ^ this.x1);
            this.T[(this.g + 1) & 0x1f] = (byte) (this.T[(this.g + 1) & 0x1f] ^ this.x2);
            this.T[(this.g + 2) & 0x1f] = (byte) (this.T[(this.g + 2) & 0x1f] ^ this.x3);
            this.T[(this.g + 3) & 0x1f] = (byte) (this.T[(this.g + 3) & 0x1f] ^ this.x4);
            this.g = (byte) ((this.g + 4) & 0x1f);
            byte num2 = this.P[this.n & 0xff];
            this.P[this.n & 0xff] = this.P[this.s & 0xff];
            this.P[this.s & 0xff] = num2;
            this.n = (byte) ((this.n + 1) & 0xff);
        }

        public virtual string AlgorithmName =>
            "VMPC-MAC";
    }
}

