namespace Org.BouncyCastle.Crypto.Operators
{
    using Org.BouncyCastle.Crypto;
    using System;

    internal class VerifierResult : IVerifier
    {
        private readonly ISigner sig;

        internal VerifierResult(ISigner sig)
        {
            this.sig = sig;
        }

        public bool IsVerified(byte[] signature) => 
            this.sig.VerifySignature(signature);

        public bool IsVerified(byte[] signature, int off, int length)
        {
            byte[] destinationArray = new byte[length];
            Array.Copy(signature, 0, destinationArray, off, destinationArray.Length);
            return this.sig.VerifySignature(signature);
        }
    }
}

