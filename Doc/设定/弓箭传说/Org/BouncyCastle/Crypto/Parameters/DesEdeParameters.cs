namespace Org.BouncyCastle.Crypto.Parameters
{
    using System;

    public class DesEdeParameters : DesParameters
    {
        public const int DesEdeKeyLength = 0x18;

        public DesEdeParameters(byte[] key) : base(FixKey(key, 0, key.Length))
        {
        }

        public DesEdeParameters(byte[] key, int keyOff, int keyLen) : base(FixKey(key, keyOff, keyLen))
        {
        }

        private static byte[] FixKey(byte[] key, int keyOff, int keyLen)
        {
            byte[] destinationArray = new byte[0x18];
            if (keyLen != 0x10)
            {
                if (keyLen != 0x18)
                {
                    throw new ArgumentException("Bad length for DESede key: " + keyLen, "keyLen");
                }
            }
            else
            {
                Array.Copy(key, keyOff, destinationArray, 0, 0x10);
                Array.Copy(key, keyOff, destinationArray, 0x10, 8);
                goto Label_0063;
            }
            Array.Copy(key, keyOff, destinationArray, 0, 0x18);
        Label_0063:
            if (IsWeakKey(destinationArray))
            {
                throw new ArgumentException("attempt to create weak DESede key");
            }
            return destinationArray;
        }

        public static bool IsReal2Key(byte[] key, int offset)
        {
            bool flag = false;
            for (int i = offset; i != (offset + 8); i++)
            {
                flag |= key[i] != key[i + 8];
            }
            return flag;
        }

        public static bool IsReal3Key(byte[] key, int offset)
        {
            bool flag = false;
            bool flag2 = false;
            bool flag3 = false;
            for (int i = offset; i != (offset + 8); i++)
            {
                flag |= key[i] != key[i + 8];
                flag2 |= key[i] != key[i + 0x10];
                flag3 |= key[i + 8] != key[i + 0x10];
            }
            return ((flag && flag2) && flag3);
        }

        public static bool IsRealEdeKey(byte[] key, int offset) => 
            ((key.Length != 0x10) ? IsReal3Key(key, offset) : IsReal2Key(key, offset));

        public static bool IsWeakKey(byte[] key) => 
            IsWeakKey(key, 0, key.Length);

        public static bool IsWeakKey(byte[] key, int offset) => 
            IsWeakKey(key, offset, key.Length - offset);

        public static bool IsWeakKey(byte[] key, int offset, int length)
        {
            for (int i = offset; i < length; i += 8)
            {
                if (DesParameters.IsWeakKey(key, i))
                {
                    return true;
                }
            }
            return false;
        }
    }
}

