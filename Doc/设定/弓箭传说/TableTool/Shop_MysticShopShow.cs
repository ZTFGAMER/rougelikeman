namespace TableTool
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Shop_MysticShopShow : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <ID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <ShowProb>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <AddProb>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int[] <ShowRoom>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int[] <ShopTypeProb>k__BackingField;

        public Shop_MysticShopShow Copy() => 
            new Shop_MysticShopShow { 
                ID = this.ID,
                ShowProb = this.ShowProb,
                AddProb = this.AddProb,
                ShowRoom = this.ShowRoom,
                ShopTypeProb = this.ShopTypeProb
            };

        protected override bool ReadImpl()
        {
            this.ID = base.readInt();
            this.ShowProb = base.readInt();
            this.AddProb = base.readInt();
            this.ShowRoom = base.readArrayint();
            this.ShopTypeProb = base.readArrayint();
            return true;
        }

        public int ID { get; private set; }

        public int ShowProb { get; private set; }

        public int AddProb { get; private set; }

        public int[] ShowRoom { get; private set; }

        public int[] ShopTypeProb { get; private set; }
    }
}

