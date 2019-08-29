namespace Org.BouncyCastle.Crypto.Parameters
{
    using Org.BouncyCastle.Utilities;
    using System;

    public class DHValidationParameters
    {
        private readonly byte[] seed;
        private readonly int counter;

        public DHValidationParameters(byte[] seed, int counter)
        {
            if (seed == null)
            {
                throw new ArgumentNullException("seed");
            }
            this.seed = (byte[]) seed.Clone();
            this.counter = counter;
        }

        protected bool Equals(DHValidationParameters other) => 
            ((this.counter == other.counter) && Arrays.AreEqual(this.seed, other.seed));

        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }
            DHValidationParameters other = obj as DHValidationParameters;
            if (other == null)
            {
                return false;
            }
            return this.Equals(other);
        }

        public override int GetHashCode() => 
            (this.counter.GetHashCode() ^ Arrays.GetHashCode(this.seed));

        public byte[] GetSeed() => 
            ((byte[]) this.seed.Clone());

        public int Counter =>
            this.counter;
    }
}

