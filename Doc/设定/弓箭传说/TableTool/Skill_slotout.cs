namespace TableTool
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Skill_slotout : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <GroupID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Type>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Quality>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <BaseAttributes>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float[] <AddAttributes>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <LevelLimit>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <InitialPower>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <AddPower>k__BackingField;

        public Skill_slotout Copy() => 
            new Skill_slotout { 
                GroupID = this.GroupID,
                Type = this.Type,
                Quality = this.Quality,
                BaseAttributes = this.BaseAttributes,
                AddAttributes = this.AddAttributes,
                LevelLimit = this.LevelLimit,
                InitialPower = this.InitialPower,
                AddPower = this.AddPower
            };

        protected override bool ReadImpl()
        {
            this.GroupID = base.readInt();
            this.Type = base.readInt();
            this.Quality = base.readInt();
            this.BaseAttributes = base.readArraystring();
            this.AddAttributes = base.readArrayfloat();
            this.LevelLimit = base.readInt();
            this.InitialPower = base.readLocalString();
            this.AddPower = base.readLocalString();
            return true;
        }

        public int GroupID { get; private set; }

        public int Type { get; private set; }

        public int Quality { get; private set; }

        public string[] BaseAttributes { get; private set; }

        public float[] AddAttributes { get; private set; }

        public int LevelLimit { get; private set; }

        public string InitialPower { get; private set; }

        public string AddPower { get; private set; }
    }
}

