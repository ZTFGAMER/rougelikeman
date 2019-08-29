namespace Org.BouncyCastle.Security
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Asn1.CryptoPro;
    using Org.BouncyCastle.Asn1.Oiw;
    using Org.BouncyCastle.Asn1.Pkcs;
    using Org.BouncyCastle.Asn1.X509;
    using Org.BouncyCastle.Asn1.X9;
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Generators;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using System;
    using System.IO;

    public sealed class PublicKeyFactory
    {
        private PublicKeyFactory()
        {
        }

        public static AsymmetricKeyParameter CreateKey(byte[] keyInfoData) => 
            CreateKey(SubjectPublicKeyInfo.GetInstance(Asn1Object.FromByteArray(keyInfoData)));

        public static AsymmetricKeyParameter CreateKey(SubjectPublicKeyInfo keyInfo)
        {
            DerOctetString publicKey;
            AlgorithmIdentifier algorithmID = keyInfo.AlgorithmID;
            DerObjectIdentifier algorithm = algorithmID.Algorithm;
            if ((algorithm.Equals(PkcsObjectIdentifiers.RsaEncryption) || algorithm.Equals(X509ObjectIdentifiers.IdEARsa)) || (algorithm.Equals(PkcsObjectIdentifiers.IdRsassaPss) || algorithm.Equals(PkcsObjectIdentifiers.IdRsaesOaep)))
            {
                RsaPublicKeyStructure instance = RsaPublicKeyStructure.GetInstance(keyInfo.GetPublicKey());
                return new RsaKeyParameters(false, instance.Modulus, instance.PublicExponent);
            }
            if (algorithm.Equals(X9ObjectIdentifiers.DHPublicNumber))
            {
                Asn1Sequence instance = Asn1Sequence.GetInstance(algorithmID.Parameters.ToAsn1Object());
                BigInteger y = DHPublicKey.GetInstance(keyInfo.GetPublicKey()).Y.Value;
                if (IsPkcsDHParam(instance))
                {
                    return ReadPkcsDHParam(algorithm, y, instance);
                }
                DHDomainParameters parameters = DHDomainParameters.GetInstance(instance);
                BigInteger p = parameters.P.Value;
                BigInteger g = parameters.G.Value;
                BigInteger q = parameters.Q.Value;
                BigInteger j = null;
                if (parameters.J != null)
                {
                    j = parameters.J.Value;
                }
                DHValidationParameters validation = null;
                DHValidationParms validationParms = parameters.ValidationParms;
                if (validationParms != null)
                {
                    byte[] seed = validationParms.Seed.GetBytes();
                    BigInteger integer6 = validationParms.PgenCounter.Value;
                    validation = new DHValidationParameters(seed, integer6.IntValue);
                }
                return new DHPublicKeyParameters(y, new DHParameters(p, g, q, j, validation));
            }
            if (algorithm.Equals(PkcsObjectIdentifiers.DhKeyAgreement))
            {
                Asn1Sequence instance = Asn1Sequence.GetInstance(algorithmID.Parameters.ToAsn1Object());
                DerInteger publicKey = (DerInteger) keyInfo.GetPublicKey();
                return ReadPkcsDHParam(algorithm, publicKey.Value, instance);
            }
            if (algorithm.Equals(OiwObjectIdentifiers.ElGamalAlgorithm))
            {
                ElGamalParameter parameter = new ElGamalParameter(Asn1Sequence.GetInstance(algorithmID.Parameters.ToAsn1Object()));
                DerInteger publicKey = (DerInteger) keyInfo.GetPublicKey();
                return new ElGamalPublicKeyParameters(publicKey.Value, new ElGamalParameters(parameter.P, parameter.G));
            }
            if (algorithm.Equals(X9ObjectIdentifiers.IdDsa) || algorithm.Equals(OiwObjectIdentifiers.DsaWithSha1))
            {
                DerInteger publicKey = (DerInteger) keyInfo.GetPublicKey();
                Asn1Encodable parameters = algorithmID.Parameters;
                DsaParameters parameters3 = null;
                if (parameters != null)
                {
                    DsaParameter instance = DsaParameter.GetInstance(parameters.ToAsn1Object());
                    parameters3 = new DsaParameters(instance.P, instance.Q, instance.G);
                }
                return new DsaPublicKeyParameters(publicKey.Value, parameters3);
            }
            if (algorithm.Equals(X9ObjectIdentifiers.IdECPublicKey))
            {
                X9ECParameters parameters5;
                X962Parameters parameters4 = new X962Parameters(algorithmID.Parameters.ToAsn1Object());
                if (parameters4.IsNamedCurve)
                {
                    parameters5 = ECKeyPairGenerator.FindECCurveByOid((DerObjectIdentifier) parameters4.Parameters);
                }
                else
                {
                    parameters5 = new X9ECParameters((Asn1Sequence) parameters4.Parameters);
                }
                Asn1OctetString s = new DerOctetString(keyInfo.PublicKeyData.GetBytes());
                X9ECPoint point = new X9ECPoint(parameters5.Curve, s);
                ECPoint q = point.Point;
                if (parameters4.IsNamedCurve)
                {
                    return new ECPublicKeyParameters("EC", q, (DerObjectIdentifier) parameters4.Parameters);
                }
                return new ECPublicKeyParameters(q, new ECDomainParameters(parameters5.Curve, parameters5.G, parameters5.N, parameters5.H, parameters5.GetSeed()));
            }
            if (algorithm.Equals(CryptoProObjectIdentifiers.GostR3410x2001))
            {
                Asn1OctetString publicKey;
                Gost3410PublicKeyAlgParameters parameters7 = new Gost3410PublicKeyAlgParameters((Asn1Sequence) algorithmID.Parameters);
                try
                {
                    publicKey = (Asn1OctetString) keyInfo.GetPublicKey();
                }
                catch (IOException)
                {
                    throw new ArgumentException("invalid info structure in GOST3410 public key");
                }
                byte[] buffer2 = publicKey.GetOctets();
                byte[] buffer3 = new byte[0x20];
                byte[] buffer4 = new byte[0x20];
                for (int j = 0; j != buffer4.Length; j++)
                {
                    buffer3[j] = buffer2[0x1f - j];
                }
                for (int k = 0; k != buffer3.Length; k++)
                {
                    buffer4[k] = buffer2[0x3f - k];
                }
                ECDomainParameters byOid = ECGost3410NamedCurves.GetByOid(parameters7.PublicKeyParamSet);
                if (byOid == null)
                {
                    return null;
                }
                return new ECPublicKeyParameters("ECGOST3410", byOid.Curve.CreatePoint(new BigInteger(1, buffer3), new BigInteger(1, buffer4)), parameters7.PublicKeyParamSet);
            }
            if (!algorithm.Equals(CryptoProObjectIdentifiers.GostR3410x94))
            {
                throw new SecurityUtilityException("algorithm identifier in key not recognised: " + algorithm);
            }
            Gost3410PublicKeyAlgParameters parameters9 = new Gost3410PublicKeyAlgParameters((Asn1Sequence) algorithmID.Parameters);
            try
            {
                publicKey = (DerOctetString) keyInfo.GetPublicKey();
            }
            catch (IOException)
            {
                throw new ArgumentException("invalid info structure in GOST3410 public key");
            }
            byte[] octets = publicKey.GetOctets();
            byte[] bytes = new byte[octets.Length];
            for (int i = 0; i != octets.Length; i++)
            {
                bytes[i] = octets[(octets.Length - 1) - i];
            }
            return new Gost3410PublicKeyParameters(new BigInteger(1, bytes), parameters9.PublicKeyParamSet);
        }

        public static AsymmetricKeyParameter CreateKey(Stream inStr) => 
            CreateKey(SubjectPublicKeyInfo.GetInstance(Asn1Object.FromStream(inStr)));

        private static bool IsPkcsDHParam(Asn1Sequence seq)
        {
            if (seq.Count == 2)
            {
                return true;
            }
            if (seq.Count > 3)
            {
                return false;
            }
            DerInteger instance = DerInteger.GetInstance(seq[2]);
            DerInteger integer2 = DerInteger.GetInstance(seq[0]);
            return (instance.Value.CompareTo(BigInteger.ValueOf((long) integer2.Value.BitLength)) <= 0);
        }

        private static DHPublicKeyParameters ReadPkcsDHParam(DerObjectIdentifier algOid, BigInteger y, Asn1Sequence seq)
        {
            DHParameter parameter = new DHParameter(seq);
            BigInteger l = parameter.L;
            int num = (l != null) ? l.IntValue : 0;
            return new DHPublicKeyParameters(y, new DHParameters(parameter.P, parameter.G, null, num), algOid);
        }
    }
}

