namespace Org.BouncyCastle.Math.EC.Multiplier
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;

    public interface ECMultiplier
    {
        ECPoint Multiply(ECPoint p, BigInteger k);
    }
}

