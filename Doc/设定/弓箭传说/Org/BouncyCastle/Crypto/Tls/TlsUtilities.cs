namespace Org.BouncyCastle.Crypto.Tls
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Asn1.Nist;
    using Org.BouncyCastle.Asn1.Pkcs;
    using Org.BouncyCastle.Asn1.X509;
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Digests;
    using Org.BouncyCastle.Crypto.Macs;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Security;
    using Org.BouncyCastle.Utilities;
    using Org.BouncyCastle.Utilities.Date;
    using Org.BouncyCastle.Utilities.IO;
    using System;
    using System.Collections;
    using System.IO;

    public abstract class TlsUtilities
    {
        public static readonly byte[] EmptyBytes = new byte[0];
        public static readonly short[] EmptyShorts = new short[0];
        public static readonly int[] EmptyInts = new int[0];
        public static readonly long[] EmptyLongs = new long[0];
        internal static readonly byte[] SSL_CLIENT = new byte[] { 0x43, 0x4c, 0x4e, 0x54 };
        internal static readonly byte[] SSL_SERVER = new byte[] { 0x53, 0x52, 0x56, 0x52 };
        internal static readonly byte[][] SSL3_CONST = GenSsl3Const();

        protected TlsUtilities()
        {
        }

        public static void AddSignatureAlgorithmsExtension(IDictionary extensions, IList supportedSignatureAlgorithms)
        {
            extensions[13] = CreateSignatureAlgorithmsExtension(supportedSignatureAlgorithms);
        }

        internal static byte[] CalculateKeyBlock(TlsContext context, int size)
        {
            SecurityParameters securityParameters = context.SecurityParameters;
            byte[] masterSecret = securityParameters.MasterSecret;
            byte[] random = Concat(securityParameters.ServerRandom, securityParameters.ClientRandom);
            if (IsSsl(context))
            {
                return CalculateKeyBlock_Ssl(masterSecret, random, size);
            }
            return PRF(context, masterSecret, "key expansion", random, size);
        }

        internal static byte[] CalculateKeyBlock_Ssl(byte[] master_secret, byte[] random, int size)
        {
            IDigest digest = CreateHash((byte) 1);
            IDigest digest2 = CreateHash((byte) 2);
            int digestSize = digest.GetDigestSize();
            byte[] output = new byte[digest2.GetDigestSize()];
            byte[] buffer2 = new byte[size + digestSize];
            int index = 0;
            int outOff = 0;
            while (outOff < size)
            {
                byte[] input = SSL3_CONST[index];
                digest2.BlockUpdate(input, 0, input.Length);
                digest2.BlockUpdate(master_secret, 0, master_secret.Length);
                digest2.BlockUpdate(random, 0, random.Length);
                digest2.DoFinal(output, 0);
                digest.BlockUpdate(master_secret, 0, master_secret.Length);
                digest.BlockUpdate(output, 0, output.Length);
                digest.DoFinal(buffer2, outOff);
                outOff += digestSize;
                index++;
            }
            return Arrays.CopyOfRange(buffer2, 0, size);
        }

        internal static byte[] CalculateMasterSecret(TlsContext context, byte[] pre_master_secret)
        {
            SecurityParameters securityParameters = context.SecurityParameters;
            byte[] random = !securityParameters.extendedMasterSecret ? Concat(securityParameters.ClientRandom, securityParameters.ServerRandom) : securityParameters.SessionHash;
            if (IsSsl(context))
            {
                return CalculateMasterSecret_Ssl(pre_master_secret, random);
            }
            string asciiLabel = !securityParameters.extendedMasterSecret ? "master secret" : ExporterLabel.extended_master_secret;
            return PRF(context, pre_master_secret, asciiLabel, random, 0x30);
        }

        internal static byte[] CalculateMasterSecret_Ssl(byte[] pre_master_secret, byte[] random)
        {
            IDigest digest = CreateHash((byte) 1);
            IDigest digest2 = CreateHash((byte) 2);
            int digestSize = digest.GetDigestSize();
            byte[] output = new byte[digest2.GetDigestSize()];
            byte[] buffer2 = new byte[digestSize * 3];
            int outOff = 0;
            for (int i = 0; i < 3; i++)
            {
                byte[] input = SSL3_CONST[i];
                digest2.BlockUpdate(input, 0, input.Length);
                digest2.BlockUpdate(pre_master_secret, 0, pre_master_secret.Length);
                digest2.BlockUpdate(random, 0, random.Length);
                digest2.DoFinal(output, 0);
                digest.BlockUpdate(pre_master_secret, 0, pre_master_secret.Length);
                digest.BlockUpdate(output, 0, output.Length);
                digest.DoFinal(buffer2, outOff);
                outOff += digestSize;
            }
            return buffer2;
        }

        internal static byte[] CalculateVerifyData(TlsContext context, string asciiLabel, byte[] handshakeHash)
        {
            if (IsSsl(context))
            {
                return handshakeHash;
            }
            SecurityParameters securityParameters = context.SecurityParameters;
            byte[] masterSecret = securityParameters.MasterSecret;
            int verifyDataLength = securityParameters.VerifyDataLength;
            return PRF(context, masterSecret, asciiLabel, handshakeHash, verifyDataLength);
        }

        public static void CheckUint16(int i)
        {
            if (!IsValidUint16(i))
            {
                throw new TlsFatalAlert(80);
            }
        }

        public static void CheckUint16(long i)
        {
            if (!IsValidUint16(i))
            {
                throw new TlsFatalAlert(80);
            }
        }

        public static void CheckUint24(int i)
        {
            if (!IsValidUint24(i))
            {
                throw new TlsFatalAlert(80);
            }
        }

        public static void CheckUint24(long i)
        {
            if (!IsValidUint24(i))
            {
                throw new TlsFatalAlert(80);
            }
        }

        public static void CheckUint32(long i)
        {
            if (!IsValidUint32(i))
            {
                throw new TlsFatalAlert(80);
            }
        }

        public static void CheckUint48(long i)
        {
            if (!IsValidUint48(i))
            {
                throw new TlsFatalAlert(80);
            }
        }

        public static void CheckUint64(long i)
        {
            if (!IsValidUint64(i))
            {
                throw new TlsFatalAlert(80);
            }
        }

        public static void CheckUint8(int i)
        {
            if (!IsValidUint8(i))
            {
                throw new TlsFatalAlert(80);
            }
        }

        public static void CheckUint8(long i)
        {
            if (!IsValidUint8(i))
            {
                throw new TlsFatalAlert(80);
            }
        }

        public static IDigest CloneHash(byte hashAlgorithm, IDigest hash)
        {
            switch (hashAlgorithm)
            {
                case 1:
                    return new MD5Digest((MD5Digest) hash);

                case 2:
                    return new Sha1Digest((Sha1Digest) hash);

                case 3:
                    return new Sha224Digest((Sha224Digest) hash);

                case 4:
                    return new Sha256Digest((Sha256Digest) hash);

                case 5:
                    return new Sha384Digest((Sha384Digest) hash);

                case 6:
                    return new Sha512Digest((Sha512Digest) hash);
            }
            throw new ArgumentException("unknown HashAlgorithm", "hashAlgorithm");
        }

        public static IDigest ClonePrfHash(int prfAlgorithm, IDigest hash)
        {
            if (prfAlgorithm != 0)
            {
                return CloneHash(GetHashAlgorithmForPrfAlgorithm(prfAlgorithm), hash);
            }
            return new CombinedHash((CombinedHash) hash);
        }

        internal static byte[] Concat(byte[] a, byte[] b)
        {
            byte[] destinationArray = new byte[a.Length + b.Length];
            Array.Copy(a, 0, destinationArray, 0, a.Length);
            Array.Copy(b, 0, destinationArray, a.Length, b.Length);
            return destinationArray;
        }

        public static IDigest CreateHash(SignatureAndHashAlgorithm signatureAndHashAlgorithm) => 
            ((signatureAndHashAlgorithm != null) ? CreateHash(signatureAndHashAlgorithm.Hash) : new CombinedHash());

        public static IDigest CreateHash(byte hashAlgorithm)
        {
            switch (hashAlgorithm)
            {
                case 1:
                    return new MD5Digest();

                case 2:
                    return new Sha1Digest();

                case 3:
                    return new Sha224Digest();

                case 4:
                    return new Sha256Digest();

                case 5:
                    return new Sha384Digest();

                case 6:
                    return new Sha512Digest();
            }
            throw new ArgumentException("unknown HashAlgorithm", "hashAlgorithm");
        }

        public static IDigest CreatePrfHash(int prfAlgorithm)
        {
            if (prfAlgorithm != 0)
            {
                return CreateHash(GetHashAlgorithmForPrfAlgorithm(prfAlgorithm));
            }
            return new CombinedHash();
        }

        public static byte[] CreateSignatureAlgorithmsExtension(IList supportedSignatureAlgorithms)
        {
            MemoryStream output = new MemoryStream();
            EncodeSupportedSignatureAlgorithms(supportedSignatureAlgorithms, false, output);
            return output.ToArray();
        }

        public static TlsSigner CreateTlsSigner(byte clientCertificateType)
        {
            if (clientCertificateType == 1)
            {
                return new TlsRsaSigner();
            }
            if (clientCertificateType != 2)
            {
                if (clientCertificateType != 0x40)
                {
                    throw new ArgumentException("not a type with signing capability", "clientCertificateType");
                }
                return new TlsECDsaSigner();
            }
            return new TlsDssSigner();
        }

        public static byte[] EncodeOpaque8(byte[] buf)
        {
            CheckUint8(buf.Length);
            return Arrays.Prepend(buf, (byte) buf.Length);
        }

        public static void EncodeSupportedSignatureAlgorithms(IList supportedSignatureAlgorithms, bool allowAnonymous, Stream output)
        {
            if (supportedSignatureAlgorithms == null)
            {
                throw new ArgumentNullException("supportedSignatureAlgorithms");
            }
            if ((supportedSignatureAlgorithms.Count < 1) || (supportedSignatureAlgorithms.Count >= 0x8000))
            {
                throw new ArgumentException("must have length from 1 to (2^15 - 1)", "supportedSignatureAlgorithms");
            }
            int i = 2 * supportedSignatureAlgorithms.Count;
            CheckUint16(i);
            WriteUint16(i, output);
            IEnumerator enumerator = supportedSignatureAlgorithms.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    SignatureAndHashAlgorithm current = (SignatureAndHashAlgorithm) enumerator.Current;
                    if (!allowAnonymous && (current.Signature == 0))
                    {
                        throw new ArgumentException("SignatureAlgorithm.anonymous MUST NOT appear in the signature_algorithms extension");
                    }
                    current.Encode(output);
                }
            }
            finally
            {
                if (enumerator is IDisposable disposable)
                {
                    IDisposable disposable;
                    disposable.Dispose();
                }
            }
        }

        public static byte[] EncodeUint16ArrayWithUint16Length(int[] uints)
        {
            int num = 2 * uints.Length;
            byte[] buf = new byte[2 + num];
            WriteUint16ArrayWithUint16Length(uints, buf, 0);
            return buf;
        }

        public static byte[] EncodeUint8ArrayWithUint8Length(byte[] uints)
        {
            byte[] buf = new byte[1 + uints.Length];
            WriteUint8ArrayWithUint8Length(uints, buf, 0);
            return buf;
        }

        private static byte[][] GenSsl3Const()
        {
            int num = 10;
            byte[][] bufferArray = new byte[num][];
            for (int i = 0; i < num; i++)
            {
                byte[] buf = new byte[i + 1];
                Arrays.Fill(buf, (byte) (0x41 + i));
                bufferArray[i] = buf;
            }
            return bufferArray;
        }

        public static int GetCipherType(int ciphersuite)
        {
            switch (GetEncryptionAlgorithm(ciphersuite))
            {
                case 1:
                case 2:
                    return 0;

                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 12:
                case 13:
                case 14:
                    return 1;

                case 10:
                case 11:
                case 15:
                case 0x10:
                case 0x11:
                case 0x12:
                case 0x13:
                case 20:
                case 0x66:
                case 0x67:
                case 0x68:
                    return 2;
            }
            throw new TlsFatalAlert(80);
        }

        internal static short GetClientCertificateType(Certificate clientCertificate, Certificate serverCertificate)
        {
            short num;
            if (clientCertificate.IsEmpty)
            {
                return -1;
            }
            X509CertificateStructure certificateAt = clientCertificate.GetCertificateAt(0);
            SubjectPublicKeyInfo subjectPublicKeyInfo = certificateAt.SubjectPublicKeyInfo;
            try
            {
                AsymmetricKeyParameter parameter = PublicKeyFactory.CreateKey(subjectPublicKeyInfo);
                if (parameter.IsPrivate)
                {
                    throw new TlsFatalAlert(80);
                }
                if (parameter is RsaKeyParameters)
                {
                    ValidateKeyUsage(certificateAt, 0x80);
                    return 1;
                }
                if (parameter is DsaPublicKeyParameters)
                {
                    ValidateKeyUsage(certificateAt, 0x80);
                    return 2;
                }
                if (!(parameter is ECPublicKeyParameters))
                {
                    throw new TlsFatalAlert(0x2b);
                }
                ValidateKeyUsage(certificateAt, 0x80);
                num = 0x40;
            }
            catch (Exception exception)
            {
                throw new TlsFatalAlert(0x2b, exception);
            }
            return num;
        }

        public static IList GetDefaultDssSignatureAlgorithms() => 
            VectorOfOne(new SignatureAndHashAlgorithm(2, 2));

        public static IList GetDefaultECDsaSignatureAlgorithms() => 
            VectorOfOne(new SignatureAndHashAlgorithm(2, 3));

        public static IList GetDefaultRsaSignatureAlgorithms() => 
            VectorOfOne(new SignatureAndHashAlgorithm(2, 1));

        public static IList GetDefaultSupportedSignatureAlgorithms()
        {
            byte[] buffer = new byte[] { 2, 3, 4, 5, 6, 0, 0, 0 };
            byte[] buffer2 = new byte[] { 1, 2, 3, 0 };
            IList list = Platform.CreateArrayList();
            for (int i = 0; i < buffer2.Length; i++)
            {
                for (int j = 0; j < buffer.Length; j++)
                {
                    list.Add(new SignatureAndHashAlgorithm(buffer[j], buffer2[i]));
                }
            }
            return list;
        }

        public static int GetEncryptionAlgorithm(int ciphersuite)
        {
            switch (ciphersuite)
            {
                case 0xc001:
                case 0xc006:
                case 0xc00b:
                case 0xc010:
                case 0xc015:
                case 0xc039:
                case 0x2c:
                case 0x2d:
                case 0x2e:
                    goto Label_057E;

                case 0xc002:
                case 0xc007:
                case 0xc00c:
                case 0xc011:
                case 0xc016:
                case 0xc033:
                case 0x8a:
                case 0x8e:
                case 0x92:
                    goto Label_0586;

                case 0xc003:
                case 0xc008:
                case 0xc00d:
                case 0xc012:
                case 0xc017:
                case 0xc01a:
                case 0xc01b:
                case 0xc01c:
                case 0xc034:
                case 0x8b:
                case 0x8f:
                case 0x93:
                    break;

                case 0xc004:
                case 0xc009:
                case 0xc00e:
                case 0xc013:
                case 0xc018:
                case 0xc01d:
                case 0xc01e:
                case 0xc01f:
                case 0xc023:
                case 0xc025:
                case 0xc027:
                case 0xc029:
                case 0xc035:
                case 0xc037:
                case 140:
                case 0x90:
                case 0x94:
                case 0xae:
                case 0xb2:
                case 0xb6:
                case 0x2f:
                case 0x30:
                case 0x31:
                case 50:
                case 0x33:
                case 60:
                case 0x3e:
                case 0x3f:
                case 0x40:
                    goto Label_0547;

                case 0xc005:
                case 0xc00a:
                case 0xc00f:
                case 0xc014:
                case 0xc019:
                case 0xc020:
                case 0xc021:
                case 0xc022:
                case 0xc024:
                case 0xc026:
                case 0xc028:
                case 0xc02a:
                case 0xc036:
                case 0xc038:
                case 0x8d:
                case 0x91:
                case 0x95:
                case 0xaf:
                case 0xb3:
                case 0xb7:
                case 0x35:
                case 0x36:
                case 0x37:
                case 0x38:
                case 0x39:
                case 0x3d:
                    goto Label_0555;

                case 0xc02b:
                case 0xc02d:
                case 0xc02f:
                case 0xc031:
                case 0x9c:
                case 0x9e:
                case 160:
                case 0xa2:
                case 0xa4:
                case 0xa8:
                case 170:
                case 0xac:
                    return 10;

                case 0xc02c:
                case 0xc02e:
                case 0xc030:
                case 0xc032:
                case 0x9d:
                case 0x9f:
                case 0xa1:
                case 0xa3:
                case 0xa5:
                case 0xa9:
                case 0xab:
                case 0xad:
                    return 11;

                case 0xc03a:
                case 0xb0:
                case 180:
                case 0xb8:
                case 0x3b:
                    return 0;

                case 0xc03b:
                case 0xb1:
                case 0xb5:
                case 0xb9:
                    return 0;

                case 0xc072:
                case 0xc074:
                case 0xc076:
                case 0xc078:
                case 0xc094:
                case 0xc096:
                case 0xc098:
                case 0xc09a:
                case 0xba:
                case 0xbb:
                case 0xbc:
                case 0xbd:
                case 190:
                    return 12;

                case 0xc073:
                case 0xc075:
                case 0xc077:
                case 0xc079:
                case 0xc095:
                case 0xc097:
                case 0xc099:
                case 0xc09b:
                    return 13;

                case 0xc07a:
                case 0xc07c:
                case 0xc07e:
                case 0xc080:
                case 0xc082:
                case 0xc086:
                case 0xc088:
                case 0xc08a:
                case 0xc08c:
                case 0xc08e:
                case 0xc090:
                case 0xc092:
                    return 0x13;

                case 0xc07b:
                case 0xc07d:
                case 0xc07f:
                case 0xc081:
                case 0xc083:
                case 0xc087:
                case 0xc089:
                case 0xc08b:
                case 0xc08d:
                case 0xc08f:
                case 0xc091:
                case 0xc093:
                    return 20;

                case 0xc09c:
                case 0xc09e:
                case 0xc0a4:
                case 0xc0a6:
                case 0xc0ac:
                    return 15;

                case 0xc09d:
                case 0xc09f:
                case 0xc0a5:
                case 0xc0a7:
                case 0xc0ad:
                    return 0x11;

                case 0xc0a0:
                case 0xc0a2:
                case 0xc0a8:
                case 0xc0aa:
                case 0xc0ae:
                    return 0x10;

                case 0xc0a1:
                case 0xc0a3:
                case 0xc0a9:
                case 0xc0ab:
                case 0xc0af:
                    return 0x12;

                case 0x84:
                case 0x85:
                case 0x86:
                case 0x87:
                case 0x88:
                    return 13;

                case 150:
                case 0x97:
                case 0x98:
                case 0x99:
                case 0x9a:
                    return 14;

                case 0xc0:
                case 0xc1:
                case 0xc2:
                case 0xc3:
                case 0xc4:
                    return 13;

                case 0x41:
                case 0x42:
                case 0x43:
                case 0x44:
                case 0x45:
                    return 12;

                default:
                    switch (ciphersuite)
                    {
                        case 0xcca8:
                        case 0xcca9:
                        case 0xccaa:
                        case 0xccab:
                        case 0xccac:
                        case 0xccad:
                        case 0xccae:
                            return 0x66;

                        default:
                            switch (ciphersuite)
                            {
                                case 0xff00:
                                case 0xff02:
                                case 0xff04:
                                case 0xff10:
                                case 0xff12:
                                case 0xff14:
                                    return 0x67;

                                case 0xff01:
                                case 0xff03:
                                case 0xff05:
                                case 0xff11:
                                case 0xff13:
                                case 0xff15:
                                    return 0x68;

                                case 1:
                                    return 0;

                                case 2:
                                    goto Label_057E;

                                case 4:
                                    goto Label_0584;

                                case 5:
                                    goto Label_0586;
                            }
                            break;
                    }
                    switch (ciphersuite)
                    {
                        case 0x67:
                            goto Label_0547;

                        case 0x68:
                        case 0x69:
                        case 0x6a:
                        case 0x6b:
                            goto Label_0555;

                        default:
                            switch (ciphersuite)
                            {
                                case 0x18:
                                    goto Label_0584;
                            }
                            break;
                    }
                    switch (ciphersuite)
                    {
                        case 13:
                        case 0x10:
                            break;

                        case 14:
                        case 15:
                            goto Label_058B;

                        default:
                            goto Label_058B;
                    }
                    break;
            }
            return 7;
        Label_0547:
            return 8;
        Label_0555:
            return 9;
        Label_057E:
            return 0;
        Label_0584:
            return 2;
        Label_0586:
            return 2;
        Label_058B:
            throw new TlsFatalAlert(80);
        }

        public static byte[] GetExtensionData(IDictionary extensions, int extensionType) => 
            ((extensions != null) ? ((byte[]) extensions[extensionType]) : null);

        public static byte GetHashAlgorithmForPrfAlgorithm(int prfAlgorithm)
        {
            switch (prfAlgorithm)
            {
                case 0:
                    throw new ArgumentException("legacy PRF not a valid algorithm", "prfAlgorithm");

                case 1:
                    return 4;

                case 2:
                    return 5;
            }
            throw new ArgumentException("unknown PrfAlgorithm", "prfAlgorithm");
        }

        public static int GetKeyExchangeAlgorithm(int ciphersuite)
        {
            switch (ciphersuite)
            {
                case 0xc001:
                case 0xc002:
                case 0xc003:
                case 0xc004:
                case 0xc005:
                case 0xc025:
                case 0xc026:
                case 0xc02d:
                case 0xc02e:
                case 0xc074:
                case 0xc075:
                case 0xc088:
                case 0xc089:
                    goto Label_054C;

                case 0xc006:
                case 0xc007:
                case 0xc008:
                case 0xc009:
                case 0xc00a:
                case 0xc023:
                case 0xc024:
                case 0xc02b:
                case 0xc02c:
                case 0xc072:
                case 0xc073:
                case 0xc086:
                case 0xc087:
                case 0xc0ac:
                case 0xc0ad:
                case 0xc0ae:
                case 0xc0af:
                    goto Label_0552;

                case 0xc00b:
                case 0xc00c:
                case 0xc00d:
                case 0xc00e:
                case 0xc00f:
                case 0xc029:
                case 0xc02a:
                case 0xc031:
                case 0xc032:
                case 0xc078:
                case 0xc079:
                case 0xc08c:
                case 0xc08d:
                    return 0x12;

                case 0xc010:
                case 0xc011:
                case 0xc012:
                case 0xc013:
                case 0xc014:
                case 0xc027:
                case 0xc028:
                case 0xc02f:
                case 0xc030:
                case 0xc076:
                case 0xc077:
                case 0xc08a:
                case 0xc08b:
                    goto Label_0558;

                case 0xc015:
                case 0xc016:
                case 0xc017:
                case 0xc018:
                case 0xc019:
                    return 20;

                case 0xc01a:
                case 0xc01d:
                case 0xc020:
                    return 0x15;

                case 0xc01b:
                case 0xc01e:
                case 0xc021:
                    return 0x17;

                case 0xc01c:
                case 0xc01f:
                case 0xc022:
                    return 0x16;

                case 0xc033:
                case 0xc034:
                case 0xc035:
                case 0xc036:
                case 0xc037:
                case 0xc038:
                case 0xc039:
                case 0xc03a:
                case 0xc03b:
                case 0xc09a:
                case 0xc09b:
                    goto Label_0555;

                case 0xc07a:
                case 0xc07b:
                case 0xc09c:
                case 0xc09d:
                case 0xc0a0:
                case 0xc0a1:
                case 0x84:
                case 150:
                case 0x9c:
                case 0x9d:
                case 0xba:
                case 0xc0:
                case 0x2f:
                case 0x35:
                case 0x3b:
                case 60:
                case 0x3d:
                case 0x41:
                    goto Label_055E;

                case 0xc07c:
                case 0xc07d:
                case 0xc09e:
                case 0xc09f:
                case 0xc0a2:
                case 0xc0a3:
                case 0x88:
                case 0x9a:
                case 0x9e:
                case 0x9f:
                case 190:
                case 0xc4:
                case 0x33:
                case 0x39:
                case 0x45:
                    goto Label_0547;

                case 0xc07e:
                case 0xc07f:
                case 0x86:
                case 0x98:
                case 160:
                case 0xa1:
                case 0xbc:
                case 0xc2:
                case 0x31:
                case 0x37:
                case 0x3f:
                case 0x43:
                    goto Label_053F;

                case 0xc080:
                case 0xc081:
                case 0x87:
                case 0x99:
                case 0xa2:
                case 0xa3:
                case 0xbd:
                case 0xc3:
                case 50:
                case 0x38:
                case 0x40:
                case 0x44:
                    goto Label_0542;

                case 0xc082:
                case 0xc083:
                case 0x85:
                case 0x97:
                case 0xa4:
                case 0xa5:
                case 0xbb:
                case 0xc1:
                case 0x30:
                case 0x36:
                case 0x3e:
                case 0x42:
                    break;

                case 0xc08e:
                case 0xc08f:
                case 0xc094:
                case 0xc095:
                case 0xc0a4:
                case 0xc0a5:
                case 0xc0a8:
                case 0xc0a9:
                case 0x8a:
                case 0x8b:
                case 140:
                case 0x8d:
                case 0xa8:
                case 0xa9:
                case 0xae:
                case 0xaf:
                case 0xb0:
                case 0xb1:
                case 0x2c:
                    goto Label_055B;

                case 0xc090:
                case 0xc091:
                case 0xc096:
                case 0xc097:
                case 0xc0a6:
                case 0xc0a7:
                case 0xc0aa:
                case 0xc0ab:
                case 0x8e:
                case 0x8f:
                case 0x90:
                case 0x91:
                case 170:
                case 0xab:
                case 0xb2:
                case 0xb3:
                case 180:
                case 0xb5:
                case 0x2d:
                    goto Label_0544;

                case 0xc092:
                case 0xc093:
                case 0xc098:
                case 0xc099:
                case 0x92:
                case 0x93:
                case 0x94:
                case 0x95:
                case 0xac:
                case 0xad:
                case 0xb6:
                case 0xb7:
                case 0xb8:
                case 0xb9:
                case 0x2e:
                    return 15;

                default:
                    switch (ciphersuite)
                    {
                        case 0xcca8:
                            goto Label_0558;

                        case 0xcca9:
                            goto Label_0552;

                        case 0xccaa:
                            goto Label_0547;

                        case 0xccab:
                            goto Label_055B;

                        case 0xccac:
                            goto Label_0555;

                        case 0xccad:
                            goto Label_0544;

                        case 0xccae:
                            goto Label_055E;

                        default:
                            switch (ciphersuite)
                            {
                                case 0xff00:
                                case 0xff01:
                                    goto Label_0547;

                                case 0xff02:
                                case 0xff03:
                                    goto Label_0558;

                                case 0xff04:
                                case 0xff05:
                                    goto Label_054C;

                                case 0xff10:
                                case 0xff11:
                                    goto Label_055B;

                                case 0xff12:
                                case 0xff13:
                                    goto Label_0544;

                                case 0xff14:
                                case 0xff15:
                                    goto Label_0555;

                                case 1:
                                case 2:
                                case 4:
                                case 5:
                                case 10:
                                    goto Label_055E;
                            }
                            break;
                    }
                    switch (ciphersuite)
                    {
                        case 0x67:
                        case 0x6b:
                            goto Label_0547;

                        case 0x68:
                            break;

                        case 0x69:
                            goto Label_053F;

                        case 0x6a:
                            goto Label_0542;

                        default:
                            switch (ciphersuite)
                            {
                                case 0x10:
                                    goto Label_053F;
                            }
                            break;
                    }
                    switch (ciphersuite)
                    {
                        case 0x13:
                            goto Label_0542;

                        case 20:
                        case 0x15:
                            goto Label_056C;

                        case 0x16:
                            goto Label_0547;

                        default:
                            goto Label_056C;
                    }
                    break;
            }
            return 7;
        Label_053F:
            return 9;
        Label_0542:
            return 3;
        Label_0544:
            return 14;
        Label_0547:
            return 5;
        Label_054C:
            return 0x10;
        Label_0552:
            return 0x11;
        Label_0555:
            return 0x18;
        Label_0558:
            return 0x13;
        Label_055B:
            return 13;
        Label_055E:
            return 1;
        Label_056C:
            throw new TlsFatalAlert(80);
        }

        public static int GetMacAlgorithm(int ciphersuite)
        {
            switch (ciphersuite)
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
                case 0xc01a:
                case 0xc01b:
                case 0xc01c:
                case 0xc01d:
                case 0xc01e:
                case 0xc01f:
                case 0xc020:
                case 0xc021:
                case 0xc022:
                case 0xc033:
                case 0xc034:
                case 0xc035:
                case 0xc036:
                case 0xc039:
                case 0x84:
                case 0x85:
                case 0x86:
                case 0x87:
                case 0x88:
                case 0x8a:
                case 0x8b:
                case 140:
                case 0x8d:
                case 0x8e:
                case 0x8f:
                case 0x90:
                case 0x91:
                case 0x92:
                case 0x93:
                case 0x94:
                case 0x95:
                case 150:
                case 0x97:
                case 0x98:
                case 0x99:
                case 0x9a:
                case 0x2c:
                case 0x2d:
                case 0x2e:
                case 0x2f:
                case 0x30:
                case 0x31:
                case 50:
                case 0x33:
                case 0x35:
                case 0x36:
                case 0x37:
                case 0x38:
                case 0x39:
                case 0x41:
                case 0x42:
                case 0x43:
                case 0x44:
                case 0x45:
                    goto Label_0541;

                case 0xc023:
                case 0xc025:
                case 0xc027:
                case 0xc029:
                case 0xc037:
                case 0xc03a:
                case 0xc072:
                case 0xc074:
                case 0xc076:
                case 0xc078:
                case 0xc094:
                case 0xc096:
                case 0xc098:
                case 0xc09a:
                case 0xae:
                case 0xb0:
                case 0xb2:
                case 180:
                case 0xb6:
                case 0xb8:
                case 0xba:
                case 0xbb:
                case 0xbc:
                case 0xbd:
                case 190:
                case 0xc0:
                case 0xc1:
                case 0xc2:
                case 0xc3:
                case 0xc4:
                case 0x3b:
                case 60:
                case 0x3d:
                case 0x3e:
                case 0x3f:
                case 0x40:
                    goto Label_0543;

                case 0xc024:
                case 0xc026:
                case 0xc028:
                case 0xc02a:
                case 0xc038:
                case 0xc03b:
                case 0xc073:
                case 0xc075:
                case 0xc077:
                case 0xc079:
                case 0xc095:
                case 0xc097:
                case 0xc099:
                case 0xc09b:
                case 0xaf:
                case 0xb1:
                case 0xb3:
                case 0xb5:
                case 0xb7:
                case 0xb9:
                    return 4;

                case 0xc02b:
                case 0xc02c:
                case 0xc02d:
                case 0xc02e:
                case 0xc02f:
                case 0xc030:
                case 0xc031:
                case 0xc032:
                case 0xc07a:
                case 0xc07b:
                case 0xc07c:
                case 0xc07d:
                case 0xc07e:
                case 0xc07f:
                case 0xc080:
                case 0xc081:
                case 0xc082:
                case 0xc083:
                case 0xc086:
                case 0xc087:
                case 0xc088:
                case 0xc089:
                case 0xc08a:
                case 0xc08b:
                case 0xc08c:
                case 0xc08d:
                case 0xc08e:
                case 0xc08f:
                case 0xc090:
                case 0xc091:
                case 0xc092:
                case 0xc093:
                case 0xc09c:
                case 0xc09d:
                case 0xc09e:
                case 0xc09f:
                case 0xc0a0:
                case 0xc0a1:
                case 0xc0a2:
                case 0xc0a3:
                case 0xc0a4:
                case 0xc0a5:
                case 0xc0a6:
                case 0xc0a7:
                case 0xc0a8:
                case 0xc0a9:
                case 0xc0aa:
                case 0xc0ab:
                case 0xc0ac:
                case 0xc0ad:
                case 0xc0ae:
                case 0xc0af:
                case 0x9c:
                case 0x9d:
                case 0x9e:
                case 0x9f:
                case 160:
                case 0xa1:
                case 0xa2:
                case 0xa3:
                case 0xa4:
                case 0xa5:
                case 0xa8:
                case 0xa9:
                case 170:
                case 0xab:
                case 0xac:
                case 0xad:
                    break;

                default:
                    switch (ciphersuite)
                    {
                        case 0xcca8:
                        case 0xcca9:
                        case 0xccaa:
                        case 0xccab:
                        case 0xccac:
                        case 0xccad:
                        case 0xccae:
                            break;

                        default:
                            switch (ciphersuite)
                            {
                                case 1:
                                case 4:
                                    return 1;

                                case 2:
                                case 5:
                                case 10:
                                    goto Label_0541;
                            }
                            break;
                    }
                    switch (ciphersuite)
                    {
                        case 0x67:
                        case 0x68:
                        case 0x69:
                        case 0x6a:
                        case 0x6b:
                            goto Label_0543;

                        default:
                            switch (ciphersuite)
                            {
                                case 13:
                                case 0x10:
                                    goto Label_0541;
                            }
                            break;
                    }
                    switch (ciphersuite)
                    {
                        case 0x13:
                        case 0x16:
                            goto Label_0541;

                        case 20:
                        case 0x15:
                            goto Label_0547;

                        default:
                            goto Label_0547;
                    }
                    break;
            }
            return 0;
        Label_0541:
            return 2;
        Label_0543:
            return 3;
        Label_0547:
            throw new TlsFatalAlert(80);
        }

        public static ProtocolVersion GetMinimumVersion(int ciphersuite)
        {
            switch (ciphersuite)
            {
                case 0xc072:
                case 0xc073:
                case 0xc074:
                case 0xc075:
                case 0xc076:
                case 0xc077:
                case 0xc078:
                case 0xc079:
                case 0xc07a:
                case 0xc07b:
                case 0xc07c:
                case 0xc07d:
                case 0xc07e:
                case 0xc07f:
                case 0xc080:
                case 0xc081:
                case 0xc082:
                case 0xc083:
                case 0xc084:
                case 0xc085:
                case 0xc086:
                case 0xc087:
                case 0xc088:
                case 0xc089:
                case 0xc08a:
                case 0xc08b:
                case 0xc08c:
                case 0xc08d:
                case 0xc08e:
                case 0xc08f:
                case 0xc090:
                case 0xc091:
                case 0xc092:
                case 0xc093:
                case 0xc09c:
                case 0xc09d:
                case 0xc09e:
                case 0xc09f:
                case 0xc0a0:
                case 0xc0a1:
                case 0xc0a2:
                case 0xc0a3:
                case 0xc0a4:
                case 0xc0a5:
                case 0xc0a6:
                case 0xc0a7:
                case 0xc0a8:
                case 0xc0a9:
                case 0xc0aa:
                case 0xc0ab:
                case 0xc0ac:
                case 0xc0ad:
                case 0xc0ae:
                case 0xc0af:
                case 0x9c:
                case 0x9d:
                case 0x9e:
                case 0x9f:
                case 160:
                case 0xa1:
                case 0xa2:
                case 0xa3:
                case 0xa4:
                case 0xa5:
                case 0xa8:
                case 0xa9:
                case 170:
                case 0xab:
                case 0xac:
                case 0xad:
                case 0xba:
                case 0xbb:
                case 0xbc:
                case 0xbd:
                case 190:
                case 0xbf:
                case 0xc0:
                case 0xc1:
                case 0xc2:
                case 0xc3:
                case 0xc4:
                case 0xc5:
                    break;

                default:
                    switch (ciphersuite)
                    {
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
                            break;

                        default:
                            return ProtocolVersion.SSLv3;
                    }
                    break;
            }
            return ProtocolVersion.TLSv12;
        }

        public static DerObjectIdentifier GetOidForHashAlgorithm(byte hashAlgorithm)
        {
            switch (hashAlgorithm)
            {
                case 1:
                    return PkcsObjectIdentifiers.MD5;

                case 2:
                    return X509ObjectIdentifiers.IdSha1;

                case 3:
                    return NistObjectIdentifiers.IdSha224;

                case 4:
                    return NistObjectIdentifiers.IdSha256;

                case 5:
                    return NistObjectIdentifiers.IdSha384;

                case 6:
                    return NistObjectIdentifiers.IdSha512;
            }
            throw new ArgumentException("unknown HashAlgorithm", "hashAlgorithm");
        }

        public static IList GetSignatureAlgorithmsExtension(IDictionary extensions)
        {
            byte[] extensionData = GetExtensionData(extensions, 13);
            return ((extensionData != null) ? ReadSignatureAlgorithmsExtension(extensionData) : null);
        }

        public static SignatureAndHashAlgorithm GetSignatureAndHashAlgorithm(TlsContext context, TlsSignerCredentials signerCredentials)
        {
            SignatureAndHashAlgorithm signatureAndHashAlgorithm = null;
            if (IsTlsV12(context))
            {
                signatureAndHashAlgorithm = signerCredentials.SignatureAndHashAlgorithm;
                if (signatureAndHashAlgorithm == null)
                {
                    throw new TlsFatalAlert(80);
                }
            }
            return signatureAndHashAlgorithm;
        }

        public static bool HasExpectedEmptyExtensionData(IDictionary extensions, int extensionType, byte alertDescription)
        {
            byte[] extensionData = GetExtensionData(extensions, extensionType);
            if (extensionData == null)
            {
                return false;
            }
            if (extensionData.Length != 0)
            {
                throw new TlsFatalAlert(alertDescription);
            }
            return true;
        }

        public static bool HasSigningCapability(byte clientCertificateType)
        {
            if (((clientCertificateType != 1) && (clientCertificateType != 2)) && (clientCertificateType != 0x40))
            {
                return false;
            }
            return true;
        }

        internal static void HMacHash(IDigest digest, byte[] secret, byte[] seed, byte[] output)
        {
            HMac mac = new HMac(digest);
            mac.Init(new KeyParameter(secret));
            byte[] input = seed;
            int digestSize = digest.GetDigestSize();
            int num2 = ((output.Length + digestSize) - 1) / digestSize;
            byte[] buffer2 = new byte[mac.GetMacSize()];
            byte[] buffer3 = new byte[mac.GetMacSize()];
            for (int i = 0; i < num2; i++)
            {
                mac.BlockUpdate(input, 0, input.Length);
                mac.DoFinal(buffer2, 0);
                input = buffer2;
                mac.BlockUpdate(input, 0, input.Length);
                mac.BlockUpdate(seed, 0, seed.Length);
                mac.DoFinal(buffer3, 0);
                Array.Copy(buffer3, 0, output, digestSize * i, Math.Min(digestSize, output.Length - (digestSize * i)));
            }
        }

        public static TlsSession ImportSession(byte[] sessionID, SessionParameters sessionParameters) => 
            new TlsSessionImpl(sessionID, sessionParameters);

        public static bool IsAeadCipherSuite(int ciphersuite) => 
            (2 == GetCipherType(ciphersuite));

        public static bool IsBlockCipherSuite(int ciphersuite) => 
            (1 == GetCipherType(ciphersuite));

        public static bool IsSignatureAlgorithmsExtensionAllowed(ProtocolVersion clientVersion) => 
            ProtocolVersion.TLSv12.IsEqualOrEarlierVersionOf(clientVersion.GetEquivalentTLSVersion());

        public static bool IsSsl(TlsContext context) => 
            context.ServerVersion.IsSsl;

        public static bool IsStreamCipherSuite(int ciphersuite) => 
            (0 == GetCipherType(ciphersuite));

        public static bool IsTlsV11(ProtocolVersion version) => 
            ProtocolVersion.TLSv11.IsEqualOrEarlierVersionOf(version.GetEquivalentTLSVersion());

        public static bool IsTlsV11(TlsContext context) => 
            IsTlsV11(context.ServerVersion);

        public static bool IsTlsV12(ProtocolVersion version) => 
            ProtocolVersion.TLSv12.IsEqualOrEarlierVersionOf(version.GetEquivalentTLSVersion());

        public static bool IsTlsV12(TlsContext context) => 
            IsTlsV12(context.ServerVersion);

        public static bool IsValidCipherSuiteForVersion(int cipherSuite, ProtocolVersion serverVersion) => 
            GetMinimumVersion(cipherSuite).IsEqualOrEarlierVersionOf(serverVersion.GetEquivalentTLSVersion());

        public static bool IsValidUint16(int i) => 
            ((i & 0xffff) == i);

        public static bool IsValidUint16(long i) => 
            ((i & 0xffffL) == i);

        public static bool IsValidUint24(int i) => 
            ((i & 0xffffff) == i);

        public static bool IsValidUint24(long i) => 
            ((i & 0xffffffL) == i);

        public static bool IsValidUint32(long i) => 
            ((((ulong) i) & 0xffffffffL) == i);

        public static bool IsValidUint48(long i) => 
            ((i & 0xffffffffffffL) == i);

        public static bool IsValidUint64(long i) => 
            true;

        public static bool IsValidUint8(int i) => 
            ((i & 0xff) == i);

        public static bool IsValidUint8(long i) => 
            ((i & 0xffL) == i);

        public static IList ParseSupportedSignatureAlgorithms(bool allowAnonymous, Stream input)
        {
            int num = ReadUint16(input);
            if ((num < 2) || ((num & 1) != 0))
            {
                throw new TlsFatalAlert(50);
            }
            int capacity = num / 2;
            IList list = Platform.CreateArrayList(capacity);
            for (int i = 0; i < capacity; i++)
            {
                SignatureAndHashAlgorithm algorithm = SignatureAndHashAlgorithm.Parse(input);
                if (!allowAnonymous && (algorithm.Signature == 0))
                {
                    throw new TlsFatalAlert(0x2f);
                }
                list.Add(algorithm);
            }
            return list;
        }

        public static byte[] PRF(TlsContext context, byte[] secret, string asciiLabel, byte[] seed, int size)
        {
            if (context.ServerVersion.IsSsl)
            {
                throw new InvalidOperationException("No PRF available for SSLv3 session");
            }
            byte[] a = Strings.ToByteArray(asciiLabel);
            byte[] labelSeed = Concat(a, seed);
            int prfAlgorithm = context.SecurityParameters.PrfAlgorithm;
            if (prfAlgorithm == 0)
            {
                return PRF_legacy(secret, a, labelSeed, size);
            }
            IDigest digest = CreatePrfHash(prfAlgorithm);
            byte[] output = new byte[size];
            HMacHash(digest, secret, labelSeed, output);
            return output;
        }

        public static byte[] PRF_legacy(byte[] secret, string asciiLabel, byte[] seed, int size)
        {
            byte[] a = Strings.ToByteArray(asciiLabel);
            byte[] labelSeed = Concat(a, seed);
            return PRF_legacy(secret, a, labelSeed, size);
        }

        internal static byte[] PRF_legacy(byte[] secret, byte[] label, byte[] labelSeed, int size)
        {
            int length = (secret.Length + 1) / 2;
            byte[] destinationArray = new byte[length];
            byte[] buffer2 = new byte[length];
            Array.Copy(secret, 0, destinationArray, 0, length);
            Array.Copy(secret, secret.Length - length, buffer2, 0, length);
            byte[] output = new byte[size];
            byte[] buffer4 = new byte[size];
            HMacHash(CreateHash((byte) 1), destinationArray, labelSeed, output);
            HMacHash(CreateHash((byte) 2), buffer2, labelSeed, buffer4);
            for (int i = 0; i < size; i++)
            {
                output[i] = (byte) (output[i] ^ buffer4[i]);
            }
            return output;
        }

        public static byte[] ReadAllOrNothing(int length, Stream input)
        {
            if (length < 1)
            {
                return EmptyBytes;
            }
            byte[] buf = new byte[length];
            int num = Streams.ReadFully(input, buf);
            if (num == 0)
            {
                return null;
            }
            if (num != length)
            {
                throw new EndOfStreamException();
            }
            return buf;
        }

        public static Asn1Object ReadAsn1Object(byte[] encoding)
        {
            MemoryStream inputStream = new MemoryStream(encoding, false);
            Asn1Object obj2 = new Asn1InputStream(inputStream, encoding.Length).ReadObject();
            if (obj2 == null)
            {
                throw new TlsFatalAlert(50);
            }
            if (inputStream.Position != inputStream.Length)
            {
                throw new TlsFatalAlert(50);
            }
            return obj2;
        }

        public static Asn1Object ReadDerObject(byte[] encoding)
        {
            Asn1Object obj2 = ReadAsn1Object(encoding);
            if (!Arrays.AreEqual(obj2.GetEncoded("DER"), encoding))
            {
                throw new TlsFatalAlert(50);
            }
            return obj2;
        }

        public static byte[] ReadFully(int length, Stream input)
        {
            if (length < 1)
            {
                return EmptyBytes;
            }
            byte[] buf = new byte[length];
            if (length != Streams.ReadFully(input, buf))
            {
                throw new EndOfStreamException();
            }
            return buf;
        }

        public static void ReadFully(byte[] buf, Stream input)
        {
            if (Streams.ReadFully(input, buf, 0, buf.Length) < buf.Length)
            {
                throw new EndOfStreamException();
            }
        }

        public static byte[] ReadOpaque16(Stream input)
        {
            byte[] buf = new byte[ReadUint16(input)];
            ReadFully(buf, input);
            return buf;
        }

        public static byte[] ReadOpaque24(Stream input) => 
            ReadFully(ReadUint24(input), input);

        public static byte[] ReadOpaque8(Stream input)
        {
            byte[] buf = new byte[ReadUint8(input)];
            ReadFully(buf, input);
            return buf;
        }

        public static IList ReadSignatureAlgorithmsExtension(byte[] extensionData)
        {
            if (extensionData == null)
            {
                throw new ArgumentNullException("extensionData");
            }
            MemoryStream input = new MemoryStream(extensionData, false);
            IList list = ParseSupportedSignatureAlgorithms(false, input);
            TlsProtocol.AssertEmpty(input);
            return list;
        }

        public static int ReadUint16(Stream input)
        {
            int num = input.ReadByte();
            int num2 = input.ReadByte();
            if (num2 < 0)
            {
                throw new EndOfStreamException();
            }
            return ((num << 8) | num2);
        }

        public static int ReadUint16(byte[] buf, int offset)
        {
            uint num = (uint) (buf[offset] << 8);
            num |= buf[++offset];
            return (int) num;
        }

        public static int[] ReadUint16Array(int count, Stream input)
        {
            int[] numArray = new int[count];
            for (int i = 0; i < count; i++)
            {
                numArray[i] = ReadUint16(input);
            }
            return numArray;
        }

        public static int ReadUint24(Stream input)
        {
            int num = input.ReadByte();
            int num2 = input.ReadByte();
            int num3 = input.ReadByte();
            if (num3 < 0)
            {
                throw new EndOfStreamException();
            }
            return (((num << 0x10) | (num2 << 8)) | num3);
        }

        public static int ReadUint24(byte[] buf, int offset)
        {
            uint num = (uint) (buf[offset] << 0x10);
            num |= (uint) (buf[++offset] << 8);
            num |= buf[++offset];
            return (int) num;
        }

        public static long ReadUint32(Stream input)
        {
            int num = input.ReadByte();
            int num2 = input.ReadByte();
            int num3 = input.ReadByte();
            int num4 = input.ReadByte();
            if (num4 < 0)
            {
                throw new EndOfStreamException();
            }
            return (long) ((ulong) ((((num << 0x18) | (num2 << 0x10)) | (num3 << 8)) | num4));
        }

        public static long ReadUint32(byte[] buf, int offset)
        {
            uint num = (uint) (buf[offset] << 0x18);
            num |= (uint) (buf[++offset] << 0x10);
            num |= (uint) (buf[++offset] << 8);
            num |= buf[++offset];
            return (long) num;
        }

        public static long ReadUint48(Stream input)
        {
            int num = ReadUint24(input);
            int num2 = ReadUint24(input);
            return (((num & 0xffffffffL) << 0x18) | (num2 & 0xffffffffL));
        }

        public static long ReadUint48(byte[] buf, int offset)
        {
            int num = ReadUint24(buf, offset);
            int num2 = ReadUint24(buf, offset + 3);
            return (((num & 0xffffffffL) << 0x18) | (num2 & 0xffffffffL));
        }

        public static byte ReadUint8(Stream input)
        {
            int num = input.ReadByte();
            if (num < 0)
            {
                throw new EndOfStreamException();
            }
            return (byte) num;
        }

        public static byte ReadUint8(byte[] buf, int offset) => 
            buf[offset];

        public static byte[] ReadUint8Array(int count, Stream input)
        {
            byte[] buffer = new byte[count];
            for (int i = 0; i < count; i++)
            {
                buffer[i] = ReadUint8(input);
            }
            return buffer;
        }

        public static ProtocolVersion ReadVersion(Stream input)
        {
            int major = input.ReadByte();
            int minor = input.ReadByte();
            if (minor < 0)
            {
                throw new EndOfStreamException();
            }
            return ProtocolVersion.Get(major, minor);
        }

        public static ProtocolVersion ReadVersion(byte[] buf, int offset) => 
            ProtocolVersion.Get(buf[offset], buf[offset + 1]);

        public static int ReadVersionRaw(Stream input)
        {
            int num = input.ReadByte();
            int num2 = input.ReadByte();
            if (num2 < 0)
            {
                throw new EndOfStreamException();
            }
            return ((num << 8) | num2);
        }

        public static int ReadVersionRaw(byte[] buf, int offset) => 
            ((buf[offset] << 8) | buf[offset + 1]);

        internal static void TrackHashAlgorithms(TlsHandshakeHash handshakeHash, IList supportedSignatureAlgorithms)
        {
            if (supportedSignatureAlgorithms != null)
            {
                IEnumerator enumerator = supportedSignatureAlgorithms.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        SignatureAndHashAlgorithm current = (SignatureAndHashAlgorithm) enumerator.Current;
                        byte hash = current.Hash;
                        if (!HashAlgorithm.IsPrivate(hash))
                        {
                            handshakeHash.TrackHashAlgorithm(hash);
                        }
                    }
                }
                finally
                {
                    if (enumerator is IDisposable disposable)
                    {
                        IDisposable disposable;
                        disposable.Dispose();
                    }
                }
            }
        }

        internal static void ValidateKeyUsage(X509CertificateStructure c, int keyUsageBits)
        {
            X509Extensions extensions = c.TbsCertificate.Extensions;
            if (extensions != null)
            {
                X509Extension extension = extensions.GetExtension(X509Extensions.KeyUsage);
                if (extension != null)
                {
                    int num = KeyUsage.GetInstance(extension).GetBytes()[0];
                    if ((num & keyUsageBits) != keyUsageBits)
                    {
                        throw new TlsFatalAlert(0x2e);
                    }
                }
            }
        }

        private static IList VectorOfOne(object obj)
        {
            IList list = Platform.CreateArrayList(1);
            list.Add(obj);
            return list;
        }

        public static void VerifySupportedSignatureAlgorithm(IList supportedSignatureAlgorithms, SignatureAndHashAlgorithm signatureAlgorithm)
        {
            if (supportedSignatureAlgorithms == null)
            {
                throw new ArgumentNullException("supportedSignatureAlgorithms");
            }
            if ((supportedSignatureAlgorithms.Count < 1) || (supportedSignatureAlgorithms.Count >= 0x8000))
            {
                throw new ArgumentException("must have length from 1 to (2^15 - 1)", "supportedSignatureAlgorithms");
            }
            if (signatureAlgorithm == null)
            {
                throw new ArgumentNullException("signatureAlgorithm");
            }
            if (signatureAlgorithm.Signature != 0)
            {
                IEnumerator enumerator = supportedSignatureAlgorithms.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        SignatureAndHashAlgorithm current = (SignatureAndHashAlgorithm) enumerator.Current;
                        if ((current.Hash == signatureAlgorithm.Hash) && (current.Signature == signatureAlgorithm.Signature))
                        {
                            return;
                        }
                    }
                }
                finally
                {
                    if (enumerator is IDisposable disposable)
                    {
                        IDisposable disposable;
                        disposable.Dispose();
                    }
                }
            }
            throw new TlsFatalAlert(0x2f);
        }

        public static void WriteGmtUnixTime(byte[] buf, int offset)
        {
            int num = (int) (DateTimeUtilities.CurrentUnixMs() / 0x3e8L);
            buf[offset] = (byte) (num >> 0x18);
            buf[offset + 1] = (byte) (num >> 0x10);
            buf[offset + 2] = (byte) (num >> 8);
            buf[offset + 3] = (byte) num;
        }

        public static void WriteOpaque16(byte[] buf, Stream output)
        {
            WriteUint16(buf.Length, output);
            output.Write(buf, 0, buf.Length);
        }

        public static void WriteOpaque24(byte[] buf, Stream output)
        {
            WriteUint24(buf.Length, output);
            output.Write(buf, 0, buf.Length);
        }

        public static void WriteOpaque8(byte[] buf, Stream output)
        {
            WriteUint8((byte) buf.Length, output);
            output.Write(buf, 0, buf.Length);
        }

        public static void WriteUint16(int i, Stream output)
        {
            output.WriteByte((byte) (i >> 8));
            output.WriteByte((byte) i);
        }

        public static void WriteUint16(int i, byte[] buf, int offset)
        {
            buf[offset] = (byte) (i >> 8);
            buf[offset + 1] = (byte) i;
        }

        public static void WriteUint16Array(int[] uints, Stream output)
        {
            for (int i = 0; i < uints.Length; i++)
            {
                WriteUint16(uints[i], output);
            }
        }

        public static void WriteUint16Array(int[] uints, byte[] buf, int offset)
        {
            for (int i = 0; i < uints.Length; i++)
            {
                WriteUint16(uints[i], buf, offset);
                offset += 2;
            }
        }

        public static void WriteUint16ArrayWithUint16Length(int[] uints, Stream output)
        {
            int i = 2 * uints.Length;
            CheckUint16(i);
            WriteUint16(i, output);
            WriteUint16Array(uints, output);
        }

        public static void WriteUint16ArrayWithUint16Length(int[] uints, byte[] buf, int offset)
        {
            int i = 2 * uints.Length;
            CheckUint16(i);
            WriteUint16(i, buf, offset);
            WriteUint16Array(uints, buf, offset + 2);
        }

        public static void WriteUint24(int i, Stream output)
        {
            output.WriteByte((byte) (i >> 0x10));
            output.WriteByte((byte) (i >> 8));
            output.WriteByte((byte) i);
        }

        public static void WriteUint24(int i, byte[] buf, int offset)
        {
            buf[offset] = (byte) (i >> 0x10);
            buf[offset + 1] = (byte) (i >> 8);
            buf[offset + 2] = (byte) i;
        }

        public static void WriteUint32(long i, Stream output)
        {
            output.WriteByte((byte) (i >> 0x18));
            output.WriteByte((byte) (i >> 0x10));
            output.WriteByte((byte) (i >> 8));
            output.WriteByte((byte) i);
        }

        public static void WriteUint32(long i, byte[] buf, int offset)
        {
            buf[offset] = (byte) (i >> 0x18);
            buf[offset + 1] = (byte) (i >> 0x10);
            buf[offset + 2] = (byte) (i >> 8);
            buf[offset + 3] = (byte) i;
        }

        public static void WriteUint48(long i, Stream output)
        {
            output.WriteByte((byte) (i >> 40));
            output.WriteByte((byte) (i >> 0x20));
            output.WriteByte((byte) (i >> 0x18));
            output.WriteByte((byte) (i >> 0x10));
            output.WriteByte((byte) (i >> 8));
            output.WriteByte((byte) i);
        }

        public static void WriteUint48(long i, byte[] buf, int offset)
        {
            buf[offset] = (byte) (i >> 40);
            buf[offset + 1] = (byte) (i >> 0x20);
            buf[offset + 2] = (byte) (i >> 0x18);
            buf[offset + 3] = (byte) (i >> 0x10);
            buf[offset + 4] = (byte) (i >> 8);
            buf[offset + 5] = (byte) i;
        }

        public static void WriteUint64(long i, Stream output)
        {
            output.WriteByte((byte) (i >> 0x38));
            output.WriteByte((byte) (i >> 0x30));
            output.WriteByte((byte) (i >> 40));
            output.WriteByte((byte) (i >> 0x20));
            output.WriteByte((byte) (i >> 0x18));
            output.WriteByte((byte) (i >> 0x10));
            output.WriteByte((byte) (i >> 8));
            output.WriteByte((byte) i);
        }

        public static void WriteUint64(long i, byte[] buf, int offset)
        {
            buf[offset] = (byte) (i >> 0x38);
            buf[offset + 1] = (byte) (i >> 0x30);
            buf[offset + 2] = (byte) (i >> 40);
            buf[offset + 3] = (byte) (i >> 0x20);
            buf[offset + 4] = (byte) (i >> 0x18);
            buf[offset + 5] = (byte) (i >> 0x10);
            buf[offset + 6] = (byte) (i >> 8);
            buf[offset + 7] = (byte) i;
        }

        public static void WriteUint8(byte i, Stream output)
        {
            output.WriteByte(i);
        }

        public static void WriteUint8(byte i, byte[] buf, int offset)
        {
            buf[offset] = i;
        }

        public static void WriteUint8Array(byte[] uints, Stream output)
        {
            output.Write(uints, 0, uints.Length);
        }

        public static void WriteUint8Array(byte[] uints, byte[] buf, int offset)
        {
            for (int i = 0; i < uints.Length; i++)
            {
                WriteUint8(uints[i], buf, offset);
                offset++;
            }
        }

        public static void WriteUint8ArrayWithUint8Length(byte[] uints, Stream output)
        {
            CheckUint8(uints.Length);
            WriteUint8((byte) uints.Length, output);
            WriteUint8Array(uints, output);
        }

        public static void WriteUint8ArrayWithUint8Length(byte[] uints, byte[] buf, int offset)
        {
            CheckUint8(uints.Length);
            WriteUint8((byte) uints.Length, buf, offset);
            WriteUint8Array(uints, buf, offset + 1);
        }

        public static void WriteVersion(ProtocolVersion version, Stream output)
        {
            output.WriteByte((byte) version.MajorVersion);
            output.WriteByte((byte) version.MinorVersion);
        }

        public static void WriteVersion(ProtocolVersion version, byte[] buf, int offset)
        {
            buf[offset] = (byte) version.MajorVersion;
            buf[offset + 1] = (byte) version.MinorVersion;
        }
    }
}

