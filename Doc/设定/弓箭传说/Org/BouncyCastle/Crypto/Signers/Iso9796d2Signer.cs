namespace Org.BouncyCastle.Crypto.Signers
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Utilities;
    using System;

    public class Iso9796d2Signer : ISignerWithRecovery, ISigner
    {
        [Obsolete("Use 'IsoTrailers' instead")]
        public const int TrailerImplicit = 0xbc;
        [Obsolete("Use 'IsoTrailers' instead")]
        public const int TrailerRipeMD160 = 0x31cc;
        [Obsolete("Use 'IsoTrailers' instead")]
        public const int TrailerRipeMD128 = 0x32cc;
        [Obsolete("Use 'IsoTrailers' instead")]
        public const int TrailerSha1 = 0x33cc;
        [Obsolete("Use 'IsoTrailers' instead")]
        public const int TrailerSha256 = 0x34cc;
        [Obsolete("Use 'IsoTrailers' instead")]
        public const int TrailerSha512 = 0x35cc;
        [Obsolete("Use 'IsoTrailers' instead")]
        public const int TrailerSha384 = 0x36cc;
        [Obsolete("Use 'IsoTrailers' instead")]
        public const int TrailerWhirlpool = 0x37cc;
        private IDigest digest;
        private IAsymmetricBlockCipher cipher;
        private int trailer;
        private int keyBits;
        private byte[] block;
        private byte[] mBuf;
        private int messageLength;
        private bool fullMessage;
        private byte[] recoveredMessage;
        private byte[] preSig;
        private byte[] preBlock;

        public Iso9796d2Signer(IAsymmetricBlockCipher cipher, IDigest digest) : this(cipher, digest, false)
        {
        }

        public Iso9796d2Signer(IAsymmetricBlockCipher cipher, IDigest digest, bool isImplicit)
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

        public virtual void BlockUpdate(byte[] input, int inOff, int length)
        {
            while ((length > 0) && (this.messageLength < this.mBuf.Length))
            {
                this.Update(input[inOff]);
                inOff++;
                length--;
            }
            this.digest.BlockUpdate(input, inOff, length);
            this.messageLength += length;
        }

        private void ClearBlock(byte[] block)
        {
            Array.Clear(block, 0, block.Length);
        }

        public virtual byte[] GenerateSignature()
        {
            int digestSize = this.digest.GetDigestSize();
            int num2 = 0;
            int outOff = 0;
            if (this.trailer == 0xbc)
            {
                num2 = 8;
                outOff = (this.block.Length - digestSize) - 1;
                this.digest.DoFinal(this.block, outOff);
                this.block[this.block.Length - 1] = 0xbc;
            }
            else
            {
                num2 = 0x10;
                outOff = (this.block.Length - digestSize) - 2;
                this.digest.DoFinal(this.block, outOff);
                this.block[this.block.Length - 2] = (byte) (this.trailer >> 8);
                this.block[this.block.Length - 1] = (byte) this.trailer;
            }
            byte num4 = 0;
            int num5 = ((((digestSize + this.messageLength) * 8) + num2) + 4) - this.keyBits;
            if (num5 > 0)
            {
                int length = this.messageLength - ((num5 + 7) / 8);
                num4 = 0x60;
                outOff -= length;
                Array.Copy(this.mBuf, 0, this.block, outOff, length);
            }
            else
            {
                num4 = 0x40;
                outOff -= this.messageLength;
                Array.Copy(this.mBuf, 0, this.block, outOff, this.messageLength);
            }
            if ((outOff - 1) > 0)
            {
                for (int i = outOff - 1; i != 0; i--)
                {
                    this.block[i] = 0xbb;
                }
                this.block[outOff - 1] = (byte) (this.block[outOff - 1] ^ 1);
                this.block[0] = 11;
                this.block[0] = (byte) (this.block[0] | num4);
            }
            else
            {
                this.block[0] = 10;
                this.block[0] = (byte) (this.block[0] | num4);
            }
            byte[] buffer = this.cipher.ProcessBlock(this.block, 0, this.block.Length);
            this.ClearBlock(this.mBuf);
            this.ClearBlock(this.block);
            return buffer;
        }

        public byte[] GetRecoveredMessage() => 
            this.recoveredMessage;

        public virtual bool HasFullMessage() => 
            this.fullMessage;

        public virtual void Init(bool forSigning, ICipherParameters parameters)
        {
            RsaKeyParameters parameters2 = (RsaKeyParameters) parameters;
            this.cipher.Init(forSigning, parameters2);
            this.keyBits = parameters2.Modulus.BitLength;
            this.block = new byte[(this.keyBits + 7) / 8];
            if (this.trailer == 0xbc)
            {
                this.mBuf = new byte[(this.block.Length - this.digest.GetDigestSize()) - 2];
            }
            else
            {
                this.mBuf = new byte[(this.block.Length - this.digest.GetDigestSize()) - 3];
            }
            this.Reset();
        }

        private bool IsSameAs(byte[] a, byte[] b)
        {
            int length;
            if (this.messageLength > this.mBuf.Length)
            {
                if (this.mBuf.Length > b.Length)
                {
                    return false;
                }
                length = this.mBuf.Length;
            }
            else
            {
                if (this.messageLength != b.Length)
                {
                    return false;
                }
                length = b.Length;
            }
            bool flag = true;
            for (int i = 0; i != length; i++)
            {
                if (a[i] != b[i])
                {
                    flag = false;
                }
            }
            return flag;
        }

        public virtual void Reset()
        {
            this.digest.Reset();
            this.messageLength = 0;
            this.ClearBlock(this.mBuf);
            if (this.recoveredMessage != null)
            {
                this.ClearBlock(this.recoveredMessage);
            }
            this.recoveredMessage = null;
            this.fullMessage = false;
            if (this.preSig != null)
            {
                this.preSig = null;
                this.ClearBlock(this.preBlock);
                this.preBlock = null;
            }
        }

        private bool ReturnFalse(byte[] block)
        {
            this.ClearBlock(this.mBuf);
            this.ClearBlock(block);
            return false;
        }

        public virtual void Update(byte input)
        {
            this.digest.Update(input);
            if (this.messageLength < this.mBuf.Length)
            {
                this.mBuf[this.messageLength] = input;
            }
            this.messageLength++;
        }

        public virtual void UpdateWithRecoveredMessage(byte[] signature)
        {
            byte[] sourceArray = this.cipher.ProcessBlock(signature, 0, signature.Length);
            if (((sourceArray[0] & 0xc0) ^ 0x40) != 0)
            {
                throw new InvalidCipherTextException("malformed signature");
            }
            if (((sourceArray[sourceArray.Length - 1] & 15) ^ 12) != 0)
            {
                throw new InvalidCipherTextException("malformed signature");
            }
            int num = 0;
            if (((sourceArray[sourceArray.Length - 1] & 0xff) ^ 0xbc) == 0)
            {
                num = 1;
            }
            else
            {
                int num2 = ((sourceArray[sourceArray.Length - 2] & 0xff) << 8) | (sourceArray[sourceArray.Length - 1] & 0xff);
                if (IsoTrailers.NoTrailerAvailable(this.digest))
                {
                    throw new ArgumentException("unrecognised hash in signature");
                }
                if (num2 != IsoTrailers.GetTrailer(this.digest))
                {
                    throw new InvalidOperationException("signer initialised with wrong digest for trailer " + num2);
                }
                num = 2;
            }
            int index = 0;
            index = 0;
            while (index != sourceArray.Length)
            {
                if (((sourceArray[index] & 15) ^ 10) == 0)
                {
                    break;
                }
                index++;
            }
            index++;
            int num4 = (sourceArray.Length - num) - this.digest.GetDigestSize();
            if ((num4 - index) <= 0)
            {
                throw new InvalidCipherTextException("malformed block");
            }
            if ((sourceArray[0] & 0x20) == 0)
            {
                this.fullMessage = true;
                this.recoveredMessage = new byte[num4 - index];
                Array.Copy(sourceArray, index, this.recoveredMessage, 0, this.recoveredMessage.Length);
            }
            else
            {
                this.fullMessage = false;
                this.recoveredMessage = new byte[num4 - index];
                Array.Copy(sourceArray, index, this.recoveredMessage, 0, this.recoveredMessage.Length);
            }
            this.preSig = signature;
            this.preBlock = sourceArray;
            this.digest.BlockUpdate(this.recoveredMessage, 0, this.recoveredMessage.Length);
            this.messageLength = this.recoveredMessage.Length;
            this.recoveredMessage.CopyTo(this.mBuf, 0);
        }

        public virtual bool VerifySignature(byte[] signature)
        {
            byte[] preBlock;
            if (this.preSig == null)
            {
                try
                {
                    preBlock = this.cipher.ProcessBlock(signature, 0, signature.Length);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
            {
                if (!Arrays.AreEqual(this.preSig, signature))
                {
                    throw new InvalidOperationException("updateWithRecoveredMessage called on different signature");
                }
                preBlock = this.preBlock;
                this.preSig = null;
                this.preBlock = null;
            }
            if (((preBlock[0] & 0xc0) ^ 0x40) != 0)
            {
                return this.ReturnFalse(preBlock);
            }
            if (((preBlock[preBlock.Length - 1] & 15) ^ 12) != 0)
            {
                return this.ReturnFalse(preBlock);
            }
            int num = 0;
            if (((preBlock[preBlock.Length - 1] & 0xff) ^ 0xbc) == 0)
            {
                num = 1;
            }
            else
            {
                int num2 = ((preBlock[preBlock.Length - 2] & 0xff) << 8) | (preBlock[preBlock.Length - 1] & 0xff);
                if (IsoTrailers.NoTrailerAvailable(this.digest))
                {
                    throw new ArgumentException("unrecognised hash in signature");
                }
                if (num2 != IsoTrailers.GetTrailer(this.digest))
                {
                    throw new InvalidOperationException("signer initialised with wrong digest for trailer " + num2);
                }
                num = 2;
            }
            int index = 0;
            while (index != preBlock.Length)
            {
                if (((preBlock[index] & 15) ^ 10) == 0)
                {
                    break;
                }
                index++;
            }
            index++;
            byte[] output = new byte[this.digest.GetDigestSize()];
            int num4 = (preBlock.Length - num) - output.Length;
            if ((num4 - index) <= 0)
            {
                return this.ReturnFalse(preBlock);
            }
            if ((preBlock[0] & 0x20) == 0)
            {
                this.fullMessage = true;
                if (this.messageLength > (num4 - index))
                {
                    return this.ReturnFalse(preBlock);
                }
                this.digest.Reset();
                this.digest.BlockUpdate(preBlock, index, num4 - index);
                this.digest.DoFinal(output, 0);
                bool flag2 = true;
                for (int i = 0; i != output.Length; i++)
                {
                    preBlock[num4 + i] = (byte) (preBlock[num4 + i] ^ output[i]);
                    if (preBlock[num4 + i] != 0)
                    {
                        flag2 = false;
                    }
                }
                if (!flag2)
                {
                    return this.ReturnFalse(preBlock);
                }
                this.recoveredMessage = new byte[num4 - index];
                Array.Copy(preBlock, index, this.recoveredMessage, 0, this.recoveredMessage.Length);
            }
            else
            {
                this.fullMessage = false;
                this.digest.DoFinal(output, 0);
                bool flag3 = true;
                for (int i = 0; i != output.Length; i++)
                {
                    preBlock[num4 + i] = (byte) (preBlock[num4 + i] ^ output[i]);
                    if (preBlock[num4 + i] != 0)
                    {
                        flag3 = false;
                    }
                }
                if (!flag3)
                {
                    return this.ReturnFalse(preBlock);
                }
                this.recoveredMessage = new byte[num4 - index];
                Array.Copy(preBlock, index, this.recoveredMessage, 0, this.recoveredMessage.Length);
            }
            if ((this.messageLength != 0) && !this.IsSameAs(this.mBuf, this.recoveredMessage))
            {
                return this.ReturnFalse(preBlock);
            }
            this.ClearBlock(this.mBuf);
            this.ClearBlock(preBlock);
            return true;
        }

        public virtual string AlgorithmName =>
            (this.digest.AlgorithmName + "withISO9796-2S1");
    }
}

