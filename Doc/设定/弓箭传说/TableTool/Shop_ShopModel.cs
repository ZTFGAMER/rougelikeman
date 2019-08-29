namespace TableTool
{
    using System;

    public class Shop_ShopModel : LocalModel<Shop_Shop, int>
    {
        private const string _Filename = "Shop_Shop";

        public int get_buy_gold_diamond(int index)
        {
            if ((index >= 0) && (index < 3))
            {
                return (int) base.GetBeanById(0x65 + index).Price;
            }
            return 0;
        }

        protected override int GetBeanKey(Shop_Shop bean) => 
            bean.ID;

        protected override string Filename =>
            "Shop_Shop";
    }
}

