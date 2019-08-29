namespace Org.BouncyCastle.Crypto.Tls
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Utilities.IO;
    using System;
    using System.IO;

    internal class DigestInputBuffer : MemoryStream
    {
        internal void UpdateDigest(IDigest d)
        {
            this.WriteTo(new DigStream(d));
        }

        private class DigStream : BaseOutputStream
        {
            private readonly IDigest d;

            internal DigStream(IDigest d)
            {
                this.d = d;
            }

            public override void Write(byte[] buf, int off, int len)
            {
                this.d.BlockUpdate(buf, off, len);
            }

            public override void WriteByte(byte b)
            {
                this.d.Update(b);
            }
        }
    }
}

