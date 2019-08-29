namespace Org.BouncyCastle.Utilities.Collections
{
    using System;
    using System.Collections;

    public sealed class EmptyEnumerable : IEnumerable
    {
        public static readonly IEnumerable Instance = new EmptyEnumerable();

        private EmptyEnumerable()
        {
        }

        public IEnumerator GetEnumerator() => 
            EmptyEnumerator.Instance;
    }
}

