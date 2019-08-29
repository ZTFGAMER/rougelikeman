using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingReportCtrl : MonoBehaviour
{
    public ButtonCtrl Button_Report;
    public Text Text_Report;

    private void Awake()
    {
        this.Button_Report.onClick = new Action(this.OnClickButton);
        this.UpdateShow();
    }

    private void OnClickButton()
    {
        WindowUI.ShowWindow(WindowID.WindowID_Report);
    }

    public void UpdateLanguage()
    {
        this.UpdateShow();
    }

    private void UpdateShow()
    {
        this.Text_Report.text = GameLogic.Hold.Language.GetLanguageByTID("setting_report", Array.Empty<object>());
    }
}

