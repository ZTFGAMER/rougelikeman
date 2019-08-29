namespace Org.BouncyCastle.Asn1
{
    using Org.BouncyCastle.Utilities.IO;
    using System;
    using System.IO;

    internal class ConstructedOctetStream : BaseInputStream
    {
        private readonly Asn1StreamParser _parser;
        private bool _first = true;
        private Stream _currentStream;

        internal ConstructedOctetStream(Asn1StreamParser parser)
        {
            this._parser = parser;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (this._currentStream == null)
            {
                if (!this._first)
                {
                    return 0;
                }
                Asn1OctetStringParser parser = (Asn1OctetStringParser) this._parser.ReadObject();
                if (parser == null)
                {
                    return 0;
                }
                this._first = false;
                this._currentStream = parser.GetOctetStream();
            }
            int num = 0;
            while (true)
            {
                int num2 = this._currentStream.Read(buffer, offset + num, count - num);
                if (num2 > 0)
                {
                    num += num2;
                    if (num == count)
                    {
                        return num;
                    }
                }
                else
                {
                    Asn1OctetStringParser parser2 = (Asn1OctetStringParser) this._parser.ReadObject();
                    if (parser2 == null)
                    {
                        this._currentStream = null;
                        return num;
                    }
                    this._currentStream = parser2.GetOctetStream();
                }
            }
        }

        public override int ReadByte()
        {
            if (this._currentStream == null)
            {
                if (!this._first)
                {
                    return 0;
                }
                Asn1OctetStringParser parser = (Asn1OctetStringParser) this._parser.ReadObject();
                if (parser == null)
                {
                    return 0;
                }
                this._first = false;
                this._currentStream = parser.GetOctetStream();
            }
            while (true)
            {
                int num = this._currentStream.ReadByte();
                if (num >= 0)
                {
                    return num;
                }
                Asn1OctetStringParser parser2 = (Asn1OctetStringParser) this._parser.ReadObject();
                if (parser2 == null)
                {
                    this._currentStream = null;
                    return -1;
                }
                this._currentStream = parser2.GetOctetStream();
            }
        }
    }
}

