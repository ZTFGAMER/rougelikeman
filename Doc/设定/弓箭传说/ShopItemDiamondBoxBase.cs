using Dxx.Util;
using PureMVC.Patterns;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemDiamondBoxBase : MonoBehaviour
{
    public Text Text_Content;
    public Text Text_Title;
    public Text Text_BoxContent;
    public Image Image_BG;
    public ButtonCtrl Button_Get;
    public GoldTextCtrl mGoldCtrl;
    public Text Text_Free;
    public RedNodeCtrl mRedCtrl;
    public GameObject FreeParent;
    public GameObject NotFreeParent;
    public CountDownCtrl mCountDownCtrl;
    public GameObject extraparent;
    public Text Text_Extra;
    protected bool bFreeShow = true;
    protected long mStartTime;
    protected int PerTime;
    protected long currenttime;
    protected int count;
    protected long last;
    protected float Text_FreeX;
    protected BoxOpenSingleProxy.Transfer mTransfer = new BoxOpenSingleProxy.Transfer();
    protected LocalSave.TimeBoxType mBoxType = LocalSave.TimeBoxType.BoxChoose_DiamondNormal;
    protected int mIndex;

    private void Awake()
    {
        this.Button_Get.SetDepondNet(true);
        this.Button_Get.onClick = new Action(this.ClickButton);
        this.Text_FreeX = this.Text_Free.get_rectTransform().anchoredPosition.x;
        this.OnAwake();
    }

    protected bool CheckCanOpen(int type, int price)
    {
        if (type == 1)
        {
            if (LocalSave.Instance.GetGold() >= price)
            {
                return true;
            }
            CInstance<TipsUIManager>.Instance.Show(ETips.Tips_GoldNotEnough, Array.Empty<string>());
            Facade.Instance.SendNotification("MainUI_GotoShop", "ShopOneGold");
            return false;
        }
        if (type == 2)
        {
            if (LocalSave.Instance.GetDiamond() >= price)
            {
                return true;
            }
            CInstance<TipsUIManager>.Instance.Show(ETips.Tips_DiamondNotEnough, Array.Empty<string>());
            Facade.Instance.SendNotification("MainUI_GotoShop", "ShopOneDiamond");
        }
        return false;
    }

    public void ClickButton()
    {
        this.OnClickButton();
    }

    protected void CountDownShow(bool value)
    {
        if (this.mCountDownCtrl != null)
        {
            this.mCountDownCtrl.Show(value);
        }
    }

    public void Deinit()
    {
        this.OnDeinit();
    }

    protected void FreeShow(bool value)
    {
        if (!value)
        {
            int diamondExtraCount = LocalSave.Instance.GetDiamondExtraCount(this.mBoxType);
            bool flag = diamondExtraCount > 0;
            if (this.extraparent != null)
            {
                this.extraparent.SetActive(flag);
            }
            if (this.mGoldCtrl != null)
            {
                this.mGoldCtrl.gameObject.SetActive(!flag);
            }
            if (this.Text_Content != null)
            {
                this.Text_Content.enabled = !flag;
            }
            if (flag && (this.Text_Extra != null))
            {
                object[] args = new object[] { diamondExtraCount };
                this.Text_Extra.text = Utils.FormatString("{0}/1", args);
            }
        }
        if (this.bFreeShow != value)
        {
            this.bFreeShow = value;
            if (this.FreeParent != null)
            {
                this.FreeParent.SetActive(value);
            }
            if (this.NotFreeParent != null)
            {
                this.NotFreeParent.SetActive(!value);
            }
        }
    }

    public void Init(int index)
    {
        this.mIndex = index;
        this.mRedCtrl.SetType(RedNodeType.eRedCount);
        this.OnInit();
    }

    public void LanguageChange()
    {
        this.OnLanguageChange();
    }

    protected virtual void OnAwake()
    {
    }

    protected virtual void OnClickButton()
    {
    }

    protected virtual void OnDeinit()
    {
    }

    protected virtual void OnInit()
    {
    }

    protected virtual void OnLanguageChange()
    {
    }

    protected void set_red(int count)
    {
        this.mRedCtrl.Value = count;
    }

    private void Update()
    {
        if (LocalSave.Instance.IsTimeBoxMax(this.mBoxType))
        {
            this.CountDownShow(false);
            this.FreeShow(true);
        }
        else
        {
            this.FreeShow(false);
            this.CountDownShow(true);
            this.currenttime = Utils.GetTimeStamp();
            this.count = (int) (((float) (this.currenttime - this.mStartTime)) / ((float) this.PerTime));
            if (this.count > 0)
            {
                LocalSave.Instance.Modify_TimeBoxCount(this.mBoxType, this.count, false);
                this.mStartTime += this.count * this.PerTime;
                LocalSave.Instance.SetTimeBoxTime(this.mBoxType, this.mStartTime);
                this.UpdateBox();
            }
            else
            {
                this.last = this.PerTime - (this.currenttime - this.mStartTime);
                this.mCountDownCtrl.Refresh(this.last, 1f - (((float) this.last) / ((float) this.PerTime)));
                object[] args = new object[] { this.mCountDownCtrl.GetTimeString() };
                this.mCountDownCtrl.Text_Time.text = GameLogic.Hold.Language.GetLanguageByTID("diamondbox1_freetime", args);
            }
        }
    }

    public void update_red()
    {
        this.set_red(LocalSave.Instance.GetDiamondBoxFreeCount(this.mBoxType));
    }

    protected void UpdateBox()
    {
        this.mStartTime = LocalSave.Instance.GetTimeBoxTime(this.mBoxType);
        if (LocalSave.Instance.GetTimeBoxCount(this.mBoxType) > 0)
        {
            this.FreeShow(true);
            this.CountDownShow(false);
        }
        else
        {
            this.FreeShow(false);
            this.CountDownShow(true);
        }
        this.update_red();
    }

    public void UpdateNet()
    {
    }
}

