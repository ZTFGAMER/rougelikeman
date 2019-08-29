using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingSoundCtrl : MonoBehaviour
{
    public Text Text_Sound;
    public ButtonSwitchCtrl Button_Sound;

    private void Awake()
    {
        this.Button_Sound.onClick = new Action(this.OnClickButton);
        this.UpdateShow();
    }

    private void OnClickButton()
    {
        GameLogic.Hold.Sound.ChangeSound();
        this.UpdateShow();
    }

    public void UpdateLanguage()
    {
        this.UpdateShow();
    }

    private void UpdateShow()
    {
        if ((GameLogic.Hold != null) && (GameLogic.Hold.Sound != null))
        {
            this.Button_Sound.SetSwitch(GameLogic.Hold.Sound.GetSound());
        }
        this.Text_Sound.text = GameLogic.Hold.Language.GetLanguageByTID("设置_音效", Array.Empty<object>());
    }
}

