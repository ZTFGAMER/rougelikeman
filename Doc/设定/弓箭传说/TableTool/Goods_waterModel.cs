namespace TableTool
{
    using System;

    public class Goods_waterModel : LocalModel<Goods_water, string>
    {
        private const string _Filename = "Goods_water";

        protected override string GetBeanKey(Goods_water bean) => 
            bean.CheckID;

        protected override string Filename =>
            "Goods_water";
    }
}

