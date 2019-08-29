namespace Org.BouncyCastle.Crypto.Tls
{
    using Org.BouncyCastle.Asn1.X509;
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Security;
    using System;
    using System.Collections;
    using System.IO;

    public class TlsDHKeyExchange : AbstractTlsKeyExchange
    {
        protected TlsSigner mTlsSigner;
        protected DHParameters mDHParameters;
        protected AsymmetricKeyParameter mServerPublicKey;
        protected TlsAgreementCredentials mAgreementCredentials;
        protected DHPrivateKeyParameters mDHAgreePrivateKey;
        protected DHPublicKeyParameters mDHAgreePublicKey;

        public TlsDHKeyExchange(int keyExchange, IList supportedSignatureAlgorithms, DHParameters dhParameters) : base(keyExchange, supportedSignatureAlgorithms)
        {
            switch (keyExchange)
            {
                case 3:
                    this.mTlsSigner = new TlsDssSigner();
                    break;

                case 5:
                    this.mTlsSigner = new TlsRsaSigner();
                    break;

                case 7:
                case 9:
                    this.mTlsSigner = null;
                    break;

                default:
                    throw new InvalidOperationException("unsupported key exchange algorithm");
            }
            this.mDHParameters = dhParameters;
        }

        public override void GenerateClientKeyExchange(Stream output)
        {
            if (this.mAgreementCredentials == null)
            {
                this.mDHAgreePrivateKey = TlsDHUtilities.GenerateEphemeralClientKeyExchange(base.mContext.SecureRandom, this.mDHParameters, output);
            }
        }

        public override byte[] GeneratePremasterSecret()
        {
            if (this.mAgreementCredentials != null)
            {
                return this.mAgreementCredentials.GenerateAgreement(this.mDHAgreePublicKey);
            }
            if (this.mDHAgreePrivateKey == null)
            {
                throw new TlsFatalAlert(80);
            }
            return TlsDHUtilities.CalculateDHBasicAgreement(this.mDHAgreePublicKey, this.mDHAgreePrivateKey);
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
        }

        public override void ProcessClientCredentials(TlsCredentials clientCredentials)
        {
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
            if (this.mDHAgreePublicKey == null)
            {
                BigInteger y = TlsDHUtilities.ReadDHParameter(input);
                this.mDHAgreePublicKey = TlsDHUtilities.ValidateDHPublicKey(new DHPublicKeyParameters(y, this.mDHParameters));
            }
        }

        public override void ProcessServerCertificate(Certificate serverCertificate)
        {
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
                    this.mDHAgreePublicKey = TlsDHUtilities.ValidateDHPublicKey((DHPublicKeyParameters) this.mServerPublicKey);
                    this.mDHParameters = this.ValidateDHParameters(this.mDHAgreePublicKey.Parameters);
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

        public override void SkipServerCredentials()
        {
            throw new TlsFatalAlert(10);
        }

        public override void ValidateCertificateRequest(CertificateRequest certificateRequest)
        {
            byte[] certificateTypes = certificateRequest.CertificateTypes;
            for (int i = 0; i < certificateTypes.Length; i++)
            {
                switch (certificateTypes[i])
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 0x40:
                        break;

                    default:
                        throw new TlsFatalAlert(0x2f);
                }
            }
        }

        protected virtual DHParameters ValidateDHParameters(DHParameters parameters)
        {
            if (parameters.P.BitLength < this.MinimumPrimeBits)
            {
                throw new TlsFatalAlert(0x47);
            }
            return TlsDHUtilities.ValidateDHParameters(parameters);
        }

        public override bool RequiresServerKeyExchange
        {
            get
            {
                switch (base.mKeyExchange)
                {
                    case 3:
                    case 5:
                    case 11:
                        return true;
                }
                return false;
            }
        }

        protected virtual int MinimumPrimeBits =>
            0x400;
    }
}

