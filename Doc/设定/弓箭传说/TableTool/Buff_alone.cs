namespace TableTool
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Buff_alone : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <BuffID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Notes>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <FxId>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <OverType>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <BuffType>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <DizzyChance>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Attribute>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <FirstEffects>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <Effects>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <Attributes>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float[] <Args>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <ArgsContent>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Time>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Delay_time>k__BackingField;

        public Buff_alone Copy() => 
            new Buff_alone { 
                BuffID = this.BuffID,
                Notes = this.Notes,
                FxId = this.FxId,
                OverType = this.OverType,
                BuffType = this.BuffType,
                DizzyChance = this.DizzyChance,
                Attribute = this.Attribute,
                FirstEffects = this.FirstEffects,
                Effects = this.Effects,
                Attributes = this.Attributes,
                Args = this.Args,
                ArgsContent = this.ArgsContent,
                Time = this.Time,
                Delay_time = this.Delay_time
            };

        protected override bool ReadImpl()
        {
            this.BuffID = base.readInt();
            this.Notes = base.readLocalString();
            this.FxId = base.readInt();
            this.OverType = base.readInt();
            this.BuffType = base.readInt();
            this.DizzyChance = base.readInt();
            this.Attribute = base.readLocalString();
            this.FirstEffects = base.readArraystring();
            this.Effects = base.readArraystring();
            this.Attributes = base.readArraystring();
            this.Args = base.readArrayfloat();
            this.ArgsContent = base.readLocalString();
            this.Time = base.readInt();
            this.Delay_time = base.readInt();
            return true;
        }

        public int BuffID { get; private set; }

        public string Notes { get; private set; }

        public int FxId { get; private set; }

        public int OverType { get; private set; }

        public int BuffType { get; private set; }

        public int DizzyChance { get; private set; }

        public string Attribute { get; private set; }

        public string[] FirstEffects { get; private set; }

        public string[] Effects { get; private set; }

        public string[] Attributes { get; private set; }

        public float[] Args { get; private set; }

        public string ArgsContent { get; private set; }

        public int Time { get; private set; }

        public int Delay_time { get; private set; }
    }
}

