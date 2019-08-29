namespace Org.BouncyCastle.Security
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Prng;
    using Org.BouncyCastle.Utilities;
    using System;
    using System.Threading;

    public class SecureRandom : Random
    {
        private static long counter = Times.NanoTime();
        private static readonly SecureRandom master = new SecureRandom(new CryptoApiRandomGenerator());
        protected readonly IRandomGenerator generator;
        private static readonly double DoubleScale = Math.Pow(2.0, 64.0);

        public SecureRandom() : this(CreatePrng("SHA256", true))
        {
        }

        [Obsolete("Use GetInstance/SetSeed instead")]
        public SecureRandom(byte[] seed) : this(CreatePrng("SHA1", false))
        {
            this.SetSeed(seed);
        }

        public SecureRandom(IRandomGenerator generator) : base(0)
        {
            this.generator = generator;
        }

        private static DigestRandomGenerator CreatePrng(string digestName, bool autoSeed)
        {
            IDigest digest = DigestUtilities.GetDigest(digestName);
            if (digest == null)
            {
                return null;
            }
            DigestRandomGenerator generator = new DigestRandomGenerator(digest);
            if (autoSeed)
            {
                generator.AddSeedMaterial(NextCounterValue());
                generator.AddSeedMaterial(GetNextBytes(Master, digest.GetDigestSize()));
            }
            return generator;
        }

        public virtual byte[] GenerateSeed(int length) => 
            GetNextBytes(Master, length);

        public static SecureRandom GetInstance(string algorithm) => 
            GetInstance(algorithm, true);

        public static SecureRandom GetInstance(string algorithm, bool autoSeed)
        {
            string source = Platform.ToUpperInvariant(algorithm);
            if (Platform.EndsWith(source, "PRNG"))
            {
                DigestRandomGenerator generator = CreatePrng(source.Substring(0, source.Length - "PRNG".Length), autoSeed);
                if (generator != null)
                {
                    return new SecureRandom(generator);
                }
            }
            throw new ArgumentException("Unrecognised PRNG algorithm: " + algorithm, "algorithm");
        }

        public static byte[] GetNextBytes(SecureRandom secureRandom, int length)
        {
            byte[] buffer = new byte[length];
            secureRandom.NextBytes(buffer);
            return buffer;
        }

        [Obsolete("Call GenerateSeed() on a SecureRandom instance instead")]
        public static byte[] GetSeed(int length) => 
            GetNextBytes(Master, length);

        public override int Next() => 
            (this.NextInt() & 0x7fffffff);

        public override int Next(int maxValue)
        {
            int num;
            int num2;
            if (maxValue < 2)
            {
                if (maxValue < 0)
                {
                    throw new ArgumentOutOfRangeException("maxValue", "cannot be negative");
                }
                return 0;
            }
            if ((maxValue & (maxValue - 1)) == 0)
            {
                num = this.NextInt() & 0x7fffffff;
                return ((num * maxValue) >> 0x1f);
            }
            do
            {
                num = this.NextInt() & 0x7fffffff;
                num2 = num % maxValue;
            }
            while (((num - num2) + (maxValue - 1)) < 0);
            return num2;
        }

        public override int Next(int minValue, int maxValue)
        {
            int num2;
            if (maxValue <= minValue)
            {
                if (maxValue != minValue)
                {
                    throw new ArgumentException("maxValue cannot be less than minValue");
                }
                return minValue;
            }
            int num = maxValue - minValue;
            if (num > 0)
            {
                return (minValue + this.Next(num));
            }
            do
            {
                num2 = this.NextInt();
            }
            while ((num2 < minValue) || (num2 >= maxValue));
            return num2;
        }

        public override void NextBytes(byte[] buf)
        {
            this.generator.NextBytes(buf);
        }

        public virtual void NextBytes(byte[] buf, int off, int len)
        {
            this.generator.NextBytes(buf, off, len);
        }

        private static long NextCounterValue() => 
            Interlocked.Increment(ref counter);

        public override double NextDouble() => 
            (Convert.ToDouble((ulong) this.NextLong()) / DoubleScale);

        public virtual int NextInt()
        {
            byte[] buffer = new byte[4];
            this.NextBytes(buffer);
            uint num = buffer[0];
            num = num << 8;
            num |= buffer[1];
            num = num << 8;
            num |= buffer[2];
            num = num << 8;
            num |= buffer[3];
            return (int) num;
        }

        public virtual long NextLong() => 
            ((long) ((((ulong) this.NextInt()) << 0x20) | ((ulong) this.NextInt())));

        public virtual void SetSeed(byte[] seed)
        {
            this.generator.AddSeedMaterial(seed);
        }

        public virtual void SetSeed(long seed)
        {
            this.generator.AddSeedMaterial(seed);
        }

        private static SecureRandom Master =>
            master;
    }
}

