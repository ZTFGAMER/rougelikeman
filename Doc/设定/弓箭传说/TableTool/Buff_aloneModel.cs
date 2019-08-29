namespace TableTool
{
    using System;

    public class Buff_aloneModel : LocalModel<Buff_alone, int>
    {
        private const string _Filename = "Buff_alone";

        protected override int GetBeanKey(Buff_alone bean) => 
            bean.BuffID;

        protected override string Filename =>
            "Buff_alone";
    }
}

