namespace Org.BouncyCastle.Math.EC.Endo
{
    using Org.BouncyCastle.Math.EC;
    using System;

    public interface ECEndomorphism
    {
        ECPointMap PointMap { get; }

        bool HasEfficientPointMap { get; }
    }
}

