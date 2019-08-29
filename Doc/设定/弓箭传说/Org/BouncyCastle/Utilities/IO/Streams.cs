namespace Org.BouncyCastle.Utilities.IO
{
    using System;
    using System.IO;

    public sealed class Streams
    {
        private const int BufferSize = 0x200;

        private Streams()
        {
        }

        public static void Drain(Stream inStr)
        {
            byte[] buffer = new byte[0x200];
            while (inStr.Read(buffer, 0, buffer.Length) > 0)
            {
            }
        }

        public static void PipeAll(Stream inStr, Stream outStr)
        {
            int num;
            byte[] buffer = new byte[0x200];
            while ((num = inStr.Read(buffer, 0, buffer.Length)) > 0)
            {
                outStr.Write(buffer, 0, num);
            }
        }

        public static long PipeAllLimited(Stream inStr, long limit, Stream outStr)
        {
            int num2;
            byte[] buffer = new byte[0x200];
            long num = 0L;
            while ((num2 = inStr.Read(buffer, 0, buffer.Length)) > 0)
            {
                if ((limit - num) < num2)
                {
                    throw new StreamOverflowException("Data Overflow");
                }
                num += num2;
                outStr.Write(buffer, 0, num2);
            }
            return num;
        }

        public static byte[] ReadAll(Stream inStr)
        {
            MemoryStream outStr = new MemoryStream();
            PipeAll(inStr, outStr);
            return outStr.ToArray();
        }

        public static byte[] ReadAllLimited(Stream inStr, int limit)
        {
            MemoryStream outStr = new MemoryStream();
            PipeAllLimited(inStr, (long) limit, outStr);
            return outStr.ToArray();
        }

        public static int ReadFully(Stream inStr, byte[] buf) => 
            ReadFully(inStr, buf, 0, buf.Length);

        public static int ReadFully(Stream inStr, byte[] buf, int off, int len)
        {
            int num = 0;
            while (num < len)
            {
                int num2 = inStr.Read(buf, off + num, len - num);
                if (num2 < 1)
                {
                    return num;
                }
                num += num2;
            }
            return num;
        }
    }
}

