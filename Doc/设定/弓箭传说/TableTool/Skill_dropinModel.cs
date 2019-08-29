namespace TableTool
{
    using System;

    public class Skill_dropinModel : LocalModel<Skill_dropin, int>
    {
        private const string _Filename = "Skill_dropin";

        protected override int GetBeanKey(Skill_dropin bean) => 
            bean.ID;

        protected override string Filename =>
            "Skill_dropin";
    }
}

