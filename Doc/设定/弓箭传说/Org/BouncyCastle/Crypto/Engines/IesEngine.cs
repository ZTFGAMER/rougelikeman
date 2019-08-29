namespace Org.BouncyCastle.Crypto.Engines
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Utilities;
    using System;

    public class IesEngine
    {
        private readonly IBasicAgreement agree;
        private readonly IDerivationFunction kdf;
        private readonly IMac mac;
        private readonly BufferedBlockCipher cipher;
        private readonly byte[] macBuf;
        private bool forEncryption;
        private ICipherParameters privParam;
        private ICipherParameters pubParam;
        private IesParameters param;

        public IesEngine(IBasicAgreement agree, IDerivationFunction kdf, IMac mac)
        {
            this.agree = agree;
            this.kdf = kdf;
            this.mac = mac;
            this.macBuf = new byte[mac.GetMacSize()];
        }

        public IesEngine(IBasicAgreement agree, IDerivationFunction kdf, IMac mac, BufferedBlockCipher cipher)
        {
            this.agree = agree;
            this.kdf = kdf;
            this.mac = mac;
            this.macBuf = new byte[mac.GetMacSize()];
            this.cipher = cipher;
        }

        private byte[] DecryptBlock(byte[] in_enc, int inOff, int inLen, byte[] z)
        {
            byte[] buffer = null;
            KeyParameter parameter = null;
            KdfParameters parameters = new KdfParameters(z, this.param.GetDerivationV());
            int macKeySize = this.param.MacKeySize;
            this.kdf.Init(parameters);
            if (inLen < this.mac.GetMacSize())
            {
                throw new InvalidCipherTextException("Length of input must be greater than the MAC");
            }
            inLen -= this.mac.GetMacSize();
            if (this.cipher == null)
            {
                byte[] key = this.GenerateKdfBytes(parameters, inLen + (macKeySize / 8));
                buffer = new byte[inLen];
                for (int i = 0; i != inLen; i++)
                {
                    buffer[i] = (byte) (in_enc[inOff + i] ^ key[i]);
                }
                parameter = new KeyParameter(key, inLen, macKeySize / 8);
            }
            else
            {
                int cipherKeySize = ((IesWithCipherParameters) this.param).CipherKeySize;
                byte[] key = this.GenerateKdfBytes(parameters, (cipherKeySize / 8) + (macKeySize / 8));
                this.cipher.Init(false, new KeyParameter(key, 0, cipherKeySize / 8));
                buffer = this.cipher.DoFinal(in_enc, inOff, inLen);
                parameter = new KeyParameter(key, cipherKeySize / 8, macKeySize / 8);
            }
            byte[] encodingV = this.param.GetEncodingV();
            this.mac.Init(parameter);
            this.mac.BlockUpdate(in_enc, inOff, inLen);
            this.mac.BlockUpdate(encodingV, 0, encodingV.Length);
            this.mac.DoFinal(this.macBuf, 0);
            inOff += inLen;
            if (!Arrays.ConstantTimeAreEqual(Arrays.CopyOfRange(in_enc, inOff, inOff + this.macBuf.Length), this.macBuf))
            {
                throw new InvalidCipherTextException("Invalid MAC.");
            }
            return buffer;
        }

        private byte[] EncryptBlock(byte[] input, int inOff, int inLen, byte[] z)
        {
            byte[] destinationArray = null;
            KeyParameter parameter = null;
            KdfParameters kParam = new KdfParameters(z, this.param.GetDerivationV());
            int len = 0;
            int macKeySize = this.param.MacKeySize;
            if (this.cipher == null)
            {
                byte[] key = this.GenerateKdfBytes(kParam, inLen + (macKeySize / 8));
                destinationArray = new byte[inLen + this.mac.GetMacSize()];
                len = inLen;
                for (int i = 0; i != inLen; i++)
                {
                    destinationArray[i] = (byte) (input[inOff + i] ^ key[i]);
                }
                parameter = new KeyParameter(key, inLen, macKeySize / 8);
            }
            else
            {
                int cipherKeySize = ((IesWithCipherParameters) this.param).CipherKeySize;
                byte[] key = this.GenerateKdfBytes(kParam, (cipherKeySize / 8) + (macKeySize / 8));
                this.cipher.Init(true, new KeyParameter(key, 0, cipherKeySize / 8));
                byte[] output = new byte[this.cipher.GetOutputSize(inLen)];
                int outOff = this.cipher.ProcessBytes(input, inOff, inLen, output, 0);
                outOff += this.cipher.DoFinal(output, outOff);
                destinationArray = new byte[outOff + this.mac.GetMacSize()];
                len = outOff;
                Array.Copy(output, 0, destinationArray, 0, outOff);
                parameter = new KeyParameter(key, cipherKeySize / 8, macKeySize / 8);
            }
            byte[] encodingV = this.param.GetEncodingV();
            this.mac.Init(parameter);
            this.mac.BlockUpdate(destinationArray, 0, len);
            this.mac.BlockUpdate(encodingV, 0, encodingV.Length);
            this.mac.DoFinal(destinationArray, len);
            return destinationArray;
        }

        private byte[] GenerateKdfBytes(KdfParameters kParam, int length)
        {
            byte[] output = new byte[length];
            this.kdf.Init(kParam);
            this.kdf.GenerateBytes(output, 0, output.Length);
            return output;
        }

        public virtual void Init(bool forEncryption, ICipherParameters privParameters, ICipherParameters pubParameters, ICipherParameters iesParameters)
        {
            this.forEncryption = forEncryption;
            this.privParam = privParameters;
            this.pubParam = pubParameters;
            this.param = (IesParameters) iesParameters;
        }

        public virtual byte[] ProcessBlock(byte[] input, int inOff, int inLen)
        {
            byte[] buffer2;
            this.agree.Init(this.privParam);
            BigInteger n = this.agree.CalculateAgreement(this.pubParam);
            byte[] z = BigIntegers.AsUnsignedByteArray(this.agree.GetFieldSize(), n);
            try
            {
                buffer2 = !this.forEncryption ? this.DecryptBlock(input, inOff, inLen, z) : this.EncryptBlock(input, inOff, inLen, z);
            }
            finally
            {
                Array.Clear(z, 0, z.Length);
            }
            return buffer2;
        }
    }
}

