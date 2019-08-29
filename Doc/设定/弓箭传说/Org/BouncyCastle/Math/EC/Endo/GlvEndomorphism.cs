namespace Org.BouncyCastle.Math.EC.Endo
{
    using Org.BouncyCastle.Math;

    public interface GlvEndomorphism : ECEndomorphism
    {
        BigInteger[] DecomposeScalar(BigInteger k);
    }
}

