namespace BestHTTP.Decompression.Zlib
{
    using System;
    using System.IO;

    internal class DeflateStream : Stream
    {
        internal ZlibBaseStream _baseStream;
        internal Stream _innerStream;
        private bool _disposed;

        public DeflateStream(Stream stream, CompressionMode mode) : this(stream, mode, CompressionLevel.Default, false)
        {
        }

        public DeflateStream(Stream stream, CompressionMode mode, CompressionLevel level) : this(stream, mode, level, false)
        {
        }

        public DeflateStream(Stream stream, CompressionMode mode, bool leaveOpen) : this(stream, mode, CompressionLevel.Default, leaveOpen)
        {
        }

        public DeflateStream(Stream stream, CompressionMode mode, CompressionLevel level, bool leaveOpen)
        {
            this._innerStream = stream;
            this._baseStream = new ZlibBaseStream(stream, mode, level, ZlibStreamFlavor.DEFLATE, leaveOpen);
        }

        public DeflateStream(Stream stream, CompressionMode mode, CompressionLevel level, bool leaveOpen, int windowBits)
        {
            this._innerStream = stream;
            this._baseStream = new ZlibBaseStream(stream, mode, level, ZlibStreamFlavor.DEFLATE, leaveOpen, windowBits);
        }

        public static byte[] CompressBuffer(byte[] b)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                Stream compressor = new DeflateStream(stream, CompressionMode.Compress, CompressionLevel.BestCompression);
                ZlibBaseStream.CompressBuffer(b, compressor);
                return stream.ToArray();
            }
        }

        public static byte[] CompressString(string s)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                Stream compressor = new DeflateStream(stream, CompressionMode.Compress, CompressionLevel.BestCompression);
                ZlibBaseStream.CompressString(s, compressor);
                return stream.ToArray();
            }
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (!this._disposed)
                {
                    if (disposing && (this._baseStream != null))
                    {
                        this._baseStream.Close();
                    }
                    this._disposed = true;
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        public override void Flush()
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException("DeflateStream");
            }
            this._baseStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException("DeflateStream");
            }
            return this._baseStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            this._baseStream.SetLength(value);
        }

        public static byte[] UncompressBuffer(byte[] compressed)
        {
            using (MemoryStream stream = new MemoryStream(compressed))
            {
                Stream decompressor = new DeflateStream(stream, CompressionMode.Decompress);
                return ZlibBaseStream.UncompressBuffer(compressed, decompressor);
            }
        }

        public static string UncompressString(byte[] compressed)
        {
            using (MemoryStream stream = new MemoryStream(compressed))
            {
                Stream decompressor = new DeflateStream(stream, CompressionMode.Decompress);
                return ZlibBaseStream.UncompressString(compressed, decompressor);
            }
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException("DeflateStream");
            }
            this._baseStream.Write(buffer, offset, count);
        }

        public virtual FlushType FlushMode
        {
            get => 
                this._baseStream._flushMode;
            set
            {
                if (this._disposed)
                {
                    throw new ObjectDisposedException("DeflateStream");
                }
                this._baseStream._flushMode = value;
            }
        }

        public int BufferSize
        {
            get => 
                this._baseStream._bufferSize;
            set
            {
                if (this._disposed)
                {
                    throw new ObjectDisposedException("DeflateStream");
                }
                if (this._baseStream._workingBuffer != null)
                {
                    throw new ZlibException("The working buffer is already set.");
                }
                if (value < 0x400)
                {
                    throw new ZlibException($"Don't be silly. {value} bytes?? Use a bigger buffer, at least {0x400}.");
                }
                this._baseStream._bufferSize = value;
            }
        }

        public CompressionStrategy Strategy
        {
            get => 
                this._baseStream.Strategy;
            set
            {
                if (this._disposed)
                {
                    throw new ObjectDisposedException("DeflateStream");
                }
                this._baseStream.Strategy = value;
            }
        }

        public virtual long TotalIn =>
            this._baseStream._z.TotalBytesIn;

        public virtual long TotalOut =>
            this._baseStream._z.TotalBytesOut;

        public override bool CanRead
        {
            get
            {
                if (this._disposed)
                {
                    throw new ObjectDisposedException("DeflateStream");
                }
                return this._baseStream._stream.CanRead;
            }
        }

        public override bool CanSeek =>
            false;

        public override bool CanWrite
        {
            get
            {
                if (this._disposed)
                {
                    throw new ObjectDisposedException("DeflateStream");
                }
                return this._baseStream._stream.CanWrite;
            }
        }

        public override long Length
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override long Position
        {
            get
            {
                if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Writer)
                {
                    return this._baseStream._z.TotalBytesOut;
                }
                if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Reader)
                {
                    return this._baseStream._z.TotalBytesIn;
                }
                return 0L;
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}

