namespace Org.BouncyCastle.Crypto
{
    using System;

    [Serializable]
    public class MaxBytesExceededException : CryptoException
    {
        public MaxBytesExceededException()
        {
        }

        public MaxBytesExceededException(string message) : base(message)
        {
        }

        public MaxBytesExceededException(string message, Exception e) : base(message, e)
        {
        }
    }
}

