using System;
using UnityEngine;

public class UILanguagePanel : MonoBehaviour
{
    public UIOptions options;
    public UILanguageCell[] languageCells;

    public void Reset()
    {
        this.options.UpdateLanguageText();
        for (int i = 0; i < this.languageCells.Length; i++)
        {
            this.languageCells[i].selectedImage.SetActive(false);
        }
    }

    public void SelectLanguage(SystemLanguage language)
    {
        for (int i = 0; i < this.languageCells.Length; i++)
        {
            if (language == this.languageCells[i].selectedLanguage)
            {
                this.languageCells[i].selectedImage.SetActive(true);
                break;
            }
        }
    }

    public void Start()
    {
        this.Reset();
        this.SelectLanguage((SystemLanguage) PlayerPrefs.GetInt(StaticConstants.lastSelectedLanguage));
    }
}

