namespace TableTool
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Skill_skill : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <SkillID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <SkillIcon>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <Attributes>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int[] <Effects>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int[] <Buffs>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int[] <Debuffs>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <LearnEffectID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <Args>k__BackingField;

        public Skill_skill Copy() => 
            new Skill_skill { 
                SkillID = this.SkillID,
                SkillIcon = this.SkillIcon,
                Attributes = this.Attributes,
                Effects = this.Effects,
                Buffs = this.Buffs,
                Debuffs = this.Debuffs,
                LearnEffectID = this.LearnEffectID,
                Args = this.Args
            };

        protected override bool ReadImpl()
        {
            this.SkillID = base.readInt();
            this.SkillIcon = base.readInt();
            this.Attributes = base.readArraystring();
            this.Effects = base.readArrayint();
            this.Buffs = base.readArrayint();
            this.Debuffs = base.readArrayint();
            this.LearnEffectID = base.readInt();
            this.Args = base.readArraystring();
            return true;
        }

        public int SkillID { get; private set; }

        public int SkillIcon { get; private set; }

        public string[] Attributes { get; private set; }

        public int[] Effects { get; private set; }

        public int[] Buffs { get; private set; }

        public int[] Debuffs { get; private set; }

        public int LearnEffectID { get; private set; }

        public string[] Args { get; private set; }
    }
}

