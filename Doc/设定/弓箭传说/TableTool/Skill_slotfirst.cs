namespace TableTool
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Skill_slotfirst : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <SkillID>k__BackingField;

        public Skill_slotfirst Copy() => 
            new Skill_slotfirst { SkillID = this.SkillID };

        protected override bool ReadImpl()
        {
            this.SkillID = base.readInt();
            return true;
        }

        public int SkillID { get; private set; }
    }
}

