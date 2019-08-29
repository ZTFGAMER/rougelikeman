namespace Org.BouncyCastle.Crypto.Engines
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Modes;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Security;
    using Org.BouncyCastle.Utilities;
    using System;

    public class RC2WrapEngine : IWrapper
    {
        private CbcBlockCipher engine;
        private ICipherParameters parameters;
        private ParametersWithIV paramPlusIV;
        private byte[] iv;
        private bool forWrapping;
        private SecureRandom sr;
        private static readonly byte[] IV2 = new byte[] { 0x4a, 0xdd, 0xa2, 0x2c, 0x79, 0xe8, 0x21, 5 };
        private IDigest sha1 = new Sha1Digest();
        private byte[] digest = new byte[20];

        private byte[] CalculateCmsKeyChecksum(byte[] key)
        {
            this.sha1.BlockUpdate(key, 0, key.Length);
            this.sha1.DoFinal(this.digest, 0);
            byte[] destinationArray = new byte[8];
            Array.Copy(this.digest, 0, destinationArray, 0, 8);
            return destinationArray;
        }

        private bool CheckCmsKeyChecksum(byte[] key, byte[] checksum) => 
            Arrays.ConstantTimeAreEqual(this.CalculateCmsKeyChecksum(key), checksum);

        public virtual void Init(bool forWrapping, ICipherParameters parameters)
        {
            this.forWrapping = forWrapping;
            this.engine = new CbcBlockCipher(new RC2Engine());
            if (parameters is ParametersWithRandom)
            {
                ParametersWithRandom random = (ParametersWithRandom) parameters;
                this.sr = random.Random;
                parameters = random.Parameters;
            }
            else
            {
                this.sr = new SecureRandom();
            }
            if (parameters is ParametersWithIV)
            {
                if (!forWrapping)
                {
                    throw new ArgumentException("You should not supply an IV for unwrapping");
                }
                this.paramPlusIV = (ParametersWithIV) parameters;
                this.iv = this.paramPlusIV.GetIV();
                this.parameters = this.paramPlusIV.Parameters;
                if (this.iv.Length != 8)
                {
                    throw new ArgumentException("IV is not 8 octets");
                }
            }
            else
            {
                this.parameters = parameters;
                if (this.forWrapping)
                {
                    this.iv = new byte[8];
                    this.sr.NextBytes(this.iv);
                    this.paramPlusIV = new ParametersWithIV(this.parameters, this.iv);
                }
            }
        }

        public virtual byte[] Unwrap(byte[] input, int inOff, int length)
        {
            if (this.forWrapping)
            {
                throw new InvalidOperationException("Not set for unwrapping");
            }
            if (input == null)
            {
                throw new InvalidCipherTextException("Null pointer as ciphertext");
            }
            if ((length % this.engine.GetBlockSize()) != 0)
            {
                throw new InvalidCipherTextException("Ciphertext not multiple of " + this.engine.GetBlockSize());
            }
            ParametersWithIV parameters = new ParametersWithIV(this.parameters, IV2);
            this.engine.Init(false, parameters);
            byte[] destinationArray = new byte[length];
            Array.Copy(input, inOff, destinationArray, 0, length);
            for (int i = 0; i < (destinationArray.Length / this.engine.GetBlockSize()); i++)
            {
                int num2 = i * this.engine.GetBlockSize();
                this.engine.ProcessBlock(destinationArray, num2, destinationArray, num2);
            }
            byte[] sourceArray = new byte[destinationArray.Length];
            for (int j = 0; j < destinationArray.Length; j++)
            {
                sourceArray[j] = destinationArray[destinationArray.Length - (j + 1)];
            }
            this.iv = new byte[8];
            byte[] buffer3 = new byte[sourceArray.Length - 8];
            Array.Copy(sourceArray, 0, this.iv, 0, 8);
            Array.Copy(sourceArray, 8, buffer3, 0, sourceArray.Length - 8);
            this.paramPlusIV = new ParametersWithIV(this.parameters, this.iv);
            this.engine.Init(false, this.paramPlusIV);
            byte[] buffer4 = new byte[buffer3.Length];
            Array.Copy(buffer3, 0, buffer4, 0, buffer3.Length);
            for (int k = 0; k < (buffer4.Length / this.engine.GetBlockSize()); k++)
            {
                int num5 = k * this.engine.GetBlockSize();
                this.engine.ProcessBlock(buffer4, num5, buffer4, num5);
            }
            byte[] buffer5 = new byte[buffer4.Length - 8];
            byte[] buffer6 = new byte[8];
            Array.Copy(buffer4, 0, buffer5, 0, buffer4.Length - 8);
            Array.Copy(buffer4, buffer4.Length - 8, buffer6, 0, 8);
            if (!this.CheckCmsKeyChecksum(buffer5, buffer6))
            {
                throw new InvalidCipherTextException("Checksum inside ciphertext is corrupted");
            }
            if ((buffer5.Length - ((buffer5[0] & 0xff) + 1)) > 7)
            {
                throw new InvalidCipherTextException("too many pad bytes (" + (buffer5.Length - ((buffer5[0] & 0xff) + 1)) + ")");
            }
            byte[] buffer7 = new byte[buffer5[0]];
            Array.Copy(buffer5, 1, buffer7, 0, buffer7.Length);
            return buffer7;
        }

        public virtual byte[] Wrap(byte[] input, int inOff, int length)
        {
            if (!this.forWrapping)
            {
                throw new InvalidOperationException("Not initialized for wrapping");
            }
            int num = length + 1;
            if ((num % 8) != 0)
            {
                num += 8 - (num % 8);
            }
            byte[] destinationArray = new byte[num];
            destinationArray[0] = (byte) length;
            Array.Copy(input, inOff, destinationArray, 1, length);
            byte[] buffer = new byte[(destinationArray.Length - length) - 1];
            if (buffer.Length > 0)
            {
                this.sr.NextBytes(buffer);
                Array.Copy(buffer, 0, destinationArray, length + 1, buffer.Length);
            }
            byte[] sourceArray = this.CalculateCmsKeyChecksum(destinationArray);
            byte[] buffer4 = new byte[destinationArray.Length + sourceArray.Length];
            Array.Copy(destinationArray, 0, buffer4, 0, destinationArray.Length);
            Array.Copy(sourceArray, 0, buffer4, destinationArray.Length, sourceArray.Length);
            byte[] buffer5 = new byte[buffer4.Length];
            Array.Copy(buffer4, 0, buffer5, 0, buffer4.Length);
            int num2 = buffer4.Length / this.engine.GetBlockSize();
            if ((buffer4.Length % this.engine.GetBlockSize()) != 0)
            {
                throw new InvalidOperationException("Not multiple of block length");
            }
            this.engine.Init(true, this.paramPlusIV);
            for (int i = 0; i < num2; i++)
            {
                int num5 = i * this.engine.GetBlockSize();
                this.engine.ProcessBlock(buffer5, num5, buffer5, num5);
            }
            byte[] buffer6 = new byte[this.iv.Length + buffer5.Length];
            Array.Copy(this.iv, 0, buffer6, 0, this.iv.Length);
            Array.Copy(buffer5, 0, buffer6, this.iv.Length, buffer5.Length);
            byte[] buffer7 = new byte[buffer6.Length];
            for (int j = 0; j < buffer6.Length; j++)
            {
                buffer7[j] = buffer6[buffer6.Length - (j + 1)];
            }
            ParametersWithIV parameters = new ParametersWithIV(this.parameters, IV2);
            this.engine.Init(true, parameters);
            for (int k = 0; k < (num2 + 1); k++)
            {
                int num8 = k * this.engine.GetBlockSize();
                this.engine.ProcessBlock(buffer7, num8, buffer7, num8);
            }
            return buffer7;
        }

        public virtual string AlgorithmName =>
            "RC2";
    }
}

