using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class LanguageOneCtrl : MonoBehaviour
{
    public Text Text_Language;
    public ButtonCtrl Button_Language;
    public GameObject fg;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string <mLanguage>k__BackingField;
    public Action<LanguageOneCtrl> OnClickButton;

    private void Awake()
    {
        this.Button_Language.onClick = delegate {
            if (this.OnClickButton != null)
            {
                this.OnClickButton(this);
            }
        };
    }

    public void Init(int index, string language)
    {
        this.mLanguage = language;
        this.Text_Language.text = LanguageManager.languagedic[language];
        this.UpdateChoose();
    }

    private void UpdateChoose()
    {
        this.fg.SetActive(this.mLanguage == GameLogic.Hold.Language.GetLanguageString());
    }

    public string mLanguage { get; private set; }
}

