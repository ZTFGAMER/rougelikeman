namespace Org.BouncyCastle.Utilities.IO
{
    using Org.BouncyCastle.Utilities;
    using System;
    using System.IO;

    public class TeeOutputStream : BaseOutputStream
    {
        private readonly Stream output;
        private readonly Stream tee;

        public TeeOutputStream(Stream output, Stream tee)
        {
            this.output = output;
            this.tee = tee;
        }

        public override void Close()
        {
            Platform.Dispose(this.output);
            Platform.Dispose(this.tee);
            base.Close();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            this.output.Write(buffer, offset, count);
            this.tee.Write(buffer, offset, count);
        }

        public override void WriteByte(byte b)
        {
            this.output.WriteByte(b);
            this.tee.WriteByte(b);
        }
    }
}

