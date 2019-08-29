namespace Org.BouncyCastle.Crypto.Operators
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Asn1.X509;
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Security;
    using System;

    public class Asn1VerifierFactory : IVerifierFactory
    {
        private readonly AlgorithmIdentifier algID;
        private readonly AsymmetricKeyParameter publicKey;

        public Asn1VerifierFactory(AlgorithmIdentifier algorithm, AsymmetricKeyParameter publicKey)
        {
            this.publicKey = publicKey;
            this.algID = algorithm;
        }

        public Asn1VerifierFactory(string algorithm, AsymmetricKeyParameter publicKey)
        {
            DerObjectIdentifier algorithmOid = X509Utilities.GetAlgorithmOid(algorithm);
            this.publicKey = publicKey;
            this.algID = X509Utilities.GetSigAlgID(algorithmOid, algorithm);
        }

        public IStreamCalculator CreateCalculator()
        {
            ISigner sig = SignerUtilities.GetSigner(X509Utilities.GetSignatureName(this.algID));
            sig.Init(false, this.publicKey);
            return new VerifierCalculator(sig);
        }

        public object AlgorithmDetails =>
            this.algID;
    }
}

