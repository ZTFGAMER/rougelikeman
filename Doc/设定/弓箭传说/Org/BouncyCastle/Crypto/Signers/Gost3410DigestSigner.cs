namespace Org.BouncyCastle.Crypto.Signers
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Security;
    using System;

    public class Gost3410DigestSigner : ISigner
    {
        private readonly IDigest digest;
        private readonly IDsa dsaSigner;
        private bool forSigning;

        public Gost3410DigestSigner(IDsa signer, IDigest digest)
        {
            this.dsaSigner = signer;
            this.digest = digest;
        }

        public virtual void BlockUpdate(byte[] input, int inOff, int length)
        {
            this.digest.BlockUpdate(input, inOff, length);
        }

        public virtual byte[] GenerateSignature()
        {
            byte[] buffer5;
            if (!this.forSigning)
            {
                throw new InvalidOperationException("GOST3410DigestSigner not initialised for signature generation.");
            }
            byte[] output = new byte[this.digest.GetDigestSize()];
            this.digest.DoFinal(output, 0);
            try
            {
                BigInteger[] integerArray = this.dsaSigner.GenerateSignature(output);
                byte[] array = new byte[0x40];
                byte[] buffer3 = integerArray[0].ToByteArrayUnsigned();
                byte[] buffer4 = integerArray[1].ToByteArrayUnsigned();
                buffer4.CopyTo(array, (int) (0x20 - buffer4.Length));
                buffer3.CopyTo(array, (int) (0x40 - buffer3.Length));
                buffer5 = array;
            }
            catch (Exception exception)
            {
                throw new SignatureException(exception.Message, exception);
            }
            return buffer5;
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
            BigInteger integer;
            BigInteger integer2;
            if (this.forSigning)
            {
                throw new InvalidOperationException("DSADigestSigner not initialised for verification");
            }
            byte[] output = new byte[this.digest.GetDigestSize()];
            this.digest.DoFinal(output, 0);
            try
            {
                integer = new BigInteger(1, signature, 0x20, 0x20);
                integer2 = new BigInteger(1, signature, 0, 0x20);
            }
            catch (Exception exception)
            {
                throw new SignatureException("error decoding signature bytes.", exception);
            }
            return this.dsaSigner.VerifySignature(output, integer, integer2);
        }

        public virtual string AlgorithmName =>
            (this.digest.AlgorithmName + "with" + this.dsaSigner.AlgorithmName);
    }
}

