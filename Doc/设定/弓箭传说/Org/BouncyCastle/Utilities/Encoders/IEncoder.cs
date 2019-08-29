namespace Org.BouncyCastle.Utilities.Encoders
{
    using System;
    using System.IO;

    public interface IEncoder
    {
        int Decode(byte[] data, int off, int length, Stream outStream);
        int DecodeString(string data, Stream outStream);
        int Encode(byte[] data, int off, int length, Stream outStream);
    }
}

