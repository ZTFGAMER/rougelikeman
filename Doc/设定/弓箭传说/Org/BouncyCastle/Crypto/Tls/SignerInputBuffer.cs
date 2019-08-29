namespace Org.BouncyCastle.Crypto.Tls
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Utilities.IO;
    using System;
    using System.IO;

    internal class SignerInputBuffer : MemoryStream
    {
        internal void UpdateSigner(ISigner s)
        {
            this.WriteTo(new SigStream(s));
        }

        private class SigStream : BaseOutputStream
        {
            private readonly ISigner s;

            internal SigStream(ISigner s)
            {
                this.s = s;
            }

            public override void Write(byte[] buf, int off, int len)
            {
                this.s.BlockUpdate(buf, off, len);
            }

            public override void WriteByte(byte b)
            {
                this.s.Update(b);
            }
        }
    }
}

