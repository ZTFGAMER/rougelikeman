namespace Org.BouncyCastle.Crypto.Prng
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Utilities;
    using System;

    public class DigestRandomGenerator : IRandomGenerator
    {
        private const long CYCLE_COUNT = 10L;
        private long stateCounter;
        private long seedCounter;
        private IDigest digest;
        private byte[] state;
        private byte[] seed;

        public DigestRandomGenerator(IDigest digest)
        {
            this.digest = digest;
            this.seed = new byte[digest.GetDigestSize()];
            this.seedCounter = 1L;
            this.state = new byte[digest.GetDigestSize()];
            this.stateCounter = 1L;
        }

        public void AddSeedMaterial(byte[] inSeed)
        {
            object obj2 = this;
            lock (obj2)
            {
                this.DigestUpdate(inSeed);
                this.DigestUpdate(this.seed);
                this.DigestDoFinal(this.seed);
            }
        }

        public void AddSeedMaterial(long rSeed)
        {
            object obj2 = this;
            lock (obj2)
            {
                this.DigestAddCounter(rSeed);
                this.DigestUpdate(this.seed);
                this.DigestDoFinal(this.seed);
            }
        }

        private void CycleSeed()
        {
            long num;
            this.DigestUpdate(this.seed);
            this.seedCounter = (num = this.seedCounter) + 1L;
            this.DigestAddCounter(num);
            this.DigestDoFinal(this.seed);
        }

        private void DigestAddCounter(long seedVal)
        {
            byte[] bs = new byte[8];
            Pack.UInt64_To_LE((ulong) seedVal, bs);
            this.digest.BlockUpdate(bs, 0, bs.Length);
        }

        private void DigestDoFinal(byte[] result)
        {
            this.digest.DoFinal(result, 0);
        }

        private void DigestUpdate(byte[] inSeed)
        {
            this.digest.BlockUpdate(inSeed, 0, inSeed.Length);
        }

        private void GenerateState()
        {
            long num;
            this.stateCounter = (num = this.stateCounter) + 1L;
            this.DigestAddCounter(num);
            this.DigestUpdate(this.state);
            this.DigestUpdate(this.seed);
            this.DigestDoFinal(this.state);
            if ((this.stateCounter % 10L) == 0L)
            {
                this.CycleSeed();
            }
        }

        public void NextBytes(byte[] bytes)
        {
            this.NextBytes(bytes, 0, bytes.Length);
        }

        public void NextBytes(byte[] bytes, int start, int len)
        {
            object obj2 = this;
            lock (obj2)
            {
                int num = 0;
                this.GenerateState();
                int num2 = start + len;
                for (int i = start; i < num2; i++)
                {
                    if (num == this.state.Length)
                    {
                        this.GenerateState();
                        num = 0;
                    }
                    bytes[i] = this.state[num++];
                }
            }
        }
    }
}

