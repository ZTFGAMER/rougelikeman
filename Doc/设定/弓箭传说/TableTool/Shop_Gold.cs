namespace TableTool
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Shop_Gold : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <ID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Level>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Price>k__BackingField;

        public Shop_Gold Copy() => 
            new Shop_Gold { 
                ID = this.ID,
                Level = this.Level,
                Price = this.Price
            };

        protected override bool ReadImpl()
        {
            this.ID = base.readInt();
            this.Level = base.readInt();
            this.Price = base.readInt();
            return true;
        }

        public int ID { get; private set; }

        public int Level { get; private set; }

        public int Price { get; private set; }
    }
}

