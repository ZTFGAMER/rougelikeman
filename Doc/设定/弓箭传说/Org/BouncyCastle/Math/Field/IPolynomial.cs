namespace Org.BouncyCastle.Math.Field
{
    using System;

    public interface IPolynomial
    {
        int[] GetExponentsPresent();

        int Degree { get; }
    }
}

