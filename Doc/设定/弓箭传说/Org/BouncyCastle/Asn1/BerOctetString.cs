namespace Org.BouncyCastle.Asn1
{
    using Org.BouncyCastle.Utilities;
    using System;
    using System.Collections;
    using System.IO;

    public class BerOctetString : DerOctetString, IEnumerable
    {
        private const int MaxLength = 0x3e8;
        private readonly IEnumerable octs;

        public BerOctetString(byte[] str) : base(str)
        {
        }

        public BerOctetString(Asn1Encodable obj) : base(obj.ToAsn1Object())
        {
        }

        public BerOctetString(Asn1Object obj) : base(obj)
        {
        }

        public BerOctetString(IEnumerable octets) : base(ToBytes(octets))
        {
            this.octs = octets;
        }

        internal override void Encode(DerOutputStream derOut)
        {
            if ((derOut is Asn1OutputStream) || (derOut is BerOutputStream))
            {
                derOut.WriteByte(0x24);
                derOut.WriteByte(0x80);
                IEnumerator enumerator = this.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        DerOctetString current = (DerOctetString) enumerator.Current;
                        derOut.WriteObject((Asn1Object) current);
                    }
                }
                finally
                {
                    if (enumerator is IDisposable disposable)
                    {
                        IDisposable disposable;
                        disposable.Dispose();
                    }
                }
                derOut.WriteByte(0);
                derOut.WriteByte(0);
            }
            else
            {
                base.Encode(derOut);
            }
        }

        public static BerOctetString FromSequence(Asn1Sequence seq)
        {
            IList octets = Platform.CreateArrayList();
            IEnumerator enumerator = seq.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Asn1Encodable current = (Asn1Encodable) enumerator.Current;
                    octets.Add(current);
                }
            }
            finally
            {
                if (enumerator is IDisposable disposable)
                {
                    IDisposable disposable;
                    disposable.Dispose();
                }
            }
            return new BerOctetString(octets);
        }

        private IList GenerateOcts()
        {
            IList list = Platform.CreateArrayList();
            for (int i = 0; i < base.str.Length; i += 0x3e8)
            {
                byte[] destinationArray = new byte[Math.Min(base.str.Length, i + 0x3e8) - i];
                Array.Copy(base.str, i, destinationArray, 0, destinationArray.Length);
                list.Add(new DerOctetString(destinationArray));
            }
            return list;
        }

        public IEnumerator GetEnumerator() => 
            this.octs?.GetEnumerator();

        [Obsolete("Use GetEnumerator() instead")]
        public IEnumerator GetObjects() => 
            this.GetEnumerator();

        public override byte[] GetOctets() => 
            base.str;

        private static byte[] ToBytes(IEnumerable octs)
        {
            MemoryStream stream = new MemoryStream();
            IEnumerator enumerator = octs.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    byte[] octets = ((DerOctetString) enumerator.Current).GetOctets();
                    stream.Write(octets, 0, octets.Length);
                }
            }
            finally
            {
                if (enumerator is IDisposable disposable)
                {
                    IDisposable disposable;
                    disposable.Dispose();
                }
            }
            return stream.ToArray();
        }
    }
}

