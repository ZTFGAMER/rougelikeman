namespace Org.BouncyCastle.Crypto.Tls
{
    using Org.BouncyCastle.Crypto;
    using System;

    public interface TlsSigner
    {
        ISigner CreateSigner(AsymmetricKeyParameter privateKey);
        ISigner CreateSigner(SignatureAndHashAlgorithm algorithm, AsymmetricKeyParameter privateKey);
        ISigner CreateVerifyer(AsymmetricKeyParameter publicKey);
        ISigner CreateVerifyer(SignatureAndHashAlgorithm algorithm, AsymmetricKeyParameter publicKey);
        byte[] GenerateRawSignature(AsymmetricKeyParameter privateKey, byte[] md5AndSha1);
        byte[] GenerateRawSignature(SignatureAndHashAlgorithm algorithm, AsymmetricKeyParameter privateKey, byte[] hash);
        void Init(TlsContext context);
        bool IsValidPublicKey(AsymmetricKeyParameter publicKey);
        bool VerifyRawSignature(byte[] sigBytes, AsymmetricKeyParameter publicKey, byte[] md5AndSha1);
        bool VerifyRawSignature(SignatureAndHashAlgorithm algorithm, byte[] sigBytes, AsymmetricKeyParameter publicKey, byte[] hash);
    }
}

