namespace Org.BouncyCastle.Crypto.Engines
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Crypto.Utilities;
    using System;

    public class Salsa20Engine : IStreamCipher
    {
        public static readonly int DEFAULT_ROUNDS = 20;
        private const int StateSize = 0x10;
        private static readonly uint[] TAU_SIGMA = Pack.LE_To_UInt32(Strings.ToAsciiByteArray("expand 16-byte kexpand 32-byte k"), 0, 8);
        [Obsolete]
        protected static readonly byte[] sigma = Strings.ToAsciiByteArray("expand 32-byte k");
        [Obsolete]
        protected static readonly byte[] tau = Strings.ToAsciiByteArray("expand 16-byte k");
        protected int rounds;
        private int index;
        internal uint[] engineState;
        internal uint[] x;
        private byte[] keyStream;
        private bool initialised;
        private uint cW0;
        private uint cW1;
        private uint cW2;

        public Salsa20Engine() : this(DEFAULT_ROUNDS)
        {
        }

        public Salsa20Engine(int rounds)
        {
            this.engineState = new uint[0x10];
            this.x = new uint[0x10];
            this.keyStream = new byte[0x40];
            if ((rounds <= 0) || ((rounds & 1) != 0))
            {
                throw new ArgumentException("'rounds' must be a positive, even number");
            }
            this.rounds = rounds;
        }

        protected virtual void AdvanceCounter()
        {
            if (++this.engineState[8] == 0)
            {
                this.engineState[9]++;
            }
        }

        protected virtual void GenerateKeyStream(byte[] output)
        {
            SalsaCore(this.rounds, this.engineState, this.x);
            Pack.UInt32_To_LE(this.x, output, 0);
        }

        public virtual void Init(bool forEncryption, ICipherParameters parameters)
        {
            ParametersWithIV hiv = parameters as ParametersWithIV;
            if (hiv == null)
            {
                throw new ArgumentException(this.AlgorithmName + " Init requires an IV", "parameters");
            }
            byte[] iV = hiv.GetIV();
            if ((iV == null) || (iV.Length != this.NonceSize))
            {
                object[] objArray1 = new object[] { this.AlgorithmName, " requires exactly ", this.NonceSize, " bytes of IV" };
                throw new ArgumentException(string.Concat(objArray1));
            }
            ICipherParameters parameters2 = hiv.Parameters;
            if (parameters2 == null)
            {
                if (!this.initialised)
                {
                    throw new InvalidOperationException(this.AlgorithmName + " KeyParameter can not be null for first initialisation");
                }
                this.SetKey(null, iV);
            }
            else
            {
                if (!(parameters2 is KeyParameter))
                {
                    throw new ArgumentException(this.AlgorithmName + " Init parameters must contain a KeyParameter (or null for re-init)");
                }
                this.SetKey(((KeyParameter) parameters2).GetKey(), iV);
            }
            this.Reset();
            this.initialised = true;
        }

        private bool LimitExceeded() => 
            (((++this.cW0 == 0) && (++this.cW1 == 0)) && ((++this.cW2 & 0x20) != 0));

        private bool LimitExceeded(uint len)
        {
            uint num = this.cW0;
            this.cW0 += len;
            return (((this.cW0 < num) && (++this.cW1 == 0)) && ((++this.cW2 & 0x20) != 0));
        }

        internal void PackTauOrSigma(int keyLength, uint[] state, int stateOffset)
        {
            int index = (keyLength - 0x10) / 4;
            state[stateOffset] = TAU_SIGMA[index];
            state[stateOffset + 1] = TAU_SIGMA[index + 1];
            state[stateOffset + 2] = TAU_SIGMA[index + 2];
            state[stateOffset + 3] = TAU_SIGMA[index + 3];
        }

        public virtual void ProcessBytes(byte[] inBytes, int inOff, int len, byte[] outBytes, int outOff)
        {
            if (!this.initialised)
            {
                throw new InvalidOperationException(this.AlgorithmName + " not initialised");
            }
            Check.DataLength(inBytes, inOff, len, "input buffer too short");
            Check.OutputLength(outBytes, outOff, len, "output buffer too short");
            if (this.LimitExceeded((uint) len))
            {
                throw new MaxBytesExceededException("2^70 byte limit per IV would be exceeded; Change IV");
            }
            for (int i = 0; i < len; i++)
            {
                if (this.index == 0)
                {
                    this.GenerateKeyStream(this.keyStream);
                    this.AdvanceCounter();
                }
                outBytes[i + outOff] = (byte) (this.keyStream[this.index] ^ inBytes[i + inOff]);
                this.index = (this.index + 1) & 0x3f;
            }
        }

        internal static uint R(uint x, int y) => 
            ((x << y) | (x >> (0x20 - y)));

        public virtual void Reset()
        {
            this.index = 0;
            this.ResetLimitCounter();
            this.ResetCounter();
        }

        protected virtual void ResetCounter()
        {
            this.engineState[8] = this.engineState[9] = 0;
        }

        private void ResetLimitCounter()
        {
            this.cW0 = 0;
            this.cW1 = 0;
            this.cW2 = 0;
        }

        public virtual byte ReturnByte(byte input)
        {
            if (this.LimitExceeded())
            {
                throw new MaxBytesExceededException("2^70 byte limit per IV; Change IV");
            }
            if (this.index == 0)
            {
                this.GenerateKeyStream(this.keyStream);
                this.AdvanceCounter();
            }
            byte num = (byte) (this.keyStream[this.index] ^ input);
            this.index = (this.index + 1) & 0x3f;
            return num;
        }

        internal static void SalsaCore(int rounds, uint[] input, uint[] x)
        {
            if (input.Length != 0x10)
            {
                throw new ArgumentException();
            }
            if (x.Length != 0x10)
            {
                throw new ArgumentException();
            }
            if ((rounds % 2) != 0)
            {
                throw new ArgumentException("Number of rounds must be even");
            }
            uint num = input[0];
            uint num2 = input[1];
            uint num3 = input[2];
            uint num4 = input[3];
            uint num5 = input[4];
            uint num6 = input[5];
            uint num7 = input[6];
            uint num8 = input[7];
            uint num9 = input[8];
            uint num10 = input[9];
            uint num11 = input[10];
            uint num12 = input[11];
            uint num13 = input[12];
            uint num14 = input[13];
            uint num15 = input[14];
            uint num16 = input[15];
            for (int i = rounds; i > 0; i -= 2)
            {
                num5 ^= R(num + num13, 7);
                num9 ^= R(num5 + num, 9);
                num13 ^= R(num9 + num5, 13);
                num ^= R(num13 + num9, 0x12);
                num10 ^= R(num6 + num2, 7);
                num14 ^= R(num10 + num6, 9);
                num2 ^= R(num14 + num10, 13);
                num6 ^= R(num2 + num14, 0x12);
                num15 ^= R(num11 + num7, 7);
                num3 ^= R(num15 + num11, 9);
                num7 ^= R(num3 + num15, 13);
                num11 ^= R(num7 + num3, 0x12);
                num4 ^= R(num16 + num12, 7);
                num8 ^= R(num4 + num16, 9);
                num12 ^= R(num8 + num4, 13);
                num16 ^= R(num12 + num8, 0x12);
                num2 ^= R(num + num4, 7);
                num3 ^= R(num2 + num, 9);
                num4 ^= R(num3 + num2, 13);
                num ^= R(num4 + num3, 0x12);
                num7 ^= R(num6 + num5, 7);
                num8 ^= R(num7 + num6, 9);
                num5 ^= R(num8 + num7, 13);
                num6 ^= R(num5 + num8, 0x12);
                num12 ^= R(num11 + num10, 7);
                num9 ^= R(num12 + num11, 9);
                num10 ^= R(num9 + num12, 13);
                num11 ^= R(num10 + num9, 0x12);
                num13 ^= R(num16 + num15, 7);
                num14 ^= R(num13 + num16, 9);
                num15 ^= R(num14 + num13, 13);
                num16 ^= R(num15 + num14, 0x12);
            }
            x[0] = num + input[0];
            x[1] = num2 + input[1];
            x[2] = num3 + input[2];
            x[3] = num4 + input[3];
            x[4] = num5 + input[4];
            x[5] = num6 + input[5];
            x[6] = num7 + input[6];
            x[7] = num8 + input[7];
            x[8] = num9 + input[8];
            x[9] = num10 + input[9];
            x[10] = num11 + input[10];
            x[11] = num12 + input[11];
            x[12] = num13 + input[12];
            x[13] = num14 + input[13];
            x[14] = num15 + input[14];
            x[15] = num16 + input[15];
        }

        protected virtual void SetKey(byte[] keyBytes, byte[] ivBytes)
        {
            if (keyBytes != null)
            {
                if ((keyBytes.Length != 0x10) && (keyBytes.Length != 0x20))
                {
                    throw new ArgumentException(this.AlgorithmName + " requires 128 bit or 256 bit key");
                }
                int index = (keyBytes.Length - 0x10) / 4;
                this.engineState[0] = TAU_SIGMA[index];
                this.engineState[5] = TAU_SIGMA[index + 1];
                this.engineState[10] = TAU_SIGMA[index + 2];
                this.engineState[15] = TAU_SIGMA[index + 3];
                Pack.LE_To_UInt32(keyBytes, 0, this.engineState, 1, 4);
                Pack.LE_To_UInt32(keyBytes, keyBytes.Length - 0x10, this.engineState, 11, 4);
            }
            Pack.LE_To_UInt32(ivBytes, 0, this.engineState, 6, 2);
        }

        protected virtual int NonceSize =>
            8;

        public virtual string AlgorithmName
        {
            get
            {
                string str = "Salsa20";
                if (this.rounds != DEFAULT_ROUNDS)
                {
                    str = str + "/" + this.rounds;
                }
                return str;
            }
        }
    }
}

