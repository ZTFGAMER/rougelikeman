namespace Org.BouncyCastle.Crypto.Parameters
{
    using System;

    public class DesParameters : KeyParameter
    {
        public const int DesKeyLength = 8;
        private const int N_DES_WEAK_KEYS = 0x10;
        private static readonly byte[] DES_weak_keys = new byte[] { 
            1, 1, 1, 1, 1, 1, 1, 1, 0x1f, 0x1f, 0x1f, 0x1f, 14, 14, 14, 14,
            0xe0, 0xe0, 0xe0, 0xe0, 0xf1, 0xf1, 0xf1, 0xf1, 0xfe, 0xfe, 0xfe, 0xfe, 0xfe, 0xfe, 0xfe, 0xfe,
            1, 0xfe, 1, 0xfe, 1, 0xfe, 1, 0xfe, 0x1f, 0xe0, 0x1f, 0xe0, 14, 0xf1, 14, 0xf1,
            1, 0xe0, 1, 0xe0, 1, 0xf1, 1, 0xf1, 0x1f, 0xfe, 0x1f, 0xfe, 14, 0xfe, 14, 0xfe,
            1, 0x1f, 1, 0x1f, 1, 14, 1, 14, 0xe0, 0xfe, 0xe0, 0xfe, 0xf1, 0xfe, 0xf1, 0xfe,
            0xfe, 1, 0xfe, 1, 0xfe, 1, 0xfe, 1, 0xe0, 0x1f, 0xe0, 0x1f, 0xf1, 14, 0xf1, 14,
            0xe0, 1, 0xe0, 1, 0xf1, 1, 0xf1, 1, 0xfe, 0x1f, 0xfe, 0x1f, 0xfe, 14, 0xfe, 14,
            0x1f, 1, 0x1f, 1, 14, 1, 14, 1, 0xfe, 0xe0, 0xfe, 0xe0, 0xfe, 0xf1, 0xfe, 0xf1
        };

        public DesParameters(byte[] key) : base(key)
        {
            if (IsWeakKey(key))
            {
                throw new ArgumentException("attempt to create weak DES key");
            }
        }

        public DesParameters(byte[] key, int keyOff, int keyLen) : base(key, keyOff, keyLen)
        {
            if (IsWeakKey(key, keyOff))
            {
                throw new ArgumentException("attempt to create weak DES key");
            }
        }

        public static bool IsWeakKey(byte[] key) => 
            IsWeakKey(key, 0);

        public static bool IsWeakKey(byte[] key, int offset)
        {
            if ((key.Length - offset) < 8)
            {
                throw new ArgumentException("key material too short.");
            }
            for (int i = 0; i < 0x10; i++)
            {
                bool flag = false;
                for (int j = 0; j < 8; j++)
                {
                    if (key[j + offset] != DES_weak_keys[(i * 8) + j])
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    return true;
                }
            }
            return false;
        }

        public static byte SetOddParity(byte b)
        {
            uint num = (uint) (b ^ 1);
            num ^= num >> 4;
            num ^= num >> 2;
            num ^= num >> 1;
            num &= 1;
            return (byte) (b ^ num);
        }

        public static void SetOddParity(byte[] bytes)
        {
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = SetOddParity(bytes[i]);
            }
        }

        public static void SetOddParity(byte[] bytes, int off, int len)
        {
            for (int i = 0; i < len; i++)
            {
                bytes[off + i] = SetOddParity(bytes[off + i]);
            }
        }
    }
}

