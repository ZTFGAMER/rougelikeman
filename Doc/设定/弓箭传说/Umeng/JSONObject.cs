namespace Umeng
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class JSONObject : JSONNode, IEnumerable
    {
        private Dictionary<string, JSONNode> m_Dict = new Dictionary<string, JSONNode>();

        public override void Add(string aKey, JSONNode aItem)
        {
            if (aItem == null)
            {
                aItem = new JSONNull();
            }
            if (!string.IsNullOrEmpty(aKey))
            {
                if (this.m_Dict.ContainsKey(aKey))
                {
                    this.m_Dict[aKey] = aItem;
                }
                else
                {
                    this.m_Dict.Add(aKey, aItem);
                }
            }
            else
            {
                this.m_Dict.Add(Guid.NewGuid().ToString(), aItem);
            }
        }

        [DebuggerHidden]
        public IEnumerator GetEnumerator() => 
            new <GetEnumerator>c__Iterator1 { $this = this };

        public override JSONNode Remove(int aIndex)
        {
            if ((aIndex < 0) || (aIndex >= this.m_Dict.Count))
            {
                return null;
            }
            KeyValuePair<string, JSONNode> pair = this.m_Dict.ElementAt<KeyValuePair<string, JSONNode>>(aIndex);
            this.m_Dict.Remove(pair.Key);
            return pair.Value;
        }

        public override JSONNode Remove(string aKey)
        {
            if (!this.m_Dict.ContainsKey(aKey))
            {
                return null;
            }
            JSONNode node = this.m_Dict[aKey];
            this.m_Dict.Remove(aKey);
            return node;
        }

        public override JSONNode Remove(JSONNode aNode)
        {
            <Remove>c__AnonStorey2 storey = new <Remove>c__AnonStorey2 {
                aNode = aNode
            };
            try
            {
                KeyValuePair<string, JSONNode> pair = this.m_Dict.Where<KeyValuePair<string, JSONNode>>(new Func<KeyValuePair<string, JSONNode>, bool>(storey.<>m__0)).First<KeyValuePair<string, JSONNode>>();
                this.m_Dict.Remove(pair.Key);
                return storey.aNode;
            }
            catch
            {
                return null;
            }
        }

        public override void Serialize(BinaryWriter aWriter)
        {
            aWriter.Write((byte) 2);
            aWriter.Write(this.m_Dict.Count);
            foreach (string str in this.m_Dict.Keys)
            {
                aWriter.Write(str);
                this.m_Dict[str].Serialize(aWriter);
            }
        }

        public override string ToString()
        {
            string str = "{";
            foreach (KeyValuePair<string, JSONNode> pair in this.m_Dict)
            {
                if (str.Length > 2)
                {
                    str = str + ", ";
                }
                string str2 = str;
                string[] textArray1 = new string[] { str2, "\"", JSONNode.Escape(pair.Key), "\":", pair.Value.ToString() };
                str = string.Concat(textArray1);
            }
            return (str + "}");
        }

        internal override string ToString(string aIndent, string aPrefix)
        {
            string str = "{ ";
            foreach (KeyValuePair<string, JSONNode> pair in this.m_Dict)
            {
                if (str.Length > 3)
                {
                    str = str + ", ";
                }
                string str2 = str;
                string[] textArray1 = new string[] { str2, "\n", aPrefix, aIndent, "\"", JSONNode.Escape(pair.Key), "\" : " };
                str = string.Concat(textArray1);
                str = str + pair.Value.ToString(aIndent, aPrefix + aIndent);
            }
            return (str + "\n" + aPrefix + "}");
        }

        public override JSONNodeType Tag =>
            JSONNodeType.Object;

        public override bool IsObject =>
            true;

        public override JSONNode this[string aKey]
        {
            get
            {
                if (this.m_Dict.ContainsKey(aKey))
                {
                    return this.m_Dict[aKey];
                }
                return new JSONLazyCreator(this, aKey);
            }
            set
            {
                if (value == null)
                {
                    value = new JSONNull();
                }
                if (this.m_Dict.ContainsKey(aKey))
                {
                    this.m_Dict[aKey] = value;
                }
                else
                {
                    this.m_Dict.Add(aKey, value);
                }
            }
        }

        public override JSONNode this[int aIndex]
        {
            get
            {
                if ((aIndex >= 0) && (aIndex < this.m_Dict.Count))
                {
                    return this.m_Dict.ElementAt<KeyValuePair<string, JSONNode>>(aIndex).Value;
                }
                return null;
            }
            set
            {
                if (value == null)
                {
                    value = new JSONNull();
                }
                if ((aIndex >= 0) && (aIndex < this.m_Dict.Count))
                {
                    string key = this.m_Dict.ElementAt<KeyValuePair<string, JSONNode>>(aIndex).Key;
                    this.m_Dict[key] = value;
                }
            }
        }

        public override int Count =>
            this.m_Dict.Count;

        public override IEnumerable<JSONNode> Children =>
            new <>c__Iterator0 { 
                $this=this,
                $PC=-2
            };

        [CompilerGenerated]
        private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<JSONNode>, IEnumerator, IDisposable, IEnumerator<JSONNode>
        {
            internal Dictionary<string, JSONNode>.Enumerator $locvar0;
            internal KeyValuePair<string, JSONNode> <N>__1;
            internal JSONObject $this;
            internal JSONNode $current;
            internal bool $disposing;
            internal int $PC;

            [DebuggerHidden]
            public void Dispose()
            {
                uint num = (uint) this.$PC;
                this.$disposing = true;
                this.$PC = -1;
                switch (num)
                {
                    case 1:
                        try
                        {
                        }
                        finally
                        {
                            this.$locvar0.Dispose();
                        }
                        break;
                }
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                bool flag = false;
                switch (num)
                {
                    case 0:
                        this.$locvar0 = this.$this.m_Dict.GetEnumerator();
                        num = 0xfffffffd;
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_00B7;
                }
                try
                {
                    while (this.$locvar0.MoveNext())
                    {
                        this.<N>__1 = this.$locvar0.Current;
                        this.$current = this.<N>__1.Value;
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        flag = true;
                        return true;
                    }
                }
                finally
                {
                    if (!flag)
                    {
                    }
                    this.$locvar0.Dispose();
                }
                this.$PC = -1;
            Label_00B7:
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            [DebuggerHidden]
            IEnumerator<JSONNode> IEnumerable<JSONNode>.GetEnumerator()
            {
                if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
                {
                    return this;
                }
                return new JSONObject.<>c__Iterator0 { $this = this.$this };
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator() => 
                this.System.Collections.Generic.IEnumerable<Umeng.JSONNode>.GetEnumerator();

            JSONNode IEnumerator<JSONNode>.Current =>
                this.$current;

            object IEnumerator.Current =>
                this.$current;
        }

        [CompilerGenerated]
        private sealed class <GetEnumerator>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal Dictionary<string, JSONNode>.Enumerator $locvar0;
            internal KeyValuePair<string, JSONNode> <N>__1;
            internal JSONObject $this;
            internal object $current;
            internal bool $disposing;
            internal int $PC;

            [DebuggerHidden]
            public void Dispose()
            {
                uint num = (uint) this.$PC;
                this.$disposing = true;
                this.$PC = -1;
                switch (num)
                {
                    case 1:
                        try
                        {
                        }
                        finally
                        {
                            this.$locvar0.Dispose();
                        }
                        break;
                }
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                bool flag = false;
                switch (num)
                {
                    case 0:
                        this.$locvar0 = this.$this.m_Dict.GetEnumerator();
                        num = 0xfffffffd;
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_00B7;
                }
                try
                {
                    while (this.$locvar0.MoveNext())
                    {
                        this.<N>__1 = this.$locvar0.Current;
                        this.$current = this.<N>__1;
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        flag = true;
                        return true;
                    }
                }
                finally
                {
                    if (!flag)
                    {
                    }
                    this.$locvar0.Dispose();
                }
                this.$PC = -1;
            Label_00B7:
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current =>
                this.$current;

            object IEnumerator.Current =>
                this.$current;
        }

        [CompilerGenerated]
        private sealed class <Remove>c__AnonStorey2
        {
            internal JSONNode aNode;

            internal bool <>m__0(KeyValuePair<string, JSONNode> k) => 
                (k.Value == this.aNode);
        }
    }
}

