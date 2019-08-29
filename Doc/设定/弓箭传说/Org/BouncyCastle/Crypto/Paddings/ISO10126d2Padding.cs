namespace Org.BouncyCastle.Crypto.Paddings
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Security;
    using System;

    public class ISO10126d2Padding : IBlockCipherPadding
    {
        private SecureRandom random;

        public int AddPadding(byte[] input, int inOff)
        {
            byte num = (byte) (input.Length - inOff);
            while (inOff < (input.Length - 1))
            {
                input[inOff] = (byte) this.random.NextInt();
                inOff++;
            }
            input[inOff] = num;
            return num;
        }

        public void Init(SecureRandom random)
        {
            this.random = (random == null) ? new SecureRandom() : random;
        }

        public int PadCount(byte[] input)
        {
            int num = input[input.Length - 1] & 0xff;
            if (num > input.Length)
            {
                throw new InvalidCipherTextException("pad block corrupted");
            }
            return num;
        }

        public string PaddingName =>
            "ISO10126-2";
    }
}

