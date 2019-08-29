namespace Org.BouncyCastle.Crypto.Tls
{
    using System;
    using System.Collections.Generic;

    public sealed class LegacyTlsClient : DefaultTlsClient
    {
        private readonly Uri TargetUri;
        private readonly ICertificateVerifyer verifyer;
        private readonly IClientCredentialsProvider credProvider;

        public LegacyTlsClient(Uri targetUri, ICertificateVerifyer verifyer, IClientCredentialsProvider prov, List<string> hostNames)
        {
            this.TargetUri = targetUri;
            this.verifyer = verifyer;
            this.credProvider = prov;
            base.HostNames = hostNames;
        }

        public override TlsAuthentication GetAuthentication() => 
            new LegacyTlsAuthentication(this.TargetUri, this.verifyer, this.credProvider);
    }
}

