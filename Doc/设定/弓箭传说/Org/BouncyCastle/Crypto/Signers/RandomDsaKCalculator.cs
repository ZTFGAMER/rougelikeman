namespace Org.BouncyCastle.Crypto.Signers
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Security;
    using System;

    public class RandomDsaKCalculator : IDsaKCalculator
    {
        private BigInteger q;
        private SecureRandom random;

        public virtual void Init(BigInteger n, SecureRandom random)
        {
            this.q = n;
            this.random = random;
        }

        public virtual void Init(BigInteger n, BigInteger d, byte[] message)
        {
            throw new InvalidOperationException("Operation not supported");
        }

        public virtual BigInteger NextK()
        {
            BigInteger integer;
            int bitLength = this.q.BitLength;
            do
            {
                integer = new BigInteger(bitLength, this.random);
            }
            while ((integer.SignValue < 1) || (integer.CompareTo(this.q) >= 0));
            return integer;
        }

        public virtual bool IsDeterministic =>
            false;
    }
}

