namespace Org.BouncyCastle.Utilities
{
    using Org.BouncyCastle.Math;
    using System;
    using System.Text;

    public abstract class Arrays
    {
        protected Arrays()
        {
        }

        public static byte[] Append(byte[] a, byte b)
        {
            if (a == null)
            {
                return new byte[] { b };
            }
            int length = a.Length;
            byte[] destinationArray = new byte[length + 1];
            Array.Copy(a, 0, destinationArray, 0, length);
            destinationArray[length] = b;
            return destinationArray;
        }

        public static short[] Append(short[] a, short b)
        {
            if (a == null)
            {
                return new short[] { b };
            }
            int length = a.Length;
            short[] destinationArray = new short[length + 1];
            Array.Copy(a, 0, destinationArray, 0, length);
            destinationArray[length] = b;
            return destinationArray;
        }

        public static int[] Append(int[] a, int b)
        {
            if (a == null)
            {
                return new int[] { b };
            }
            int length = a.Length;
            int[] destinationArray = new int[length + 1];
            Array.Copy(a, 0, destinationArray, 0, length);
            destinationArray[length] = b;
            return destinationArray;
        }

        public static bool AreEqual(bool[] a, bool[] b) => 
            ((a == b) || (((a != null) && (b != null)) && HaveSameContents(a, b)));

        public static bool AreEqual(byte[] a, byte[] b) => 
            ((a == b) || (((a != null) && (b != null)) && HaveSameContents(a, b)));

        public static bool AreEqual(char[] a, char[] b) => 
            ((a == b) || (((a != null) && (b != null)) && HaveSameContents(a, b)));

        public static bool AreEqual(int[] a, int[] b) => 
            ((a == b) || (((a != null) && (b != null)) && HaveSameContents(a, b)));

        public static bool AreEqual(uint[] a, uint[] b) => 
            ((a == b) || (((a != null) && (b != null)) && HaveSameContents(a, b)));

        [Obsolete("Use 'AreEqual' method instead")]
        public static bool AreSame(byte[] a, byte[] b) => 
            AreEqual(a, b);

        public static byte[] Clone(byte[] data) => 
            ((data != null) ? ((byte[]) data.Clone()) : null);

        public static int[] Clone(int[] data) => 
            ((data != null) ? ((int[]) data.Clone()) : null);

        public static long[] Clone(long[] data) => 
            ((data != null) ? ((long[]) data.Clone()) : null);

        internal static uint[] Clone(uint[] data) => 
            ((data != null) ? ((uint[]) data.Clone()) : null);

        public static ulong[] Clone(ulong[] data) => 
            ((data != null) ? ((ulong[]) data.Clone()) : null);

        public static byte[] Clone(byte[] data, byte[] existing)
        {
            if (data == null)
            {
                return null;
            }
            if ((existing == null) || (existing.Length != data.Length))
            {
                return Clone(data);
            }
            Array.Copy(data, 0, existing, 0, existing.Length);
            return existing;
        }

        public static ulong[] Clone(ulong[] data, ulong[] existing)
        {
            if (data == null)
            {
                return null;
            }
            if ((existing == null) || (existing.Length != data.Length))
            {
                return Clone(data);
            }
            Array.Copy(data, 0, existing, 0, existing.Length);
            return existing;
        }

        public static byte[] Concatenate(byte[] a, byte[] b)
        {
            if (a == null)
            {
                return Clone(b);
            }
            if (b == null)
            {
                return Clone(a);
            }
            byte[] destinationArray = new byte[a.Length + b.Length];
            Array.Copy(a, 0, destinationArray, 0, a.Length);
            Array.Copy(b, 0, destinationArray, a.Length, b.Length);
            return destinationArray;
        }

        public static int[] Concatenate(int[] a, int[] b)
        {
            if (a == null)
            {
                return Clone(b);
            }
            if (b == null)
            {
                return Clone(a);
            }
            int[] destinationArray = new int[a.Length + b.Length];
            Array.Copy(a, 0, destinationArray, 0, a.Length);
            Array.Copy(b, 0, destinationArray, a.Length, b.Length);
            return destinationArray;
        }

        public static byte[] ConcatenateAll(params byte[][] vs)
        {
            byte[][] bufferArray = new byte[vs.Length][];
            int num = 0;
            int num2 = 0;
            for (int i = 0; i < vs.Length; i++)
            {
                byte[] buffer = vs[i];
                if (buffer != null)
                {
                    bufferArray[num++] = buffer;
                    num2 += buffer.Length;
                }
            }
            byte[] destinationArray = new byte[num2];
            int destinationIndex = 0;
            for (int j = 0; j < num; j++)
            {
                byte[] sourceArray = bufferArray[j];
                Array.Copy(sourceArray, 0, destinationArray, destinationIndex, sourceArray.Length);
                destinationIndex += sourceArray.Length;
            }
            return destinationArray;
        }

