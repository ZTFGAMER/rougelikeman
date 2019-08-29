namespace Org.BouncyCastle.Crypto.Tls
{
    using System;

    public class AbstractTlsCipherFactory : TlsCipherFactory
    {
        public virtual TlsCipher CreateCipher(TlsContext context, int encryptionAlgorithm, int macAlgorithm)
        {
            throw new TlsFatalAlert(80);
        }
    }
}

