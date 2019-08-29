namespace Org.BouncyCastle.Crypto.Paddings
{
    using Org.BouncyCastle.Security;
    using System;

    public class TbcPadding : IBlockCipherPadding
    {
        public virtual int AddPadding(byte[] input, int inOff)
        {
            byte num2;
            int num = input.Length - inOff;
            if (inOff > 0)
            {
                num2 = ((input[inOff - 1] & 1) != 0) ? ((byte) 0) : ((byte) 0xff);
            }
            else
            {
                num2 = ((input[input.Length - 1] & 1) != 0) ? ((byte) 0) : ((byte) 0xff);
            }
            while (inOff < input.Length)
            {
                input[inOff] = num2;
                inOff++;
            }
            return num;
        }

        public virtual void Init(SecureRandom random)
        {
        }

        public virtual int PadCount(byte[] input)
        {
            byte num = input[input.Length - 1];
            int num2 = input.Length - 1;
            while ((num2 > 0) && (input[num2 - 1] == num))
            {
                num2--;
            }
            return (input.Length - num2);
        }

        public string PaddingName =>
            "TBC";
    }
}

