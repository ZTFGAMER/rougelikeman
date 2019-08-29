namespace Org.BouncyCastle.Security
{
    using System;

    [Serializable]
    public class SecurityUtilityException : Exception
    {
        public SecurityUtilityException()
        {
        }

        public SecurityUtilityException(string message) : base(message)
        {
        }

        public SecurityUtilityException(string message, Exception exception) : base(message, exception)
        {
        }
    }
}

