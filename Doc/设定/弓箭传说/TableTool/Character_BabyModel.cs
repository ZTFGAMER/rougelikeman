namespace TableTool
{
    using System;

    public class Character_BabyModel : LocalModel<Character_Baby, string>
    {
        private const string _Filename = "Character_Baby";

        protected override string GetBeanKey(Character_Baby bean) => 
            bean.BabyID;

        protected override string Filename =>
            "Character_Baby";
    }
}

