namespace Org.BouncyCastle.Crypto.Engines
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Crypto.Utilities;
    using Org.BouncyCastle.Utilities;
    using System;

    public class HC256Engine : IStreamCipher
    {
        private uint[] p = new uint[0x400];
        private uint[] q = new uint[0x400];
        private uint cnt;
        private byte[] key;
        private byte[] iv;
        private bool initialised;
        private byte[] buf = new byte[4];
        private int idx;

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

        private void Init()
        {
            if ((this.key.Length != 0x20) && (this.key.Length != 0x10))
            {
                throw new ArgumentException("The key must be 128/256 bits long");
            }
            if (this.iv.Length < 0x10)
            {
                throw new ArgumentException("The IV must be at least 128 bits long");
            }
            if (this.key.Length != 0x20)
            {
                byte[] destinationArray = new byte[0x20];
                Array.Copy(this.key, 0, destinationArray, 0, this.key.Length);
                Array.Copy(this.key, 0, destinationArray, 0x10, this.key.Length);
                this.key = destinationArray;
            }
            if (this.iv.Length < 0x20)
            {
                byte[] destinationArray = new byte[0x20];
                Array.Copy(this.iv, 0, destinationArray, 0, this.iv.Length);
                Array.Copy(this.iv, 0, destinationArray, this.iv.Length, destinationArray.Length - this.iv.Length);
                this.iv = destinationArray;
            }
            this.idx = 0;
            this.cnt = 0;
            uint[] sourceArray = new uint[0xa00];
            for (int i = 0; i < 0x20; i++)
            {
                sourceArray[i >> 2] |= (uint) (this.key[i] << (8 * (i & 3)));
            }
            for (int j = 0; j < 0x20; j++)
            {
                sourceArray[(j >> 2) + 8] |= (uint) (this.iv[j] << (8 * (j & 3)));
            }
            for (uint k = 0x10; k < 0xa00; k++)
            {
                uint x = sourceArray[(int) ((IntPtr) (k - 2))];
                uint num5 = sourceArray[(int) ((IntPtr) (k - 15))];
                sourceArray[k] = (((((RotateRight(x, 0x11) ^ RotateRight(x, 0x13)) ^ (x >> 10)) + sourceArray[(int) ((IntPtr) (k - 7))]) + ((RotateRight(num5, 7) ^ RotateRight(num5, 0x12)) ^ (num5 >> 3))) + sourceArray[(int) ((IntPtr) (k - 0x10))]) + k;
            }
            Array.Copy(sourceArray, 0x200, this.p, 0, 0x400);
            Array.Copy(sourceArray, 0x600, this.q, 0, 0x400);
            for (int m = 0; m < 0x1000; m++)
            {
                this.Step();
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
                throw new ArgumentException("Invalid parameter passed to HC256 init - " + Platform.GetTypeName(parameters), "parameters");
            }
            this.key = ((KeyParameter) parameters2).GetKey();
            this.Init();
            this.initialised = true;
        }

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

        private static uint RotateRight(uint x, int bits) => 
            ((x >> bits) | (x << -bits));

        private uint Step()
        {
            uint num2;
            uint index = this.cnt & 0x3ff;
            if (this.cnt < 0x400)
            {
                uint x = this.p[(int) ((IntPtr) ((index - 3) & 0x3ff))];
                uint num4 = this.p[(int) ((IntPtr) ((index - 0x3ff) & 0x3ff))];
                this.p[index] += (this.p[(int) ((IntPtr) ((index - 10) & 0x3ff))] + (RotateRight(x, 10) ^ RotateRight(num4, 0x17))) + this.q[(int) ((IntPtr) ((x ^ num4) & 0x3ff))];
                x = this.p[(int) ((IntPtr) ((index - 12) & 0x3ff))];
                num2 = (((this.q[(int) ((IntPtr) (x & 0xff))] + this.q[(int) ((IntPtr) (((x >> 8) & 0xff) + 0x100))]) + this.q[(int) ((IntPtr) (((x >> 0x10) & 0xff) + 0x200))]) + this.q[(int) ((IntPtr) (((x >> 0x18) & 0xff) + 0x300))]) ^ this.p[index];
            }
            else
            {
                uint x = this.q[(int) ((IntPtr) ((index - 3) & 0x3ff))];
                uint num6 = this.q[(int) ((IntPtr) ((index - 0x3ff) & 0x3ff))];
                this.q[index] += (this.q[(int) ((IntPtr) ((index - 10) & 0x3ff))] + (RotateRight(x, 10) ^ RotateRight(num6, 0x17))) + this.p[(int) ((IntPtr) ((x ^ num6) & 0x3ff))];
                x = this.q[(int) ((IntPtr) ((index - 12) & 0x3ff))];
                num2 = (((this.p[(int) ((IntPtr) (x & 0xff))] + this.p[(int) ((IntPtr) (((x >> 8) & 0xff) + 0x100))]) + this.p[(int) ((IntPtr) (((x >> 0x10) & 0xff) + 0x200))]) + this.p[(int) ((IntPtr) (((x >> 0x18) & 0xff) + 0x300))]) ^ this.q[index];
            }
            this.cnt = (this.cnt + 1) & 0x7ff;
            return num2;
        }

        public virtual string AlgorithmName =>
            "HC-256";
    }
}

