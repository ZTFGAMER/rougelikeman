namespace Org.BouncyCastle.Utilities.Collections
{
    using System;
    using System.Collections;

    public sealed class EmptyEnumerator : IEnumerator
    {
        public static readonly IEnumerator Instance = new EmptyEnumerator();

        private EmptyEnumerator()
        {
        }

        public bool MoveNext() => 
            false;

        public void Reset()
        {
        }

        public object Current
        {
            get
            {
                throw new InvalidOperationException("No elements");
            }
        }
    }
}

