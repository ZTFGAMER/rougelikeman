namespace Org.BouncyCastle.Crypto.Tls
{
    using System;

    public interface TlsEncryptionCredentials : TlsCredentials
    {
        byte[] DecryptPreMasterSecret(byte[] encryptedPreMasterSecret);
    }
}

