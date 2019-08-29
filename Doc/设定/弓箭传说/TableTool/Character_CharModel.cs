namespace TableTool
{
    using System;

    public class Character_CharModel : LocalModel<Character_Char, int>
    {
        private const string _Filename = "Character_Char";

        protected override int GetBeanKey(Character_Char bean) => 
            bean.CharID;

        protected override string Filename =>
            "Character_Char";
    }
}

