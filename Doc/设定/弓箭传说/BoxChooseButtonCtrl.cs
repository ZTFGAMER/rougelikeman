using Dxx.Util;
using System;
using UnityEngine;
using UnityEngine.UI;

public class BoxChooseButtonCtrl : MonoBehaviour
{
    private Text Text_Get;
    private Text Text_FreeGet;
    private Text Text_FreeTime;
    private GoldTextCtrl mGoldCtrl;
    private ButtonCtrl mButton;
    private LocalSave.TimeBoxType chooseType;
    private long localtime;
    private long needtime;
    private int needgold;
    private bool textgetshow = true;
    private bool freetimeshow = true;
    private bool goldshow = true;

    private void Awake()
    {
        this.Text_Get = base.transform.Find("Button/fg/Text_Get").GetComponent<Text>();
        this.Text_FreeGet = base.transform.Find("Button/fg/Text_FreeGet").GetComponent<Text>();
        this.Text_FreeTime = base.transform.Find("Button/Text_FreeTime").GetComponent<Text>();
        this.mGoldCtrl = base.transform.Find("Button/fg/ResourceText").GetComponent<GoldTextCtrl>();
        this.mButton = base.transform.Find("Button").GetComponent<ButtonCtrl>();
        this.mButton.onClick = new Action(this.OnClickButton);
    }

    public void Init(LocalSave.TimeBoxType type)
    {
        this.chooseType = type;
        if ((type == LocalSave.TimeBoxType.BoxChoose_DiamondNormal) || (type == LocalSave.TimeBoxType.BoxChoose_DiamondLarge))
        {
            this.Text_Get.text = GameLogic.Hold.Language.GetLanguageByTID("BoxChoose_Name1", Array.Empty<object>());
        }
        else
        {
            this.Text_Get.text = GameLogic.Hold.Language.GetLanguageByTID("BoxChoose_Name10", Array.Empty<object>());
        }
        this.needgold = GameConfig.GetBoxChooseGold(this.chooseType);
        this.mGoldCtrl.SetValue(this.needgold);
        this.SetTextGetShow(false);
        this.SetFreeTimeShow(false);
        this.SetGoldShow(true);
        this.needtime = GameConfig.GetBoxChooseTime(this.chooseType);
        this.UpdateBoxChooseTime();
    }

    private void OnClickButton()
    {
    }

    private void SetFreeTimeShow(bool value)
    {
        if (this.freetimeshow != value)
        {
            this.freetimeshow = value;
            if (this.Text_FreeTime != null)
            {
                this.Text_FreeTime.gameObject.SetActive(value);
            }
        }
    }

    private void SetGoldShow(bool value)
    {
        if (this.goldshow != value)
        {
            this.goldshow = value;
            if (this.mGoldCtrl != null)
            {
                this.mGoldCtrl.gameObject.SetActive(value);
            }
        }
    }

    private void SetTextGetShow(bool value)
    {
        if (this.textgetshow != value)
        {
            this.textgetshow = value;
            if (this.Text_Get != null)
            {
                this.Text_FreeGet.gameObject.SetActive(value);
            }
        }
    }

    private void Update()
    {
        if (!LocalSave.Instance.IsTimeBoxMax(this.chooseType))
        {
            long num = this.needtime - (Utils.GetTimeStamp() - this.localtime);
            if (num > 0L)
            {
                string str = Utils.GetSecond3String(num / 0x3e8L);
                object[] args = new object[] { str };
                this.Text_FreeTime.text = GameLogic.Hold.Language.GetLanguageByTID("BoxChoose_FreeTime", args);
                this.SetTextGetShow(false);
                this.SetFreeTimeShow(true);
                this.SetGoldShow(true);
            }
            else
            {
                this.SetTextGetShow(true);
                this.SetFreeTimeShow(false);
                this.SetGoldShow(false);
            }
        }
    }

    private void UpdateBoxChooseTime()
    {
        this.localtime = LocalSave.Instance.GetTimeBoxTime(this.chooseType);
    }
}

