using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TableTool;

public class SelfAttributeData
{
    public EntityAttributeBase attribute;
    private List<string> levelups = new List<string>();
    private List<LocalSave.EquipOne> mEquips = new List<LocalSave.EquipOne>();
    public EntityAttributeBase.ValueFloatBase InGameGold = new EntityAttributeBase.ValueFloatBase();
    public EntityAttributeBase.ValueFloatBase InGameExp = new EntityAttributeBase.ValueFloatBase();
    public EntityAttributeBase.ValueFloatBase Up_Weapon = new EntityAttributeBase.ValueFloatBase();
    public EntityAttributeBase.ValueFloatBase Up_Hero = new EntityAttributeBase.ValueFloatBase();
    public EntityAttributeBase.ValueFloatBase Up_Armor = new EntityAttributeBase.ValueFloatBase();
    public EntityAttributeBase.ValueFloatBase Up_Pet = new EntityAttributeBase.ValueFloatBase();
    public EntityAttributeBase.ValueFloatBase Up_Ornament = new EntityAttributeBase.ValueFloatBase();
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$map5;

    public void Attribute2LevelUp(EntityData data)
    {
        int num = 0;
        int count = this.levelups.Count;
        while (num < count)
        {
            data.ExcuteAttributes(this.levelups[num]);
            num++;
        }
    }

    public void ClearBattle()
    {
        this.InGameGold.InitValue(0L, 0L);
        this.InGameExp.InitValue(0L, 0L);
        this.Up_Weapon.InitValue(0L, 0L);
        this.Up_Hero.InitValue(0L, 0L);
        this.Up_Armor.InitValue(0L, 0L);
        this.Up_Pet.InitValue(0L, 0L);
        this.Up_Ornament.InitValue(0L, 0L);
    }

    private void ClearCards()
    {
        this.levelups.Clear();
    }

    public void Excute(string att)
    {
        Goods_goods.GoodData goodData = Goods_goods.GetGoodData(att);
        if ((!this.Excute(goodData.goodType, goodData.value) && !this.attribute.Excute(goodData)) && att.Contains("LevelUp:"))
        {
            this.levelups.Add(att);
        }
    }

    public bool Excute(string type, long value)
    {
        bool flag = true;
        if (type != null)
        {
            if (<>f__switch$map5 == null)
            {
                Dictionary<string, int> dictionary = new Dictionary<string, int>(9) {
                    { 
                        "Global_HarvestLevel",
                        0
                    },
                    { 
                        "Global_InGameGold%",
                        1
                    },
                    { 
                        "Global_InGameExp%",
                        2
                    },
                    { 
                        "Global_UP_Weapon%",
                        3
                    },
                    { 
                        "Global_UP_Hero%",
                        4
                    },
                    { 
                        "Global_UP_Armor%",
                        5
                    },
                    { 
                        "Global_UP_Pet%",
                        6
                    },
                    { 
                        "Global_UP_Ornament%",
                        7
                    },
                    { 
                        "Global_UP_EquipAll%",
                        8
                    }
                };
                <>f__switch$map5 = dictionary;
            }
            if (<>f__switch$map5.TryGetValue(type, out int num))
            {
                switch (num)
                {
                    case 0:
                        return flag;

                    case 1:
                        this.InGameGold.UpdateValuePercent(value);
                        return flag;

                    case 2:
                        this.InGameExp.UpdateValuePercent(value);
                        return flag;

                    case 3:
                        this.Up_Weapon.UpdateValuePercent(value);
                        return flag;

                    case 4:
                        this.Up_Hero.UpdateValuePercent(value);
                        return flag;

                    case 5:
                        this.Up_Armor.UpdateValuePercent(value);
                        return flag;

                    case 6:
                        this.Up_Pet.UpdateValuePercent(value);
                        return flag;

                    case 7:
                        this.Up_Ornament.UpdateValuePercent(value);
                        return flag;

                    case 8:
                        this.Up_Weapon.UpdateValuePercent(value);
                        this.Up_Armor.UpdateValuePercent(value);
                        this.Up_Ornament.UpdateValuePercent(value);
                        this.Up_Pet.UpdateValuePercent(value);
                        return flag;
                }
            }
        }
        return false;
    }

    public float GetUpPercent(int position)
    {
        switch (position)
        {
            case 0:
                return this.Up_Hero.Value;

            case 1:
                return this.Up_Weapon.Value;

            case 2:
                return this.Up_Armor.Value;

            case 5:
                return this.Up_Ornament.Value;

            case 6:
                return this.Up_Pet.Value;
        }
        return 0f;
    }

    public void Init()
    {
        this.InitAttribute();
    }

    private void InitAttribute()
    {
        this.attribute = new EntityAttributeBase(0x3e9);
        this.attribute.Excute("HPMax + 600");
        this.attribute.Excute("Attack + 150");
        this.InitCards();
        this.InitEquips();
    }

    public void InitBabies()
    {
        if (GameLogic.InGame)
        {
            List<LocalSave.EquipOne> list = LocalSave.Instance.Equip_get_equip_babies();
            int num = 0;
            int count = list.Count;
            while (num < count)
            {
                LocalSave.EquipOne one = list[num];
                object[] args = new object[] { one };
                GameLogic.Self.AddSkillBaby(one.EquipID, args);
                num++;
            }
        }
    }

    private void InitCards()
    {
        this.ClearCards();
        List<LocalSave.CardOne> wearCards = LocalSave.Instance.GetWearCards();
        int num = 0;
        int count = wearCards.Count;
        while (num < count)
        {
            List<string> attributes = LocalModelManager.Instance.Skill_slotout.GetAttributes(wearCards[num]);
            int num3 = 0;
            int num4 = attributes.Count;
            while (num3 < num4)
            {
                this.Excute(attributes[num3]);
                num3++;
            }
            num++;
        }
    }

    private void InitEquips()
    {
        LocalSave.Instance.Equip_Attribute2(this);
    }
}

