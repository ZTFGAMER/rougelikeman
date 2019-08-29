namespace Org.BouncyCastle.Crypto.Tls
{
    using System;
    using System.IO;

    public class ByteQueueStream : Stream
    {
        private readonly ByteQueue buffer = new ByteQueue();

        public override void Flush()
        {
        }

        public virtual int Peek(byte[] buf)
        {
            int len = Math.Min(this.buffer.Available, buf.Length);
            this.buffer.Read(buf, 0, len, 0);
            return len;
        }

        public virtual int Read(byte[] buf) => 
            this.Read(buf, 0, buf.Length);

        public override int Read(byte[] buf, int off, int len)
        {
            int num = Math.Min(this.buffer.Available, len);
            this.buffer.RemoveData(buf, off, num, 0);
            return num;
        }

        public override int ReadByte()
        {
            if (this.buffer.Available == 0)
            {
                return -1;
            }
            return (this.buffer.RemoveData(1, 0)[0] & 0xff);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public virtual int Skip(int n)
        {
            int i = Math.Min(this.buffer.Available, n);
            this.buffer.RemoveData(i);
            return i;
        }

        public virtual void Write(byte[] buf)
        {
            this.buffer.AddData(buf, 0, buf.Length);
        }

        public override void Write(byte[] buf, int off, int len)
        {
            this.buffer.AddData(buf, off, len);
        }

        public override void WriteByte(byte b)
        {
            byte[] data = new byte[] { b };
            this.buffer.AddData(data, 0, 1);
        }

        public virtual int Available =>
            this.buffer.Available;

        public override bool CanRead =>
            true;

        public override bool CanSeek =>
            false;

        public override bool CanWrite =>
            true;

        public override long Length
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public override long Position
        {
            get
            {
                throw new NotSupportedException();
            }
            set
            {
                throw new NotSupportedException();
            }
        }
    }
}

