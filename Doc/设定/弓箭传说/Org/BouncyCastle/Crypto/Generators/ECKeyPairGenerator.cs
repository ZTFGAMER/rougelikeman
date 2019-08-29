namespace Org.BouncyCastle.Crypto.Generators
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Asn1.Sec;
    using Org.BouncyCastle.Asn1.X9;
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.EC;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Math.EC.Multiplier;
    using Org.BouncyCastle.Security;
    using System;

    public class ECKeyPairGenerator : IAsymmetricCipherKeyPairGenerator
    {
        private readonly string algorithm;
        private ECDomainParameters parameters;
        private DerObjectIdentifier publicKeyParamSet;
        private SecureRandom random;

        public ECKeyPairGenerator() : this("EC")
        {
        }

        public ECKeyPairGenerator(string algorithm)
        {
            if (algorithm == null)
            {
                throw new ArgumentNullException("algorithm");
            }
            this.algorithm = ECKeyParameters.VerifyAlgorithmName(algorithm);
        }

        protected virtual ECMultiplier CreateBasePointMultiplier() => 
            new FixedPointCombMultiplier();

        internal static X9ECParameters FindECCurveByOid(DerObjectIdentifier oid)
        {
            X9ECParameters byOid = CustomNamedCurves.GetByOid(oid);
            if (byOid == null)
            {
                byOid = ECNamedCurveTable.GetByOid(oid);
            }
            return byOid;
        }

        public AsymmetricCipherKeyPair GenerateKeyPair()
        {
            BigInteger integer2;
            BigInteger n = this.parameters.N;
            int num = n.BitLength >> 2;
            do
            {
                integer2 = new BigInteger(n.BitLength, this.random);
            }
            while (((integer2.CompareTo(BigInteger.Two) < 0) || (integer2.CompareTo(n) >= 0)) || (WNafUtilities.GetNafWeight(integer2) < num));
            ECPoint q = this.CreateBasePointMultiplier().Multiply(this.parameters.G, integer2);
            if (this.publicKeyParamSet != null)
            {
                return new AsymmetricCipherKeyPair(new ECPublicKeyParameters(this.algorithm, q, this.publicKeyParamSet), new ECPrivateKeyParameters(this.algorithm, integer2, this.publicKeyParamSet));
            }
            return new AsymmetricCipherKeyPair(new ECPublicKeyParameters(this.algorithm, q, this.parameters), new ECPrivateKeyParameters(this.algorithm, integer2, this.parameters));
        }

        internal static ECPublicKeyParameters GetCorrespondingPublicKey(ECPrivateKeyParameters privKey)
        {
            ECDomainParameters parameters = privKey.Parameters;
            ECPoint q = new FixedPointCombMultiplier().Multiply(parameters.G, privKey.D);
            if (privKey.PublicKeyParamSet != null)
            {
                return new ECPublicKeyParameters(privKey.AlgorithmName, q, privKey.PublicKeyParamSet);
            }
            return new ECPublicKeyParameters(privKey.AlgorithmName, q, parameters);
        }

        public void Init(KeyGenerationParameters parameters)
        {
            if (parameters is ECKeyGenerationParameters)
            {
                ECKeyGenerationParameters parameters2 = (ECKeyGenerationParameters) parameters;
                this.publicKeyParamSet = parameters2.PublicKeyParamSet;
                this.parameters = parameters2.DomainParameters;
            }
            else
            {
                DerObjectIdentifier identifier;
                switch (parameters.Strength)
                {
                    case 0xc0:
                        identifier = X9ObjectIdentifiers.Prime192v1;
                        break;

                    case 0xe0:
                        identifier = SecObjectIdentifiers.SecP224r1;
                        break;

                    case 0xef:
                        identifier = X9ObjectIdentifiers.Prime239v1;
                        break;

                    case 0x100:
                        identifier = X9ObjectIdentifiers.Prime256v1;
                        break;

                    case 0x180:
                        identifier = SecObjectIdentifiers.SecP384r1;
                        break;

                    case 0x209:
                        identifier = SecObjectIdentifiers.SecP521r1;
                        break;

                    default:
                        throw new InvalidParameterException("unknown key size.");
                }
                X9ECParameters parameters3 = FindECCurveByOid(identifier);
                this.publicKeyParamSet = identifier;
                this.parameters = new ECDomainParameters(parameters3.Curve, parameters3.G, parameters3.N, parameters3.H, parameters3.GetSeed());
            }
            this.random = parameters.Random;
            if (this.random == null)
            {
                this.random = new SecureRandom();
            }
        }
    }
}

