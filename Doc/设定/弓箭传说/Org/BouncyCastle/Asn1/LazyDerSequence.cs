namespace Org.BouncyCastle.Asn1
{
    using System;
    using System.Collections;
    using System.Reflection;

    internal class LazyDerSequence : DerSequence
    {
        private byte[] encoded;

        internal LazyDerSequence(byte[] encoded)
        {
            this.encoded = encoded;
        }

        internal override void Encode(DerOutputStream derOut)
        {
            object obj2 = this;
            lock (obj2)
            {
                if (this.encoded == null)
                {
                    base.Encode(derOut);
                }
                else
                {
                    derOut.WriteEncoded(0x30, this.encoded);
                }
            }
        }

        public override IEnumerator GetEnumerator()
        {
            this.Parse();
            return base.GetEnumerator();
        }

        private void Parse()
        {
            object obj2 = this;
            lock (obj2)
            {
                if (this.encoded != null)
                {
                    Asn1Object obj3;
                    Asn1InputStream stream = new LazyAsn1InputStream(this.encoded);
                    while ((obj3 = stream.ReadObject()) != null)
                    {
                        base.AddObject(obj3);
                    }
                    this.encoded = null;
                }
            }
        }

        public override Asn1Encodable this[int index]
        {
            get
            {
                this.Parse();
                return base[index];
            }
        }

        public override int Count
        {
            get
            {
                this.Parse();
                return base.Count;
            }
        }
    }
}

