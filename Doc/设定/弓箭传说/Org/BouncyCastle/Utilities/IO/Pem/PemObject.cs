namespace Org.BouncyCastle.Utilities.IO.Pem
{
    using Org.BouncyCastle.Utilities;
    using System;
    using System.Collections;

    public class PemObject : PemObjectGenerator
    {
        private string type;
        private IList headers;
        private byte[] content;

        public PemObject(string type, byte[] content) : this(type, Platform.CreateArrayList(), content)
        {
        }

        public PemObject(string type, IList headers, byte[] content)
        {
            this.type = type;
            this.headers = Platform.CreateArrayList((ICollection) headers);
            this.content = content;
        }

        public PemObject Generate() => 
            this;

        public string Type =>
            this.type;

        public IList Headers =>
            this.headers;

        public byte[] Content =>
            this.content;
    }
}

