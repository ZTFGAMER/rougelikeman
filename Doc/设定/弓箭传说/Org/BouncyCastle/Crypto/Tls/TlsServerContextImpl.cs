namespace Org.BouncyCastle.Crypto.Tls
{
    using Org.BouncyCastle.Security;
    using System;

    internal class TlsServerContextImpl : AbstractTlsContext, TlsServerContext, TlsContext
    {
        internal TlsServerContextImpl(SecureRandom secureRandom, SecurityParameters securityParameters) : base(secureRandom, securityParameters)
        {
        }

        public override bool IsServer =>
            true;
    }
}

