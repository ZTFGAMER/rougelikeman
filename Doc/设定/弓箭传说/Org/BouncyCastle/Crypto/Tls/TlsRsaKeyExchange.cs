namespace Org.BouncyCastle.Crypto.Tls
{
    using Org.BouncyCastle.Asn1.X509;
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Security;
    using Org.BouncyCastle.Utilities.IO;
    using System;
    using System.Collections;
    using System.IO;

    public class TlsRsaKeyExchange : AbstractTlsKeyExchange
    {
        protected AsymmetricKeyParameter mServerPublicKey;
        protected RsaKeyParameters mRsaServerPublicKey;
        protected TlsEncryptionCredentials mServerCredentials;
        protected byte[] mPremasterSecret;

        public TlsRsaKeyExchange(IList supportedSignatureAlgorithms) : base(1, supportedSignatureAlgorithms)
        {
        }

        public override void GenerateClientKeyExchange(Stream output)
        {
            this.mPremasterSecret = TlsRsaUtilities.GenerateEncryptedPreMasterSecret(base.mContext, this.mRsaServerPublicKey, output);
        }

        public override byte[] GeneratePremasterSecret()
        {
            if (this.mPremasterSecret == null)
            {
                throw new TlsFatalAlert(80);
            }
            byte[] mPremasterSecret = this.mPremasterSecret;
            this.mPremasterSecret = null;
            return mPremasterSecret;
        }

        public override void ProcessClientCredentials(TlsCredentials clientCredentials)
        {
            if (!(clientCredentials is TlsSignerCredentials))
            {
                throw new TlsFatalAlert(80);
            }
        }

        public override void ProcessClientKeyExchange(Stream input)
        {
            byte[] buffer;
            if (TlsUtilities.IsSsl(base.mContext))
            {
                buffer = Streams.ReadAll(input);
            }
            else
            {
                buffer = TlsUtilities.ReadOpaque16(input);
            }
            this.mPremasterSecret = this.mServerCredentials.DecryptPreMasterSecret(buffer);
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
            if (this.mServerPublicKey.IsPrivate)
            {
                throw new TlsFatalAlert(80);
            }
            this.mRsaServerPublicKey = this.ValidateRsaPublicKey((RsaKeyParameters) this.mServerPublicKey);
            TlsUtilities.ValidateKeyUsage(certificateAt, 0x20);
            base.ProcessServerCertificate(serverCertificate);
        }

        public override void ProcessServerCredentials(TlsCredentials serverCredentials)
        {
            if (!(serverCredentials is TlsEncryptionCredentials))
            {
                throw new TlsFatalAlert(80);
            }
            this.ProcessServerCertificate(serverCredentials.Certificate);
            this.mServerCredentials = (TlsEncryptionCredentials) serverCredentials;
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
                    case 0x40:
                        break;

                    default:
                        throw new TlsFatalAlert(0x2f);
                }
            }
        }

        protected virtual RsaKeyParameters ValidateRsaPublicKey(RsaKeyParameters key)
        {
            if (!key.Exponent.IsProbablePrime(2))
            {
                throw new TlsFatalAlert(0x2f);
            }
            return key;
        }
    }
}

