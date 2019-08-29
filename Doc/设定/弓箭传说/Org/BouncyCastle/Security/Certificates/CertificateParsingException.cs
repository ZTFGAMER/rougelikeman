namespace Org.BouncyCastle.Security.Certificates
{
    using System;

    [Serializable]
    public class CertificateParsingException : CertificateException
    {
        public CertificateParsingException()
        {
        }

        public CertificateParsingException(string message) : base(message)
        {
        }

        public CertificateParsingException(string message, Exception exception) : base(message, exception)
        {
        }
    }
}

