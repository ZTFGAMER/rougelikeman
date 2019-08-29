namespace TableTool
{
    using System;

    public class Box_TimeBoxModel : LocalModel<Box_TimeBox, int>
    {
        private const string _Filename = "Box_TimeBox";

        protected override int GetBeanKey(Box_TimeBox bean) => 
            bean.ID;

        protected override string Filename =>
            "Box_TimeBox";
    }
}

