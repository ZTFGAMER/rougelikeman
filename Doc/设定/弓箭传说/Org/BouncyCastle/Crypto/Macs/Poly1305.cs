namespace Org.BouncyCastle.Crypto.Macs
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Crypto.Utilities;
    using System;

    public class Poly1305 : IMac
    {
        private const int BlockSize = 0x10;
        private readonly IBlockCipher cipher;
        private readonly byte[] singleByte;
        private uint r0;
        private uint r1;
        private uint r2;
        private uint r3;
        private uint r4;
        private uint s1;
        private uint s2;
        private uint s3;
        private uint s4;
        private uint k0;
        private uint k1;
        private uint k2;
        private uint k3;
        private byte[] currentBlock;
        private int currentBlockOffset;
        private uint h0;
        private uint h1;
        private uint h2;
        private uint h3;
        private uint h4;

        public Poly1305()
        {
            this.singleByte = new byte[1];
            this.currentBlock = new byte[0x10];
            this.cipher = null;
        }

        public Poly1305(IBlockCipher cipher)
        {
            this.singleByte = new byte[1];
            this.currentBlock = new byte[0x10];
            if (cipher.GetBlockSize() != 0x10)
            {
                throw new ArgumentException("Poly1305 requires a 128 bit block cipher.");
            }
            this.cipher = cipher;
        }

        public void BlockUpdate(byte[] input, int inOff, int len)
        {
            int num = 0;
            while (len > num)
            {
                if (this.currentBlockOffset == 0x10)
                {
                    this.ProcessBlock();
                    this.currentBlockOffset = 0;
                }
                int length = Math.Min((int) (len - num), (int) (0x10 - this.currentBlockOffset));
                Array.Copy(input, num + inOff, this.currentBlock, this.currentBlockOffset, length);
                num += length;
                this.currentBlockOffset += length;
            }
        }

        public int DoFinal(byte[] output, int outOff)
        {
            Check.DataLength(output, outOff, 0x10, "Output buffer is too short.");
            if (this.currentBlockOffset > 0)
            {
                this.ProcessBlock();
            }
            uint num5 = this.h0 >> 0x1a;
            this.h0 &= 0x3ffffff;
            this.h1 += num5;
            num5 = this.h1 >> 0x1a;
            this.h1 &= 0x3ffffff;
            this.h2 += num5;
            num5 = this.h2 >> 0x1a;
            this.h2 &= 0x3ffffff;
            this.h3 += num5;
            num5 = this.h3 >> 0x1a;
            this.h3 &= 0x3ffffff;
            this.h4 += num5;
            num5 = this.h4 >> 0x1a;
            this.h4 &= 0x3ffffff;
            this.h0 += num5 * 5;
            uint num6 = this.h0 + 5;
            num5 = num6 >> 0x1a;
            num6 &= 0x3ffffff;
            uint num7 = this.h1 + num5;
            num5 = num7 >> 0x1a;
            num7 &= 0x3ffffff;
            uint num8 = this.h2 + num5;
            num5 = num8 >> 0x1a;
            num8 &= 0x3ffffff;
            uint num9 = this.h3 + num5;
            num5 = num9 >> 0x1a;
            num9 &= 0x3ffffff;
            uint num10 = (this.h4 + num5) - 0x4000000;
            num5 = (num10 >> 0x1f) - 1;
            uint num11 = ~num5;
            this.h0 = (this.h0 & num11) | (num6 & num5);
            this.h1 = (this.h1 & num11) | (num7 & num5);
            this.h2 = (this.h2 & num11) | (num8 & num5);
            this.h3 = (this.h3 & num11) | (num9 & num5);
            this.h4 = (this.h4 & num11) | (num10 & num5);
            ulong num = (this.h0 | (this.h1 << 0x1a)) + this.k0;
            ulong num2 = ((this.h1 >> 6) | (this.h2 << 20)) + this.k1;
            ulong num3 = ((this.h2 >> 12) | (this.h3 << 14)) + this.k2;
            ulong num4 = ((this.h3 >> 0x12) | (this.h4 << 8)) + this.k3;
            Pack.UInt32_To_LE((uint) num, output, outOff);
            num2 += num >> 0x20;
            Pack.UInt32_To_LE((uint) num2, output, outOff + 4);
            num3 += num2 >> 0x20;
            Pack.UInt32_To_LE((uint) num3, output, outOff + 8);
            num4 += num3 >> 0x20;
            Pack.UInt32_To_LE((uint) num4, output, outOff + 12);
            this.Reset();
            return 0x10;
        }

        public int GetMacSize() => 
            0x10;

        public void Init(ICipherParameters parameters)
        {
            byte[] nonce = null;
            if (this.cipher != null)
            {
                if (!(parameters is ParametersWithIV))
                {
                    throw new ArgumentException("Poly1305 requires an IV when used with a block cipher.", "parameters");
                }
                ParametersWithIV hiv = (ParametersWithIV) parameters;
                nonce = hiv.GetIV();
                parameters = hiv.Parameters;
            }
            if (!(parameters is KeyParameter))
            {
                throw new ArgumentException("Poly1305 requires a key.");
            }
            this.SetKey(((KeyParameter) parameters).GetKey(), nonce);
            this.Reset();
        }

        private static ulong mul32x32_64(uint i1, uint i2) => 
            (i1 * i2);

        private void ProcessBlock()
        {
            if (this.currentBlockOffset < 0x10)
            {
                this.currentBlock[this.currentBlockOffset] = 1;
                for (int i = this.currentBlockOffset + 1; i < 0x10; i++)
                {
                    this.currentBlock[i] = 0;
                }
            }
            ulong num2 = Pack.LE_To_UInt32(this.currentBlock, 0);
            ulong num3 = Pack.LE_To_UInt32(this.currentBlock, 4);
            ulong num4 = Pack.LE_To_UInt32(this.currentBlock, 8);
            ulong num5 = Pack.LE_To_UInt32(this.currentBlock, 12);
            this.h0 += (uint) (num2 & ((ulong) 0x3ffffffL));
            this.h1 += (uint) ((((num3 << 0x20) | num2) >> 0x1a) & ((ulong) 0x3ffffffL));
            this.h2 += (uint) ((((num4 << 0x20) | num3) >> 20) & ((ulong) 0x3ffffffL));
            this.h3 += (uint) ((((num5 << 0x20) | num4) >> 14) & ((ulong) 0x3ffffffL));
            this.h4 += (uint) (num5 >> 8);
            if (this.currentBlockOffset == 0x10)
            {
                this.h4 += 0x1000000;
            }
            ulong num6 = (((mul32x32_64(this.h0, this.r0) + mul32x32_64(this.h1, this.s4)) + mul32x32_64(this.h2, this.s3)) + mul32x32_64(this.h3, this.s2)) + mul32x32_64(this.h4, this.s1);
            ulong num7 = (((mul32x32_64(this.h0, this.r1) + mul32x32_64(this.h1, this.r0)) + mul32x32_64(this.h2, this.s4)) + mul32x32_64(this.h3, this.s3)) + mul32x32_64(this.h4, this.s2);
            ulong num8 = (((mul32x32_64(this.h0, this.r2) + mul32x32_64(this.h1, this.r1)) + mul32x32_64(this.h2, this.r0)) + mul32x32_64(this.h3, this.s4)) + mul32x32_64(this.h4, this.s3);
            ulong num9 = (((mul32x32_64(this.h0, this.r3) + mul32x32_64(this.h1, this.r2)) + mul32x32_64(this.h2, this.r1)) + mul32x32_64(this.h3, this.r0)) + mul32x32_64(this.h4, this.s4);
            ulong num10 = (((mul32x32_64(this.h0, this.r4) + mul32x32_64(this.h1, this.r3)) + mul32x32_64(this.h2, this.r2)) + mul32x32_64(this.h3, this.r1)) + mul32x32_64(this.h4, this.r0);
            this.h0 = ((uint) num6) & 0x3ffffff;
            ulong num11 = num6 >> 0x1a;
            num7 += num11;
            this.h1 = ((uint) num7) & 0x3ffffff;
            num11 = num7 >> 0x1a;
            num8 += num11;
            this.h2 = ((uint) num8) & 0x3ffffff;
            num11 = num8 >> 0x1a;
            num9 += num11;
            this.h3 = ((uint) num9) & 0x3ffffff;
            num11 = num9 >> 0x1a;
            num10 += num11;
            this.h4 = ((uint) num10) & 0x3ffffff;
            num11 = num10 >> 0x1a;
            this.h0 += (uint) (num11 * ((ulong) 5L));
        }

        public void Reset()
        {
            this.currentBlockOffset = 0;
            this.h0 = this.h1 = this.h2 = this.h3 = this.h4 = 0;
        }

        private void SetKey(byte[] key, byte[] nonce)
        {
            byte[] buffer;
            int num5;
            if (key.Length != 0x20)
            {
                throw new ArgumentException("Poly1305 key must be 256 bits.");
            }
            if ((this.cipher != null) && ((nonce == null) || (nonce.Length != 0x10)))
            {
                throw new ArgumentException("Poly1305 requires a 128 bit IV.");
            }
            uint num = Pack.LE_To_UInt32(key, 0);
            uint num2 = Pack.LE_To_UInt32(key, 4);
            uint num3 = Pack.LE_To_UInt32(key, 8);
            uint num4 = Pack.LE_To_UInt32(key, 12);
            this.r0 = num & 0x3ffffff;
            this.r1 = ((num >> 0x1a) | (num2 << 6)) & 0x3ffff03;
            this.r2 = ((num2 >> 20) | (num3 << 12)) & 0x3ffc0ff;
            this.r3 = ((num3 >> 14) | (num4 << 0x12)) & 0x3f03fff;
            this.r4 = (num4 >> 8) & 0xfffff;
            this.s1 = this.r1 * 5;
            this.s2 = this.r2 * 5;
            this.s3 = this.r3 * 5;
            this.s4 = this.r4 * 5;
            if (this.cipher == null)
            {
                buffer = key;
                num5 = 0x10;
            }
            else
            {
                buffer = new byte[0x10];
                num5 = 0;
                this.cipher.Init(true, new KeyParameter(key, 0x10, 0x10));
                this.cipher.ProcessBlock(nonce, 0, buffer, 0);
            }
            this.k0 = Pack.LE_To_UInt32(buffer, num5);
            this.k1 = Pack.LE_To_UInt32(buffer, num5 + 4);
            this.k2 = Pack.LE_To_UInt32(buffer, num5 + 8);
            this.k3 = Pack.LE_To_UInt32(buffer, num5 + 12);
        }

        public void Update(byte input)
        {
            this.singleByte[0] = input;
            this.BlockUpdate(this.singleByte, 0, 1);
        }

        public string AlgorithmName =>
            ((this.cipher != null) ? ("Poly1305-" + this.cipher.AlgorithmName) : "Poly1305");
    }
}

