namespace Umeng
{
    using System;
    using System.IO;

    public class JSONString : JSONNode
    {
        private string m_Data;

        public JSONString(string aData)
        {
            this.m_Data = aData;
        }

        public override void Serialize(BinaryWriter aWriter)
        {
            aWriter.Write((byte) 3);
            aWriter.Write(this.m_Data);
        }

        public override string ToString() => 
            ("\"" + JSONNode.Escape(this.m_Data) + "\"");

        internal override string ToString(string aIndent, string aPrefix) => 
            ("\"" + JSONNode.Escape(this.m_Data) + "\"");

        public override JSONNodeType Tag =>
            JSONNodeType.String;

        public override bool IsString =>
            true;

        public override string Value
        {
            get => 
                this.m_Data;
            set => 
                (this.m_Data = value);
        }
    }
}

