namespace Org.BouncyCastle.Crypto.Paddings
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Security;
    using System;

    public class Pkcs7Padding : IBlockCipherPadding
    {
        public int AddPadding(byte[] input, int inOff)
        {
            byte num = (byte) (input.Length - inOff);
            while (inOff < input.Length)
            {
                input[inOff] = num;
                inOff++;
            }
            return num;
        }

        public void Init(SecureRandom random)
        {
        }

        public int PadCount(byte[] input)
        {
            byte num = input[input.Length - 1];
            int num2 = num;
            if ((num2 < 1) || (num2 > input.Length))
            {
                throw new InvalidCipherTextException("pad block corrupted");
            }
            for (int i = 2; i <= num2; i++)
            {
                if (input[input.Length - i] != num)
                {
                    throw new InvalidCipherTextException("pad block corrupted");
                }
            }
            return num2;
        }

        public string PaddingName =>
            "PKCS7";
    }
}

