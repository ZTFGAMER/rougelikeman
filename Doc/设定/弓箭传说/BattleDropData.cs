using Dxx.Util;
using System;

public class BattleDropData
{
    public FoodType type;
    public FoodOneType childtype;
    public object data;

    public BattleDropData(FoodType type, object data)
    {
        FoodOneType childtype = FoodOneType.eGold01;
        if (type != FoodType.eGold)
        {
            if (type == FoodType.eEquip)
            {
                this.Init(type, childtype, data);
            }
            else
            {
                object[] args = new object[] { type };
                SdkManager.Bugly_Report("GameData.BattleDropData", Utils.FormatString("new BattleDropData type[{0}] is error!", args));
            }
        }
        else
        {
            int num = (int) data;
            if (num < 10)
            {
                childtype = FoodOneType.eGold01;
            }
            else if (num < 100)
            {
                childtype = FoodOneType.eGold02;
            }
            else if (num < 0x3e8)
            {
                childtype = FoodOneType.eGold03;
            }
            else
            {
                childtype = FoodOneType.eGold04;
            }
            this.Init(type, childtype, data);
        }
    }

    public BattleDropData(FoodType type, FoodOneType childtype, object data)
    {
        this.Init(type, childtype, data);
    }

    private void Init(FoodType type, FoodOneType childtype, object data)
    {
        this.type = type;
        this.childtype = childtype;
        this.data = data;
    }

    public override string ToString()
    {
        object[] args = new object[] { this.type, this.childtype, this.data };
        return Utils.FormatString("FoodType:{0} FoodOneType:{1} data:{2}", args);
    }
}

