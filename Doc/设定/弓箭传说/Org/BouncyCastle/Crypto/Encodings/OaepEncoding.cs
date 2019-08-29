namespace Org.BouncyCastle.Crypto.Encodings
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Security;
    using System;

    public class OaepEncoding : IAsymmetricBlockCipher
    {
        private byte[] defHash;
        private IDigest hash;
        private IDigest mgf1Hash;
        private IAsymmetricBlockCipher engine;
        private SecureRandom random;
        private bool forEncryption;

        public OaepEncoding(IAsymmetricBlockCipher cipher) : this(cipher, new Sha1Digest(), null)
        {
        }

        public OaepEncoding(IAsymmetricBlockCipher cipher, IDigest hash) : this(cipher, hash, null)
        {
        }

        public OaepEncoding(IAsymmetricBlockCipher cipher, IDigest hash, byte[] encodingParams) : this(cipher, hash, hash, encodingParams)
        {
        }

        public OaepEncoding(IAsymmetricBlockCipher cipher, IDigest hash, IDigest mgf1Hash, byte[] encodingParams)
        {
            this.engine = cipher;
            this.hash = hash;
            this.mgf1Hash = mgf1Hash;
            this.defHash = new byte[hash.GetDigestSize()];
            if (encodingParams != null)
            {
                hash.BlockUpdate(encodingParams, 0, encodingParams.Length);
            }
            hash.DoFinal(this.defHash, 0);
        }

        private byte[] DecodeBlock(byte[] inBytes, int inOff, int inLen)
        {
            byte[] buffer2;
            byte[] sourceArray = this.engine.ProcessBlock(inBytes, inOff, inLen);
            if (sourceArray.Length < this.engine.GetOutputBlockSize())
            {
                buffer2 = new byte[this.engine.GetOutputBlockSize()];
                Array.Copy(sourceArray, 0, buffer2, buffer2.Length - sourceArray.Length, sourceArray.Length);
            }
            else
            {
                buffer2 = sourceArray;
            }
            if (buffer2.Length < ((2 * this.defHash.Length) + 1))
            {
                throw new InvalidCipherTextException("data too short");
            }
            byte[] buffer3 = this.maskGeneratorFunction1(buffer2, this.defHash.Length, buffer2.Length - this.defHash.Length, this.defHash.Length);
            for (int i = 0; i != this.defHash.Length; i++)
            {
                buffer2[i] = (byte) (buffer2[i] ^ buffer3[i]);
            }
            buffer3 = this.maskGeneratorFunction1(buffer2, 0, this.defHash.Length, buffer2.Length - this.defHash.Length);
            for (int j = this.defHash.Length; j != buffer2.Length; j++)
            {
                buffer2[j] = (byte) (buffer2[j] ^ buffer3[j - this.defHash.Length]);
            }
            int num3 = 0;
            for (int k = 0; k < this.defHash.Length; k++)
            {
                num3 |= (byte) (this.defHash[k] ^ buffer2[this.defHash.Length + k]);
            }
            if (num3 != 0)
            {
                throw new InvalidCipherTextException("data hash wrong");
            }
            int index = 2 * this.defHash.Length;
            while (index != buffer2.Length)
            {
                if (buffer2[index] != 0)
                {
                    break;
                }
                index++;
            }
            if ((index > (buffer2.Length - 1)) || (buffer2[index] != 1))
            {
                throw new InvalidCipherTextException("data start wrong " + index);
            }
            index++;
            byte[] destinationArray = new byte[buffer2.Length - index];
            Array.Copy(buffer2, index, destinationArray, 0, destinationArray.Length);
            return destinationArray;
        }

        private byte[] EncodeBlock(byte[] inBytes, int inOff, int inLen)
        {
            byte[] destinationArray = new byte[(this.GetInputBlockSize() + 1) + (2 * this.defHash.Length)];
            Array.Copy(inBytes, inOff, destinationArray, destinationArray.Length - inLen, inLen);
            destinationArray[(destinationArray.Length - inLen) - 1] = 1;
            Array.Copy(this.defHash, 0, destinationArray, this.defHash.Length, this.defHash.Length);
            byte[] nextBytes = SecureRandom.GetNextBytes(this.random, this.defHash.Length);
            byte[] buffer3 = this.maskGeneratorFunction1(nextBytes, 0, nextBytes.Length, destinationArray.Length - this.defHash.Length);
            for (int i = this.defHash.Length; i != destinationArray.Length; i++)
            {
                destinationArray[i] = (byte) (destinationArray[i] ^ buffer3[i - this.defHash.Length]);
            }
            Array.Copy(nextBytes, 0, destinationArray, 0, this.defHash.Length);
            buffer3 = this.maskGeneratorFunction1(destinationArray, this.defHash.Length, destinationArray.Length - this.defHash.Length, this.defHash.Length);
            for (int j = 0; j != this.defHash.Length; j++)
            {
                destinationArray[j] = (byte) (destinationArray[j] ^ buffer3[j]);
            }
            return this.engine.ProcessBlock(destinationArray, 0, destinationArray.Length);
        }

        public int GetInputBlockSize()
        {
            int inputBlockSize = this.engine.GetInputBlockSize();
            if (this.forEncryption)
            {
                return ((inputBlockSize - 1) - (2 * this.defHash.Length));
            }
            return inputBlockSize;
        }

        public int GetOutputBlockSize()
        {
            int outputBlockSize = this.engine.GetOutputBlockSize();
            if (this.forEncryption)
            {
                return outputBlockSize;
            }
            return ((outputBlockSize - 1) - (2 * this.defHash.Length));
        }

        public IAsymmetricBlockCipher GetUnderlyingCipher() => 
            this.engine;

        public void Init(bool forEncryption, ICipherParameters param)
        {
            if (param is ParametersWithRandom)
            {
                ParametersWithRandom random = (ParametersWithRandom) param;
                this.random = random.Random;
            }
            else
            {
                this.random = new SecureRandom();
            }
            this.engine.Init(forEncryption, param);
            this.forEncryption = forEncryption;
        }

        private void ItoOSP(int i, byte[] sp)
        {
            sp[0] = (byte) (i >> 0x18);
            sp[1] = (byte) (i >> 0x10);
            sp[2] = (byte) (i >> 8);
            sp[3] = (byte) (i >> 0);
        }

        private byte[] maskGeneratorFunction1(byte[] Z, int zOff, int zLen, int length)
        {
            byte[] destinationArray = new byte[length];
            byte[] output = new byte[this.mgf1Hash.GetDigestSize()];
            byte[] sp = new byte[4];
            int i = 0;
            this.hash.Reset();
            do
            {
                this.ItoOSP(i, sp);
                this.mgf1Hash.BlockUpdate(Z, zOff, zLen);
                this.mgf1Hash.BlockUpdate(sp, 0, sp.Length);
                this.mgf1Hash.DoFinal(output, 0);
                Array.Copy(output, 0, destinationArray, i * output.Length, output.Length);
            }
            while (++i < (length / output.Length));
            if ((i * output.Length) < length)
            {
                this.ItoOSP(i, sp);
                this.mgf1Hash.BlockUpdate(Z, zOff, zLen);
                this.mgf1Hash.BlockUpdate(sp, 0, sp.Length);
                this.mgf1Hash.DoFinal(output, 0);
                Array.Copy(output, 0, destinationArray, i * output.Length, destinationArray.Length - (i * output.Length));
            }
            return destinationArray;
        }

        public byte[] ProcessBlock(byte[] inBytes, int inOff, int inLen)
        {
            if (this.forEncryption)
            {
                return this.EncodeBlock(inBytes, inOff, inLen);
            }
            return this.DecodeBlock(inBytes, inOff, inLen);
        }

        public string AlgorithmName =>
            (this.engine.AlgorithmName + "/OAEPPadding");
    }
}

