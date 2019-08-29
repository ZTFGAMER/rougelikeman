namespace TableTool
{
    using System;

    public class Soldier_soldierModel : LocalModel<Soldier_soldier, int>
    {
        private const string _Filename = "Soldier_soldier";

        protected override int GetBeanKey(Soldier_soldier bean) => 
            bean.CharID;

        protected override string Filename =>
            "Soldier_soldier";
    }
}

