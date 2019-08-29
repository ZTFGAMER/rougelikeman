namespace Org.BouncyCastle.Crypto.Tls
{
    using Org.BouncyCastle.Asn1.X509;
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Security;
    using System;
    using System.Collections;
    using System.IO;

    public class TlsECDHKeyExchange : AbstractTlsKeyExchange
    {
        protected TlsSigner mTlsSigner;
        protected int[] mNamedCurves;
        protected byte[] mClientECPointFormats;
        protected byte[] mServerECPointFormats;
        protected AsymmetricKeyParameter mServerPublicKey;
        protected TlsAgreementCredentials mAgreementCredentials;
        protected ECPrivateKeyParameters mECAgreePrivateKey;
        protected ECPublicKeyParameters mECAgreePublicKey;

        public TlsECDHKeyExchange(int keyExchange, IList supportedSignatureAlgorithms, int[] namedCurves, byte[] clientECPointFormats, byte[] serverECPointFormats) : base(keyExchange, supportedSignatureAlgorithms)
        {
            switch (keyExchange)
            {
                case 0x10:
                case 0x12:
                case 20:
                    this.mTlsSigner = null;
                    break;

                case 0x11:
                    this.mTlsSigner = new TlsECDsaSigner();
                    break;

                case 0x13:
                    this.mTlsSigner = new TlsRsaSigner();
                    break;

                default:
                    throw new InvalidOperationException("unsupported key exchange algorithm");
            }
            this.mNamedCurves = namedCurves;
            this.mClientECPointFormats = clientECPointFormats;
            this.mServerECPointFormats = serverECPointFormats;
        }

        public override void GenerateClientKeyExchange(Stream output)
        {
            if (this.mAgreementCredentials == null)
            {
                this.mECAgreePrivateKey = TlsEccUtilities.GenerateEphemeralClientKeyExchange(base.mContext.SecureRandom, this.mServerECPointFormats, this.mECAgreePublicKey.Parameters, output);
            }
        }

        public override byte[] GeneratePremasterSecret()
        {
            if (this.mAgreementCredentials != null)
            {
                return this.mAgreementCredentials.GenerateAgreement(this.mECAgreePublicKey);
            }
            if (this.mECAgreePrivateKey == null)
            {
                throw new TlsFatalAlert(80);
            }
            return TlsEccUtilities.CalculateECDHBasicAgreement(this.mECAgreePublicKey, this.mECAgreePrivateKey);
        }

        public override byte[] GenerateServerKeyExchange()
        {
            if (!this.RequiresServerKeyExchange)
            {
                return null;
            }
            MemoryStream output = new MemoryStream();
            this.mECAgreePrivateKey = TlsEccUtilities.GenerateEphemeralServerKeyExchange(base.mContext.SecureRandom, this.mNamedCurves, this.mClientECPointFormats, output);
            return output.ToArray();
        }

        public override void Init(TlsContext context)
        {
            base.Init(context);
            if (this.mTlsSigner != null)
            {
                this.mTlsSigner.Init(context);
            }
        }

        public override void ProcessClientCertificate(Certificate clientCertificate)
        {
            if (base.mKeyExchange == 20)
            {
                throw new TlsFatalAlert(10);
            }
        }

        public override void ProcessClientCredentials(TlsCredentials clientCredentials)
        {
            if (base.mKeyExchange == 20)
            {
                throw new TlsFatalAlert(80);
            }
            if (clientCredentials is TlsAgreementCredentials)
            {
                this.mAgreementCredentials = (TlsAgreementCredentials) clientCredentials;
            }
            else if (!(clientCredentials is TlsSignerCredentials))
            {
                throw new TlsFatalAlert(80);
            }
        }

        public override void ProcessClientKeyExchange(Stream input)
        {
            if (this.mECAgreePublicKey == null)
            {
                byte[] encoding = TlsUtilities.ReadOpaque8(input);
                ECDomainParameters parameters = this.mECAgreePrivateKey.Parameters;
                this.mECAgreePublicKey = TlsEccUtilities.ValidateECPublicKey(TlsEccUtilities.DeserializeECPublicKey(this.mServerECPointFormats, parameters, encoding));
            }
        }

        public override void ProcessServerCertificate(Certificate serverCertificate)
        {
            if (base.mKeyExchange == 20)
            {
                throw new TlsFatalAlert(10);
            }
            if (serverCertificate.IsEmpty)
            {
                throw new TlsFatalAlert(0x2a);
            }
            X509CertificateStructure certificateAt = serverCertificate.GetCertificateAt(0);
            SubjectPublicKeyInfo subjectPublicKeyInfo = certificateAt.SubjectPublicKeyInfo;
            try
            {
                this.mServerPublicKey = PublicKeyFactory.CreateKey(subjectPublicKeyInfo);
            }
            catch (Exception exception)
            {
                throw new TlsFatalAlert(0x2b, exception);
            }
            if (this.mTlsSigner == null)
            {
                try
                {
                    this.mECAgreePublicKey = TlsEccUtilities.ValidateECPublicKey((ECPublicKeyParameters) this.mServerPublicKey);
                }
                catch (InvalidCastException exception2)
                {
                    throw new TlsFatalAlert(0x2e, exception2);
                }
                TlsUtilities.ValidateKeyUsage(certificateAt, 8);
            }
            else
            {
                if (!this.mTlsSigner.IsValidPublicKey(this.mServerPublicKey))
                {
                    throw new TlsFatalAlert(0x2e);
                }
                TlsUtilities.ValidateKeyUsage(certificateAt, 0x80);
            }
            base.ProcessServerCertificate(serverCertificate);
        }

        public override void ProcessServerKeyExchange(Stream input)
        {
            if (!this.RequiresServerKeyExchange)
            {
                throw new TlsFatalAlert(10);
            }
            ECDomainParameters parameters = TlsEccUtilities.ReadECParameters(this.mNamedCurves, this.mClientECPointFormats, input);
            byte[] encoding = TlsUtilities.ReadOpaque8(input);
            this.mECAgreePublicKey = TlsEccUtilities.ValidateECPublicKey(TlsEccUtilities.DeserializeECPublicKey(this.mClientECPointFormats, parameters, encoding));
        }

        public override void SkipServerCredentials()
        {
            if (base.mKeyExchange != 20)
            {
                throw new TlsFatalAlert(10);
            }
        }

        public override void ValidateCertificateRequest(CertificateRequest certificateRequest)
        {
            byte[] certificateTypes = certificateRequest.CertificateTypes;
            for (int i = 0; i < certificateTypes.Length; i++)
            {
                switch (certificateTypes[i])
                {
                    case 0x40:
                    case 0x41:
                    case 0x42:
                    case 1:
                    case 2:
                        break;

                    default:
                        throw new TlsFatalAlert(0x2f);
                }
            }
        }

        public override bool RequiresServerKeyExchange
        {
            get
            {
                switch (base.mKeyExchange)
                {
                    case 0x11:
                    case 0x13:
                    case 20:
                        return true;
                }
                return false;
            }
        }
    }
}

