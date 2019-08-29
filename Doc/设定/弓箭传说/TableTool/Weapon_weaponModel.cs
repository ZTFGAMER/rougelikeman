namespace TableTool
{
    using System;

    public class Weapon_weaponModel : LocalModel<Weapon_weapon, int>
    {
        private const string _Filename = "Weapon_weapon";

        protected override int GetBeanKey(Weapon_weapon bean) => 
            bean.WeaponID;

        protected override string Filename =>
            "Weapon_weapon";
    }
}

