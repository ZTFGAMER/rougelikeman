namespace Org.BouncyCastle.Crypto.Engines
{
    using Org.BouncyCastle.Crypto.Utilities;
    using System;

    public class ChaCha7539Engine : Salsa20Engine
    {
        protected override void AdvanceCounter()
        {
            if (++base.engineState[12] == 0)
            {
                throw new InvalidOperationException("attempt to increase counter past 2^32.");
            }
        }

        protected override void GenerateKeyStream(byte[] output)
        {
            ChaChaEngine.ChachaCore(base.rounds, base.engineState, base.x);
            Pack.UInt32_To_LE(base.x, output, 0);
        }

        protected override void ResetCounter()
        {
            base.engineState[12] = 0;
        }

        protected override void SetKey(byte[] keyBytes, byte[] ivBytes)
        {
            if (keyBytes != null)
            {
                if (keyBytes.Length != 0x20)
                {
                    throw new ArgumentException(this.AlgorithmName + " requires 256 bit key");
                }
                base.PackTauOrSigma(keyBytes.Length, base.engineState, 0);
                Pack.LE_To_UInt32(keyBytes, 0, base.engineState, 4, 8);
            }
            Pack.LE_To_UInt32(ivBytes, 0, base.engineState, 13, 3);
        }

        public override string AlgorithmName =>
            ("ChaCha" + base.rounds);

        protected override int NonceSize =>
            12;
    }
}

