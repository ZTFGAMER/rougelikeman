namespace Org.BouncyCastle.Crypto.Operators
{
    using Org.BouncyCastle.Asn1.X509;
    using Org.BouncyCastle.Crypto;
    using System;
    using System.Collections;

    public class Asn1VerifierFactoryProvider : IVerifierFactoryProvider
    {
        private readonly AsymmetricKeyParameter publicKey;

        public Asn1VerifierFactoryProvider(AsymmetricKeyParameter publicKey)
        {
            this.publicKey = publicKey;
        }

        public IVerifierFactory CreateVerifierFactory(object algorithmDetails) => 
            new Asn1VerifierFactory((AlgorithmIdentifier) algorithmDetails, this.publicKey);

        public IEnumerable SignatureAlgNames =>
            X509Utilities.GetAlgNames();
    }
}

