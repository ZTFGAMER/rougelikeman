namespace TableTool
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Test_AttrValue : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <TypeId>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Notes>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <DeltaValue>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private long <Startlong>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <Test>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <Testfloat>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double <Testdouble>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private short <Testshort>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int[] <Testarrayint>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float[] <Testarrayfloat>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private short[] <Testarrayshort>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private long[] <Testarraylong>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double[] <Testarraydouble>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool[] <Testarraybool>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <Testarraystring>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Testint>k__BackingField;

        public Test_AttrValue Copy() => 
            new Test_AttrValue { 
                TypeId = this.TypeId,
                Notes = this.Notes,
                DeltaValue = this.DeltaValue,
                Startlong = this.Startlong,
                Test = this.Test,
                Testfloat = this.Testfloat,
                Testdouble = this.Testdouble,
                Testshort = this.Testshort,
                Testarrayint = this.Testarrayint,
                Testarrayfloat = this.Testarrayfloat,
                Testarrayshort = this.Testarrayshort,
                Testarraylong = this.Testarraylong,
                Testarraydouble = this.Testarraydouble,
                Testarraybool = this.Testarraybool,
                Testarraystring = this.Testarraystring,
                Testint = this.Testint
            };

        protected override bool ReadImpl()
        {
            this.TypeId = base.readLocalString();
            this.Notes = base.readLocalString();
            this.DeltaValue = base.readInt();
            this.Startlong = base.readLong();
            this.Test = base.readBool();
            this.Testfloat = base.readFloat();
            this.Testdouble = base.readDouble();
            this.Testshort = base.readShort();
            this.Testarrayint = base.readArrayint();
            this.Testarrayfloat = base.readArrayfloat();
            this.Testarrayshort = base.readArrayshort();
            this.Testarraylong = base.readArraylong();
            this.Testarraydouble = base.readArraydouble();
            this.Testarraybool = base.readArraybool();
            this.Testarraystring = base.readArraystring();
            this.Testint = base.readInt();
            return true;
        }

        public string TypeId { get; private set; }

        public string Notes { get; private set; }

        public int DeltaValue { get; private set; }

        public long Startlong { get; private set; }

        public bool Test { get; private set; }

        public float Testfloat { get; private set; }

        public double Testdouble { get; private set; }

        public short Testshort { get; private set; }

        public int[] Testarrayint { get; private set; }

        public float[] Testarrayfloat { get; private set; }

        public short[] Testarrayshort { get; private set; }

        public long[] Testarraylong { get; private set; }

        public double[] Testarraydouble { get; private set; }

        public bool[] Testarraybool { get; private set; }

        public string[] Testarraystring { get; private set; }

        public int Testint { get; private set; }
    }
}

