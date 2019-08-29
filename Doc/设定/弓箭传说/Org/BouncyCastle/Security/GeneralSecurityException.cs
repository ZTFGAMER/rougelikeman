namespace Org.BouncyCastle.Security
{
    using System;

    [Serializable]
    public class GeneralSecurityException : Exception
    {
        public GeneralSecurityException()
        {
        }

        public GeneralSecurityException(string message) : base(message)
        {
        }

        public GeneralSecurityException(string message, Exception exception) : base(message, exception)
        {
        }
    }
}

