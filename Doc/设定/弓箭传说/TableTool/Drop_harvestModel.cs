namespace TableTool
{
    using System;

    public class Drop_harvestModel : LocalModel<Drop_harvest, int>
    {
        private const string _Filename = "Drop_harvest";

        protected override int GetBeanKey(Drop_harvest bean) => 
            bean.ID;

        protected override string Filename =>
            "Drop_harvest";
    }
}

