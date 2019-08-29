using Dxx.Util;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class ShopOneDiamondBox : ShopOneBase
{
    public Text Text_Title;
    public List<ShopItemDiamondBoxBase> mList;

    protected override void OnDeinit()
    {
        int num = 0;
        int count = this.mList.Count;
        while (num < count)
        {
            this.mList[num].Deinit();
            num++;
        }
    }

    protected override void OnInit()
    {
        int index = 0;
        int count = this.mList.Count;
        while (index < count)
        {
            this.mList[index].Init(index);
            index++;
        }
        this.OnLanguageChange();
    }

    public override void OnLanguageChange()
    {
        this.Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("商店_宝箱标题", Array.Empty<object>()), Array.Empty<object>());
        int num = 0;
        int count = this.mList.Count;
        while (num < count)
        {
            this.mList[num].LanguageChange();
            num++;
        }
    }

    public override void UpdateNet()
    {
        int num = 0;
        int count = this.mList.Count;
        while (num < count)
        {
            this.mList[num].UpdateNet();
            num++;
        }
    }
}

