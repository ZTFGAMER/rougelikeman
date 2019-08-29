namespace Org.BouncyCastle.Security.Certificates
{
    using Org.BouncyCastle.Security;
    using System;

    [Serializable]
    public class CrlException : GeneralSecurityException
    {
        public CrlException()
        {
        }

        public CrlException(string msg) : base(msg)
        {
        }

        public CrlException(string msg, Exception e) : base(msg, e)
        {
        }
    }
}

