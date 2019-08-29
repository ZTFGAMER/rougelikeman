namespace Org.BouncyCastle.Crypto.Engines
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Crypto.Utilities;
    using Org.BouncyCastle.Utilities;
    using System;

    public class HC128Engine : IStreamCipher
    {
        private uint[] p = new uint[0x200];
        private uint[] q = new uint[0x200];
        private uint cnt;
        private byte[] key;
        private byte[] iv;
        private bool initialised;
        private byte[] buf = new byte[4];
        private int idx;

        private static uint Dim(uint x, uint y) => 
            Mod512(x - y);

        private static uint F1(uint x) => 
            ((RotateRight(x, 7) ^ RotateRight(x, 0x12)) ^ (x >> 3));

        private static uint F2(uint x) => 
            ((RotateRight(x, 0x11) ^ RotateRight(x, 0x13)) ^ (x >> 10));

        private uint G1(uint x, uint y, uint z) => 
            ((RotateRight(x, 10) ^ RotateRight(z, 0x17)) + RotateRight(y, 8));

        private uint G2(uint x, uint y, uint z) => 
            ((RotateLeft(x, 10) ^ RotateLeft(z, 0x17)) + RotateLeft(y, 8));

        private byte GetByte()
        {
            if (this.idx == 0)
            {
                Pack.UInt32_To_LE(this.Step(), this.buf);
            }
            byte num = this.buf[this.idx];
            this.idx = (this.idx + 1) & 3;
            return num;
        }

        private uint H1(uint x) => 
            (this.q[(int) ((IntPtr) (x & 0xff))] + this.q[(int) ((IntPtr) (((x >> 0x10) & 0xff) + 0x100))]);

        private uint H2(uint x) => 
            (this.p[(int) ((IntPtr) (x & 0xff))] + this.p[(int) ((IntPtr) (((x >> 0x10) & 0xff) + 0x100))]);

        private void Init()
        {
            if (this.key.Length != 0x10)
            {
                throw new ArgumentException("The key must be 128 bits long");
            }
            this.idx = 0;
            this.cnt = 0;
            uint[] sourceArray = new uint[0x500];
            for (int i = 0; i < 0x10; i++)
            {
                sourceArray[i >> 2] |= (uint) (this.key[i] << (8 * (i & 3)));
            }
            Array.Copy(sourceArray, 0, sourceArray, 4, 4);
            for (int j = 0; (j < this.iv.Length) && (j < 0x10); j++)
            {
                sourceArray[(j >> 2) + 8] |= (uint) (this.iv[j] << (8 * (j & 3)));
            }
            Array.Copy(sourceArray, 8, sourceArray, 12, 4);
            for (uint k = 0x10; k < 0x500; k++)
            {
                sourceArray[k] = (((F2(sourceArray[(int) ((IntPtr) (k - 2))]) + sourceArray[(int) ((IntPtr) (k - 7))]) + F1(sourceArray[(int) ((IntPtr) (k - 15))])) + sourceArray[(int) ((IntPtr) (k - 0x10))]) + k;
            }
            Array.Copy(sourceArray, 0x100, this.p, 0, 0x200);
            Array.Copy(sourceArray, 0x300, this.q, 0, 0x200);
            for (int m = 0; m < 0x200; m++)
            {
                this.p[m] = this.Step();
            }
            for (int n = 0; n < 0x200; n++)
            {
                this.q[n] = this.Step();
            }
            this.cnt = 0;
        }

        public virtual void Init(bool forEncryption, ICipherParameters parameters)
        {
            ICipherParameters parameters2 = parameters;
            if (parameters is ParametersWithIV)
            {
                this.iv = ((ParametersWithIV) parameters).GetIV();
                parameters2 = ((ParametersWithIV) parameters).Parameters;
            }
            else
            {
                this.iv = new byte[0];
            }
            if (!(parameters2 is KeyParameter))
            {
                throw new ArgumentException("Invalid parameter passed to HC128 init - " + Platform.GetTypeName(parameters), "parameters");
            }
            this.key = ((KeyParameter) parameters2).GetKey();
            this.Init();
            this.initialised = true;
        }

        private static uint Mod1024(uint x) => 
            (x & 0x3ff);

        private static uint Mod512(uint x) => 
            (x & 0x1ff);

        public virtual void ProcessBytes(byte[] input, int inOff, int len, byte[] output, int outOff)
        {
            if (!this.initialised)
            {
                throw new InvalidOperationException(this.AlgorithmName + " not initialised");
            }
            Check.DataLength(input, inOff, len, "input buffer too short");
            Check.OutputLength(output, outOff, len, "output buffer too short");
            for (int i = 0; i < len; i++)
            {
                output[outOff + i] = (byte) (input[inOff + i] ^ this.GetByte());
            }
        }

        public virtual void Reset()
        {
            this.Init();
        }

        public virtual byte ReturnByte(byte input) => 
            ((byte) (input ^ this.GetByte()));

        private static uint RotateLeft(uint x, int bits) => 
            ((x << bits) | (x >> -bits));

        private static uint RotateRight(uint x, int bits) => 
            ((x >> bits) | (x << -bits));

        private uint Step()
        {
            uint num2;
            uint index = Mod512(this.cnt);
            if (this.cnt < 0x200)
            {
                this.p[index] += this.G1(this.p[Dim(index, 3)], this.p[Dim(index, 10)], this.p[Dim(index, 0x1ff)]);
                num2 = this.H1(this.p[Dim(index, 12)]) ^ this.p[index];
            }
            else
            {
                this.q[index] += this.G2(this.q[Dim(index, 3)], this.q[Dim(index, 10)], this.q[Dim(index, 0x1ff)]);
                num2 = this.H2(this.q[Dim(index, 12)]) ^ this.q[index];
            }
            this.cnt = Mod1024(this.cnt + 1);
            return num2;
        }

        public virtual string AlgorithmName =>
            "HC-128";
    }
}

