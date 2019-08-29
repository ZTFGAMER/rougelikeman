namespace Umeng
{
    using System;
    using System.IO;

    public class JSONBool : JSONNode
    {
        private bool m_Data;

        public JSONBool(bool aData)
        {
            this.m_Data = aData;
        }

        public JSONBool(string aData)
        {
            this.Value = aData;
        }

        public override void Serialize(BinaryWriter aWriter)
        {
            aWriter.Write((byte) 6);
            aWriter.Write(this.m_Data);
        }

        public override string ToString() => 
            (!this.m_Data ? "false" : "true");

        internal override string ToString(string aIndent, string aPrefix) => 
            (!this.m_Data ? "false" : "true");

        public override JSONNodeType Tag =>
            JSONNodeType.Boolean;

        public override bool IsBoolean =>
            true;

        public override string Value
        {
            get => 
                this.m_Data.ToString();
            set
            {
                if (bool.TryParse(value, out bool flag))
                {
                    this.m_Data = flag;
                }
            }
        }

        public override bool AsBool
        {
            get => 
                this.m_Data;
            set => 
                (this.m_Data = value);
        }
    }
}

