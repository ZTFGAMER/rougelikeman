namespace Org.BouncyCastle.Crypto.Tls
{
    using System;

    public class LegacyTlsAuthentication : TlsAuthentication
    {
        protected ICertificateVerifyer verifyer;
        protected IClientCredentialsProvider credProvider;
        protected Uri TargetUri;

        public LegacyTlsAuthentication(Uri targetUri, ICertificateVerifyer verifyer, IClientCredentialsProvider prov)
        {
            this.TargetUri = targetUri;
            this.verifyer = verifyer;
            this.credProvider = prov;
        }

        public virtual TlsCredentials GetClientCredentials(TlsContext context, CertificateRequest certificateRequest) => 
            this.credProvider?.GetClientCredentials(context, certificateRequest);

        public virtual void NotifyServerCertificate(Certificate serverCertificate)
        {
            if (!this.verifyer.IsValid(this.TargetUri, serverCertificate.GetCertificateList()))
            {
                throw new TlsFatalAlert(90);
            }
        }
    }
}

