namespace Org.BouncyCastle.Crypto.Tls
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Digests;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Crypto.Signers;
    using System;

    public abstract class TlsDsaSigner : AbstractTlsSigner
    {
        protected TlsDsaSigner()
        {
        }

        protected abstract IDsa CreateDsaImpl(byte hashAlgorithm);
        public override ISigner CreateSigner(SignatureAndHashAlgorithm algorithm, AsymmetricKeyParameter privateKey) => 
            this.MakeSigner(algorithm, false, true, privateKey);

        public override ISigner CreateVerifyer(SignatureAndHashAlgorithm algorithm, AsymmetricKeyParameter publicKey) => 
            this.MakeSigner(algorithm, false, false, publicKey);

        public override byte[] GenerateRawSignature(SignatureAndHashAlgorithm algorithm, AsymmetricKeyParameter privateKey, byte[] hash)
        {
            ISigner signer = this.MakeSigner(algorithm, true, true, new ParametersWithRandom(privateKey, base.mContext.SecureRandom));
            if (algorithm == null)
            {
                signer.BlockUpdate(hash, 0x10, 20);
            }
            else
            {
                signer.BlockUpdate(hash, 0, hash.Length);
            }
            return signer.GenerateSignature();
        }

        protected virtual ICipherParameters MakeInitParameters(bool forSigning, ICipherParameters cp) => 
            cp;

        protected virtual ISigner MakeSigner(SignatureAndHashAlgorithm algorithm, bool raw, bool forSigning, ICipherParameters cp)
        {
            if ((algorithm != null) != TlsUtilities.IsTlsV12(base.mContext))
            {
                throw new InvalidOperationException();
            }
            if ((algorithm != null) && (algorithm.Signature != this.SignatureAlgorithm))
            {
                throw new InvalidOperationException();
            }
            byte hashAlgorithm = (algorithm != null) ? algorithm.Hash : ((byte) 2);
            IDigest digest = !raw ? TlsUtilities.CreateHash(hashAlgorithm) : new NullDigest();
            ISigner signer = new DsaDigestSigner(this.CreateDsaImpl(hashAlgorithm), digest);
            signer.Init(forSigning, this.MakeInitParameters(forSigning, cp));
            return signer;
        }

        public override bool VerifyRawSignature(SignatureAndHashAlgorithm algorithm, byte[] sigBytes, AsymmetricKeyParameter publicKey, byte[] hash)
        {
            ISigner signer = this.MakeSigner(algorithm, true, false, publicKey);
            if (algorithm == null)
            {
                signer.BlockUpdate(hash, 0x10, 20);
            }
            else
            {
                signer.BlockUpdate(hash, 0, hash.Length);
            }
            return signer.VerifySignature(sigBytes);
        }

        protected abstract byte SignatureAlgorithm { get; }
    }
}

