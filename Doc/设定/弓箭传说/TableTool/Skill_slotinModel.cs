namespace TableTool
{
    using System;
    using System.Collections.Generic;

    public class Skill_slotinModel : LocalModel<Skill_slotin, int>
    {
        private const string _Filename = "Skill_slotin";

        protected override int GetBeanKey(Skill_slotin bean) => 
            bean.SkillID;

        public List<int> GetSkillsByStage(int stage)
        {
            List<int> list = new List<int>();
            IList<Skill_slotin> allBeans = base.GetAllBeans();
            int num = 0;
            int count = allBeans.Count;
            while (num < count)
            {
                Skill_slotin _slotin = allBeans[num];
                if ((_slotin.UnlockStage == stage) && !this.is_have_same_skill(list, _slotin.SkillID))
                {
                    list.Add(_slotin.SkillID);
                }
                num++;
            }
            return list;
        }

        private bool is_have_same_skill(List<int> list, int skillid)
        {
            int num = 0;
            int count = list.Count;
            while (num < count)
            {
                Skill_skill beanById = LocalModelManager.Instance.Skill_skill.GetBeanById(list[num]);
                Skill_skill _skill2 = LocalModelManager.Instance.Skill_skill.GetBeanById(skillid);
                if (((beanById != null) && (_skill2 != null)) && (beanById.SkillIcon == _skill2.SkillIcon))
                {
                    return true;
                }
                num++;
            }
            return false;
        }

        public bool IsWeaponSkillID(int id)
        {
            switch (id)
            {
                case 0xf425f:
                case 0xf4260:
                case 0xf4261:
                case 0xf4264:
                    return true;
            }
            return false;
        }

        protected override string Filename =>
            "Skill_slotin";
    }
}

