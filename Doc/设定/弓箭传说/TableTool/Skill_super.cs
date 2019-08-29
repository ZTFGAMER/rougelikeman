namespace TableTool
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Skill_super : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <SkillID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <CD>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float[] <Args>k__BackingField;

        public Skill_super Copy() => 
            new Skill_super { 
                SkillID = this.SkillID,
                CD = this.CD,
                Args = this.Args
            };

        protected override bool ReadImpl()
        {
            this.SkillID = base.readInt();
            this.CD = base.readFloat();
            this.Args = base.readArrayfloat();
            return true;
        }

        public int SkillID { get; private set; }

        public float CD { get; private set; }

        public float[] Args { get; private set; }
    }
}

