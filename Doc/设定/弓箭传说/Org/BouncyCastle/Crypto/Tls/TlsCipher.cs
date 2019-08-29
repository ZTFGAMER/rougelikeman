namespace Org.BouncyCastle.Crypto.Tls
{
    using System;

    public interface TlsCipher
    {
        byte[] DecodeCiphertext(long seqNo, byte type, byte[] ciphertext, int offset, int len);
        byte[] EncodePlaintext(long seqNo, byte type, byte[] plaintext, int offset, int len);
        int GetPlaintextLimit(int ciphertextLimit);
    }
}

