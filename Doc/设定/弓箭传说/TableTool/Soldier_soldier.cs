namespace TableTool
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Soldier_soldier : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <CharID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Notes>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <GoldDropLevel>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <ScrollDropLevel>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <GoldDropGold1>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <GoldDropGold2>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <EquipRate>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Exp>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <DropRadius>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <HPDrop1>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <HPDrop2>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <HPDrop3>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <BodyHitSoundID>k__BackingField;

        public Soldier_soldier Copy() => 
            new Soldier_soldier { 
                CharID = this.CharID,
                Notes = this.Notes,
                GoldDropLevel = this.GoldDropLevel,
                ScrollDropLevel = this.ScrollDropLevel,
                GoldDropGold1 = this.GoldDropGold1,
                GoldDropGold2 = this.GoldDropGold2,
                EquipRate = this.EquipRate,
                Exp = this.Exp,
                DropRadius = this.DropRadius,
                HPDrop1 = this.HPDrop1,
                HPDrop2 = this.HPDrop2,
                HPDrop3 = this.HPDrop3,
                BodyHitSoundID = this.BodyHitSoundID
            };

        protected override bool ReadImpl()
        {
            this.CharID = base.readInt();
            this.Notes = base.readLocalString();
            this.GoldDropLevel = base.readInt();
            this.ScrollDropLevel = base.readInt();
            this.GoldDropGold1 = base.readInt();
            this.GoldDropGold2 = base.readInt();
            this.EquipRate = base.readFloat();
            this.Exp = base.readInt();
            this.DropRadius = base.readInt();
            this.HPDrop1 = base.readInt();
            this.HPDrop2 = base.readInt();
            this.HPDrop3 = base.readInt();
            this.BodyHitSoundID = base.readInt();
            return true;
        }

        public int CharID { get; private set; }

        public string Notes { get; private set; }

        public int GoldDropLevel { get; private set; }

        public int ScrollDropLevel { get; private set; }

        public int GoldDropGold1 { get; private set; }

        public int GoldDropGold2 { get; private set; }

        public float EquipRate { get; private set; }

        public int Exp { get; private set; }

        public int DropRadius { get; private set; }

        public int HPDrop1 { get; private set; }

        public int HPDrop2 { get; private set; }

        public int HPDrop3 { get; private set; }

        public int BodyHitSoundID { get; private set; }
    }
}

