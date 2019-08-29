namespace Org.BouncyCastle.Crypto.Macs
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Crypto.Utilities;
    using System;

    public class SipHash : IMac
    {
        protected readonly int c;
        protected readonly int d;
        protected long k0;
        protected long k1;
        protected long v0;
        protected long v1;
        protected long v2;
        protected long v3;
        protected long m;
        protected int wordPos;
        protected int wordCount;

        public SipHash() : this(2, 4)
        {
        }

        public SipHash(int c, int d)
        {
            this.c = c;
            this.d = d;
        }

        protected virtual void ApplySipRounds(int n)
        {
            long x = this.v0;
            long num2 = this.v1;
            long num3 = this.v2;
            long num4 = this.v3;
            for (int i = 0; i < n; i++)
            {
                x += num2;
                num3 += num4;
                num2 = RotateLeft(num2, 13);
                num4 = RotateLeft(num4, 0x10);
                num2 ^= x;
                num4 ^= num3;
                x = RotateLeft(x, 0x20);
                num3 += num2;
                x += num4;
                num2 = RotateLeft(num2, 0x11);
                num4 = RotateLeft(num4, 0x15);
                num2 ^= num3;
                num4 ^= x;
                num3 = RotateLeft(num3, 0x20);
            }
            this.v0 = x;
            this.v1 = num2;
            this.v2 = num3;
            this.v3 = num4;
        }

        public virtual void BlockUpdate(byte[] input, int offset, int length)
        {
            int num = 0;
            int num2 = length & -8;
            if (this.wordPos == 0)
            {
                while (num < num2)
                {
                    this.m = (long) Pack.LE_To_UInt64(input, offset + num);
                    this.ProcessMessageWord();
                    num += 8;
                }
                while (num < length)
                {
                    this.m = (long) (((ulong) (this.m >> 8)) | (input[offset + num] << 0x38));
                    num++;
                }
                this.wordPos = length - num2;
            }
            else
            {
                int num3 = this.wordPos << 3;
                while (num < num2)
                {
                    ulong num4 = Pack.LE_To_UInt64(input, offset + num);
                    this.m = ((long) (num4 << num3)) | (this.m >> -num3);
                    this.ProcessMessageWord();
                    this.m = (long) num4;
                    num += 8;
                }
                while (num < length)
                {
                    this.m = (long) (((ulong) (this.m >> 8)) | (input[offset + num] << 0x38));
                    if (++this.wordPos == 8)
                    {
                        this.ProcessMessageWord();
                        this.wordPos = 0;
                    }
                    num++;
                }
            }
        }

        public virtual long DoFinal()
        {
            this.m = this.m >> ((7 - this.wordPos) << 3);
            this.m = this.m >> 8;
            this.m |= ((this.wordCount << 3) + this.wordPos) << 0x38;
            this.ProcessMessageWord();
            this.v2 ^= 0xffL;
            this.ApplySipRounds(this.d);
            long num = ((this.v0 ^ this.v1) ^ this.v2) ^ this.v3;
            this.Reset();
            return num;
        }

        public virtual int DoFinal(byte[] output, int outOff)
        {
            Pack.UInt64_To_LE((ulong) this.DoFinal(), output, outOff);
            return 8;
        }

        public virtual int GetMacSize() => 
            8;

        public virtual void Init(ICipherParameters parameters)
        {
            KeyParameter parameter = parameters as KeyParameter;
            if (parameter == null)
            {
                throw new ArgumentException("must be an instance of KeyParameter", "parameters");
            }
            byte[] key = parameter.GetKey();
            if (key.Length != 0x10)
            {
                throw new ArgumentException("must be a 128-bit key", "parameters");
            }
            this.k0 = (long) Pack.LE_To_UInt64(key, 0);
            this.k1 = (long) Pack.LE_To_UInt64(key, 8);
            this.Reset();
        }

        protected virtual void ProcessMessageWord()
        {
            this.wordCount++;
            this.v3 ^= this.m;
            this.ApplySipRounds(this.c);
            this.v0 ^= this.m;
        }

        public virtual void Reset()
        {
            this.v0 = this.k0 ^ 0x736f6d6570736575L;
            this.v1 = this.k1 ^ 0x646f72616e646f6dL;
            this.v2 = this.k0 ^ 0x6c7967656e657261L;
            this.v3 = this.k1 ^ 0x7465646279746573L;
            this.m = 0L;
            this.wordPos = 0;
            this.wordCount = 0;
        }

        protected static long RotateLeft(long x, int n)
        {
            ulong num = (ulong) x;
            num = (num << n) | (num >> -n);
            return (long) num;
        }

        public virtual void Update(byte input)
        {
            this.m = (long) (((ulong) (this.m >> 8)) | (input << 0x38));
            if (++this.wordPos == 8)
            {
                this.ProcessMessageWord();
                this.wordPos = 0;
            }
        }

        public virtual string AlgorithmName
        {
            get
            {
                object[] objArray1 = new object[] { "SipHash-", this.c, "-", this.d };
                return string.Concat(objArray1);
            }
        }
    }
}

