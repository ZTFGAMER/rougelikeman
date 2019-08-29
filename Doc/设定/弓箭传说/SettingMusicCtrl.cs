using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingMusicCtrl : MonoBehaviour
{
    public Text Text_Music;
    public ButtonSwitchCtrl Button_Music;

    private void Awake()
    {
        this.Button_Music.onClick = new Action(this.OnClickButton);
        this.UpdateShow();
    }

    private void OnClickButton()
    {
        GameLogic.Hold.Sound.ChangeMusic();
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
            this.Button_Music.SetSwitch(GameLogic.Hold.Sound.GetMusic());
        }
        this.Text_Music.text = GameLogic.Hold.Language.GetLanguageByTID("设置_音乐", Array.Empty<object>());
    }
}

