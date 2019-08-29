using Dxx.Util;
using System;

public class Food2001 : FoodBase
{
    protected override void OnGetGoods(EntityBase entity)
    {
        object[] args = new object[] { "Gold", (int) base.data };
        string str = Utils.FormatString("{0} + {1}", args);
        entity.m_EntityData.ExcuteAttributes(str);
    }
}

