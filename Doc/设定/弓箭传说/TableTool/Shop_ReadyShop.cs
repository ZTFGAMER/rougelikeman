namespace TableTool
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Shop_ReadyShop : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <ID>k__BackingField;
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

        public Shop_ReadyShop Copy() => 
            new Shop_ReadyShop { 
                ID = this.ID,
                ProductType = this.ProductType,
                ProductId = this.ProductId,
                ProductNum = this.ProductNum,
                PriceType = this.PriceType,
                Price = this.Price
            };

        protected override bool ReadImpl()
        {
            this.ID = base.readInt();
            this.ProductType = base.readInt();
            this.ProductId = base.readInt();
            this.ProductNum = base.readInt();
            this.PriceType = base.readInt();
            this.Price = base.readInt();
            return true;
        }

        public int ID { get; private set; }

        public int ProductType { get; private set; }

        public int ProductId { get; private set; }

        public int ProductNum { get; private set; }

        public int PriceType { get; private set; }

        public int Price { get; private set; }
    }
}

