namespace Org.BouncyCastle.Crypto.Signers
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Security;
    using System;
    using System.IO;

    public class DsaDigestSigner : ISigner
    {
        private readonly IDigest digest;
        private readonly IDsa dsaSigner;
        private bool forSigning;

        public DsaDigestSigner(IDsa signer, IDigest digest)
        {
            this.digest = digest;
            this.dsaSigner = signer;
        }

        public virtual void BlockUpdate(byte[] input, int inOff, int length)
        {
            this.digest.BlockUpdate(input, inOff, length);
        }

        private BigInteger[] DerDecode(byte[] encoding)
        {
            Asn1Sequence sequence = (Asn1Sequence) Asn1Object.FromByteArray(encoding);
            return new BigInteger[] { ((DerInteger) sequence[0]).Value, ((DerInteger) sequence[1]).Value };
        }

        private byte[] DerEncode(BigInteger r, BigInteger s)
        {
            Asn1Encodable[] v = new Asn1Encodable[] { new DerInteger(r), new DerInteger(s) };
            return new DerSequence(v).GetDerEncoded();
        }

        public virtual byte[] GenerateSignature()
        {
            if (!this.forSigning)
            {
                throw new InvalidOperationException("DSADigestSigner not initialised for signature generation.");
            }
            byte[] output = new byte[this.digest.GetDigestSize()];
            this.digest.DoFinal(output, 0);
            BigInteger[] integerArray = this.dsaSigner.GenerateSignature(output);
            return this.DerEncode(integerArray[0], integerArray[1]);
        }

        public virtual void Init(bool forSigning, ICipherParameters parameters)
        {
            AsymmetricKeyParameter parameter;
            this.forSigning = forSigning;
            if (parameters is ParametersWithRandom)
            {
                parameter = (AsymmetricKeyParameter) ((ParametersWithRandom) parameters).Parameters;
            }
            else
            {
                parameter = (AsymmetricKeyParameter) parameters;
            }
            if (forSigning && !parameter.IsPrivate)
            {
                throw new InvalidKeyException("Signing Requires Private Key.");
            }
            if (!forSigning && parameter.IsPrivate)
            {
                throw new InvalidKeyException("Verification Requires Public Key.");
            }
            this.Reset();
            this.dsaSigner.Init(forSigning, parameters);
        }

        public virtual void Reset()
        {
            this.digest.Reset();
        }

        public virtual void Update(byte input)
        {
            this.digest.Update(input);
        }

        public virtual bool VerifySignature(byte[] signature)
        {
            if (this.forSigning)
            {
                throw new InvalidOperationException("DSADigestSigner not initialised for verification");
            }
            byte[] output = new byte[this.digest.GetDigestSize()];
            this.digest.DoFinal(output, 0);
            try
            {
                BigInteger[] integerArray = this.DerDecode(signature);
                return this.dsaSigner.VerifySignature(output, integerArray[0], integerArray[1]);
            }
            catch (IOException)
            {
                return false;
            }
        }

        public virtual string AlgorithmName =>
            (this.digest.AlgorithmName + "with" + this.dsaSigner.AlgorithmName);
    }
}

