namespace TableTool
{
    using System;

    public class Language_lauguageModel : LocalModel<Language_lauguage, string>
    {
        private const string _Filename = "Language_lauguage";

        protected override string GetBeanKey(Language_lauguage bean) => 
            bean.TID;

        protected override string Filename =>
            "Language_lauguage";
    }
}

