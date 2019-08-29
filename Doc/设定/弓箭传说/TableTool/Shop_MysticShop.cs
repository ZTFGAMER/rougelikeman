namespace TableTool
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Shop_MysticShop : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <ID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int[] <Stage>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <ShopType>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int[] <Position>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <ProductType>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <ProductId>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <ProductNum>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <PriceType>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Price>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Weights>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <AdProb>k__BackingField;

        public Shop_MysticShop Copy() => 
            new Shop_MysticShop { 
                ID = this.ID,
                Stage = this.Stage,
                ShopType = this.ShopType,
                Position = this.Position,
                ProductType = this.ProductType,
                ProductId = this.ProductId,
                ProductNum = this.ProductNum,
                PriceType = this.PriceType,
                Price = this.Price,
                Weights = this.Weights,
                AdProb = this.AdProb
            };

        protected override bool ReadImpl()
        {
            this.ID = base.readInt();
            this.Stage = base.readArrayint();
            this.ShopType = base.readInt();
            this.Position = base.readArrayint();
            this.ProductType = base.readInt();
            this.ProductId = base.readInt();
            this.ProductNum = base.readInt();
            this.PriceType = base.readInt();
            this.Price = base.readInt();
            this.Weights = base.readInt();
            this.AdProb = base.readInt();
            return true;
        }

        public int ID { get; private set; }

        public int[] Stage { get; private set; }

        public int ShopType { get; private set; }

        public int[] Position { get; private set; }

        public int ProductType { get; private set; }

        public int ProductId { get; private set; }

        public int ProductNum { get; private set; }

        public int PriceType { get; private set; }

        public int Price { get; private set; }

        public int Weights { get; private set; }

        public int AdProb { get; private set; }
    }
}

