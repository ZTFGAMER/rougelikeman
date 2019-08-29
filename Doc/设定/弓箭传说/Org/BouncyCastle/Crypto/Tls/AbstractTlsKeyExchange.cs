namespace Org.BouncyCastle.Crypto.Tls
{
    using System;
    using System.Collections;
    using System.IO;

    public abstract class AbstractTlsKeyExchange : TlsKeyExchange
    {
        protected readonly int mKeyExchange;
        protected IList mSupportedSignatureAlgorithms;
        protected TlsContext mContext;

        protected AbstractTlsKeyExchange(int keyExchange, IList supportedSignatureAlgorithms)
        {
            this.mKeyExchange = keyExchange;
            this.mSupportedSignatureAlgorithms = supportedSignatureAlgorithms;
        }

        public abstract void GenerateClientKeyExchange(Stream output);
        public abstract byte[] GeneratePremasterSecret();
        public virtual byte[] GenerateServerKeyExchange()
        {
            if (this.RequiresServerKeyExchange)
            {
                throw new TlsFatalAlert(80);
            }
            return null;
        }

        public virtual void Init(TlsContext context)
        {
            this.mContext = context;
            ProtocolVersion clientVersion = context.ClientVersion;
            if (!TlsUtilities.IsSignatureAlgorithmsExtensionAllowed(clientVersion))
            {
                if (this.mSupportedSignatureAlgorithms != null)
                {
                    throw new InvalidOperationException("supported_signature_algorithms not allowed for " + clientVersion);
                }
                return;
            }
            if (this.mSupportedSignatureAlgorithms == null)
            {
                int mKeyExchange = this.mKeyExchange;
                switch (mKeyExchange)
                {
                    case 13:
                    case 14:
                    case 0x15:
                    case 0x18:
                        return;

                    case 15:
                    case 0x12:
                    case 0x13:
                    case 0x17:
                        goto Label_00B5;

                    case 0x10:
                    case 0x11:
                        this.mSupportedSignatureAlgorithms = TlsUtilities.GetDefaultECDsaSignatureAlgorithms();
                        return;

                    case 0x16:
                        break;

                    default:
                        switch (mKeyExchange)
                        {
                            case 1:
                            case 5:
                            case 9:
                                goto Label_00B5;

                            case 2:
                            case 4:
                            case 6:
                            case 8:
                                goto Label_00CA;

                            case 3:
                            case 7:
                                break;

                            default:
                                goto Label_00CA;
                        }
                        break;
                }
                this.mSupportedSignatureAlgorithms = TlsUtilities.GetDefaultDssSignatureAlgorithms();
            }
            return;
        Label_00B5:
            this.mSupportedSignatureAlgorithms = TlsUtilities.GetDefaultRsaSignatureAlgorithms();
            return;
        Label_00CA:
            throw new InvalidOperationException("unsupported key exchange algorithm");
        }

        protected virtual DigitallySigned ParseSignature(Stream input)
        {
            DigitallySigned signed = DigitallySigned.Parse(this.mContext, input);
            SignatureAndHashAlgorithm signatureAlgorithm = signed.Algorithm;
            if (signatureAlgorithm != null)
            {
                TlsUtilities.VerifySupportedSignatureAlgorithm(this.mSupportedSignatureAlgorithms, signatureAlgorithm);
            }
            return signed;
        }

        public virtual void ProcessClientCertificate(Certificate clientCertificate)
        {
        }

        public abstract void ProcessClientCredentials(TlsCredentials clientCredentials);
        public virtual void ProcessClientKeyExchange(Stream input)
        {
            throw new TlsFatalAlert(80);
        }

        public virtual void ProcessServerCertificate(Certificate serverCertificate)
        {
            if (this.mSupportedSignatureAlgorithms == null)
            {
            }
        }

        public virtual void ProcessServerCredentials(TlsCredentials serverCredentials)
        {
            this.ProcessServerCertificate(serverCredentials.Certificate);
        }

        public virtual void ProcessServerKeyExchange(Stream input)
        {
            if (!this.RequiresServerKeyExchange)
            {
                throw new TlsFatalAlert(10);
            }
        }

        public virtual void SkipClientCredentials()
        {
        }

        public abstract void SkipServerCredentials();
        public virtual void SkipServerKeyExchange()
        {
            if (this.RequiresServerKeyExchange)
            {
                throw new TlsFatalAlert(10);
            }
        }

        public abstract void ValidateCertificateRequest(CertificateRequest certificateRequest);

        public virtual bool RequiresServerKeyExchange =>
            false;
    }
}

