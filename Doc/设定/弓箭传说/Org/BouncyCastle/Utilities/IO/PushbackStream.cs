namespace Org.BouncyCastle.Utilities.IO
{
    using System;
    using System.IO;

    public class PushbackStream : FilterStream
    {
        private int buf;

        public PushbackStream(Stream s) : base(s)
        {
            this.buf = -1;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if ((this.buf != -1) && (count > 0))
            {
                buffer[offset] = (byte) this.buf;
                this.buf = -1;
                return 1;
            }
            return base.Read(buffer, offset, count);
        }

        public override int ReadByte()
        {
            if (this.buf != -1)
            {
                int buf = this.buf;
                this.buf = -1;
                return buf;
            }
            return base.ReadByte();
        }

        public virtual void Unread(int b)
        {
            if (this.buf != -1)
            {
                throw new InvalidOperationException("Can only push back one byte");
            }
            this.buf = b & 0xff;
        }
    }
}

