namespace Org.BouncyCastle.Utilities.IO
{
    using System;
    using System.IO;

    public abstract class BaseOutputStream : Stream
    {
        private bool closed;

        protected BaseOutputStream()
        {
        }

        public override void Close()
        {
            this.closed = true;
            base.Close();
        }

        public override void Flush()
        {
        }

        public sealed override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public sealed override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public sealed override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public virtual void Write(params byte[] buffer)
        {
            this.Write(buffer, 0, buffer.Length);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            int num = offset + count;
            for (int i = offset; i < num; i++)
            {
                this.WriteByte(buffer[i]);
            }
        }

        public sealed override bool CanRead =>
            false;

        public sealed override bool CanSeek =>
            false;

        public sealed override bool CanWrite =>
            !this.closed;

        public sealed override long Length
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public sealed override long Position
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

