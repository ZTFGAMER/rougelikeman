namespace Org.BouncyCastle.Utilities.Zlib
{
    using Org.BouncyCastle.Utilities;
    using System;
    using System.IO;

    public class ZOutputStream : Stream
    {
        private const int BufferSize = 0x200;
        protected ZStream z;
        protected int flushLevel;
        protected byte[] buf;
        protected byte[] buf1;
        protected bool compress;
        protected Stream output;
        protected bool closed;

        public ZOutputStream(Stream output) : this(output, false)
        {
        }

        public ZOutputStream(Stream output, ZStream z)
        {
            this.buf = new byte[0x200];
            this.buf1 = new byte[1];
            if (z == null)
            {
                z = new ZStream();
            }
            if ((z.istate == null) && (z.dstate == null))
            {
                z.inflateInit();
            }
            this.output = output;
            this.compress = z.istate == null;
            this.z = z;
        }

        public ZOutputStream(Stream output, bool nowrap) : this(output, GetDefaultZStream(nowrap))
        {
        }

        public ZOutputStream(Stream output, int level) : this(output, level, false)
        {
        }

        public ZOutputStream(Stream output, int level, bool nowrap)
        {
            this.buf = new byte[0x200];
            this.buf1 = new byte[1];
            this.output = output;
            this.compress = true;
            this.z = new ZStream();
            this.z.deflateInit(level, nowrap);
        }

        public override void Close()
        {
            if (!this.closed)
            {
                this.DoClose();
                base.Close();
            }
        }

        private void DoClose()
        {
            try
            {
                this.Finish();
            }
            catch (IOException)
            {
            }
            finally
            {
                this.closed = true;
                this.End();
                Platform.Dispose(this.output);
                this.output = null;
            }
        }

        public virtual void End()
        {
            if (this.z != null)
            {
                if (this.compress)
                {
                    this.z.deflateEnd();
                }
                else
                {
                    this.z.inflateEnd();
                }
                this.z.free();
                this.z = null;
            }
        }

        public virtual void Finish()
        {
            do
            {
                this.z.next_out = this.buf;
                this.z.next_out_index = 0;
                this.z.avail_out = this.buf.Length;
                int num = !this.compress ? this.z.inflate(4) : this.z.deflate(4);
                if ((num != 1) && (num != 0))
                {
                    throw new IOException((!this.compress ? "in" : "de") + "flating: " + this.z.msg);
                }
                int count = this.buf.Length - this.z.avail_out;
                if (count > 0)
                {
                    this.output.Write(this.buf, 0, count);
                }
            }
            while ((this.z.avail_in > 0) || (this.z.avail_out == 0));
            this.Flush();
        }

        public override void Flush()
        {
            this.output.Flush();
        }

        private static ZStream GetDefaultZStream(bool nowrap)
        {
            ZStream stream = new ZStream();
            stream.inflateInit(nowrap);
            return stream;
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

        public override void Write(byte[] b, int off, int len)
        {
            if (len != 0)
            {
                this.z.next_in = b;
                this.z.next_in_index = off;
                this.z.avail_in = len;
                do
                {
                    this.z.next_out = this.buf;
                    this.z.next_out_index = 0;
                    this.z.avail_out = this.buf.Length;
                    if ((!this.compress ? this.z.inflate(this.flushLevel) : this.z.deflate(this.flushLevel)) != 0)
                    {
                        throw new IOException((!this.compress ? "in" : "de") + "flating: " + this.z.msg);
                    }
                    this.output.Write(this.buf, 0, this.buf.Length - this.z.avail_out);
                }
                while ((this.z.avail_in > 0) || (this.z.avail_out == 0));
            }
        }

        public override void WriteByte(byte b)
        {
            this.buf1[0] = b;
            this.Write(this.buf1, 0, 1);
        }

        public sealed override bool CanRead =>
            false;

        public sealed override bool CanSeek =>
            false;

        public sealed override bool CanWrite =>
            !this.closed;

        public virtual int FlushMode
        {
            get => 
                this.flushLevel;
            set => 
                (this.flushLevel = value);
        }

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

        public virtual long TotalIn =>
            this.z.total_in;

        public virtual long TotalOut =>
            this.z.total_out;
    }
}

