namespace TableTool
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Skill_alone : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <SkillID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Notes>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <Attributes>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int[] <DeBuffs>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <CreateEffectID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <Args>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <ArgsNote>k__BackingField;

        public Skill_alone Copy() => 
            new Skill_alone { 
                SkillID = this.SkillID,
                Notes = this.Notes,
                Attributes = this.Attributes,
                DeBuffs = this.DeBuffs,
                CreateEffectID = this.CreateEffectID,
                Args = this.Args,
                ArgsNote = this.ArgsNote
            };

        protected override bool ReadImpl()
        {
            this.SkillID = base.readInt();
            this.Notes = base.readLocalString();
            this.Attributes = base.readArraystring();
            this.DeBuffs = base.readArrayint();
            this.CreateEffectID = base.readInt();
            this.Args = base.readArraystring();
            this.ArgsNote = base.readLocalString();
            return true;
        }

        public int SkillID { get; private set; }

        public string Notes { get; private set; }

        public string[] Attributes { get; private set; }

        public int[] DeBuffs { get; private set; }

        public int CreateEffectID { get; private set; }

        public string[] Args { get; private set; }

        public string ArgsNote { get; private set; }
    }
}

