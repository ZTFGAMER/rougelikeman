namespace TableTool
{
    using System;

    public class Character_LevelModel : LocalModel<Character_Level, int>
    {
        private const string _Filename = "Character_Level";
        private int maxLevel;

        protected override int GetBeanKey(Character_Level bean) => 
            bean.ID;

        public int GetExp(int level)
        {
            Character_Level beanById = base.GetBeanById(level);
            if (beanById != null)
            {
                return beanById.Exp;
            }
            return 1;
        }

        public int GetLevelUpCount(int addexp)
        {
            int level = LocalSave.Instance.GetLevel();
            int exp = (int) LocalSave.Instance.GetExp();
            int num3 = LocalModelManager.Instance.Character_Level.GetExp(level);
            int num4 = 0;
            while ((exp + addexp) >= num3)
            {
                level++;
                num4++;
                addexp -= num3 - exp;
                exp = 0;
                num3 = LocalModelManager.Instance.Character_Level.GetExp(level);
            }
            return num4;
        }

        public int GetMaxLevel() => 
            this.maxLevel;

        public void Init()
        {
            this.maxLevel = base.GetAllBeans().Count;
        }

        protected override string Filename =>
            "Character_Level";
    }
}

