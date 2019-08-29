namespace Org.BouncyCastle.Crypto.Parameters
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Asn1.CryptoPro;
    using Org.BouncyCastle.Asn1.X9;
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Generators;
    using Org.BouncyCastle.Security;
    using Org.BouncyCastle.Utilities;
    using System;

    public abstract class ECKeyParameters : AsymmetricKeyParameter
    {
        private static readonly string[] algorithms = new string[] { "EC", "ECDSA", "ECDH", "ECDHC", "ECGOST3410", "ECMQV" };
        private readonly string algorithm;
        private readonly ECDomainParameters parameters;
        private readonly DerObjectIdentifier publicKeyParamSet;

        protected ECKeyParameters(string algorithm, bool isPrivate, DerObjectIdentifier publicKeyParamSet) : base(isPrivate)
        {
            if (algorithm == null)
            {
                throw new ArgumentNullException("algorithm");
            }
            if (publicKeyParamSet == null)
            {
                throw new ArgumentNullException("publicKeyParamSet");
            }
            this.algorithm = VerifyAlgorithmName(algorithm);
            this.parameters = LookupParameters(publicKeyParamSet);
            this.publicKeyParamSet = publicKeyParamSet;
        }

        protected ECKeyParameters(string algorithm, bool isPrivate, ECDomainParameters parameters) : base(isPrivate)
        {
            if (algorithm == null)
            {
                throw new ArgumentNullException("algorithm");
            }
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }
            this.algorithm = VerifyAlgorithmName(algorithm);
            this.parameters = parameters;
        }

        internal ECKeyGenerationParameters CreateKeyGenerationParameters(SecureRandom random)
        {
            if (this.publicKeyParamSet != null)
            {
                return new ECKeyGenerationParameters(this.publicKeyParamSet, random);
            }
            return new ECKeyGenerationParameters(this.parameters, random);
        }

        protected bool Equals(ECKeyParameters other) => 
            (this.parameters.Equals(other.parameters) && base.Equals((AsymmetricKeyParameter) other));

        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }
            ECDomainParameters parameters = obj as ECDomainParameters;
            if (parameters == null)
            {
                return false;
            }
            return this.Equals(parameters);
        }

        public override int GetHashCode() => 
            (this.parameters.GetHashCode() ^ base.GetHashCode());

        internal static ECDomainParameters LookupParameters(DerObjectIdentifier publicKeyParamSet)
        {
            if (publicKeyParamSet == null)
            {
                throw new ArgumentNullException("publicKeyParamSet");
            }
            ECDomainParameters byOid = ECGost3410NamedCurves.GetByOid(publicKeyParamSet);
            if (byOid != null)
            {
                return byOid;
            }
            X9ECParameters parameters2 = ECKeyPairGenerator.FindECCurveByOid(publicKeyParamSet);
            if (parameters2 == null)
            {
                throw new ArgumentException("OID is not a valid public key parameter set", "publicKeyParamSet");
            }
            return new ECDomainParameters(parameters2.Curve, parameters2.G, parameters2.N, parameters2.H, parameters2.GetSeed());
        }

        internal static string VerifyAlgorithmName(string algorithm)
        {
            string str = Platform.ToUpperInvariant(algorithm);
            if (Array.IndexOf<string>(algorithms, algorithm, 0, algorithms.Length) < 0)
            {
                throw new ArgumentException("unrecognised algorithm: " + algorithm, "algorithm");
            }
            return str;
        }

        public string AlgorithmName =>
            this.algorithm;

        public ECDomainParameters Parameters =>
            this.parameters;

        public DerObjectIdentifier PublicKeyParamSet =>
            this.publicKeyParamSet;
    }
}

