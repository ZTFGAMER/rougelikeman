namespace Umeng
{
    using System;
    using System.IO;

    public class JSONNumber : JSONNode
    {
        private double m_Data;

        public JSONNumber(double aData)
        {
            this.m_Data = aData;
        }

        public JSONNumber(string aData)
        {
            this.Value = aData;
        }

        public override void Serialize(BinaryWriter aWriter)
        {
            aWriter.Write((byte) 4);
            aWriter.Write(this.m_Data);
        }

        public override string ToString() => 
            this.m_Data.ToString();

        internal override string ToString(string aIndent, string aPrefix) => 
            this.m_Data.ToString();

        public override JSONNodeType Tag =>
            JSONNodeType.Number;

        public override bool IsNumber =>
            true;

        public override string Value
        {
            get => 
                this.m_Data.ToString();
            set
            {
                if (double.TryParse(value, out double num))
                {
                    this.m_Data = num;
                }
            }
        }

        public override double AsDouble
        {
            get => 
                this.m_Data;
            set => 
                (this.m_Data = value);
        }
    }
}

