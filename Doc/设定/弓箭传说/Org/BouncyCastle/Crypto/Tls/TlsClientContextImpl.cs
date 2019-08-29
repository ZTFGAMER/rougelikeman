namespace Org.BouncyCastle.Crypto.Tls
{
    using Org.BouncyCastle.Security;
    using System;

    internal class TlsClientContextImpl : AbstractTlsContext, TlsClientContext, TlsContext
    {
        internal TlsClientContextImpl(SecureRandom secureRandom, SecurityParameters securityParameters) : base(secureRandom, securityParameters)
        {
        }

        public override bool IsServer =>
            false;
    }
}

