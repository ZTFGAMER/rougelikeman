namespace Org.BouncyCastle.Crypto.Digests
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Utilities;
    using System;

    public sealed class WhirlpoolDigest : IDigest, IMemoable
    {
        private const int BYTE_LENGTH = 0x40;
        private const int DIGEST_LENGTH_BYTES = 0x40;
        private const int ROUNDS = 10;
        private const int REDUCTION_POLYNOMIAL = 0x11d;
        private static readonly int[] SBOX = new int[] { 
            0x18, 0x23, 0xc6, 0xe8, 0x87, 0xb8, 1, 0x4f, 0x36, 0xa6, 210, 0xf5, 0x79, 0x6f, 0x91, 0x52,
            0x60, 0xbc, 0x9b, 0x8e, 0xa3, 12, 0x7b, 0x35, 0x1d, 0xe0, 0xd7, 0xc2, 0x2e, 0x4b, 0xfe, 0x57,
            0x15, 0x77, 0x37, 0xe5, 0x9f, 240, 0x4a, 0xda, 0x58, 0xc9, 0x29, 10, 0xb1, 160, 0x6b, 0x85,
            0xbd, 0x5d, 0x10, 0xf4, 0xcb, 0x3e, 5, 0x67, 0xe4, 0x27, 0x41, 0x8b, 0xa7, 0x7d, 0x95, 0xd8,
            0xfb, 0xee, 0x7c, 0x66, 0xdd, 0x17, 0x47, 0x9e, 0xca, 0x2d, 0xbf, 7, 0xad, 90, 0x83, 0x33,
            0x63, 2, 170, 0x71, 200, 0x19, 0x49, 0xd9, 0xf2, 0xe3, 0x5b, 0x88, 0x9a, 0x26, 50, 0xb0,
            0xe9, 15, 0xd5, 0x80, 190, 0xcd, 0x34, 0x48, 0xff, 0x7a, 0x90, 0x5f, 0x20, 0x68, 0x1a, 0xae,
            180, 0x54, 0x93, 0x22, 100, 0xf1, 0x73, 0x12, 0x40, 8, 0xc3, 0xec, 0xdb, 0xa1, 0x8d, 0x3d,
            0x97, 0, 0xcf, 0x2b, 0x76, 130, 0xd6, 0x1b, 0xb5, 0xaf, 0x6a, 80, 0x45, 0xf3, 0x30, 0xef,
            0x3f, 0x55, 0xa2, 0xea, 0x65, 0xba, 0x2f, 0xc0, 0xde, 0x1c, 0xfd, 0x4d, 0x92, 0x75, 6, 0x8a,
            0xb2, 230, 14, 0x1f, 0x62, 0xd4, 0xa8, 150, 0xf9, 0xc5, 0x25, 0x59, 0x84, 0x72, 0x39, 0x4c,
            0x5e, 120, 0x38, 140, 0xd1, 0xa5, 0xe2, 0x61, 0xb3, 0x21, 0x9c, 30, 0x43, 0xc7, 0xfc, 4,
            0x51, 0x99, 0x6d, 13, 250, 0xdf, 0x7e, 0x24, 0x3b, 0xab, 0xce, 0x11, 0x8f, 0x4e, 0xb7, 0xeb,
            60, 0x81, 0x94, 0xf7, 0xb9, 0x13, 0x2c, 0xd3, 0xe7, 110, 0xc4, 3, 0x56, 0x44, 0x7f, 0xa9,
            0x2a, 0xbb, 0xc1, 0x53, 220, 11, 0x9d, 0x6c, 0x31, 0x74, 0xf6, 70, 0xac, 0x89, 20, 0xe1,
            0x16, 0x3a, 0x69, 9, 0x70, 0xb6, 0xd0, 0xed, 0xcc, 0x42, 0x98, 0xa4, 40, 0x5c, 0xf8, 0x86
        };
        private static readonly long[] C0 = new long[0x100];
        private static readonly long[] C1 = new long[0x100];
        private static readonly long[] C2 = new long[0x100];
        private static readonly long[] C3 = new long[0x100];
        private static readonly long[] C4 = new long[0x100];
        private static readonly long[] C5 = new long[0x100];
        private static readonly long[] C6 = new long[0x100];
        private static readonly long[] C7 = new long[0x100];
        private readonly long[] _rc;
        private static readonly short[] EIGHT = new short[0x20];
        private const int BITCOUNT_ARRAY_SIZE = 0x20;
        private byte[] _buffer;
        private int _bufferPos;
        private short[] _bitCount;
        private long[] _hash;
        private long[] _K;
        private long[] _L;
        private long[] _block;
        private long[] _state;

        static WhirlpoolDigest()
        {
            EIGHT[0x1f] = 8;
            for (int i = 0; i < 0x100; i++)
            {
                int num2 = SBOX[i];
                int num3 = maskWithReductionPolynomial(num2 << 1);
                int num4 = maskWithReductionPolynomial(num3 << 1);
                int num5 = num4 ^ num2;
                int num6 = maskWithReductionPolynomial(num4 << 1);
                int num7 = num6 ^ num2;
                C0[i] = packIntoLong(num2, num2, num4, num2, num6, num5, num3, num7);
                C1[i] = packIntoLong(num7, num2, num2, num4, num2, num6, num5, num3);
                C2[i] = packIntoLong(num3, num7, num2, num2, num4, num2, num6, num5);
                C3[i] = packIntoLong(num5, num3, num7, num2, num2, num4, num2, num6);
                C4[i] = packIntoLong(num6, num5, num3, num7, num2, num2, num4, num2);
                C5[i] = packIntoLong(num2, num6, num5, num3, num7, num2, num2, num4);
                C6[i] = packIntoLong(num4, num2, num6, num5, num3, num7, num2, num2);
                C7[i] = packIntoLong(num2, num4, num2, num6, num5, num3, num7, num2);
            }
        }

        public WhirlpoolDigest()
        {
            this._rc = new long[11];
            this._buffer = new byte[0x40];
            this._bitCount = new short[0x20];
            this._hash = new long[8];
            this._K = new long[8];
            this._L = new long[8];
            this._block = new long[8];
            this._state = new long[8];
            this._rc[0] = 0L;
            for (int i = 1; i <= 10; i++)
            {
                int index = 8 * (i - 1);
                this._rc[i] = ((((long) (((ulong) ((((C0[index] & -72057594037927936L) ^ (C1[index + 1] & 0xff000000000000L)) ^ (C2[index + 2] & 0xff0000000000L)) ^ (C3[index + 3] & 0xff00000000L))) ^ (((ulong) C4[index + 4]) & 0xff000000L))) ^ (C5[index + 5] & 0xff0000L)) ^ (C6[index + 6] & 0xff00L)) ^ (C7[index + 7] & 0xffL);
            }
        }

        public WhirlpoolDigest(WhirlpoolDigest originalDigest)
        {
            this._rc = new long[11];
            this._buffer = new byte[0x40];
            this._bitCount = new short[0x20];
            this._hash = new long[8];
            this._K = new long[8];
            this._L = new long[8];
            this._block = new long[8];
            this._state = new long[8];
            this.Reset(originalDigest);
        }

        public void BlockUpdate(byte[] input, int inOff, int length)
        {
            while (length > 0)
            {
                this.Update(input[inOff]);
                inOff++;
                length--;
            }
        }

        private static long bytesToLongFromBuffer(byte[] buffer, int startPos) => 
            (((((((((buffer[startPos] & 0xffL) << 0x38) | ((buffer[startPos + 1] & 0xffL) << 0x30)) | ((buffer[startPos + 2] & 0xffL) << 40)) | ((buffer[startPos + 3] & 0xffL) << 0x20)) | ((buffer[startPos + 4] & 0xffL) << 0x18)) | ((buffer[startPos + 5] & 0xffL) << 0x10)) | ((buffer[startPos + 6] & 0xffL) << 8)) | (buffer[startPos + 7] & 0xffL));

        private static void convertLongToByteArray(long inputLong, byte[] outputArray, int offSet)
        {
            for (int i = 0; i < 8; i++)
            {
                outputArray[offSet + i] = (byte) ((inputLong >> (0x38 - (i * 8))) & 0xffL);
            }
        }

        public IMemoable Copy() => 
            new WhirlpoolDigest(this);

        private byte[] copyBitLength()
        {
            byte[] buffer = new byte[0x20];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = (byte) (this._bitCount[i] & 0xff);
            }
            return buffer;
        }

        public int DoFinal(byte[] output, int outOff)
        {
            this.finish();
            for (int i = 0; i < 8; i++)
            {
                convertLongToByteArray(this._hash[i], output, outOff + (i * 8));
            }
            this.Reset();
            return this.GetDigestSize();
        }

        private void finish()
        {
            byte[] sourceArray = this.copyBitLength();
            this._buffer[this._bufferPos++] = (byte) (this._buffer[this._bufferPos++] | 0x80);
            if (this._bufferPos == this._buffer.Length)
            {
                this.processFilledBuffer();
            }
            if (this._bufferPos > 0x20)
            {
                while (this._bufferPos != 0)
                {
                    this.Update(0);
                }
            }
            while (this._bufferPos <= 0x20)
            {
                this.Update(0);
            }
            Array.Copy(sourceArray, 0, this._buffer, 0x20, sourceArray.Length);
            this.processFilledBuffer();
        }

        public int GetByteLength() => 
            0x40;

        public int GetDigestSize() => 
            0x40;

        private void increment()
        {
            int num = 0;
            for (int i = this._bitCount.Length - 1; i >= 0; i--)
            {
                int num3 = ((this._bitCount[i] & 0xff) + EIGHT[i]) + num;
                num = num3 >> 8;
                this._bitCount[i] = (short) (num3 & 0xff);
            }
        }

        private static int maskWithReductionPolynomial(int input)
        {
            int num = input;
            if (num >= 0x100L)
            {
                num ^= 0x11d;
            }
            return num;
        }

        private static long packIntoLong(int b7, int b6, int b5, int b4, int b3, int b2, int b1, int b0) => 
            ((((((((b7 << 0x38) ^ (b6 << 0x30)) ^ (b5 << 40)) ^ (b4 << 0x20)) ^ (b3 << 0x18)) ^ (b2 << 0x10)) ^ (b1 << 8)) ^ b0);

        private void processBlock()
        {
            for (int i = 0; i < 8; i++)
            {
                long num2;
                this._K[i] = num2 = this._hash[i];
                this._state[i] = this._block[i] ^ num2;
            }
            for (int j = 1; j <= 10; j++)
            {
                for (int m = 0; m < 8; m++)
                {
                    this._L[m] = 0L;
                    this._L[m] ^= C0[((int) (this._K[m & 7] >> 0x38)) & 0xff];
                    this._L[m] ^= C1[((int) (this._K[(m - 1) & 7] >> 0x30)) & 0xff];
                    this._L[m] ^= C2[((int) (this._K[(m - 2) & 7] >> 40)) & 0xff];
                    this._L[m] ^= C3[((int) (this._K[(m - 3) & 7] >> 0x20)) & 0xff];
                    this._L[m] ^= C4[((int) (this._K[(m - 4) & 7] >> 0x18)) & 0xff];
                    this._L[m] ^= C5[((int) (this._K[(m - 5) & 7] >> 0x10)) & 0xff];
                    this._L[m] ^= C6[((int) (this._K[(m - 6) & 7] >> 8)) & 0xff];
                    this._L[m] ^= C7[((int) this._K[(m - 7) & 7]) & 0xff];
                }
                Array.Copy(this._L, 0, this._K, 0, this._K.Length);
                this._K[0] ^= this._rc[j];
                for (int n = 0; n < 8; n++)
                {
                    this._L[n] = this._K[n];
                    this._L[n] ^= C0[((int) (this._state[n & 7] >> 0x38)) & 0xff];
                    this._L[n] ^= C1[((int) (this._state[(n - 1) & 7] >> 0x30)) & 0xff];
                    this._L[n] ^= C2[((int) (this._state[(n - 2) & 7] >> 40)) & 0xff];
                    this._L[n] ^= C3[((int) (this._state[(n - 3) & 7] >> 0x20)) & 0xff];
                    this._L[n] ^= C4[((int) (this._state[(n - 4) & 7] >> 0x18)) & 0xff];
                    this._L[n] ^= C5[((int) (this._state[(n - 5) & 7] >> 0x10)) & 0xff];
                    this._L[n] ^= C6[((int) (this._state[(n - 6) & 7] >> 8)) & 0xff];
                    this._L[n] ^= C7[((int) this._state[(n - 7) & 7]) & 0xff];
                }
                Array.Copy(this._L, 0, this._state, 0, this._state.Length);
            }
            for (int k = 0; k < 8; k++)
            {
                this._hash[k] ^= this._state[k] ^ this._block[k];
            }
        }

        private void processFilledBuffer()
        {
            for (int i = 0; i < this._state.Length; i++)
            {
                this._block[i] = bytesToLongFromBuffer(this._buffer, i * 8);
            }
            this.processBlock();
            this._bufferPos = 0;
            Array.Clear(this._buffer, 0, this._buffer.Length);
        }

        public void Reset()
        {
            this._bufferPos = 0;
            Array.Clear(this._bitCount, 0, this._bitCount.Length);
            Array.Clear(this._buffer, 0, this._buffer.Length);
            Array.Clear(this._hash, 0, this._hash.Length);
            Array.Clear(this._K, 0, this._K.Length);
            Array.Clear(this._L, 0, this._L.Length);
            Array.Clear(this._block, 0, this._block.Length);
            Array.Clear(this._state, 0, this._state.Length);
        }

        public void Reset(IMemoable other)
        {
            WhirlpoolDigest digest = (WhirlpoolDigest) other;
            Array.Copy(digest._rc, 0, this._rc, 0, this._rc.Length);
            Array.Copy(digest._buffer, 0, this._buffer, 0, this._buffer.Length);
            this._bufferPos = digest._bufferPos;
            Array.Copy(digest._bitCount, 0, this._bitCount, 0, this._bitCount.Length);
            Array.Copy(digest._hash, 0, this._hash, 0, this._hash.Length);
            Array.Copy(digest._K, 0, this._K, 0, this._K.Length);
            Array.Copy(digest._L, 0, this._L, 0, this._L.Length);
            Array.Copy(digest._block, 0, this._block, 0, this._block.Length);
            Array.Copy(digest._state, 0, this._state, 0, this._state.Length);
        }

        public void Update(byte input)
        {
            this._buffer[this._bufferPos] = input;
            this._bufferPos++;
            if (this._bufferPos == this._buffer.Length)
            {
                this.processFilledBuffer();
            }
            this.increment();
        }

        public string AlgorithmName =>
            "Whirlpool";
    }
}

