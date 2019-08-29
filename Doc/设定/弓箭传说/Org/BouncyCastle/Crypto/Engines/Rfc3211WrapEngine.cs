namespace Org.BouncyCastle.Crypto.Engines
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Modes;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Security;
    using System;

    public class Rfc3211WrapEngine : IWrapper
    {
        private CbcBlockCipher engine;
        private ParametersWithIV param;
        private bool forWrapping;
        private SecureRandom rand;

        public Rfc3211WrapEngine(IBlockCipher engine)
        {
            this.engine = new CbcBlockCipher(engine);
        }

        public virtual void Init(bool forWrapping, ICipherParameters param)
        {
            this.forWrapping = forWrapping;
            if (param is ParametersWithRandom)
            {
                ParametersWithRandom random = (ParametersWithRandom) param;
                this.rand = random.Random;
                this.param = (ParametersWithIV) random.Parameters;
            }
            else
            {
                if (forWrapping)
                {
                    this.rand = new SecureRandom();
                }
                this.param = (ParametersWithIV) param;
            }
        }

        public virtual byte[] Unwrap(byte[] inBytes, int inOff, int inLen)
        {
            if (this.forWrapping)
            {
                throw new InvalidOperationException("not set for unwrapping");
            }
            int blockSize = this.engine.GetBlockSize();
            if (inLen < (2 * blockSize))
            {
                throw new InvalidCipherTextException("input too short");
            }
            byte[] destinationArray = new byte[inLen];
            byte[] buffer2 = new byte[blockSize];
            Array.Copy(inBytes, inOff, destinationArray, 0, inLen);
            Array.Copy(inBytes, inOff, buffer2, 0, buffer2.Length);
            this.engine.Init(false, new ParametersWithIV(this.param.Parameters, buffer2));
            for (int i = blockSize; i < destinationArray.Length; i += blockSize)
            {
                this.engine.ProcessBlock(destinationArray, i, destinationArray, i);
            }
            Array.Copy(destinationArray, destinationArray.Length - buffer2.Length, buffer2, 0, buffer2.Length);
            this.engine.Init(false, new ParametersWithIV(this.param.Parameters, buffer2));
            this.engine.ProcessBlock(destinationArray, 0, destinationArray, 0);
            this.engine.Init(false, this.param);
            for (int j = 0; j < destinationArray.Length; j += blockSize)
            {
                this.engine.ProcessBlock(destinationArray, j, destinationArray, j);
            }
            if ((destinationArray[0] & 0xff) > (destinationArray.Length - 4))
            {
                throw new InvalidCipherTextException("wrapped key corrupted");
            }
            byte[] buffer3 = new byte[destinationArray[0] & 0xff];
            Array.Copy(destinationArray, 4, buffer3, 0, destinationArray[0]);
            int num4 = 0;
            for (int k = 0; k != 3; k++)
            {
                byte num6 = ~destinationArray[1 + k];
                num4 |= num6 ^ buffer3[k];
            }
            if (num4 != 0)
            {
                throw new InvalidCipherTextException("wrapped key fails checksum");
            }
            return buffer3;
        }

        public virtual byte[] Wrap(byte[] inBytes, int inOff, int inLen)
        {
            byte[] buffer;
            if (!this.forWrapping)
            {
                throw new InvalidOperationException("not set for wrapping");
            }
            this.engine.Init(true, this.param);
            int blockSize = this.engine.GetBlockSize();
            if ((inLen + 4) < (blockSize * 2))
            {
                buffer = new byte[blockSize * 2];
            }
            else
            {
                buffer = new byte[(((inLen + 4) % blockSize) != 0) ? ((((inLen + 4) / blockSize) + 1) * blockSize) : (inLen + 4)];
            }
            buffer[0] = (byte) inLen;
            buffer[1] = ~inBytes[inOff];
            buffer[2] = ~inBytes[inOff + 1];
            buffer[3] = ~inBytes[inOff + 2];
            Array.Copy(inBytes, inOff, buffer, 4, inLen);
            this.rand.NextBytes(buffer, inLen + 4, (buffer.Length - inLen) - 4);
            for (int i = 0; i < buffer.Length; i += blockSize)
            {
                this.engine.ProcessBlock(buffer, i, buffer, i);
            }
            for (int j = 0; j < buffer.Length; j += blockSize)
            {
                this.engine.ProcessBlock(buffer, j, buffer, j);
            }
            return buffer;
        }

        public virtual string AlgorithmName =>
            (this.engine.GetUnderlyingCipher().AlgorithmName + "/RFC3211Wrap");
    }
}

