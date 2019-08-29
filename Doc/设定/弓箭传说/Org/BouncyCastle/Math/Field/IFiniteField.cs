namespace Org.BouncyCastle.Math.Field
{
    using Org.BouncyCastle.Math;
    using System;

    public interface IFiniteField
    {
        BigInteger Characteristic { get; }

        int Dimension { get; }
    }
}

