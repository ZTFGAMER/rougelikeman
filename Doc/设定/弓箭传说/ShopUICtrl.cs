using Dxx.Util;
using PureMVC.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class ShopUICtrl : MediatorCtrlBase
{
    public const string String_ShopOneStageDiscount = "ShopOneStageDiscount";
    public const string String_ShopOneEquipExp = "ShopOneEquipExp";
    public const string String_ShopOneFreeBox = "ShopOneFreeBox";
    public const string String_ShopOneDiamondBox = "ShopOneDiamondBox";
    public const string String_ShopOneDiamond = "ShopOneDiamond";
    public const string String_ShopOneGold = "ShopOneGold";
    public ScrollRectBase mScrollRect;
    public MainUIScrollRectInsideCtrl mInsideCtrl;
    public GameObject window;
    private Dictionary<string, ShopOneBase> mList = new Dictionary<string, ShopOneBase>();
    private List<string> openlist;
    private Dictionary<string, Func<bool>> mOpenCondition;
    private float gotopos;
    private float maxcontenty;
    private float uppos;
    private int opencheck;
    private RectTransform windowt;
    private bool bOpened;
    [CompilerGenerated]
    private static Func<bool> <>f__am$cache0;
    [CompilerGenerated]
    private static Func<bool> <>f__am$cache1;

    public ShopUICtrl()
    {
        List<string> list = new List<string> { 
            "ShopOneStageDiscount",
            "ShopOneDiamondBox",
            "ShopOneDiamond",
            "ShopOneGold"
        };
        this.openlist = list;
        this.mOpenCondition = new Dictionary<string, Func<bool>>();
    }

    private ShopOneBase get_one(string str)
    {
        ShopOneBase base2 = null;
        if (this.mList.TryGetValue(str, out base2))
        {
        }
        return base2;
    }

    private int GetOpenCheck()
    {
        int num = 0;
        if (GameLogic.Hold.Guide.mEquip.process > 0)
        {
            num |= 2;
        }
        if (this.mOpenCondition["ShopOneStageDiscount"]())
        {
            num |= 4;
        }
        return num;
    }

    private ShopOneBase GetShop(string path)
    {
        if (!this.mList.TryGetValue(path, out ShopOneBase component))
        {
            object[] args = new object[] { path };
            GameObject child = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>(Utils.FormatString("UIPanel/ShopUI/{0}", args)));
            child.SetParentNormal(this.mScrollRect.get_content().transform);
            component = child.GetComponent<ShopOneBase>();
            component.name = path;
            this.mList.Add(path, component);
        }
        return component;
    }

    private void Goto(int index, bool play = false)
    {
        for (int i = index; i < this.openlist.Count; i++)
        {
            if (this.Goto(this.openlist[i], play))
            {
                break;
            }
        }
    }

    private bool Goto(string name, bool play = false)
    {
        if (this.mList.TryGetValue(name, out ShopOneBase base2))
        {
            this.gotopos = -base2.mRectTransform.anchoredPosition.y + this.uppos;
            this.gotopos = MathDxx.Clamp(this.gotopos, 0f, this.maxcontenty);
            this.mScrollRect.Goto(this.gotopos, play);
            return true;
        }
        return false;
    }

    private void InitUI()
    {
        this.uppos = -70f + PlatformHelper.GetFringeHeight();
        float uppos = this.uppos;
        float y = -uppos;
        int num3 = 0;
        int count = this.openlist.Count;
        while (num3 < count)
        {
            Func<bool> func = null;
            if ((this.mOpenCondition.TryGetValue(this.openlist[num3], out func) && (func != null)) && !func())
            {
                ShopOneBase base2 = this.get_one(this.openlist[num3]);
                if (base2 != null)
                {
                    base2.gameObject.SetActive(false);
                }
            }
            else
            {
                ShopOneBase shop = this.GetShop(this.openlist[num3]);
                shop.gameObject.SetActive(true);
                shop.Init();
                shop.mRectTransform.anchoredPosition = new Vector3(0f, uppos, 0f);
                uppos -= shop.mRectTransform.sizeDelta.y;
                y += shop.mRectTransform.sizeDelta.y;
            }
            num3++;
        }
        this.UpdateNet();
        y += 200f;
        this.mScrollRect.get_content().sizeDelta = new Vector2(this.mScrollRect.get_content().sizeDelta.x, y);
        this.maxcontenty = y - this.windowt.sizeDelta.y;
        this.maxcontenty = MathDxx.Clamp(this.maxcontenty, 0f, this.maxcontenty);
        this.Goto(0, false);
    }

    protected override void OnClose()
    {
        this.bOpened = false;
        Dictionary<string, ShopOneBase>.Enumerator enumerator = this.mList.GetEnumerator();
        while (enumerator.MoveNext())
        {
            KeyValuePair<string, ShopOneBase> current = enumerator.Current;
            current.Value.Deinit();
        }
    }

    public override object OnGetEvent(string eventName) => 
        null;

    public override void OnHandleNotification(INotification notification)
    {
        string name = notification.Name;
        object body = notification.Body;
        if (name != null)
        {
            if (name == "PUB_NETCONNECT_UPDATE")
            {
                this.UpdateNet();
            }
            else if (name == "MainUI_GotoShop")
            {
                string str2 = (string) body;
                this.Goto(str2, true);
            }
            else if (name == "MainUI_TimeBoxUpdate")
            {
                if (this.mList.TryGetValue("ShopOneFreeBox", out ShopOneBase base2))
                {
                    base2.UpdateNet();
                }
            }
            else if (name == "ShopUI_Update")
            {
                LocalSave.Instance.mShop.bRefresh = true;
                if (this.bOpened)
                {
                    this.OnOpen();
                }
            }
        }
    }

    protected override void OnInit()
    {
        RectTransform transform = base.transform as RectTransform;
        this.windowt = this.window.transform as RectTransform;
        this.windowt.sizeDelta = new Vector2(this.windowt.sizeDelta.x, transform.sizeDelta.y + this.windowt.anchoredPosition.y);
        this.mOpenCondition.Clear();
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = () => ((LocalSave.Instance.mGuideData.is_system_open(1) || (GameLogic.Hold.Guide.mEquip.process > 0)) || (LocalSave.Instance.GetDiamondBoxFreeCount(LocalSave.TimeBoxType.BoxChoose_DiamondNormal) > 0)) || (LocalSave.Instance.GetHaveEquips(true).Count > 1);
        }
        this.mOpenCondition.Add("ShopOneDiamondBox", <>f__am$cache0);
        if (<>f__am$cache1 == null)
        {
            <>f__am$cache1 = delegate {
                if (!PurchaseManager.Instance.IsValid())
                {
                    return false;
                }
                return ShopOneStageDiscount.IsValid();
            };
        }
        this.mOpenCondition.Add("ShopOneStageDiscount", <>f__am$cache1);
    }

    public override void OnLanguageChange()
    {
        Dictionary<string, ShopOneBase>.Enumerator enumerator = this.mList.GetEnumerator();
        while (enumerator.MoveNext())
        {
            KeyValuePair<string, ShopOneBase> current = enumerator.Current;
            current.Value.OnLanguageChange();
        }
    }

    protected override void OnOpen()
    {
        this.bOpened = true;
        SdkManager.send_event_iap("SHOW", ShopOpenSource.ESHOP_PAGE, string.Empty, string.Empty, string.Empty);
        int openCheck = this.GetOpenCheck();
        this.opencheck = openCheck;
        LocalSave.Instance.mShop.bRefresh = false;
        this.InitUI();
        this.OnLanguageChange();
    }

    protected override void OnSetArgs(object o)
    {
        this.mInsideCtrl.anotherScrollRect = o as ScrollRectBase;
    }

    private void UpdateList()
    {
    }

    private void UpdateNet()
    {
        Dictionary<string, ShopOneBase>.Enumerator enumerator = this.mList.GetEnumerator();
        while (enumerator.MoveNext())
        {
            KeyValuePair<string, ShopOneBase> current = enumerator.Current;
            current.Value.UpdateNet();
        }
    }
}

