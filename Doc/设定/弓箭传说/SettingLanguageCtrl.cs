using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingLanguageCtrl : MonoBehaviour
{
    private static List<string> languagelist = new List<string>();
    public Text Text_LanguageContent;
    public Text Text_Language;
    public ButtonCtrl Button_Language;

    private void Awake()
    {
        this.Button_Language.onClick = new Action(this.OnClickButton);
        this.UpdateLanguage();
    }

    public static List<string> GetLanguageList()
    {
        if (languagelist.Count == 0)
        {
            Dictionary<string, string>.Enumerator enumerator = LanguageManager.languagedic.GetEnumerator();
            while (enumerator.MoveNext())
            {
                KeyValuePair<string, string> current = enumerator.Current;
                languagelist.Add(current.Key);
            }
        }
        return languagelist;
    }

    private void OnClickButton()
    {
        WindowUI.ShowWindow(WindowID.WindowID_Language);
    }

    public void UpdateLanguage()
    {
        this.Text_LanguageContent.text = GameLogic.Hold.Language.GetLanguageByTID("设置_语言", Array.Empty<object>());
        string languageString = GameLogic.Hold.Language.GetLanguageString();
        if (LanguageManager.languagedic.ContainsKey(languageString))
        {
            this.Text_Language.text = LanguageManager.languagedic[languageString];
        }
        else
        {
            this.Text_Language.text = LanguageManager.languagedic["EN"];
        }
    }
}

