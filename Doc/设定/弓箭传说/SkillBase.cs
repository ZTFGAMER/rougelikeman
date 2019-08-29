using Dxx.Util;
using System;
using System.Collections.Generic;
using TableTool;

public class SkillBase
{
    protected EntityBase m_Entity;
    protected Skill_skill m_Data;
    private List<SkillAloneBase> effects = new List<SkillAloneBase>();

    public void Install(EntityBase entity, Skill_skill data, params object[] args)
    {
        this.m_Entity = entity;
        this.m_Data = data;
        this.InstallEffects(args);
        this.OnInstall(args);
    }

    private void InstallEffects(params object[] args)
    {
        this.UpdateAttributes(1);
        int index = 0;
        int length = this.m_Data.Effects.Length;
        while (index < length)
        {
            int key = this.m_Data.Effects[index];
            object[] objArray1 = new object[] { "SkillAlone", key };
            object[] objArray2 = new object[] { "SkillAlone", key };
            SkillAloneBase item = Type.GetType(Utils.GetString(objArray1)).Assembly.CreateInstance(Utils.GetString(objArray2)) as SkillAloneBase;
            item.Install(this.m_Entity, this.m_Data, LocalModelManager.Instance.Skill_alone.GetBeanById(key), args);
            this.effects.Add(item);
            index++;
        }
        int num4 = 0;
        int num5 = this.m_Data.Buffs.Length;
        while (num4 < num5)
        {
            GameLogic.SendBuff(this.m_Entity, this.m_Data.Buffs[num4], Array.Empty<float>());
            num4++;
        }
        int num6 = 0;
        int num7 = this.m_Data.Debuffs.Length;
        while (num6 < num7)
        {
            this.m_Entity.AddDebuff(this.m_Data.Debuffs[num6]);
            num6++;
        }
        if (this.m_Data.LearnEffectID != 0)
        {
            this.m_Entity.PlayEffect(this.m_Data.LearnEffectID);
        }
    }

    protected virtual void OnInstall(params object[] args)
    {
    }

    protected virtual void OnUninstall()
    {
    }

    public void Uninstall()
    {
        this.UpdateAttributes(-1);
        for (int i = this.effects.Count - 1; i >= 0; i--)
        {
            this.effects[i].Uninstall();
        }
        int index = 0;
        int length = this.m_Data.Debuffs.Length;
        while (index < length)
        {
            this.m_Entity.RemoveDebuff(this.m_Data.Debuffs[index]);
            index++;
        }
        int num4 = 0;
        int num5 = this.m_Data.Buffs.Length;
        while (num4 < num5)
        {
            BattleStruct.BuffStruct data = new BattleStruct.BuffStruct {
                entity = this.m_Entity,
                buffId = this.m_Data.Buffs[num4]
            };
            this.m_Entity.ExcuteCommend(EBattleAction.EBattle_Action_Remove_Buff, data);
            num4++;
        }
        this.effects.Clear();
        this.OnUninstall();
    }

    private void UpdateAttributes(int symbol)
    {
        int index = 0;
        int length = this.m_Data.Attributes.Length;
        while (index < length)
        {
            Goods_goods.GoodData goodData = Goods_goods.GetGoodData(this.m_Data.Attributes[index]);
            this.m_Entity.m_EntityData.ExcuteAttributes(goodData.goodType, goodData.value * symbol);
            index++;
        }
    }
}

