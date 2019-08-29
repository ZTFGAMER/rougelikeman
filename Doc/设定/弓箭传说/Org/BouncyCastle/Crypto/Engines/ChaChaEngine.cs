namespace Org.BouncyCastle.Crypto.Engines
{
    using Org.BouncyCastle.Crypto.Utilities;
    using System;

    public class ChaChaEngine : Salsa20Engine
    {
        public ChaChaEngine()
        {
        }

        public ChaChaEngine(int rounds) : base(rounds)
        {
        }

        protected override void AdvanceCounter()
        {
            if (++base.engineState[12] == 0)
            {
                base.engineState[13]++;
            }
        }

        internal static void ChachaCore(int rounds, uint[] input, uint[] x)
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
                num += num5;
                num13 = Salsa20Engine.R(num13 ^ num, 0x10);
                num9 += num13;
                num5 = Salsa20Engine.R(num5 ^ num9, 12);
                num += num5;
                num13 = Salsa20Engine.R(num13 ^ num, 8);
                num9 += num13;
                num5 = Salsa20Engine.R(num5 ^ num9, 7);
                num2 += num6;
                num14 = Salsa20Engine.R(num14 ^ num2, 0x10);
                num10 += num14;
                num6 = Salsa20Engine.R(num6 ^ num10, 12);
                num2 += num6;
                num14 = Salsa20Engine.R(num14 ^ num2, 8);
                num10 += num14;
                num6 = Salsa20Engine.R(num6 ^ num10, 7);
                num3 += num7;
                num15 = Salsa20Engine.R(num15 ^ num3, 0x10);
                num11 += num15;
                num7 = Salsa20Engine.R(num7 ^ num11, 12);
                num3 += num7;
                num15 = Salsa20Engine.R(num15 ^ num3, 8);
                num11 += num15;
                num7 = Salsa20Engine.R(num7 ^ num11, 7);
                num4 += num8;
                num16 = Salsa20Engine.R(num16 ^ num4, 0x10);
                num12 += num16;
                num8 = Salsa20Engine.R(num8 ^ num12, 12);
                num4 += num8;
                num16 = Salsa20Engine.R(num16 ^ num4, 8);
                num12 += num16;
                num8 = Salsa20Engine.R(num8 ^ num12, 7);
                num += num6;
                num16 = Salsa20Engine.R(num16 ^ num, 0x10);
                num11 += num16;
                num6 = Salsa20Engine.R(num6 ^ num11, 12);
                num += num6;
                num16 = Salsa20Engine.R(num16 ^ num, 8);
                num11 += num16;
                num6 = Salsa20Engine.R(num6 ^ num11, 7);
                num2 += num7;
                num13 = Salsa20Engine.R(num13 ^ num2, 0x10);
                num12 += num13;
                num7 = Salsa20Engine.R(num7 ^ num12, 12);
                num2 += num7;
                num13 = Salsa20Engine.R(num13 ^ num2, 8);
                num12 += num13;
                num7 = Salsa20Engine.R(num7 ^ num12, 7);
                num3 += num8;
                num14 = Salsa20Engine.R(num14 ^ num3, 0x10);
                num9 += num14;
                num8 = Salsa20Engine.R(num8 ^ num9, 12);
                num3 += num8;
                num14 = Salsa20Engine.R(num14 ^ num3, 8);
                num9 += num14;
                num8 = Salsa20Engine.R(num8 ^ num9, 7);
                num4 += num5;
                num15 = Salsa20Engine.R(num15 ^ num4, 0x10);
                num10 += num15;
                num5 = Salsa20Engine.R(num5 ^ num10, 12);
                num4 += num5;
                num15 = Salsa20Engine.R(num15 ^ num4, 8);
                num10 += num15;
                num5 = Salsa20Engine.R(num5 ^ num10, 7);
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

        protected override void GenerateKeyStream(byte[] output)
        {
            ChachaCore(base.rounds, base.engineState, base.x);
            Pack.UInt32_To_LE(base.x, output, 0);
        }

        protected override void ResetCounter()
        {
            base.engineState[12] = base.engineState[13] = 0;
        }

        protected override void SetKey(byte[] keyBytes, byte[] ivBytes)
        {
            if (keyBytes != null)
            {
                if ((keyBytes.Length != 0x10) && (keyBytes.Length != 0x20))
                {
                    throw new ArgumentException(this.AlgorithmName + " requires 128 bit or 256 bit key");
                }
                base.PackTauOrSigma(keyBytes.Length, base.engineState, 0);
                Pack.LE_To_UInt32(keyBytes, 0, base.engineState, 4, 4);
                Pack.LE_To_UInt32(keyBytes, keyBytes.Length - 0x10, base.engineState, 8, 4);
            }
            Pack.LE_To_UInt32(ivBytes, 0, base.engineState, 14, 2);
        }

        public override string AlgorithmName =>
            ("ChaCha" + base.rounds);
    }
}

