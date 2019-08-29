using Dxx.Util;
using System;
using System.Collections.Generic;
using TableTool;
using UnityEngine;

public class SpriteManager
{
    public static Dictionary<CurrencyType, string> PropPath;
    private const string ATLAS_UICOMMON = "uicommon";
    private const string ATLAS_MAIN = "mainui";
    private const string ATLAS_BATTLEUIS = "battleuis";
    private const string ATLAS_SKILLICON = "skillicon";
    private const string ATLAS_CARD = "cardui";
    private const string ATLAS_CHARUI = "charui";
    private const string ATLAS_EQUIPS = "equips";
    private const string ATLAS_HEROICON = "heroicon";
    private const string ATLAS_SUPERICON = "supericon";
    private const string ATLAS_MAP = "map";

    static SpriteManager()
    {
        Dictionary<CurrencyType, string> dictionary = new Dictionary<CurrencyType, string> {
            { 
                CurrencyType.Gold,
                "Currency_Gold"
            },
            { 
                CurrencyType.Diamond,
                "Currency_Diamond"
            },
            { 
                CurrencyType.Key,
                "Currency_Key"
            },
            { 
                CurrencyType.Free_Ad,
                "Currency_AdRemove"
            },
            { 
                CurrencyType.DiamondBoxNormal,
                "Currency_DiamondBoxNormal"
            },
            { 
                CurrencyType.DiamondBoxLarge,
                "Currency_DiamondBoxLarge"
            },
            { 
                CurrencyType.Equip_Random_Quality_3,
                "Currency_Equip_Random_Quality_3"
            },
            { 
                CurrencyType.Equip_Random_Quality_4,
                "Currency_Equip_Random_Quality_4"
            },
            { 
                CurrencyType.Equip_Random_Quality_5,
                "Currency_Equip_Random_Quality_5"
            },
            { 
                CurrencyType.Equip_Random_Quality_6,
                "Currency_Equip_Random_Quality_6"
            },
            { 
                CurrencyType.Equip_Random_Quality_7,
                "Currency_Equip_Random_Quality_7"
            },
            { 
                CurrencyType.EquipExp_Random,
                "Currency_EquipExp_Random"
            }
        };
        PropPath = dictionary;
    }

    public static Sprite GetBattle(string name) => 
        ResourceManager.GetSprite("battleuis", name);

    public static Sprite GetCard(int id) => 
        GetCard(id.ToString());

    public static Sprite GetCard(string value) => 
        ResourceManager.GetSprite("cardui", value);

    public static Sprite GetCharUI(int id) => 
        GetCard(id.ToString());

    public static Sprite GetCharUI(string value) => 
        ResourceManager.GetSprite("charui", value);

    public static Sprite GetEquip(int equipid) => 
        GetEquip(equipid.ToString());

    public static Sprite GetEquip(string value) => 
        ResourceManager.GetSprite("equips", value);

    public static Sprite GetHeroIcon(int id) => 
        ResourceManager.GetSprite("heroicon", id.ToString());

    public static Sprite GetHeroIcon(string value) => 
        ResourceManager.GetSprite("heroicon", value);

    private static string getIDString(int id)
    {
        if (id < 10)
        {
            object[] args = new object[] { "0", id };
            return Utils.GetString(args);
        }
        return id.ToString();
    }

    public static Sprite GetMain(string name) => 
        ResourceManager.GetSprite("mainui", name);

    public static Sprite GetMap(string spriteName)
    {
        string stylePrefix = GetStylePrefix();
        object[] args = new object[] { "map", stylePrefix };
        return ResourceManager.GetSprite(Utils.GetString(args), spriteName);
    }

    public static Sprite GetSkillIcon(int id) => 
        ResourceManager.GetSprite("skillicon", id.ToString());

    public static Sprite GetSkillIconByID(int skillid)
    {
        Skill_skill beanById = LocalModelManager.Instance.Skill_skill.GetBeanById(skillid);
        if (beanById == null)
        {
            return null;
        }
        int skillIcon = beanById.SkillIcon;
        return ResourceManager.GetSprite("skillicon", skillIcon.ToString());
    }

    public static string GetStylePrefix()
    {
        if (((GameLogic.Release.Mode != null) && (GameLogic.Release.Mode.RoomGenerate != null)) && ((GameLogic.Hold != null) && (GameLogic.Hold.BattleData != null)))
        {
            int currentRoomID = GameLogic.Release.Mode.RoomGenerate.GetCurrentRoomID();
            return LocalModelManager.Instance.Stage_Level_stagechapter.GetStyleString(currentRoomID);
        }
        return "0101";
    }

    public static Sprite GetSuperIcon(int id) => 
        ResourceManager.GetSprite("supericon", id.ToString());

    public static Sprite GetUICommon(string name) => 
        ResourceManager.GetSprite("uicommon", name);

    public static Sprite GetUICommonCurrency(CurrencyType type)
    {
        string str = "Currency_Gold";
        if (!PropPath.TryGetValue(type, out str))
        {
            object[] args = new object[] { type };
            SdkManager.Bugly_Report("SpriteManager", Utils.FormatString("GetUICommonCurrency[{0}] is not in ConstString.PropPath", args));
        }
        return GetUICommon(str);
    }
}

