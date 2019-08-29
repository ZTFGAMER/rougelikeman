namespace Org.BouncyCastle.Crypto.Tls
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Security;
    using Org.BouncyCastle.Utilities.IO;
    using System;
    using System.Collections;
    using System.IO;

    public class TlsDheKeyExchange : TlsDHKeyExchange
    {
        protected TlsSignerCredentials mServerCredentials;

        public TlsDheKeyExchange(int keyExchange, IList supportedSignatureAlgorithms, DHParameters dhParameters) : base(keyExchange, supportedSignatureAlgorithms, dhParameters)
        {
        }

        public override byte[] GenerateServerKeyExchange()
        {
            if (base.mDHParameters == null)
            {
                throw new TlsFatalAlert(80);
            }
            DigestInputBuffer output = new DigestInputBuffer();
            base.mDHAgreePrivateKey = TlsDHUtilities.GenerateEphemeralServerKeyExchange(base.mContext.SecureRandom, base.mDHParameters, output);
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
            ServerDHParams @params = ServerDHParams.Parse(stream);
            DigitallySigned signed = this.ParseSignature(input);
            ISigner s = this.InitVerifyer(base.mTlsSigner, signed.Algorithm, securityParameters);
            tee.UpdateSigner(s);
            if (!s.VerifySignature(signed.Signature))
            {
                throw new TlsFatalAlert(0x33);
            }
            base.mDHAgreePublicKey = TlsDHUtilities.ValidateDHPublicKey(@params.PublicKey);
            base.mDHParameters = this.ValidateDHParameters(base.mDHAgreePublicKey.Parameters);
        }
    }
}

