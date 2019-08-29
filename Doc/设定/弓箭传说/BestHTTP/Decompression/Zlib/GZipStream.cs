namespace BestHTTP.Decompression.Zlib
{
    using System;
    using System.IO;
    using System.Text;

    internal class GZipStream : Stream
    {
        public DateTime? LastModified;
        private int _headerByteCount;
        internal ZlibBaseStream _baseStream;
        private bool _disposed;
        private bool _firstReadDone;
        private string _FileName;
        private string _Comment;
        private int _Crc32;
        internal static readonly DateTime _unixEpoch = new DateTime(0x7b2, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        internal static readonly Encoding iso8859dash1 = Encoding.GetEncoding("iso-8859-1");

        public GZipStream(Stream stream, CompressionMode mode) : this(stream, mode, CompressionLevel.Default, false)
        {
        }

        public GZipStream(Stream stream, CompressionMode mode, CompressionLevel level) : this(stream, mode, level, false)
        {
        }

        public GZipStream(Stream stream, CompressionMode mode, bool leaveOpen) : this(stream, mode, CompressionLevel.Default, leaveOpen)
        {
        }

        public GZipStream(Stream stream, CompressionMode mode, CompressionLevel level, bool leaveOpen)
        {
            this._baseStream = new ZlibBaseStream(stream, mode, level, ZlibStreamFlavor.GZIP, leaveOpen);
        }

        public static byte[] CompressBuffer(byte[] b)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                Stream compressor = new GZipStream(stream, CompressionMode.Compress, CompressionLevel.BestCompression);
                ZlibBaseStream.CompressBuffer(b, compressor);
                return stream.ToArray();
            }
        }

        public static byte[] CompressString(string s)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                Stream compressor = new GZipStream(stream, CompressionMode.Compress, CompressionLevel.BestCompression);
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
                        this._Crc32 = this._baseStream.Crc32;
                    }
                    this._disposed = true;
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        private int EmitHeader()
        {
            byte[] sourceArray = (this.Comment != null) ? iso8859dash1.GetBytes(this.Comment) : null;
            byte[] buffer2 = (this.FileName != null) ? iso8859dash1.GetBytes(this.FileName) : null;
            int num = (this.Comment != null) ? (sourceArray.Length + 1) : 0;
            int num2 = (this.FileName != null) ? (buffer2.Length + 1) : 0;
            int num3 = (10 + num) + num2;
            byte[] destinationArray = new byte[num3];
            int destinationIndex = 0;
            destinationArray[destinationIndex++] = 0x1f;
            destinationArray[destinationIndex++] = 0x8b;
            destinationArray[destinationIndex++] = 8;
            byte num5 = 0;
            if (this.Comment != null)
            {
                num5 = (byte) (num5 ^ 0x10);
            }
            if (this.FileName != null)
            {
                num5 = (byte) (num5 ^ 8);
            }
            destinationArray[destinationIndex++] = num5;
            if (!this.LastModified.HasValue)
            {
                this.LastModified = new DateTime?(DateTime.Now);
            }
            TimeSpan span = this.LastModified.Value - _unixEpoch;
            int totalSeconds = (int) span.TotalSeconds;
            Array.Copy(BitConverter.GetBytes(totalSeconds), 0, destinationArray, destinationIndex, 4);
            destinationIndex += 4;
            destinationArray[destinationIndex++] = 0;
            destinationArray[destinationIndex++] = 0xff;
            if (num2 != 0)
            {
                Array.Copy(buffer2, 0, destinationArray, destinationIndex, num2 - 1);
                destinationIndex += num2 - 1;
                destinationArray[destinationIndex++] = 0;
            }
            if (num != 0)
            {
                Array.Copy(sourceArray, 0, destinationArray, destinationIndex, num - 1);
                destinationIndex += num - 1;
                destinationArray[destinationIndex++] = 0;
            }
            this._baseStream._stream.Write(destinationArray, 0, destinationArray.Length);
            return destinationArray.Length;
        }

        public override void Flush()
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException("GZipStream");
            }
            this._baseStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException("GZipStream");
            }
            int num = this._baseStream.Read(buffer, offset, count);
            if (!this._firstReadDone)
            {
                this._firstReadDone = true;
                this.FileName = this._baseStream._GzipFileName;
                this.Comment = this._baseStream._GzipComment;
            }
            return num;
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
                Stream decompressor = new GZipStream(stream, CompressionMode.Decompress);
                return ZlibBaseStream.UncompressBuffer(compressed, decompressor);
            }
        }

        public static string UncompressString(byte[] compressed)
        {
            using (MemoryStream stream = new MemoryStream(compressed))
            {
                Stream decompressor = new GZipStream(stream, CompressionMode.Decompress);
                return ZlibBaseStream.UncompressString(compressed, decompressor);
            }
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException("GZipStream");
            }
            if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Undefined)
            {
                if (!this._baseStream._wantCompress)
                {
                    throw new InvalidOperationException();
                }
                this._headerByteCount = this.EmitHeader();
            }
            this._baseStream.Write(buffer, offset, count);
        }

        public string Comment
        {
            get => 
                this._Comment;
            set
            {
                if (this._disposed)
                {
                    throw new ObjectDisposedException("GZipStream");
                }
                this._Comment = value;
            }
        }

        public string FileName
        {
            get => 
                this._FileName;
            set
            {
                if (this._disposed)
                {
                    throw new ObjectDisposedException("GZipStream");
                }
                this._FileName = value;
                if (this._FileName != null)
                {
                    if (this._FileName.IndexOf("/") != -1)
                    {
                        this._FileName = this._FileName.Replace("/", @"\");
                    }
                    if (this._FileName.EndsWith(@"\"))
                    {
                        throw new Exception("Illegal filename");
                    }
                    if (this._FileName.IndexOf(@"\") != -1)
                    {
                        this._FileName = Path.GetFileName(this._FileName);
                    }
                }
            }
        }

        public int Crc32 =>
            this._Crc32;

        public virtual FlushType FlushMode
        {
            get => 
                this._baseStream._flushMode;
            set
            {
                if (this._disposed)
                {
                    throw new ObjectDisposedException("GZipStream");
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
                    throw new ObjectDisposedException("GZipStream");
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
                    throw new ObjectDisposedException("GZipStream");
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
                    throw new ObjectDisposedException("GZipStream");
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
                    return (this._baseStream._z.TotalBytesOut + this._headerByteCount);
                }
                if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Reader)
                {
                    return (this._baseStream._z.TotalBytesIn + this._baseStream._gzipHeaderByteCount);
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

