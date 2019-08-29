namespace Org.BouncyCastle.Crypto.Parameters
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Security;
    using System;

    public class ECKeyGenerationParameters : KeyGenerationParameters
    {
        private readonly ECDomainParameters domainParams;
        private readonly DerObjectIdentifier publicKeyParamSet;

        public ECKeyGenerationParameters(DerObjectIdentifier publicKeyParamSet, SecureRandom random) : this(ECKeyParameters.LookupParameters(publicKeyParamSet), random)
        {
            this.publicKeyParamSet = publicKeyParamSet;
        }

        public ECKeyGenerationParameters(ECDomainParameters domainParameters, SecureRandom random) : base(random, domainParameters.N.BitLength)
        {
            this.domainParams = domainParameters;
        }

        public ECDomainParameters DomainParameters =>
            this.domainParams;

        public DerObjectIdentifier PublicKeyParamSet =>
            this.publicKeyParamSet;
    }
}

