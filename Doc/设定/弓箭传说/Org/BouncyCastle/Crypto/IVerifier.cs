namespace Org.BouncyCastle.Crypto
{
    using System;

    public interface IVerifier
    {
        bool IsVerified(byte[] data);
        bool IsVerified(byte[] source, int off, int length);
    }
}

