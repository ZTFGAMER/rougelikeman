namespace Org.BouncyCastle.Crypto.Operators
{
    using Org.BouncyCastle.Crypto;
    using System;
    using System.IO;

    internal class SigCalculator : IStreamCalculator
    {
        private readonly ISigner sig;
        private readonly System.IO.Stream stream;

        internal SigCalculator(ISigner sig)
        {
            this.sig = sig;
            this.stream = new SignerBucket(sig);
        }

        public object GetResult() => 
            new SigResult(this.sig);

        public System.IO.Stream Stream =>
            this.stream;
    }
}

