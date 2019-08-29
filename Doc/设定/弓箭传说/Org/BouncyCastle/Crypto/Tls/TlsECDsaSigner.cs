namespace Org.BouncyCastle.Crypto.Tls
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Crypto.Signers;
    using System;

    public class TlsECDsaSigner : TlsDsaSigner
    {
        protected override IDsa CreateDsaImpl(byte hashAlgorithm) => 
            new ECDsaSigner(new HMacDsaKCalculator(TlsUtilities.CreateHash(hashAlgorithm)));

        public override bool IsValidPublicKey(AsymmetricKeyParameter publicKey) => 
            (publicKey is ECPublicKeyParameters);

        protected override byte SignatureAlgorithm =>
            3;
    }
}

