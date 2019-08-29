namespace Org.BouncyCastle.Crypto.Paddings
{
    using Org.BouncyCastle.Security;
    using System;

    public class ZeroBytePadding : IBlockCipherPadding
    {
        public int AddPadding(byte[] input, int inOff)
        {
            int num = input.Length - inOff;
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
            int length = input.Length;
            while (length > 0)
            {
                if (input[length - 1] != 0)
                {
                    break;
                }
                length--;
            }
            return (input.Length - length);
        }

        public string PaddingName =>
            "ZeroBytePadding";
    }
}

