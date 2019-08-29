namespace TableTool
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Equip_Upgrade : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <LevelId>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <UpMaterials>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <UpCoins>k__BackingField;

        public Equip_Upgrade Copy() => 
            new Equip_Upgrade { 
                LevelId = this.LevelId,
                UpMaterials = this.UpMaterials,
                UpCoins = this.UpCoins
            };

        protected override bool ReadImpl()
        {
            this.LevelId = base.readInt();
            this.UpMaterials = base.readInt();
            this.UpCoins = base.readInt();
            return true;
        }

        public int LevelId { get; private set; }

        public int UpMaterials { get; private set; }

        public int UpCoins { get; private set; }
    }
}