        public static bool ConstantTimeAreEqual(byte[] a, byte[] b)
        {
            int length = a.Length;
            if (length != b.Length)
            {
                return false;
            }
            int num2 = 0;
            while (length != 0)
            {
                length--;
                num2 |= a[length] ^ b[length];
            }
            return (num2 == 0);
        }

        public static bool Contains(byte[] a, byte n)
        {
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] == n)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool Contains(short[] a, short n)
        {
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] == n)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool Contains(int[] a, int n)
        {
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] == n)
                {
                    return true;
                }
            }
            return false;
        }

        public static BigInteger[] CopyOf(BigInteger[] data, int newLength)
        {
            BigInteger[] destinationArray = new BigInteger[newLength];
            Array.Copy(data, 0, destinationArray, 0, Math.Min(newLength, data.Length));
            return destinationArray;
        }

        public static byte[] CopyOf(byte[] data, int newLength)
        {
            byte[] destinationArray = new byte[newLength];
            Array.Copy(data, 0, destinationArray, 0, Math.Min(newLength, data.Length));
            return destinationArray;
        }

        public static char[] CopyOf(char[] data, int newLength)
        {
            char[] destinationArray = new char[newLength];
            Array.Copy(data, 0, destinationArray, 0, Math.Min(newLength, data.Length));
            return destinationArray;
        }

        public static int[] CopyOf(int[] data, int newLength)
        {
            int[] destinationArray = new int[newLength];
            Array.Copy(data, 0, destinationArray, 0, Math.Min(newLength, data.Length));
            return destinationArray;
        }

        public static long[] CopyOf(long[] data, int newLength)
        {
            long[] destinationArray = new long[newLength];
            Array.Copy(data, 0, destinationArray, 0, Math.Min(newLength, data.Length));
            return destinationArray;
        }

        public static BigInteger[] CopyOfRange(BigInteger[] data, int from, int to)
        {
            int length = GetLength(from, to);
            BigInteger[] destinationArray = new BigInteger[length];
            Array.Copy(data, from, destinationArray, 0, Math.Min(length, data.Length - from));
            return destinationArray;
        }

        public static byte[] CopyOfRange(byte[] data, int from, int to)
        {
            int length = GetLength(from, to);
            byte[] destinationArray = new byte[length];
            Array.Copy(data, from, destinationArray, 0, Math.Min(length, data.Length - from));
            return destinationArray;
        }

        public static int[] CopyOfRange(int[] data, int from, int to)
        {
            int length = GetLength(from, to);
            int[] destinationArray = new int[length];
            Array.Copy(data, from, destinationArray, 0, Math.Min(length, data.Length - from));
            return destinationArray;
        }

        public static long[] CopyOfRange(long[] data, int from, int to)
        {
            int length = GetLength(from, to);
            long[] destinationArray = new long[length];
            Array.Copy(data, from, destinationArray, 0, Math.Min(length, data.Length - from));
            return destinationArray;
        }

        public static void Fill(byte[] buf, byte b)
        {
            int length = buf.Length;
            while (length > 0)
            {
                buf[--length] = b;
            }
        }

        public static int GetHashCode(byte[] data)
        {
            if (data == null)
            {
                return 0;
            }
            int length = data.Length;
            int num2 = length + 1;
            while (--length >= 0)
            {
                num2 *= 0x101;
                num2 ^= data[length];
            }
            return num2;
        }

        public static int GetHashCode(int[] data)
        {
            if (data == null)
            {
                return 0;
            }
            int length = data.Length;
            int num2 = length + 1;
            while (--length >= 0)
            {
                num2 *= 0x101;
                num2 ^= data[length];
            }
            return num2;
        }

        public static int GetHashCode(uint[] data)
        {
            if (data == null)
            {
                return 0;
            }
            int length = data.Length;
            int num2 = length + 1;
            while (--length >= 0)
            {
                num2 *= 0x101;
                num2 ^= (int) data[length];
            }
            return num2;
        }

        public static int GetHashCode(ulong[] data)
        {
            if (data == null)
            {
                return 0;
            }
            int length = data.Length;
            int num2 = length + 1;
            while (--length >= 0)
            {
                ulong num3 = data[length];
                num2 *= 0x101;
                num2 ^= (int) num3;
                num2 *= 0x101;
                num2 ^= (int) (num3 >> 0x20);
            }
            return num2;
        }

        public static int GetHashCode(byte[] data, int off, int len)
        {
            if (data == null)
            {
                return 0;
            }
            int num = len;
            int num2 = num + 1;
            while (--num >= 0)
            {
                num2 *= 0x101;
                num2 ^= data[off + num];
            }
            return num2;
        }

        public static int GetHashCode(int[] data, int off, int len)
        {
            if (data == null)
            {
                return 0;
            }
            int num = len;
            int num2 = num + 1;
            while (--num >= 0)
            {
                num2 *= 0x101;
                num2 ^= data[off + num];
            }
            return num2;
        }

        public static int GetHashCode(uint[] data, int off, int len)
        {
            if (data == null)
            {
                return 0;
            }
            int num = len;
            int num2 = num + 1;
            while (--num >= 0)
            {
                num2 *= 0x101;
                num2 ^= (int) data[off + num];
            }
            return num2;
        }

        public static int GetHashCode(ulong[] data, int off, int len)
        {
            if (data == null)
            {
                return 0;
            }
            int num = len;
            int num2 = num + 1;
            while (--num >= 0)
            {
                ulong num3 = data[off + num];
                num2 *= 0x101;
                num2 ^= (int) num3;
                num2 *= 0x101;
                num2 ^= (int) (num3 >> 0x20);
            }
            return num2;
        }

        private static int GetLength(int from, int to)
        {
            int num = to - from;
            if (num < 0)
            {
                throw new ArgumentException(from + " > " + to);
            }
            return num;
        }

        private static bool HaveSameContents(bool[] a, bool[] b)
        {
            int length = a.Length;
            if (length != b.Length)
            {
                return false;
            }
            while (length != 0)
            {
                length--;
                if (a[length] != b[length])
                {
                    return false;
                }
            }
            return true;
        }

        private static bool HaveSameContents(byte[] a, byte[] b)
        {
            int length = a.Length;
            if (length != b.Length)
            {
                return false;
            }
            while (length != 0)
            {
                length--;
                if (a[length] != b[length])
                {
                    return false;
                }
            }
            return true;
        }

        private static bool HaveSameContents(char[] a, char[] b)
        {
            int length = a.Length;
            if (length != b.Length)
            {
                return false;
            }
            while (length != 0)
            {
                length--;
                if (a[length] != b[length])
                {
                    return false;
                }
            }
            return true;
        }

        private static bool HaveSameContents(int[] a, int[] b)
        {
            int length = a.Length;
            if (length != b.Length)
            {
                return false;
            }
            while (length != 0)
            {
                length--;
                if (a[length] != b[length])
                {
                    return false;
                }
            }
            return true;
        }

        private static bool HaveSameContents(uint[] a, uint[] b)
        {
            int length = a.Length;
            if (length != b.Length)
            {
                return false;
            }
            while (length != 0)
            {
                length--;
                if (a[length] != b[length])
                {
                    return false;
                }
            }
            return true;
        }

        public static byte[] Prepend(byte[] a, byte b)
        {
            if (a == null)
            {
                return new byte[] { b };
            }
            int length = a.Length;
            byte[] destinationArray = new byte[length + 1];
            Array.Copy(a, 0, destinationArray, 1, length);
            destinationArray[0] = b;
            return destinationArray;
        }

        public static short[] Prepend(short[] a, short b)
        {
            if (a == null)
            {
                return new short[] { b };
            }
            int length = a.Length;
            short[] destinationArray = new short[length + 1];
            Array.Copy(a, 0, destinationArray, 1, length);
            destinationArray[0] = b;
            return destinationArray;
        }

        public static int[] Prepend(int[] a, int b)
        {
            if (a == null)
            {
                return new int[] { b };
            }
            int length = a.Length;
            int[] destinationArray = new int[length + 1];
            Array.Copy(a, 0, destinationArray, 1, length);
            destinationArray[0] = b;
            return destinationArray;
        }

        public static byte[] Reverse(byte[] a)
        {
            if (a == null)
            {
                return null;
            }
            int num = 0;
            int length = a.Length;
            byte[] buffer = new byte[length];
            while (--length >= 0)
            {
                buffer[length] = a[num++];
            }
            return buffer;
        }

        public static int[] Reverse(int[] a)
        {
            if (a == null)
            {
                return null;
            }
            int num = 0;
            int length = a.Length;
            int[] numArray = new int[length];
            while (--length >= 0)
            {
                numArray[length] = a[num++];
            }
            return numArray;
        }

        public static string ToString(object[] a)
        {
            StringBuilder builder = new StringBuilder(0x5b);
            if (a.Length > 0)
            {
                builder.Append(a[0]);
                for (int i = 1; i < a.Length; i++)
                {
                    builder.Append(", ").Append(a[i]);
                }
            }
            builder.Append(']');
            return builder.ToString();
        }
    }
}

