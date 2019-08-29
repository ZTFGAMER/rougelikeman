namespace Org.BouncyCastle.Crypto.Parameters
{
    using Org.BouncyCastle.Utilities;
    using System;

    public class DsaValidationParameters
    {
        private readonly byte[] seed;
        private readonly int counter;
        private readonly int usageIndex;

        public DsaValidationParameters(byte[] seed, int counter) : this(seed, counter, -1)
        {
        }

        public DsaValidationParameters(byte[] seed, int counter, int usageIndex)
        {
            if (seed == null)
            {
                throw new ArgumentNullException("seed");
            }
            this.seed = (byte[]) seed.Clone();
            this.counter = counter;
            this.usageIndex = usageIndex;
        }

        protected virtual bool Equals(DsaValidationParameters other) => 
            ((this.counter == other.counter) && Arrays.AreEqual(this.seed, other.seed));

        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }
            DsaValidationParameters other = obj as DsaValidationParameters;
            if (other == null)
            {
                return false;
            }
            return this.Equals(other);
        }

        public override int GetHashCode() => 
            (this.counter.GetHashCode() ^ Arrays.GetHashCode(this.seed));

        public virtual byte[] GetSeed() => 
            ((byte[]) this.seed.Clone());

        public virtual int Counter =>
            this.counter;

        public virtual int UsageIndex =>
            this.usageIndex;
    }
}

