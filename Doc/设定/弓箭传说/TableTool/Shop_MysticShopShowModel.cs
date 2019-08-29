namespace TableTool
{
    using System;

    public class Shop_MysticShopShowModel : LocalModel<Shop_MysticShopShow, int>
    {
        private const string _Filename = "Shop_MysticShopShow";

        protected override int GetBeanKey(Shop_MysticShopShow bean) => 
            bean.ID;

        protected override string Filename =>
            "Shop_MysticShopShow";
    }
}

