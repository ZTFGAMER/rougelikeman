using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static LanguageManager <instance>k__BackingField;
    public List<Language> languages;
    public readonly SystemLanguage defaultLanguage = SystemLanguage.English;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private Language <currentLanguage>k__BackingField;
    private List<string> defaultLanguageString;
    private List<string> currentLanguageString;
    [CompilerGenerated]
    private static Func<Language, bool> <>f__am$cache0;
    [CompilerGenerated]
    private static Func<Language, bool> <>f__am$cache1;

    private void Awake()
    {
        if ((instance != null) && (instance != this))
        {
            UnityEngine.Object.Destroy(base.gameObject);
        }
        else
        {
            instance = this;
            UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
        }
        if (PlayerPrefs.HasKey(StaticConstants.lastSelectedLanguage))
        {
            this.SetCurrentLanguage((SystemLanguage) PlayerPrefs.GetInt(StaticConstants.lastSelectedLanguage));
        }
        else
        {
            this.SetCurrentLanguage(Application.systemLanguage);
        }
    }

    public void CheckLocalizedValues()
    {
        if (this.currentLanguageString.Count < this.defaultLanguageString.Count)
        {
            for (int i = this.currentLanguageString.Count; i < this.defaultLanguageString.Count; i++)
            {
                this.currentLanguageString.Add(this.defaultLanguageString[i]);
                UnityEngine.Debug.LogWarning("TextNotLocalized: " + this.defaultLanguageString[i]);
            }
        }
    }

    public string GetLocalizedText(LanguageKeysEnum key) => 
        this.currentLanguageString[(int) key];

    public string GetLocalizedText(string defaultLanguageText)
    {
        for (int i = 0; i < this.defaultLanguageString.Count; i++)
        {
            if (defaultLanguageText.ToUpper() == this.defaultLanguageString[i].ToUpper())
            {
                return this.currentLanguageString[i];
            }
        }
        for (int j = 0; j < this.currentLanguageString.Count; j++)
        {
            if (defaultLanguageText.ToUpper() == this.currentLanguageString[j].ToUpper())
            {
                return defaultLanguageText;
            }
        }
        UnityEngine.Debug.LogWarning("TextNotLocalized: " + defaultLanguageText);
        return defaultLanguageText;
    }

    public bool IsReverseLanguage(SystemLanguage language) => 
        ((language == SystemLanguage.ChineseSimplified) || ((language == SystemLanguage.ChineseTraditional) || ((language == SystemLanguage.Korean) || (language == SystemLanguage.Japanese))));

    public void SetCurrentLanguage(SystemLanguage language)
    {
        <SetCurrentLanguage>c__AnonStorey0 storey = new <SetCurrentLanguage>c__AnonStorey0 {
            language = language
        };
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = l => l.language == SystemLanguage.English;
        }
        char[] separator = new char[] { '\n' };
        this.defaultLanguageString = Enumerable.First<Language>(this.languages, <>f__am$cache0).languageTextAsset.text.Split(separator).ToList<string>();
        if (Enumerable.Any<Language>(this.languages, new Func<Language, bool>(storey.<>m__0)))
        {
            this.currentLanguage = Enumerable.First<Language>(this.languages, new Func<Language, bool>(storey.<>m__1));
        }
        else
        {
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = l => l.language == SystemLanguage.English;
            }
            this.currentLanguage = Enumerable.First<Language>(this.languages, <>f__am$cache1);
        }
        UnityEngine.Debug.Log("Language set: " + this.currentLanguage.language);
        char[] chArray2 = new char[] { '\n' };
        this.currentLanguageString = this.currentLanguage.languageTextAsset.text.Split(chArray2).ToList<string>();
        PlayerPrefs.SetInt(StaticConstants.lastSelectedLanguage, (int) this.currentLanguage.language);
        PlayerPrefs.Save();
        this.CheckLocalizedValues();
        this.UpdateLocalizers();
    }

    public void UpdateLocalizers()
    {
        LanguageTextLocalizer[] localizerArray = Resources.FindObjectsOfTypeAll<LanguageTextLocalizer>();
        for (int i = 0; i < localizerArray.Length; i++)
        {
            localizerArray[i].Localize();
        }
        if (DataLoader.initialized)
        {
            DataLoader.gui.achievementsPanel.UpdateLocalization();
        }
    }

    public static LanguageManager instance
    {
        [CompilerGenerated]
        get => 
            <instance>k__BackingField;
        [CompilerGenerated]
        private set => 
            (<instance>k__BackingField = value);
    }

    public Language currentLanguage { get; private set; }

    [CompilerGenerated]
    private sealed class <SetCurrentLanguage>c__AnonStorey0
    {
        internal SystemLanguage language;

        internal bool <>m__0(Language l) => 
            (l.language == this.language);

        internal bool <>m__1(Language l) => 
            (l.language == this.language);
    }
}

