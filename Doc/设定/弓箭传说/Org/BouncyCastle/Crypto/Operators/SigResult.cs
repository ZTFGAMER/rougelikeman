namespace Org.BouncyCastle.Crypto.Operators
{
    using Org.BouncyCastle.Crypto;
    using System;

    internal class SigResult : IBlockResult
    {
        private readonly ISigner sig;

        internal SigResult(ISigner sig)
        {
            this.sig = sig;
        }

        public byte[] Collect() => 
            this.sig.GenerateSignature();

        public int Collect(byte[] destination, int offset)
        {
            byte[] sourceArray = this.Collect();
            Array.Copy(sourceArray, 0, destination, offset, sourceArray.Length);
            return sourceArray.Length;
        }
    }
}

