namespace Umeng
{
    using System;
    using System.Reflection;

    internal class JSONLazyCreator : JSONNode
    {
        private JSONNode m_Node;
        private string m_Key;

        public JSONLazyCreator(JSONNode aNode)
        {
            this.m_Node = aNode;
            this.m_Key = null;
        }

        public JSONLazyCreator(JSONNode aNode, string aKey)
        {
            this.m_Node = aNode;
            this.m_Key = aKey;
        }

        public override void Add(JSONNode aItem)
        {
            JSONArray aVal = new JSONArray {
                aItem
            };
            this.Set(aVal);
        }

        public override void Add(string aKey, JSONNode aItem)
        {
            JSONObject aVal = new JSONObject {
                { 
                    aKey,
                    aItem
                }
            };
            this.Set(aVal);
        }

        public override bool Equals(object obj) => 
            ((obj == null) || object.ReferenceEquals(this, obj));

        public override int GetHashCode() => 
            base.GetHashCode();

        public static bool operator ==(JSONLazyCreator a, object b) => 
            ((b == null) || object.ReferenceEquals(a, b));

        public static bool operator !=(JSONLazyCreator a, object b) => 
            (a != b);

        private void Set(JSONNode aVal)
        {
            if (this.m_Key == null)
            {
                this.m_Node.Add(aVal);
            }
            else
            {
                this.m_Node.Add(this.m_Key, aVal);
            }
            this.m_Node = null;
        }

        public override string ToString() => 
            string.Empty;

        internal override string ToString(string aIndent, string aPrefix) => 
            string.Empty;

        public override JSONNodeType Tag =>
            JSONNodeType.None;

        public override JSONNode this[int aIndex]
        {
            get => 
                new JSONLazyCreator(this);
            set
            {
                JSONArray aVal = new JSONArray {
                    value
                };
                this.Set(aVal);
            }
        }

        public override JSONNode this[string aKey]
        {
            get => 
                new JSONLazyCreator(this, aKey);
            set
            {
                JSONObject aVal = new JSONObject {
                    { 
                        aKey,
                        value
                    }
                };
                this.Set(aVal);
            }
        }

        public override int AsInt
        {
            get
            {
                JSONNumber aVal = new JSONNumber(0.0);
                this.Set(aVal);
                return 0;
            }
            set
            {
                JSONNumber aVal = new JSONNumber((double) value);
                this.Set(aVal);
            }
        }

        public override float AsFloat
        {
            get
            {
                JSONNumber aVal = new JSONNumber(0.0);
                this.Set(aVal);
                return 0f;
            }
            set
            {
                JSONNumber aVal = new JSONNumber((double) value);
                this.Set(aVal);
            }
        }

        public override double AsDouble
        {
            get
            {
                JSONNumber aVal = new JSONNumber(0.0);
                this.Set(aVal);
                return 0.0;
            }
            set
            {
                JSONNumber aVal = new JSONNumber(value);
                this.Set(aVal);
            }
        }

        public override bool AsBool
        {
            get
            {
                JSONBool aVal = new JSONBool(false);
                this.Set(aVal);
                return false;
            }
            set
            {
                JSONBool aVal = new JSONBool(value);
                this.Set(aVal);
            }
        }

        public override JSONArray AsArray
        {
            get
            {
                JSONArray aVal = new JSONArray();
                this.Set(aVal);
                return aVal;
            }
        }

        public override JSONObject AsObject
        {
            get
            {
                JSONObject aVal = new JSONObject();
                this.Set(aVal);
                return aVal;
            }
        }
    }
}

