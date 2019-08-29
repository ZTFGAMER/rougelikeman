namespace Org.BouncyCastle.Crypto.Signers
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Utilities;
    using System;

    public class X931Signer : ISigner
    {
        [Obsolete("Use 'IsoTrailers' instead")]
        public const int TRAILER_IMPLICIT = 0xbc;
        [Obsolete("Use 'IsoTrailers' instead")]
        public const int TRAILER_RIPEMD160 = 0x31cc;
        [Obsolete("Use 'IsoTrailers' instead")]
        public const int TRAILER_RIPEMD128 = 0x32cc;
        [Obsolete("Use 'IsoTrailers' instead")]
        public const int TRAILER_SHA1 = 0x33cc;
        [Obsolete("Use 'IsoTrailers' instead")]
        public const int TRAILER_SHA256 = 0x34cc;
        [Obsolete("Use 'IsoTrailers' instead")]
        public const int TRAILER_SHA512 = 0x35cc;
        [Obsolete("Use 'IsoTrailers' instead")]
        public const int TRAILER_SHA384 = 0x36cc;
        [Obsolete("Use 'IsoTrailers' instead")]
        public const int TRAILER_WHIRLPOOL = 0x37cc;
        [Obsolete("Use 'IsoTrailers' instead")]
        public const int TRAILER_SHA224 = 0x38cc;
        private IDigest digest;
        private IAsymmetricBlockCipher cipher;
        private RsaKeyParameters kParam;
        private int trailer;
        private int keyBits;
        private byte[] block;

        public X931Signer(IAsymmetricBlockCipher cipher, IDigest digest) : this(cipher, digest, false)
        {
        }

        public X931Signer(IAsymmetricBlockCipher cipher, IDigest digest, bool isImplicit)
        {
            this.cipher = cipher;
            this.digest = digest;
            if (isImplicit)
            {
                this.trailer = 0xbc;
            }
            else
            {
                if (IsoTrailers.NoTrailerAvailable(digest))
                {
                    throw new ArgumentException("no valid trailer", "digest");
                }
                this.trailer = IsoTrailers.GetTrailer(digest);
            }
        }

        public virtual void BlockUpdate(byte[] input, int off, int len)
        {
            this.digest.BlockUpdate(input, off, len);
        }

        private void ClearBlock(byte[] block)
        {
            Array.Clear(block, 0, block.Length);
        }

        private void CreateSignatureBlock()
        {
            int num2;
            int digestSize = this.digest.GetDigestSize();
            if (this.trailer == 0xbc)
            {
                num2 = (this.block.Length - digestSize) - 1;
                this.digest.DoFinal(this.block, num2);
                this.block[this.block.Length - 1] = 0xbc;
            }
            else
            {
                num2 = (this.block.Length - digestSize) - 2;
                this.digest.DoFinal(this.block, num2);
                this.block[this.block.Length - 2] = (byte) (this.trailer >> 8);
                this.block[this.block.Length - 1] = (byte) this.trailer;
            }
            this.block[0] = 0x6b;
            for (int i = num2 - 2; i != 0; i--)
            {
                this.block[i] = 0xbb;
            }
            this.block[num2 - 1] = 0xba;
        }

        public virtual byte[] GenerateSignature()
        {
            this.CreateSignatureBlock();
            BigInteger n = new BigInteger(1, this.cipher.ProcessBlock(this.block, 0, this.block.Length));
            this.ClearBlock(this.block);
            n = n.Min(this.kParam.Modulus.Subtract(n));
            return BigIntegers.AsUnsignedByteArray((this.kParam.Modulus.BitLength + 7) / 8, n);
        }

        public virtual void Init(bool forSigning, ICipherParameters parameters)
        {
            this.kParam = (RsaKeyParameters) parameters;
            this.cipher.Init(forSigning, this.kParam);
            this.keyBits = this.kParam.Modulus.BitLength;
            this.block = new byte[(this.keyBits + 7) / 8];
            this.Reset();
        }

        public virtual void Reset()
        {
            this.digest.Reset();
        }

        public virtual void Update(byte b)
        {
            this.digest.Update(b);
        }

        public virtual bool VerifySignature(byte[] signature)
        {
            BigInteger integer2;
            try
            {
                this.block = this.cipher.ProcessBlock(signature, 0, signature.Length);
            }
            catch (Exception)
            {
                return false;
            }
            BigInteger n = new BigInteger(1, this.block);
            if ((n.IntValue & 15) == 12)
            {
                integer2 = n;
            }
            else
            {
                n = this.kParam.Modulus.Subtract(n);
                if ((n.IntValue & 15) == 12)
                {
                    integer2 = n;
                }
                else
                {
                    return false;
                }
            }
            this.CreateSignatureBlock();
            byte[] b = BigIntegers.AsUnsignedByteArray(this.block.Length, integer2);
            bool flag2 = Arrays.ConstantTimeAreEqual(this.block, b);
            this.ClearBlock(this.block);
            this.ClearBlock(b);
            return flag2;
        }

        public virtual string AlgorithmName =>
            (this.digest.AlgorithmName + "with" + this.cipher.AlgorithmName + "/X9.31");
    }
}

