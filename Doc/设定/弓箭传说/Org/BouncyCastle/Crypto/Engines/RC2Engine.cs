namespace Org.BouncyCastle.Crypto.Engines
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Utilities;
    using System;

    public class RC2Engine : IBlockCipher
    {
        private static readonly byte[] piTable = new byte[] { 
            0xd9, 120, 0xf9, 0xc4, 0x19, 0xdd, 0xb5, 0xed, 40, 0xe9, 0xfd, 0x79, 0x4a, 160, 0xd8, 0x9d,
            0xc6, 0x7e, 0x37, 0x83, 0x2b, 0x76, 0x53, 0x8e, 0x62, 0x4c, 100, 0x88, 0x44, 0x8b, 0xfb, 0xa2,
            0x17, 0x9a, 0x59, 0xf5, 0x87, 0xb3, 0x4f, 0x13, 0x61, 0x45, 0x6d, 0x8d, 9, 0x81, 0x7d, 50,
            0xbd, 0x8f, 0x40, 0xeb, 0x86, 0xb7, 0x7b, 11, 240, 0x95, 0x21, 0x22, 0x5c, 0x6b, 0x4e, 130,
            0x54, 0xd6, 0x65, 0x93, 0xce, 0x60, 0xb2, 0x1c, 0x73, 0x56, 0xc0, 20, 0xa7, 140, 0xf1, 220,
            0x12, 0x75, 0xca, 0x1f, 0x3b, 190, 0xe4, 0xd1, 0x42, 0x3d, 0xd4, 0x30, 0xa3, 60, 0xb6, 0x26,
            0x6f, 0xbf, 14, 0xda, 70, 0x69, 7, 0x57, 0x27, 0xf2, 0x1d, 0x9b, 0xbc, 0x94, 0x43, 3,
            0xf8, 0x11, 0xc7, 0xf6, 0x90, 0xef, 0x3e, 0xe7, 6, 0xc3, 0xd5, 0x2f, 200, 0x66, 30, 0xd7,
            8, 0xe8, 0xea, 0xde, 0x80, 0x52, 0xee, 0xf7, 0x84, 170, 0x72, 0xac, 0x35, 0x4d, 0x6a, 0x2a,
            150, 0x1a, 210, 0x71, 90, 0x15, 0x49, 0x74, 0x4b, 0x9f, 0xd0, 0x5e, 4, 0x18, 0xa4, 0xec,
            0xc2, 0xe0, 0x41, 110, 15, 0x51, 0xcb, 0xcc, 0x24, 0x91, 0xaf, 80, 0xa1, 0xf4, 0x70, 0x39,
            0x99, 0x7c, 0x3a, 0x85, 0x23, 0xb8, 180, 0x7a, 0xfc, 2, 0x36, 0x5b, 0x25, 0x55, 0x97, 0x31,
            0x2d, 0x5d, 250, 0x98, 0xe3, 0x8a, 0x92, 0xae, 5, 0xdf, 0x29, 0x10, 0x67, 0x6c, 0xba, 0xc9,
            0xd3, 0, 230, 0xcf, 0xe1, 0x9e, 0xa8, 0x2c, 0x63, 0x16, 1, 0x3f, 0x58, 0xe2, 0x89, 0xa9,
            13, 0x38, 0x34, 0x1b, 0xab, 0x33, 0xff, 0xb0, 0xbb, 0x48, 12, 0x5f, 0xb9, 0xb1, 0xcd, 0x2e,
            0xc5, 0xf3, 0xdb, 0x47, 0xe5, 0xa5, 0x9c, 0x77, 10, 0xa6, 0x20, 0x68, 0xfe, 0x7f, 0xc1, 0xad
        };
        private const int BLOCK_SIZE = 8;
        private int[] workingKey;
        private bool encrypting;

        private void DecryptBlock(byte[] input, int inOff, byte[] outBytes, int outOff)
        {
            int x = ((input[inOff + 7] & 0xff) << 8) + (input[inOff + 6] & 0xff);
            int num2 = ((input[inOff + 5] & 0xff) << 8) + (input[inOff + 4] & 0xff);
            int num3 = ((input[inOff + 3] & 0xff) << 8) + (input[inOff + 2] & 0xff);
            int num4 = ((input[inOff + 1] & 0xff) << 8) + (input[inOff] & 0xff);
            for (int i = 60; i >= 0x2c; i -= 4)
            {
                x = this.RotateWordLeft(x, 11) - (((num4 & ~num2) + (num3 & num2)) + this.workingKey[i + 3]);
                num2 = this.RotateWordLeft(num2, 13) - (((x & ~num3) + (num4 & num3)) + this.workingKey[i + 2]);
                num3 = this.RotateWordLeft(num3, 14) - (((num2 & ~num4) + (x & num4)) + this.workingKey[i + 1]);
                num4 = this.RotateWordLeft(num4, 15) - (((num3 & ~x) + (num2 & x)) + this.workingKey[i]);
            }
            x -= this.workingKey[num2 & 0x3f];
            num2 -= this.workingKey[num3 & 0x3f];
            num3 -= this.workingKey[num4 & 0x3f];
            num4 -= this.workingKey[x & 0x3f];
            for (int j = 40; j >= 20; j -= 4)
            {
                x = this.RotateWordLeft(x, 11) - (((num4 & ~num2) + (num3 & num2)) + this.workingKey[j + 3]);
                num2 = this.RotateWordLeft(num2, 13) - (((x & ~num3) + (num4 & num3)) + this.workingKey[j + 2]);
                num3 = this.RotateWordLeft(num3, 14) - (((num2 & ~num4) + (x & num4)) + this.workingKey[j + 1]);
                num4 = this.RotateWordLeft(num4, 15) - (((num3 & ~x) + (num2 & x)) + this.workingKey[j]);
            }
            x -= this.workingKey[num2 & 0x3f];
            num2 -= this.workingKey[num3 & 0x3f];
            num3 -= this.workingKey[num4 & 0x3f];
            num4 -= this.workingKey[x & 0x3f];
            for (int k = 0x10; k >= 0; k -= 4)
            {
                x = this.RotateWordLeft(x, 11) - (((num4 & ~num2) + (num3 & num2)) + this.workingKey[k + 3]);
                num2 = this.RotateWordLeft(num2, 13) - (((x & ~num3) + (num4 & num3)) + this.workingKey[k + 2]);
                num3 = this.RotateWordLeft(num3, 14) - (((num2 & ~num4) + (x & num4)) + this.workingKey[k + 1]);
                num4 = this.RotateWordLeft(num4, 15) - (((num3 & ~x) + (num2 & x)) + this.workingKey[k]);
            }
            outBytes[outOff] = (byte) num4;
            outBytes[outOff + 1] = (byte) (num4 >> 8);
            outBytes[outOff + 2] = (byte) num3;
            outBytes[outOff + 3] = (byte) (num3 >> 8);
            outBytes[outOff + 4] = (byte) num2;
            outBytes[outOff + 5] = (byte) (num2 >> 8);
            outBytes[outOff + 6] = (byte) x;
            outBytes[outOff + 7] = (byte) (x >> 8);
        }

        private void EncryptBlock(byte[] input, int inOff, byte[] outBytes, int outOff)
        {
            int num = ((input[inOff + 7] & 0xff) << 8) + (input[inOff + 6] & 0xff);
            int num2 = ((input[inOff + 5] & 0xff) << 8) + (input[inOff + 4] & 0xff);
            int num3 = ((input[inOff + 3] & 0xff) << 8) + (input[inOff + 2] & 0xff);
            int num4 = ((input[inOff + 1] & 0xff) << 8) + (input[inOff] & 0xff);
            for (int i = 0; i <= 0x10; i += 4)
            {
                num4 = this.RotateWordLeft(((num4 + (num3 & ~num)) + (num2 & num)) + this.workingKey[i], 1);
                num3 = this.RotateWordLeft(((num3 + (num2 & ~num4)) + (num & num4)) + this.workingKey[i + 1], 2);
                num2 = this.RotateWordLeft(((num2 + (num & ~num3)) + (num4 & num3)) + this.workingKey[i + 2], 3);
                num = this.RotateWordLeft(((num + (num4 & ~num2)) + (num3 & num2)) + this.workingKey[i + 3], 5);
            }
            num4 += this.workingKey[num & 0x3f];
            num3 += this.workingKey[num4 & 0x3f];
            num2 += this.workingKey[num3 & 0x3f];
            num += this.workingKey[num2 & 0x3f];
            for (int j = 20; j <= 40; j += 4)
            {
                num4 = this.RotateWordLeft(((num4 + (num3 & ~num)) + (num2 & num)) + this.workingKey[j], 1);
                num3 = this.RotateWordLeft(((num3 + (num2 & ~num4)) + (num & num4)) + this.workingKey[j + 1], 2);
                num2 = this.RotateWordLeft(((num2 + (num & ~num3)) + (num4 & num3)) + this.workingKey[j + 2], 3);
                num = this.RotateWordLeft(((num + (num4 & ~num2)) + (num3 & num2)) + this.workingKey[j + 3], 5);
            }
            num4 += this.workingKey[num & 0x3f];
            num3 += this.workingKey[num4 & 0x3f];
            num2 += this.workingKey[num3 & 0x3f];
            num += this.workingKey[num2 & 0x3f];
            for (int k = 0x2c; k < 0x40; k += 4)
            {
                num4 = this.RotateWordLeft(((num4 + (num3 & ~num)) + (num2 & num)) + this.workingKey[k], 1);
                num3 = this.RotateWordLeft(((num3 + (num2 & ~num4)) + (num & num4)) + this.workingKey[k + 1], 2);
                num2 = this.RotateWordLeft(((num2 + (num & ~num3)) + (num4 & num3)) + this.workingKey[k + 2], 3);
                num = this.RotateWordLeft(((num + (num4 & ~num2)) + (num3 & num2)) + this.workingKey[k + 3], 5);
            }
            outBytes[outOff] = (byte) num4;
            outBytes[outOff + 1] = (byte) (num4 >> 8);
            outBytes[outOff + 2] = (byte) num3;
            outBytes[outOff + 3] = (byte) (num3 >> 8);
            outBytes[outOff + 4] = (byte) num2;
            outBytes[outOff + 5] = (byte) (num2 >> 8);
            outBytes[outOff + 6] = (byte) num;
            outBytes[outOff + 7] = (byte) (num >> 8);
        }

        private int[] GenerateWorkingKey(byte[] key, int bits)
        {
            int num;
            int[] numArray = new int[0x80];
            for (int i = 0; i != key.Length; i++)
            {
                numArray[i] = key[i] & 0xff;
            }
            int length = key.Length;
            if (length < 0x80)
            {
                int num4 = 0;
                num = numArray[length - 1];
                do
                {
                    num = piTable[(num + numArray[num4++]) & 0xff] & 0xff;
                    numArray[length++] = num;
                }
                while (length < 0x80);
            }
            length = (bits + 7) >> 3;
            num = piTable[numArray[0x80 - length] & (((int) 0xff) >> (7 & -bits))] & 0xff;
            numArray[0x80 - length] = num;
            for (int j = (0x80 - length) - 1; j >= 0; j--)
            {
                numArray[j] = piTable[num ^ numArray[j + length]] & 0xff;
            }
            int[] numArray2 = new int[0x40];
            for (int k = 0; k != numArray2.Length; k++)
            {
                numArray2[k] = numArray[2 * k] + (numArray[(2 * k) + 1] << 8);
            }
            return numArray2;
        }

        public virtual int GetBlockSize() => 
            8;

        public virtual void Init(bool forEncryption, ICipherParameters parameters)
        {
            this.encrypting = forEncryption;
            if (parameters is RC2Parameters)
            {
                RC2Parameters parameters2 = (RC2Parameters) parameters;
                this.workingKey = this.GenerateWorkingKey(parameters2.GetKey(), parameters2.EffectiveKeyBits);
            }
            else
            {
                if (!(parameters is KeyParameter))
                {
                    throw new ArgumentException("invalid parameter passed to RC2 init - " + Platform.GetTypeName(parameters));
                }
                byte[] key = ((KeyParameter) parameters).GetKey();
                this.workingKey = this.GenerateWorkingKey(key, key.Length * 8);
            }
        }

        public virtual int ProcessBlock(byte[] input, int inOff, byte[] output, int outOff)
        {
            if (this.workingKey == null)
            {
                throw new InvalidOperationException("RC2 engine not initialised");
            }
            Check.DataLength(input, inOff, 8, "input buffer too short");
            Check.OutputLength(output, outOff, 8, "output buffer too short");
            if (this.encrypting)
            {
                this.EncryptBlock(input, inOff, output, outOff);
            }
            else
            {
                this.DecryptBlock(input, inOff, output, outOff);
            }
            return 8;
        }

        public virtual void Reset()
        {
        }

        private int RotateWordLeft(int x, int y)
        {
            x &= 0xffff;
            return ((x << y) | (x >> (0x10 - y)));
        }

        public virtual string AlgorithmName =>
            "RC2";

        public virtual bool IsPartialBlockOkay =>
            false;
    }
}

