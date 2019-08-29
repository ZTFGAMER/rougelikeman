namespace Org.BouncyCastle.Crypto.Engines
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Utilities;
    using System;

    public class RC6Engine : IBlockCipher
    {
        private static readonly int wordSize = 0x20;
        private static readonly int bytesPerWord = (wordSize / 8);
        private static readonly int _noRounds = 20;
        private int[] _S;
        private static readonly int P32 = -1209970333;
        private static readonly int Q32 = -1640531527;
        private static readonly int LGW = 5;
        private bool forEncryption;

        private int BytesToWord(byte[] src, int srcOff)
        {
            int num = 0;
            for (int i = bytesPerWord - 1; i >= 0; i--)
            {
                num = (num << 8) + (src[i + srcOff] & 0xff);
            }
            return num;
        }

        private int DecryptBlock(byte[] input, int inOff, byte[] outBytes, int outOff)
        {
            int x = this.BytesToWord(input, inOff);
            int word = this.BytesToWord(input, inOff + bytesPerWord);
            int num3 = this.BytesToWord(input, inOff + (bytesPerWord * 2));
            int num4 = this.BytesToWord(input, inOff + (bytesPerWord * 3));
            num3 -= this._S[(2 * _noRounds) + 3];
            x -= this._S[(2 * _noRounds) + 2];
            for (int i = _noRounds; i >= 1; i--)
            {
                int num6 = 0;
                int num7 = 0;
                int num8 = num4;
                num4 = num3;
                num3 = word;
                word = x;
                x = num8;
                num6 = word * ((2 * word) + 1);
                num6 = this.RotateLeft(num6, LGW);
                num7 = num4 * ((2 * num4) + 1);
                num7 = this.RotateLeft(num7, LGW);
                num3 -= this._S[(2 * i) + 1];
                num3 = this.RotateRight(num3, num6) ^ num7;
                x -= this._S[2 * i];
                x = this.RotateRight(x, num7) ^ num6;
            }
            num4 -= this._S[1];
            word -= this._S[0];
            this.WordToBytes(x, outBytes, outOff);
            this.WordToBytes(word, outBytes, outOff + bytesPerWord);
            this.WordToBytes(num3, outBytes, outOff + (bytesPerWord * 2));
            this.WordToBytes(num4, outBytes, outOff + (bytesPerWord * 3));
            return (4 * bytesPerWord);
        }

        private int EncryptBlock(byte[] input, int inOff, byte[] outBytes, int outOff)
        {
            int x = this.BytesToWord(input, inOff);
            int word = this.BytesToWord(input, inOff + bytesPerWord);
            int num3 = this.BytesToWord(input, inOff + (bytesPerWord * 2));
            int num4 = this.BytesToWord(input, inOff + (bytesPerWord * 3));
            word += this._S[0];
            num4 += this._S[1];
            for (int i = 1; i <= _noRounds; i++)
            {
                int num6 = 0;
                int num7 = 0;
                num6 = word * ((2 * word) + 1);
                num6 = this.RotateLeft(num6, 5);
                num7 = num4 * ((2 * num4) + 1);
                num7 = this.RotateLeft(num7, 5);
                x ^= num6;
                x = this.RotateLeft(x, num7) + this._S[2 * i];
                num3 ^= num7;
                num3 = this.RotateLeft(num3, num6) + this._S[(2 * i) + 1];
                int num8 = x;
                x = word;
                word = num3;
                num3 = num4;
                num4 = num8;
            }
            x += this._S[(2 * _noRounds) + 2];
            num3 += this._S[(2 * _noRounds) + 3];
            this.WordToBytes(x, outBytes, outOff);
            this.WordToBytes(word, outBytes, outOff + bytesPerWord);
            this.WordToBytes(num3, outBytes, outOff + (bytesPerWord * 2));
            this.WordToBytes(num4, outBytes, outOff + (bytesPerWord * 3));
            return (4 * bytesPerWord);
        }

        public virtual int GetBlockSize() => 
            (4 * bytesPerWord);

        public virtual void Init(bool forEncryption, ICipherParameters parameters)
        {
            if (!(parameters is KeyParameter))
            {
                throw new ArgumentException("invalid parameter passed to RC6 init - " + Platform.GetTypeName(parameters));
            }
            this.forEncryption = forEncryption;
            this.SetKey(((KeyParameter) parameters).GetKey());
        }

        public virtual int ProcessBlock(byte[] input, int inOff, byte[] output, int outOff)
        {
            int blockSize = this.GetBlockSize();
            if (this._S == null)
            {
                throw new InvalidOperationException("RC6 engine not initialised");
            }
            Check.DataLength(input, inOff, blockSize, "input buffer too short");
            Check.OutputLength(output, outOff, blockSize, "output buffer too short");
            return (!this.forEncryption ? this.DecryptBlock(input, inOff, output, outOff) : this.EncryptBlock(input, inOff, output, outOff));
        }

        public virtual void Reset()
        {
        }

        private int RotateLeft(int x, int y) => 
            ((x << (y & (wordSize - 1))) | (x >> (wordSize - (y & (wordSize - 1)))));

        private int RotateRight(int x, int y) => 
            ((x >> (y & (wordSize - 1))) | (x << (wordSize - (y & (wordSize - 1)))));

        private void SetKey(byte[] key)
        {
            int num4;
            if (((key.Length + (bytesPerWord - 1)) / bytesPerWord) == 0)
            {
            }
            int[] numArray = new int[((key.Length + bytesPerWord) - 1) / bytesPerWord];
            for (int i = key.Length - 1; i >= 0; i--)
            {
                numArray[i / bytesPerWord] = (numArray[i / bytesPerWord] << 8) + (key[i] & 0xff);
            }
            this._S = new int[(2 + (2 * _noRounds)) + 2];
            this._S[0] = P32;
            for (int j = 1; j < this._S.Length; j++)
            {
                this._S[j] = this._S[j - 1] + Q32;
            }
            if (numArray.Length > this._S.Length)
            {
                num4 = 3 * numArray.Length;
            }
            else
            {
                num4 = 3 * this._S.Length;
            }
            int num5 = 0;
            int num6 = 0;
            int index = 0;
            int num8 = 0;
            for (int k = 0; k < num4; k++)
            {
                num5 = this._S[index] = this.RotateLeft((this._S[index] + num5) + num6, 3);
                num6 = numArray[num8] = this.RotateLeft((numArray[num8] + num5) + num6, num5 + num6);
                index = (index + 1) % this._S.Length;
                num8 = (num8 + 1) % numArray.Length;
            }
        }

        private void WordToBytes(int word, byte[] dst, int dstOff)
        {
            for (int i = 0; i < bytesPerWord; i++)
            {
                dst[i + dstOff] = (byte) word;
                word = word >> 8;
            }
        }

        public virtual string AlgorithmName =>
            "RC6";

        public virtual bool IsPartialBlockOkay =>
            false;
    }
}

