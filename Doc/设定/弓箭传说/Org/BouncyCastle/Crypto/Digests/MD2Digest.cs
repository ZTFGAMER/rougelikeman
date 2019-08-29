namespace Org.BouncyCastle.Crypto.Digests
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Utilities;
    using System;

    public class MD2Digest : IDigest, IMemoable
    {
        private const int DigestLength = 0x10;
        private const int BYTE_LENGTH = 0x10;
        private byte[] X;
        private int xOff;
        private byte[] M;
        private int mOff;
        private byte[] C;
        private int COff;
        private static readonly byte[] S = new byte[] { 
            0x29, 0x2e, 0x43, 0xc9, 0xa2, 0xd8, 0x7c, 1, 0x3d, 0x36, 0x54, 0xa1, 0xec, 240, 6, 0x13,
            0x62, 0xa7, 5, 0xf3, 0xc0, 0xc7, 0x73, 140, 0x98, 0x93, 0x2b, 0xd9, 0xbc, 0x4c, 130, 0xca,
            30, 0x9b, 0x57, 60, 0xfd, 0xd4, 0xe0, 0x16, 0x67, 0x42, 0x6f, 0x18, 0x8a, 0x17, 0xe5, 0x12,
            190, 0x4e, 0xc4, 0xd6, 0xda, 0x9e, 0xde, 0x49, 160, 0xfb, 0xf5, 0x8e, 0xbb, 0x2f, 0xee, 0x7a,
            0xa9, 0x68, 0x79, 0x91, 0x15, 0xb2, 7, 0x3f, 0x94, 0xc2, 0x10, 0x89, 11, 0x22, 0x5f, 0x21,
            0x80, 0x7f, 0x5d, 0x9a, 90, 0x90, 50, 0x27, 0x35, 0x3e, 0xcc, 0xe7, 0xbf, 0xf7, 0x97, 3,
            0xff, 0x19, 0x30, 0xb3, 0x48, 0xa5, 0xb5, 0xd1, 0xd7, 0x5e, 0x92, 0x2a, 0xac, 0x56, 170, 0xc6,
            0x4f, 0xb8, 0x38, 210, 150, 0xa4, 0x7d, 0xb6, 0x76, 0xfc, 0x6b, 0xe2, 0x9c, 0x74, 4, 0xf1,
            0x45, 0x9d, 0x70, 0x59, 100, 0x71, 0x87, 0x20, 0x86, 0x5b, 0xcf, 0x65, 230, 0x2d, 0xa8, 2,
            0x1b, 0x60, 0x25, 0xad, 0xae, 0xb0, 0xb9, 0xf6, 0x1c, 70, 0x61, 0x69, 0x34, 0x40, 0x7e, 15,
            0x55, 0x47, 0xa3, 0x23, 0xdd, 0x51, 0xaf, 0x3a, 0xc3, 0x5c, 0xf9, 0xce, 0xba, 0xc5, 0xea, 0x26,
            0x2c, 0x53, 13, 110, 0x85, 40, 0x84, 9, 0xd3, 0xdf, 0xcd, 0xf4, 0x41, 0x81, 0x4d, 0x52,
            0x6a, 220, 0x37, 200, 0x6c, 0xc1, 0xab, 250, 0x24, 0xe1, 0x7b, 8, 12, 0xbd, 0xb1, 0x4a,
            120, 0x88, 0x95, 0x8b, 0xe3, 0x63, 0xe8, 0x6d, 0xe9, 0xcb, 0xd5, 0xfe, 0x3b, 0, 0x1d, 0x39,
            0xf2, 0xef, 0xb7, 14, 0x66, 0x58, 0xd0, 0xe4, 0xa6, 0x77, 0x72, 0xf8, 0xeb, 0x75, 0x4b, 10,
            0x31, 0x44, 80, 180, 0x8f, 0xed, 0x1f, 0x1a, 0xdb, 0x99, 0x8d, 0x33, 0x9f, 0x11, 0x83, 20
        };

        public MD2Digest()
        {
            this.X = new byte[0x30];
            this.M = new byte[0x10];
            this.C = new byte[0x10];
            this.Reset();
        }

        public MD2Digest(MD2Digest t)
        {
            this.X = new byte[0x30];
            this.M = new byte[0x10];
            this.C = new byte[0x10];
            this.CopyIn(t);
        }

        public void BlockUpdate(byte[] input, int inOff, int length)
        {
            while ((this.mOff != 0) && (length > 0))
            {
                this.Update(input[inOff]);
                inOff++;
                length--;
            }
            while (length > 0x10)
            {
                Array.Copy(input, inOff, this.M, 0, 0x10);
                this.ProcessChecksum(this.M);
                this.ProcessBlock(this.M);
                length -= 0x10;
                inOff += 0x10;
            }
            while (length > 0)
            {
                this.Update(input[inOff]);
                inOff++;
                length--;
            }
        }

        public IMemoable Copy() => 
            new MD2Digest(this);

        private void CopyIn(MD2Digest t)
        {
            Array.Copy(t.X, 0, this.X, 0, t.X.Length);
            this.xOff = t.xOff;
            Array.Copy(t.M, 0, this.M, 0, t.M.Length);
            this.mOff = t.mOff;
            Array.Copy(t.C, 0, this.C, 0, t.C.Length);
            this.COff = t.COff;
        }

        public int DoFinal(byte[] output, int outOff)
        {
            byte num = (byte) (this.M.Length - this.mOff);
            for (int i = this.mOff; i < this.M.Length; i++)
            {
                this.M[i] = num;
            }
            this.ProcessChecksum(this.M);
            this.ProcessBlock(this.M);
            this.ProcessBlock(this.C);
            Array.Copy(this.X, this.xOff, output, outOff, 0x10);
            this.Reset();
            return 0x10;
        }

        public int GetByteLength() => 
            0x10;

        public int GetDigestSize() => 
            0x10;

        internal void ProcessBlock(byte[] m)
        {
            for (int i = 0; i < 0x10; i++)
            {
                this.X[i + 0x10] = m[i];
                this.X[i + 0x20] = (byte) (m[i] ^ this.X[i]);
            }
            int index = 0;
            for (int j = 0; j < 0x12; j++)
            {
                for (int k = 0; k < 0x30; k++)
                {
                    index = this.X[k] = (byte) (this.X[k] ^ S[index]);
                    index &= 0xff;
                }
                index = (index + j) % 0x100;
            }
        }

        internal void ProcessChecksum(byte[] m)
        {
            int num = this.C[15];
            for (int i = 0; i < 0x10; i++)
            {
                this.C[i] = (byte) (this.C[i] ^ S[(m[i] ^ num) & 0xff]);
                num = this.C[i];
            }
        }

        public void Reset()
        {
            this.xOff = 0;
            for (int i = 0; i != this.X.Length; i++)
            {
                this.X[i] = 0;
            }
            this.mOff = 0;
            for (int j = 0; j != this.M.Length; j++)
            {
                this.M[j] = 0;
            }
            this.COff = 0;
            for (int k = 0; k != this.C.Length; k++)
            {
                this.C[k] = 0;
            }
        }

        public void Reset(IMemoable other)
        {
            MD2Digest t = (MD2Digest) other;
            this.CopyIn(t);
        }

        public void Update(byte input)
        {
            this.M[this.mOff++] = input;
            if (this.mOff == 0x10)
            {
                this.ProcessChecksum(this.M);
                this.ProcessBlock(this.M);
                this.mOff = 0;
            }
        }

        public string AlgorithmName =>
            "MD2";
    }
}

