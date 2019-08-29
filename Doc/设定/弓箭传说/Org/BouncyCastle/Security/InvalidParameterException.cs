namespace Org.BouncyCastle.Security
{
    using System;

    [Serializable]
    public class InvalidParameterException : KeyException
    {
        public InvalidParameterException()
        {
        }

        public InvalidParameterException(string message) : base(message)
        {
        }

        public InvalidParameterException(string message, Exception exception) : base(message, exception)
        {
        }
    }
}

