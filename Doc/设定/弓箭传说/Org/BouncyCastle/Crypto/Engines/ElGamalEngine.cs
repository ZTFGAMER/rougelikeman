namespace Org.BouncyCastle.Crypto.Engines
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Security;
    using System;

    public class ElGamalEngine : IAsymmetricBlockCipher
    {
        private ElGamalKeyParameters key;
        private SecureRandom random;
        private bool forEncryption;
        private int bitSize;

        public virtual int GetInputBlockSize()
        {
            if (this.forEncryption)
            {
                return ((this.bitSize - 1) / 8);
            }
            return (2 * ((this.bitSize + 7) / 8));
        }

        public virtual int GetOutputBlockSize()
        {
            if (this.forEncryption)
            {
                return (2 * ((this.bitSize + 7) / 8));
            }
            return ((this.bitSize - 1) / 8);
        }

        public virtual void Init(bool forEncryption, ICipherParameters parameters)
        {
            if (parameters is ParametersWithRandom)
            {
                ParametersWithRandom random = (ParametersWithRandom) parameters;
                this.key = (ElGamalKeyParameters) random.Parameters;
                this.random = random.Random;
            }
            else
            {
                this.key = (ElGamalKeyParameters) parameters;
                this.random = new SecureRandom();
            }
            this.forEncryption = forEncryption;
            this.bitSize = this.key.Parameters.P.BitLength;
            if (forEncryption)
            {
                if (!(this.key is ElGamalPublicKeyParameters))
                {
                    throw new ArgumentException("ElGamalPublicKeyParameters are required for encryption.");
                }
            }
            else if (!(this.key is ElGamalPrivateKeyParameters))
            {
                throw new ArgumentException("ElGamalPrivateKeyParameters are required for decryption.");
            }
        }

        public virtual byte[] ProcessBlock(byte[] input, int inOff, int length)
        {
            BigInteger integer7;
            if (this.key == null)
            {
                throw new InvalidOperationException("ElGamal engine not initialised");
            }
            int num = !this.forEncryption ? this.GetInputBlockSize() : (((this.bitSize - 1) + 7) / 8);
            if (length > num)
            {
                throw new DataLengthException("input too large for ElGamal cipher.\n");
            }
            BigInteger p = this.key.Parameters.P;
            if (this.key is ElGamalPrivateKeyParameters)
            {
                int num2 = length / 2;
                BigInteger integer2 = new BigInteger(1, input, inOff, num2);
                BigInteger val = new BigInteger(1, input, inOff + num2, num2);
                ElGamalPrivateKeyParameters parameters = (ElGamalPrivateKeyParameters) this.key;
                return integer2.ModPow(p.Subtract(BigInteger.One).Subtract(parameters.X), p).Multiply(val).Mod(p).ToByteArrayUnsigned();
            }
            BigInteger integer5 = new BigInteger(1, input, inOff, length);
            if (integer5.BitLength >= p.BitLength)
            {
                throw new DataLengthException("input too large for ElGamal cipher.\n");
            }
            ElGamalPublicKeyParameters key = (ElGamalPublicKeyParameters) this.key;
            BigInteger integer6 = p.Subtract(BigInteger.Two);
            do
            {
                integer7 = new BigInteger(p.BitLength, this.random);
            }
            while ((integer7.SignValue == 0) || (integer7.CompareTo(integer6) > 0));
            BigInteger integer9 = this.key.Parameters.G.ModPow(integer7, p);
            BigInteger integer10 = integer5.Multiply(key.Y.ModPow(integer7, p)).Mod(p);
            byte[] array = new byte[this.GetOutputBlockSize()];
            byte[] buffer2 = integer9.ToByteArrayUnsigned();
            byte[] buffer3 = integer10.ToByteArrayUnsigned();
            buffer2.CopyTo(array, (int) ((array.Length / 2) - buffer2.Length));
            buffer3.CopyTo(array, (int) (array.Length - buffer3.Length));
            return array;
        }

        public virtual string AlgorithmName =>
            "ElGamal";
    }
}

