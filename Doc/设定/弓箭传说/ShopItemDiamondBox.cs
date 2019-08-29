using Dxx.Util;
using System;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemDiamondBox : MonoBehaviour
{
    public Text Text_Title;
    public Text Text_Free;
    public Image Image_BG;
    public ButtonCtrl Button_Get;
    public Image Image_Icon;
    public GoldTextCtrl mGoldCtrl;
    public CountDownCtrl mCountDownCtrl;
    public Action<int, ShopItemDiamondBox> OnClickButton;
    private Shop_Shop shopdata;
    private int mIndex;
    private bool bFreeShow = true;
    private LocalSave.TimeBoxType mBoxType = LocalSave.TimeBoxType.BoxChoose_DiamondNormal;
    private long mStartTime;
    private int PerTime;
    private long currenttime;
    private int count;
    private long last;

    private void Awake()
    {
        this.Button_Get.onClick = delegate {
            if (this.OnClickButton != null)
            {
                this.OnClickButton(this.mIndex, this);
            }
        };
        this.PerTime = GameConfig.GetTimeBoxTime(this.mBoxType);
    }

    private void CountDownShow(bool value)
    {
        if (this.mCountDownCtrl != null)
        {
            this.mCountDownCtrl.Show(value);
        }
    }

    private void FreeShow(bool value)
    {
        if (this.bFreeShow != value)
        {
            this.bFreeShow = value;
            if (this.Text_Free != null)
            {
                this.Text_Free.gameObject.SetActive(value);
                this.mGoldCtrl.gameObject.SetActive(!value);
                this.Image_BG.set_color(!value ? Color.white : Color.yellow);
                if (value)
                {
                    this.Text_Free.text = GameLogic.Hold.Language.GetLanguageByTID("商店_免费抽取", Array.Empty<object>());
                }
            }
        }
    }

    public bool GetCanFree() => 
        ((this.mIndex == 0) && (LocalSave.Instance.GetTimeBoxCount(this.mBoxType) > 0));

    public void Init(int index)
    {
        this.mIndex = index;
        object[] args = new object[] { index };
        this.Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("商店_宝箱{0}", args), Array.Empty<object>());
        this.mGoldCtrl.SetValue((int) ((index * 0x1130) + 600));
        this.UpdateBox();
    }

    public void OnLanguageChange()
    {
        this.Init(this.mIndex);
    }

    private void Update()
    {
        if ((this.mCountDownCtrl != null) && (this.mIndex == 0))
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
                    this.mCountDownCtrl.Refresh(this.last, 1f - (((float) this.last) / (((float) this.PerTime) / 1000f)));
                }
            }
        }
    }

    private void UpdateBox()
    {
        if (this.mIndex == 0)
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
        }
        else
        {
            this.FreeShow(false);
            this.CountDownShow(false);
        }
    }
}

