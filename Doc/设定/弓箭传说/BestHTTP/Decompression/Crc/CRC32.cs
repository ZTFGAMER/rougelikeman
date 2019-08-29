namespace BestHTTP.Decompression.Crc
{
    using System;
    using System.IO;

    internal class CRC32
    {
        private uint dwPolynomial;
        private long _TotalBytesRead;
        private bool reverseBits;
        private uint[] crc32Table;
        private const int BUFFER_SIZE = 0x2000;
        private uint _register;

        public CRC32() : this(false)
        {
        }

        public CRC32(bool reverseBits) : this(-306674912, reverseBits)
        {
        }

        public CRC32(int polynomial, bool reverseBits)
        {
            this._register = uint.MaxValue;
            this.reverseBits = reverseBits;
            this.dwPolynomial = (uint) polynomial;
            this.GenerateLookupTable();
        }

        internal int _InternalComputeCrc32(uint W, byte B) => 
            ((int) (this.crc32Table[(int) ((IntPtr) ((W ^ B) & 0xff))] ^ (W >> 8)));

        public void Combine(int crc, int length)
        {
            uint[] square = new uint[0x20];
            uint[] mat = new uint[0x20];
            if (length != 0)
            {
                uint vec = ~this._register;
                uint num2 = (uint) crc;
                mat[0] = this.dwPolynomial;
                uint num3 = 1;
                for (int i = 1; i < 0x20; i++)
                {
                    mat[i] = num3;
                    num3 = num3 << 1;
                }
                this.gf2_matrix_square(square, mat);
                this.gf2_matrix_square(mat, square);
                uint num5 = (uint) length;
                do
                {
                    this.gf2_matrix_square(square, mat);
                    if ((num5 & 1) == 1)
                    {
                        vec = this.gf2_matrix_times(square, vec);
                    }
                    num5 = num5 >> 1;
                    if (num5 == 0)
                    {
                        break;
                    }
                    this.gf2_matrix_square(mat, square);
                    if ((num5 & 1) == 1)
                    {
                        vec = this.gf2_matrix_times(mat, vec);
                    }
                    num5 = num5 >> 1;
                }
                while (num5 != 0);
                vec ^= num2;
                this._register = ~vec;
            }
        }

        public int ComputeCrc32(int W, byte B) => 
            this._InternalComputeCrc32((uint) W, B);

        private void GenerateLookupTable()
        {
            this.crc32Table = new uint[0x100];
            byte data = 0;
            do
            {
                uint num = data;
                for (byte i = 8; i > 0; i = (byte) (i - 1))
                {
                    if ((num & 1) == 1)
                    {
                        num = (num >> 1) ^ this.dwPolynomial;
                    }
                    else
                    {
                        num = num >> 1;
                    }
                }
                if (this.reverseBits)
                {
                    this.crc32Table[ReverseBits(data)] = ReverseBits(num);
                }
                else
                {
                    this.crc32Table[data] = num;
                }
                data = (byte) (data + 1);
            }
            while (data != 0);
        }

        public int GetCrc32(Stream input) => 
            this.GetCrc32AndCopy(input, null);

        public int GetCrc32AndCopy(Stream input, Stream output)
        {
            if (input == null)
            {
                throw new Exception("The input stream must not be null.");
            }
            byte[] buffer = new byte[0x2000];
            int count = 0x2000;
            this._TotalBytesRead = 0L;
            int num2 = input.Read(buffer, 0, count);
            if (output != null)
            {
                output.Write(buffer, 0, num2);
            }
            this._TotalBytesRead += num2;
            while (num2 > 0)
            {
                this.SlurpBlock(buffer, 0, num2);
                num2 = input.Read(buffer, 0, count);
                if (output != null)
                {
                    output.Write(buffer, 0, num2);
                }
                this._TotalBytesRead += num2;
            }
            return (int) ~this._register;
        }

        private void gf2_matrix_square(uint[] square, uint[] mat)
        {
            for (int i = 0; i < 0x20; i++)
            {
                square[i] = this.gf2_matrix_times(mat, mat[i]);
            }
        }

        private uint gf2_matrix_times(uint[] matrix, uint vec)
        {
            uint num = 0;
            for (int i = 0; vec != 0; i++)
            {
                if ((vec & 1) == 1)
                {
                    num ^= matrix[i];
                }
                vec = vec >> 1;
            }
            return num;
        }

        public void Reset()
        {
            this._register = uint.MaxValue;
        }

        private static byte ReverseBits(byte data)
        {
            uint num = (uint) (data * 0x20202);
            uint num2 = 0x1044010;
            uint num3 = num & num2;
            uint num4 = (num << 2) & (num2 << 1);
            return (byte) ((0x1001001 * (num3 + num4)) >> 0x18);
        }

        private static uint ReverseBits(uint data)
        {
            uint num = data;
            num = ((uint) ((num & 0x55555555) << 1)) | ((num >> 1) & 0x55555555);
            num = ((uint) ((num & 0x33333333) << 2)) | ((num >> 2) & 0x33333333);
            num = ((uint) ((num & 0xf0f0f0f) << 4)) | ((num >> 4) & 0xf0f0f0f);
            return ((((num << 0x18) | ((uint) ((num & 0xff00) << 8))) | ((num >> 8) & 0xff00)) | (num >> 0x18));
        }

        public void SlurpBlock(byte[] block, int offset, int count)
        {
            if (block == null)
            {
                throw new Exception("The data buffer must not be null.");
            }
            for (int i = 0; i < count; i++)
            {
                int index = offset + i;
                byte num3 = block[index];
                if (this.reverseBits)
                {
                    uint num4 = (this._register >> 0x18) ^ num3;
                    this._register = (this._register << 8) ^ this.crc32Table[num4];
                }
                else
                {
                    uint num5 = (this._register & 0xff) ^ num3;
                    this._register = (this._register >> 8) ^ this.crc32Table[num5];
                }
            }
            this._TotalBytesRead += count;
        }

        public void UpdateCRC(byte b)
        {
            if (this.reverseBits)
            {
                uint index = (this._register >> 0x18) ^ b;
                this._register = (this._register << 8) ^ this.crc32Table[index];
            }
            else
            {
                uint index = (this._register & 0xff) ^ b;
                this._register = (this._register >> 8) ^ this.crc32Table[index];
            }
        }

        public void UpdateCRC(byte b, int n)
        {
            while (n-- > 0)
            {
                if (this.reverseBits)
                {
                    uint num = (this._register >> 0x18) ^ b;
                    this._register = (this._register << 8) ^ this.crc32Table[(num < 0) ? ((int) ((IntPtr) (num + 0x100))) : ((int) ((IntPtr) num))];
                }
                else
                {
                    uint num2 = (this._register & 0xff) ^ b;
                    this._register = (this._register >> 8) ^ this.crc32Table[(num2 < 0) ? ((int) ((IntPtr) (num2 + 0x100))) : ((int) ((IntPtr) num2))];
                }
            }
        }

        public long TotalBytesRead =>
            this._TotalBytesRead;

        public int Crc32Result =>
            ((int) ~this._register);
    }
}

