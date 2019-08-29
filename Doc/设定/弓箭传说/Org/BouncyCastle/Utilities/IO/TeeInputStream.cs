namespace Org.BouncyCastle.Utilities.IO
{
    using Org.BouncyCastle.Utilities;
    using System;
    using System.IO;

    public class TeeInputStream : BaseInputStream
    {
        private readonly Stream input;
        private readonly Stream tee;

        public TeeInputStream(Stream input, Stream tee)
        {
            this.input = input;
            this.tee = tee;
        }

        public override void Close()
        {
            Platform.Dispose(this.input);
            Platform.Dispose(this.tee);
            base.Close();
        }

        public override int Read(byte[] buf, int off, int len)
        {
            int count = this.input.Read(buf, off, len);
            if (count > 0)
            {
                this.tee.Write(buf, off, count);
            }
            return count;
        }

        public override int ReadByte()
        {
            int num = this.input.ReadByte();
            if (num >= 0)
            {
                this.tee.WriteByte((byte) num);
            }
            return num;
        }
    }
}

