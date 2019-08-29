using Dxx.Util;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class ShopSingleUICtrl : MediatorCtrlBase
{
    public ButtonCtrl Button_Close;
    public ScrollRectBase mScrollRect;
    public RectTransform window;
    private ShopSingleProxy.Transfer mTransfer;
    private List<string> shops = new List<string>();
    private List<ShopOneBase> mShopItemList = new List<ShopOneBase>();
    private Dictionary<string, Func<bool>> mOpenCondition = new Dictionary<string, Func<bool>>();
    private Action onUIClose;
    [CompilerGenerated]
    private static Func<bool> <>f__am$cache0;
    [CompilerGenerated]
    private static Action <>f__am$cache1;

    private ShopOneBase GetShop(string path)
    {
        object[] args = new object[] { path };
        GameObject child = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>(Utils.FormatString("UIPanel/ShopUI/{0}", args)));
        child.SetParentNormal(this.mScrollRect.get_content().transform);
        ShopOneBase component = child.GetComponent<ShopOneBase>();
        this.mShopItemList.Add(component);
        return component;
    }

    private void InitUI()
    {
        this.shops.Clear();
        int num = 0;
        int count = this.mShopItemList.Count;
        while (num < count)
        {
            if (this.mShopItemList[num] != null)
            {
                Object.Destroy(this.mShopItemList[num].gameObject);
            }
            num++;
        }
        this.mShopItemList.Clear();
        if (this.mTransfer.type == ShopSingleProxy.SingleType.eDiamond)
        {
            this.shops.Add("ShopOneStageDiscount");
            this.shops.Add("ShopOneDiamond");
        }
        float y = 100f;
        int num4 = 0;
        int num5 = this.shops.Count;
        while (num4 < num5)
        {
            Func<bool> func = null;
            if ((!this.mOpenCondition.TryGetValue(this.shops[num4], out func) || (func == null)) || func())
            {
                ShopOneBase shop = this.GetShop(this.shops[num4]);
                shop.Init();
                shop.mRectTransform.anchoredPosition = new Vector2(0f, -y);
                y += shop.mRectTransform.sizeDelta.y;
            }
            num4++;
        }
        if (y > this.window.sizeDelta.y)
        {
            y += 200f;
            this.mScrollRect.movementType = ScrollRect.MovementType.Elastic;
        }
        else
        {
            this.mScrollRect.movementType = ScrollRect.MovementType.Clamped;
        }
        this.mScrollRect.get_content().sizeDelta = new Vector2(this.mScrollRect.get_content().sizeDelta.x, y);
    }

    protected override void OnClose()
    {
        WindowUI.CloseCurrency();
        if (this.onUIClose != null)
        {
            this.onUIClose();
        }
    }

    public override object OnGetEvent(string eventName) => 
        null;

    public override void OnHandleNotification(INotification notification)
    {
        string name = notification.Name;
        object body = notification.Body;
        if ((name != null) && (name == "ShopUI_Update"))
        {
            this.OnOpen();
        }
    }

    protected override void OnInit()
    {
        this.mOpenCondition.Clear();
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = delegate {
                if (!PurchaseManager.Instance.IsValid())
                {
                    return false;
                }
                return ShopOneStageDiscount.IsValid();
            };
        }
        this.mOpenCondition.Add("ShopOneStageDiscount", <>f__am$cache0);
        RectTransform transform = base.transform as RectTransform;
        this.window.sizeDelta = new Vector2(this.window.sizeDelta.x, transform.sizeDelta.y + this.window.anchoredPosition.y);
        if (<>f__am$cache1 == null)
        {
            <>f__am$cache1 = () => WindowUI.CloseWindow(WindowID.WindowID_ShopSingle);
        }
        this.Button_Close.onClick = <>f__am$cache1;
    }

    public override void OnLanguageChange()
    {
    }

    protected override void OnOpen()
    {
        SdkManager.send_event_iap("SHOW", PurchaseManager.Instance.GetOpenSource(), string.Empty, string.Empty, string.Empty);
        WindowUI.ShowCurrency(WindowID.WindowID_Currency);
        IProxy proxy = Facade.Instance.RetrieveProxy("ShopSingleProxy");
        if ((proxy == null) || (proxy.Data == null))
        {
            SdkManager.Bugly_Report("ShopSingleUICtrl.cs", Utils.FormatString("OnOpen ShopSingleProxy is error.", Array.Empty<object>()));
            this.Button_Close.onClick();
        }
        else
        {
            this.mTransfer = proxy.Data as ShopSingleProxy.Transfer;
            this.onUIClose = proxy.Event_Para0;
            this.InitUI();
        }
    }

    private void UpdateList()
    {
    }
}

