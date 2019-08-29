namespace Org.BouncyCastle.Crypto.Encodings
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Security;
    using Org.BouncyCastle.Utilities;
    using System;

    public class Pkcs1Encoding : IAsymmetricBlockCipher
    {
        public const string StrictLengthEnabledProperty = "Org.BouncyCastle.Pkcs1.Strict";
        private const int HeaderLength = 10;
        private static readonly bool[] strictLengthEnabled;
        private SecureRandom random;
        private IAsymmetricBlockCipher engine;
        private bool forEncryption;
        private bool forPrivateKey;
        private bool useStrictLength;
        private int pLen;
        private byte[] fallback;

        static Pkcs1Encoding()
        {
            string environmentVariable = Platform.GetEnvironmentVariable("Org.BouncyCastle.Pkcs1.Strict");
            strictLengthEnabled = new bool[] { (environmentVariable == null) || environmentVariable.Equals("true") };
        }

        public Pkcs1Encoding(IAsymmetricBlockCipher cipher)
        {
            this.pLen = -1;
            this.engine = cipher;
            this.useStrictLength = StrictLengthEnabled;
        }

        public Pkcs1Encoding(IAsymmetricBlockCipher cipher, int pLen)
        {
            this.pLen = -1;
            this.engine = cipher;
            this.useStrictLength = StrictLengthEnabled;
            this.pLen = pLen;
        }

        public Pkcs1Encoding(IAsymmetricBlockCipher cipher, byte[] fallback)
        {
            this.pLen = -1;
            this.engine = cipher;
            this.useStrictLength = StrictLengthEnabled;
            this.fallback = fallback;
            this.pLen = fallback.Length;
        }

        private static int CheckPkcs1Encoding(byte[] encoded, int pLen)
        {
            int num = 0;
            num |= encoded[0] ^ 2;
            int num2 = encoded.Length - (pLen + 1);
            for (int i = 1; i < num2; i++)
            {
                int num4 = encoded[i];
                num4 |= num4 >> 1;
                num4 |= num4 >> 2;
                num4 |= num4 >> 4;
                num |= (num4 & 1) - 1;
            }
            num |= encoded[encoded.Length - (pLen + 1)];
            num |= num >> 1;
            num |= num >> 2;
            num |= num >> 4;
            return ~((num & 1) - 1);
        }

        private byte[] DecodeBlock(byte[] input, int inOff, int inLen)
        {
            if (this.pLen != -1)
            {
                return this.DecodeBlockOrRandom(input, inOff, inLen);
            }
            byte[] sourceArray = this.engine.ProcessBlock(input, inOff, inLen);
            if (sourceArray.Length < this.GetOutputBlockSize())
            {
                throw new InvalidCipherTextException("block truncated");
            }
            byte num = sourceArray[0];
            if ((num != 1) && (num != 2))
            {
                throw new InvalidCipherTextException("unknown block type");
            }
            if (this.useStrictLength && (sourceArray.Length != this.engine.GetOutputBlockSize()))
            {
                throw new InvalidCipherTextException("block incorrect size");
            }
            int index = 1;
            while (index != sourceArray.Length)
            {
                byte num3 = sourceArray[index];
                if (num3 == 0)
                {
                    break;
                }
                if ((num == 1) && (num3 != 0xff))
                {
                    throw new InvalidCipherTextException("block padding incorrect");
                }
                index++;
            }
            index++;
            if ((index > sourceArray.Length) || (index < 10))
            {
                throw new InvalidCipherTextException("no data in block");
            }
            byte[] destinationArray = new byte[sourceArray.Length - index];
            Array.Copy(sourceArray, index, destinationArray, 0, destinationArray.Length);
            return destinationArray;
        }

        private byte[] DecodeBlockOrRandom(byte[] input, int inOff, int inLen)
        {
            if (!this.forPrivateKey)
            {
                throw new InvalidCipherTextException("sorry, this method is only for decryption, not for signing");
            }
            byte[] encoded = this.engine.ProcessBlock(input, inOff, inLen);
            byte[] buffer = null;
            if (this.fallback == null)
            {
                buffer = new byte[this.pLen];
                this.random.NextBytes(buffer);
            }
            else
            {
                buffer = this.fallback;
            }
            if (encoded.Length < this.GetOutputBlockSize())
            {
                throw new InvalidCipherTextException("block truncated");
            }
            if (this.useStrictLength && (encoded.Length != this.engine.GetOutputBlockSize()))
            {
                throw new InvalidCipherTextException("block incorrect size");
            }
            int num = CheckPkcs1Encoding(encoded, this.pLen);
            byte[] buffer3 = new byte[this.pLen];
            for (int i = 0; i < this.pLen; i++)
            {
                buffer3[i] = (byte) ((encoded[i + (encoded.Length - this.pLen)] & ~num) | (buffer[i] & num));
            }
            return buffer3;
        }

        private byte[] EncodeBlock(byte[] input, int inOff, int inLen)
        {
            if (inLen > this.GetInputBlockSize())
            {
                throw new ArgumentException("input data too large", "inLen");
            }
            byte[] buffer = new byte[this.engine.GetInputBlockSize()];
            if (this.forPrivateKey)
            {
                buffer[0] = 1;
                for (int i = 1; i != ((buffer.Length - inLen) - 1); i++)
                {
                    buffer[i] = 0xff;
                }
            }
            else
            {
                this.random.NextBytes(buffer);
                buffer[0] = 2;
                for (int i = 1; i != ((buffer.Length - inLen) - 1); i++)
                {
                    while (buffer[i] == 0)
                    {
                        buffer[i] = (byte) this.random.NextInt();
                    }
                }
            }
            buffer[(buffer.Length - inLen) - 1] = 0;
            Array.Copy(input, inOff, buffer, buffer.Length - inLen, inLen);
            return this.engine.ProcessBlock(buffer, 0, buffer.Length);
        }

        public int GetInputBlockSize()
        {
            int inputBlockSize = this.engine.GetInputBlockSize();
            return (!this.forEncryption ? inputBlockSize : (inputBlockSize - 10));
        }

        public int GetOutputBlockSize()
        {
            int outputBlockSize = this.engine.GetOutputBlockSize();
            return (!this.forEncryption ? (outputBlockSize - 10) : outputBlockSize);
        }

        public IAsymmetricBlockCipher GetUnderlyingCipher() => 
            this.engine;

        public void Init(bool forEncryption, ICipherParameters parameters)
        {
            AsymmetricKeyParameter parameter;
            if (parameters is ParametersWithRandom)
            {
                ParametersWithRandom random = (ParametersWithRandom) parameters;
                this.random = random.Random;
                parameter = (AsymmetricKeyParameter) random.Parameters;
            }
            else
            {
                this.random = new SecureRandom();
                parameter = (AsymmetricKeyParameter) parameters;
            }
            this.engine.Init(forEncryption, parameters);
            this.forPrivateKey = parameter.IsPrivate;
            this.forEncryption = forEncryption;
        }

        public byte[] ProcessBlock(byte[] input, int inOff, int length) => 
            (!this.forEncryption ? this.DecodeBlock(input, inOff, length) : this.EncodeBlock(input, inOff, length));

        public static bool StrictLengthEnabled
        {
            get => 
                strictLengthEnabled[0];
            set => 
                (strictLengthEnabled[0] = value);
        }

        public string AlgorithmName =>
            (this.engine.AlgorithmName + "/PKCS1Padding");
    }
}

