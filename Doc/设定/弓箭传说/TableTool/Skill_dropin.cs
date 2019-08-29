namespace TableTool
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Skill_dropin : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <ID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Weight>k__BackingField;

        public Skill_dropin Copy() => 
            new Skill_dropin { 
                ID = this.ID,
                Weight = this.Weight
            };

        protected override bool ReadImpl()
        {
            this.ID = base.readInt();
            this.Weight = base.readInt();
            return true;
        }

        public int ID { get; private set; }

        public int Weight { get; private set; }
    }
}

