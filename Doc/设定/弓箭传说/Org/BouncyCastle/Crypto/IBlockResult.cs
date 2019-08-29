namespace Org.BouncyCastle.Crypto
{
    using System;

    public interface IBlockResult
    {
        byte[] Collect();
        int Collect(byte[] destination, int offset);
    }
}

