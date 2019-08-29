namespace Org.BouncyCastle.Crypto.Tls
{
    using Org.BouncyCastle.Asn1.X9;
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Agreement;
    using Org.BouncyCastle.Crypto.EC;
    using Org.BouncyCastle.Crypto.Generators;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Math.Field;
    using Org.BouncyCastle.Security;
    using Org.BouncyCastle.Utilities;
    using System;
    using System.Collections;
    using System.IO;

    public abstract class TlsEccUtilities
    {
        private static readonly string[] CurveNames = new string[] { 
            "sect163k1", "sect163r1", "sect163r2", "sect193r1", "sect193r2", "sect233k1", "sect233r1", "sect239k1", "sect283k1", "sect283r1", "sect409k1", "sect409r1", "sect571k1", "sect571r1", "secp160k1", "secp160r1",
            "secp160r2", "secp192k1", "secp192r1", "secp224k1", "secp224r1", "secp256k1", "secp256r1", "secp384r1", "secp521r1", "brainpoolP256r1", "brainpoolP384r1", "brainpoolP512r1"
        };

        protected TlsEccUtilities()
        {
        }

        public static void AddSupportedEllipticCurvesExtension(IDictionary extensions, int[] namedCurves)
        {
            extensions[10] = CreateSupportedEllipticCurvesExtension(namedCurves);
        }

        public static void AddSupportedPointFormatsExtension(IDictionary extensions, byte[] ecPointFormats)
        {
            extensions[11] = CreateSupportedPointFormatsExtension(ecPointFormats);
        }

        public static bool AreOnSameCurve(ECDomainParameters a, ECDomainParameters b) => 
            ((a != null) && a.Equals(b));

        public static byte[] CalculateECDHBasicAgreement(ECPublicKeyParameters publicKey, ECPrivateKeyParameters privateKey)
        {
            ECDHBasicAgreement agreement = new ECDHBasicAgreement();
            agreement.Init(privateKey);
            BigInteger n = agreement.CalculateAgreement(publicKey);
            return BigIntegers.AsUnsignedByteArray(agreement.GetFieldSize(), n);
        }

        private static void CheckNamedCurve(int[] namedCurves, int namedCurve)
        {
            if ((namedCurves != null) && !Arrays.Contains(namedCurves, namedCurve))
            {
                throw new TlsFatalAlert(0x2f);
            }
        }

        public static bool ContainsEccCipherSuites(int[] cipherSuites)
        {
            for (int i = 0; i < cipherSuites.Length; i++)
            {
                if (IsEccCipherSuite(cipherSuites[i]))
                {
                    return true;
                }
            }
            return false;
        }

        public static byte[] CreateSupportedEllipticCurvesExtension(int[] namedCurves)
        {
            if ((namedCurves == null) || (namedCurves.Length < 1))
            {
                throw new TlsFatalAlert(80);
            }
            return TlsUtilities.EncodeUint16ArrayWithUint16Length(namedCurves);
        }

        public static byte[] CreateSupportedPointFormatsExtension(byte[] ecPointFormats)
        {
            if ((ecPointFormats == null) || !Arrays.Contains(ecPointFormats, 0))
            {
                ecPointFormats = Arrays.Append(ecPointFormats, 0);
            }
            return TlsUtilities.EncodeUint8ArrayWithUint8Length(ecPointFormats);
        }

        public static BigInteger DeserializeECFieldElement(int fieldSize, byte[] encoding)
        {
            int num = (fieldSize + 7) / 8;
            if (encoding.Length != num)
            {
                throw new TlsFatalAlert(50);
            }
            return new BigInteger(1, encoding);
        }

        public static ECPoint DeserializeECPoint(byte[] ecPointFormats, ECCurve curve, byte[] encoding)
        {
            byte num;
            if ((encoding == null) || (encoding.Length < 1))
            {
                throw new TlsFatalAlert(0x2f);
            }
            switch (encoding[0])
            {
                case 2:
                case 3:
                    if (!ECAlgorithms.IsF2mCurve(curve))
                    {
                        if (!ECAlgorithms.IsFpCurve(curve))
                        {
                            throw new TlsFatalAlert(0x2f);
                        }
                        num = 1;
                    }
                    else
                    {
                        num = 2;
                    }
                    break;

                case 4:
                    num = 0;
                    break;

                default:
                    throw new TlsFatalAlert(0x2f);
            }
            if ((num != 0) && ((ecPointFormats == null) || !Arrays.Contains(ecPointFormats, num)))
            {
                throw new TlsFatalAlert(0x2f);
            }
            return curve.DecodePoint(encoding);
        }

        public static ECPublicKeyParameters DeserializeECPublicKey(byte[] ecPointFormats, ECDomainParameters curve_params, byte[] encoding)
        {
            ECPublicKeyParameters parameters;
            try
            {
                parameters = new ECPublicKeyParameters(DeserializeECPoint(ecPointFormats, curve_params.Curve, encoding), curve_params);
            }
            catch (Exception exception)
            {
                throw new TlsFatalAlert(0x2f, exception);
            }
            return parameters;
        }

        public static AsymmetricCipherKeyPair GenerateECKeyPair(SecureRandom random, ECDomainParameters ecParams)
        {
            ECKeyPairGenerator generator = new ECKeyPairGenerator();
            generator.Init(new ECKeyGenerationParameters(ecParams, random));
            return generator.GenerateKeyPair();
        }

        public static ECPrivateKeyParameters GenerateEphemeralClientKeyExchange(SecureRandom random, byte[] ecPointFormats, ECDomainParameters ecParams, Stream output)
        {
            AsymmetricCipherKeyPair pair = GenerateECKeyPair(random, ecParams);
            ECPublicKeyParameters @public = (ECPublicKeyParameters) pair.Public;
            WriteECPoint(ecPointFormats, @public.Q, output);
            return (ECPrivateKeyParameters) pair.Private;
        }

        internal static ECPrivateKeyParameters GenerateEphemeralServerKeyExchange(SecureRandom random, int[] namedCurves, byte[] ecPointFormats, Stream output)
        {
            int namedCurve = -1;
            if (namedCurves == null)
            {
                namedCurve = 0x17;
            }
            else
            {
                for (int i = 0; i < namedCurves.Length; i++)
                {
                    int num3 = namedCurves[i];
                    if (NamedCurve.IsValid(num3) && IsSupportedNamedCurve(num3))
                    {
                        namedCurve = num3;
                        break;
                    }
                }
            }
            ECDomainParameters ecParameters = null;
            if (namedCurve >= 0)
            {
                ecParameters = GetParametersForNamedCurve(namedCurve);
            }
            else if (Arrays.Contains(namedCurves, 0xff01))
            {
                ecParameters = GetParametersForNamedCurve(0x17);
            }
            else if (Arrays.Contains(namedCurves, 0xff02))
            {
                ecParameters = GetParametersForNamedCurve(10);
            }
            if (ecParameters == null)
            {
                throw new TlsFatalAlert(80);
            }
            if (namedCurve < 0)
            {
                WriteExplicitECParameters(ecPointFormats, ecParameters, output);
            }
            else
            {
                WriteNamedECParameters(namedCurve, output);
            }
            return GenerateEphemeralClientKeyExchange(random, ecPointFormats, ecParameters, output);
        }

        public static string GetNameOfNamedCurve(int namedCurve) => 
            (!IsSupportedNamedCurve(namedCurve) ? null : CurveNames[namedCurve - 1]);

        public static ECDomainParameters GetParametersForNamedCurve(int namedCurve)
        {
            string nameOfNamedCurve = GetNameOfNamedCurve(namedCurve);
            if (nameOfNamedCurve == null)
            {
                return null;
            }
            X9ECParameters byName = CustomNamedCurves.GetByName(nameOfNamedCurve);
            if (byName == null)
            {
                byName = ECNamedCurveTable.GetByName(nameOfNamedCurve);
                if (byName == null)
                {
                    return null;
                }
            }
            return new ECDomainParameters(byName.Curve, byName.G, byName.N, byName.H, byName.GetSeed());
        }

        public static int[] GetSupportedEllipticCurvesExtension(IDictionary extensions)
        {
            byte[] extensionData = TlsUtilities.GetExtensionData(extensions, 10);
            return ((extensionData != null) ? ReadSupportedEllipticCurvesExtension(extensionData) : null);
        }

        public static byte[] GetSupportedPointFormatsExtension(IDictionary extensions)
        {
            byte[] extensionData = TlsUtilities.GetExtensionData(extensions, 11);
            return ((extensionData != null) ? ReadSupportedPointFormatsExtension(extensionData) : null);
        }

        public static bool HasAnySupportedNamedCurves() => 
            (CurveNames.Length > 0);

        public static bool IsCompressionPreferred(byte[] ecPointFormats, byte compressionFormat)
        {
            if (ecPointFormats != null)
            {
                for (int i = 0; i < ecPointFormats.Length; i++)
                {
                    byte num2 = ecPointFormats[i];
                    if (num2 == 0)
                    {
                        return false;
                    }
                    if (num2 == compressionFormat)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool IsEccCipherSuite(int cipherSuite)
        {
            switch (cipherSuite)
            {
                case 0xc001:
                case 0xc002:
                case 0xc003:
                case 0xc004:
                case 0xc005:
                case 0xc006:
                case 0xc007:
                case 0xc008:
                case 0xc009:
                case 0xc00a:
                case 0xc00b:
                case 0xc00c:
                case 0xc00d:
                case 0xc00e:
                case 0xc00f:
                case 0xc010:
                case 0xc011:
                case 0xc012:
                case 0xc013:
                case 0xc014:
                case 0xc015:
                case 0xc016:
                case 0xc017:
                case 0xc018:
                case 0xc019:
                case 0xc023:
                case 0xc024:
                case 0xc025:
                case 0xc026:
                case 0xc027:
                case 0xc028:
                case 0xc029:
                case 0xc02a:
                case 0xc02b:
                case 0xc02c:
                case 0xc02d:
                case 0xc02e:
                case 0xc02f:
                case 0xc030:
                case 0xc031:
                case 0xc032:
                case 0xc033:
                case 0xc034:
                case 0xc035:
                case 0xc036:
                case 0xc037:
                case 0xc038:
                case 0xc039:
                case 0xc03a:
                case 0xc03b:
                    break;

                default:
                    switch (cipherSuite)
                    {
                        case 0xc072:
                        case 0xc073:
                        case 0xc074:
                        case 0xc075:
                        case 0xc076:
                        case 0xc077:
                        case 0xc078:
                        case 0xc079:
                            break;

                        default:
                            break;
                    }
                    if (((cipherSuite != 0xc09a) && (cipherSuite != 0xc09b)) && ((cipherSuite != 0xff14) && (cipherSuite != 0xff15)))
                    {
                        return false;
                    }
                    break;
            }
            return true;
        }

        public static bool IsSupportedNamedCurve(int namedCurve) => 
            ((namedCurve > 0) && (namedCurve <= CurveNames.Length));

        public static int ReadECExponent(int fieldSize, Stream input)
        {
            BigInteger integer = ReadECParameter(input);
            if (integer.BitLength < 0x20)
            {
                int intValue = integer.IntValue;
                if ((intValue > 0) && (intValue < fieldSize))
                {
                    return intValue;
                }
            }
            throw new TlsFatalAlert(0x2f);
        }

        public static BigInteger ReadECFieldElement(int fieldSize, Stream input) => 
            DeserializeECFieldElement(fieldSize, TlsUtilities.ReadOpaque8(input));

        public static BigInteger ReadECParameter(Stream input) => 
            new BigInteger(1, TlsUtilities.ReadOpaque8(input));

        public static ECDomainParameters ReadECParameters(int[] namedCurves, byte[] ecPointFormats, Stream input)
        {
            ECDomainParameters parametersForNamedCurve;
            try
            {
                int num2;
                byte num3;
                int num7;
                switch (TlsUtilities.ReadUint8(input))
                {
                    case 1:
                    {
                        CheckNamedCurve(namedCurves, 0xff01);
                        BigInteger q = ReadECParameter(input);
                        BigInteger integer2 = ReadECFieldElement(q.BitLength, input);
                        BigInteger integer3 = ReadECFieldElement(q.BitLength, input);
                        byte[] buffer = TlsUtilities.ReadOpaque8(input);
                        BigInteger integer4 = ReadECParameter(input);
                        BigInteger integer5 = ReadECParameter(input);
                        ECCurve curve = new FpCurve(q, integer2, integer3, integer4, integer5);
                        return new ECDomainParameters(curve, DeserializeECPoint(ecPointFormats, curve, buffer), integer4, integer5);
                    }
                    case 2:
                        CheckNamedCurve(namedCurves, 0xff02);
                        num2 = TlsUtilities.ReadUint16(input);
                        num3 = TlsUtilities.ReadUint8(input);
                        if (!ECBasisType.IsValid(num3))
                        {
                            throw new TlsFatalAlert(0x2f);
                        }
                        break;

                    case 3:
                        num7 = TlsUtilities.ReadUint16(input);
                        if (!NamedCurve.RefersToASpecificNamedCurve(num7))
                        {
                            throw new TlsFatalAlert(0x2f);
                        }
                        goto Label_018A;

                    default:
                        throw new TlsFatalAlert(0x2f);
                }
                int k = ReadECExponent(num2, input);
                int num5 = -1;
                int num6 = -1;
                if (num3 == 2)
                {
                    num5 = ReadECExponent(num2, input);
                    num6 = ReadECExponent(num2, input);
                }
                BigInteger a = ReadECFieldElement(num2, input);
                BigInteger b = ReadECFieldElement(num2, input);
                byte[] encoding = TlsUtilities.ReadOpaque8(input);
                BigInteger order = ReadECParameter(input);
                BigInteger cofactor = ReadECParameter(input);
                ECCurve curve2 = (num3 != 2) ? new F2mCurve(num2, k, a, b, order, cofactor) : new F2mCurve(num2, k, num5, num6, a, b, order, cofactor);
                return new ECDomainParameters(curve2, DeserializeECPoint(ecPointFormats, curve2, encoding), order, cofactor);
            Label_018A:
                CheckNamedCurve(namedCurves, num7);
                parametersForNamedCurve = GetParametersForNamedCurve(num7);
            }
            catch (Exception exception)
            {
                throw new TlsFatalAlert(0x2f, exception);
            }
            return parametersForNamedCurve;
        }

        public static int[] ReadSupportedEllipticCurvesExtension(byte[] extensionData)
        {
            if (extensionData == null)
            {
                throw new ArgumentNullException("extensionData");
            }
            MemoryStream input = new MemoryStream(extensionData, false);
            int num = TlsUtilities.ReadUint16(input);
            if ((num < 2) || ((num & 1) != 0))
            {
                throw new TlsFatalAlert(50);
            }
            int[] numArray = TlsUtilities.ReadUint16Array(num / 2, input);
            TlsProtocol.AssertEmpty(input);
            return numArray;
        }

        public static byte[] ReadSupportedPointFormatsExtension(byte[] extensionData)
        {
            if (extensionData == null)
            {
                throw new ArgumentNullException("extensionData");
            }
            MemoryStream input = new MemoryStream(extensionData, false);
            byte count = TlsUtilities.ReadUint8(input);
            if (count < 1)
            {
                throw new TlsFatalAlert(50);
            }
            byte[] a = TlsUtilities.ReadUint8Array(count, input);
            TlsProtocol.AssertEmpty(input);
            if (!Arrays.Contains(a, 0))
            {
                throw new TlsFatalAlert(0x2f);
            }
            return a;
        }

        public static byte[] SerializeECFieldElement(int fieldSize, BigInteger x) => 
            BigIntegers.AsUnsignedByteArray((fieldSize + 7) / 8, x);

        public static byte[] SerializeECPoint(byte[] ecPointFormats, ECPoint point)
        {
            ECCurve c = point.Curve;
            bool compressed = false;
            if (ECAlgorithms.IsFpCurve(c))
            {
                compressed = IsCompressionPreferred(ecPointFormats, 1);
            }
            else if (ECAlgorithms.IsF2mCurve(c))
            {
                compressed = IsCompressionPreferred(ecPointFormats, 2);
            }
            return point.GetEncoded(compressed);
        }

        public static byte[] SerializeECPublicKey(byte[] ecPointFormats, ECPublicKeyParameters keyParameters) => 
            SerializeECPoint(ecPointFormats, keyParameters.Q);

        public static ECPublicKeyParameters ValidateECPublicKey(ECPublicKeyParameters key) => 
            key;

        public static void WriteECExponent(int k, Stream output)
        {
            WriteECParameter(BigInteger.ValueOf((long) k), output);
        }

        public static void WriteECFieldElement(ECFieldElement x, Stream output)
        {
            TlsUtilities.WriteOpaque8(x.GetEncoded(), output);
        }

        public static void WriteECFieldElement(int fieldSize, BigInteger x, Stream output)
        {
            TlsUtilities.WriteOpaque8(SerializeECFieldElement(fieldSize, x), output);
        }

        public static void WriteECParameter(BigInteger x, Stream output)
        {
            TlsUtilities.WriteOpaque8(BigIntegers.AsUnsignedByteArray(x), output);
        }

        public static void WriteECPoint(byte[] ecPointFormats, ECPoint point, Stream output)
        {
            TlsUtilities.WriteOpaque8(SerializeECPoint(ecPointFormats, point), output);
        }

        public static void WriteExplicitECParameters(byte[] ecPointFormats, ECDomainParameters ecParameters, Stream output)
        {
            ECCurve c = ecParameters.Curve;
            if (ECAlgorithms.IsFpCurve(c))
            {
                TlsUtilities.WriteUint8(1, output);
                WriteECParameter(c.Field.Characteristic, output);
            }
            else
            {
                if (!ECAlgorithms.IsF2mCurve(c))
                {
                    throw new ArgumentException("'ecParameters' not a known curve type");
                }
                IPolynomialExtensionField field = (IPolynomialExtensionField) c.Field;
                int[] exponentsPresent = field.MinimalPolynomial.GetExponentsPresent();
                TlsUtilities.WriteUint8(2, output);
                int i = exponentsPresent[exponentsPresent.Length - 1];
                TlsUtilities.CheckUint16(i);
                TlsUtilities.WriteUint16(i, output);
                if (exponentsPresent.Length == 3)
                {
                    TlsUtilities.WriteUint8(1, output);
                    WriteECExponent(exponentsPresent[1], output);
                }
                else
                {
                    if (exponentsPresent.Length != 5)
                    {
                        throw new ArgumentException("Only trinomial and pentomial curves are supported");
                    }
                    TlsUtilities.WriteUint8(2, output);
                    WriteECExponent(exponentsPresent[1], output);
                    WriteECExponent(exponentsPresent[2], output);
                    WriteECExponent(exponentsPresent[3], output);
                }
            }
            WriteECFieldElement(c.A, output);
            WriteECFieldElement(c.B, output);
            TlsUtilities.WriteOpaque8(SerializeECPoint(ecPointFormats, ecParameters.G), output);
            WriteECParameter(ecParameters.N, output);
            WriteECParameter(ecParameters.H, output);
        }

        public static void WriteNamedECParameters(int namedCurve, Stream output)
        {
            if (!NamedCurve.RefersToASpecificNamedCurve(namedCurve))
            {
                throw new TlsFatalAlert(80);
            }
            TlsUtilities.WriteUint8(3, output);
            TlsUtilities.CheckUint16(namedCurve);
            TlsUtilities.WriteUint16(namedCurve, output);
        }
    }
}

