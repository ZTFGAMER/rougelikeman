namespace Org.BouncyCastle.Crypto.Tls
{
    using System;

    public class ByteQueue
    {
        private const int DefaultCapacity = 0x400;
        private byte[] databuf;
        private int skipped;
        private int available;

        public ByteQueue() : this(0x400)
        {
        }

        public ByteQueue(int capacity)
        {
            this.databuf = new byte[capacity];
        }

        public void AddData(byte[] data, int offset, int len)
        {
            if (((this.skipped + this.available) + len) > this.databuf.Length)
            {
                int num = NextTwoPow(this.available + len);
                if (num > this.databuf.Length)
                {
                    byte[] destinationArray = new byte[num];
                    Array.Copy(this.databuf, this.skipped, destinationArray, 0, this.available);
                    this.databuf = destinationArray;
                }
                else
                {
                    Array.Copy(this.databuf, this.skipped, this.databuf, 0, this.available);
                }
                this.skipped = 0;
            }
            Array.Copy(data, offset, this.databuf, this.skipped + this.available, len);
            this.available += len;
        }

        public static int NextTwoPow(int i)
        {
            i |= i >> 1;
            i |= i >> 2;
            i |= i >> 4;
            i |= i >> 8;
            i |= i >> 0x10;
            return (i + 1);
        }

        public void Read(byte[] buf, int offset, int len, int skip)
        {
            if ((buf.Length - offset) < len)
            {
                object[] objArray1 = new object[] { "Buffer size of ", buf.Length, " is too small for a read of ", len, " bytes" };
                throw new ArgumentException(string.Concat(objArray1));
            }
            if ((this.available - skip) < len)
            {
                throw new InvalidOperationException("Not enough data to read");
            }
            Array.Copy(this.databuf, this.skipped + skip, buf, offset, len);
        }

        public void RemoveData(int i)
        {
            if (i > this.available)
            {
                object[] objArray1 = new object[] { "Cannot remove ", i, " bytes, only got ", this.available };
                throw new InvalidOperationException(string.Concat(objArray1));
            }
            this.available -= i;
            this.skipped += i;
        }

        public byte[] RemoveData(int len, int skip)
        {
            byte[] buf = new byte[len];
            this.RemoveData(buf, 0, len, skip);
            return buf;
        }

        public void RemoveData(byte[] buf, int off, int len, int skip)
        {
            this.Read(buf, off, len, skip);
            this.RemoveData(skip + len);
        }

        public int Available =>
            this.available;
    }
}

