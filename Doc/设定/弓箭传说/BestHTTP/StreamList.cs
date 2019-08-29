namespace BestHTTP
{
    using BestHTTP.Extensions;
    using System;
    using System.IO;

    internal sealed class StreamList : Stream
    {
        private Stream[] Streams;
        private int CurrentIdx;

        public StreamList(params Stream[] streams)
        {
            this.Streams = streams;
            this.CurrentIdx = 0;
        }

        protected override void Dispose(bool disposing)
        {
            for (int i = 0; i < this.Streams.Length; i++)
            {
                try
                {
                    this.Streams[i].Dispose();
                }
                catch (Exception exception)
                {
                    HTTPManager.Logger.Exception("StreamList", "Dispose", exception);
                }
            }
        }

        public override void Flush()
        {
            if (this.CurrentIdx < this.Streams.Length)
            {
                for (int i = 0; i <= this.CurrentIdx; i++)
                {
                    this.Streams[i].Flush();
                }
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (this.CurrentIdx >= this.Streams.Length)
            {
                return -1;
            }
            int num = this.Streams[this.CurrentIdx].Read(buffer, offset, count);
            while ((num < count) && (this.CurrentIdx++ < this.Streams.Length))
            {
                num += this.Streams[this.CurrentIdx].Read(buffer, offset + num, count - num);
            }
            return num;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            if (this.CurrentIdx >= this.Streams.Length)
            {
                return 0L;
            }
            return this.Streams[this.CurrentIdx].Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException("SetLength");
        }

        public void Write(string str)
        {
            byte[] aSCIIBytes = str.GetASCIIBytes();
            this.Write(aSCIIBytes, 0, aSCIIBytes.Length);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (this.CurrentIdx < this.Streams.Length)
            {
                this.Streams[this.CurrentIdx].Write(buffer, offset, count);
            }
        }

        public override bool CanRead
        {
            get
            {
                if (this.CurrentIdx >= this.Streams.Length)
                {
                    return false;
                }
                return this.Streams[this.CurrentIdx].CanRead;
            }
        }

        public override bool CanSeek =>
            false;

        public override bool CanWrite
        {
            get
            {
                if (this.CurrentIdx >= this.Streams.Length)
                {
                    return false;
                }
                return this.Streams[this.CurrentIdx].CanWrite;
            }
        }

        public override long Length
        {
            get
            {
                if (this.CurrentIdx >= this.Streams.Length)
                {
                    return 0L;
                }
                long num = 0L;
                for (int i = 0; i < this.Streams.Length; i++)
                {
                    num += this.Streams[i].Length;
                }
                return num;
            }
        }

        public override long Position
        {
            get
            {
                throw new NotImplementedException("Position get");
            }
            set
            {
                throw new NotImplementedException("Position set");
            }
        }
    }
}

