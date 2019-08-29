namespace TableTool
{
    using System;

    public class Shop_ReadyShopModel : LocalModel<Shop_ReadyShop, int>
    {
        private const string _Filename = "Shop_ReadyShop";

        protected override int GetBeanKey(Shop_ReadyShop bean) => 
            bean.ID;

        protected override string Filename =>
            "Shop_ReadyShop";
    }
}

