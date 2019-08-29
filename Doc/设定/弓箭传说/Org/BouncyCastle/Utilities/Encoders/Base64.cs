namespace Org.BouncyCastle.Utilities.Encoders
{
    using Org.BouncyCastle.Utilities;
    using System;
    using System.IO;

    public sealed class Base64
    {
        private Base64()
        {
        }

        public static byte[] Decode(byte[] data) => 
            Convert.FromBase64String(Strings.FromAsciiByteArray(data));

        public static byte[] Decode(string data) => 
            Convert.FromBase64String(data);

        public static int Decode(string data, Stream outStream)
        {
            byte[] buffer = Decode(data);
            outStream.Write(buffer, 0, buffer.Length);
            return buffer.Length;
        }

        public static byte[] Encode(byte[] data) => 
            Encode(data, 0, data.Length);

        public static int Encode(byte[] data, Stream outStream)
        {
            byte[] buffer = Encode(data);
            outStream.Write(buffer, 0, buffer.Length);
            return buffer.Length;
        }

        public static byte[] Encode(byte[] data, int off, int length) => 
            Strings.ToAsciiByteArray(Convert.ToBase64String(data, off, length));

        public static int Encode(byte[] data, int off, int length, Stream outStream)
        {
            byte[] buffer = Encode(data, off, length);
            outStream.Write(buffer, 0, buffer.Length);
            return buffer.Length;
        }

        public static string ToBase64String(byte[] data) => 
            Convert.ToBase64String(data, 0, data.Length);

        public static string ToBase64String(byte[] data, int off, int length) => 
            Convert.ToBase64String(data, off, length);
    }
}

