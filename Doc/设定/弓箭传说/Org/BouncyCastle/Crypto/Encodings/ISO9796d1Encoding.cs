namespace Org.BouncyCastle.Crypto.Encodings
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Math;
    using System;

    public class ISO9796d1Encoding : IAsymmetricBlockCipher
    {
        private static readonly BigInteger Sixteen = BigInteger.ValueOf(0x10L);
        private static readonly BigInteger Six = BigInteger.ValueOf(6L);
        private static readonly byte[] shadows = new byte[] { 14, 3, 5, 8, 9, 4, 2, 15, 0, 13, 11, 6, 7, 10, 12, 1 };
        private static readonly byte[] inverse = new byte[] { 8, 15, 6, 1, 5, 2, 11, 12, 3, 4, 13, 10, 14, 9, 0, 7 };
        private readonly IAsymmetricBlockCipher engine;
        private bool forEncryption;
        private int bitSize;
        private int padBits;
        private BigInteger modulus;

        public ISO9796d1Encoding(IAsymmetricBlockCipher cipher)
        {
            this.engine = cipher;
        }

        private byte[] DecodeBlock(byte[] input, int inOff, int inLen)
        {
            BigInteger integer2;
            byte[] bytes = this.engine.ProcessBlock(input, inOff, inLen);
            int num = 1;
            int num2 = (this.bitSize + 13) / 0x10;
            BigInteger n = new BigInteger(1, bytes);
            if (n.Mod(Sixteen).Equals(Six))
            {
                integer2 = n;
            }
            else
            {
                integer2 = this.modulus.Subtract(n);
                if (!integer2.Mod(Sixteen).Equals(Six))
                {
                    throw new InvalidCipherTextException("resulting integer iS or (modulus - iS) is not congruent to 6 mod 16");
                }
            }
            bytes = integer2.ToByteArrayUnsigned();
            if ((bytes[bytes.Length - 1] & 15) != 6)
            {
                throw new InvalidCipherTextException("invalid forcing byte in block");
            }
            bytes[bytes.Length - 1] = (byte) ((((ushort) (bytes[bytes.Length - 1] & 0xff)) >> 4) | (inverse[(bytes[bytes.Length - 2] & 0xff) >> 4] << 4));
            bytes[0] = (byte) ((shadows[(bytes[1] & 0xff) >> 4] << 4) | shadows[bytes[1] & 15]);
            bool flag = false;
            int index = 0;
            for (int i = bytes.Length - 1; i >= (bytes.Length - (2 * num2)); i -= 2)
            {
                int num5 = (shadows[(bytes[i] & 0xff) >> 4] << 4) | shadows[bytes[i] & 15];
                if (((bytes[i - 1] ^ num5) & 0xff) != 0)
                {
                    if (flag)
                    {
                        throw new InvalidCipherTextException("invalid tsums in block");
                    }
                    flag = true;
                    num = (bytes[i - 1] ^ num5) & 0xff;
                    index = i - 1;
                }
            }
            bytes[index] = 0;
            byte[] buffer2 = new byte[(bytes.Length - index) / 2];
            for (int j = 0; j < buffer2.Length; j++)
            {
                buffer2[j] = bytes[((2 * j) + index) + 1];
            }
            this.padBits = num - 1;
            return buffer2;
        }

        private byte[] EncodeBlock(byte[] input, int inOff, int inLen)
        {
            byte[] destinationArray = new byte[(this.bitSize + 7) / 8];
            int num = this.padBits + 1;
            int length = inLen;
            int num3 = (this.bitSize + 13) / 0x10;
            for (int i = 0; i < num3; i += length)
            {
                if (i > (num3 - length))
                {
                    Array.Copy(input, (int) ((inOff + inLen) - (num3 - i)), destinationArray, (int) (destinationArray.Length - num3), (int) (num3 - i));
                }
                else
                {
                    Array.Copy(input, inOff, destinationArray, destinationArray.Length - (i + length), length);
                }
            }
            for (int j = destinationArray.Length - (2 * num3); j != destinationArray.Length; j += 2)
            {
                byte num6 = destinationArray[(destinationArray.Length - num3) + (j / 2)];
                destinationArray[j] = (byte) ((shadows[(num6 & 0xff) >> 4] << 4) | shadows[num6 & 15]);
                destinationArray[j + 1] = num6;
            }
            destinationArray[destinationArray.Length - (2 * length)] = (byte) (destinationArray[destinationArray.Length - (2 * length)] ^ ((byte) num));
            destinationArray[destinationArray.Length - 1] = (byte) ((destinationArray[destinationArray.Length - 1] << 4) | 6);
            int num7 = 8 - ((this.bitSize - 1) % 8);
            int num8 = 0;
            if (num7 != 8)
            {
                destinationArray[0] = (byte) (destinationArray[0] & ((byte) (((int) 0xff) >> num7)));
                destinationArray[0] = (byte) (destinationArray[0] | ((byte) (((int) 0x80) >> num7)));
            }
            else
            {
                destinationArray[0] = 0;
                destinationArray[1] = (byte) (destinationArray[1] | 0x80);
                num8 = 1;
            }
            return this.engine.ProcessBlock(destinationArray, num8, destinationArray.Length - num8);
        }

        public int GetInputBlockSize()
        {
            int inputBlockSize = this.engine.GetInputBlockSize();
            if (this.forEncryption)
            {
                return ((inputBlockSize + 1) / 2);
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
            return ((outputBlockSize + 1) / 2);
        }

        public int GetPadBits() => 
            this.padBits;

        public IAsymmetricBlockCipher GetUnderlyingCipher() => 
            this.engine;

        public void Init(bool forEncryption, ICipherParameters parameters)
        {
            RsaKeyParameters parameters2;
            if (parameters is ParametersWithRandom)
            {
                ParametersWithRandom random = (ParametersWithRandom) parameters;
                parameters2 = (RsaKeyParameters) random.Parameters;
            }
            else
            {
                parameters2 = (RsaKeyParameters) parameters;
            }
            this.engine.Init(forEncryption, parameters);
            this.modulus = parameters2.Modulus;
            this.bitSize = this.modulus.BitLength;
            this.forEncryption = forEncryption;
        }

        public byte[] ProcessBlock(byte[] input, int inOff, int length)
        {
            if (this.forEncryption)
            {
                return this.EncodeBlock(input, inOff, length);
            }
            return this.DecodeBlock(input, inOff, length);
        }

        public void SetPadBits(int padBits)
        {
            if (padBits > 7)
            {
                throw new ArgumentException("padBits > 7");
            }
            this.padBits = padBits;
        }

        public string AlgorithmName =>
            (this.engine.AlgorithmName + "/ISO9796-1Padding");
    }
}

