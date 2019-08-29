namespace Org.BouncyCastle.Crypto.Generators
{
    using Org.BouncyCastle.Crypto;
    using System;

    public class Poly1305KeyGenerator : CipherKeyGenerator
    {
        private const byte R_MASK_LOW_2 = 0xfc;
        private const byte R_MASK_HIGH_4 = 15;

        public static void CheckKey(byte[] key)
        {
            if (key.Length != 0x20)
            {
                throw new ArgumentException("Poly1305 key must be 256 bits.");
            }
            CheckMask(key[3], 15);
            CheckMask(key[7], 15);
            CheckMask(key[11], 15);
            CheckMask(key[15], 15);
            CheckMask(key[4], 0xfc);
            CheckMask(key[8], 0xfc);
            CheckMask(key[12], 0xfc);
        }

        private static void CheckMask(byte b, byte mask)
        {
            if ((b & ~mask) != 0)
            {
                throw new ArgumentException("Invalid format for r portion of Poly1305 key.");
            }
        }

        public static void Clamp(byte[] key)
        {
            if (key.Length != 0x20)
            {
                throw new ArgumentException("Poly1305 key must be 256 bits.");
            }
            key[3] = (byte) (key[3] & 15);
            key[7] = (byte) (key[7] & 15);
            key[11] = (byte) (key[11] & 15);
            key[15] = (byte) (key[15] & 15);
            key[4] = (byte) (key[4] & 0xfc);
            key[8] = (byte) (key[8] & 0xfc);
            key[12] = (byte) (key[12] & 0xfc);
        }

        protected override byte[] engineGenerateKey()
        {
            byte[] key = base.engineGenerateKey();
            Clamp(key);
            return key;
        }

        protected override void engineInit(KeyGenerationParameters param)
        {
            base.random = param.Random;
            base.strength = 0x20;
        }
    }
}

