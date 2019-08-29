namespace TableTool
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Drop_harvest : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <ID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <GoldDrop>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <EquipExp>k__BackingField;

        public Drop_harvest Copy() => 
            new Drop_harvest { 
                ID = this.ID,
                GoldDrop = this.GoldDrop,
                EquipExp = this.EquipExp
            };

        protected override bool ReadImpl()
        {
            this.ID = base.readInt();
            this.GoldDrop = base.readInt();
            this.EquipExp = base.readInt();
            return true;
        }

        public int ID { get; private set; }

        public int GoldDrop { get; private set; }

        public int EquipExp { get; private set; }
    }
}

