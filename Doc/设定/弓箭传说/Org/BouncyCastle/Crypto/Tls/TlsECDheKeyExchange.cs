namespace Org.BouncyCastle.Crypto.Tls
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Security;
    using Org.BouncyCastle.Utilities.IO;
    using System;
    using System.Collections;
    using System.IO;

    public class TlsECDheKeyExchange : TlsECDHKeyExchange
    {
        protected TlsSignerCredentials mServerCredentials;

        public TlsECDheKeyExchange(int keyExchange, IList supportedSignatureAlgorithms, int[] namedCurves, byte[] clientECPointFormats, byte[] serverECPointFormats) : base(keyExchange, supportedSignatureAlgorithms, namedCurves, clientECPointFormats, serverECPointFormats)
        {
        }

        public override byte[] GenerateServerKeyExchange()
        {
            DigestInputBuffer output = new DigestInputBuffer();
            base.mECAgreePrivateKey = TlsEccUtilities.GenerateEphemeralServerKeyExchange(base.mContext.SecureRandom, base.mNamedCurves, base.mClientECPointFormats, output);
            SignatureAndHashAlgorithm signatureAndHashAlgorithm = TlsUtilities.GetSignatureAndHashAlgorithm(base.mContext, this.mServerCredentials);
            IDigest d = TlsUtilities.CreateHash(signatureAndHashAlgorithm);
            SecurityParameters securityParameters = base.mContext.SecurityParameters;
            d.BlockUpdate(securityParameters.clientRandom, 0, securityParameters.clientRandom.Length);
            d.BlockUpdate(securityParameters.serverRandom, 0, securityParameters.serverRandom.Length);
            output.UpdateDigest(d);
            byte[] hash = DigestUtilities.DoFinal(d);
            byte[] signature = this.mServerCredentials.GenerateCertificateSignature(hash);
            new DigitallySigned(signatureAndHashAlgorithm, signature).Encode(output);
            return output.ToArray();
        }

        protected virtual ISigner InitVerifyer(TlsSigner tlsSigner, SignatureAndHashAlgorithm algorithm, SecurityParameters securityParameters)
        {
            ISigner signer = tlsSigner.CreateVerifyer(algorithm, base.mServerPublicKey);
            signer.BlockUpdate(securityParameters.clientRandom, 0, securityParameters.clientRandom.Length);
            signer.BlockUpdate(securityParameters.serverRandom, 0, securityParameters.serverRandom.Length);
            return signer;
        }

        public override void ProcessClientCredentials(TlsCredentials clientCredentials)
        {
            if (!(clientCredentials is TlsSignerCredentials))
            {
                throw new TlsFatalAlert(80);
            }
        }

        public override void ProcessServerCredentials(TlsCredentials serverCredentials)
        {
            if (!(serverCredentials is TlsSignerCredentials))
            {
                throw new TlsFatalAlert(80);
            }
            this.ProcessServerCertificate(serverCredentials.Certificate);
            this.mServerCredentials = (TlsSignerCredentials) serverCredentials;
        }

        public override void ProcessServerKeyExchange(Stream input)
        {
            SecurityParameters securityParameters = base.mContext.SecurityParameters;
            SignerInputBuffer tee = new SignerInputBuffer();
            Stream stream = new TeeInputStream(input, tee);
            ECDomainParameters parameters2 = TlsEccUtilities.ReadECParameters(base.mNamedCurves, base.mClientECPointFormats, stream);
            byte[] encoding = TlsUtilities.ReadOpaque8(stream);
            DigitallySigned signed = this.ParseSignature(input);
            ISigner s = this.InitVerifyer(base.mTlsSigner, signed.Algorithm, securityParameters);
            tee.UpdateSigner(s);
            if (!s.VerifySignature(signed.Signature))
            {
                throw new TlsFatalAlert(0x33);
            }
            base.mECAgreePublicKey = TlsEccUtilities.ValidateECPublicKey(TlsEccUtilities.DeserializeECPublicKey(base.mClientECPointFormats, parameters2, encoding));
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
    }
}

