namespace Org.BouncyCastle.Crypto.Paddings
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Security;
    using System;

    public class ISO7816d4Padding : IBlockCipherPadding
    {
        public int AddPadding(byte[] input, int inOff)
        {
            int num = input.Length - inOff;
            input[inOff] = 0x80;
            inOff++;
            while (inOff < input.Length)
            {
                input[inOff] = 0;
                inOff++;
            }
            return num;
        }

        public void Init(SecureRandom random)
        {
        }

        public int PadCount(byte[] input)
        {
            int index = input.Length - 1;
            while ((index > 0) && (input[index] == 0))
            {
                index--;
            }
            if (input[index] != 0x80)
            {
                throw new InvalidCipherTextException("pad block corrupted");
            }
            return (input.Length - index);
        }

        public string PaddingName =>
            "ISO7816-4";
    }
}

