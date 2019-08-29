namespace Org.BouncyCastle.Crypto.Tls
{
    using System;

    public abstract class AbstractTlsEncryptionCredentials : AbstractTlsCredentials, TlsEncryptionCredentials, TlsCredentials
    {
        protected AbstractTlsEncryptionCredentials()
        {
        }

        public abstract byte[] DecryptPreMasterSecret(byte[] encryptedPreMasterSecret);
    }
}

