using Dxx.Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopOneEquipExp : ShopOneBase
{
    public const float itemwidth = 235f;
    public Text Text_Title;
    public Text Text_Content;
    public GameObject goldparent;
    private List<ShopItemEquipExp> mList = new List<ShopItemEquipExp>();
    private GameObject _itemgold;
    private LocalUnityObjctPool mPool;
    private string oncestring;
    private string timestring;
    private int lasttime;
    private float m_flasttime;

    protected override void OnAwake()
    {
        if (this.mPool == null)
        {
            this.mPool = LocalUnityObjctPool.Create(base.gameObject);
            this.mPool.CreateCache<ShopItemEquipExp>(this.itemgold);
            this.itemgold.SetActive(false);
        }
    }

    private void OnClickEquipExp(int index, ShopItemEquipExp item)
    {
    }

    protected override void OnDeinit()
    {
    }

    protected override void OnInit()
    {
        this.mPool.Collect<ShopItemEquipExp>();
        this.mList.Clear();
        this.Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("shopui_equipexp_title", Array.Empty<object>());
        this.oncestring = GameLogic.Hold.Language.GetLanguageByTID("shopui_equipexp_buy_once", Array.Empty<object>());
        this.timestring = GameLogic.Hold.Language.GetLanguageByTID("battle_level_nexttime", Array.Empty<object>());
        this.lasttime = GameLogic.Random(500, 0x1388);
        this.m_flasttime = this.lasttime;
        this.Update();
        int num = 3;
        float num2 = (num - 1) * 235f;
        for (int i = 0; i < num; i++)
        {
            ShopItemEquipExp item = this.mPool.DeQueue<ShopItemEquipExp>();
            item.gameObject.SetParentNormal(this.goldparent);
            RectTransform transform = item.transform as RectTransform;
            transform.anchoredPosition = new Vector2((-num2 / 2f) + (235f * i), 0f);
            item.Init(i);
            item.OnClickButton = new Action<int, ShopItemEquipExp>(this.OnOpenWindowSure);
            this.mList.Add(item);
        }
    }

    public override void OnLanguageChange()
    {
        this.OnInit();
    }

    private void OnOpenWindowSure(int index, ShopItemEquipExp item)
    {
    }

    private void Update()
    {
        this.m_flasttime -= Time.deltaTime;
        string str = Utils.GetSecond3String((long) ((int) this.m_flasttime));
        object[] args = new object[] { this.oncestring, this.timestring, str };
        this.Text_Content.text = Utils.FormatString("{0}  {1}:{2}", args);
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

    private GameObject itemgold
    {
        get
        {
            if (this._itemgold == null)
            {
                this._itemgold = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>("UIPanel/ShopUI/ShopItemEquipExpOne"));
                this._itemgold.SetParentNormal(this.goldparent);
            }
            return this._itemgold;
        }
    }
}

