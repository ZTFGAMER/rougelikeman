namespace Org.BouncyCastle.Utilities.Encoders
{
    using Org.BouncyCastle.Utilities;
    using System;
    using System.IO;

    public class HexEncoder : IEncoder
    {
        protected readonly byte[] encodingTable = new byte[] { 0x30, 0x31, 50, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x61, 0x62, 0x63, 100, 0x65, 0x66 };
        protected readonly byte[] decodingTable = new byte[0x80];

        public HexEncoder()
        {
            this.InitialiseDecodingTable();
        }

        public int Decode(byte[] data, int off, int length, Stream outStream)
        {
            int num3 = 0;
            int num4 = off + length;
            while (num4 > off)
            {
                if (!Ignore((char) data[num4 - 1]))
                {
                    break;
                }
                num4--;
            }
            int index = off;
            while (index < num4)
            {
                while ((index < num4) && Ignore((char) data[index]))
                {
                    index++;
                }
                byte num = this.decodingTable[data[index++]];
                while ((index < num4) && Ignore((char) data[index]))
                {
                    index++;
                }
                byte num2 = this.decodingTable[data[index++]];
                if ((num | num2) >= 0x80)
                {
                    throw new IOException("invalid characters encountered in Hex data");
                }
                outStream.WriteByte((byte) ((num << 4) | num2));
                num3++;
            }
            return num3;
        }

        public int DecodeString(string data, Stream outStream)
        {
            int num3 = 0;
            int length = data.Length;
            while (length > 0)
            {
                if (!Ignore(data[length - 1]))
                {
                    break;
                }
                length--;
            }
            int num5 = 0;
            while (num5 < length)
            {
                while ((num5 < length) && Ignore(data[num5]))
                {
                    num5++;
                }
                byte num = this.decodingTable[data[num5++]];
                while ((num5 < length) && Ignore(data[num5]))
                {
                    num5++;
                }
                byte num2 = this.decodingTable[data[num5++]];
                if ((num | num2) >= 0x80)
                {
                    throw new IOException("invalid characters encountered in Hex data");
                }
                outStream.WriteByte((byte) ((num << 4) | num2));
                num3++;
            }
            return num3;
        }

        public int Encode(byte[] data, int off, int length, Stream outStream)
        {
            for (int i = off; i < (off + length); i++)
            {
                int num2 = data[i];
                outStream.WriteByte(this.encodingTable[num2 >> 4]);
                outStream.WriteByte(this.encodingTable[num2 & 15]);
            }
            return (length * 2);
        }

        private static bool Ignore(char c) => 
            ((((c == '\n') || (c == '\r')) || (c == '\t')) || (c == ' '));

        protected void InitialiseDecodingTable()
        {
            Arrays.Fill(this.decodingTable, 0xff);
            for (int i = 0; i < this.encodingTable.Length; i++)
            {
                this.decodingTable[this.encodingTable[i]] = (byte) i;
            }
            this.decodingTable[0x41] = this.decodingTable[0x61];
            this.decodingTable[0x42] = this.decodingTable[0x62];
            this.decodingTable[0x43] = this.decodingTable[0x63];
            this.decodingTable[0x44] = this.decodingTable[100];
            this.decodingTable[0x45] = this.decodingTable[0x65];
            this.decodingTable[70] = this.decodingTable[0x66];
        }
    }
}

