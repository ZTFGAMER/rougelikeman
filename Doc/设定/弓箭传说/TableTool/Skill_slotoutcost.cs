namespace TableTool
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Skill_slotoutcost : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Id>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <UpperLimit>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <LowerLimit>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <CoinCost>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <TimeCost>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <NeedLevel>k__BackingField;

        public Skill_slotoutcost Copy() => 
            new Skill_slotoutcost { 
                Id = this.Id,
                UpperLimit = this.UpperLimit,
                LowerLimit = this.LowerLimit,
                CoinCost = this.CoinCost,
                TimeCost = this.TimeCost,
                NeedLevel = this.NeedLevel
            };

        protected override bool ReadImpl()
        {
            this.Id = base.readInt();
            this.UpperLimit = base.readInt();
            this.LowerLimit = base.readInt();
            this.CoinCost = base.readInt();
            this.TimeCost = base.readInt();
            this.NeedLevel = base.readInt();
            return true;
        }

        public int Id { get; private set; }

        public int UpperLimit { get; private set; }

        public int LowerLimit { get; private set; }

        public int CoinCost { get; private set; }

        public int TimeCost { get; private set; }

        public int NeedLevel { get; private set; }
    }
}

