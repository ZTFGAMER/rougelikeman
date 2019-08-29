using Dxx.Util;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TableTool;
using UnityEngine;

public class LanguageManager
{
    public const string CN_s = "CN_s";
    public const string CN_t = "CN_t";
    public const string EN = "EN";
    public const string FR = "FR";
    public const string DE = "DE";
    public const string ID = "ID";
    public const string JP = "JP";
    public const string KR = "KR";
    public const string PT_BR = "PT_BR";
    public const string RU = "RU";
    public const string ES_ES = "ES_ES";
    public static Dictionary<string, string> languagedic;
    public static Dictionary<SystemLanguage, string> m_LanguageIDMap;
    private SystemLanguage CurrentLanguage;
    private Dictionary<string, Language_lauguage> m_LanguageList;
    private int argsLength;
    private int geti;
    private string containstring;
    private string currentstring;
    private Dictionary<int, string> mStageIndexs;
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$map6;

    static LanguageManager()
    {
        Dictionary<string, string> dictionary = new Dictionary<string, string> {
            { 
                "EN",
                "English"
            },
            { 
                "CN_s",
                "简体中文"
            },
            { 
                "KR",
                "한국어"
            },
            { 
                "JP",
                "日本語"
            },
            { 
                "FR",
                "Fran\x00e7ais"
            },
            { 
                "DE",
                "Deutsch"
            },
            { 
                "ES_ES",
                "Espa\x00f1ol"
            },
            { 
                "PT_BR",
                "Portugu\x00eas"
            },
            { 
                "ID",
                "Bahasa Indonesia"
            },
            { 
                "RU",
                "русский"
            },
            { 
                "CN_t",
                "繁體中文"
            }
        };
        languagedic = dictionary;
        Dictionary<SystemLanguage, string> dictionary2 = new Dictionary<SystemLanguage, string> {
            { 
                SystemLanguage.Chinese,
                "CN_s"
            },
            { 
                SystemLanguage.ChineseSimplified,
                "CN_s"
            },
            { 
                SystemLanguage.ChineseTraditional,
                "CN_t"
            },
            { 
                SystemLanguage.English,
                "EN"
            },
            { 
                SystemLanguage.Afrikaans,
                "EN"
            },
            { 
                SystemLanguage.Arabic,
                "EN"
            },
            { 
                SystemLanguage.Basque,
                "EN"
            },
            { 
                SystemLanguage.Belarusian,
                "RU"
            },
            { 
                SystemLanguage.Bulgarian,
                "EN"
            },
            { 
                SystemLanguage.Catalan,
                "EN"
            },
            { 
                SystemLanguage.Czech,
                "EN"
            },
            { 
                SystemLanguage.Danish,
                "EN"
            },
            { 
                SystemLanguage.Dutch,
                "EN"
            },
            { 
                SystemLanguage.Estonian,
                "EN"
            },
            { 
                SystemLanguage.Faroese,
                "EN"
            },
            { 
                SystemLanguage.Finnish,
                "EN"
            },
            { 
                SystemLanguage.French,
                "FR"
            },
            { 
                SystemLanguage.German,
                "DE"
            },
            { 
                SystemLanguage.Greek,
                "EN"
            },
            { 
                SystemLanguage.Hebrew,
                "EN"
            },
            { 
                SystemLanguage.Hungarian,
                "EN"
            },
            { 
                SystemLanguage.Icelandic,
                "EN"
            },
            { 
                SystemLanguage.Indonesian,
                "ID"
            },
            { 
                SystemLanguage.Italian,
                "EN"
            },
            { 
                SystemLanguage.Japanese,
                "JP"
            },
            { 
                SystemLanguage.Korean,
                "KR"
            },
            { 
                SystemLanguage.Latvian,
                "EN"
            },
            { 
                SystemLanguage.Lithuanian,
                "EN"
            },
            { 
                SystemLanguage.Norwegian,
                "EN"
            },
            { 
                SystemLanguage.Polish,
                "EN"
            },
            { 
                SystemLanguage.Portuguese,
                "PT_BR"
            },
            { 
                SystemLanguage.Romanian,
                "EN"
            },
            { 
                SystemLanguage.Russian,
                "RU"
            },
            { 
                SystemLanguage.SerboCroatian,
                "EN"
            },
            { 
                SystemLanguage.Slovak,
                "EN"
            },
            { 
                SystemLanguage.Slovenian,
                "EN"
            },
            { 
                SystemLanguage.Spanish,
                "ES_ES"
            },
            { 
                SystemLanguage.Swedish,
                "EN"
            },
            { 
                SystemLanguage.Thai,
                "EN"
            },
            { 
                SystemLanguage.Turkish,
                "EN"
            },
            { 
                SystemLanguage.Ukrainian,
                "EN"
            },
            { 
                SystemLanguage.Unknown,
                "EN"
            },
            { 
                SystemLanguage.Vietnamese,
                "EN"
            }
        };
        m_LanguageIDMap = dictionary2;
    }

