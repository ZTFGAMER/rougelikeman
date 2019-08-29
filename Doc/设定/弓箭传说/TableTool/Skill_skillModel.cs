namespace TableTool
{
    using System;

    public class Skill_skillModel : LocalModel<Skill_skill, int>
    {
        private const string _Filename = "Skill_skill";

        protected override int GetBeanKey(Skill_skill bean) => 
            bean.SkillID;

        protected override string Filename =>
            "Skill_skill";
    }
}

