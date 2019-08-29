namespace Org.BouncyCastle.Crypto.Tls
{
    using Org.BouncyCastle.Crypto;
    using System;

    public abstract class AbstractTlsSigner : TlsSigner
    {
        protected TlsContext mContext;

        protected AbstractTlsSigner()
        {
        }

        public virtual ISigner CreateSigner(AsymmetricKeyParameter privateKey) => 
            this.CreateSigner(null, privateKey);

        public abstract ISigner CreateSigner(SignatureAndHashAlgorithm algorithm, AsymmetricKeyParameter privateKey);
        public virtual ISigner CreateVerifyer(AsymmetricKeyParameter publicKey) => 
            this.CreateVerifyer(null, publicKey);

        public abstract ISigner CreateVerifyer(SignatureAndHashAlgorithm algorithm, AsymmetricKeyParameter publicKey);
        public virtual byte[] GenerateRawSignature(AsymmetricKeyParameter privateKey, byte[] md5AndSha1) => 
            this.GenerateRawSignature(null, privateKey, md5AndSha1);

        public abstract byte[] GenerateRawSignature(SignatureAndHashAlgorithm algorithm, AsymmetricKeyParameter privateKey, byte[] hash);
        public virtual void Init(TlsContext context)
        {
            this.mContext = context;
        }

        public abstract bool IsValidPublicKey(AsymmetricKeyParameter publicKey);
        public virtual bool VerifyRawSignature(byte[] sigBytes, AsymmetricKeyParameter publicKey, byte[] md5AndSha1) => 
            this.VerifyRawSignature(null, sigBytes, publicKey, md5AndSha1);

        public abstract bool VerifyRawSignature(SignatureAndHashAlgorithm algorithm, byte[] sigBytes, AsymmetricKeyParameter publicKey, byte[] hash);
    }
}

