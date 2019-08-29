namespace Org.BouncyCastle.Crypto.Operators
{
    using Org.BouncyCastle.Crypto;
    using System;
    using System.IO;

    internal class SignerBucket : Stream
    {
        protected readonly ISigner signer;

        public SignerBucket(ISigner signer)
        {
            this.signer = signer;
        }

        public override void Flush()
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override int ReadByte()
        {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long length)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (count > 0)
            {
                this.signer.BlockUpdate(buffer, offset, count);
            }
        }

        public override void WriteByte(byte b)
        {
            this.signer.Update(b);
        }

        public override bool CanRead =>
            false;

        public override bool CanWrite =>
            true;

        public override bool CanSeek =>
            false;

        public override long Length =>
            0L;

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
    }
}

