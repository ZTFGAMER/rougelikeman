namespace Umeng
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class JSONArray : JSONNode, IEnumerable
    {
        private List<JSONNode> m_List = new List<JSONNode>();

        public override void Add(string aKey, JSONNode aItem)
        {
            if (aItem == null)
            {
                aItem = new JSONNull();
            }
            this.m_List.Add(aItem);
        }

        [DebuggerHidden]
        public IEnumerator GetEnumerator() => 
            new <GetEnumerator>c__Iterator1 { $this = this };

        public override JSONNode Remove(int aIndex)
        {
            if ((aIndex < 0) || (aIndex >= this.m_List.Count))
            {
                return null;
            }
            JSONNode node = this.m_List[aIndex];
            this.m_List.RemoveAt(aIndex);
            return node;
        }

        public override JSONNode Remove(JSONNode aNode)
        {
            this.m_List.Remove(aNode);
            return aNode;
        }

        public override void Serialize(BinaryWriter aWriter)
        {
            aWriter.Write((byte) 1);
            aWriter.Write(this.m_List.Count);
            for (int i = 0; i < this.m_List.Count; i++)
            {
                this.m_List[i].Serialize(aWriter);
            }
        }

        public override string ToString()
        {
            string str = "[ ";
            foreach (JSONNode node in this.m_List)
            {
                if (str.Length > 2)
                {
                    str = str + ", ";
                }
                str = str + node.ToString();
            }
            return (str + " ]");
        }

        internal override string ToString(string aIndent, string aPrefix)
        {
            string str = "[ ";
            foreach (JSONNode node in this.m_List)
            {
                if (str.Length > 3)
                {
                    str = str + ", ";
                }
                string str2 = str;
                string[] textArray1 = new string[] { str2, "\n", aPrefix, aIndent, node.ToString(aIndent, aPrefix + aIndent) };
                str = string.Concat(textArray1);
            }
            return (str + "\n" + aPrefix + "]");
        }

        public override JSONNodeType Tag =>
            JSONNodeType.Array;

        public override bool IsArray =>
            true;

        public override JSONNode this[int aIndex]
        {
            get
            {
                if ((aIndex >= 0) && (aIndex < this.m_List.Count))
                {
                    return this.m_List[aIndex];
                }
                return new JSONLazyCreator(this);
            }
            set
            {
                if (value == null)
                {
                    value = new JSONNull();
                }
                if ((aIndex < 0) || (aIndex >= this.m_List.Count))
                {
                    this.m_List.Add(value);
                }
                else
                {
                    this.m_List[aIndex] = value;
                }
            }
        }

        public override JSONNode this[string aKey]
        {
            get => 
                new JSONLazyCreator(this);
            set
            {
                if (value == null)
                {
                    value = new JSONNull();
                }
                this.m_List.Add(value);
            }
        }

        public override int Count =>
            this.m_List.Count;

        public override IEnumerable<JSONNode> Children =>
            new <>c__Iterator0 { 
                $this=this,
                $PC=-2
            };

        [CompilerGenerated]
        private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<JSONNode>, IEnumerator, IDisposable, IEnumerator<JSONNode>
        {
            internal List<JSONNode>.Enumerator $locvar0;
            internal JSONNode <N>__1;
            internal JSONArray $this;
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
                        this.$locvar0 = this.$this.m_List.GetEnumerator();
                        num = 0xfffffffd;
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_00B2;
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
            Label_00B2:
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
                return new JSONArray.<>c__Iterator0 { $this = this.$this };
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
            internal List<JSONNode>.Enumerator $locvar0;
            internal JSONNode <N>__1;
            internal JSONArray $this;
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
                        this.$locvar0 = this.$this.m_List.GetEnumerator();
                        num = 0xfffffffd;
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_00B2;
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
            Label_00B2:
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
    }
}

