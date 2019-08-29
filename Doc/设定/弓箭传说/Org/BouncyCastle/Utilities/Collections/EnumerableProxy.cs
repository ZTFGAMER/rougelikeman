namespace Org.BouncyCastle.Utilities.Collections
{
    using System;
    using System.Collections;

    public sealed class EnumerableProxy : IEnumerable
    {
        private readonly IEnumerable inner;

        public EnumerableProxy(IEnumerable inner)
        {
            if (inner == null)
            {
                throw new ArgumentNullException("inner");
            }
            this.inner = inner;
        }

        public IEnumerator GetEnumerator() => 
            this.inner.GetEnumerator();
    }
}

