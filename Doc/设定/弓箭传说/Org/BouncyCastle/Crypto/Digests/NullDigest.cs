namespace Org.BouncyCastle.Crypto.Digests
{
    using Org.BouncyCastle.Crypto;
    using System;
    using System.IO;

    public class NullDigest : IDigest
    {
        private readonly MemoryStream bOut = new MemoryStream();

        public void BlockUpdate(byte[] inBytes, int inOff, int len)
        {
            this.bOut.Write(inBytes, inOff, len);
        }

        public int DoFinal(byte[] outBytes, int outOff)
        {
            byte[] buffer = this.bOut.ToArray();
            buffer.CopyTo(outBytes, outOff);
            this.Reset();
            return buffer.Length;
        }

        public int GetByteLength() => 
            0;

        public int GetDigestSize() => 
            ((int) this.bOut.Length);

        public void Reset()
        {
            this.bOut.SetLength(0L);
        }

        public void Update(byte b)
        {
            this.bOut.WriteByte(b);
        }

        public string AlgorithmName =>
            "NULL";
    }
}

