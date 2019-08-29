namespace Org.BouncyCastle.Crypto.Modes.Gcm
{
    using Org.BouncyCastle.Crypto.Utilities;
    using Org.BouncyCastle.Utilities;
    using System;

    public class Tables8kGcmMultiplier : IGcmMultiplier
    {
        private byte[] H;
        private uint[][][] M;

        public void Init(byte[] H)
        {
            if (this.M == null)
            {
                this.M = new uint[0x20][][];
            }
            else if (Arrays.AreEqual(this.H, H))
            {
                return;
            }
            this.H = Arrays.Clone(H);
            this.M[0] = new uint[0x10][];
            this.M[1] = new uint[0x10][];
            this.M[0][0] = new uint[4];
            this.M[1][0] = new uint[4];
            this.M[1][8] = GcmUtilities.AsUints(H);
            for (int i = 4; i >= 1; i = i >> 1)
            {
                uint[] numArray = (uint[]) this.M[1][i + i].Clone();
                GcmUtilities.MultiplyP(numArray);
                this.M[1][i] = numArray;
            }
            uint[] x = (uint[]) this.M[1][1].Clone();
            GcmUtilities.MultiplyP(x);
            this.M[0][8] = x;
            for (int j = 4; j >= 1; j = j >> 1)
            {
                uint[] numArray3 = (uint[]) this.M[0][j + j].Clone();
                GcmUtilities.MultiplyP(numArray3);
                this.M[0][j] = numArray3;
            }
            int index = 0;
            while (true)
            {
                for (int k = 2; k < 0x10; k += k)
                {
                    for (int m = 1; m < k; m++)
                    {
                        uint[] numArray4 = (uint[]) this.M[index][k].Clone();
                        GcmUtilities.Xor(numArray4, this.M[index][m]);
                        this.M[index][k + m] = numArray4;
                    }
                }
                if (++index == 0x20)
                {
                    return;
                }
                if (index > 1)
                {
                    this.M[index] = new uint[0x10][];
                    this.M[index][0] = new uint[4];
                    for (int m = 8; m > 0; m = m >> 1)
                    {
                        uint[] numArray5 = (uint[]) this.M[index - 2][m].Clone();
                        GcmUtilities.MultiplyP8(numArray5);
                        this.M[index][m] = numArray5;
                    }
                }
            }
        }

        public void MultiplyH(byte[] x)
        {
            uint[] ns = new uint[4];
            for (int i = 15; i >= 0; i--)
            {
                uint[] numArray2 = this.M[i + i][x[i] & 15];
                ns[0] ^= numArray2[0];
                ns[1] ^= numArray2[1];
                ns[2] ^= numArray2[2];
                ns[3] ^= numArray2[3];
                numArray2 = this.M[(i + i) + 1][(x[i] & 240) >> 4];
                ns[0] ^= numArray2[0];
                ns[1] ^= numArray2[1];
                ns[2] ^= numArray2[2];
                ns[3] ^= numArray2[3];
            }
            Pack.UInt32_To_BE(ns, x, 0);
        }
    }
}

