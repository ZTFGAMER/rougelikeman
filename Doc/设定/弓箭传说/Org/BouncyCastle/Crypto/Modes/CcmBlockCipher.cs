namespace Org.BouncyCastle.Crypto.Modes
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Macs;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Utilities;
    using System;
    using System.IO;

    public class CcmBlockCipher : IAeadBlockCipher
    {
        private static readonly int BlockSize = 0x10;
        private readonly IBlockCipher cipher;
        private readonly byte[] macBlock;
        private bool forEncryption;
        private byte[] nonce;
        private byte[] initialAssociatedText;
        private int macSize;
        private ICipherParameters keyParam;
        private readonly MemoryStream associatedText = new MemoryStream();
        private readonly MemoryStream data = new MemoryStream();

        public CcmBlockCipher(IBlockCipher cipher)
        {
            this.cipher = cipher;
            this.macBlock = new byte[BlockSize];
            if (cipher.GetBlockSize() != BlockSize)
            {
                throw new ArgumentException("cipher required with a block size of " + BlockSize + ".");
            }
        }

        private int CalculateMac(byte[] data, int dataOff, int dataLen, byte[] macBlock)
        {
            IMac mac = new CbcBlockCipherMac(this.cipher, this.macSize * 8);
            mac.Init(this.keyParam);
            byte[] destinationArray = new byte[0x10];
            if (this.HasAssociatedText())
            {
                destinationArray[0] = (byte) (destinationArray[0] | 0x40);
            }
            destinationArray[0] = (byte) (destinationArray[0] | ((byte) ((((mac.GetMacSize() - 2) / 2) & 7) << 3)));
            destinationArray[0] = (byte) (destinationArray[0] | ((byte) (((15 - this.nonce.Length) - 1) & 7)));
            Array.Copy(this.nonce, 0, destinationArray, 1, this.nonce.Length);
            int num = dataLen;
            for (int i = 1; num > 0; i++)
            {
                destinationArray[destinationArray.Length - i] = (byte) (num & 0xff);
                num = num >> 8;
            }
            mac.BlockUpdate(destinationArray, 0, destinationArray.Length);
            if (this.HasAssociatedText())
            {
                int num3;
                int associatedTextLength = this.GetAssociatedTextLength();
                if (associatedTextLength < 0xff00)
                {
                    mac.Update((byte) (associatedTextLength >> 8));
                    mac.Update((byte) associatedTextLength);
                    num3 = 2;
                }
                else
                {
                    mac.Update(0xff);
                    mac.Update(0xfe);
                    mac.Update((byte) (associatedTextLength >> 0x18));
                    mac.Update((byte) (associatedTextLength >> 0x10));
                    mac.Update((byte) (associatedTextLength >> 8));
                    mac.Update((byte) associatedTextLength);
                    num3 = 6;
                }
                if (this.initialAssociatedText != null)
                {
                    mac.BlockUpdate(this.initialAssociatedText, 0, this.initialAssociatedText.Length);
                }
                if (this.associatedText.Position > 0L)
                {
                    byte[] buffer = this.associatedText.GetBuffer();
                    int position = (int) this.associatedText.Position;
                    mac.BlockUpdate(buffer, 0, position);
                }
                num3 = (num3 + associatedTextLength) % 0x10;
                if (num3 != 0)
                {
                    for (int j = num3; j < 0x10; j++)
                    {
                        mac.Update(0);
                    }
                }
            }
            mac.BlockUpdate(data, dataOff, dataLen);
            return mac.DoFinal(macBlock, 0);
        }

        public virtual int DoFinal(byte[] outBytes, int outOff)
        {
            byte[] input = this.data.GetBuffer();
            int position = (int) this.data.Position;
            int num2 = this.ProcessPacket(input, 0, position, outBytes, outOff);
            this.Reset();
            return num2;
        }

        private int GetAssociatedTextLength() => 
            (((int) this.associatedText.Length) + ((this.initialAssociatedText != null) ? this.initialAssociatedText.Length : 0));

        public virtual int GetBlockSize() => 
            this.cipher.GetBlockSize();

        public virtual byte[] GetMac() => 
            Arrays.CopyOfRange(this.macBlock, 0, this.macSize);

        public virtual int GetOutputSize(int len)
        {
            int num = ((int) this.data.Length) + len;
            if (this.forEncryption)
            {
                return (num + this.macSize);
            }
            return ((num >= this.macSize) ? (num - this.macSize) : 0);
        }

        public virtual IBlockCipher GetUnderlyingCipher() => 
            this.cipher;

        public virtual int GetUpdateOutputSize(int len) => 
            0;

        private bool HasAssociatedText() => 
            (this.GetAssociatedTextLength() > 0);

        public virtual void Init(bool forEncryption, ICipherParameters parameters)
        {
            ICipherParameters key;
            this.forEncryption = forEncryption;
            if (parameters is AeadParameters)
            {
                AeadParameters parameters3 = (AeadParameters) parameters;
                this.nonce = parameters3.GetNonce();
                this.initialAssociatedText = parameters3.GetAssociatedText();
                this.macSize = parameters3.MacSize / 8;
                key = parameters3.Key;
            }
            else
            {
                if (!(parameters is ParametersWithIV))
                {
                    throw new ArgumentException("invalid parameters passed to CCM");
                }
                ParametersWithIV hiv = (ParametersWithIV) parameters;
                this.nonce = hiv.GetIV();
                this.initialAssociatedText = null;
                this.macSize = this.macBlock.Length / 2;
                key = hiv.Parameters;
            }
            if (key != null)
            {
                this.keyParam = key;
            }
            if (((this.nonce == null) || (this.nonce.Length < 7)) || (this.nonce.Length > 13))
            {
                throw new ArgumentException("nonce must have length from 7 to 13 octets");
            }
            this.Reset();
        }

        public virtual void ProcessAadByte(byte input)
        {
            this.associatedText.WriteByte(input);
        }

        public virtual void ProcessAadBytes(byte[] inBytes, int inOff, int len)
        {
            this.associatedText.Write(inBytes, inOff, len);
        }

        public virtual int ProcessByte(byte input, byte[] outBytes, int outOff)
        {
            this.data.WriteByte(input);
            return 0;
        }

        public virtual int ProcessBytes(byte[] inBytes, int inOff, int inLen, byte[] outBytes, int outOff)
        {
            Check.DataLength(inBytes, inOff, inLen, "Input buffer too short");
            this.data.Write(inBytes, inOff, inLen);
            return 0;
        }

        public virtual byte[] ProcessPacket(byte[] input, int inOff, int inLen)
        {
            byte[] buffer;
            if (this.forEncryption)
            {
                buffer = new byte[inLen + this.macSize];
            }
            else
            {
                if (inLen < this.macSize)
                {
                    throw new InvalidCipherTextException("data too short");
                }
                buffer = new byte[inLen - this.macSize];
            }
            this.ProcessPacket(input, inOff, inLen, buffer, 0);
            return buffer;
        }

        public virtual int ProcessPacket(byte[] input, int inOff, int inLen, byte[] output, int outOff)
        {
            int num4;
            if (this.keyParam == null)
            {
                throw new InvalidOperationException("CCM cipher unitialized.");
            }
            int length = this.nonce.Length;
            int num2 = 15 - length;
            if (num2 < 4)
            {
                int num3 = ((int) 1) << (8 * num2);
                if (inLen >= num3)
                {
                    throw new InvalidOperationException("CCM packet too large for choice of q.");
                }
            }
            byte[] array = new byte[BlockSize];
            array[0] = (byte) ((num2 - 1) & 7);
            this.nonce.CopyTo(array, 1);
            IBlockCipher cipher = new SicBlockCipher(this.cipher);
            cipher.Init(this.forEncryption, new ParametersWithIV(this.keyParam, array));
            int num5 = inOff;
            int num6 = outOff;
            if (this.forEncryption)
            {
                num4 = inLen + this.macSize;
                Check.OutputLength(output, outOff, num4, "Output buffer too short.");
                this.CalculateMac(input, inOff, inLen, this.macBlock);
                byte[] outBuf = new byte[BlockSize];
                cipher.ProcessBlock(this.macBlock, 0, outBuf, 0);
                while (num5 < ((inOff + inLen) - BlockSize))
                {
                    cipher.ProcessBlock(input, num5, output, num6);
                    num6 += BlockSize;
                    num5 += BlockSize;
                }
                byte[] buffer3 = new byte[BlockSize];
                Array.Copy(input, num5, buffer3, 0, (inLen + inOff) - num5);
                cipher.ProcessBlock(buffer3, 0, buffer3, 0);
                Array.Copy(buffer3, 0, output, num6, (inLen + inOff) - num5);
                Array.Copy(outBuf, 0, output, outOff + inLen, this.macSize);
                return num4;
            }
            if (inLen < this.macSize)
            {
                throw new InvalidCipherTextException("data too short");
            }
            num4 = inLen - this.macSize;
            Check.OutputLength(output, outOff, num4, "Output buffer too short.");
            Array.Copy(input, inOff + num4, this.macBlock, 0, this.macSize);
            cipher.ProcessBlock(this.macBlock, 0, this.macBlock, 0);
            for (int i = this.macSize; i != this.macBlock.Length; i++)
            {
                this.macBlock[i] = 0;
            }
            while (num5 < ((inOff + num4) - BlockSize))
            {
                cipher.ProcessBlock(input, num5, output, num6);
                num6 += BlockSize;
                num5 += BlockSize;
            }
            byte[] destinationArray = new byte[BlockSize];
            Array.Copy(input, num5, destinationArray, 0, num4 - (num5 - inOff));
            cipher.ProcessBlock(destinationArray, 0, destinationArray, 0);
            Array.Copy(destinationArray, 0, output, num6, num4 - (num5 - inOff));
            byte[] macBlock = new byte[BlockSize];
            this.CalculateMac(output, outOff, num4, macBlock);
            if (!Arrays.ConstantTimeAreEqual(this.macBlock, macBlock))
            {
                throw new InvalidCipherTextException("mac check in CCM failed");
            }
            return num4;
        }

        public virtual void Reset()
        {
            this.cipher.Reset();
            this.associatedText.SetLength(0L);
            this.data.SetLength(0L);
        }

        public virtual string AlgorithmName =>
            (this.cipher.AlgorithmName + "/CCM");
    }
}

