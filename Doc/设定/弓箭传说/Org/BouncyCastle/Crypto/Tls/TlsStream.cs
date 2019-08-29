namespace Org.BouncyCastle.Crypto.Tls
{
    using System;
    using System.IO;

    internal class TlsStream : Stream
    {
        private readonly TlsProtocol handler;

        internal TlsStream(TlsProtocol handler)
        {
            this.handler = handler;
        }

        public override void Close()
        {
            this.handler.Close();
            base.Close();
        }

        public override void Flush()
        {
            this.handler.Flush();
        }

        public override int Read(byte[] buf, int off, int len) => 
            this.handler.ReadApplicationData(buf, off, len);

        public override int ReadByte()
        {
            byte[] buffer = new byte[1];
            if (this.Read(buffer, 0, 1) <= 0)
            {
                return -1;
            }
            return buffer[0];
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buf, int off, int len)
        {
            this.handler.WriteData(buf, off, len);
        }

        public override void WriteByte(byte b)
        {
            byte[] buf = new byte[] { b };
            this.handler.WriteData(buf, 0, 1);
        }

        public override bool CanRead =>
            !this.handler.IsClosed;

        public override bool CanSeek =>
            false;

        public override bool CanWrite =>
            !this.handler.IsClosed;

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

