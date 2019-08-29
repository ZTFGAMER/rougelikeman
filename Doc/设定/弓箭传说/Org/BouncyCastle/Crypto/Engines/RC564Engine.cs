namespace Org.BouncyCastle.Crypto.Engines
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Utilities;
    using System;

    public class RC564Engine : IBlockCipher
    {
        private static readonly int wordSize = 0x40;
        private static readonly int bytesPerWord = (wordSize / 8);
        private int _noRounds = 12;
        private long[] _S;
        private static readonly long P64 = -5196783011329398165L;
        private static readonly long Q64 = -7046029254386353131L;
        private bool forEncryption;

        private long BytesToWord(byte[] src, int srcOff)
        {
            long num = 0L;
            for (int i = bytesPerWord - 1; i >= 0; i--)
            {
                num = (num << 8) + (src[i + srcOff] & 0xff);
            }
            return num;
        }

        private int DecryptBlock(byte[] input, int inOff, byte[] outBytes, int outOff)
        {
            long y = this.BytesToWord(input, inOff);
            long num2 = this.BytesToWord(input, inOff + bytesPerWord);
            for (int i = this._noRounds; i >= 1; i--)
            {
                num2 = this.RotateRight(num2 - this._S[(2 * i) + 1], y) ^ y;
                y = this.RotateRight(y - this._S[2 * i], num2) ^ num2;
            }
            this.WordToBytes(y - this._S[0], outBytes, outOff);
            this.WordToBytes(num2 - this._S[1], outBytes, outOff + bytesPerWord);
            return (2 * bytesPerWord);
        }

        private int EncryptBlock(byte[] input, int inOff, byte[] outBytes, int outOff)
        {
            long y = this.BytesToWord(input, inOff) + this._S[0];
            long num2 = this.BytesToWord(input, inOff + bytesPerWord) + this._S[1];
            for (int i = 1; i <= this._noRounds; i++)
            {
                y = this.RotateLeft(y ^ num2, num2) + this._S[2 * i];
                num2 = this.RotateLeft(num2 ^ y, y) + this._S[(2 * i) + 1];
            }
            this.WordToBytes(y, outBytes, outOff);
            this.WordToBytes(num2, outBytes, outOff + bytesPerWord);
            return (2 * bytesPerWord);
        }

        public virtual int GetBlockSize() => 
            (2 * bytesPerWord);

        public virtual void Init(bool forEncryption, ICipherParameters parameters)
        {
            if (!typeof(RC5Parameters).IsInstanceOfType(parameters))
            {
                throw new ArgumentException("invalid parameter passed to RC564 init - " + Platform.GetTypeName(parameters));
            }
            RC5Parameters parameters2 = (RC5Parameters) parameters;
            this.forEncryption = forEncryption;
            this._noRounds = parameters2.Rounds;
            this.SetKey(parameters2.GetKey());
        }

        public virtual int ProcessBlock(byte[] input, int inOff, byte[] output, int outOff) => 
            (!this.forEncryption ? this.DecryptBlock(input, inOff, output, outOff) : this.EncryptBlock(input, inOff, output, outOff));

        public virtual void Reset()
        {
        }

        private long RotateLeft(long x, long y) => 
            ((x << (((int) y) & (wordSize - 1))) | (x >> (wordSize - (((int) y) & (wordSize - 1)))));

        private long RotateRight(long x, long y) => 
            ((x >> (((int) y) & (wordSize - 1))) | (x << (wordSize - (((int) y) & (wordSize - 1)))));

        private void SetKey(byte[] key)
        {
            int num3;
            long[] numArray = new long[(key.Length + (bytesPerWord - 1)) / bytesPerWord];
            for (int i = 0; i != key.Length; i++)
            {
                numArray[i / bytesPerWord] += (key[i] & 0xff) << (8 * (i % bytesPerWord));
            }
            this._S = new long[2 * (this._noRounds + 1)];
            this._S[0] = P64;
            for (int j = 1; j < this._S.Length; j++)
            {
                this._S[j] = this._S[j - 1] + Q64;
            }
            if (numArray.Length > this._S.Length)
            {
                num3 = 3 * numArray.Length;
            }
            else
            {
                num3 = 3 * this._S.Length;
            }
            long num4 = 0L;
            long num5 = 0L;
            int index = 0;
            int num7 = 0;
            for (int k = 0; k < num3; k++)
            {
                num4 = this._S[index] = this.RotateLeft((this._S[index] + num4) + num5, 3L);
                num5 = numArray[num7] = this.RotateLeft((numArray[num7] + num4) + num5, num4 + num5);
                index = (index + 1) % this._S.Length;
                num7 = (num7 + 1) % numArray.Length;
            }
        }

        private void WordToBytes(long word, byte[] dst, int dstOff)
        {
            for (int i = 0; i < bytesPerWord; i++)
            {
                dst[i + dstOff] = (byte) word;
                word = word >> 8;
            }
        }

        public virtual string AlgorithmName =>
            "RC5-64";

        public virtual bool IsPartialBlockOkay =>
            false;
    }
}

