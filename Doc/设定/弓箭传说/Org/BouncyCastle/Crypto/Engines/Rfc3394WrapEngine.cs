namespace Org.BouncyCastle.Crypto.Engines
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Utilities;
    using System;

    public class Rfc3394WrapEngine : IWrapper
    {
        private readonly IBlockCipher engine;
        private KeyParameter param;
        private bool forWrapping;
        private byte[] iv = new byte[] { 0xa6, 0xa6, 0xa6, 0xa6, 0xa6, 0xa6, 0xa6, 0xa6 };

        public Rfc3394WrapEngine(IBlockCipher engine)
        {
            this.engine = engine;
        }

        public virtual void Init(bool forWrapping, ICipherParameters parameters)
        {
            this.forWrapping = forWrapping;
            if (parameters is ParametersWithRandom)
            {
                parameters = ((ParametersWithRandom) parameters).Parameters;
            }
            if (parameters is KeyParameter)
            {
                this.param = (KeyParameter) parameters;
            }
            else if (parameters is ParametersWithIV)
            {
                ParametersWithIV hiv = (ParametersWithIV) parameters;
                byte[] iV = hiv.GetIV();
                if (iV.Length != 8)
                {
                    throw new ArgumentException("IV length not equal to 8", "parameters");
                }
                this.iv = iV;
                this.param = (KeyParameter) hiv.Parameters;
            }
        }

        public virtual byte[] Unwrap(byte[] input, int inOff, int inLen)
        {
            if (this.forWrapping)
            {
                throw new InvalidOperationException("not set for unwrapping");
            }
            int num = inLen / 8;
            if ((num * 8) != inLen)
            {
                throw new InvalidCipherTextException("unwrap data must be a multiple of 8 bytes");
            }
            byte[] destinationArray = new byte[inLen - this.iv.Length];
            byte[] buffer2 = new byte[this.iv.Length];
            byte[] buffer3 = new byte[8 + this.iv.Length];
            Array.Copy(input, inOff, buffer2, 0, this.iv.Length);
            Array.Copy(input, inOff + this.iv.Length, destinationArray, 0, inLen - this.iv.Length);
            this.engine.Init(false, this.param);
            num--;
            for (int i = 5; i >= 0; i--)
            {
                for (int j = num; j >= 1; j--)
                {
                    Array.Copy(buffer2, 0, buffer3, 0, this.iv.Length);
                    Array.Copy(destinationArray, 8 * (j - 1), buffer3, this.iv.Length, 8);
                    int num4 = (num * i) + j;
                    for (int k = 1; num4 != 0; k++)
                    {
                        byte num6 = (byte) num4;
                        buffer3[this.iv.Length - k] = (byte) (buffer3[this.iv.Length - k] ^ num6);
                        num4 = num4 >> 8;
                    }
                    this.engine.ProcessBlock(buffer3, 0, buffer3, 0);
                    Array.Copy(buffer3, 0, buffer2, 0, 8);
                    Array.Copy(buffer3, 8, destinationArray, 8 * (j - 1), 8);
                }
            }
            if (!Arrays.ConstantTimeAreEqual(buffer2, this.iv))
            {
                throw new InvalidCipherTextException("checksum failed");
            }
            return destinationArray;
        }

        public virtual byte[] Wrap(byte[] input, int inOff, int inLen)
        {
            if (!this.forWrapping)
            {
                throw new InvalidOperationException("not set for wrapping");
            }
            int num = inLen / 8;
            if ((num * 8) != inLen)
            {
                throw new DataLengthException("wrap data must be a multiple of 8 bytes");
            }
            byte[] destinationArray = new byte[inLen + this.iv.Length];
            byte[] buffer2 = new byte[8 + this.iv.Length];
            Array.Copy(this.iv, 0, destinationArray, 0, this.iv.Length);
            Array.Copy(input, inOff, destinationArray, this.iv.Length, inLen);
            this.engine.Init(true, this.param);
            for (int i = 0; i != 6; i++)
            {
                for (int j = 1; j <= num; j++)
                {
                    Array.Copy(destinationArray, 0, buffer2, 0, this.iv.Length);
                    Array.Copy(destinationArray, 8 * j, buffer2, this.iv.Length, 8);
                    this.engine.ProcessBlock(buffer2, 0, buffer2, 0);
                    int num4 = (num * i) + j;
                    for (int k = 1; num4 != 0; k++)
                    {
                        byte num6 = (byte) num4;
                        buffer2[this.iv.Length - k] = (byte) (buffer2[this.iv.Length - k] ^ num6);
                        num4 = num4 >> 8;
                    }
                    Array.Copy(buffer2, 0, destinationArray, 0, 8);
                    Array.Copy(buffer2, 8, destinationArray, 8 * j, 8);
                }
            }
            return destinationArray;
        }

        public virtual string AlgorithmName =>
            this.engine.AlgorithmName;
    }
}

