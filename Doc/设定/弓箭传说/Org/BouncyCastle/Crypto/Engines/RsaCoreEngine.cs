namespace Org.BouncyCastle.Crypto.Engines
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Security;
    using System;

    internal class RsaCoreEngine
    {
        private RsaKeyParameters key;
        private bool forEncryption;
        private int bitSize;

        public virtual BigInteger ConvertInput(byte[] inBuf, int inOff, int inLen)
        {
            int num = (this.bitSize + 7) / 8;
            if (inLen > num)
            {
                throw new DataLengthException("input too large for RSA cipher.");
            }
            BigInteger integer = new BigInteger(1, inBuf, inOff, inLen);
            if (integer.CompareTo(this.key.Modulus) >= 0)
            {
                throw new DataLengthException("input too large for RSA cipher.");
            }
            return integer;
        }

        public virtual byte[] ConvertOutput(BigInteger result)
        {
            byte[] buffer = result.ToByteArrayUnsigned();
            if (this.forEncryption)
            {
                int outputBlockSize = this.GetOutputBlockSize();
                if (buffer.Length < outputBlockSize)
                {
                    byte[] array = new byte[outputBlockSize];
                    buffer.CopyTo(array, (int) (array.Length - buffer.Length));
                    buffer = array;
                }
            }
            return buffer;
        }

        public virtual int GetInputBlockSize()
        {
            if (this.forEncryption)
            {
                return ((this.bitSize - 1) / 8);
            }
            return ((this.bitSize + 7) / 8);
        }

        public virtual int GetOutputBlockSize()
        {
            if (this.forEncryption)
            {
                return ((this.bitSize + 7) / 8);
            }
            return ((this.bitSize - 1) / 8);
        }

        public virtual void Init(bool forEncryption, ICipherParameters parameters)
        {
            if (parameters is ParametersWithRandom)
            {
                parameters = ((ParametersWithRandom) parameters).Parameters;
            }
            if (!(parameters is RsaKeyParameters))
            {
                throw new InvalidKeyException("Not an RSA key");
            }
            this.key = (RsaKeyParameters) parameters;
            this.forEncryption = forEncryption;
            this.bitSize = this.key.Modulus.BitLength;
        }

        public virtual BigInteger ProcessBlock(BigInteger input)
        {
            if (this.key is RsaPrivateCrtKeyParameters)
            {
                RsaPrivateCrtKeyParameters key = (RsaPrivateCrtKeyParameters) this.key;
                BigInteger p = key.P;
                BigInteger q = key.Q;
                BigInteger dP = key.DP;
                BigInteger dQ = key.DQ;
                BigInteger qInv = key.QInv;
                BigInteger integer6 = input.Remainder(p).ModPow(dP, p);
                BigInteger integer7 = input.Remainder(q).ModPow(dQ, q);
                return integer6.Subtract(integer7).Multiply(qInv).Mod(p).Multiply(q).Add(integer7);
            }
            return input.ModPow(this.key.Exponent, this.key.Modulus);
        }
    }
}

