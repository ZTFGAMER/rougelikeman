namespace Org.BouncyCastle.Crypto.Tls
{
    using System;
    using System.IO;

    public class SignatureAndHashAlgorithm
    {
        protected readonly byte mHash;
        protected readonly byte mSignature;

        public SignatureAndHashAlgorithm(byte hash, byte signature)
        {
            if (!TlsUtilities.IsValidUint8((int) hash))
            {
                throw new ArgumentException("should be a uint8", "hash");
            }
            if (!TlsUtilities.IsValidUint8((int) signature))
            {
                throw new ArgumentException("should be a uint8", "signature");
            }
            if (signature == 0)
            {
                throw new ArgumentException("MUST NOT be \"anonymous\"", "signature");
            }
            this.mHash = hash;
            this.mSignature = signature;
        }

        public virtual void Encode(Stream output)
        {
            TlsUtilities.WriteUint8(this.Hash, output);
            TlsUtilities.WriteUint8(this.Signature, output);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is SignatureAndHashAlgorithm))
            {
                return false;
            }
            SignatureAndHashAlgorithm algorithm = (SignatureAndHashAlgorithm) obj;
            return ((algorithm.Hash == this.Hash) && (algorithm.Signature == this.Signature));
        }

        public override int GetHashCode() => 
            ((this.Hash << 0x10) | this.Signature);

        public static SignatureAndHashAlgorithm Parse(Stream input)
        {
            byte hash = TlsUtilities.ReadUint8(input);
            return new SignatureAndHashAlgorithm(hash, TlsUtilities.ReadUint8(input));
        }

        public virtual byte Hash =>
            this.mHash;

        public virtual byte Signature =>
            this.mSignature;
    }
}

