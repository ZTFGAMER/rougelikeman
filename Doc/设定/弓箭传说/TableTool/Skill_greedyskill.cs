namespace TableTool
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Skill_greedyskill : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <SkillID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Notes>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Type>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Weight>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Gold>k__BackingField;

        public Skill_greedyskill Copy() => 
            new Skill_greedyskill { 
                SkillID = this.SkillID,
                Notes = this.Notes,
                Type = this.Type,
                Weight = this.Weight,
                Gold = this.Gold
            };

        protected override bool ReadImpl()
        {
            this.SkillID = base.readInt();
            this.Notes = base.readLocalString();
            this.Type = base.readInt();
            this.Weight = base.readInt();
            this.Gold = base.readInt();
            return true;
        }

        public int SkillID { get; private set; }

        public string Notes { get; private set; }

        public int Type { get; private set; }

        public int Weight { get; private set; }

        public int Gold { get; private set; }
    }
}

