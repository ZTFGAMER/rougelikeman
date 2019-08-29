namespace Org.BouncyCastle.Utilities.IO
{
    using Org.BouncyCastle.Utilities;
    using System;
    using System.IO;

    public class FilterStream : Stream
    {
        protected readonly Stream s;

        public FilterStream(Stream s)
        {
            this.s = s;
        }

        public override void Close()
        {
            Platform.Dispose(this.s);
            base.Close();
        }

        public override void Flush()
        {
            this.s.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count) => 
            this.s.Read(buffer, offset, count);

        public override int ReadByte() => 
            this.s.ReadByte();

        public override long Seek(long offset, SeekOrigin origin) => 
            this.s.Seek(offset, origin);

        public override void SetLength(long value)
        {
            this.s.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            this.s.Write(buffer, offset, count);
        }

        public override void WriteByte(byte value)
        {
            this.s.WriteByte(value);
        }

        public override bool CanRead =>
            this.s.CanRead;

        public override bool CanSeek =>
            this.s.CanSeek;

        public override bool CanWrite =>
            this.s.CanWrite;

        public override long Length =>
            this.s.Length;

        public override long Position
        {
            get => 
                this.s.Position;
            set => 
                (this.s.Position = value);
        }
    }
}

