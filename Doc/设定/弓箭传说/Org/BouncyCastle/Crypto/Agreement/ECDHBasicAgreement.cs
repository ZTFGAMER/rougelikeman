namespace Org.BouncyCastle.Crypto.Agreement
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using System;

    public class ECDHBasicAgreement : IBasicAgreement
    {
        protected internal ECPrivateKeyParameters privKey;

        public virtual BigInteger CalculateAgreement(ICipherParameters pubKey)
        {
            ECPublicKeyParameters parameters = (ECPublicKeyParameters) pubKey;
            if (!parameters.Parameters.Equals(this.privKey.Parameters))
            {
                throw new InvalidOperationException("ECDH public key has wrong domain parameters");
            }
            ECPoint point = parameters.Q.Multiply(this.privKey.D).Normalize();
            if (point.IsInfinity)
            {
                throw new InvalidOperationException("Infinity is not a valid agreement value for ECDH");
            }
            return point.AffineXCoord.ToBigInteger();
        }

        public virtual int GetFieldSize() => 
            ((this.privKey.Parameters.Curve.FieldSize + 7) / 8);

        public virtual void Init(ICipherParameters parameters)
        {
            if (parameters is ParametersWithRandom)
            {
                parameters = ((ParametersWithRandom) parameters).Parameters;
            }
            this.privKey = (ECPrivateKeyParameters) parameters;
        }
    }
}

