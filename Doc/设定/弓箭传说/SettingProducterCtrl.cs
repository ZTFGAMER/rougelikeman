using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingProducterCtrl : MonoBehaviour
{
    public Text Text_Producter;
    public Text Text_Value;
    public ButtonCtrl Button_producter;

    private void Awake()
    {
        this.Button_producter.onClick = new Action(this.OnClickButton);
        this.UpdateLanguage();
    }

    private void OnClickButton()
    {
        WindowUI.ShowWindow(WindowID.WindowID_Producer);
    }

    public void UpdateLanguage()
    {
        this.Text_Producter.text = GameLogic.Hold.Language.GetLanguageByTID("设置_制作人员", Array.Empty<object>());
        this.Text_Value.text = GameLogic.Hold.Language.GetLanguageByTID("设置_查看", Array.Empty<object>());
    }
}

