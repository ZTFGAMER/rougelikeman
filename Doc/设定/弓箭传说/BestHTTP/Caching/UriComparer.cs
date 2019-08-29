namespace BestHTTP.Caching
{
    using System;
    using System.Collections.Generic;

    public sealed class UriComparer : IEqualityComparer<Uri>
    {
        public bool Equals(Uri x, Uri y) => 
            (Uri.Compare(x, y, UriComponents.HttpRequestUrl, UriFormat.SafeUnescaped, StringComparison.Ordinal) == 0);

        public int GetHashCode(Uri uri) => 
            uri.ToString().GetHashCode();
    }
}

