namespace Org.BouncyCastle.Crypto.Signers
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Digests;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Security;
    using System;

    public class PssSigner : ISigner
    {
        public const byte TrailerImplicit = 0xbc;
        private readonly IDigest contentDigest1;
        private readonly IDigest contentDigest2;
        private readonly IDigest mgfDigest;
        private readonly IAsymmetricBlockCipher cipher;
        private SecureRandom random;
        private int hLen;
        private int mgfhLen;
        private int sLen;
        private bool sSet;
        private int emBits;
        private byte[] salt;
        private byte[] mDash;
        private byte[] block;
        private byte trailer;

        public PssSigner(IAsymmetricBlockCipher cipher, IDigest digest) : this(cipher, digest, digest.GetDigestSize())
        {
        }

        public PssSigner(IAsymmetricBlockCipher cipher, IDigest digest, int saltLen) : this(cipher, digest, saltLen, 0xbc)
        {
        }

        public PssSigner(IAsymmetricBlockCipher cipher, IDigest digest, byte[] salt) : this(cipher, digest, digest, digest, salt.Length, salt, 0xbc)
        {
        }

        public PssSigner(IAsymmetricBlockCipher cipher, IDigest contentDigest, IDigest mgfDigest, int saltLen) : this(cipher, contentDigest, mgfDigest, saltLen, 0xbc)
        {
        }

        public PssSigner(IAsymmetricBlockCipher cipher, IDigest contentDigest, IDigest mgfDigest, byte[] salt) : this(cipher, contentDigest, contentDigest, mgfDigest, salt.Length, salt, 0xbc)
        {
        }

        public PssSigner(IAsymmetricBlockCipher cipher, IDigest digest, int saltLen, byte trailer) : this(cipher, digest, digest, saltLen, 0xbc)
        {
        }

        public PssSigner(IAsymmetricBlockCipher cipher, IDigest contentDigest, IDigest mgfDigest, int saltLen, byte trailer) : this(cipher, contentDigest, contentDigest, mgfDigest, saltLen, null, trailer)
        {
        }

        private PssSigner(IAsymmetricBlockCipher cipher, IDigest contentDigest1, IDigest contentDigest2, IDigest mgfDigest, int saltLen, byte[] salt, byte trailer)
        {
            this.cipher = cipher;
            this.contentDigest1 = contentDigest1;
            this.contentDigest2 = contentDigest2;
            this.mgfDigest = mgfDigest;
            this.hLen = contentDigest2.GetDigestSize();
            this.mgfhLen = mgfDigest.GetDigestSize();
            this.sLen = saltLen;
            this.sSet = salt != null;
            if (this.sSet)
            {
                this.salt = salt;
            }
            else
            {
                this.salt = new byte[saltLen];
            }
            this.mDash = new byte[(8 + saltLen) + this.hLen];
            this.trailer = trailer;
        }

        public virtual void BlockUpdate(byte[] input, int inOff, int length)
        {
            this.contentDigest1.BlockUpdate(input, inOff, length);
        }

        private void ClearBlock(byte[] block)
        {
            Array.Clear(block, 0, block.Length);
        }

        public static PssSigner CreateRawSigner(IAsymmetricBlockCipher cipher, IDigest digest) => 
            new PssSigner(cipher, new NullDigest(), digest, digest, digest.GetDigestSize(), null, 0xbc);

        public static PssSigner CreateRawSigner(IAsymmetricBlockCipher cipher, IDigest contentDigest, IDigest mgfDigest, int saltLen, byte trailer) => 
            new PssSigner(cipher, new NullDigest(), contentDigest, mgfDigest, saltLen, null, trailer);

        public virtual byte[] GenerateSignature()
        {
            this.contentDigest1.DoFinal(this.mDash, (this.mDash.Length - this.hLen) - this.sLen);
            if (this.sLen != 0)
            {
                if (!this.sSet)
                {
                    this.random.NextBytes(this.salt);
                }
                this.salt.CopyTo(this.mDash, (int) (this.mDash.Length - this.sLen));
            }
            byte[] output = new byte[this.hLen];
            this.contentDigest2.BlockUpdate(this.mDash, 0, this.mDash.Length);
            this.contentDigest2.DoFinal(output, 0);
            this.block[(((this.block.Length - this.sLen) - 1) - this.hLen) - 1] = 1;
            this.salt.CopyTo(this.block, (int) (((this.block.Length - this.sLen) - this.hLen) - 1));
            byte[] buffer2 = this.MaskGeneratorFunction1(output, 0, output.Length, (this.block.Length - this.hLen) - 1);
            for (int i = 0; i != buffer2.Length; i++)
            {
                this.block[i] = (byte) (this.block[i] ^ buffer2[i]);
            }
            this.block[0] = (byte) (this.block[0] & ((byte) (((int) 0xff) >> ((this.block.Length * 8) - this.emBits))));
            output.CopyTo(this.block, (int) ((this.block.Length - this.hLen) - 1));
            this.block[this.block.Length - 1] = this.trailer;
            byte[] buffer3 = this.cipher.ProcessBlock(this.block, 0, this.block.Length);
            this.ClearBlock(this.block);
            return buffer3;
        }

        public virtual void Init(bool forSigning, ICipherParameters parameters)
        {
            RsaKeyParameters publicKey;
            if (parameters is ParametersWithRandom)
            {
                ParametersWithRandom random = (ParametersWithRandom) parameters;
                parameters = random.Parameters;
                this.random = random.Random;
            }
            else if (forSigning)
            {
                this.random = new SecureRandom();
            }
            this.cipher.Init(forSigning, parameters);
            if (parameters is RsaBlindingParameters)
            {
                publicKey = ((RsaBlindingParameters) parameters).PublicKey;
            }
            else
            {
                publicKey = (RsaKeyParameters) parameters;
            }
            this.emBits = publicKey.Modulus.BitLength - 1;
            if (this.emBits < (((8 * this.hLen) + (8 * this.sLen)) + 9))
            {
                throw new ArgumentException("key too small for specified hash and salt lengths");
            }
            this.block = new byte[(this.emBits + 7) / 8];
        }

        private void ItoOSP(int i, byte[] sp)
        {
            sp[0] = (byte) (i >> 0x18);
            sp[1] = (byte) (i >> 0x10);
            sp[2] = (byte) (i >> 8);
            sp[3] = (byte) (i >> 0);
        }

        private byte[] MaskGeneratorFunction1(byte[] Z, int zOff, int zLen, int length)
        {
            byte[] array = new byte[length];
            byte[] output = new byte[this.mgfhLen];
            byte[] sp = new byte[4];
            int i = 0;
            this.mgfDigest.Reset();
            while (i < (length / this.mgfhLen))
            {
                this.ItoOSP(i, sp);
                this.mgfDigest.BlockUpdate(Z, zOff, zLen);
                this.mgfDigest.BlockUpdate(sp, 0, sp.Length);
                this.mgfDigest.DoFinal(output, 0);
                output.CopyTo(array, (int) (i * this.mgfhLen));
                i++;
            }
            if ((i * this.mgfhLen) < length)
            {
                this.ItoOSP(i, sp);
                this.mgfDigest.BlockUpdate(Z, zOff, zLen);
                this.mgfDigest.BlockUpdate(sp, 0, sp.Length);
                this.mgfDigest.DoFinal(output, 0);
                Array.Copy(output, 0, array, i * this.mgfhLen, array.Length - (i * this.mgfhLen));
            }
            return array;
        }

        public virtual void Reset()
        {
            this.contentDigest1.Reset();
        }

        public virtual void Update(byte input)
        {
            this.contentDigest1.Update(input);
        }

        public virtual bool VerifySignature(byte[] signature)
        {
            this.contentDigest1.DoFinal(this.mDash, (this.mDash.Length - this.hLen) - this.sLen);
            byte[] buffer = this.cipher.ProcessBlock(signature, 0, signature.Length);
            buffer.CopyTo(this.block, (int) (this.block.Length - buffer.Length));
            if (this.block[this.block.Length - 1] != this.trailer)
            {
                this.ClearBlock(this.block);
                return false;
            }
            byte[] buffer2 = this.MaskGeneratorFunction1(this.block, (this.block.Length - this.hLen) - 1, this.hLen, (this.block.Length - this.hLen) - 1);
            for (int i = 0; i != buffer2.Length; i++)
            {
                this.block[i] = (byte) (this.block[i] ^ buffer2[i]);
            }
            this.block[0] = (byte) (this.block[0] & ((byte) (((int) 0xff) >> ((this.block.Length * 8) - this.emBits))));
            for (int j = 0; j != (((this.block.Length - this.hLen) - this.sLen) - 2); j++)
            {
                if (this.block[j] != 0)
                {
                    this.ClearBlock(this.block);
                    return false;
                }
            }
            if (this.block[((this.block.Length - this.hLen) - this.sLen) - 2] != 1)
            {
                this.ClearBlock(this.block);
                return false;
            }
            if (this.sSet)
            {
                Array.Copy(this.salt, 0, this.mDash, this.mDash.Length - this.sLen, this.sLen);
            }
            else
            {
                Array.Copy(this.block, ((this.block.Length - this.sLen) - this.hLen) - 1, this.mDash, this.mDash.Length - this.sLen, this.sLen);
            }
            this.contentDigest2.BlockUpdate(this.mDash, 0, this.mDash.Length);
            this.contentDigest2.DoFinal(this.mDash, this.mDash.Length - this.hLen);
            int index = (this.block.Length - this.hLen) - 1;
            for (int k = this.mDash.Length - this.hLen; k != this.mDash.Length; k++)
            {
                if ((this.block[index] ^ this.mDash[k]) != 0)
                {
                    this.ClearBlock(this.mDash);
                    this.ClearBlock(this.block);
                    return false;
                }
                index++;
            }
            this.ClearBlock(this.mDash);
            this.ClearBlock(this.block);
            return true;
        }

        public virtual string AlgorithmName =>
            (this.mgfDigest.AlgorithmName + "withRSAandMGF1");
    }
}

