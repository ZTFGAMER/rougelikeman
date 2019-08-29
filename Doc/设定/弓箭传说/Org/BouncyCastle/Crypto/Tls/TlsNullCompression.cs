namespace Org.BouncyCastle.Crypto.Tls
{
    using System;
    using System.IO;

    public class TlsNullCompression : TlsCompression
    {
        public virtual Stream Compress(Stream output) => 
            output;

        public virtual Stream Decompress(Stream output) => 
            output;
    }
}

