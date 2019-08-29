using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSwitchCtrl : ButtonCtrl
{
    public Image Image_Icon;
    public Text Text_Value;
    public Sprite Sprite_Open;
    public Sprite Sprite_Close;
    public string LanguageTID_Open;
    public string LanguageTID_Close;

    public void SetSwitch(bool value)
    {
        this.Image_Icon.set_sprite(!value ? this.Sprite_Close : this.Sprite_Open);
        this.Text_Value.text = GameLogic.Hold.Language.GetLanguageByTID(!value ? this.LanguageTID_Close : this.LanguageTID_Open, Array.Empty<object>());
    }
}