    public LanguageManager()
    {
        Dictionary<int, string> dictionary = new Dictionary<int, string> {
            { 
                1,
                "I"
            },
            { 
                2,
                "II"
            },
            { 
                3,
                "III"
            },
            { 
                4,
                "IV"
            },
            { 
                5,
                "V"
            },
            { 
                6,
                "VI"
            },
            { 
                7,
                "VII"
            },
            { 
                8,
                "VIII"
            },
            { 
                9,
                "IX"
            },
            { 
                10,
                "X"
            },
            { 
                11,
                "XI"
            },
            { 
                12,
                "XII"
            }
        };
        this.mStageIndexs = dictionary;
        this.m_LanguageList = LocalModelManager.Instance.Language_lauguage.GetBeanDic();
        int @int = PlayerPrefsEncrypt.GetInt("LocalLanguage", -1);
        if (@int < 0)
        {
            this.CurrentLanguage = Application.systemLanguage;
            if (this.CurrentLanguage == SystemLanguage.Chinese)
            {
                this.CurrentLanguage = SystemLanguage.ChineseSimplified;
            }
        }
        else
        {
            this.CurrentLanguage = (SystemLanguage) @int;
        }
    }

    public void ChangeLanguage(string language)
    {
        Dictionary<SystemLanguage, string>.Enumerator enumerator = m_LanguageIDMap.GetEnumerator();
        while (enumerator.MoveNext())
        {
            KeyValuePair<SystemLanguage, string> current = enumerator.Current;
            if (current.Value == language)
            {
                KeyValuePair<SystemLanguage, string> pair2 = enumerator.Current;
                this.CurrentLanguage = pair2.Key;
                Debug.Log("change language -> " + this.CurrentLanguage);
                PlayerPrefsEncrypt.SetInt("LocalLanguage", (int) this.CurrentLanguage);
                Facade.Instance.SendNotification("PUB_LANGUAGE_UPDATE");
                break;
            }
        }
    }

    public string GetEquipSpecialInfo(int equipid)
    {
        object[] args = new object[] { equipid };
        string key = Utils.FormatString("装备特性描述{0}", args);
        string str2 = string.Empty;
        Language_lauguage _lauguage = null;
        if (this.m_LanguageList.TryGetValue(key, out _lauguage))
        {
            str2 = this.GetString(_lauguage);
        }
        return str2;
    }

    public SystemLanguage GetLanguage() => 
        this.CurrentLanguage;

    public string GetLanguageByTID(string tid, params object[] args)
    {
        this.argsLength = args.Length;
        Language_lauguage _lauguage = null;
        this.m_LanguageList.TryGetValue(tid, out _lauguage);
        if (_lauguage == null)
        {
            object[] objArray1 = new object[] { tid };
            SdkManager.Bugly_Report("GetLanguageByTID", Utils.FormatString("TID:[{0}] is not invalid.", objArray1));
            return string.Empty;
        }
        this.currentstring = this.GetString(_lauguage);
        this.geti = 1;
        while (this.geti <= this.argsLength)
        {
            object[] objArray2 = new object[] { "%s", this.geti };
            this.containstring = Utils.GetString(objArray2);
            if (this.currentstring.Contains(this.containstring))
            {
                this.currentstring = this.currentstring.Replace(this.containstring, args[this.geti - 1].ToString());
            }
            else
            {
                SdkManager.Bugly_Report("LanguageManager.GetLanguageByTID", this.CurrentLanguage.ToString() + " tid:" + tid + " dont have " + this.containstring);
            }
            this.geti++;
        }
        object[] objArray3 = new object[] { "%s", this.geti };
        if (this.currentstring.Contains(Utils.GetString(objArray3)))
        {
            SdkManager.Bugly_Report("LanguageManager.GetLanguageByTID", " tid:" + tid + " need more args!!!!");
        }
        this.currentstring = this.currentstring.Replace(@"\n", "\n");
        return this.currentstring;
    }

