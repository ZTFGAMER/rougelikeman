namespace TableTool
{
    using System;

    public class Box_SilverBoxModel : LocalModel<Box_SilverBox, int>
    {
        private const string _Filename = "Box_SilverBox";

        protected override int GetBeanKey(Box_SilverBox bean) => 
            bean.ID;

        protected override string Filename =>
            "Box_SilverBox";
    }
}

