namespace Org.BouncyCastle.Crypto
{
    using System;

    public interface IAsymmetricCipherKeyPairGenerator
    {
        AsymmetricCipherKeyPair GenerateKeyPair();
        void Init(KeyGenerationParameters parameters);
    }
}

