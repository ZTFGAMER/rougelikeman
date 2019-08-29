namespace BestHTTP.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class KeyValuePairList
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<HeaderValue> <Values>k__BackingField;

        public bool TryGet(string valueKeyName, out HeaderValue param)
        {
            param = null;
            for (int i = 0; i < this.Values.Count; i++)
            {
                if (string.CompareOrdinal(this.Values[i].Key, valueKeyName) == 0)
                {
                    param = this.Values[i];
                    return true;
                }
            }
            return false;
        }

        public List<HeaderValue> Values { get; protected set; }
    }
}

