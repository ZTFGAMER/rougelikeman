namespace TableTool
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Character_Char : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <CharID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <TypeID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <ModelID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <BodyScale>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <TextureID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <WeaponID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Attackrangetype>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Speed>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <HP>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <RotateSpeed>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <BodyAttack>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Divide>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int[] <Skills>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <BackRatio>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float[] <ActionSpeed>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <HittedEffectID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <DeadSoundID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Cache>k__BackingField;

        public Character_Char Copy() => 
            new Character_Char { 
                CharID = this.CharID,
                TypeID = this.TypeID,
                ModelID = this.ModelID,
                BodyScale = this.BodyScale,
                TextureID = this.TextureID,
                WeaponID = this.WeaponID,
                Attackrangetype = this.Attackrangetype,
                Speed = this.Speed,
                HP = this.HP,
                RotateSpeed = this.RotateSpeed,
                BodyAttack = this.BodyAttack,
                Divide = this.Divide,
                Skills = this.Skills,
                BackRatio = this.BackRatio,
                ActionSpeed = this.ActionSpeed,
                HittedEffectID = this.HittedEffectID,
                DeadSoundID = this.DeadSoundID,
                Cache = this.Cache
            };

        protected override bool ReadImpl()
        {
            this.CharID = base.readInt();
            this.TypeID = base.readInt();
            this.ModelID = base.readLocalString();
            this.BodyScale = base.readFloat();
            this.TextureID = base.readLocalString();
            this.WeaponID = base.readInt();
            this.Attackrangetype = base.readInt();
            this.Speed = base.readInt();
            this.HP = base.readInt();
            this.RotateSpeed = base.readInt();
            this.BodyAttack = base.readInt();
            this.Divide = base.readInt();
            this.Skills = base.readArrayint();
            this.BackRatio = base.readFloat();
            this.ActionSpeed = base.readArrayfloat();
            this.HittedEffectID = base.readInt();
            this.DeadSoundID = base.readInt();
            this.Cache = base.readInt();
            return true;
        }

        public int CharID { get; private set; }

        public int TypeID { get; private set; }

        public string ModelID { get; private set; }

        public float BodyScale { get; private set; }

        public string TextureID { get; private set; }

        public int WeaponID { get; private set; }

        public int Attackrangetype { get; private set; }

        public int Speed { get; private set; }

        public int HP { get; private set; }

        public int RotateSpeed { get; private set; }

        public int BodyAttack { get; private set; }

        public int Divide { get; private set; }

        public int[] Skills { get; private set; }

        public float BackRatio { get; private set; }

        public float[] ActionSpeed { get; private set; }

        public int HittedEffectID { get; private set; }

        public int DeadSoundID { get; private set; }

        public int Cache { get; private set; }
    }
}

