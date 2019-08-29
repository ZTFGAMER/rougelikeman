namespace Org.BouncyCastle.Crypto.Engines
{
    using Org.BouncyCastle.Crypto.Utilities;
    using System;

    public sealed class Cast6Engine : Cast5Engine
    {
        private const int ROUNDS = 12;
        private const int BLOCK_SIZE = 0x10;
        private int[] _Kr = new int[0x30];
        private uint[] _Km = new uint[0x30];
        private int[] _Tr = new int[0xc0];
        private uint[] _Tm = new uint[0xc0];
        private uint[] _workingKey = new uint[8];

        private void CAST_Decipher(uint A, uint B, uint C, uint D, uint[] result)
        {
            for (int i = 0; i < 6; i++)
            {
                int index = (11 - i) * 4;
                C ^= Cast5Engine.F1(D, this._Km[index], this._Kr[index]);
                B ^= Cast5Engine.F2(C, this._Km[index + 1], this._Kr[index + 1]);
                A ^= Cast5Engine.F3(B, this._Km[index + 2], this._Kr[index + 2]);
                D ^= Cast5Engine.F1(A, this._Km[index + 3], this._Kr[index + 3]);
            }
            for (int j = 6; j < 12; j++)
            {
                int index = (11 - j) * 4;
                D ^= Cast5Engine.F1(A, this._Km[index + 3], this._Kr[index + 3]);
                A ^= Cast5Engine.F3(B, this._Km[index + 2], this._Kr[index + 2]);
                B ^= Cast5Engine.F2(C, this._Km[index + 1], this._Kr[index + 1]);
                C ^= Cast5Engine.F1(D, this._Km[index], this._Kr[index]);
            }
            result[0] = A;
            result[1] = B;
            result[2] = C;
            result[3] = D;
        }

        private void CAST_Encipher(uint A, uint B, uint C, uint D, uint[] result)
        {
            for (int i = 0; i < 6; i++)
            {
                int index = i * 4;
                C ^= Cast5Engine.F1(D, this._Km[index], this._Kr[index]);
                B ^= Cast5Engine.F2(C, this._Km[index + 1], this._Kr[index + 1]);
                A ^= Cast5Engine.F3(B, this._Km[index + 2], this._Kr[index + 2]);
                D ^= Cast5Engine.F1(A, this._Km[index + 3], this._Kr[index + 3]);
            }
            for (int j = 6; j < 12; j++)
            {
                int index = j * 4;
                D ^= Cast5Engine.F1(A, this._Km[index + 3], this._Kr[index + 3]);
                A ^= Cast5Engine.F3(B, this._Km[index + 2], this._Kr[index + 2]);
                B ^= Cast5Engine.F2(C, this._Km[index + 1], this._Kr[index + 1]);
                C ^= Cast5Engine.F1(D, this._Km[index], this._Kr[index]);
            }
            result[0] = A;
            result[1] = B;
            result[2] = C;
            result[3] = D;
        }

        internal override int DecryptBlock(byte[] src, int srcIndex, byte[] dst, int dstIndex)
        {
            uint a = Pack.BE_To_UInt32(src, srcIndex);
            uint b = Pack.BE_To_UInt32(src, srcIndex + 4);
            uint c = Pack.BE_To_UInt32(src, srcIndex + 8);
            uint d = Pack.BE_To_UInt32(src, srcIndex + 12);
            uint[] result = new uint[4];
            this.CAST_Decipher(a, b, c, d, result);
            Pack.UInt32_To_BE(result[0], dst, dstIndex);
            Pack.UInt32_To_BE(result[1], dst, dstIndex + 4);
            Pack.UInt32_To_BE(result[2], dst, dstIndex + 8);
            Pack.UInt32_To_BE(result[3], dst, dstIndex + 12);
            return 0x10;
        }

        internal override int EncryptBlock(byte[] src, int srcIndex, byte[] dst, int dstIndex)
        {
            uint a = Pack.BE_To_UInt32(src, srcIndex);
            uint b = Pack.BE_To_UInt32(src, srcIndex + 4);
            uint c = Pack.BE_To_UInt32(src, srcIndex + 8);
            uint d = Pack.BE_To_UInt32(src, srcIndex + 12);
            uint[] result = new uint[4];
            this.CAST_Encipher(a, b, c, d, result);
            Pack.UInt32_To_BE(result[0], dst, dstIndex);
            Pack.UInt32_To_BE(result[1], dst, dstIndex + 4);
            Pack.UInt32_To_BE(result[2], dst, dstIndex + 8);
            Pack.UInt32_To_BE(result[3], dst, dstIndex + 12);
            return 0x10;
        }

        public override int GetBlockSize() => 
            0x10;

        public override void Reset()
        {
        }

        internal override void SetKey(byte[] key)
        {
            uint num = 0x5a827999;
            uint num2 = 0x6ed9eba1;
            int num3 = 0x13;
            int num4 = 0x11;
            for (int i = 0; i < 0x18; i++)
            {
                for (int m = 0; m < 8; m++)
                {
                    this._Tm[(i * 8) + m] = num;
                    num += num2;
                    this._Tr[(i * 8) + m] = num3;
                    num3 = (num3 + num4) & 0x1f;
                }
            }
            byte[] array = new byte[0x40];
            key.CopyTo(array, 0);
            for (int j = 0; j < 8; j++)
            {
                this._workingKey[j] = Pack.BE_To_UInt32(array, j * 4);
            }
            for (int k = 0; k < 12; k++)
            {
                int index = (k * 2) * 8;
                this._workingKey[6] ^= Cast5Engine.F1(this._workingKey[7], this._Tm[index], this._Tr[index]);
                this._workingKey[5] ^= Cast5Engine.F2(this._workingKey[6], this._Tm[index + 1], this._Tr[index + 1]);
                this._workingKey[4] ^= Cast5Engine.F3(this._workingKey[5], this._Tm[index + 2], this._Tr[index + 2]);
                this._workingKey[3] ^= Cast5Engine.F1(this._workingKey[4], this._Tm[index + 3], this._Tr[index + 3]);
                this._workingKey[2] ^= Cast5Engine.F2(this._workingKey[3], this._Tm[index + 4], this._Tr[index + 4]);
                this._workingKey[1] ^= Cast5Engine.F3(this._workingKey[2], this._Tm[index + 5], this._Tr[index + 5]);
                this._workingKey[0] ^= Cast5Engine.F1(this._workingKey[1], this._Tm[index + 6], this._Tr[index + 6]);
                this._workingKey[7] ^= Cast5Engine.F2(this._workingKey[0], this._Tm[index + 7], this._Tr[index + 7]);
                index = ((k * 2) + 1) * 8;
                this._workingKey[6] ^= Cast5Engine.F1(this._workingKey[7], this._Tm[index], this._Tr[index]);
                this._workingKey[5] ^= Cast5Engine.F2(this._workingKey[6], this._Tm[index + 1], this._Tr[index + 1]);
                this._workingKey[4] ^= Cast5Engine.F3(this._workingKey[5], this._Tm[index + 2], this._Tr[index + 2]);
                this._workingKey[3] ^= Cast5Engine.F1(this._workingKey[4], this._Tm[index + 3], this._Tr[index + 3]);
                this._workingKey[2] ^= Cast5Engine.F2(this._workingKey[3], this._Tm[index + 4], this._Tr[index + 4]);
                this._workingKey[1] ^= Cast5Engine.F3(this._workingKey[2], this._Tm[index + 5], this._Tr[index + 5]);
                this._workingKey[0] ^= Cast5Engine.F1(this._workingKey[1], this._Tm[index + 6], this._Tr[index + 6]);
                this._workingKey[7] ^= Cast5Engine.F2(this._workingKey[0], this._Tm[index + 7], this._Tr[index + 7]);
                this._Kr[k * 4] = ((int) this._workingKey[0]) & 0x1f;
                this._Kr[(k * 4) + 1] = ((int) this._workingKey[2]) & 0x1f;
                this._Kr[(k * 4) + 2] = ((int) this._workingKey[4]) & 0x1f;
                this._Kr[(k * 4) + 3] = ((int) this._workingKey[6]) & 0x1f;
                this._Km[k * 4] = this._workingKey[7];
                this._Km[(k * 4) + 1] = this._workingKey[5];
                this._Km[(k * 4) + 2] = this._workingKey[3];
                this._Km[(k * 4) + 3] = this._workingKey[1];
            }
        }

        public override string AlgorithmName =>
            "CAST6";
    }
}

