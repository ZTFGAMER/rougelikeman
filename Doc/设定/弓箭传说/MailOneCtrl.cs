using Dxx.Util;
using GameProtocol;
using System;
using UnityEngine;
using UnityEngine.UI;

public class MailOneCtrl : MonoBehaviour
{
    public Text Text_Name;
    public Text Text_Info;
    public Text Text_Time;
    public Text Text_New;
    public RedNodeCtrl m_RedCtrl;
    public ButtonCtrl Button_Open;
    public CanvasGroup mCanvasGroup;
    public ContentSizeFitter InfoFitter;
    public Action<int, MailOneCtrl> OnClickButton;
    private CMailInfo mData;
    private int mIndex;

    private void Awake()
    {
        this.Button_Open.onClick = delegate {
            if (this.OnClickButton != null)
            {
                this.OnClickButton(this.mIndex, this);
                this.UpdateMail();
            }
        };
    }

    public void Init(int index, CMailInfo data)
    {
        this.mData = data;
        this.mCanvasGroup.alpha = 1f;
        this.mIndex = index;
        this.Text_Name.text = this.mData.m_strTitle;
        this.Text_Time.text = Utils.GetTimeGo((double) this.mData.m_i64PubTime);
        this.Text_Info.text = Utils.CutString(this.mData.m_strContent, 0x19);
        this.Text_New.text = string.Empty;
        this.UpdateMail();
    }

    public void SetRedShow(bool value)
    {
        this.m_RedCtrl.SetType(RedNodeType.eRedNew);
        this.m_RedCtrl.Value = !value ? 0 : 1;
    }

    public void UpdateMail()
    {
        this.SetRedShow(this.mData.IsShowRed);
    }
}

