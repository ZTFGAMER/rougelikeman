namespace TableTool
{
    using System;

    public class Equip_UpgradeModel : LocalModel<Equip_Upgrade, int>
    {
        private const string _Filename = "Equip_Upgrade";

        protected override int GetBeanKey(Equip_Upgrade bean) => 
            bean.LevelId;

        protected override string Filename =>
            "Equip_Upgrade";
    }
}

