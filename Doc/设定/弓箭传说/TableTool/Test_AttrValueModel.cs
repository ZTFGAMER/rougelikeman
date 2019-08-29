namespace TableTool
{
    using System;

    public class Test_AttrValueModel : LocalModel<Test_AttrValue, string>
    {
        private const string _Filename = "Test_AttrValue";

        protected override string GetBeanKey(Test_AttrValue bean) => 
            bean.TypeId;

        protected override string Filename =>
            "Test_AttrValue";
    }
}

