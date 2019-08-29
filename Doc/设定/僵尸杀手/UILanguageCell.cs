using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UILanguageCell : MonoBehaviour
{
    public GameObject selectedImage;
    public SystemLanguage selectedLanguage;
    public Text languageText;

    public void Select()
    {
        LanguageManager.instance.SetCurrentLanguage(this.selectedLanguage);
        DataLoader.gui.UpdateMenuContent();
        DataLoader.Instance.UpdateClosedWallsText();
        UnityEngine.Object.FindObjectOfType<UILanguagePanel>().Reset();
        this.selectedImage.SetActive(true);
    }

    public void SetLanguageText()
    {
        Language language = Enumerable.First<Language>(LanguageManager.instance.languages, l => l.language == this.selectedLanguage);
        this.languageText.text = language.languageName;
        this.languageText.set_font(language.font);
    }

    public void Start()
    {
        this.SetLanguageText();
    }
}

