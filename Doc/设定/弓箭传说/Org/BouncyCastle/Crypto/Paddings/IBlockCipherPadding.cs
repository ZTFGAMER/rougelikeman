namespace Org.BouncyCastle.Crypto.Paddings
{
    using Org.BouncyCastle.Security;
    using System;

    public interface IBlockCipherPadding
    {
        int AddPadding(byte[] input, int inOff);
        void Init(SecureRandom random);
        int PadCount(byte[] input);

        string PaddingName { get; }
    }
}

