namespace BestHTTP.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public sealed class HeaderValue
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Key>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Value>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<HeaderValue> <Options>k__BackingField;
        [CompilerGenerated]
        private static Func<char, bool> <>f__am$cache0;

        public HeaderValue()
        {
        }

        public HeaderValue(string key)
        {
            this.Key = key;
        }

        public void Parse(string headerStr, ref int pos)
        {
            this.ParseImplementation(headerStr, ref pos, true);
        }

        private void ParseImplementation(string headerStr, ref int pos, bool isOptionIsAnOption)
        {
            int? nullable3;
            int? nullable5;
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = ch => ((ch != ';') && (ch != '=')) && (ch != ',');
            }
            string str = headerStr.Read(ref pos, <>f__am$cache0, true);
            this.Key = str;
            char? nullable = headerStr.Peek(pos - 1);
            bool flag = (!nullable.HasValue ? ((int?) null) : new int?(nullable.Value)) == 0x3d;
            for (bool flag2 = isOptionIsAnOption && (((nullable3 = !nullable.HasValue ? ((int?) (nullable3 = null)) : new int?(nullable.Value)).GetValueOrDefault() == 0x3b) && nullable3.HasValue); (nullable.HasValue && flag) || flag2; flag2 = isOptionIsAnOption && (((nullable5 = !nullable.HasValue ? ((int?) (nullable5 = null)) : new int?(nullable.Value)).GetValueOrDefault() == 0x3b) && nullable5.HasValue))
            {
                if (flag)
                {
                    string str2 = headerStr.ReadPossibleQuotedText(ref pos);
                    this.Value = str2;
                }
                else if (flag2)
                {
                    HeaderValue item = new HeaderValue();
                    item.ParseImplementation(headerStr, ref pos, false);
                    if (this.Options == null)
                    {
                        this.Options = new List<HeaderValue>();
                    }
                    this.Options.Add(item);
                }
                if (!isOptionIsAnOption)
                {
                    return;
                }
                nullable = headerStr.Peek(pos - 1);
                flag = (!nullable.HasValue ? ((int?) null) : new int?(nullable.Value)) == 0x3d;
            }
        }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(this.Value))
            {
                return (this.Key + '=' + this.Value);
            }
            return this.Key;
        }

        public bool TryGetOption(string key, out HeaderValue option)
        {
            option = null;
            if ((this.Options != null) && (this.Options.Count != 0))
            {
                for (int i = 0; i < this.Options.Count; i++)
                {
                    if (string.Equals(this.Options[i].Key, key, StringComparison.OrdinalIgnoreCase))
                    {
                        option = this.Options[i];
                        return true;
                    }
                }
            }
            return false;
        }

        public string Key { get; set; }

        public string Value { get; set; }

        public List<HeaderValue> Options { get; set; }

        public bool HasValue =>
            !string.IsNullOrEmpty(this.Value);
    }
}

