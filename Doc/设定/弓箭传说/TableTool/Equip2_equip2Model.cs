namespace TableTool
{
    using System;

    public class Equip2_equip2Model : LocalModel<Equip2_equip2, int>
    {
        private const string _Filename = "Equip2_equip2";

        protected override int GetBeanKey(Equip2_equip2 bean) => 
            bean.Id;

        protected override string Filename =>
            "Equip2_equip2";
    }
}

