namespace TableTool
{
    using System;

    public class Skill_aloneModel : LocalModel<Skill_alone, int>
    {
        private const string _Filename = "Skill_alone";

        protected override int GetBeanKey(Skill_alone bean) => 
            bean.SkillID;

        protected override string Filename =>
            "Skill_alone";
    }
}

