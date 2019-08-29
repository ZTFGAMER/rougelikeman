namespace Org.BouncyCastle.Security.Certificates
{
    using System;

    [Serializable]
    public class CertificateExpiredException : CertificateException
    {
        public CertificateExpiredException()
        {
        }

        public CertificateExpiredException(string message) : base(message)
        {
        }

        public CertificateExpiredException(string message, Exception exception) : base(message, exception)
        {
        }
    }
}

