using System;
using System.Collections.Generic;
using TableTool;

public class SkillAlone1027 : SkillAloneBabyBase
{
    private EntityBase mParent;
    private string attackparentatt = string.Empty;

    private void OnAttackUpdate(long value)
    {
        if (!string.IsNullOrEmpty(this.attackparentatt))
        {
            base.baby.m_EntityData.ExcuteAttributes(this.attackparentatt);
        }
    }

    protected override void OnInstall(object[] args)
    {
        base.mBabyID = int.Parse(base.m_SkillData.Args[0]);
        base.OnInstall();
        this.mParent = base.baby.GetParent();
        if (this.mParent != null)
        {
            this.mParent.m_EntityData.attribute.OnAttackUpdate = (Action<long>) Delegate.Combine(this.mParent.m_EntityData.attribute.OnAttackUpdate, new Action<long>(this.OnAttackUpdate));
            if (this.mParent.m_EntityData.attribute.BabyCountAttack.Value > 0f)
            {
                this.mParent.m_EntityData.ExcuteAttributes("Attack%", this.mParent.m_EntityData.attribute.BabyCountAttack.ValueLong);
            }
            if (this.mParent.m_EntityData.attribute.BabyCountAttackSpeed.Value > 0f)
            {
                this.mParent.m_EntityData.ExcuteAttributes("AttackSpeed%", this.mParent.m_EntityData.attribute.BabyCountAttackSpeed.ValueLong);
            }
        }
        for (int i = 1; i < base.m_SkillData.Args.Length; i++)
        {
            string str = base.m_SkillData.Args[i];
            Goods_goods.GoodData goodData = Goods_goods.GetGoodData(str);
            if (goodData.goodType == "AttackParentAttack%")
            {
                this.attackparentatt = str;
            }
            base.baby.m_EntityData.ExcuteAttributes(goodData);
        }
        if (((args.Length == 1) && (args[0] != null)) && (args[0] is LocalSave.EquipOne))
        {
            LocalSave.EquipOne one = args[0] as LocalSave.EquipOne;
            List<Goods_goods.GoodData> babyAttributes = one.GetBabyAttributes();
            for (int j = 0; j < babyAttributes.Count; j++)
            {
                base.baby.m_EntityData.ExcuteAttributes(babyAttributes[j]);
            }
            List<int> babySkills = one.GetBabySkills();
            for (int k = 0; k < babySkills.Count; k++)
            {
                base.baby.AddSkillBaby(babySkills[k], Array.Empty<object>());
            }
        }
    }

    protected override void OnUninstall()
    {
        base.OnUninstall();
        if (this.mParent != null)
        {
            this.mParent.m_EntityData.attribute.OnAttackUpdate = (Action<long>) Delegate.Remove(this.mParent.m_EntityData.attribute.OnAttackUpdate, new Action<long>(this.OnAttackUpdate));
        }
    }
}

