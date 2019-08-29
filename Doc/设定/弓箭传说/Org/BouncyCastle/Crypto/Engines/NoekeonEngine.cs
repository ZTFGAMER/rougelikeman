namespace Org.BouncyCastle.Crypto.Engines
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Crypto.Utilities;
    using Org.BouncyCastle.Utilities;
    using System;

    public class NoekeonEngine : IBlockCipher
    {
        private const int GenericSize = 0x10;
        private static readonly uint[] nullVector = new uint[4];
        private static readonly uint[] roundConstants = new uint[] { 
            0x80, 0x1b, 0x36, 0x6c, 0xd8, 0xab, 0x4d, 0x9a, 0x2f, 0x5e, 0xbc, 0x63, 0xc6, 0x97, 0x35, 0x6a,
            0xd4
        };
        private uint[] state = new uint[4];
        private uint[] subKeys = new uint[4];
        private uint[] decryptKeys = new uint[4];
        private bool _initialised = false;
        private bool _forEncryption;

        private int decryptBlock(byte[] input, int inOff, byte[] output, int outOff)
        {
            this.state[0] = Pack.BE_To_UInt32(input, inOff);
            this.state[1] = Pack.BE_To_UInt32(input, inOff + 4);
            this.state[2] = Pack.BE_To_UInt32(input, inOff + 8);
            this.state[3] = Pack.BE_To_UInt32(input, inOff + 12);
            Array.Copy(this.subKeys, 0, this.decryptKeys, 0, this.subKeys.Length);
            this.theta(this.decryptKeys, nullVector);
            int index = 0x10;
            while (index > 0)
            {
                this.theta(this.state, this.decryptKeys);
                this.state[0] ^= roundConstants[index];
                this.pi1(this.state);
                this.gamma(this.state);
                this.pi2(this.state);
                index--;
            }
            this.theta(this.state, this.decryptKeys);
            this.state[0] ^= roundConstants[index];
            Pack.UInt32_To_BE(this.state[0], output, outOff);
            Pack.UInt32_To_BE(this.state[1], output, outOff + 4);
            Pack.UInt32_To_BE(this.state[2], output, outOff + 8);
            Pack.UInt32_To_BE(this.state[3], output, outOff + 12);
            return 0x10;
        }

        private int encryptBlock(byte[] input, int inOff, byte[] output, int outOff)
        {
            this.state[0] = Pack.BE_To_UInt32(input, inOff);
            this.state[1] = Pack.BE_To_UInt32(input, inOff + 4);
            this.state[2] = Pack.BE_To_UInt32(input, inOff + 8);
            this.state[3] = Pack.BE_To_UInt32(input, inOff + 12);
            int index = 0;
            while (index < 0x10)
            {
                this.state[0] ^= roundConstants[index];
                this.theta(this.state, this.subKeys);
                this.pi1(this.state);
                this.gamma(this.state);
                this.pi2(this.state);
                index++;
            }
            this.state[0] ^= roundConstants[index];
            this.theta(this.state, this.subKeys);
            Pack.UInt32_To_BE(this.state[0], output, outOff);
            Pack.UInt32_To_BE(this.state[1], output, outOff + 4);
            Pack.UInt32_To_BE(this.state[2], output, outOff + 8);
            Pack.UInt32_To_BE(this.state[3], output, outOff + 12);
            return 0x10;
        }

        private void gamma(uint[] a)
        {
            a[1] ^= ~a[3] & ~a[2];
            a[0] ^= a[2] & a[1];
            uint num = a[3];
            a[3] = a[0];
            a[0] = num;
            a[2] ^= (a[0] ^ a[1]) ^ a[3];
            a[1] ^= ~a[3] & ~a[2];
            a[0] ^= a[2] & a[1];
        }

        public virtual int GetBlockSize() => 
            0x10;

        public virtual void Init(bool forEncryption, ICipherParameters parameters)
        {
            if (!(parameters is KeyParameter))
            {
                throw new ArgumentException("Invalid parameters passed to Noekeon init - " + Platform.GetTypeName(parameters), "parameters");
            }
            this._forEncryption = forEncryption;
            this._initialised = true;
            this.setKey(((KeyParameter) parameters).GetKey());
        }

        private void pi1(uint[] a)
        {
            a[1] = this.rotl(a[1], 1);
            a[2] = this.rotl(a[2], 5);
            a[3] = this.rotl(a[3], 2);
        }

        private void pi2(uint[] a)
        {
            a[1] = this.rotl(a[1], 0x1f);
            a[2] = this.rotl(a[2], 0x1b);
            a[3] = this.rotl(a[3], 30);
        }

        public virtual int ProcessBlock(byte[] input, int inOff, byte[] output, int outOff)
        {
            if (!this._initialised)
            {
                throw new InvalidOperationException(this.AlgorithmName + " not initialised");
            }
            Check.DataLength(input, inOff, 0x10, "input buffer too short");
            Check.OutputLength(output, outOff, 0x10, "output buffer too short");
            return (!this._forEncryption ? this.decryptBlock(input, inOff, output, outOff) : this.encryptBlock(input, inOff, output, outOff));
        }

        public virtual void Reset()
        {
        }

        private uint rotl(uint x, int y) => 
            ((x << y) | (x >> (0x20 - y)));

        private void setKey(byte[] key)
        {
            this.subKeys[0] = Pack.BE_To_UInt32(key, 0);
            this.subKeys[1] = Pack.BE_To_UInt32(key, 4);
            this.subKeys[2] = Pack.BE_To_UInt32(key, 8);
            this.subKeys[3] = Pack.BE_To_UInt32(key, 12);
        }

        private void theta(uint[] a, uint[] k)
        {
            uint x = a[0] ^ a[2];
            x ^= this.rotl(x, 8) ^ this.rotl(x, 0x18);
            a[1] ^= x;
            a[3] ^= x;
            for (int i = 0; i < 4; i++)
            {
                a[i] ^= k[i];
            }
            x = a[1] ^ a[3];
            x ^= this.rotl(x, 8) ^ this.rotl(x, 0x18);
            a[0] ^= x;
            a[2] ^= x;
        }

        public virtual string AlgorithmName =>
            "Noekeon";

        public virtual bool IsPartialBlockOkay =>
            false;
    }
}

