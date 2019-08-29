namespace Org.BouncyCastle.Crypto.Engines
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Modes;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Security;
    using Org.BouncyCastle.Utilities;
    using System;

    public class DesEdeWrapEngine : IWrapper
    {
        private CbcBlockCipher engine;
        private KeyParameter param;
        private ParametersWithIV paramPlusIV;
        private byte[] iv;
        private bool forWrapping;
        private static readonly byte[] IV2 = new byte[] { 0x4a, 0xdd, 0xa2, 0x2c, 0x79, 0xe8, 0x21, 5 };
        private readonly IDigest sha1 = new Sha1Digest();
        private readonly byte[] digest = new byte[20];

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
            SecureRandom random;
            this.forWrapping = forWrapping;
            this.engine = new CbcBlockCipher(new DesEdeEngine());
            if (parameters is ParametersWithRandom)
            {
                ParametersWithRandom random2 = (ParametersWithRandom) parameters;
                parameters = random2.Parameters;
                random = random2.Random;
            }
            else
            {
                random = new SecureRandom();
            }
            if (parameters is KeyParameter)
            {
                this.param = (KeyParameter) parameters;
                if (this.forWrapping)
                {
                    this.iv = new byte[8];
                    random.NextBytes(this.iv);
                    this.paramPlusIV = new ParametersWithIV(this.param, this.iv);
                }
            }
            else if (parameters is ParametersWithIV)
            {
                if (!forWrapping)
                {
                    throw new ArgumentException("You should not supply an IV for unwrapping");
                }
                this.paramPlusIV = (ParametersWithIV) parameters;
                this.iv = this.paramPlusIV.GetIV();
                this.param = (KeyParameter) this.paramPlusIV.Parameters;
                if (this.iv.Length != 8)
                {
                    throw new ArgumentException("IV is not 8 octets", "parameters");
                }
            }
        }

        private static byte[] reverse(byte[] bs)
        {
            byte[] buffer = new byte[bs.Length];
            for (int i = 0; i < bs.Length; i++)
            {
                buffer[i] = bs[bs.Length - (i + 1)];
            }
            return buffer;
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
            int blockSize = this.engine.GetBlockSize();
            if ((length % blockSize) != 0)
            {
                throw new InvalidCipherTextException("Ciphertext not multiple of " + blockSize);
            }
            ParametersWithIV parameters = new ParametersWithIV(this.param, IV2);
            this.engine.Init(false, parameters);
            byte[] output = new byte[length];
            for (int i = 0; i != output.Length; i += blockSize)
            {
                this.engine.ProcessBlock(input, inOff + i, output, i);
            }
            byte[] sourceArray = reverse(output);
            this.iv = new byte[8];
            byte[] destinationArray = new byte[sourceArray.Length - 8];
            Array.Copy(sourceArray, 0, this.iv, 0, 8);
            Array.Copy(sourceArray, 8, destinationArray, 0, sourceArray.Length - 8);
            this.paramPlusIV = new ParametersWithIV(this.param, this.iv);
            this.engine.Init(false, this.paramPlusIV);
            byte[] buffer4 = new byte[destinationArray.Length];
            for (int j = 0; j != buffer4.Length; j += blockSize)
            {
                this.engine.ProcessBlock(destinationArray, j, buffer4, j);
            }
            byte[] buffer5 = new byte[buffer4.Length - 8];
            byte[] buffer6 = new byte[8];
            Array.Copy(buffer4, 0, buffer5, 0, buffer4.Length - 8);
            Array.Copy(buffer4, buffer4.Length - 8, buffer6, 0, 8);
            if (!this.CheckCmsKeyChecksum(buffer5, buffer6))
            {
                throw new InvalidCipherTextException("Checksum inside ciphertext is corrupted");
            }
            return buffer5;
        }

        public virtual byte[] Wrap(byte[] input, int inOff, int length)
        {
            if (!this.forWrapping)
            {
                throw new InvalidOperationException("Not initialized for wrapping");
            }
            byte[] destinationArray = new byte[length];
            Array.Copy(input, inOff, destinationArray, 0, length);
            byte[] sourceArray = this.CalculateCmsKeyChecksum(destinationArray);
            byte[] buffer3 = new byte[destinationArray.Length + sourceArray.Length];
            Array.Copy(destinationArray, 0, buffer3, 0, destinationArray.Length);
            Array.Copy(sourceArray, 0, buffer3, destinationArray.Length, sourceArray.Length);
            int blockSize = this.engine.GetBlockSize();
            if ((buffer3.Length % blockSize) != 0)
            {
                throw new InvalidOperationException("Not multiple of block length");
            }
            this.engine.Init(true, this.paramPlusIV);
            byte[] output = new byte[buffer3.Length];
            for (int i = 0; i != buffer3.Length; i += blockSize)
            {
                this.engine.ProcessBlock(buffer3, i, output, i);
            }
            byte[] buffer5 = new byte[this.iv.Length + output.Length];
            Array.Copy(this.iv, 0, buffer5, 0, this.iv.Length);
            Array.Copy(output, 0, buffer5, this.iv.Length, output.Length);
            byte[] buffer6 = reverse(buffer5);
            ParametersWithIV parameters = new ParametersWithIV(this.param, IV2);
            this.engine.Init(true, parameters);
            for (int j = 0; j != buffer6.Length; j += blockSize)
            {
                this.engine.ProcessBlock(buffer6, j, buffer6, j);
            }
            return buffer6;
        }

        public virtual string AlgorithmName =>
            "DESede";
    }
}

