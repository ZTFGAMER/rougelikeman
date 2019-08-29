namespace TableTool
{
    using System;

    public class Shop_itemModel : LocalModel<Shop_item, int>
    {
        private const string _Filename = "Shop_item";

        protected override int GetBeanKey(Shop_item bean) => 
            bean.ItemID;

        protected override string Filename =>
            "Shop_item";
    }
}

