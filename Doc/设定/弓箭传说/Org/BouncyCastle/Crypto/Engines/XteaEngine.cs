namespace Org.BouncyCastle.Crypto.Engines
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Crypto.Utilities;
    using Org.BouncyCastle.Utilities;
    using System;

    public class XteaEngine : IBlockCipher
    {
        private const int rounds = 0x20;
        private const int block_size = 8;
        private const int delta = -1640531527;
        private uint[] _S = new uint[4];
        private uint[] _sum0 = new uint[0x20];
        private uint[] _sum1 = new uint[0x20];
        private bool _initialised = false;
        private bool _forEncryption;

        private int decryptBlock(byte[] inBytes, int inOff, byte[] outBytes, int outOff)
        {
            uint n = Pack.BE_To_UInt32(inBytes, inOff);
            uint num2 = Pack.BE_To_UInt32(inBytes, inOff + 4);
            for (int i = 0x1f; i >= 0; i--)
            {
                num2 -= (((n << 4) ^ (n >> 5)) + n) ^ this._sum1[i];
                n -= (((num2 << 4) ^ (num2 >> 5)) + num2) ^ this._sum0[i];
            }
            Pack.UInt32_To_BE(n, outBytes, outOff);
            Pack.UInt32_To_BE(num2, outBytes, outOff + 4);
            return 8;
        }

        private int encryptBlock(byte[] inBytes, int inOff, byte[] outBytes, int outOff)
        {
            uint n = Pack.BE_To_UInt32(inBytes, inOff);
            uint num2 = Pack.BE_To_UInt32(inBytes, inOff + 4);
            for (int i = 0; i < 0x20; i++)
            {
                n += (((num2 << 4) ^ (num2 >> 5)) + num2) ^ this._sum0[i];
                num2 += (((n << 4) ^ (n >> 5)) + n) ^ this._sum1[i];
            }
            Pack.UInt32_To_BE(n, outBytes, outOff);
            Pack.UInt32_To_BE(num2, outBytes, outOff + 4);
            return 8;
        }

        public virtual int GetBlockSize() => 
            8;

        public virtual void Init(bool forEncryption, ICipherParameters parameters)
        {
            if (!(parameters is KeyParameter))
            {
                throw new ArgumentException("invalid parameter passed to TEA init - " + Platform.GetTypeName(parameters));
            }
            this._forEncryption = forEncryption;
            this._initialised = true;
            this.setKey(((KeyParameter) parameters).GetKey());
        }

        public virtual int ProcessBlock(byte[] inBytes, int inOff, byte[] outBytes, int outOff)
        {
            if (!this._initialised)
            {
                throw new InvalidOperationException(this.AlgorithmName + " not initialised");
            }
            Check.DataLength(inBytes, inOff, 8, "input buffer too short");
            Check.OutputLength(outBytes, outOff, 8, "output buffer too short");
            return (!this._forEncryption ? this.decryptBlock(inBytes, inOff, outBytes, outOff) : this.encryptBlock(inBytes, inOff, outBytes, outOff));
        }

        public virtual void Reset()
        {
        }

        private void setKey(byte[] key)
        {
            int num2;
            int index = num2 = 0;
            while (index < 4)
            {
                this._S[index] = Pack.BE_To_UInt32(key, num2);
                index++;
                num2 += 4;
            }
            for (index = num2 = 0; index < 0x20; index++)
            {
                this._sum0[index] = ((uint) num2) + this._S[num2 & 3];
                num2 += -1640531527;
                this._sum1[index] = ((uint) num2) + this._S[(num2 >> 11) & 3];
            }
        }

        public virtual string AlgorithmName =>
            "XTEA";

        public virtual bool IsPartialBlockOkay =>
            false;
    }
}

