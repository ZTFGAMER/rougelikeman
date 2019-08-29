namespace Org.BouncyCastle.Utilities
{
    using System;
    using System.Text;

    public abstract class Strings
    {
        protected Strings()
        {
        }

        public static string FromAsciiByteArray(byte[] bytes) => 
            Encoding.ASCII.GetString(bytes, 0, bytes.Length);

        public static string FromByteArray(byte[] bs)
        {
            char[] chArray = new char[bs.Length];
            for (int i = 0; i < chArray.Length; i++)
            {
                chArray[i] = Convert.ToChar(bs[i]);
            }
            return new string(chArray);
        }

        public static string FromUtf8ByteArray(byte[] bytes) => 
            Encoding.UTF8.GetString(bytes, 0, bytes.Length);

        internal static bool IsOneOf(string s, params string[] candidates)
        {
            foreach (string str in candidates)
            {
                if (s == str)
                {
                    return true;
                }
            }
            return false;
        }

        public static byte[] ToAsciiByteArray(char[] cs) => 
            Encoding.ASCII.GetBytes(cs);

        public static byte[] ToAsciiByteArray(string s) => 
            Encoding.ASCII.GetBytes(s);

        public static byte[] ToByteArray(char[] cs)
        {
            byte[] buffer = new byte[cs.Length];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = Convert.ToByte(cs[i]);
            }
            return buffer;
        }

        public static byte[] ToByteArray(string s)
        {
            byte[] buffer = new byte[s.Length];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = Convert.ToByte(s[i]);
            }
            return buffer;
        }

        public static byte[] ToUtf8ByteArray(char[] cs) => 
            Encoding.UTF8.GetBytes(cs);

        public static byte[] ToUtf8ByteArray(string s) => 
            Encoding.UTF8.GetBytes(s);
    }
}

