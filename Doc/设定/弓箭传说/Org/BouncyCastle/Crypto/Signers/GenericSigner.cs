namespace Org.BouncyCastle.Crypto.Signers
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Security;
    using Org.BouncyCastle.Utilities;
    using System;

    public class GenericSigner : ISigner
    {
        private readonly IAsymmetricBlockCipher engine;
        private readonly IDigest digest;
        private bool forSigning;

        public GenericSigner(IAsymmetricBlockCipher engine, IDigest digest)
        {
            this.engine = engine;
            this.digest = digest;
        }

        public virtual void BlockUpdate(byte[] input, int inOff, int length)
        {
            this.digest.BlockUpdate(input, inOff, length);
        }

        public virtual byte[] GenerateSignature()
        {
            if (!this.forSigning)
            {
                throw new InvalidOperationException("GenericSigner not initialised for signature generation.");
            }
            byte[] output = new byte[this.digest.GetDigestSize()];
            this.digest.DoFinal(output, 0);
            return this.engine.ProcessBlock(output, 0, output.Length);
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
                throw new InvalidKeyException("Signing requires private key.");
            }
            if (!forSigning && parameter.IsPrivate)
            {
                throw new InvalidKeyException("Verification requires public key.");
            }
            this.Reset();
            this.engine.Init(forSigning, parameters);
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
                throw new InvalidOperationException("GenericSigner not initialised for verification");
            }
            byte[] output = new byte[this.digest.GetDigestSize()];
            this.digest.DoFinal(output, 0);
            try
            {
                byte[] sourceArray = this.engine.ProcessBlock(signature, 0, signature.Length);
                if (sourceArray.Length < output.Length)
                {
                    byte[] destinationArray = new byte[output.Length];
                    Array.Copy(sourceArray, 0, destinationArray, destinationArray.Length - sourceArray.Length, sourceArray.Length);
                    sourceArray = destinationArray;
                }
                return Arrays.ConstantTimeAreEqual(sourceArray, output);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public virtual string AlgorithmName
        {
            get
            {
                string[] textArray1 = new string[] { "Generic(", this.engine.AlgorithmName, "/", this.digest.AlgorithmName, ")" };
                return string.Concat(textArray1);
            }
        }
    }
}

