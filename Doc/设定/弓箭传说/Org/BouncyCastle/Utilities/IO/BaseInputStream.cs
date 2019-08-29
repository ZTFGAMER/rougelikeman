namespace Org.BouncyCastle.Utilities.IO
{
    using System;
    using System.IO;

    public abstract class BaseInputStream : Stream
    {
        private bool closed;

        protected BaseInputStream()
        {
        }

        public override void Close()
        {
            this.closed = true;
            base.Close();
        }

        public sealed override void Flush()
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int num = offset;
            try
            {
                int num2 = offset + count;
                while (num < num2)
                {
                    int num3 = this.ReadByte();
                    if (num3 == -1)
                    {
                        goto Label_0042;
                    }
                    buffer[num++] = (byte) num3;
                }
            }
            catch (IOException)
            {
                if (num == offset)
                {
                    throw;
                }
            }
        Label_0042:
            return (num - offset);
        }

        public sealed override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public sealed override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public sealed override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public sealed override bool CanRead =>
            !this.closed;

        public sealed override bool CanSeek =>
            false;

        public sealed override bool CanWrite =>
            false;

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

