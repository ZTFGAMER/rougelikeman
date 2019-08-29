namespace Org.BouncyCastle.Crypto.Tls
{
    using System;

    public interface TlsCipherFactory
    {
        TlsCipher CreateCipher(TlsContext context, int encryptionAlgorithm, int macAlgorithm);
    }
}

