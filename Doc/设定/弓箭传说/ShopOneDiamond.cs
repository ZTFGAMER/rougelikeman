using Dxx.Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopOneDiamond : ShopOneBase
{
    public const float itemwidth = 235f;
    public const float itemheight = 360f;
    public Text Text_Title;
    public Text Text_NotReady;
    public GameObject diamondparent;
    private List<ShopItemDiamond> mList = new List<ShopItemDiamond>();
    private GameObject _itemgdiamond;
    private LocalUnityObjctPool mPool;

    protected override void OnAwake()
    {
        if (this.mPool == null)
        {
            this.mPool = LocalUnityObjctPool.Create(base.gameObject);
            this.mPool.CreateCache<ShopItemDiamond>(this.itemgdiamond);
            this.itemgdiamond.SetActive(false);
        }
    }

    private void OnClickDiamond(string productID)
    {
    }

    protected override void OnDeinit()
    {
    }

    protected override void OnInit()
    {
        this.mPool.Collect<ShopItemDiamond>();
        this.mList.Clear();
        this.Text_NotReady.gameObject.SetActive(false);
        int num = 6;
        int num2 = 3;
        float num3 = (num2 - 1) * 235f;
        for (int i = 0; i < num; i++)
        {
            ShopItemDiamond item = this.mPool.DeQueue<ShopItemDiamond>();
            item.gameObject.SetParentNormal(this.diamondparent);
            RectTransform transform = item.transform as RectTransform;
            transform.anchoredPosition = new Vector2((-num3 / 2f) + (235f * (i % num2)), (i / num2) * -360f);
            item.Init(i);
            this.mList.Add(item);
        }
    }

    public override void OnLanguageChange()
    {
        this.Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("商店_钻石标题", Array.Empty<object>()), Array.Empty<object>());
        this.Text_NotReady.text = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("商店_正在准备商品", Array.Empty<object>()), Array.Empty<object>());
        for (int i = 0; i < this.mList.Count; i++)
        {
            this.mList[i].Init(i);
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

    private GameObject itemgdiamond
    {
        get
        {
            if (this._itemgdiamond == null)
            {
                this._itemgdiamond = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>("UIPanel/ShopUI/ShopItemDiamondOne"));
                this._itemgdiamond.SetParentNormal(this.diamondparent);
            }
            return this._itemgdiamond;
        }
    }
}

