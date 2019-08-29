namespace Org.BouncyCastle.Utilities.IO
{
    using System;
    using System.IO;

    [Serializable]
    public class StreamOverflowException : IOException
    {
        public StreamOverflowException()
        {
        }

        public StreamOverflowException(string message) : base(message)
        {
        }

        public StreamOverflowException(string message, Exception exception) : base(message, exception)
        {
        }
    }
}

