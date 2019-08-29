namespace Org.BouncyCastle.Crypto.Digests
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Engines;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Crypto.Utilities;
    using Org.BouncyCastle.Utilities;
    using System;

    public class Gost3411Digest : IDigest, IMemoable
    {
        private const int DIGEST_LENGTH = 0x20;
        private byte[] H;
        private byte[] L;
        private byte[] M;
        private byte[] Sum;
        private byte[][] C;
        private byte[] xBuf;
        private int xBufOff;
        private ulong byteCount;
        private readonly IBlockCipher cipher;
        private byte[] sBox;
        private byte[] K;
        private byte[] a;
        internal short[] wS;
        internal short[] w_S;
        internal byte[] S;
        internal byte[] U;
        internal byte[] V;
        internal byte[] W;
        private static readonly byte[] C2 = new byte[] { 
            0, 0xff, 0, 0xff, 0, 0xff, 0, 0xff, 0xff, 0, 0xff, 0, 0xff, 0, 0xff, 0,
            0, 0xff, 0xff, 0, 0xff, 0, 0, 0xff, 0xff, 0, 0, 0, 0xff, 0xff, 0, 0xff
        };

        public Gost3411Digest()
        {
            this.H = new byte[0x20];
            this.L = new byte[0x20];
            this.M = new byte[0x20];
            this.Sum = new byte[0x20];
            this.C = MakeC();
            this.xBuf = new byte[0x20];
            this.cipher = new Gost28147Engine();
            this.K = new byte[0x20];
            this.a = new byte[8];
            this.wS = new short[0x10];
            this.w_S = new short[0x10];
            this.S = new byte[0x20];
            this.U = new byte[0x20];
            this.V = new byte[0x20];
            this.W = new byte[0x20];
            this.sBox = Gost28147Engine.GetSBox("D-A");
            this.cipher.Init(true, new ParametersWithSBox(null, this.sBox));
            this.Reset();
        }

        public Gost3411Digest(byte[] sBoxParam)
        {
            this.H = new byte[0x20];
            this.L = new byte[0x20];
            this.M = new byte[0x20];
            this.Sum = new byte[0x20];
            this.C = MakeC();
            this.xBuf = new byte[0x20];
            this.cipher = new Gost28147Engine();
            this.K = new byte[0x20];
            this.a = new byte[8];
            this.wS = new short[0x10];
            this.w_S = new short[0x10];
            this.S = new byte[0x20];
            this.U = new byte[0x20];
            this.V = new byte[0x20];
            this.W = new byte[0x20];
            this.sBox = Arrays.Clone(sBoxParam);
            this.cipher.Init(true, new ParametersWithSBox(null, this.sBox));
            this.Reset();
        }

        public Gost3411Digest(Gost3411Digest t)
        {
            this.H = new byte[0x20];
            this.L = new byte[0x20];
            this.M = new byte[0x20];
            this.Sum = new byte[0x20];
            this.C = MakeC();
            this.xBuf = new byte[0x20];
            this.cipher = new Gost28147Engine();
            this.K = new byte[0x20];
            this.a = new byte[8];
            this.wS = new short[0x10];
            this.w_S = new short[0x10];
            this.S = new byte[0x20];
            this.U = new byte[0x20];
            this.V = new byte[0x20];
            this.W = new byte[0x20];
            this.Reset(t);
        }

        private byte[] A(byte[] input)
        {
            for (int i = 0; i < 8; i++)
            {
                this.a[i] = (byte) (input[i] ^ input[i + 8]);
            }
            Array.Copy(input, 8, input, 0, 0x18);
            Array.Copy(this.a, 0, input, 0x18, 8);
            return input;
        }

        public void BlockUpdate(byte[] input, int inOff, int length)
        {
            while ((this.xBufOff != 0) && (length > 0))
            {
                this.Update(input[inOff]);
                inOff++;
                length--;
            }
            while (length > this.xBuf.Length)
            {
                Array.Copy(input, inOff, this.xBuf, 0, this.xBuf.Length);
                this.sumByteArray(this.xBuf);
                this.processBlock(this.xBuf, 0);
                inOff += this.xBuf.Length;
                length -= this.xBuf.Length;
                this.byteCount += (ulong) this.xBuf.Length;
            }
            while (length > 0)
            {
                this.Update(input[inOff]);
                inOff++;
                length--;
            }
        }

        public IMemoable Copy() => 
            new Gost3411Digest(this);

        private static void cpyBytesToShort(byte[] S, short[] wS)
        {
            for (int i = 0; i < (S.Length / 2); i++)
            {
                wS[i] = (short) (((S[(i * 2) + 1] << 8) & 0xff00) | (S[i * 2] & 0xff));
            }
        }

        private static void cpyShortToBytes(short[] wS, byte[] S)
        {
            for (int i = 0; i < (S.Length / 2); i++)
            {
                S[(i * 2) + 1] = (byte) (wS[i] >> 8);
                S[i * 2] = (byte) wS[i];
            }
        }

        public int DoFinal(byte[] output, int outOff)
        {
            this.finish();
            this.H.CopyTo(output, outOff);
            this.Reset();
            return 0x20;
        }

        private void E(byte[] key, byte[] s, int sOff, byte[] input, int inOff)
        {
            this.cipher.Init(true, new KeyParameter(key));
            this.cipher.ProcessBlock(input, inOff, s, sOff);
        }

        private void finish()
        {
            ulong n = this.byteCount * ((ulong) 8L);
            Pack.UInt64_To_LE(n, this.L);
            while (this.xBufOff != 0)
            {
                this.Update(0);
            }
            this.processBlock(this.L, 0);
            this.processBlock(this.Sum, 0);
        }

        private void fw(byte[] input)
        {
            cpyBytesToShort(input, this.wS);
            this.w_S[15] = (short) (((((this.wS[0] ^ this.wS[1]) ^ this.wS[2]) ^ this.wS[3]) ^ this.wS[12]) ^ this.wS[15]);
            Array.Copy(this.wS, 1, this.w_S, 0, 15);
            cpyShortToBytes(this.w_S, input);
        }

        public int GetByteLength() => 
            0x20;

        public int GetDigestSize() => 
            0x20;

        private static byte[][] MakeC()
        {
            byte[][] bufferArray = new byte[4][];
            for (int i = 0; i < 4; i++)
            {
                bufferArray[i] = new byte[0x20];
            }
            return bufferArray;
        }

        private byte[] P(byte[] input)
        {
            int num = 0;
            for (int i = 0; i < 8; i++)
            {
                this.K[num++] = input[i];
                this.K[num++] = input[8 + i];
                this.K[num++] = input[0x10 + i];
                this.K[num++] = input[0x18 + i];
            }
            return this.K;
        }

        private void processBlock(byte[] input, int inOff)
        {
            Array.Copy(input, inOff, this.M, 0, 0x20);
            this.H.CopyTo(this.U, 0);
            this.M.CopyTo(this.V, 0);
            for (int i = 0; i < 0x20; i++)
            {
                this.W[i] = (byte) (this.U[i] ^ this.V[i]);
            }
            this.E(this.P(this.W), this.S, 0, this.H, 0);
            for (int j = 1; j < 4; j++)
            {
                byte[] buffer = this.A(this.U);
                for (int num3 = 0; num3 < 0x20; num3++)
                {
                    this.U[num3] = (byte) (buffer[num3] ^ this.C[j][num3]);
                }
                this.V = this.A(this.A(this.V));
                for (int num4 = 0; num4 < 0x20; num4++)
                {
                    this.W[num4] = (byte) (this.U[num4] ^ this.V[num4]);
                }
                this.E(this.P(this.W), this.S, j * 8, this.H, j * 8);
            }
            for (int k = 0; k < 12; k++)
            {
                this.fw(this.S);
            }
            for (int m = 0; m < 0x20; m++)
            {
                this.S[m] = (byte) (this.S[m] ^ this.M[m]);
            }
            this.fw(this.S);
            for (int n = 0; n < 0x20; n++)
            {
                this.S[n] = (byte) (this.H[n] ^ this.S[n]);
            }
            for (int num8 = 0; num8 < 0x3d; num8++)
            {
                this.fw(this.S);
            }
            Array.Copy(this.S, 0, this.H, 0, this.H.Length);
        }

        public void Reset()
        {
            this.byteCount = 0L;
            this.xBufOff = 0;
            Array.Clear(this.H, 0, this.H.Length);
            Array.Clear(this.L, 0, this.L.Length);
            Array.Clear(this.M, 0, this.M.Length);
            Array.Clear(this.C[1], 0, this.C[1].Length);
            Array.Clear(this.C[3], 0, this.C[3].Length);
            Array.Clear(this.Sum, 0, this.Sum.Length);
            Array.Clear(this.xBuf, 0, this.xBuf.Length);
            C2.CopyTo(this.C[2], 0);
        }

        public void Reset(IMemoable other)
        {
            Gost3411Digest digest = (Gost3411Digest) other;
            this.sBox = digest.sBox;
            this.cipher.Init(true, new ParametersWithSBox(null, this.sBox));
            this.Reset();
            Array.Copy(digest.H, 0, this.H, 0, digest.H.Length);
            Array.Copy(digest.L, 0, this.L, 0, digest.L.Length);
            Array.Copy(digest.M, 0, this.M, 0, digest.M.Length);
            Array.Copy(digest.Sum, 0, this.Sum, 0, digest.Sum.Length);
            Array.Copy(digest.C[1], 0, this.C[1], 0, digest.C[1].Length);
            Array.Copy(digest.C[2], 0, this.C[2], 0, digest.C[2].Length);
            Array.Copy(digest.C[3], 0, this.C[3], 0, digest.C[3].Length);
            Array.Copy(digest.xBuf, 0, this.xBuf, 0, digest.xBuf.Length);
            this.xBufOff = digest.xBufOff;
            this.byteCount = digest.byteCount;
        }

        private void sumByteArray(byte[] input)
        {
            int num = 0;
            for (int i = 0; i != this.Sum.Length; i++)
            {
                int num3 = ((this.Sum[i] & 0xff) + (input[i] & 0xff)) + num;
                this.Sum[i] = (byte) num3;
                num = num3 >> 8;
            }
        }

        public void Update(byte input)
        {
            this.xBuf[this.xBufOff++] = input;
            if (this.xBufOff == this.xBuf.Length)
            {
                this.sumByteArray(this.xBuf);
                this.processBlock(this.xBuf, 0);
                this.xBufOff = 0;
            }
            this.byteCount += (ulong) 1L;
        }

        public string AlgorithmName =>
            "Gost3411";
    }
}

