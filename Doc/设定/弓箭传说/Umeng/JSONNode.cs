namespace Umeng
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;

    public abstract class JSONNode
    {
        internal static StringBuilder m_EscapeBuilder = new StringBuilder();

        protected JSONNode()
        {
        }

        public virtual void Add(JSONNode aItem)
        {
            this.Add(string.Empty, aItem);
        }

        public virtual void Add(string aKey, JSONNode aItem)
        {
        }

        public static JSONNode Deserialize(BinaryReader aReader)
        {
            JSONNodeType type = (JSONNodeType) aReader.ReadByte();
            switch (type)
            {
                case JSONNodeType.Array:
                {
                    int num = aReader.ReadInt32();
                    JSONArray array = new JSONArray();
                    for (int i = 0; i < num; i++)
                    {
                        array.Add(Deserialize(aReader));
                    }
                    return array;
                }
                case JSONNodeType.Object:
                {
                    int num3 = aReader.ReadInt32();
                    JSONObject obj2 = new JSONObject();
                    for (int i = 0; i < num3; i++)
                    {
                        string aKey = aReader.ReadString();
                        JSONNode aItem = Deserialize(aReader);
                        obj2.Add(aKey, aItem);
                    }
                    return obj2;
                }
                case JSONNodeType.String:
                    return new JSONString(aReader.ReadString());

                case JSONNodeType.Number:
                    return new JSONNumber(aReader.ReadDouble());

                case JSONNodeType.NullValue:
                    return new JSONNull();

                case JSONNodeType.Boolean:
                    return new JSONBool(aReader.ReadBoolean());
            }
            throw new Exception("Error deserializing JSON. Unknown tag: " + type);
        }

        public override bool Equals(object obj) => 
            object.ReferenceEquals(this, obj);

        internal static string Escape(string aText)
        {
            m_EscapeBuilder.Length = 0;
            if (m_EscapeBuilder.Capacity < (aText.Length + (aText.Length / 10)))
            {
                m_EscapeBuilder.Capacity = aText.Length + (aText.Length / 10);
            }
            foreach (char ch in aText)
            {
                switch (ch)
                {
                    case '\b':
                    {
                        m_EscapeBuilder.Append(@"\b");
                        continue;
                    }
                    case '\t':
                    {
                        m_EscapeBuilder.Append(@"\t");
                        continue;
                    }
                    case '\n':
                    {
                        m_EscapeBuilder.Append(@"\n");
                        continue;
                    }
                    case '\f':
                    {
                        m_EscapeBuilder.Append(@"\f");
                        continue;
                    }
                    case '\r':
                    {
                        m_EscapeBuilder.Append(@"\r");
                        continue;
                    }
                    default:
                    {
                        if (ch != '"')
                        {
                            if (ch != '\\')
                            {
                                break;
                            }
                            m_EscapeBuilder.Append(@"\\");
                        }
                        else
                        {
                            m_EscapeBuilder.Append("\"");
                        }
                        continue;
                    }
                }
                m_EscapeBuilder.Append(ch);
            }
            string str2 = m_EscapeBuilder.ToString();
            m_EscapeBuilder.Length = 0;
            return str2;
        }

        public override int GetHashCode() => 
            base.GetHashCode();

        public static JSONNode LoadFromBase64(string aBase64)
        {
            MemoryStream aData = new MemoryStream(Convert.FromBase64String(aBase64)) {
                Position = 0L
            };
            return LoadFromStream(aData);
        }

        public static JSONNode LoadFromCompressedBase64(string aBase64)
        {
            throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
        }

        public static JSONNode LoadFromCompressedFile(string aFileName)
        {
            throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
        }

        public static JSONNode LoadFromCompressedStream(Stream aData)
        {
            throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
        }

        public static JSONNode LoadFromFile(string aFileName)
        {
            using (FileStream stream = File.OpenRead(aFileName))
            {
                return LoadFromStream(stream);
            }
        }

        public static JSONNode LoadFromStream(Stream aData)
        {
            using (BinaryReader reader = new BinaryReader(aData))
            {
                return Deserialize(reader);
            }
        }

        public static bool operator ==(JSONNode a, object b)
        {
            if (object.ReferenceEquals(a, b))
            {
                return true;
            }
            bool flag = ((a is JSONNull) || object.ReferenceEquals(a, null)) || (a is JSONLazyCreator);
            bool flag2 = ((b is JSONNull) || object.ReferenceEquals(b, null)) || (b is JSONLazyCreator);
            return (flag && flag2);
        }

        public static implicit operator JSONNode(bool b) => 
            new JSONBool(b);

        public static implicit operator JSONNode(double n) => 
            new JSONNumber(n);

        public static implicit operator JSONNode(int n) => 
            new JSONNumber((double) n);

        public static implicit operator JSONNode(float n) => 
            new JSONNumber((double) n);

        public static implicit operator JSONNode(string s) => 
            new JSONString(s);

        public static implicit operator bool(JSONNode d) => 
            ((d != null) ? d.AsBool : false);

        public static implicit operator double(JSONNode d) => 
            ((d != null) ? d.AsDouble : 0.0);

        public static implicit operator int(JSONNode d) => 
            ((d != null) ? d.AsInt : 0);

        public static implicit operator float(JSONNode d) => 
            ((d != null) ? d.AsFloat : 0f);

        public static implicit operator string(JSONNode d) => 
            d?.Value;

        public static bool operator !=(JSONNode a, object b) => 
            (a != b);

        public static JSONNode Parse(string aJSON)
        {
            Stack<JSONNode> stack = new Stack<JSONNode>();
            JSONNode ctx = null;
            int num = 0;
            StringBuilder builder = new StringBuilder();
            string tokenName = string.Empty;
            bool flag = false;
            bool quoted = false;
            while (num < aJSON.Length)
            {
                char ch = aJSON[num];
                switch (ch)
                {
                    case '\t':
                        goto Label_026C;

                    case '\n':
                    case '\r':
                        goto Label_0368;

                    default:
                        switch (ch)
                        {
                            case '[':
                                if (!flag)
                                {
                                    goto Label_0115;
                                }
                                builder.Append(aJSON[num]);
                                goto Label_0368;

                            case '\\':
                                goto Label_0286;

                            case ']':
                                goto Label_0154;

                            default:
                                switch (ch)
                                {
                                    case ' ':
                                        goto Label_026C;

                                    case '"':
                                        flag ^= true;
                                        quoted |= flag;
                                        goto Label_0368;
                                }
                                break;
                        }
                        switch (ch)
                        {
                            case '{':
                                if (!flag)
                                {
                                    break;
                                }
                                builder.Append(aJSON[num]);
                                goto Label_0368;

                            case '}':
                                goto Label_0154;
                        }
                        switch (ch)
                        {
                            case ',':
                                if (flag)
                                {
                                    builder.Append(aJSON[num]);
                                }
                                else
                                {
                                    if (builder.Length > 0)
                                    {
                                        ParseElement(ctx, builder.ToString(), tokenName, quoted);
                                        quoted = false;
                                    }
                                    tokenName = string.Empty;
                                    builder.Length = 0;
                                    quoted = false;
                                }
                                goto Label_0368;

                            case ':':
                                if (flag)
                                {
                                    builder.Append(aJSON[num]);
                                }
                                else
                                {
                                    tokenName = builder.ToString().Trim();
                                    builder.Length = 0;
                                    quoted = false;
                                }
                                goto Label_0368;

                            default:
                                goto Label_0355;
                        }
                        break;
                }
                stack.Push(new JSONObject());
                if (ctx != null)
                {
                    ctx.Add(tokenName, stack.Peek());
                }
                tokenName = string.Empty;
                builder.Length = 0;
                ctx = stack.Peek();
                goto Label_0368;
            Label_0115:
                stack.Push(new JSONArray());
                if (ctx != null)
                {
                    ctx.Add(tokenName, stack.Peek());
                }
                tokenName = string.Empty;
                builder.Length = 0;
                ctx = stack.Peek();
                goto Label_0368;
            Label_0154:
                if (flag)
                {
                    builder.Append(aJSON[num]);
                }
                else
                {
                    if (stack.Count == 0)
                    {
                        throw new Exception("JSON Parse: Too many closing brackets");
                    }
                    stack.Pop();
                    if (builder.Length > 0)
                    {
                        ParseElement(ctx, builder.ToString(), tokenName, quoted);
                        quoted = false;
                    }
                    tokenName = string.Empty;
                    builder.Length = 0;
                    if (stack.Count > 0)
                    {
                        ctx = stack.Peek();
                    }
                }
                goto Label_0368;
            Label_026C:
                if (flag)
                {
                    builder.Append(aJSON[num]);
                }
                goto Label_0368;
            Label_0286:
                num++;
                if (!flag)
                {
                    goto Label_0368;
                }
                char ch2 = aJSON[num];
                switch (ch2)
                {
                    case 'r':
                        builder.Append('\r');
                        goto Label_0368;

                    case 't':
                        builder.Append('\t');
                        goto Label_0368;

                    case 'u':
                    {
                        string s = aJSON.Substring(num + 1, 4);
                        builder.Append((char) int.Parse(s, NumberStyles.AllowHexSpecifier));
                        num += 4;
                        goto Label_0368;
                    }
                    default:
                        switch (ch2)
                        {
                            case 'b':
                                builder.Append('\b');
                                goto Label_0368;

                            case 'f':
                                builder.Append('\f');
                                goto Label_0368;

                            case 'n':
                                builder.Append('\n');
                                goto Label_0368;

                            default:
                                builder.Append(ch2);
                                goto Label_0368;
                        }
                        break;
                }
            Label_0355:
                builder.Append(aJSON[num]);
            Label_0368:
                num++;
            }
            if (flag)
            {
                throw new Exception("JSON Parse: Quotation marks seems to be messed up.");
            }
            return ctx;
        }

        private static void ParseElement(JSONNode ctx, string token, string tokenName, bool quoted)
        {
            if (quoted)
            {
                ctx.Add(tokenName, token);
            }
            else
            {
                string str = token.ToLower();
                switch (str)
                {
                    case "false":
                    case "true":
                        ctx.Add(tokenName, str == "true");
                        break;

                    default:
                        if (str == "null")
                        {
                            ctx.Add(tokenName, null);
                        }
                        else if (double.TryParse(token, out double num))
                        {
                            ctx.Add(tokenName, num);
                        }
                        else
                        {
                            ctx.Add(tokenName, token);
                        }
                        break;
                }
            }
        }

        public virtual JSONNode Remove(int aIndex) => 
            null;

        public virtual JSONNode Remove(string aKey) => 
            null;

        public virtual JSONNode Remove(JSONNode aNode) => 
            aNode;

        public string SaveToBase64()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                this.SaveToStream(stream);
                stream.Position = 0L;
                return Convert.ToBase64String(stream.ToArray());
            }
        }

        public string SaveToCompressedBase64()
        {
            throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
        }

        public void SaveToCompressedFile(string aFileName)
        {
            throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
        }

        public void SaveToCompressedStream(Stream aData)
        {
            throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
        }

        public void SaveToFile(string aFileName)
        {
            Directory.CreateDirectory(new FileInfo(aFileName).Directory.FullName);
            using (FileStream stream = File.OpenWrite(aFileName))
            {
                this.SaveToStream(stream);
            }
        }

        public void SaveToStream(Stream aData)
        {
            BinaryWriter aWriter = new BinaryWriter(aData);
            this.Serialize(aWriter);
        }

        public virtual void Serialize(BinaryWriter aWriter)
        {
        }

        public override string ToString() => 
            "JSONNode";

        public virtual string ToString(string aIndent) => 
            this.ToString(aIndent, string.Empty);

        internal abstract string ToString(string aIndent, string aPrefix);

        public virtual JSONNode this[int aIndex]
        {
            get => 
                null;
            set
            {
            }
        }

        public virtual JSONNode this[string aKey]
        {
            get => 
                null;
            set
            {
            }
        }

        public virtual string Value
        {
            get => 
                string.Empty;
            set
            {
            }
        }

        public virtual int Count =>
            0;

        public virtual bool IsNumber =>
            false;

        public virtual bool IsString =>
            false;

        public virtual bool IsBoolean =>
            false;

        public virtual bool IsNull =>
            false;

        public virtual bool IsArray =>
            false;

        public virtual bool IsObject =>
            false;

        public virtual IEnumerable<JSONNode> Children =>
            new <>c__Iterator0 { $PC=-2 };

        public IEnumerable<JSONNode> DeepChildren =>
            new <>c__Iterator1 { 
                $this=this,
                $PC=-2
            };

        public abstract JSONNodeType Tag { get; }

        public virtual double AsDouble
        {
            get
            {
                double result = 0.0;
                if (double.TryParse(this.Value, out result))
                {
                    return result;
                }
                return 0.0;
            }
            set => 
                (this.Value = value.ToString());
        }

        public virtual int AsInt
        {
            get => 
                ((int) this.AsDouble);
            set => 
                (this.AsDouble = value);
        }

        public virtual float AsFloat
        {
            get => 
                ((float) this.AsDouble);
            set => 
                (this.AsDouble = value);
        }

        public virtual bool AsBool
        {
            get
            {
                bool result = false;
                if (bool.TryParse(this.Value, out result))
                {
                    return result;
                }
                return !string.IsNullOrEmpty(this.Value);
            }
            set => 
                (this.Value = !value ? "false" : "true");
        }

        public virtual JSONArray AsArray =>
            (this as JSONArray);

        public virtual JSONObject AsObject =>
            (this as JSONObject);

        [CompilerGenerated]
        private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<JSONNode>, IEnumerator, IDisposable, IEnumerator<JSONNode>
        {
            internal JSONNode $current;
            internal bool $disposing;
            internal int $PC;

            [DebuggerHidden]
            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                this.$PC = -1;
                if (this.$PC == 0)
                {
                }
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
                return new JSONNode.<>c__Iterator0();
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
        private sealed class <>c__Iterator1 : IEnumerable, IEnumerable<JSONNode>, IEnumerator, IDisposable, IEnumerator<JSONNode>
        {
            internal IEnumerator<JSONNode> $locvar0;
            internal JSONNode <C>__1;
            internal IEnumerator<JSONNode> $locvar1;
            internal JSONNode <D>__2;
            internal JSONNode $this;
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
                            try
                            {
                            }
                            finally
                            {
                                if (this.$locvar1 != null)
                                {
                                    this.$locvar1.Dispose();
                                }
                            }
                        }
                        finally
                        {
                            if (this.$locvar0 != null)
                            {
                                this.$locvar0.Dispose();
                            }
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
                        this.$locvar0 = this.$this.Children.GetEnumerator();
                        num = 0xfffffffd;
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_0122;
                }
                try
                {
                    switch (num)
                    {
                        case 1:
                            goto Label_0077;
                    }
                    while (this.$locvar0.MoveNext())
                    {
                        this.<C>__1 = this.$locvar0.Current;
                        this.$locvar1 = this.<C>__1.DeepChildren.GetEnumerator();
                        num = 0xfffffffd;
                    Label_0077:
                        try
                        {
                            while (this.$locvar1.MoveNext())
                            {
                                this.<D>__2 = this.$locvar1.Current;
                                this.$current = this.<D>__2;
                                if (!this.$disposing)
                                {
                                    this.$PC = 1;
                                }
                                flag = true;
                                return true;
                            }
                            continue;
                        }
                        finally
                        {
                            if (!flag)
                            {
                            }
                            if (this.$locvar1 != null)
                            {
                                this.$locvar1.Dispose();
                            }
                        }
                    }
                }
                finally
                {
                    if (!flag)
                    {
                    }
                    if (this.$locvar0 != null)
                    {
                        this.$locvar0.Dispose();
                    }
                }
                this.$PC = -1;
            Label_0122:
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
                return new JSONNode.<>c__Iterator1 { $this = this.$this };
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator() => 
                this.System.Collections.Generic.IEnumerable<Umeng.JSONNode>.GetEnumerator();

            JSONNode IEnumerator<JSONNode>.Current =>
                this.$current;

            object IEnumerator.Current =>
                this.$current;
        }
    }
}

