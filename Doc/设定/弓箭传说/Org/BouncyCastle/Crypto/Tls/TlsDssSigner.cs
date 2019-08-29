namespace Org.BouncyCastle.Crypto.Tls
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Crypto.Signers;
    using System;

    public class TlsDssSigner : TlsDsaSigner
    {
        protected override IDsa CreateDsaImpl(byte hashAlgorithm) => 
            new DsaSigner(new HMacDsaKCalculator(TlsUtilities.CreateHash(hashAlgorithm)));

        public override bool IsValidPublicKey(AsymmetricKeyParameter publicKey) => 
            (publicKey is DsaPublicKeyParameters);

        protected override byte SignatureAlgorithm =>
            2;
    }
}

