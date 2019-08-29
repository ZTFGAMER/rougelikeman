namespace Org.BouncyCastle.Crypto.Operators
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Asn1.X509;
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Security;
    using System;
    using System.Collections;

    public class Asn1SignatureFactory : ISignatureFactory
    {
        private readonly AlgorithmIdentifier algID;
        private readonly string algorithm;
        private readonly AsymmetricKeyParameter privateKey;
        private readonly SecureRandom random;

        public Asn1SignatureFactory(string algorithm, AsymmetricKeyParameter privateKey) : this(algorithm, privateKey, null)
        {
        }

        public Asn1SignatureFactory(string algorithm, AsymmetricKeyParameter privateKey, SecureRandom random)
        {
            DerObjectIdentifier algorithmOid = X509Utilities.GetAlgorithmOid(algorithm);
            this.algorithm = algorithm;
            this.privateKey = privateKey;
            this.random = random;
            this.algID = X509Utilities.GetSigAlgID(algorithmOid, algorithm);
        }

        public IStreamCalculator CreateCalculator()
        {
            ISigner sig = SignerUtilities.GetSigner(this.algorithm);
            if (this.random != null)
            {
                sig.Init(true, new ParametersWithRandom(this.privateKey, this.random));
            }
            else
            {
                sig.Init(true, this.privateKey);
            }
            return new SigCalculator(sig);
        }

        public object AlgorithmDetails =>
            this.algID;

        public static IEnumerable SignatureAlgNames =>
            X509Utilities.GetAlgNames();
    }
}

