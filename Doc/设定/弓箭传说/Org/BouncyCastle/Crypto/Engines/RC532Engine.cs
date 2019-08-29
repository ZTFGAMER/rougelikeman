namespace Org.BouncyCastle.Crypto.Engines
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Utilities;
    using System;

    public class RC532Engine : IBlockCipher
    {
        private int _noRounds = 12;
        private int[] _S;
        private static readonly int P32 = -1209970333;
        private static readonly int Q32 = -1640531527;
        private bool forEncryption;

        private int BytesToWord(byte[] src, int srcOff) => 
            ((((src[srcOff] & 0xff) | ((src[srcOff + 1] & 0xff) << 8)) | ((src[srcOff + 2] & 0xff) << 0x10)) | ((src[srcOff + 3] & 0xff) << 0x18));

        private int DecryptBlock(byte[] input, int inOff, byte[] outBytes, int outOff)
        {
            int y = this.BytesToWord(input, inOff);
            int num2 = this.BytesToWord(input, inOff + 4);
            for (int i = this._noRounds; i >= 1; i--)
            {
                num2 = this.RotateRight(num2 - this._S[(2 * i) + 1], y) ^ y;
                y = this.RotateRight(y - this._S[2 * i], num2) ^ num2;
            }
            this.WordToBytes(y - this._S[0], outBytes, outOff);
            this.WordToBytes(num2 - this._S[1], outBytes, outOff + 4);
            return 8;
        }

        private int EncryptBlock(byte[] input, int inOff, byte[] outBytes, int outOff)
        {
            int y = this.BytesToWord(input, inOff) + this._S[0];
            int num2 = this.BytesToWord(input, inOff + 4) + this._S[1];
            for (int i = 1; i <= this._noRounds; i++)
            {
                y = this.RotateLeft(y ^ num2, num2) + this._S[2 * i];
                num2 = this.RotateLeft(num2 ^ y, y) + this._S[(2 * i) + 1];
            }
            this.WordToBytes(y, outBytes, outOff);
            this.WordToBytes(num2, outBytes, outOff + 4);
            return 8;
        }

        public virtual int GetBlockSize() => 
            8;

        public virtual void Init(bool forEncryption, ICipherParameters parameters)
        {
            if (typeof(RC5Parameters).IsInstanceOfType(parameters))
            {
                RC5Parameters parameters2 = (RC5Parameters) parameters;
                this._noRounds = parameters2.Rounds;
                this.SetKey(parameters2.GetKey());
            }
            else
            {
                if (!typeof(KeyParameter).IsInstanceOfType(parameters))
                {
                    throw new ArgumentException("invalid parameter passed to RC532 init - " + Platform.GetTypeName(parameters));
                }
                this.SetKey(((KeyParameter) parameters).GetKey());
            }
            this.forEncryption = forEncryption;
        }

        public virtual int ProcessBlock(byte[] input, int inOff, byte[] output, int outOff) => 
            (!this.forEncryption ? this.DecryptBlock(input, inOff, output, outOff) : this.EncryptBlock(input, inOff, output, outOff));

        public virtual void Reset()
        {
        }

        private int RotateLeft(int x, int y) => 
            ((x << y) | (x >> (0x20 - (y & 0x1f))));

        private int RotateRight(int x, int y) => 
            ((x >> y) | (x << (0x20 - (y & 0x1f))));

        private void SetKey(byte[] key)
        {
            int num3;
            int[] numArray = new int[(key.Length + 3) / 4];
            for (int i = 0; i != key.Length; i++)
            {
                numArray[i / 4] += (key[i] & 0xff) << (8 * (i % 4));
            }
            this._S = new int[2 * (this._noRounds + 1)];
            this._S[0] = P32;
            for (int j = 1; j < this._S.Length; j++)
            {
                this._S[j] = this._S[j - 1] + Q32;
            }
            if (numArray.Length > this._S.Length)
            {
                num3 = 3 * numArray.Length;
            }
            else
            {
                num3 = 3 * this._S.Length;
            }
            int num4 = 0;
            int num5 = 0;
            int index = 0;
            int num7 = 0;
            for (int k = 0; k < num3; k++)
            {
                num4 = this._S[index] = this.RotateLeft((this._S[index] + num4) + num5, 3);
                num5 = numArray[num7] = this.RotateLeft((numArray[num7] + num4) + num5, num4 + num5);
                index = (index + 1) % this._S.Length;
                num7 = (num7 + 1) % numArray.Length;
            }
        }

        private void WordToBytes(int word, byte[] dst, int dstOff)
        {
            dst[dstOff] = (byte) word;
            dst[dstOff + 1] = (byte) (word >> 8);
            dst[dstOff + 2] = (byte) (word >> 0x10);
            dst[dstOff + 3] = (byte) (word >> 0x18);
        }

        public virtual string AlgorithmName =>
            "RC5-32";

        public virtual bool IsPartialBlockOkay =>
            false;
    }
}

