using Dxx.Util;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class PurChaseOKUICtrl : MediatorCtrlBase
{
    public Text Text_Title;
    public Text Text_ID;
    public Text Text_Receipt;
    public ScrollRectBase mScrollRect;
    public ButtonCtrl Button_Close;
    public ButtonCtrl Button_Shadow;
    private LocalUnityObjctPool mPool;
    private PurChaseOKProxy.Transfer mTransfer;
    [CompilerGenerated]
    private static Action <>f__am$cache0;

    private void InitUI()
    {
        this.Text_ID.text = this.mTransfer.id;
        int num = 0x3e8;
        string receipt = this.mTransfer.receipt;
        int num2 = (this.mTransfer.receipt.Length / num) + 1;
        float y = 0f;
        for (int i = 0; i < num2; i++)
        {
            int startIndex = i * num;
            int num6 = (i + 1) * num;
            num6 = MathDxx.Clamp(num6, 0, this.mTransfer.receipt.Length);
            if (num6 < startIndex)
            {
                startIndex = num6;
            }
            string str2 = "empty";
            if (startIndex < num6)
            {
                str2 = receipt.Substring(startIndex, num6 - startIndex);
            }
            Text text = this.mPool.DeQueue<Text>();
            text.transform.SetParentNormal(this.mScrollRect.get_content());
            text.text = str2;
            (text.transform as RectTransform).anchoredPosition = new Vector2(0f, -y);
            y += text.preferredHeight;
            if (startIndex == num6)
            {
                break;
            }
        }
        this.mScrollRect.get_content().sizeDelta = new Vector2(this.mScrollRect.get_content().sizeDelta.x, y);
        this.Text_Title.text = "读取消息流失败";
    }

    protected override void OnClose()
    {
    }

    public override object OnGetEvent(string eventName) => 
        null;

    public override void OnHandleNotification(INotification notification)
    {
    }

    protected override void OnInit()
    {
        this.mPool = LocalUnityObjctPool.Create(base.gameObject);
        this.mPool.CreateCache<Text>(this.Text_Receipt.gameObject);
        this.Text_Receipt.gameObject.SetActive(false);
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = () => WindowUI.CloseWindow(WindowID.WindowID_PurchaseOK);
        }
        this.Button_Close.onClick = <>f__am$cache0;
        this.Button_Shadow.onClick = this.Button_Close.onClick;
    }

    public override void OnLanguageChange()
    {
    }

    protected override void OnOpen()
    {
        this.mPool.Collect<Text>();
        this.mTransfer = Facade.Instance.RetrieveProxy("PurChaseOKProxy").Data as PurChaseOKProxy.Transfer;
        this.InitUI();
    }
}

