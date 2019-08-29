namespace Org.BouncyCastle.Crypto.Tls
{
    using System;
    using System.IO;

    public class DigitallySigned
    {
        protected readonly SignatureAndHashAlgorithm mAlgorithm;
        protected readonly byte[] mSignature;

        public DigitallySigned(SignatureAndHashAlgorithm algorithm, byte[] signature)
        {
            if (signature == null)
            {
                throw new ArgumentNullException("signature");
            }
            this.mAlgorithm = algorithm;
            this.mSignature = signature;
        }

        public virtual void Encode(Stream output)
        {
            if (this.mAlgorithm != null)
            {
                this.mAlgorithm.Encode(output);
            }
            TlsUtilities.WriteOpaque16(this.mSignature, output);
        }

        public static DigitallySigned Parse(TlsContext context, Stream input)
        {
            SignatureAndHashAlgorithm algorithm = null;
            if (TlsUtilities.IsTlsV12(context))
            {
                algorithm = SignatureAndHashAlgorithm.Parse(input);
            }
            return new DigitallySigned(algorithm, TlsUtilities.ReadOpaque16(input));
        }

        public virtual SignatureAndHashAlgorithm Algorithm =>
            this.mAlgorithm;

        public virtual byte[] Signature =>
            this.mSignature;
    }
}

