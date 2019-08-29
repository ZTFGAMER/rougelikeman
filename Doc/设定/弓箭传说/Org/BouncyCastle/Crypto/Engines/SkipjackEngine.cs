namespace Org.BouncyCastle.Crypto.Engines
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Utilities;
    using System;

    public class SkipjackEngine : IBlockCipher
    {
        private const int BLOCK_SIZE = 8;
        private static readonly short[] ftable = new short[] { 
            0xa3, 0xd7, 9, 0x83, 0xf8, 0x48, 0xf6, 0xf4, 0xb3, 0x21, 0x15, 120, 0x99, 0xb1, 0xaf, 0xf9,
            0xe7, 0x2d, 0x4d, 0x8a, 0xce, 0x4c, 0xca, 0x2e, 0x52, 0x95, 0xd9, 30, 0x4e, 0x38, 0x44, 40,
            10, 0xdf, 2, 160, 0x17, 0xf1, 0x60, 0x68, 0x12, 0xb7, 0x7a, 0xc3, 0xe9, 250, 0x3d, 0x53,
            150, 0x84, 0x6b, 0xba, 0xf2, 0x63, 0x9a, 0x19, 0x7c, 0xae, 0xe5, 0xf5, 0xf7, 0x16, 0x6a, 0xa2,
            0x39, 0xb6, 0x7b, 15, 0xc1, 0x93, 0x81, 0x1b, 0xee, 180, 0x1a, 0xea, 0xd0, 0x91, 0x2f, 0xb8,
            0x55, 0xb9, 0xda, 0x85, 0x3f, 0x41, 0xbf, 0xe0, 90, 0x58, 0x80, 0x5f, 0x66, 11, 0xd8, 0x90,
            0x35, 0xd5, 0xc0, 0xa7, 0x33, 6, 0x65, 0x69, 0x45, 0, 0x94, 0x56, 0x6d, 0x98, 0x9b, 0x76,
            0x97, 0xfc, 0xb2, 0xc2, 0xb0, 0xfe, 0xdb, 0x20, 0xe1, 0xeb, 0xd6, 0xe4, 0xdd, 0x47, 0x4a, 0x1d,
            0x42, 0xed, 0x9e, 110, 0x49, 60, 0xcd, 0x43, 0x27, 210, 7, 0xd4, 0xde, 0xc7, 0x67, 0x18,
            0x89, 0xcb, 0x30, 0x1f, 0x8d, 0xc6, 0x8f, 170, 200, 0x74, 220, 0xc9, 0x5d, 0x5c, 0x31, 0xa4,
            0x70, 0x88, 0x61, 0x2c, 0x9f, 13, 0x2b, 0x87, 80, 130, 0x54, 100, 0x26, 0x7d, 3, 0x40,
            0x34, 0x4b, 0x1c, 0x73, 0xd1, 0xc4, 0xfd, 0x3b, 0xcc, 0xfb, 0x7f, 0xab, 230, 0x3e, 0x5b, 0xa5,
            0xad, 4, 0x23, 0x9c, 20, 0x51, 0x22, 240, 0x29, 0x79, 0x71, 0x7e, 0xff, 140, 14, 0xe2,
            12, 0xef, 0xbc, 0x72, 0x75, 0x6f, 0x37, 0xa1, 0xec, 0xd3, 0x8e, 0x62, 0x8b, 0x86, 0x10, 0xe8,
            8, 0x77, 0x11, 190, 0x92, 0x4f, 0x24, 0xc5, 50, 0x36, 0x9d, 0xcf, 0xf3, 0xa6, 0xbb, 0xac,
            0x5e, 0x6c, 0xa9, 0x13, 0x57, 0x25, 0xb5, 0xe3, 0xbd, 0xa8, 0x3a, 1, 5, 0x59, 0x2a, 70
        };
        private int[] key0;
        private int[] key1;
        private int[] key2;
        private int[] key3;
        private bool encrypting;

        public virtual int DecryptBlock(byte[] input, int inOff, byte[] outBytes, int outOff)
        {
            int num = (input[inOff] << 8) + (input[inOff + 1] & 0xff);
            int w = (input[inOff + 2] << 8) + (input[inOff + 3] & 0xff);
            int num3 = (input[inOff + 4] << 8) + (input[inOff + 5] & 0xff);
            int num4 = (input[inOff + 6] << 8) + (input[inOff + 7] & 0xff);
            int k = 0x1f;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    int num8 = num3;
                    num3 = num4;
                    num4 = num;
                    num = this.H(k, w);
                    w = (num ^ num8) ^ (k + 1);
                    k--;
                }
                for (int m = 0; m < 8; m++)
                {
                    int num10 = num3;
                    num3 = num4;
                    num4 = (w ^ num) ^ (k + 1);
                    num = this.H(k, w);
                    w = num10;
                    k--;
                }
            }
            outBytes[outOff] = (byte) (num >> 8);
            outBytes[outOff + 1] = (byte) num;
            outBytes[outOff + 2] = (byte) (w >> 8);
            outBytes[outOff + 3] = (byte) w;
            outBytes[outOff + 4] = (byte) (num3 >> 8);
            outBytes[outOff + 5] = (byte) num3;
            outBytes[outOff + 6] = (byte) (num4 >> 8);
            outBytes[outOff + 7] = (byte) num4;
            return 8;
        }

        public virtual int EncryptBlock(byte[] input, int inOff, byte[] outBytes, int outOff)
        {
            int w = (input[inOff] << 8) + (input[inOff + 1] & 0xff);
            int num2 = (input[inOff + 2] << 8) + (input[inOff + 3] & 0xff);
            int num3 = (input[inOff + 4] << 8) + (input[inOff + 5] & 0xff);
            int num4 = (input[inOff + 6] << 8) + (input[inOff + 7] & 0xff);
            int k = 0;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    int num8 = num4;
                    num4 = num3;
                    num3 = num2;
                    num2 = this.G(k, w);
                    w = (num2 ^ num8) ^ (k + 1);
                    k++;
                }
                for (int m = 0; m < 8; m++)
                {
                    int num10 = num4;
                    num4 = num3;
                    num3 = (w ^ num2) ^ (k + 1);
                    num2 = this.G(k, w);
                    w = num10;
                    k++;
                }
            }
            outBytes[outOff] = (byte) (w >> 8);
            outBytes[outOff + 1] = (byte) w;
            outBytes[outOff + 2] = (byte) (num2 >> 8);
            outBytes[outOff + 3] = (byte) num2;
            outBytes[outOff + 4] = (byte) (num3 >> 8);
            outBytes[outOff + 5] = (byte) num3;
            outBytes[outOff + 6] = (byte) (num4 >> 8);
            outBytes[outOff + 7] = (byte) num4;
            return 8;
        }

        private int G(int k, int w)
        {
            int num = (w >> 8) & 0xff;
            int num2 = w & 0xff;
            int num3 = ftable[num2 ^ this.key0[k]] ^ num;
            int num4 = ftable[num3 ^ this.key1[k]] ^ num2;
            int num5 = ftable[num4 ^ this.key2[k]] ^ num3;
            int num6 = ftable[num5 ^ this.key3[k]] ^ num4;
            return ((num5 << 8) + num6);
        }

        public virtual int GetBlockSize() => 
            8;

        private int H(int k, int w)
        {
            int num = w & 0xff;
            int num2 = (w >> 8) & 0xff;
            int num3 = ftable[num2 ^ this.key3[k]] ^ num;
            int num4 = ftable[num3 ^ this.key2[k]] ^ num2;
            int num5 = ftable[num4 ^ this.key1[k]] ^ num3;
            int num6 = ftable[num5 ^ this.key0[k]] ^ num4;
            return ((num6 << 8) + num5);
        }

        public virtual void Init(bool forEncryption, ICipherParameters parameters)
        {
            if (!(parameters is KeyParameter))
            {
                throw new ArgumentException("invalid parameter passed to SKIPJACK init - " + Platform.GetTypeName(parameters));
            }
            byte[] key = ((KeyParameter) parameters).GetKey();
            this.encrypting = forEncryption;
            this.key0 = new int[0x20];
            this.key1 = new int[0x20];
            this.key2 = new int[0x20];
            this.key3 = new int[0x20];
            for (int i = 0; i < 0x20; i++)
            {
                this.key0[i] = key[(i * 4) % 10] & 0xff;
                this.key1[i] = key[((i * 4) + 1) % 10] & 0xff;
                this.key2[i] = key[((i * 4) + 2) % 10] & 0xff;
                this.key3[i] = key[((i * 4) + 3) % 10] & 0xff;
            }
        }

        public virtual int ProcessBlock(byte[] input, int inOff, byte[] output, int outOff)
        {
            if (this.key1 == null)
            {
                throw new InvalidOperationException("SKIPJACK engine not initialised");
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

        public virtual string AlgorithmName =>
            "SKIPJACK";

        public virtual bool IsPartialBlockOkay =>
            false;
    }
}

