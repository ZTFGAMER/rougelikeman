namespace TableTool
{
    using System;

    public class Skill_superModel : LocalModel<Skill_super, int>
    {
        private const string _Filename = "Skill_super";

        protected override int GetBeanKey(Skill_super bean) => 
            bean.SkillID;

        protected override string Filename =>
            "Skill_super";
    }
}

