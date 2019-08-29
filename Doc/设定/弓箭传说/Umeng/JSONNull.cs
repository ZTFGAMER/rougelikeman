namespace Umeng
{
    using System;
    using System.IO;

    public class JSONNull : JSONNode
    {
        public override bool Equals(object obj) => 
            (object.ReferenceEquals(this, obj) || (obj is JSONNull));

        public override int GetHashCode() => 
            base.GetHashCode();

        public override void Serialize(BinaryWriter aWriter)
        {
            aWriter.Write((byte) 5);
        }

        public override string ToString() => 
            "null";

        internal override string ToString(string aIndent, string aPrefix) => 
            "null";

        public override JSONNodeType Tag =>
            JSONNodeType.NullValue;

        public override bool IsNull =>
            true;

        public override string Value
        {
            get => 
                "null";
            set
            {
            }
        }

        public override bool AsBool
        {
            get => 
                false;
            set
            {
            }
        }
    }
}

