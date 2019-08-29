namespace GooglePlayGames.OurUtils
{
    using System;

    public static class Misc
    {
        public static bool BuffersAreIdentical(byte[] a, byte[] b)
        {
            if (a != b)
            {
                if ((a == null) || (b == null))
                {
                    return false;
                }
                if (a.Length != b.Length)
                {
                    return false;
                }
                for (int i = 0; i < a.Length; i++)
                {
                    if (a[i] != b[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static T CheckNotNull<T>(T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException();
            }
            return value;
        }

        public static T CheckNotNull<T>(T value, string paramName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(paramName);
            }
            return value;
        }

        public static byte[] GetSubsetBytes(byte[] array, int offset, int length)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            if ((offset < 0) || (offset >= array.Length))
            {
                throw new ArgumentOutOfRangeException("offset");
            }
            if ((length < 0) || ((array.Length - offset) < length))
            {
                throw new ArgumentOutOfRangeException("length");
            }
            if ((offset == 0) && (length == array.Length))
            {
                return array;
            }
            byte[] destinationArray = new byte[length];
            Array.Copy(array, offset, destinationArray, 0, length);
            return destinationArray;
        }
    }
}

