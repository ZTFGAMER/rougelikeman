namespace TableTool
{
    using System;

    public class Skill_slotoutcostModel : LocalModel<Skill_slotoutcost, int>
    {
        private const string _Filename = "Skill_slotoutcost";

        protected override int GetBeanKey(Skill_slotoutcost bean) => 
            bean.Id;

        protected override string Filename =>
            "Skill_slotoutcost";
    }
}

