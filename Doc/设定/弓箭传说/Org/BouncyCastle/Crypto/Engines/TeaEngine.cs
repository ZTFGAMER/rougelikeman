namespace Org.BouncyCastle.Crypto.Engines
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Crypto.Utilities;
    using Org.BouncyCastle.Utilities;
    using System;

    public class TeaEngine : IBlockCipher
    {
        private const int rounds = 0x20;
        private const int block_size = 8;
        private const uint delta = 0x9e3779b9;
        private const uint d_sum = 0xc6ef3720;
        private uint _a;
        private uint _b;
        private uint _c;
        private uint _d;
        private bool _initialised = false;
        private bool _forEncryption;

        private int decryptBlock(byte[] inBytes, int inOff, byte[] outBytes, int outOff)
        {
            uint n = Pack.BE_To_UInt32(inBytes, inOff);
            uint num2 = Pack.BE_To_UInt32(inBytes, inOff + 4);
            uint num3 = 0xc6ef3720;
            for (int i = 0; i != 0x20; i++)
            {
                num2 -= (((n << 4) + this._c) ^ (n + num3)) ^ ((n >> 5) + this._d);
                n -= (((num2 << 4) + this._a) ^ (num2 + num3)) ^ ((num2 >> 5) + this._b);
                num3 -= 0x9e3779b9;
            }
            Pack.UInt32_To_BE(n, outBytes, outOff);
            Pack.UInt32_To_BE(num2, outBytes, outOff + 4);
            return 8;
        }

        private int encryptBlock(byte[] inBytes, int inOff, byte[] outBytes, int outOff)
        {
            uint n = Pack.BE_To_UInt32(inBytes, inOff);
            uint num2 = Pack.BE_To_UInt32(inBytes, inOff + 4);
            uint num3 = 0;
            for (int i = 0; i != 0x20; i++)
            {
                num3 += 0x9e3779b9;
                n += (((num2 << 4) + this._a) ^ (num2 + num3)) ^ ((num2 >> 5) + this._b);
                num2 += (((n << 4) + this._c) ^ (n + num3)) ^ ((n >> 5) + this._d);
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
            this._a = Pack.BE_To_UInt32(key, 0);
            this._b = Pack.BE_To_UInt32(key, 4);
            this._c = Pack.BE_To_UInt32(key, 8);
            this._d = Pack.BE_To_UInt32(key, 12);
        }

        public virtual string AlgorithmName =>
            "TEA";

        public virtual bool IsPartialBlockOkay =>
            false;
    }
}

