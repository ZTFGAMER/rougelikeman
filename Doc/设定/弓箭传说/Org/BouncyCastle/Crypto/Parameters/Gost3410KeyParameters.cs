namespace Org.BouncyCastle.Crypto.Parameters
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Asn1.CryptoPro;
    using Org.BouncyCastle.Crypto;
    using System;

    public abstract class Gost3410KeyParameters : AsymmetricKeyParameter
    {
        private readonly Gost3410Parameters parameters;
        private readonly DerObjectIdentifier publicKeyParamSet;

        protected Gost3410KeyParameters(bool isPrivate, DerObjectIdentifier publicKeyParamSet) : base(isPrivate)
        {
            this.parameters = LookupParameters(publicKeyParamSet);
            this.publicKeyParamSet = publicKeyParamSet;
        }

        protected Gost3410KeyParameters(bool isPrivate, Gost3410Parameters parameters) : base(isPrivate)
        {
            this.parameters = parameters;
        }

        private static Gost3410Parameters LookupParameters(DerObjectIdentifier publicKeyParamSet)
        {
            if (publicKeyParamSet == null)
            {
                throw new ArgumentNullException("publicKeyParamSet");
            }
            Gost3410ParamSetParameters byOid = Gost3410NamedParameters.GetByOid(publicKeyParamSet);
            if (byOid == null)
            {
                throw new ArgumentException("OID is not a valid CryptoPro public key parameter set", "publicKeyParamSet");
            }
            return new Gost3410Parameters(byOid.P, byOid.Q, byOid.A);
        }

        public Gost3410Parameters Parameters =>
            this.parameters;

        public DerObjectIdentifier PublicKeyParamSet =>
            this.publicKeyParamSet;
    }
}

