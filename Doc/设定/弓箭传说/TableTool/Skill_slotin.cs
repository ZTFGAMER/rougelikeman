namespace TableTool
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Skill_slotin : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <SkillID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Weight>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <UnlockStage>k__BackingField;

        public Skill_slotin Copy() => 
            new Skill_slotin { 
                SkillID = this.SkillID,
                Weight = this.Weight,
                UnlockStage = this.UnlockStage
            };

        protected override bool ReadImpl()
        {
            this.SkillID = base.readInt();
            this.Weight = base.readInt();
            this.UnlockStage = base.readInt();
            return true;
        }

        public int SkillID { get; private set; }

        public int Weight { get; private set; }

        public int UnlockStage { get; private set; }
    }
}

