using System;
using System.Collections.Generic;

public class UIResourceDefine
{
    public static Dictionary<WindowID, WindowData> windowClass;
    public static string UIPrefabPath;

    static UIResourceDefine()
    {
        Dictionary<WindowID, WindowData> dictionary = new Dictionary<WindowID, WindowData>();
        WindowData data = new WindowData {
            ClassName = "MainModuleMediator",
            LayerType = WindowMediator.LayerType.eRoot
        };
        dictionary.Add(WindowID.WindowID_Main, data);
        data = new WindowData {
            ClassName = "BattleModuleMediator",
            LayerType = WindowMediator.LayerType.eInGame,
            State = 1
        };
        dictionary.Add(WindowID.WindowID_Battle, data);
        data = new WindowData {
            ClassName = "BoxChooseMediator",
            LayerType = WindowMediator.LayerType.eFrontEvent,
            isPop = true
        };
        dictionary.Add(WindowID.WindowID_BoxChoose, data);
        data = new WindowData {
            ClassName = "GameOverModuleMediator",
            LayerType = WindowMediator.LayerType.eInGame,
            State = 1
        };
        dictionary.Add(WindowID.WindowID_GameOver, data);
        data = new WindowData {
            ClassName = "LoadingModuleMediator",
            LayerType = WindowMediator.LayerType.eFrontMask
        };
        dictionary.Add(WindowID.WindowID_Loading, data);
        data = new WindowData {
            ClassName = "SettingMediator",
            LayerType = WindowMediator.LayerType.eFrontEvent,
            isPop = true
        };
        dictionary.Add(WindowID.WindowID_Setting, data);
        data = new WindowData {
            ClassName = "ChangeAccountMediator",
            LayerType = WindowMediator.LayerType.eFrontMask,
            State = 2,
            isPop = true
        };
        dictionary.Add(WindowID.WindowID_ChangeAccount, data);
        data = new WindowData {
            ClassName = "CharModuleMediator",
            LayerType = WindowMediator.LayerType.eRoot
        };
        dictionary.Add(WindowID.WindowID_Char, data);
        data = new WindowData {
            ClassName = "MaskModuleMediator",
            LayerType = WindowMediator.LayerType.eFrontMask,
            State = 2
        };
        dictionary.Add(WindowID.WindowID_Mask, data);
        data = new WindowData {
            ClassName = "GuideModuleMediator",
            LayerType = WindowMediator.LayerType.eFront2
        };
        dictionary.Add(WindowID.WindowID_Guide, data);
        data = new WindowData {
            ClassName = "ChooseSkillModuleMediator",
            LayerType = WindowMediator.LayerType.eInGame,
            State = 1
        };
        dictionary.Add(WindowID.WindowID_ChooseSkill, data);
        data = new WindowData {
            ClassName = "GameTurnTableMediator",
            LayerType = WindowMediator.LayerType.eInGame,
            State = 1
        };
        dictionary.Add(WindowID.WindowID_GameTurnTable, data);
        data = new WindowData {
            ClassName = "GameThreeMediator",
            LayerType = WindowMediator.LayerType.eInGame,
            State = 1
        };
        dictionary.Add(WindowID.WindowID_GameThree, data);
        data = new WindowData {
            ClassName = "EventAngelMediator",
            LayerType = WindowMediator.LayerType.eInGame,
            State = 1
        };
        dictionary.Add(WindowID.WindowID_EventAngel, data);
        data = new WindowData {
            ClassName = "EventBlackShopMediator",
            LayerType = WindowMediator.LayerType.eInGame,
            State = 1,
            isPop = true
        };
        dictionary.Add(WindowID.WindowID_EventBlackShop, data);
        data = new WindowData {
            ClassName = "EventFirstShopMediator",
            LayerType = WindowMediator.LayerType.eInGame,
            State = 1,
            isPop = true
        };
        dictionary.Add(WindowID.WindowID_EventFirstShop, data);
        data = new WindowData {
            ClassName = "EventDemonMediator",
            LayerType = WindowMediator.LayerType.eInGame,
            State = 1
        };
        dictionary.Add(WindowID.WindowID_EventDemon, data);
        data = new WindowData {
            ClassName = "EquipWearModuleMediator",
            LayerType = WindowMediator.LayerType.eRoot
        };
        dictionary.Add(WindowID.WindowID_EquipWear, data);
        data = new WindowData {
            ClassName = "EventChest1Mediator",
            LayerType = WindowMediator.LayerType.eInGame,
            State = 1
        };
        dictionary.Add(WindowID.WindowID_EventChect1, data);
        data = new WindowData {
            ClassName = "BattleLoadMediator",
            LayerType = WindowMediator.LayerType.eFrontMask,
            State = 2
        };
        dictionary.Add(WindowID.WindowID_BattleLoad, data);
        data = new WindowData {
            ClassName = "UnlockStageMediator",
            LayerType = WindowMediator.LayerType.eFront2
        };
        dictionary.Add(WindowID.WindowID_UnlockStage, data);
        data = new WindowData {
            ClassName = "PopWindowMediator",
            LayerType = WindowMediator.LayerType.eFrontEvent,
            State = 2,
            isPop = true
        };
        dictionary.Add(WindowID.WindowID_PopWindow, data);
        data = new WindowData {
            ClassName = "ActiveMediator",
            LayerType = WindowMediator.LayerType.eRoot
        };
        dictionary.Add(WindowID.WindowID_Active, data);
        data = new WindowData {
            ClassName = "PauseModuleMediator",
            LayerType = WindowMediator.LayerType.eInGame,
            State = 1,
            isPop = true
        };
        dictionary.Add(WindowID.WindowID_Pause, data);
        data = new WindowData {
            ClassName = "RateModuleMediator",
            LayerType = WindowMediator.LayerType.eInGame,
            State = 1,
            isPop = true
        };
        dictionary.Add(WindowID.WindowID_Rate, data);
        data = new WindowData {
            ClassName = "CurrencyModuleMediator",
            LayerType = WindowMediator.LayerType.eFront2,
            State = 3
        };
        dictionary.Add(WindowID.WindowID_Currency, data);
        data = new WindowData {
            ClassName = "EquipInfoModuleMediator",
            LayerType = WindowMediator.LayerType.eFrontEvent,
            State = 2,
            isPop = true
        };
        dictionary.Add(WindowID.WindowID_EquipInfo, data);
        data = new WindowData {
            ClassName = "CurrencyEquipMediator",
            LayerType = WindowMediator.LayerType.eFront,
            State = 2
        };
        dictionary.Add(WindowID.WindowID_CurrencyEquip, data);
        data = new WindowData {
            ClassName = "GoldBuyMediator",
            LayerType = WindowMediator.LayerType.eFrontEvent,
            isPop = true,
            State = 2
        };
        dictionary.Add(WindowID.WindowID_GoldBuy, data);
        data = new WindowData {
            ClassName = "KeyBuyMediator",
            LayerType = WindowMediator.LayerType.eFrontEvent,
            isPop = true
        };
        dictionary.Add(WindowID.WindowID_KeyBuy, data);
        data = new WindowData {
            ClassName = "EquipCombineMediator",
            LayerType = WindowMediator.LayerType.eFront
        };
        dictionary.Add(WindowID.WindowID_EquipCombine, data);
        data = new WindowData {
            ClassName = "CurrencyBattleKeyMediator",
            LayerType = WindowMediator.LayerType.eRoot,
            State = 2
        };
        dictionary.Add(WindowID.WindowID_CurrencyBattleKey, data);
        data = new WindowData {
            ClassName = "LevelUpMediator",
            LayerType = WindowMediator.LayerType.eFrontEvent,
            State = 2,
            isPop = true
        };
        dictionary.Add(WindowID.WindowID_LevelUp, data);
        data = new WindowData {
            ClassName = "RewardShowMediator",
            LayerType = WindowMediator.LayerType.eFront2
        };
        dictionary.Add(WindowID.WindowID_RewardShow, data);
        data = new WindowData {
            ClassName = "EventRewardTurnMediator",
            LayerType = WindowMediator.LayerType.eInGame,
            State = 1,
            isPop = true
        };
        dictionary.Add(WindowID.WindowID_EventRewardTurn, data);
        data = new WindowData {
            ClassName = "LoginMediator",
            LayerType = WindowMediator.LayerType.eFrontNet
        };
        dictionary.Add(WindowID.WindowID_Login, data);
        data = new WindowData {
            ClassName = "CardModuleMediator",
            LayerType = WindowMediator.LayerType.eRoot
        };
        dictionary.Add(WindowID.WindowID_Card, data);
        data = new WindowData {
            ClassName = "EquipCombineUpMediator",
            LayerType = WindowMediator.LayerType.eFront2
        };
        dictionary.Add(WindowID.WindowID_EquipCombineUp, data);
        data = new WindowData {
            ClassName = "AdHarvestMediator",
            LayerType = WindowMediator.LayerType.eFrontEvent,
            isPop = true
        };
        dictionary.Add(WindowID.WindowID_AdHarvest, data);
        data = new WindowData {
            ClassName = "BoxOpenModuleMediator",
            LayerType = WindowMediator.LayerType.eFront2
        };
        dictionary.Add(WindowID.WindowID_BoxOpen, data);
        data = new WindowData {
            ClassName = "CardLevelUpMediator",
            LayerType = WindowMediator.LayerType.eFront2
        };
        dictionary.Add(WindowID.WindowID_CardLevelUp, data);
        data = new WindowData {
            ClassName = "ChallengeMediator",
            LayerType = WindowMediator.LayerType.eFrontEvent,
            isPop = true
        };
        dictionary.Add(WindowID.WindowID_Challenge, data);
        data = new WindowData {
            ClassName = "BoxOpenOneMediator",
            LayerType = WindowMediator.LayerType.eFront3
        };
        dictionary.Add(WindowID.WindowID_BoxOpenOne, data);
        data = new WindowData {
            ClassName = "AchieveMediator",
            LayerType = WindowMediator.LayerType.eFrontEvent,
            isPop = true
        };
        dictionary.Add(WindowID.WindowID_Achieve, data);
        data = new WindowData {
            ClassName = "MailMediator",
            LayerType = WindowMediator.LayerType.eFrontEvent,
            isPop = true
        };
        dictionary.Add(WindowID.WindowID_Mail, data);
        data = new WindowData {
            ClassName = "MailInfoMediator",
            LayerType = WindowMediator.LayerType.eFrontEvent,
            isPop = true
        };
        dictionary.Add(WindowID.WindowID_MailInfo, data);
        data = new WindowData {
            ClassName = "ShopMediator",
            LayerType = WindowMediator.LayerType.eFrontEvent,
            isPop = true
        };
        dictionary.Add(WindowID.WindowID_Shop, data);
        data = new WindowData {
            ClassName = "ShopSingleMediator",
            LayerType = WindowMediator.LayerType.eFront3,
            isPop = true,
            State = 2
        };
        dictionary.Add(WindowID.WindowID_ShopSingle, data);
        data = new WindowData {
            ClassName = "SettingDebugMediator",
            LayerType = WindowMediator.LayerType.eFrontEvent,
            isPop = true
        };
        dictionary.Add(WindowID.WindowID_SettingDebug, data);
        data = new WindowData {
            ClassName = "LanguageMediator",
            LayerType = WindowMediator.LayerType.eFrontEvent,
            isPop = true
        };
        dictionary.Add(WindowID.WindowID_Language, data);
        data = new WindowData {
            ClassName = "ProducterMediator",
            LayerType = WindowMediator.LayerType.eFront2,
            isPop = true
        };
        dictionary.Add(WindowID.WindowID_Producer, data);
        data = new WindowData {
            ClassName = "CheckBattleInMediator",
            LayerType = WindowMediator.LayerType.eFront2
        };
        dictionary.Add(WindowID.WindowID_CheckBattleIn, data);
        data = new WindowData {
            ClassName = "RewardSimpleMediator",
            LayerType = WindowMediator.LayerType.eFront3,
            State = 2,
            isPop = true
        };
        dictionary.Add(WindowID.WindowID_RewardSimple, data);
        data = new WindowData {
            ClassName = "StageListMediator",
            LayerType = WindowMediator.LayerType.eFront2,
            isPop = true
        };
        dictionary.Add(WindowID.WindowID_StageList, data);
        data = new WindowData {
            ClassName = "LevelBoxMediator",
            LayerType = WindowMediator.LayerType.eFront2
        };
        dictionary.Add(WindowID.WindowID_LevelBox, data);
        data = new WindowData {
            ClassName = "StageBoxMediator",
            LayerType = WindowMediator.LayerType.eFront2
        };
        dictionary.Add(WindowID.WindowID_StageBox, data);
        data = new WindowData {
            ClassName = "NetDoingMediator",
            LayerType = WindowMediator.LayerType.eFrontNet,
            State = 2
        };
        dictionary.Add(WindowID.WindowID_NetDoing, data);
        data = new WindowData {
            ClassName = "BattleRebornMediator",
            LayerType = WindowMediator.LayerType.eInGame,
            State = 1,
            isPop = true
        };
        dictionary.Add(WindowID.WindowID_BattleReborn, data);
        data = new WindowData {
            ClassName = "PurChaseOKMediator",
            LayerType = WindowMediator.LayerType.eFrontEvent,
            isPop = true
        };
        dictionary.Add(WindowID.WindowID_PurchaseOK, data);
        data = new WindowData {
            ClassName = "GoldBuyActiveMediator",
            LayerType = WindowMediator.LayerType.eFrontEvent,
            isPop = true
        };
        dictionary.Add(WindowID.WindowID_GoldBuyActive, data);
        data = new WindowData {
            ClassName = "EventFirstGoldMediator",
            LayerType = WindowMediator.LayerType.eInGame,
            State = 1
        };
        dictionary.Add(WindowID.WindowID_EventFirstGold, data);
        data = new WindowData {
            ClassName = "LayerBoxMediator",
            LayerType = WindowMediator.LayerType.eFront
        };
        dictionary.Add(WindowID.WindowID_LayerBox, data);
        data = new WindowData {
            ClassName = "VideoMediator",
            LayerType = WindowMediator.LayerType.eFrontMask
        };
        dictionary.Add(WindowID.WindowID_VideoPlay, data);
        data = new WindowData {
            ClassName = "EquipBuyInfoMediator",
            LayerType = WindowMediator.LayerType.eInGame,
            State = 1,
            isPop = true
        };
        dictionary.Add(WindowID.WindowID_EquipBuyInfo, data);
        data = new WindowData {
            ClassName = "ForceUpdateMediator",
            LayerType = WindowMediator.LayerType.eFrontMask,
            State = 2,
            isPop = true
        };
        dictionary.Add(WindowID.WindowID_ForceUpdate, data);
        data = new WindowData {
            ClassName = "PopWindowOneMediator",
            LayerType = WindowMediator.LayerType.eFront3,
            State = 2,
            isPop = true
        };
        dictionary.Add(WindowID.WindowID_PopWindowOne, data);
        data = new WindowData {
            ClassName = "TestNoticeMediator",
            LayerType = WindowMediator.LayerType.eFrontEvent,
            isPop = true
        };
        dictionary.Add(WindowID.WindowID_TestNotice, data);
        data = new WindowData {
            ClassName = "BuyGoldSureMediator",
            LayerType = WindowMediator.LayerType.eFrontEvent,
            isPop = true
        };
        dictionary.Add(WindowID.WindowID_BuyGoldSure, data);
        data = new WindowData {
            ClassName = "EventAdTurnTableMediator",
            LayerType = WindowMediator.LayerType.eInGame,
            State = 1,
            isPop = true
        };
        dictionary.Add(WindowID.WindowID_EventAdTurnTable, data);
        data = new WindowData {
            ClassName = "BoxOpenSingleMediator",
            LayerType = WindowMediator.LayerType.eFront2
        };
        dictionary.Add(WindowID.WindowID_BoxOpenSingle, data);
        data = new WindowData {
            ClassName = "ServerAssertMediator",
            LayerType = WindowMediator.LayerType.eFrontMask,
            State = 2,
            isPop = true
        };
        dictionary.Add(WindowID.WindowID_ServerAssert, data);
        data = new WindowData {
            ClassName = "AdInsideMediator",
            LayerType = WindowMediator.LayerType.eFrontMask,
            State = 3
        };
        dictionary.Add(WindowID.WindowID_AdInside, data);
        data = new WindowData {
            ClassName = "ReportMediator",
            LayerType = WindowMediator.LayerType.eFront
        };
        dictionary.Add(WindowID.WindowID_Report, data);
        windowClass = dictionary;
        UIPrefabPath = "UIPanel/";
    }

    public static WindowMediator.LayerType GetWindowLayerType(string classname)
    {
        Dictionary<WindowID, WindowData>.Enumerator enumerator = windowClass.GetEnumerator();
        while (enumerator.MoveNext())
        {
            KeyValuePair<WindowID, WindowData> current = enumerator.Current;
            if (current.Value.ClassName == classname)
            {
                KeyValuePair<WindowID, WindowData> pair2 = enumerator.Current;
                return pair2.Value.LayerType;
            }
        }
        return WindowMediator.LayerType.eRoot;
    }

    public static bool GetWindowPop(string classname)
    {
        Dictionary<WindowID, WindowData>.Enumerator enumerator = windowClass.GetEnumerator();
        while (enumerator.MoveNext())
        {
            KeyValuePair<WindowID, WindowData> current = enumerator.Current;
            if (current.Value.ClassName == classname)
            {
                KeyValuePair<WindowID, WindowData> pair2 = enumerator.Current;
                return pair2.Value.isPop;
            }
        }
        return false;
    }

    public class WindowData
    {
        public string ClassName;
        public WindowMediator.LayerType LayerType;
        public bool isPop;
        public int State;
    }
}

