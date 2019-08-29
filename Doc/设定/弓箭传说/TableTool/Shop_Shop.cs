namespace TableTool
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Shop_Shop : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <ID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <ShopType>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int[] <ShowCond>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int[] <CloseCond>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Position>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <ProductType>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <ProductId>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <ProductNum>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <PriceType>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <Price>k__BackingField;

        public Shop_Shop Copy() => 
            new Shop_Shop { 
                ID = this.ID,
                ShopType = this.ShopType,
                ShowCond = this.ShowCond,
                CloseCond = this.CloseCond,
                Position = this.Position,
                ProductType = this.ProductType,
                ProductId = this.ProductId,
                ProductNum = this.ProductNum,
                PriceType = this.PriceType,
                Price = this.Price
            };

        protected override bool ReadImpl()
        {
            this.ID = base.readInt();
            this.ShopType = base.readInt();
            this.ShowCond = base.readArrayint();
            this.CloseCond = base.readArrayint();
            this.Position = base.readInt();
            this.ProductType = base.readInt();
            this.ProductId = base.readInt();
            this.ProductNum = base.readInt();
            this.PriceType = base.readInt();
            this.Price = base.readFloat();
            return true;
        }

        public int ID { get; private set; }

        public int ShopType { get; private set; }

        public int[] ShowCond { get; private set; }

        public int[] CloseCond { get; private set; }

        public int Position { get; private set; }

        public int ProductType { get; private set; }

        public int ProductId { get; private set; }

        public int ProductNum { get; private set; }

        public int PriceType { get; private set; }

        public float Price { get; private set; }
    }
}

