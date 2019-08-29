namespace Org.BouncyCastle.Utilities.Collections
{
    using System;
    using System.Collections;
    using System.Text;

    public abstract class CollectionUtilities
    {
        protected CollectionUtilities()
        {
        }

        public static void AddRange(IList to, IEnumerable range)
        {
            IEnumerator enumerator = range.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    object current = enumerator.Current;
                    to.Add(current);
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
        }

        public static bool CheckElementsAreOfType(IEnumerable e, Type t)
        {
            IEnumerator enumerator = e.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    object current = enumerator.Current;
                    if (!t.IsInstanceOfType(current))
                    {
                        return false;
                    }
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
            return true;
        }

        public static ISet ReadOnly(ISet s) => 
            s;

        public static IDictionary ReadOnly(IDictionary d) => 
            d;

        public static IList ReadOnly(IList l) => 
            l;

        public static string ToString(IEnumerable c)
        {
            StringBuilder builder = new StringBuilder("[");
            IEnumerator enumerator = c.GetEnumerator();
            if (enumerator.MoveNext())
            {
                builder.Append(enumerator.Current.ToString());
                while (enumerator.MoveNext())
                {
                    builder.Append(", ");
                    builder.Append(enumerator.Current.ToString());
                }
            }
            builder.Append(']');
            return builder.ToString();
        }
    }
}

