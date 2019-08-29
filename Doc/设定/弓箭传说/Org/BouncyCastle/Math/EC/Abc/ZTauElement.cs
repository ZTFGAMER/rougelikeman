namespace Org.BouncyCastle.Math.EC.Abc
{
    using Org.BouncyCastle.Math;
    using System;

    internal class ZTauElement
    {
        public readonly BigInteger u;
        public readonly BigInteger v;

        public ZTauElement(BigInteger u, BigInteger v)
        {
            this.u = u;
            this.v = v;
        }
    }
}

