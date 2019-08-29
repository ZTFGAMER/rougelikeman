namespace Org.BouncyCastle.Crypto.Engines
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Utilities;
    using System;

    public class IdeaEngine : IBlockCipher
    {
        private const int BLOCK_SIZE = 8;
        private int[] workingKey;
        private static readonly int MASK = 0xffff;
        private static readonly int BASE = 0x10001;

        private int AddInv(int x) => 
            (-x & MASK);

        private int BytesToWord(byte[] input, int inOff) => 
            (((input[inOff] << 8) & 0xff00) + (input[inOff + 1] & 0xff));

        private int[] ExpandKey(byte[] uKey)
        {
            int[] numArray = new int[0x34];
            if (uKey.Length < 0x10)
            {
                byte[] destinationArray = new byte[0x10];
                Array.Copy(uKey, 0, destinationArray, destinationArray.Length - uKey.Length, uKey.Length);
                uKey = destinationArray;
            }
            for (int i = 0; i < 8; i++)
            {
                numArray[i] = this.BytesToWord(uKey, i * 2);
            }
            for (int j = 8; j < 0x34; j++)
            {
                if ((j & 7) < 6)
                {
                    numArray[j] = (((numArray[j - 7] & 0x7f) << 9) | (numArray[j - 6] >> 7)) & MASK;
                }
                else if ((j & 7) == 6)
                {
                    numArray[j] = (((numArray[j - 7] & 0x7f) << 9) | (numArray[j - 14] >> 7)) & MASK;
                }
                else
                {
                    numArray[j] = (((numArray[j - 15] & 0x7f) << 9) | (numArray[j - 14] >> 7)) & MASK;
                }
            }
            return numArray;
        }

        private int[] GenerateWorkingKey(bool forEncryption, byte[] userKey)
        {
            if (forEncryption)
            {
                return this.ExpandKey(userKey);
            }
            return this.InvertKey(this.ExpandKey(userKey));
        }

        public virtual int GetBlockSize() => 
            8;

        private void IdeaFunc(int[] workingKey, byte[] input, int inOff, byte[] outBytes, int outOff)
        {
            int index = 0;
            int x = this.BytesToWord(input, inOff);
            int num2 = this.BytesToWord(input, inOff + 2);
            int num3 = this.BytesToWord(input, inOff + 4);
            int num4 = this.BytesToWord(input, inOff + 6);
            for (int i = 0; i < 8; i++)
            {
                x = this.Mul(x, workingKey[index++]);
                num2 += workingKey[index++];
                num2 &= MASK;
                num3 += workingKey[index++];
                num3 &= MASK;
                num4 = this.Mul(num4, workingKey[index++]);
                int num5 = num2;
                int num6 = num3;
                num3 ^= x;
                num2 ^= num4;
                num3 = this.Mul(num3, workingKey[index++]);
                num2 += num3;
                num2 &= MASK;
                num2 = this.Mul(num2, workingKey[index++]);
                num3 += num2;
                num3 &= MASK;
                x ^= num2;
                num4 ^= num3;
                num2 ^= num6;
                num3 ^= num5;
            }
            this.WordToBytes(this.Mul(x, workingKey[index++]), outBytes, outOff);
            this.WordToBytes(num3 + workingKey[index++], outBytes, outOff + 2);
            this.WordToBytes(num2 + workingKey[index++], outBytes, outOff + 4);
            this.WordToBytes(this.Mul(num4, workingKey[index]), outBytes, outOff + 6);
        }

        public virtual void Init(bool forEncryption, ICipherParameters parameters)
        {
            if (!(parameters is KeyParameter))
            {
                throw new ArgumentException("invalid parameter passed to IDEA init - " + Platform.GetTypeName(parameters));
            }
            this.workingKey = this.GenerateWorkingKey(forEncryption, ((KeyParameter) parameters).GetKey());
        }

        private int[] InvertKey(int[] inKey)
        {
            int num5 = 0x34;
            int[] numArray = new int[0x34];
            int index = 0;
            int num = this.MulInv(inKey[index++]);
            int num2 = this.AddInv(inKey[index++]);
            int num3 = this.AddInv(inKey[index++]);
            int num4 = this.MulInv(inKey[index++]);
            numArray[--num5] = num4;
            numArray[--num5] = num3;
            numArray[--num5] = num2;
            numArray[--num5] = num;
            for (int i = 1; i < 8; i++)
            {
                num = inKey[index++];
                num2 = inKey[index++];
                numArray[--num5] = num2;
                numArray[--num5] = num;
                num = this.MulInv(inKey[index++]);
                num2 = this.AddInv(inKey[index++]);
                num3 = this.AddInv(inKey[index++]);
                num4 = this.MulInv(inKey[index++]);
                numArray[--num5] = num4;
                numArray[--num5] = num2;
                numArray[--num5] = num3;
                numArray[--num5] = num;
            }
            num = inKey[index++];
            num2 = inKey[index++];
            numArray[--num5] = num2;
            numArray[--num5] = num;
            num = this.MulInv(inKey[index++]);
            num2 = this.AddInv(inKey[index++]);
            num3 = this.AddInv(inKey[index++]);
            num4 = this.MulInv(inKey[index]);
            numArray[--num5] = num4;
            numArray[--num5] = num3;
            numArray[--num5] = num2;
            numArray[--num5] = num;
            return numArray;
        }

        private int Mul(int x, int y)
        {
            if (x == 0)
            {
                x = BASE - y;
            }
            else if (y == 0)
            {
                x = BASE - x;
            }
            else
            {
                int num = x * y;
                y = num & MASK;
                x = num >> 0x10;
                x = (y - x) + ((y >= x) ? 0 : 1);
            }
            return (x & MASK);
        }

        private int MulInv(int x)
        {
            if (x < 2)
            {
                return x;
            }
            int num = 1;
            int num2 = BASE / x;
            int num4 = BASE % x;
            while (num4 != 1)
            {
                int num3 = x / num4;
                x = x % num4;
                num = (num + (num2 * num3)) & MASK;
                if (x == 1)
                {
                    return num;
                }
                num3 = num4 / x;
                num4 = num4 % x;
                num2 = (num2 + (num * num3)) & MASK;
            }
            return ((1 - num2) & MASK);
        }

        public virtual int ProcessBlock(byte[] input, int inOff, byte[] output, int outOff)
        {
            if (this.workingKey == null)
            {
                throw new InvalidOperationException("IDEA engine not initialised");
            }
            Check.DataLength(input, inOff, 8, "input buffer too short");
            Check.OutputLength(output, outOff, 8, "output buffer too short");
            this.IdeaFunc(this.workingKey, input, inOff, output, outOff);
            return 8;
        }

        public virtual void Reset()
        {
        }

        private void WordToBytes(int word, byte[] outBytes, int outOff)
        {
            outBytes[outOff] = (byte) (word >> 8);
            outBytes[outOff + 1] = (byte) word;
        }

        public virtual string AlgorithmName =>
            "IDEA";

        public virtual bool IsPartialBlockOkay =>
            false;
    }
}

