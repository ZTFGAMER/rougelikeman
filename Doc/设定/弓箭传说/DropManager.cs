using Dxx.Util;
using System;
using System.Collections.Generic;
using TableTool;

public class DropManager
{
    private const int percentnumber = 0x5f5e100;
    private long level_dropequip;
    private DropData mDropData;
    private bool equip_talent_enable;
    private bool equipexp_talent_enable;
    private bool equip_must_drop;
    private BattleDropData battle_drop_temp;
    private List<Drop_DropModel.DropData> equiplist;
    private bool can_drop = true;

    private List<BattleDropData> GetDropList()
    {
        List<BattleDropData> list = new List<BattleDropData>();
        this.equiplist = LocalSave.Instance.mFakeStageDrop.GetDropList();
        if ((this.equiplist != null) && (this.equiplist.Count > 0))
        {
            int num = 0;
            int count = this.equiplist.Count;
            while (num < count)
            {
                Drop_DropModel.DropData data = this.equiplist[num];
                this.can_drop = true;
                LocalSave.EquipOne newEquipByID = LocalSave.Instance.GetNewEquipByID(data.id, data.count);
                if (!this.equipexp_talent_enable)
                {
                    if (newEquipByID.Overlying)
                    {
                        this.can_drop = false;
                    }
                }
                else if (newEquipByID.Overlying && !LocalSave.Instance.mEquip.Get_EquipExp_CanDrop(newEquipByID))
                {
                    this.can_drop = false;
                }
                if (this.can_drop)
                {
                    list.Add(new BattleDropData(FoodType.eEquip, newEquipByID));
                }
                num++;
            }
        }
        return list;
    }

    private void GetRandomEquip(ref List<BattleDropData> list, Soldier_soldier data)
    {
        if (this.equip_talent_enable)
        {
            int num = (int) (((data.EquipRate * this.mDropData.EquipProb) * 100f) * (1f + GameLogic.Self.m_EntityData.attribute.EquipDrop.Value));
            if (this.equip_must_drop)
            {
                if (data.CharID > 0x1388)
                {
                    List<BattleDropData> dropList = this.GetDropList();
                    int num2 = 0;
                    int count = dropList.Count;
                    while (num2 < count)
                    {
                        if ((dropList[num2].type == FoodType.eEquip) && (dropList[num2].data != null))
                        {
                            LocalSave.EquipOne one = dropList[num2].data as LocalSave.EquipOne;
                            if ((one != null) && (one.data.Position != 1))
                            {
                                list.Add(dropList[num2]);
                                this.equip_must_drop = false;
                                return;
                            }
                        }
                        num2++;
                    }
                    this.GetRandomEquip(ref list, data);
                }
            }
            else
            {
                this.level_dropequip += num;
                this.level_dropequip = MathDxx.Clamp(this.level_dropequip, 0L, (long) (1E+08f * GameConfig.GetEquipDropMaxRate()));
                if (GameLogic.Random(0, 0x5f5e100) < this.level_dropequip)
                {
                    this.level_dropequip = 0L;
                    List<BattleDropData> dropList = this.GetDropList();
                    list.AddRange(dropList);
                }
                LocalSave.Instance.SaveExtra.SetEquipDropRate(this.level_dropequip);
            }
        }
    }

    private void GetRandomEquipExp(ref List<BattleDropData> list, Soldier_soldier data)
    {
        if (this.equipexp_talent_enable)
        {
            List<int> equipExpDrop = LocalModelManager.Instance.Stage_Level_stagechapter.GetEquipExpDrop((EntityType) LocalModelManager.Instance.Character_Char.GetBeanById(data.CharID).TypeID);
            int num2 = 0;
            int count = equipExpDrop.Count;
            while (num2 < count)
            {
                int num = equipExpDrop[num2];
                if (num > 0)
                {
                    int equipexpid = LocalModelManager.Instance.Equip_equip.RandomEquipExp();
                    if (LocalSave.Instance.mEquip.Get_EquipExp_CanDrop(equipexpid))
                    {
                        LocalSave.EquipOne newEquipByID = LocalSave.Instance.GetNewEquipByID(equipexpid, num);
                        list.Add(new BattleDropData(FoodType.eEquip, newEquipByID));
                    }
                }
                num2++;
            }
        }
    }

    private void GetRandomGold(ref List<BattleDropData> list, Soldier_soldier data)
    {
        int dropDataGold = GameLogic.Hold.BattleData.mModeData.GetDropDataGold(data);
        List<Drop_GoldModel.DropGold> dropList = LocalModelManager.Instance.Drop_Gold.GetDropList(dropDataGold);
        int num2 = 0;
        int count = dropList.Count;
        while (num2 < count)
        {
            int num4 = MathDxx.CeilToInt((dropList[num2].Gold * (1f + GameLogic.SelfAttribute.InGameGold.Value)) * LocalModelManager.Instance.Stage_Level_stagechapter.GetGoldRate());
            list.Add(new BattleDropData(FoodType.eGold, num4));
            num2++;
        }
    }

    public void GetRandomGoldHitted(ref List<BattleDropData> list, Soldier_soldier data)
    {
        int dropid = data.GoldDropGold1;
        List<Drop_GoldModel.DropGold> dropList = LocalModelManager.Instance.Drop_Gold.GetDropList(dropid);
        int num2 = 0;
        int count = dropList.Count;
        while (num2 < count)
        {
            list.Add(new BattleDropData(FoodType.eGold, dropList[num2].Gold));
            num2++;
        }
    }

    public void GetRandomLevel(ref List<BattleDropData> list, Soldier_soldier data)
    {
        this.GetRandomEquipExp(ref list, data);
        this.GetRandomEquip(ref list, data);
    }

    public void Reset()
    {
        this.Reset_Level();
    }

    private void Reset_Level()
    {
        this.level_dropequip = LocalSave.Instance.SaveExtra.EquipDropRate;
        this.mDropData = GameLogic.Hold.BattleData.mModeData.GetDropData();
        this.equip_talent_enable = LocalSave.Instance.SaveExtra.Get_Equip_Drop();
        this.equipexp_talent_enable = LocalSave.Instance.SaveExtra.Get_EquipExp_Drop();
        this.equip_must_drop = LocalSave.Instance.GetEquipGuide_mustdrop();
    }

    public class DropData
    {
        public int EquipProb;
    }
}

