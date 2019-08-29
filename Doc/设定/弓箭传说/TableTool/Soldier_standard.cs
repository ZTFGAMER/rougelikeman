namespace TableTool
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Soldier_standard : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Level>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Integral_Up>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Integral_Down>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Standard_Attack>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Standard_HpMax>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <Coins_Ratio>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <Exp_Ratio>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <ScrollRate>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <ScrollRateBoss>k__BackingField;

        public Soldier_standard Copy() => 
            new Soldier_standard { 
                Level = this.Level,
                Integral_Up = this.Integral_Up,
                Integral_Down = this.Integral_Down,
                Standard_Attack = this.Standard_Attack,
                Standard_HpMax = this.Standard_HpMax,
                Coins_Ratio = this.Coins_Ratio,
                Exp_Ratio = this.Exp_Ratio,
                ScrollRate = this.ScrollRate,
                ScrollRateBoss = this.ScrollRateBoss
            };

        protected override bool ReadImpl()
        {
            this.Level = base.readInt();
            this.Integral_Up = base.readInt();
            this.Integral_Down = base.readInt();
            this.Standard_Attack = base.readInt();
            this.Standard_HpMax = base.readInt();
            this.Coins_Ratio = base.readFloat();
            this.Exp_Ratio = base.readFloat();
            this.ScrollRate = base.readArraystring();
            this.ScrollRateBoss = base.readArraystring();
            return true;
        }

        public int Level { get; private set; }

        public int Integral_Up { get; private set; }

        public int Integral_Down { get; private set; }

        public int Standard_Attack { get; private set; }

        public int Standard_HpMax { get; private set; }

        public float Coins_Ratio { get; private set; }

        public float Exp_Ratio { get; private set; }

        public string[] ScrollRate { get; private set; }

        public string[] ScrollRateBoss { get; private set; }
    }
}

