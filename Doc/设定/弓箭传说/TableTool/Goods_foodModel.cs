namespace TableTool
{
    using System;

    public class Goods_foodModel : LocalModel<Goods_food, int>
    {
        private const string _Filename = "Goods_food";

        protected override int GetBeanKey(Goods_food bean) => 
            bean.GoodID;

        protected override string Filename =>
            "Goods_food";
    }
}

