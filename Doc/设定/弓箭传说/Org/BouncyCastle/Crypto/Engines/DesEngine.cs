namespace Org.BouncyCastle.Crypto.Engines
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Crypto.Utilities;
    using Org.BouncyCastle.Utilities;
    using System;

    public class DesEngine : IBlockCipher
    {
        internal const int BLOCK_SIZE = 8;
        private int[] workingKey;
        private static readonly short[] bytebit = new short[] { 0x80, 0x40, 0x20, 0x10, 8, 4, 2, 1 };
        private static readonly int[] bigbyte = new int[] { 
            0x800000, 0x400000, 0x200000, 0x100000, 0x80000, 0x40000, 0x20000, 0x10000, 0x8000, 0x4000, 0x2000, 0x1000, 0x800, 0x400, 0x200, 0x100,
            0x80, 0x40, 0x20, 0x10, 8, 4, 2, 1
        };
        private static readonly byte[] pc1 = new byte[] { 
            0x38, 0x30, 40, 0x20, 0x18, 0x10, 8, 0, 0x39, 0x31, 0x29, 0x21, 0x19, 0x11, 9, 1,
            0x3a, 50, 0x2a, 0x22, 0x1a, 0x12, 10, 2, 0x3b, 0x33, 0x2b, 0x23, 0x3e, 0x36, 0x2e, 0x26,
            30, 0x16, 14, 6, 0x3d, 0x35, 0x2d, 0x25, 0x1d, 0x15, 13, 5, 60, 0x34, 0x2c, 0x24,
            0x1c, 20, 12, 4, 0x1b, 0x13, 11, 3
        };
        private static readonly byte[] totrot = new byte[] { 1, 2, 4, 6, 8, 10, 12, 14, 15, 0x11, 0x13, 0x15, 0x17, 0x19, 0x1b, 0x1c };
        private static readonly byte[] pc2 = new byte[] { 
            13, 0x10, 10, 0x17, 0, 4, 2, 0x1b, 14, 5, 20, 9, 0x16, 0x12, 11, 3,
            0x19, 7, 15, 6, 0x1a, 0x13, 12, 1, 40, 0x33, 30, 0x24, 0x2e, 0x36, 0x1d, 0x27,
            50, 0x2c, 0x20, 0x2f, 0x2b, 0x30, 0x26, 0x37, 0x21, 0x34, 0x2d, 0x29, 0x31, 0x23, 0x1c, 0x1f
        };
        private static readonly uint[] SP1 = new uint[] { 
            0x1010400, 0, 0x10000, 0x1010404, 0x1010004, 0x10404, 4, 0x10000, 0x400, 0x1010400, 0x1010404, 0x400, 0x1000404, 0x1010004, 0x1000000, 4,
            0x404, 0x1000400, 0x1000400, 0x10400, 0x10400, 0x1010000, 0x1010000, 0x1000404, 0x10004, 0x1000004, 0x1000004, 0x10004, 0, 0x404, 0x10404, 0x1000000,
            0x10000, 0x1010404, 4, 0x1010000, 0x1010400, 0x1000000, 0x1000000, 0x400, 0x1010004, 0x10000, 0x10400, 0x1000004, 0x400, 4, 0x1000404, 0x10404,
            0x1010404, 0x10004, 0x1010000, 0x1000404, 0x1000004, 0x404, 0x10404, 0x1010400, 0x404, 0x1000400, 0x1000400, 0, 0x10004, 0x10400, 0, 0x1010004
        };
        private static readonly uint[] SP2 = new uint[] { 
            0x80108020, 0x80008000, 0x8000, 0x108020, 0x100000, 0x20, 0x80100020, 0x80008020, 0x80000020, 0x80108020, 0x80108000, 0x80000000, 0x80008000, 0x100000, 0x20, 0x80100020,
            0x108000, 0x100020, 0x80008020, 0, 0x80000000, 0x8000, 0x108020, 0x80100000, 0x100020, 0x80000020, 0, 0x108000, 0x8020, 0x80108000, 0x80100000, 0x8020,
            0, 0x108020, 0x80100020, 0x100000, 0x80008020, 0x80100000, 0x80108000, 0x8000, 0x80100000, 0x80008000, 0x20, 0x80108020, 0x108020, 0x20, 0x8000, 0x80000000,
            0x8020, 0x80108000, 0x100000, 0x80000020, 0x100020, 0x80008020, 0x80000020, 0x100020, 0x108000, 0, 0x80008000, 0x8020, 0x80000000, 0x80100020, 0x80108020, 0x108000
        };
        private static readonly uint[] SP3 = new uint[] { 
            520, 0x8020200, 0, 0x8020008, 0x8000200, 0, 0x20208, 0x8000200, 0x20008, 0x8000008, 0x8000008, 0x20000, 0x8020208, 0x20008, 0x8020000, 520,
            0x8000000, 8, 0x8020200, 0x200, 0x20200, 0x8020000, 0x8020008, 0x20208, 0x8000208, 0x20200, 0x20000, 0x8000208, 8, 0x8020208, 0x200, 0x8000000,
            0x8020200, 0x8000000, 0x20008, 520, 0x20000, 0x8020200, 0x8000200, 0, 0x200, 0x20008, 0x8020208, 0x8000200, 0x8000008, 0x200, 0, 0x8020008,
            0x8000208, 0x20000, 0x8000000, 0x8020208, 8, 0x20208, 0x20200, 0x8000008, 0x8020000, 0x8000208, 520, 0x8020000, 0x20208, 8, 0x8020008, 0x20200
        };
        private static readonly uint[] SP4 = new uint[] { 
            0x802001, 0x2081, 0x2081, 0x80, 0x802080, 0x800081, 0x800001, 0x2001, 0, 0x802000, 0x802000, 0x802081, 0x81, 0, 0x800080, 0x800001,
            1, 0x2000, 0x800000, 0x802001, 0x80, 0x800000, 0x2001, 0x2080, 0x800081, 1, 0x2080, 0x800080, 0x2000, 0x802080, 0x802081, 0x81,
            0x800080, 0x800001, 0x802000, 0x802081, 0x81, 0, 0, 0x802000, 0x2080, 0x800080, 0x800081, 1, 0x802001, 0x2081, 0x2081, 0x80,
            0x802081, 0x81, 1, 0x2000, 0x800001, 0x2001, 0x802080, 0x800081, 0x2001, 0x2080, 0x800000, 0x802001, 0x80, 0x800000, 0x2000, 0x802080
        };
        private static readonly uint[] SP5 = new uint[] { 
            0x100, 0x2080100, 0x2080000, 0x42000100, 0x80000, 0x100, 0x40000000, 0x2080000, 0x40080100, 0x80000, 0x2000100, 0x40080100, 0x42000100, 0x42080000, 0x80100, 0x40000000,
            0x2000000, 0x40080000, 0x40080000, 0, 0x40000100, 0x42080100, 0x42080100, 0x2000100, 0x42080000, 0x40000100, 0, 0x42000000, 0x2080100, 0x2000000, 0x42000000, 0x80100,
            0x80000, 0x42000100, 0x100, 0x2000000, 0x40000000, 0x2080000, 0x42000100, 0x40080100, 0x2000100, 0x40000000, 0x42080000, 0x2080100, 0x40080100, 0x100, 0x2000000, 0x42080000,
            0x42080100, 0x80100, 0x42000000, 0x42080100, 0x2080000, 0, 0x40080000, 0x42000000, 0x80100, 0x2000100, 0x40000100, 0x80000, 0, 0x40080000, 0x2080100, 0x40000100
        };
        private static readonly uint[] SP6 = new uint[] { 
            0x20000010, 0x20400000, 0x4000, 0x20404010, 0x20400000, 0x10, 0x20404010, 0x400000, 0x20004000, 0x404010, 0x400000, 0x20000010, 0x400010, 0x20004000, 0x20000000, 0x4010,
            0, 0x400010, 0x20004010, 0x4000, 0x404000, 0x20004010, 0x10, 0x20400010, 0x20400010, 0, 0x404010, 0x20404000, 0x4010, 0x404000, 0x20404000, 0x20000000,
            0x20004000, 0x10, 0x20400010, 0x404000, 0x20404010, 0x400000, 0x4010, 0x20000010, 0x400000, 0x20004000, 0x20000000, 0x4010, 0x20000010, 0x20404010, 0x404000, 0x20400000,
            0x404010, 0x20404000, 0, 0x20400010, 0x10, 0x4000, 0x20400000, 0x404010, 0x4000, 0x400010, 0x20004010, 0, 0x20404000, 0x20000000, 0x400010, 0x20004010
        };
        private static readonly uint[] SP7 = new uint[] { 
            0x200000, 0x4200002, 0x4000802, 0, 0x800, 0x4000802, 0x200802, 0x4200800, 0x4200802, 0x200000, 0, 0x4000002, 2, 0x4000000, 0x4200002, 0x802,
            0x4000800, 0x200802, 0x200002, 0x4000800, 0x4000002, 0x4200000, 0x4200800, 0x200002, 0x4200000, 0x800, 0x802, 0x4200802, 0x200800, 2, 0x4000000, 0x200800,
            0x4000000, 0x200800, 0x200000, 0x4000802, 0x4000802, 0x4200002, 0x4200002, 2, 0x200002, 0x4000000, 0x4000800, 0x200000, 0x4200800, 0x802, 0x200802, 0x4200800,
            0x802, 0x4000002, 0x4200802, 0x4200000, 0x200800, 0, 2, 0x4200802, 0, 0x200802, 0x4200000, 0x800, 0x4000002, 0x4000800, 0x800, 0x200002
        };
        private static readonly uint[] SP8 = new uint[] { 
            0x10001040, 0x1000, 0x40000, 0x10041040, 0x10000000, 0x10001040, 0x40, 0x10000000, 0x40040, 0x10040000, 0x10041040, 0x41000, 0x10041000, 0x41040, 0x1000, 0x40,
            0x10040000, 0x10000040, 0x10001000, 0x1040, 0x41000, 0x40040, 0x10040040, 0x10041000, 0x1040, 0, 0, 0x10040040, 0x10000040, 0x10001000, 0x41040, 0x40000,
            0x41040, 0x40000, 0x10041000, 0x1000, 0x40, 0x10040040, 0x1000, 0x41040, 0x10001000, 0x40, 0x10000040, 0x10040000, 0x10040040, 0x10000000, 0x40000, 0x10001040,
            0, 0x10041040, 0x40040, 0x10000040, 0x10040000, 0x10001000, 0x10001040, 0, 0x10041040, 0x41000, 0x41000, 0x1040, 0x1040, 0x40040, 0x10000000, 0x10041000
        };

        internal static void DesFunc(int[] wKey, byte[] input, int inOff, byte[] outBytes, int outOff)
        {
            uint n = Pack.BE_To_UInt32(input, inOff);
            uint num2 = Pack.BE_To_UInt32(input, inOff + 4);
            uint num3 = ((n >> 4) ^ num2) & 0xf0f0f0f;
            num2 ^= num3;
            n ^= num3 << 4;
            num3 = ((n >> 0x10) ^ num2) & 0xffff;
            num2 ^= num3;
            n ^= num3 << 0x10;
            num3 = ((num2 >> 2) ^ n) & 0x33333333;
            n ^= num3;
            num2 ^= num3 << 2;
            num3 = ((num2 >> 8) ^ n) & 0xff00ff;
            n ^= num3;
            num2 ^= num3 << 8;
            num2 = (num2 << 1) | (num2 >> 0x1f);
            num3 = (n ^ num2) & 0xaaaaaaaa;
            n ^= num3;
            num2 ^= num3;
            n = (n << 1) | (n >> 0x1f);
            for (int i = 0; i < 8; i++)
            {
                num3 = (num2 << 0x1c) | (num2 >> 4);
                num3 ^= (uint) wKey[i * 4];
                uint num5 = SP7[(int) ((IntPtr) (num3 & 0x3f))];
                num5 |= SP5[(int) ((IntPtr) ((num3 >> 8) & 0x3f))];
                num5 |= SP3[(int) ((IntPtr) ((num3 >> 0x10) & 0x3f))];
                num5 |= SP1[(int) ((IntPtr) ((num3 >> 0x18) & 0x3f))];
                num3 = num2 ^ ((uint) wKey[(i * 4) + 1]);
                num5 |= SP8[(int) ((IntPtr) (num3 & 0x3f))];
                num5 |= SP6[(int) ((IntPtr) ((num3 >> 8) & 0x3f))];
                num5 |= SP4[(int) ((IntPtr) ((num3 >> 0x10) & 0x3f))];
                num5 |= SP2[(int) ((IntPtr) ((num3 >> 0x18) & 0x3f))];
                n ^= num5;
                num3 = (n << 0x1c) | (n >> 4);
                num3 ^= (uint) wKey[(i * 4) + 2];
                num5 = SP7[(int) ((IntPtr) (num3 & 0x3f))];
                num5 |= SP5[(int) ((IntPtr) ((num3 >> 8) & 0x3f))];
                num5 |= SP3[(int) ((IntPtr) ((num3 >> 0x10) & 0x3f))];
                num5 |= SP1[(int) ((IntPtr) ((num3 >> 0x18) & 0x3f))];
                num3 = n ^ ((uint) wKey[(i * 4) + 3]);
                num5 |= SP8[(int) ((IntPtr) (num3 & 0x3f))];
                num5 |= SP6[(int) ((IntPtr) ((num3 >> 8) & 0x3f))];
                num5 |= SP4[(int) ((IntPtr) ((num3 >> 0x10) & 0x3f))];
                num5 |= SP2[(int) ((IntPtr) ((num3 >> 0x18) & 0x3f))];
                num2 ^= num5;
            }
            num2 = (num2 << 0x1f) | (num2 >> 1);
            num3 = (n ^ num2) & 0xaaaaaaaa;
            n ^= num3;
            num2 ^= num3;
            n = (n << 0x1f) | (n >> 1);
            num3 = ((n >> 8) ^ num2) & 0xff00ff;
            num2 ^= num3;
            n ^= num3 << 8;
            num3 = ((n >> 2) ^ num2) & 0x33333333;
            num2 ^= num3;
            n ^= num3 << 2;
            num3 = ((num2 >> 0x10) ^ n) & 0xffff;
            n ^= num3;
            num2 ^= num3 << 0x10;
            num3 = ((num2 >> 4) ^ n) & 0xf0f0f0f;
            n ^= num3;
            num2 ^= num3 << 4;
            Pack.UInt32_To_BE(num2, outBytes, outOff);
            Pack.UInt32_To_BE(n, outBytes, outOff + 4);
        }

        protected static int[] GenerateWorkingKey(bool encrypting, byte[] key)
        {
            int[] numArray = new int[0x20];
            bool[] flagArray = new bool[0x38];
            bool[] flagArray2 = new bool[0x38];
            for (int i = 0; i < 0x38; i++)
            {
                int num2 = pc1[i];
                flagArray[i] = (key[num2 >> 3] & bytebit[num2 & 7]) != 0;
            }
            for (int j = 0; j < 0x10; j++)
            {
                int num4;
                int num5;
                if (encrypting)
                {
                    num5 = j << 1;
                }
                else
                {
                    num5 = (15 - j) << 1;
                }
                int index = num5 + 1;
                numArray[num5] = numArray[index] = 0;
                for (int m = 0; m < 0x1c; m++)
                {
                    num4 = m + totrot[j];
                    if (num4 < 0x1c)
                    {
                        flagArray2[m] = flagArray[num4];
                    }
                    else
                    {
                        flagArray2[m] = flagArray[num4 - 0x1c];
                    }
                }
                for (int n = 0x1c; n < 0x38; n++)
                {
                    num4 = n + totrot[j];
                    if (num4 < 0x38)
                    {
                        flagArray2[n] = flagArray[num4];
                    }
                    else
                    {
                        flagArray2[n] = flagArray[num4 - 0x1c];
                    }
                }
                for (int num10 = 0; num10 < 0x18; num10++)
                {
                    if (flagArray2[pc2[num10]])
                    {
                        numArray[num5] |= bigbyte[num10];
                    }
                    if (flagArray2[pc2[num10 + 0x18]])
                    {
                        numArray[index] |= bigbyte[num10];
                    }
                }
            }
            for (int k = 0; k != 0x20; k += 2)
            {
                int num12 = numArray[k];
                int num13 = numArray[k + 1];
                numArray[k] = ((((num12 & 0xfc0000) << 6) | ((num12 & 0xfc0) << 10)) | ((num13 & 0xfc0000) >> 10)) | ((num13 & 0xfc0) >> 6);
                numArray[k + 1] = ((((num12 & 0x3f000) << 12) | ((num12 & 0x3f) << 0x10)) | ((num13 & 0x3f000) >> 4)) | (num13 & 0x3f);
            }
            return numArray;
        }

        public virtual int GetBlockSize() => 
            8;

        public virtual int[] GetWorkingKey() => 
            this.workingKey;

        public virtual void Init(bool forEncryption, ICipherParameters parameters)
        {
            if (!(parameters is KeyParameter))
            {
                throw new ArgumentException("invalid parameter passed to DES init - " + Platform.GetTypeName(parameters));
            }
            this.workingKey = GenerateWorkingKey(forEncryption, ((KeyParameter) parameters).GetKey());
        }

        public virtual int ProcessBlock(byte[] input, int inOff, byte[] output, int outOff)
        {
            if (this.workingKey == null)
            {
                throw new InvalidOperationException("DES engine not initialised");
            }
            Check.DataLength(input, inOff, 8, "input buffer too short");
            Check.OutputLength(output, outOff, 8, "output buffer too short");
            DesFunc(this.workingKey, input, inOff, output, outOff);
            return 8;
        }

        public virtual void Reset()
        {
        }

        public virtual string AlgorithmName =>
            "DES";

        public virtual bool IsPartialBlockOkay =>
            false;
    }
}

