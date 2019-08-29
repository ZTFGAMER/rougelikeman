using System;
using UnityEngine;
using UnityEngine.UI;

public class LanguageTextLocalizer : MonoBehaviour
{
    public Text text;
    public LanguageKeysEnum key;
    public string addedString = string.Empty;

    public void Localize()
    {
        if (this.text != null)
        {
            this.text.text = LanguageManager.instance.GetLocalizedText(this.key);
            this.text.text = this.text.text + this.addedString;
            this.text.set_font(LanguageManager.instance.currentLanguage.font);
        }
    }
}

