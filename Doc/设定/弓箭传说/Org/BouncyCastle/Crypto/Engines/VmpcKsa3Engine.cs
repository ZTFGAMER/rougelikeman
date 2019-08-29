namespace Org.BouncyCastle.Crypto.Engines
{
    using System;

    public class VmpcKsa3Engine : VmpcEngine
    {
        protected override void InitKey(byte[] keyBytes, byte[] ivBytes)
        {
            base.s = 0;
            base.P = new byte[0x100];
            for (int i = 0; i < 0x100; i++)
            {
                base.P[i] = (byte) i;
            }
            for (int j = 0; j < 0x300; j++)
            {
                base.s = base.P[((base.s + base.P[j & 0xff]) + keyBytes[j % keyBytes.Length]) & 0xff];
                byte num3 = base.P[j & 0xff];
                base.P[j & 0xff] = base.P[base.s & 0xff];
                base.P[base.s & 0xff] = num3;
            }
            for (int k = 0; k < 0x300; k++)
            {
                base.s = base.P[((base.s + base.P[k & 0xff]) + ivBytes[k % ivBytes.Length]) & 0xff];
                byte num5 = base.P[k & 0xff];
                base.P[k & 0xff] = base.P[base.s & 0xff];
                base.P[base.s & 0xff] = num5;
            }
            for (int m = 0; m < 0x300; m++)
            {
                base.s = base.P[((base.s + base.P[m & 0xff]) + keyBytes[m % keyBytes.Length]) & 0xff];
                byte num7 = base.P[m & 0xff];
                base.P[m & 0xff] = base.P[base.s & 0xff];
                base.P[base.s & 0xff] = num7;
            }
            base.n = 0;
        }

        public override string AlgorithmName =>
            "VMPC-KSA3";
    }
}

