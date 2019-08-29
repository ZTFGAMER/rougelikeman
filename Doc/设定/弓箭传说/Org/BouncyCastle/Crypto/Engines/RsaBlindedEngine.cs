namespace Org.BouncyCastle.Crypto.Engines
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Security;
    using Org.BouncyCastle.Utilities;
    using System;

    public class RsaBlindedEngine : IAsymmetricBlockCipher
    {
        private readonly RsaCoreEngine core = new RsaCoreEngine();
        private RsaKeyParameters key;
        private SecureRandom random;

        public virtual int GetInputBlockSize() => 
            this.core.GetInputBlockSize();

        public virtual int GetOutputBlockSize() => 
            this.core.GetOutputBlockSize();

        public virtual void Init(bool forEncryption, ICipherParameters param)
        {
            this.core.Init(forEncryption, param);
            if (param is ParametersWithRandom)
            {
                ParametersWithRandom random = (ParametersWithRandom) param;
                this.key = (RsaKeyParameters) random.Parameters;
                this.random = random.Random;
            }
            else
            {
                this.key = (RsaKeyParameters) param;
                this.random = new SecureRandom();
            }
        }

        public virtual byte[] ProcessBlock(byte[] inBuf, int inOff, int inLen)
        {
            BigInteger integer2;
            if (this.key == null)
            {
                throw new InvalidOperationException("RSA engine not initialised");
            }
            BigInteger val = this.core.ConvertInput(inBuf, inOff, inLen);
            if (this.key is RsaPrivateCrtKeyParameters)
            {
                RsaPrivateCrtKeyParameters key = (RsaPrivateCrtKeyParameters) this.key;
                BigInteger publicExponent = key.PublicExponent;
                if (publicExponent != null)
                {
                    BigInteger modulus = key.Modulus;
                    BigInteger integer5 = BigIntegers.CreateRandomInRange(BigInteger.One, modulus.Subtract(BigInteger.One), this.random);
                    BigInteger input = integer5.ModPow(publicExponent, modulus).Multiply(val).Mod(modulus);
                    BigInteger integer7 = this.core.ProcessBlock(input);
                    BigInteger integer8 = integer5.ModInverse(modulus);
                    integer2 = integer7.Multiply(integer8).Mod(modulus);
                    if (!val.Equals(integer2.ModPow(publicExponent, modulus)))
                    {
                        throw new InvalidOperationException("RSA engine faulty decryption/signing detected");
                    }
                }
                else
                {
                    integer2 = this.core.ProcessBlock(val);
                }
            }
            else
            {
                integer2 = this.core.ProcessBlock(val);
            }
            return this.core.ConvertOutput(integer2);
        }

        public virtual string AlgorithmName =>
            "RSA";
    }
}

