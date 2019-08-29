namespace Org.BouncyCastle.Asn1
{
    using Org.BouncyCastle.Utilities.IO;
    using System;
    using System.IO;

    internal class DefiniteLengthInputStream : LimitedInputStream
    {
        private static readonly byte[] EmptyBytes = new byte[0];
        private readonly int _originalLength;
        private int _remaining;

        internal DefiniteLengthInputStream(Stream inStream, int length) : base(inStream, length)
        {
            if (length < 0)
            {
                throw new ArgumentException("negative lengths not allowed", "length");
            }
            this._originalLength = length;
            this._remaining = length;
            if (length == 0)
            {
                this.SetParentEofDetect(true);
            }
        }

        public override int Read(byte[] buf, int off, int len)
        {
            if (this._remaining == 0)
            {
                return 0;
            }
            int count = Math.Min(len, this._remaining);
            int num2 = base._in.Read(buf, off, count);
            if (num2 < 1)
            {
                object[] objArray1 = new object[] { "DEF length ", this._originalLength, " object truncated by ", this._remaining };
                throw new EndOfStreamException(string.Concat(objArray1));
            }
            this._remaining -= num2;
            if (this._remaining == 0)
            {
                this.SetParentEofDetect(true);
            }
            return num2;
        }

        internal void ReadAllIntoByteArray(byte[] buf)
        {
            if (this._remaining != buf.Length)
            {
                throw new ArgumentException("buffer length not right for data");
            }
            this._remaining -= Streams.ReadFully(base._in, buf);
            if (this._remaining != 0)
            {
                object[] objArray1 = new object[] { "DEF length ", this._originalLength, " object truncated by ", this._remaining };
                throw new EndOfStreamException(string.Concat(objArray1));
            }
            this.SetParentEofDetect(true);
        }

        public override int ReadByte()
        {
            if (this._remaining == 0)
            {
                return -1;
            }
            int num = base._in.ReadByte();
            if (num < 0)
            {
                object[] objArray1 = new object[] { "DEF length ", this._originalLength, " object truncated by ", this._remaining };
                throw new EndOfStreamException(string.Concat(objArray1));
            }
            if (--this._remaining == 0)
            {
                this.SetParentEofDetect(true);
            }
            return num;
        }

        internal byte[] ToArray()
        {
            if (this._remaining == 0)
            {
                return EmptyBytes;
            }
            byte[] buf = new byte[this._remaining];
            this._remaining -= Streams.ReadFully(base._in, buf);
            if (this._remaining != 0)
            {
                object[] objArray1 = new object[] { "DEF length ", this._originalLength, " object truncated by ", this._remaining };
                throw new EndOfStreamException(string.Concat(objArray1));
            }
            this.SetParentEofDetect(true);
            return buf;
        }

        internal int Remaining =>
            this._remaining;
    }
}

