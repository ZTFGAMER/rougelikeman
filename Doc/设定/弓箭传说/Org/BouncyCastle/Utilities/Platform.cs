namespace Org.BouncyCastle.Utilities
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.IO;
    using System.Security;

    internal abstract class Platform
    {
        private static readonly CompareInfo InvariantCompareInfo = CultureInfo.InvariantCulture.CompareInfo;
        internal static readonly string NewLine = GetNewLine();

        protected Platform()
        {
        }

        internal static IList CreateArrayList() => 
            new ArrayList();

        internal static IList CreateArrayList(ICollection collection) => 
            new ArrayList(collection);

        internal static IList CreateArrayList(IEnumerable collection)
        {
            ArrayList list = new ArrayList();
            IEnumerator enumerator = collection.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    object current = enumerator.Current;
                    list.Add(current);
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
            return list;
        }

        internal static IList CreateArrayList(int capacity) => 
            new ArrayList(capacity);

        internal static IDictionary CreateHashtable() => 
            new Hashtable();

        internal static IDictionary CreateHashtable(IDictionary dictionary) => 
            new Hashtable(dictionary);

        internal static IDictionary CreateHashtable(int capacity) => 
            new Hashtable(capacity);

        internal static Exception CreateNotImplementedException(string message) => 
            new NotImplementedException(message);

        internal static void Dispose(Stream s)
        {
            s.Close();
        }

        internal static void Dispose(TextWriter t)
        {
            t.Close();
        }

        internal static bool EndsWith(string source, string suffix) => 
            InvariantCompareInfo.IsSuffix(source, suffix, CompareOptions.Ordinal);

        internal static bool EqualsIgnoreCase(string a, string b) => 
            (string.Compare(a, b, StringComparison.OrdinalIgnoreCase) == 0);

        internal static string GetEnvironmentVariable(string variable)
        {
            try
            {
                return Environment.GetEnvironmentVariable(variable);
            }
            catch (SecurityException)
            {
                return null;
            }
        }

        private static string GetNewLine() => 
            Environment.NewLine;

        internal static string GetTypeName(object obj) => 
            obj.GetType().FullName;

        internal static int IndexOf(string source, string value) => 
            InvariantCompareInfo.IndexOf(source, value, CompareOptions.Ordinal);

        internal static int LastIndexOf(string source, string value) => 
            InvariantCompareInfo.LastIndexOf(source, value, CompareOptions.Ordinal);

        internal static bool StartsWith(string source, string prefix) => 
            InvariantCompareInfo.IsPrefix(source, prefix, CompareOptions.Ordinal);

        internal static string ToLowerInvariant(string s) => 
            s.ToLower(CultureInfo.InvariantCulture);

        internal static string ToUpperInvariant(string s) => 
            s.ToUpper(CultureInfo.InvariantCulture);
    }
}