    public string GetLanguageString()
    {
        if (m_LanguageIDMap.ContainsKey(this.CurrentLanguage))
        {
            return m_LanguageIDMap[this.CurrentLanguage];
        }
        return "EN";
    }

    public string GetRomanNumber(int value)
    {
        string str = string.Empty;
        if (this.mStageIndexs.TryGetValue(value, out str))
        {
            return str;
        }
        return string.Empty;
    }

    public string GetSecond(int second)
    {
        object[] args = new object[] { second };
        return this.GetLanguageByTID("倒计时_s", args);
    }

    public string GetSkillContent(int skillId)
    {
        int skillIcon = LocalModelManager.Instance.Skill_skill.GetBeanById(skillId).SkillIcon;
        object[] args = new object[] { "技能描述", skillIcon };
        return this.GetLanguageByTID(Utils.GetString(args), Array.Empty<object>());
    }

    public string GetSkillName(int skillId)
    {
        int skillIcon = LocalModelManager.Instance.Skill_skill.GetBeanById(skillId).SkillIcon;
        object[] args = new object[] { "技能名称", skillIcon };
        return this.GetLanguageByTID(Utils.GetString(args), Array.Empty<object>());
    }

    public string GetStageLayer(int MaxLevel)
    {
        LocalSave.Instance.mStage.GetLayerBoxStageLayer(MaxLevel, out int num, out int num2);
        object[] args = new object[] { num, num2 };
        return this.GetLanguageByTID("stagelist_stage_layer", args);
    }

    private string GetString(Language_lauguage language)
    {
        if (!m_LanguageIDMap.ContainsKey(this.CurrentLanguage))
        {
            return language.EN;
        }
        string key = m_LanguageIDMap[this.CurrentLanguage];
        if (key != null)
        {
            if (<>f__switch$map6 == null)
            {
                Dictionary<string, int> dictionary = new Dictionary<string, int>(11) {
                    { 
                        "CN_s",
                        0
                    },
                    { 
                        "CN_t",
                        1
                    },
                    { 
                        "EN",
                        2
                    },
                    { 
                        "FR",
                        3
                    },
                    { 
                        "DE",
                        4
                    },
                    { 
                        "ID",
                        5
                    },
                    { 
                        "JP",
                        6
                    },
                    { 
                        "KR",
                        7
                    },
                    { 
                        "PT_BR",
                        8
                    },
                    { 
                        "RU",
                        9
                    },
                    { 
                        "ES_ES",
                        10
                    }
                };
                <>f__switch$map6 = dictionary;
            }
            if (<>f__switch$map6.TryGetValue(key, out int num))
            {
                switch (num)
                {
                    case 0:
                        return language.CN_s;

                    case 1:
                        return language.CN_t;

                    case 2:
                        return language.EN;

                    case 3:
                        return language.FR;

                    case 4:
                        return language.DE;

                    case 5:
                        return language.ID;

                    case 6:
                        return language.JP;

                    case 7:
                        return language.KR;

                    case 8:
                        return language.PT_BR;

                    case 9:
                        return language.RU;

                    case 10:
                        return language.ES_ES;
                }
            }
        }
        return string.Empty;
    }

    public string Level =>
        this.GetLanguageByTID("EquipUI_Level", Array.Empty<object>());

    public string Count =>
        this.GetLanguageByTID("EquipUI_Count", Array.Empty<object>());
}

