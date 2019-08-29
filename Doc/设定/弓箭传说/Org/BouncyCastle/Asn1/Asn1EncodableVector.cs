namespace Org.BouncyCastle.Asn1
{
    using System;
    using System.Collections;
    using System.Reflection;

    public class Asn1EncodableVector : IEnumerable
    {
        private IList v = Platform.CreateArrayList();

        public Asn1EncodableVector(params Asn1Encodable[] v)
        {
            this.Add(v);
        }

        public void Add(params Asn1Encodable[] objs)
        {
            foreach (Asn1Encodable encodable in objs)
            {
                this.v.Add(encodable);
            }
        }

        public void AddOptional(params Asn1Encodable[] objs)
        {
            if (objs != null)
            {
                foreach (Asn1Encodable encodable in objs)
                {
                    if (encodable != null)
                    {
                        this.v.Add(encodable);
                    }
                }
            }
        }

        public static Asn1EncodableVector FromEnumerable(IEnumerable e)
        {
            Asn1EncodableVector vector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
            IEnumerator enumerator = e.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Asn1Encodable current = (Asn1Encodable) enumerator.Current;
                    Asn1Encodable[] objs = new Asn1Encodable[] { current };
                    vector.Add(objs);
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
            return vector;
        }

        [Obsolete("Use 'object[index]' syntax instead")]
        public Asn1Encodable Get(int index) => 
            this[index];

        public IEnumerator GetEnumerator() => 
            this.v.GetEnumerator();

        public Asn1Encodable this[int index] =>
            ((Asn1Encodable) this.v[index]);

        [Obsolete("Use 'Count' property instead")]
        public int Size =>
            this.v.Count;

        public int Count =>
            this.v.Count;
    }
}

