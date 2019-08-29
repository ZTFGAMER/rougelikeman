namespace BestHTTP.Decompression.Zlib
{
    using BestHTTP.Decompression.Crc;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    internal class ZlibBaseStream : Stream
    {
        protected internal ZlibCodec _z;
        protected internal StreamMode _streamMode;
        protected internal FlushType _flushMode;
        protected internal ZlibStreamFlavor _flavor;
        protected internal CompressionMode _compressionMode;
        protected internal CompressionLevel _level;
        protected internal bool _leaveOpen;
        protected internal byte[] _workingBuffer;
        protected internal int _bufferSize;
        protected internal int windowBitsMax;
        protected internal byte[] _buf1;
        protected internal Stream _stream;
        protected internal CompressionStrategy Strategy;
        private CRC32 crc;
        protected internal string _GzipFileName;
        protected internal string _GzipComment;
        protected internal DateTime _GzipMtime;
        protected internal int _gzipHeaderByteCount;
        private bool nomoreinput;

        public ZlibBaseStream(Stream stream, CompressionMode compressionMode, CompressionLevel level, ZlibStreamFlavor flavor, bool leaveOpen) : this(stream, compressionMode, level, flavor, leaveOpen, 15)
        {
        }

        public ZlibBaseStream(Stream stream, CompressionMode compressionMode, CompressionLevel level, ZlibStreamFlavor flavor, bool leaveOpen, int windowBits)
        {
            this._streamMode = StreamMode.Undefined;
            this._bufferSize = 0x4000;
            this._buf1 = new byte[1];
            this._flushMode = FlushType.None;
            this._stream = stream;
            this._leaveOpen = leaveOpen;
            this._compressionMode = compressionMode;
            this._flavor = flavor;
            this._level = level;
            this.windowBitsMax = windowBits;
            if (flavor == ZlibStreamFlavor.GZIP)
            {
                this.crc = new CRC32();
            }
        }

        private int _ReadAndValidateGzipHeader()
        {
            int num = 0;
            byte[] buffer = new byte[10];
            int num2 = this._stream.Read(buffer, 0, buffer.Length);
            if (num2 == 0)
            {
                return 0;
            }
            if (num2 != 10)
            {
                throw new ZlibException("Not a valid GZIP stream.");
            }
            if (((buffer[0] != 0x1f) || (buffer[1] != 0x8b)) || (buffer[2] != 8))
            {
                throw new ZlibException("Bad GZIP header.");
            }
            int num3 = BitConverter.ToInt32(buffer, 4);
            this._GzipMtime = GZipStream._unixEpoch.AddSeconds((double) num3);
            num += num2;
            if ((buffer[3] & 4) == 4)
            {
                num2 = this._stream.Read(buffer, 0, 2);
                num += num2;
                short num4 = (short) (buffer[0] + (buffer[1] * 0x100));
                byte[] buffer2 = new byte[num4];
                num2 = this._stream.Read(buffer2, 0, buffer2.Length);
                if (num2 != num4)
                {
                    throw new ZlibException("Unexpected end-of-file reading GZIP header.");
                }
                num += num2;
            }
            if ((buffer[3] & 8) == 8)
            {
                this._GzipFileName = this.ReadZeroTerminatedString();
            }
            if ((buffer[3] & 0x10) == 0x10)
            {
                this._GzipComment = this.ReadZeroTerminatedString();
            }
            if ((buffer[3] & 2) == 2)
            {
                this.Read(this._buf1, 0, 1);
            }
            return num;
        }

        public override void Close()
        {
            if (this._stream != null)
            {
                try
                {
                    this.finish();
                }
                finally
                {
                    this.end();
                    if (!this._leaveOpen)
                    {
                        this._stream.Dispose();
                    }
                    this._stream = null;
                }
            }
        }

        public static void CompressBuffer(byte[] b, Stream compressor)
        {
            using (compressor)
            {
                compressor.Write(b, 0, b.Length);
            }
        }

        public static void CompressString(string s, Stream compressor)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            using (compressor)
            {
                compressor.Write(bytes, 0, bytes.Length);
            }
        }

        private void end()
        {
            if (this.z != null)
            {
                if (this._wantCompress)
                {
                    this._z.EndDeflate();
                }
                else
                {
                    this._z.EndInflate();
                }
                this._z = null;
            }
        }

        private void finish()
        {
            if (this._z == null)
            {
                return;
            }
            if (this._streamMode != StreamMode.Writer)
            {
                if ((this._streamMode == StreamMode.Reader) && (this._flavor == ZlibStreamFlavor.GZIP))
                {
                    if (this._wantCompress)
                    {
                        throw new ZlibException("Reading with compression is not supported.");
                    }
                    if (this._z.TotalBytesOut == 0L)
                    {
                        return;
                    }
                    byte[] destinationArray = new byte[8];
                    if (this._z.AvailableBytesIn < 8)
                    {
                        Array.Copy(this._z.InputBuffer, this._z.NextIn, destinationArray, 0, this._z.AvailableBytesIn);
                        int count = 8 - this._z.AvailableBytesIn;
                        int num5 = this._stream.Read(destinationArray, this._z.AvailableBytesIn, count);
                        if (count != num5)
                        {
                            throw new ZlibException($"Missing or incomplete GZIP trailer. Expected 8 bytes, got {this._z.AvailableBytesIn + num5}.");
                        }
                    }
                    else
                    {
                        Array.Copy(this._z.InputBuffer, this._z.NextIn, destinationArray, 0, destinationArray.Length);
                    }
                    int num6 = BitConverter.ToInt32(destinationArray, 0);
                    int num7 = this.crc.Crc32Result;
                    int num8 = BitConverter.ToInt32(destinationArray, 4);
                    int num9 = (int) (((ulong) this._z.TotalBytesOut) & 0xffffffffL);
                    if (num7 != num6)
                    {
                        throw new ZlibException($"Bad CRC32 in GZIP trailer. (actual({num7:X8})!=expected({num6:X8}))");
                    }
                    if (num9 != num8)
                    {
                        throw new ZlibException($"Bad size in GZIP trailer. (actual({num9})!=expected({num8}))");
                    }
                }
                return;
            }
            bool flag = false;
        Label_0019:
            this._z.OutputBuffer = this.workingBuffer;
            this._z.NextOut = 0;
            this._z.AvailableBytesOut = this._workingBuffer.Length;
            int num = !this._wantCompress ? this._z.Inflate(FlushType.Finish) : this._z.Deflate(FlushType.Finish);
            switch (num)
            {
                case 1:
                case 0:
                    if ((this._workingBuffer.Length - this._z.AvailableBytesOut) > 0)
                    {
                        this._stream.Write(this._workingBuffer, 0, this._workingBuffer.Length - this._z.AvailableBytesOut);
                    }
                    flag = (this._z.AvailableBytesIn == 0) && (this._z.AvailableBytesOut != 0);
                    if ((this._flavor == ZlibStreamFlavor.GZIP) && !this._wantCompress)
                    {
                        flag = (this._z.AvailableBytesIn == 8) && (this._z.AvailableBytesOut != 0);
                    }
                    if (flag)
                    {
                        this.Flush();
                        if (this._flavor == ZlibStreamFlavor.GZIP)
                        {
                            if (!this._wantCompress)
                            {
                                throw new ZlibException("Writing with decompression is not supported.");
                            }
                            int num2 = this.crc.Crc32Result;
                            this._stream.Write(BitConverter.GetBytes(num2), 0, 4);
                            int num3 = (int) (((ulong) this.crc.TotalBytesRead) & 0xffffffffL);
                            this._stream.Write(BitConverter.GetBytes(num3), 0, 4);
                        }
                        break;
                    }
                    goto Label_0019;

                default:
                {
                    string str = (!this._wantCompress ? "in" : "de") + "flating";
                    if (this._z.Message == null)
                    {
                        throw new ZlibException($"{str}: (rc = {num})");
                    }
                    throw new ZlibException(str + ": " + this._z.Message);
                }
            }
        }

        public override void Flush()
        {
            this._stream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (this._streamMode == StreamMode.Undefined)
            {
                if (!this._stream.CanRead)
                {
                    throw new ZlibException("The stream is not readable.");
                }
                this._streamMode = StreamMode.Reader;
                this.z.AvailableBytesIn = 0;
                if (this._flavor == ZlibStreamFlavor.GZIP)
                {
                    this._gzipHeaderByteCount = this._ReadAndValidateGzipHeader();
                    if (this._gzipHeaderByteCount == 0)
                    {
                        return 0;
                    }
                }
            }
            if (this._streamMode != StreamMode.Reader)
            {
                throw new ZlibException("Cannot Read after Writing.");
            }
            if (count == 0)
            {
                return 0;
            }
            if (this.nomoreinput && this._wantCompress)
            {
                return 0;
            }
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            if (offset < buffer.GetLowerBound(0))
            {
                throw new ArgumentOutOfRangeException("offset");
            }
            if ((offset + count) > buffer.GetLength(0))
            {
                throw new ArgumentOutOfRangeException("count");
            }
            int num = 0;
            this._z.OutputBuffer = buffer;
            this._z.NextOut = offset;
            this._z.AvailableBytesOut = count;
            this._z.InputBuffer = this.workingBuffer;
            do
            {
                if ((this._z.AvailableBytesIn == 0) && !this.nomoreinput)
                {
                    this._z.NextIn = 0;
                    this._z.AvailableBytesIn = this._stream.Read(this._workingBuffer, 0, this._workingBuffer.Length);
                    if (this._z.AvailableBytesIn == 0)
                    {
                        this.nomoreinput = true;
                    }
                }
                num = !this._wantCompress ? this._z.Inflate(this._flushMode) : this._z.Deflate(this._flushMode);
                if (this.nomoreinput && (num == -5))
                {
                    return 0;
                }
                if ((num != 0) && (num != 1))
                {
                    throw new ZlibException($"{!this._wantCompress ? "in" : "de"}flating:  rc={num}  msg={this._z.Message}");
                }
            }
            while (((!this.nomoreinput && (num != 1)) || (this._z.AvailableBytesOut != count)) && (((this._z.AvailableBytesOut > 0) && !this.nomoreinput) && (num == 0)));
            if (this._z.AvailableBytesOut > 0)
            {
                if ((num == 0) && (this._z.AvailableBytesIn == 0))
                {
                }
                if (this.nomoreinput && this._wantCompress)
                {
                    num = this._z.Deflate(FlushType.Finish);
                    if ((num != 0) && (num != 1))
                    {
                        throw new ZlibException($"Deflating:  rc={num}  msg={this._z.Message}");
                    }
                }
            }
            num = count - this._z.AvailableBytesOut;
            if (this.crc != null)
            {
                this.crc.SlurpBlock(buffer, offset, num);
            }
            return num;
        }

        private string ReadZeroTerminatedString()
        {
            List<byte> list = new List<byte>();
            bool flag = false;
            do
            {
                if (this._stream.Read(this._buf1, 0, 1) != 1)
                {
                    throw new ZlibException("Unexpected EOF reading GZIP header.");
                }
                if (this._buf1[0] == 0)
                {
                    flag = true;
                }
                else
                {
                    list.Add(this._buf1[0]);
                }
            }
            while (!flag);
            byte[] bytes = list.ToArray();
            return GZipStream.iso8859dash1.GetString(bytes, 0, bytes.Length);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            this._stream.SetLength(value);
            this.nomoreinput = false;
        }

        public static byte[] UncompressBuffer(byte[] compressed, Stream decompressor)
        {
            byte[] buffer = new byte[0x400];
            using (MemoryStream stream = new MemoryStream())
            {
                using (decompressor)
                {
                    int num;
                    while ((num = decompressor.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        stream.Write(buffer, 0, num);
                    }
                }
                return stream.ToArray();
            }
        }

        public static string UncompressString(byte[] compressed, Stream decompressor)
        {
            byte[] buffer = new byte[0x400];
            Encoding encoding = Encoding.UTF8;
            using (MemoryStream stream = new MemoryStream())
            {
                using (decompressor)
                {
                    int num;
                    while ((num = decompressor.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        stream.Write(buffer, 0, num);
                    }
                }
                stream.Seek(0L, SeekOrigin.Begin);
                StreamReader reader = new StreamReader(stream, encoding);
                return reader.ReadToEnd();
            }
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (this.crc != null)
            {
                this.crc.SlurpBlock(buffer, offset, count);
            }
            if (this._streamMode == StreamMode.Undefined)
            {
                this._streamMode = StreamMode.Writer;
            }
            else if (this._streamMode != StreamMode.Writer)
            {
                throw new ZlibException("Cannot Write after Reading.");
            }
            if (count != 0)
            {
                this.z.InputBuffer = buffer;
                this._z.NextIn = offset;
                this._z.AvailableBytesIn = count;
                bool flag = false;
                do
                {
                    this._z.OutputBuffer = this.workingBuffer;
                    this._z.NextOut = 0;
                    this._z.AvailableBytesOut = this._workingBuffer.Length;
                    int num = !this._wantCompress ? this._z.Inflate(this._flushMode) : this._z.Deflate(this._flushMode);
                    if ((num != 0) && (num != 1))
                    {
                        throw new ZlibException((!this._wantCompress ? "in" : "de") + "flating: " + this._z.Message);
                    }
                    this._stream.Write(this._workingBuffer, 0, this._workingBuffer.Length - this._z.AvailableBytesOut);
                    flag = (this._z.AvailableBytesIn == 0) && (this._z.AvailableBytesOut != 0);
                    if ((this._flavor == ZlibStreamFlavor.GZIP) && !this._wantCompress)
                    {
                        flag = (this._z.AvailableBytesIn == 8) && (this._z.AvailableBytesOut != 0);
                    }
                }
                while (!flag);
            }
        }

        internal int Crc32 =>
            this.crc?.Crc32Result;

        protected internal bool _wantCompress =>
            (this._compressionMode == CompressionMode.Compress);

        private ZlibCodec z
        {
            get
            {
                if (this._z == null)
                {
                    bool flag = this._flavor == ZlibStreamFlavor.ZLIB;
                    this._z = new ZlibCodec();
                    if (this._compressionMode == CompressionMode.Decompress)
                    {
                        this._z.InitializeInflate(this.windowBitsMax, flag);
                    }
                    else
                    {
                        this._z.Strategy = this.Strategy;
                        this._z.InitializeDeflate(this._level, this.windowBitsMax, flag);
                    }
                }
                return this._z;
            }
        }

        private byte[] workingBuffer
        {
            get
            {
                if (this._workingBuffer == null)
                {
                    this._workingBuffer = new byte[this._bufferSize];
                }
                return this._workingBuffer;
            }
        }

        public override bool CanRead =>
            this._stream.CanRead;

        public override bool CanSeek =>
            this._stream.CanSeek;

        public override bool CanWrite =>
            this._stream.CanWrite;

        public override long Length =>
            this._stream.Length;

        public override long Position
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        internal enum StreamMode
        {
            Writer,
            Reader,
            Undefined
        }
    }
}

