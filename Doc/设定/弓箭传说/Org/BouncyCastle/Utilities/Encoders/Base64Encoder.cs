namespace Org.BouncyCastle.Utilities.Encoders
{
    using Org.BouncyCastle.Utilities;
    using System;
    using System.IO;

    public class Base64Encoder : IEncoder
    {
        protected readonly byte[] encodingTable = new byte[] { 
            0x41, 0x42, 0x43, 0x44, 0x45, 70, 0x47, 0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, 0x4f, 80,
            0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 90, 0x61, 0x62, 0x63, 100, 0x65, 0x66,
            0x67, 0x68, 0x69, 0x6a, 0x6b, 0x6c, 0x6d, 110, 0x6f, 0x70, 0x71, 0x72, 0x73, 0x74, 0x75, 0x76,
            0x77, 120, 0x79, 0x7a, 0x30, 0x31, 50, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x2b, 0x2f
        };
        protected byte padding = 0x3d;
        protected readonly byte[] decodingTable = new byte[0x80];

        public Base64Encoder()
        {
            this.InitialiseDecodingTable();
        }

        public int Decode(byte[] data, int off, int length, Stream outStream)
        {
            int num5 = 0;
            int num6 = off + length;
            while (num6 > off)
            {
                if (!this.ignore((char) data[num6 - 1]))
                {
                    break;
                }
                num6--;
            }
            int i = off;
            int finish = num6 - 4;
            for (i = this.nextI(data, i, finish); i < finish; i = this.nextI(data, i, finish))
            {
                byte num = this.decodingTable[data[i++]];
                i = this.nextI(data, i, finish);
                byte num2 = this.decodingTable[data[i++]];
                i = this.nextI(data, i, finish);
                byte num3 = this.decodingTable[data[i++]];
                i = this.nextI(data, i, finish);
                byte num4 = this.decodingTable[data[i++]];
                if ((((num | num2) | num3) | num4) >= 0x80)
                {
                    throw new IOException("invalid characters encountered in base64 data");
                }
                outStream.WriteByte((byte) ((num << 2) | (num2 >> 4)));
                outStream.WriteByte((byte) ((num2 << 4) | (num3 >> 2)));
                outStream.WriteByte((byte) ((num3 << 6) | num4));
                num5 += 3;
            }
            return (num5 + this.decodeLastBlock(outStream, (char) data[num6 - 4], (char) data[num6 - 3], (char) data[num6 - 2], (char) data[num6 - 1]));
        }

        private int decodeLastBlock(Stream outStream, char c1, char c2, char c3, char c4)
        {
            if (c3 == this.padding)
            {
                byte num = this.decodingTable[c1];
                byte num2 = this.decodingTable[c2];
                if ((num | num2) >= 0x80)
                {
                    throw new IOException("invalid characters encountered at end of base64 data");
                }
                outStream.WriteByte((byte) ((num << 2) | (num2 >> 4)));
                return 1;
            }
            if (c4 == this.padding)
            {
                byte num3 = this.decodingTable[c1];
                byte num4 = this.decodingTable[c2];
                byte num5 = this.decodingTable[c3];
                if (((num3 | num4) | num5) >= 0x80)
                {
                    throw new IOException("invalid characters encountered at end of base64 data");
                }
                outStream.WriteByte((byte) ((num3 << 2) | (num4 >> 4)));
                outStream.WriteByte((byte) ((num4 << 4) | (num5 >> 2)));
                return 2;
            }
            byte num6 = this.decodingTable[c1];
            byte num7 = this.decodingTable[c2];
            byte num8 = this.decodingTable[c3];
            byte num9 = this.decodingTable[c4];
            if ((((num6 | num7) | num8) | num9) >= 0x80)
            {
                throw new IOException("invalid characters encountered at end of base64 data");
            }
            outStream.WriteByte((byte) ((num6 << 2) | (num7 >> 4)));
            outStream.WriteByte((byte) ((num7 << 4) | (num8 >> 2)));
            outStream.WriteByte((byte) ((num8 << 6) | num9));
            return 3;
        }

        public int DecodeString(string data, Stream outStream)
        {
            int num5 = 0;
            int length = data.Length;
            while (length > 0)
            {
                if (!this.ignore(data[length - 1]))
                {
                    break;
                }
                length--;
            }
            int i = 0;
            int finish = length - 4;
            for (i = this.nextI(data, i, finish); i < finish; i = this.nextI(data, i, finish))
            {
                byte num = this.decodingTable[data[i++]];
                i = this.nextI(data, i, finish);
                byte num2 = this.decodingTable[data[i++]];
                i = this.nextI(data, i, finish);
                byte num3 = this.decodingTable[data[i++]];
                i = this.nextI(data, i, finish);
                byte num4 = this.decodingTable[data[i++]];
                if ((((num | num2) | num3) | num4) >= 0x80)
                {
                    throw new IOException("invalid characters encountered in base64 data");
                }
                outStream.WriteByte((byte) ((num << 2) | (num2 >> 4)));
                outStream.WriteByte((byte) ((num2 << 4) | (num3 >> 2)));
                outStream.WriteByte((byte) ((num3 << 6) | num4));
                num5 += 3;
            }
            return (num5 + this.decodeLastBlock(outStream, data[length - 4], data[length - 3], data[length - 2], data[length - 1]));
        }

        public int Encode(byte[] data, int off, int length, Stream outStream)
        {
            int num = length % 3;
            int num2 = length - num;
            for (int i = off; i < (off + num2); i += 3)
            {
                int num3 = data[i] & 0xff;
                int num4 = data[i + 1] & 0xff;
                int num5 = data[i + 2] & 0xff;
                outStream.WriteByte(this.encodingTable[(num3 >> 2) & 0x3f]);
                outStream.WriteByte(this.encodingTable[((num3 << 4) | (num4 >> 4)) & 0x3f]);
                outStream.WriteByte(this.encodingTable[((num4 << 2) | (num5 >> 6)) & 0x3f]);
                outStream.WriteByte(this.encodingTable[num5 & 0x3f]);
            }
            if (num != 0)
            {
                int num7;
                int num8;
                int num10;
                if (num == 1)
                {
                    num10 = data[off + num2] & 0xff;
                    num7 = (num10 >> 2) & 0x3f;
                    num8 = (num10 << 4) & 0x3f;
                    outStream.WriteByte(this.encodingTable[num7]);
                    outStream.WriteByte(this.encodingTable[num8]);
                    outStream.WriteByte(this.padding);
                    outStream.WriteByte(this.padding);
                }
                else if (num == 2)
                {
                    num10 = data[off + num2] & 0xff;
                    int num11 = data[(off + num2) + 1] & 0xff;
                    num7 = (num10 >> 2) & 0x3f;
                    num8 = ((num10 << 4) | (num11 >> 4)) & 0x3f;
                    int index = (num11 << 2) & 0x3f;
                    outStream.WriteByte(this.encodingTable[num7]);
                    outStream.WriteByte(this.encodingTable[num8]);
                    outStream.WriteByte(this.encodingTable[index]);
                    outStream.WriteByte(this.padding);
                }
            }
            return (((num2 / 3) * 4) + ((num != 0) ? 4 : 0));
        }

        private bool ignore(char c) => 
            ((((c == '\n') || (c == '\r')) || (c == '\t')) || (c == ' '));

        protected void InitialiseDecodingTable()
        {
            Arrays.Fill(this.decodingTable, 0xff);
            for (int i = 0; i < this.encodingTable.Length; i++)
            {
                this.decodingTable[this.encodingTable[i]] = (byte) i;
            }
        }

        private int nextI(byte[] data, int i, int finish)
        {
            while ((i < finish) && this.ignore((char) data[i]))
            {
                i++;
            }
            return i;
        }

        private int nextI(string data, int i, int finish)
        {
            while ((i < finish) && this.ignore(data[i]))
            {
                i++;
            }
            return i;
        }
    }
}

