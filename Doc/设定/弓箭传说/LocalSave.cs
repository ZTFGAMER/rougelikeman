using DG.Tweening;
using Dxx.Net;
using Dxx.Util;
using GameProtocol;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using TableTool;
using UnityEngine;

public class LocalSave
{
    private static LocalSave _instance = null;
    private UserInfo userInfo;
    private CardData mCardData;
    private ActiveData mActiveData;
    private ChallengeData mChallengeData;
    private AchieveData mAchieveData;
    public const string BattleInString = "BattleInString";
    public const string BattleInModeString = "BattleInModeString";
    private int BattleIn_Mode;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private bool <BattleIn_In>k__BackingField;
    private BattleInBase mBattleIn;
    private DropCard mBoxDropCard;
    private BoxDropEquip mBoxDropEquip;
    public static Action CardUpdateEvent;
    private const string ChallengeConst = "ChallengeConstLocal";
    public const int EquipCount = 6;
    public static Dictionary<int, Color> QualityColors;
    public static Dictionary<int, int> EquipPositions;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private EquipData <mEquip>k__BackingField;
    public LocalSaveExtra _saveExtra;
    private FakeStageDrop _fakestagedrop;
    private FakeCardCost _fakecardcost;
    private GuideData _guidedata;
    private HarvestData _harvest;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private LocalMail <Mail>k__BackingField;
    private PurchaseData _purchase;
    private ShopLocal _shop;
    public Action OnMaxLevelUpdate;
    private Stage _stage;
    private StageDiscountBody mStageDiscount;
    public SaveData mSaveData;
    private Dictionary<EThreadWriteType, bool> mThreadList;
    private object mThreadDoing;
    private TimeBoxData mTimeBox;
    private const string CurrencyConst = "Currency";
    public static Action<long, long> GoldUpdateEvent;
    private long mCurrencyKeyTime;
    [CompilerGenerated]
    private static Action<string> <>f__am$cache0;

    static LocalSave()
    {
        Dictionary<int, Color> dictionary = new Dictionary<int, Color> {
            { 
                0,
                new Color(0.8862745f, 0.8862745f, 0.8862745f)
            },
            { 
                1,
                new Color(0.8862745f, 0.8862745f, 0.8862745f)
            },
            { 
                2,
                new Color(0.4117647f, 0.8509804f, 0.3882353f)
            },
            { 
                3,
                new Color(0.227451f, 0.7490196f, 0.8901961f)
            },
            { 
                4,
                new Color(0.8431373f, 0.4235294f, 1f)
            },
            { 
                5,
                new Color(0.8431373f, 0.4235294f, 1f)
            },
            { 
                6,
                new Color(0.9960784f, 0.8941177f, 0.06666667f)
            },
            { 
                7,
                new Color(1f, 0.9215686f, 0.05490196f)
            },
            { 
                8,
                new Color(1f, 0.9215686f, 0.05490196f)
            },
            { 
                9,
                new Color(1f, 0.9215686f, 0.05490196f)
            }
        };
        QualityColors = dictionary;
        Dictionary<int, int> dictionary2 = new Dictionary<int, int> {
            { 
                0,
                1
            },
            { 
                1,
                2
            },
            { 
                2,
                5
            },
            { 
                3,
                5
            },
            { 
                4,
                6
            },
            { 
                5,
                6
            }
        };
        EquipPositions = dictionary2;
    }

    public LocalSave()
    {
        Dictionary<EThreadWriteType, bool> dictionary = new Dictionary<EThreadWriteType, bool> {
            { 
                EThreadWriteType.eBattle,
                false
            },
            { 
                EThreadWriteType.eEquip,
                false
            },
            { 
                EThreadWriteType.eNet,
                false
            },
            { 
                EThreadWriteType.eLocal,
                false
            }
        };
        this.mThreadList = dictionary;
        this.mThreadDoing = new object();
    }

    public void Achieve_AddProgress(int id, int count)
    {
        this.mAchieveData.AddProgress(id, count);
    }

    public void Achieve_ExcuteCurrentStage()
    {
        if (GameLogic.Hold.BattleData.Challenge_ismainchallenge())
        {
            int activeID = GameLogic.Hold.BattleData.ActiveID;
            AchieveDataOne one = this.Achieve_Get(activeID);
            if (one != null)
            {
                one.mCondition.Excute();
            }
        }
    }

    public AchieveDataOne Achieve_Get(int id) => 
        this.mAchieveData.Get(id);

    public int Achieve_get_finish_count(int stage)
    {
        int num = 0;
        List<int> stageList = LocalModelManager.Instance.Achieve_Achieve.GetStageList(stage, true);
        int num2 = 0;
        int count = stageList.Count;
        while (num2 < count)
        {
            AchieveDataOne one = this.Achieve_Get(stageList[num2]);
            if ((one != null) && one.isfinish)
            {
                num++;
            }
            num2++;
        }
        return num;
    }

    public int Achieve_get_finish_notgot_count()
    {
        int maxChapter = LocalModelManager.Instance.Stage_Level_stagechapter.GetMaxChapter();
        int num2 = 0;
        for (int i = 1; i <= maxChapter; i++)
        {
            num2 += this.Achieve_get_finish_notgot_count(i);
        }
        return num2;
    }

    public int Achieve_get_finish_notgot_count(int stage)
    {
        int num = 0;
        List<int> stageList = LocalModelManager.Instance.Achieve_Achieve.GetStageList(stage, true);
        int num2 = 0;
        int count = stageList.Count;
        while (num2 < count)
        {
            AchieveDataOne one = this.Achieve_Get(stageList[num2]);
            if (((one != null) && one.isfinish) && !one.isgot)
            {
                num++;
            }
            num2++;
        }
        return num;
    }

    public bool Achieve_IsFinish(int id) => 
        this.mAchieveData.IsFinish(id);

    public bool Achieve_Isgot(int id) => 
        this.mAchieveData.Isgot(id);

    public CardOne AddCard(int cardid, int count) => 
        this.mCardData.AddOne(cardid, count);

    public void AddProp(CEquipmentItem item)
    {
        EquipOne data = new EquipOne {
            UniqueID = item.m_nUniqueID,
            EquipID = (int) item.m_nEquipID,
            Level = (int) item.m_nLevel,
            Count = (int) item.m_nFragment
        };
        this.mEquip.AddEquipInternal(data);
    }

    public void AddProp(Drop_DropModel.DropData item)
    {
        EquipOne data = new EquipOne {
            UniqueID = Utils.GenerateUUID(),
            EquipID = item.id,
            Level = 1,
            Count = item.count
        };
        this.mEquip.AddEquipInternal(data);
    }

    public void AddProps(List<Drop_DropModel.DropData> list)
    {
        int num = 0;
        int count = list.Count;
        while (num < count)
        {
            Drop_DropModel.DropData data = list[num];
            switch (data.type)
            {
                case PropType.eCurrency:
                    if (data.id == 1)
                    {
                        this.Modify_Gold((long) data.count, true);
                        break;
                    }
                    if (data.id == 2)
                    {
                        this.Modify_Diamond((long) data.count, true);
                        break;
                    }
                    if (data.id == 3)
                    {
                        this.Modify_Key((long) data.count, true);
                    }
                    else if (data.id == 4)
                    {
                        this.Modify_RebornCount(data.count);
                    }
                    break;

                case PropType.eEquip:
                {
                    EquipOne one = new EquipOne {
                        UniqueID = Utils.GenerateUUID(),
                        EquipID = data.id,
                        Level = 1,
                        Count = data.count
                    };
                    if (one.Overlying)
                    {
                        this.mEquip.AddEquipInternal(one);
                    }
                    break;
                }
            }
            num++;
        }
    }

    public bool BattleAd_CanShow() => 
        ((this.userInfo != null) && ((this.userInfo.BattleAdCount > 0) && (this.Card_GetLevel() >= GameConfig.BattleAdUnlockTalentLevel)));

    public int BattleAd_Get()
    {
        if (this.userInfo != null)
        {
            return this.userInfo.BattleAdCount;
        }
        return 0;
    }

    public void BattleAd_Set(int count)
    {
        if (this.userInfo != null)
        {
            this.userInfo.BattleAdCount = count;
            this.userInfo.Refresh();
        }
    }

    public void BattleAd_Use()
    {
        if (this.userInfo != null)
        {
            this.userInfo.BattleAdCount--;
            this.userInfo.Refresh();
        }
        GameLogic.Hold.BattleData.Battle_ad_use();
    }

    public void BattleIn_AddEquip(EquipOne one)
    {
        if (this.mBattleIn != null)
        {
            this.mBattleIn.AddEquip(one);
        }
    }

    public void BattleIn_AddRebornSkill()
    {
        if (this.mBattleIn != null)
        {
            this.mBattleIn.AddRebornSkill();
        }
    }

    public void BattleIn_AddRebornUI()
    {
        if (this.mBattleIn != null)
        {
            this.mBattleIn.AddRebornUI();
        }
    }

    public void BattleIn_Check()
    {
        if ((this.mBattleIn != null) && this.BattleIn_GetIn())
        {
            GameMode mode = (GameMode) this.BattleIn_Mode;
            GameLogic.Hold.BattleData.SetMode(mode, BattleSource.eWorld);
            WindowUI.ShowWindow(WindowID.WindowID_Battle);
        }
    }

    public void BattleIn_CheckInit()
    {
        this.BattleIn_InitInternal();
        if (this.mBattleIn != null)
        {
            this.BattleIn_In = this.mBattleIn.GetHaveBattle();
        }
    }

    public void BattleIn_DeInit()
    {
        if (this.mBattleIn != null)
        {
            this.mBattleIn.DeInit();
        }
        this.BattleIn_In = false;
    }

    public float BattleIn_GetExp()
    {
        if (this.mBattleIn != null)
        {
            return this.mBattleIn.exp;
        }
        return 0f;
    }

    public List<bool> BattleIn_GetFirstShop()
    {
        if (this.mBattleIn != null)
        {
            return this.mBattleIn.firstshopbuy;
        }
        return new List<bool> { 
            false,
            false
        };
    }

    public float BattleIn_GetGold()
    {
        if (this.mBattleIn != null)
        {
            return this.mBattleIn.gold;
        }
        return 0f;
    }

    public bool BattleIn_GetGoldTurn()
    {
        if (this.mBattleIn != null)
        {
            return this.mBattleIn.bGoldTurn;
        }
        return true;
    }

    public long BattleIn_GetHP()
    {
        if (this.mBattleIn != null)
        {
            return this.mBattleIn.hp;
        }
        return 0L;
    }

    public bool BattleIn_GetIn()
    {
        if ((this.mBattleIn != null) && string.IsNullOrEmpty(this.mBattleIn.TmxID))
        {
            this.BattleIn_DeInit();
            this.BattleIn_In = false;
            return false;
        }
        return this.BattleIn_In;
    }

    public int BattleIn_GetLevel()
    {
        if (this.mBattleIn != null)
        {
            return this.mBattleIn.level;
        }
        return 1;
    }

    public List<int> BattleIn_GetLevelUpSkills()
    {
        if (((this.mBattleIn != null) && (this.mBattleIn.levelupskills != null)) && (this.mBattleIn.levelupskills.Count > 0))
        {
            return this.mBattleIn.levelupskills;
        }
        return null;
    }

    public int BattleIn_GetLevelUpType()
    {
        if (this.mBattleIn != null)
        {
            return this.mBattleIn.leveluptype;
        }
        return 0;
    }

    public List<int> BattleIn_GetPotions()
    {
        if (this.mBattleIn != null)
        {
            return this.mBattleIn.potions;
        }
        return null;
    }

    public int BattleIn_GetRebornSkill()
    {
        if (this.mBattleIn != null)
        {
            return this.mBattleIn.reborn_skill_count;
        }
        return 0;
    }

    public int BattleIn_GetRebornUI()
    {
        if (this.mBattleIn != null)
        {
            return this.mBattleIn.reborn_ui_count;
        }
        return 0;
    }

    public int BattleIn_GetResourcesID()
    {
        if (this.mBattleIn != null)
        {
            return this.mBattleIn.ResourcesID;
        }
        return 1;
    }

    public int BattleIn_GetRoomID()
    {
        if (this.mBattleIn != null)
        {
            return this.mBattleIn.RoomID;
        }
        return 0;
    }

    public string BattleIn_GetTmxID()
    {
        if (this.mBattleIn != null)
        {
            return this.mBattleIn.TmxID;
        }
        return string.Empty;
    }

    public void BattleIn_InGame()
    {
        if (this.mBattleIn != null)
        {
            if (GameLogic.Hold.BattleData.GetMode() == GameMode.eLevel)
            {
                this.mBattleIn.CheckDifferentID();
                this.mBattleIn.SetHaveBattle(true);
                this.mBattleIn.serveruserid = Instance.GetServerUserID();
                int mode = (int) GameLogic.Hold.BattleData.GetMode();
                this.BattleIn_Mode = mode;
                this.SaveExtra.battleinmode = mode;
                this.SaveExtra.Refresh();
                this.mBattleIn.Refresh();
            }
            else
            {
                this.mBattleIn.SetHaveBattle(false);
                this.mBattleIn.Refresh();
            }
        }
    }

    public void BattleIn_Init()
    {
        this.BattleIn_InitInternal();
        this.BattleIn_InGame();
    }

    private void BattleIn_InitInternal()
    {
        if (this.mBattleIn == null)
        {
            this.BattleIn_Mode = this.SaveExtra.battleinmode;
            if (this.BattleIn_Mode == 0x3e9)
            {
                BattleInBase base2 = BattleInBase.Get();
                base2.LevelInit();
                this.mBattleIn = base2;
            }
        }
    }

    public void BattleIn_Restore()
    {
        if ((this.mBattleIn != null) && this.BattleIn_GetIn())
        {
            if ((this.BattleIn_Mode == 0x3e9) && (this.mBattleIn.potions != null))
            {
                int num = 0;
                int num2 = this.mBattleIn.potions.Count;
                while (num < num2)
                {
                    FirstItemOnectrl.GetOnePotion(LocalModelManager.Instance.Shop_item.GetBeanById(this.mBattleIn.potions[num]));
                    num++;
                }
            }
            int num3 = 0;
            int count = this.mBattleIn.skillids.Count;
            while (num3 < count)
            {
                GameLogic.Self.BattleInInitSkill(this.mBattleIn.skillids[num3]);
                num3++;
            }
            int num5 = 0;
            int num6 = this.mBattleIn.learnskills.Count;
            while (num5 < num6)
            {
                GameLogic.Self.LearnSkill(this.mBattleIn.learnskills[num5]);
                num5++;
            }
            int num7 = 0;
            int num8 = this.mBattleIn.goodids.Count;
            while (num7 < num8)
            {
                GameLogic.Self.BattleInGetGoods(this.mBattleIn.goodids[num7]);
                num7++;
            }
            int num9 = 0;
            int num10 = this.mBattleIn.equips.Count;
            while (num9 < num10)
            {
                GameLogic.Hold.BattleData.AddEquipInternal(this.mBattleIn.equips[num9]);
                num9++;
            }
            GameLogic.Self.m_EntityData.SetCurrentExpLevel(this.mBattleIn.exp, this.mBattleIn.level);
            long num11 = this.BattleIn_GetHP();
            if (num11 > 0L)
            {
                long maxHP = GameLogic.Self.m_EntityData.MaxHP;
                long hP = num11 - maxHP;
                if (hP < 0L)
                {
                    GameLogic.Self.ChangeHPMust(null, hP);
                }
            }
        }
    }

    public void BattleIn_SetHaveBattle(bool value)
    {
        if (this.mBattleIn != null)
        {
            this.mBattleIn.SetHaveBattle(value);
        }
    }

    public void BattleIn_UpdateExp(float exp)
    {
        if (this.mBattleIn != null)
        {
            this.mBattleIn.exp = exp;
            this.mBattleIn.Refresh();
        }
    }

    public void BattleIn_UpdateFirstShop(List<bool> list)
    {
        if (this.mBattleIn != null)
        {
            this.mBattleIn.firstshopbuy = list;
            this.mBattleIn.Refresh();
        }
    }

    public void BattleIn_UpdateGold(float gold)
    {
        if (this.mBattleIn != null)
        {
            this.mBattleIn.gold = gold;
            this.mBattleIn.Refresh();
        }
    }

    public void BattleIn_UpdateGoldTurn()
    {
        if (this.mBattleIn != null)
        {
            this.mBattleIn.bGoldTurn = true;
            this.mBattleIn.Refresh();
        }
    }

    public void BattleIn_UpdateGood(int goodid)
    {
        if (this.mBattleIn != null)
        {
            this.mBattleIn.goodids.Add(goodid);
            this.mBattleIn.Refresh();
        }
    }

    public void BattleIn_UpdateHP(long hp)
    {
        if (this.mBattleIn != null)
        {
            this.mBattleIn.hp = hp;
            this.mBattleIn.Refresh();
        }
    }

    public void BattleIn_UpdateIn()
    {
        this.BattleIn_In = false;
    }

    public void BattleIn_UpdateLearnSkill(int skillid)
    {
        if (this.mBattleIn != null)
        {
            this.mBattleIn.learnskills.Add(skillid);
            this.mBattleIn.Refresh();
        }
    }

    public void BattleIn_UpdateLevel(int level)
    {
        if (this.mBattleIn != null)
        {
            this.mBattleIn.level = level;
            this.mBattleIn.Refresh();
        }
    }

    public void BattleIn_UpdateLevelUpSkills(int type, List<int> skills)
    {
        if (this.mBattleIn != null)
        {
            this.mBattleIn.leveluptype = type;
            this.mBattleIn.levelupskills = skills;
            this.mBattleIn.Refresh();
        }
    }

    public void BattleIn_UpdatePotions(int id)
    {
        if (this.mBattleIn != null)
        {
            this.mBattleIn.AddPotion(id);
            this.mBattleIn.Refresh();
        }
    }

    public void BattleIn_UpdateResourcesID(int id)
    {
        if (this.mBattleIn != null)
        {
            this.mBattleIn.ResourcesID = id;
            this.mBattleIn.Refresh();
        }
    }

    public void BattleIn_UpdateRoomID(int roomid)
    {
        if (this.mBattleIn != null)
        {
            this.mBattleIn.RoomID = roomid;
            this.mBattleIn.Refresh();
        }
    }

    public void BattleIn_UpdateSkill(int skillid)
    {
        if (this.mBattleIn != null)
        {
            this.mBattleIn.skillids.Add(skillid);
            this.mBattleIn.Refresh();
        }
    }

    public void BattleIn_UpdateStage()
    {
        if (this.mBattleIn != null)
        {
            this.mBattleIn.stage = GameLogic.Hold.BattleData.Level_CurrentStage;
            this.mBattleIn.Refresh();
        }
    }

    public void BattleIn_UpdateTmxID(string tmxid)
    {
        if (this.mBattleIn != null)
        {
            this.mBattleIn.TmxID = tmxid;
            this.mBattleIn.Refresh();
        }
    }

    public bool Card_GetAllMax() => 
        this.mCardData.GetAllMax();

    public bool Card_GetHarvestAvailable() => 
        (this.Card_GetHarvestLevel() > 0);

    public int Card_GetHarvestGold()
    {
        int key = this.Card_GetHarvestLevel();
        Drop_harvest beanById = LocalModelManager.Instance.Drop_harvest.GetBeanById(key);
        if (beanById != null)
        {
            return beanById.GoldDrop;
        }
        return 0;
    }

    public int Card_GetHarvestID() => 
        0x6c;

    public int Card_GetHarvestLevel()
    {
        CardOne cardByID = this.GetCardByID(this.Card_GetHarvestID());
        if (cardByID == null)
        {
            return 0;
        }
        return cardByID.level;
    }

    public int Card_GetLevel() => 
        this.Card_GetRandomCount();

    public int Card_GetNeedLevel() => 
        this.mFakeCardCost.GetNeedLevel();

    public CardOne Card_GetRandom()
    {
        Drop_DropModel.DropData data = this.Card_GetRandomOnly();
        this.mFakeCardCost.AddCount();
        return this.AddCard(data.id, data.count);
    }

    public int Card_GetRandomCount() => 
        this.mFakeCardCost.count;

    public int Card_GetRandomGold() => 
        this.mFakeCardCost.GetCost();

    public Drop_DropModel.DropData Card_GetRandomOnly() => 
        Instance.GetDropCardRandom();

    public bool Card_Have(int id) => 
        this.mCardData.HaveCard(id);

    public CardOne Card_ReceiveCard(Drop_DropModel.DropData drop)
    {
        this.mFakeCardCost.AddCount();
        return this.AddCard(drop.id, drop.count);
    }

    public void Card_Set(List<CEquipmentItem> cards)
    {
        this.mCardData.Clear();
        int num = 0;
        int count = cards.Count;
        while (num < count)
        {
            CEquipmentItem item = cards[num];
            this.mCardData.SetOne((int) item.m_nEquipID, (int) item.m_nLevel);
            num++;
        }
    }

    public int Challenge_GetID() => 
        this.mChallengeData.ChallengeID;

    public int Challenge_GetPassCount() => 
        (this.Challenge_GetID() - 0x835);

    public bool Challenge_IsFirstIn() => 
        this.mChallengeData.bFirstIn;

    public void Challenge_SetFirstIn()
    {
        this.mChallengeData.bFirstIn = false;
        this.mChallengeData.Refresh();
    }

    public void ChallengeSucceed()
    {
        this.mChallengeData.ChallengeID++;
        this.mChallengeData.bFirstIn = true;
        this.mChallengeData.Refresh();
    }

    private void CreateThread()
    {
        this.mSaveData = FileUtils.GetXml<SaveData>("localsave.txt");
        new Thread(delegate {
            object obj2;
        Label_0000:
            obj2 = this.mThreadDoing;
            lock (obj2)
            {
                if (this.mThreadList[EThreadWriteType.eBattle])
                {
                    this.mThreadList[EThreadWriteType.eBattle] = false;
                    FileUtils.WriteBattleInThread(this.mBattleIn);
                    goto Label_0000;
                }
                if (this.mThreadList[EThreadWriteType.eEquip])
                {
                    this.mThreadList[EThreadWriteType.eEquip] = false;
                    FileUtils.WriteXmlThread<EquipData>("localequip.txt", this.mEquip);
                    goto Label_0000;
                }
                if (this.mThreadList[EThreadWriteType.eLocal])
                {
                    this.mThreadList[EThreadWriteType.eLocal] = false;
                    FileUtils.WriteXmlThread<SaveData>("localsave.txt", this.mSaveData);
                    goto Label_0000;
                }
                if (this.mThreadList[EThreadWriteType.eNet])
                {
                    this.mThreadList[EThreadWriteType.eNet] = false;
                    FileUtils.WriteXmlThread<NetCaches>(NetCaches.GetFileName(Instance.GetServerUserID()), NetManager.mNetCache);
                    goto Label_0000;
                }
            }
            Thread.Sleep(0x7d0);
            if (ApplicationEvent.bQuit)
            {
                return;
            }
            goto Label_0000;
        }) { IsBackground = true }.Start();
    }

    public void DoLogin(SendType sendType, Action callback)
    {
        <DoLogin>c__AnonStorey2 storey = new <DoLogin>c__AnonStorey2 {
            callback = callback,
            sendType = sendType,
            $this = this
        };
        CUserLoginPacket packet = new CUserLoginPacket();
        NetManager.SendInternal<CUserLoginPacket>(packet, storey.sendType, new Action<NetResponse>(storey.<>m__0));
    }

    public void DoLogin_Start(Action callback)
    {
        <DoLogin_Start>c__AnonStorey1 storey = new <DoLogin_Start>c__AnonStorey1 {
            callback = callback,
            $this = this
        };
        SdkManager.Login(new Action<SdkManager.LoginData>(storey.<>m__0));
    }

    public void DoLoginCallBack(CRespUserLoginPacket data, Action callback)
    {
        if (data != null)
        {
            object[] args = new object[] { data.m_nCoins };
            Debugger.Log(Utils.FormatString("m_nCoins : {0}", args));
            object[] objArray2 = new object[] { data.m_nDiamonds };
            Debugger.Log(Utils.FormatString("m_nDiamonds : {0}", objArray2));
            object[] objArray3 = new object[] { data.m_nExperince };
            Debugger.Log(Utils.FormatString("m_nExperince : {0}", objArray3));
            object[] objArray4 = new object[] { data.m_nLevel };
            Debugger.Log(Utils.FormatString("m_nLevel : {0}", objArray4));
            object[] objArray5 = new object[] { data.m_nMaxLayer };
            Debugger.Log(Utils.FormatString("m_nMaxLayer : {0}", objArray5));
            object[] objArray6 = new object[] { data.m_nUserRawId };
            Debugger.Log(Utils.FormatString("m_nUserRawId : {0}", objArray6));
            object[] objArray7 = new object[] { data.GetHarvestTime() };
            Debugger.Log(Utils.FormatString("m_lHarvestTimestamp : {0}", objArray7));
            object[] objArray8 = new object[] { data.m_nExtraNormalDiamondItem };
            Debugger.Log(Utils.FormatString("m_nExtraNormalDiamondItem : {0}", objArray8));
            object[] objArray9 = new object[] { data.m_nExtraLargeDiamondItem };
            Debugger.Log(Utils.FormatString("m_nExtraLargeDiamondItem : {0}", objArray9));
            Instance.mGuideData.SetGameSystemMask(data.m_nGameSystemMask);
            Instance.SetServerUserID(data.m_nUserRawId);
            Instance.GetUserInfo().bLogined = true;
            Instance.UserInfo_SetRebornCount(data.m_nBattleRebornCount);
            Instance.DropCard_Init((int) data.m_nTreasureRandomCount);
            Instance.mFakeCardCost.InitCount((int) data.m_nTreasureRandomCount);
            NetManager.SetNetTime((long) data.GetServerTime());
            Debugger.Log("server time = " + data.GetServerTime());
            CRestoreItem restore = data.GetRestore(CRestoreItem.EItemIndex.ENormalDiamondItemIndex);
            Instance.UsserInfo_SetTimeBoxCount(TimeBoxType.BoxChoose_DiamondNormal, restore.m_nMin);
            Instance.SetTimeBoxTime(TimeBoxType.BoxChoose_DiamondNormal, (long) restore.m_i64Timestamp);
            Debugger.Log("BoxChoose_DiamondNormal time = " + restore.m_i64Timestamp);
            CRestoreItem item2 = data.GetRestore(CRestoreItem.EItemIndex.ELargeDiamondItemIndex);
            Debugger.Log("BoxChoose_DiamondLarge time = " + item2.m_i64Timestamp);
            Instance.UsserInfo_SetTimeBoxCount(TimeBoxType.BoxChoose_DiamondLarge, item2.m_nMin);
            Instance.SetTimeBoxTime(TimeBoxType.BoxChoose_DiamondLarge, (long) item2.m_i64Timestamp);
            CRestoreItem item3 = data.GetRestore(CRestoreItem.EItemIndex.ELifeItemIndex);
            Instance.UserInfo_SetKeyTime((long) item3.m_i64Timestamp);
            CRestoreItem item4 = data.GetRestore(CRestoreItem.EItemIndex.EAdGetLifeItemIndex);
            Instance.UserInfo_SetAdKeyCount(item4.m_nMin);
            CRestoreItem item5 = data.GetRestore(CRestoreItem.EItemIndex.EAdGetLuckyItemIndex);
            Instance.BattleAd_Set(item5.m_nMin);
            object[] objArray10 = new object[] { item3.m_nMin };
            Debug.Log(Utils.FormatString("m_nLife : {0}", objArray10));
            Instance.UserInfo_Set((int) data.m_nCoins, (int) data.m_nDiamonds, (int) data.m_nExperince, item3.m_nMin, data.m_nLevel, data.m_nExtraNormalDiamondItem, data.m_nExtraLargeDiamondItem);
            Instance.SaveExtra.InitTransID(data.m_nTransID);
            Instance.Stage_InitNextID(data.m_nLayerBoxID);
            Instance.mStage.InitMaxLevel(data.m_nMaxLayer);
            Instance.mHarvest.init_last_time((long) data.GetHarvestTime());
            Instance.mShop.bRefresh = true;
            Instance.mEquip.bRefresh = true;
            List<CEquipmentItem> equips = new List<CEquipmentItem>();
            List<CEquipmentItem> cards = new List<CEquipmentItem>();
            object[] objArray11 = new object[] { data.m_arrayEquipData.Length };
            Debugger.Log(Utils.FormatString("arrayEquipItems : {0}", objArray11));
            int index = 0;
            int length = data.m_arrayEquipData.Length;
            while (index < length)
            {
                CEquipmentItem item = data.m_arrayEquipData[index];
                if (item.m_nEquipID < 0x3e8)
                {
                    if (LocalModelManager.Instance.Skill_slotout.GetBeanById((int) item.m_nEquipID) != null)
                    {
                        cards.Add(item);
                    }
                }
                else if (LocalModelManager.Instance.Equip_equip.GetBeanById((int) item.m_nEquipID) != null)
                {
                    equips.Add(item);
                }
                index++;
            }
            Instance.Equip_Set(equips);
            Instance.Card_Set(cards);
            GameLogic.Hold.Guide.Init();
            Instance.SetUserInfoInit();
        }
        if (callback != null)
        {
            callback();
        }
        Instance.Mail.SendMail();
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = d => Instance.StageDiscount_Init(d);
        }
        Instance.StageDiscount_Send(<>f__am$cache0);
    }

    public void DoThreadSave(EThreadWriteType type)
    {
        object mThreadDoing = this.mThreadDoing;
        lock (mThreadDoing)
        {
            this.mThreadList[type] = true;
        }
    }

    public void DropCard_Init(int allcount)
    {
        this.mBoxDropCard.InitCount(allcount);
    }

    public void Equip_Add(CEquipmentItem[] equips)
    {
        if (equips != null)
        {
            int index = 0;
            int length = equips.Length;
            while (index < length)
            {
                CEquipmentItem item = equips[index];
                EquipOne data = new EquipOne {
                    UniqueID = item.m_nUniqueID,
                    RowID = item.m_nRowID,
                    EquipID = (int) item.m_nEquipID,
                    Count = (int) item.m_nFragment,
                    Level = (int) item.m_nLevel
                };
                this.mEquip.AddEquipInternal(data);
                index++;
            }
        }
    }

    public void Equip_AddUpdateAction(Action callback)
    {
        EquipData mEquip = this.mEquip;
        mEquip.mUpdateAction = (Action) Delegate.Combine(mEquip.mUpdateAction, callback);
    }

    public void Equip_Attribute2(SelfAttributeData attribute)
    {
        int num = 0;
        int count = this.mEquip.wears.Count;
        while (num < count)
        {
            string str = this.mEquip.wears[num];
            if (!string.IsNullOrEmpty(str))
            {
                EquipOne equipByUniqueID = this.mEquip.GetEquipByUniqueID(str);
                if (equipByUniqueID != null)
                {
                    equipByUniqueID.EquipWear(attribute);
                }
            }
            num++;
        }
    }

    public bool Equip_can_combine(EquipOne one) => 
        this.mEquip.combine_can(one);

    public int Equip_can_combine_count() => 
        this.mEquip.combine_can_count();

    public bool Equip_can_drop_equipexp(int id)
    {
        Dictionary<string, EquipOne>.Enumerator enumerator = this.mEquip.list.GetEnumerator();
        while (enumerator.MoveNext())
        {
            KeyValuePair<string, EquipOne> current = enumerator.Current;
            if (current.Value.NeedMatID == id)
            {
                return true;
            }
        }
        return false;
    }

    public List<EquipOne> Equip_get_equip_babies()
    {
        List<EquipOne> list = new List<EquipOne>();
        int num = 0;
        int count = this.mEquip.wears.Count;
        while (num < count)
        {
            string uniqueid = this.mEquip.wears[num];
            EquipOne equipByUniqueID = this.GetEquipByUniqueID(uniqueid);
            if ((equipByUniqueID != null) && (equipByUniqueID.data.Position == 6))
            {
                list.Add(equipByUniqueID);
            }
            num++;
        }
        return list;
    }

    public int Equip_GetCanUpCount() => 
        this.mEquip.GetCanUpCount();

    public bool Equip_GetCanWear(EquipOne one, int index)
    {
        int key = 0;
        int count = this.mEquip.wears.Count;
        while ((key < count) && EquipPositions.ContainsKey(key))
        {
            if ((EquipPositions[key] == one.Position) && (key != index))
            {
                EquipOne equipByUniqueID = this.GetEquipByUniqueID(this.mEquip.wears[key]);
                if ((equipByUniqueID != null) && ((equipByUniqueID.EquipID / 100) == (one.EquipID / 100)))
                {
                    return false;
                }
            }
            key++;
        }
        return true;
    }

    public int Equip_GetCanWearCount() => 
        this.mEquip.GetCanWearCount();

    public bool Equip_GetCanWearIndex(EquipOne one, out int index)
    {
        index = -1;
        if (this.Equip_GetPositionCount(one.Position) == 1)
        {
            int num = 0;
            int num2 = this.mEquip.wears.Count;
            while ((num < num2) && EquipPositions.ContainsKey(num))
            {
                if (EquipPositions[num] == one.Position)
                {
                    index = num;
                    return true;
                }
                num++;
            }
        }
        int key = 0;
        int count = this.mEquip.wears.Count;
        while ((key < count) && EquipPositions.ContainsKey(key))
        {
            if ((EquipPositions[key] == one.Position) && string.IsNullOrEmpty(this.mEquip.wears[key]))
            {
                index = key;
                return true;
            }
            key++;
        }
        return false;
    }

    public List<int> Equip_GetCanWears(int position)
    {
        List<int> list = new List<int>();
        int item = 0;
        int count = this.mEquip.wears.Count;
        while (item < count)
        {
            if (EquipPositions[item] == position)
            {
                list.Add(item);
            }
            item++;
        }
        return list;
    }

    public int Equip_GetCloth()
    {
        if (this.mEquip.wear_enable)
        {
            string uniqueid = this.mEquip.wears[1];
            EquipOne equipByUniqueID = this.GetEquipByUniqueID(uniqueid);
            if (equipByUniqueID != null)
            {
                return equipByUniqueID.data.EquipIcon;
            }
        }
        return 0;
    }

    public bool Equip_GetHaveEquips() => 
        (this.GetHaveEquips(true).Count > 0);

    public bool Equip_GetIsEmpty(EquipOne one)
    {
        int key = 0;
        int count = this.mEquip.wears.Count;
        while ((key < count) && EquipPositions.ContainsKey(key))
        {
            if ((EquipPositions[key] == one.Position) && string.IsNullOrEmpty(this.mEquip.wears[key]))
            {
                return true;
            }
            key++;
        }
        return false;
    }

    public int Equip_GetNewCount() => 
        this.mEquip.GetNewCount();

    public int Equip_GetPet(int index)
    {
        if (this.mEquip.wear_enable)
        {
            string uniqueid = this.mEquip.wears[4 + index];
            EquipOne equipByUniqueID = this.GetEquipByUniqueID(uniqueid);
            if (equipByUniqueID != null)
            {
                return equipByUniqueID.data.EquipIcon;
            }
        }
        return 0;
    }

    private int Equip_GetPositionCount(int position)
    {
        int num = 0;
        Dictionary<int, int>.Enumerator enumerator = EquipPositions.GetEnumerator();
        while (enumerator.MoveNext())
        {
            KeyValuePair<int, int> current = enumerator.Current;
            if (current.Value == position)
            {
                num++;
            }
        }
        return num;
    }

    public bool Equip_GetRefresh() => 
        this.mEquip.bRefresh;

    public List<int> Equip_GetSkills()
    {
        List<int> list = new List<int>();
        int num = 0;
        int count = this.mEquip.wears.Count;
        while (num < count)
        {
            string str = this.mEquip.wears[num];
            if (!string.IsNullOrEmpty(str))
            {
                EquipOne equipByUniqueID = this.mEquip.GetEquipByUniqueID(str);
                if (equipByUniqueID != null)
                {
                    List<int> skills = equipByUniqueID.GetSkills();
                    list.AddRange(skills);
                }
            }
            num++;
        }
        return list;
    }

    public void Equip_GetUniqueidByEquipID(int equipid)
    {
    }

    public int Equip_GetWeapon()
    {
        if (this.mEquip.wear_enable)
        {
            string uniqueid = this.mEquip.wears[0];
            EquipOne equipByUniqueID = this.GetEquipByUniqueID(uniqueid);
            if (equipByUniqueID != null)
            {
                return equipByUniqueID.data.EquipIcon;
            }
        }
        return 0;
    }

    public bool Equip_is_same_wear(EquipOne one) => 
        false;

    public void Equip_Remove(string uniqueid)
    {
        this.mEquip.RemoveEquip(uniqueid);
    }

    public void Equip_RemoveUpdateAction(Action callback)
    {
        EquipData mEquip = this.mEquip;
        mEquip.mUpdateAction = (Action) Delegate.Remove(mEquip.mUpdateAction, callback);
    }

    public void Equip_Set(List<CEquipmentItem> equips)
    {
        this.mEquip.Init(equips);
    }

    public void Equip_SetRefresh()
    {
        this.mEquip.bRefresh = false;
    }

    private void EquipAchieve_GetNewEquip(int pos, int quality)
    {
        PlayerPrefsEncrypt.SetInt(this.EquipAchieve_GetNewEquipString(pos, quality), this.EquipAchieve_GetNewEquipLocal(pos, quality) + 1);
    }

    public int EquipAchieve_GetNewEquipLocal(int pos, int quality) => 
        PlayerPrefsEncrypt.GetInt(this.EquipAchieve_GetNewEquipString(pos, quality), 0);

    private string EquipAchieve_GetNewEquipString(int pos, int quality)
    {
        object[] args = new object[] { pos, quality };
        return Utils.FormatString("EquipAcheveCount_{0}_{1}", args);
    }

    public void EquipLevelUp(EquipOne equip)
    {
        this.mEquip.EquipLevelUp(equip);
    }

    public void EquipUnwear(string uniqueid)
    {
        this.mEquip.EquipUnwear(uniqueid);
    }

    public void EquipWear(EquipOne equip, int wearindex)
    {
        this.mEquip.EquipWear(equip, wearindex);
    }

    public void ExcuteModeChest(int dropid)
    {
        List<Drop_DropModel.DropData> dropList = LocalModelManager.Instance.Drop_Drop.GetDropList(dropid);
        Facade.Instance.RegisterProxy(new BoxOpenProxy(dropList));
    }

    public int GetActiveCount(int index)
    {
        if ((index < 0) || (index >= this.mActiveData.list.Count))
        {
            object[] args = new object[] { index, this.mActiveData.list.Count };
            SdkManager.Bugly_Report("LocaSave_Active", Utils.FormatString("GetActiveCount[{0}] is out of range, mActiveData.list.Count = {1}", args));
        }
        return this.mActiveData.list[index].Count;
    }

    public List<ActiveOne> GetActives() => 
        this.mActiveData.list;

    public bool GetCanLevelUp() => 
        (this.userInfo.Exp >= this.GetExpByLevel(this.userInfo.Level));

    public CardOne GetCardByID(int id) => 
        this.mCardData.GetCardByID(id);

    public bool GetCardMaxLevel(int id)
    {
        if (this.mCardData.mList.TryGetValue(id, out CardOne one))
        {
            return one.IsMaxLevel;
        }
        return true;
    }

    public Dictionary<int, CardOne> GetCards() => 
        this.mCardData.GetCards();

    public int GetCardsCount() => 
        this.mCardData.GetCards().Count;

    public List<CardOne> GetCardsList()
    {
        List<CardOne> list = new List<CardOne>();
        Dictionary<int, CardOne>.Enumerator enumerator = this.mCardData.GetCards().GetEnumerator();
        while (enumerator.MoveNext())
        {
            KeyValuePair<int, CardOne> current = enumerator.Current;
            list.Add(current.Value);
        }
        return list;
    }

    public void GetCardSucceed()
    {
        this.mBoxDropCard.GetSucceed();
    }

    public long GetDiamond() => 
        this.userInfo.Diamond;

    public int GetDiamondBoxFreeCount(TimeBoxType type)
    {
        int num = 0;
        num += this.GetTimeBoxCount(type);
        return (num + this.GetDiamondExtraCount(type));
    }

    public bool GetDiamondBoxGuide() => 
        this.userInfo.guide_diamondbox;

    public int GetDiamondExtraCount(TimeBoxType type)
    {
        if (this.userInfo == null)
        {
            return 0;
        }
        if (type == TimeBoxType.BoxChoose_DiamondNormal)
        {
            return this.userInfo.DiamondNormalExtraCount;
        }
        return this.userInfo.DiamondLargeExtraCount;
    }

    public Drop_DropModel.DropData GetDropCardRandom() => 
        this.mBoxDropCard.GetRandom();

    public Drop_DropModel.DropData GetDropEquipRandom() => 
        this.mBoxDropEquip.GetRandom();

    public List<Drop_DropModel.DropData> GetDropTimeBoxRandom()
    {
        int dropId = LocalModelManager.Instance.Box_TimeBox.GetBeanById(this.Stage_GetStage()).DropId;
        return LocalModelManager.Instance.Drop_Drop.GetDropList(dropId);
    }

    public EquipOne GetEquipByUniqueID(string uniqueid) => 
        this.mEquip.GetEquipByUniqueID(uniqueid);

    public bool GetEquipGuide_mustdrop()
    {
        if (GameLogic.Hold.Guide.mEquip.process > 0)
        {
            return false;
        }
        if (this.GetHaveEquips(true).Count > 1)
        {
            return false;
        }
        if (GameLogic.Hold.BattleData.GetEquips().Count > 0)
        {
            return false;
        }
        return Instance.SaveExtra.Get_Equip_Drop();
    }

    public EquipData GetEquips() => 
        this.mEquip;

    public long GetExp() => 
        this.userInfo.Exp;

    public int GetExpByLevel(int level) => 
        LocalModelManager.Instance.Character_Level.GetExp(level);

    public long GetGold() => 
        this.userInfo.Gold;

    public List<EquipOne> GetHaveEquips(bool havewear) => 
        this.mEquip.GetHaveEquips(havewear);

    public int GetKey() => 
        this.userInfo.Key;

    public long GetKeyTime() => 
        this.mCurrencyKeyTime;

    public int GetLevel() => 
        this.userInfo.Level;

    public LoginType GetLoginType() => 
        this.userInfo.loginType;

    public long GetNetID()
    {
        if (this.userInfo != null)
        {
            long netID = this.userInfo.NetID;
            this.UpdateNetID();
            return netID;
        }
        return 0L;
    }

    public EquipOne GetNewEquipByID(int equipid, int count)
    {
        Equip_equip beanById = LocalModelManager.Instance.Equip_equip.GetBeanById(equipid);
        if (beanById == null)
        {
            object[] args = new object[] { equipid };
            SdkManager.Bugly_Report("LocalSave_Equip.GetNewEquipByID", Utils.FormatString("Equip_equip dont have [{0}]", args));
        }
        return new EquipOne { 
            EquipID = beanById.Id,
            Level = 1,
            Count = count,
            WearIndex = -1
        };
    }

    public bool GetNoCard()
    {
        Dictionary<int, CardOne>.Enumerator enumerator = this.mCardData.mList.GetEnumerator();
        while (enumerator.MoveNext())
        {
            KeyValuePair<int, CardOne> current = enumerator.Current;
            if (current.Value.Unlock)
            {
                return false;
            }
        }
        return true;
    }

    public EquipOne GetPropByID(int equipid) => 
        this.mEquip.GetPropByID(equipid);

    public List<EquipOne> GetProps(EquipType type, bool havewear = true) => 
        this.mEquip.GetProps(type, havewear);

    public EquipOne GetPropShowByID(int equipid)
    {
        EquipOne propByID = this.GetPropByID(equipid);
        if (propByID == null)
        {
            propByID = new EquipOne {
                EquipID = equipid,
                Level = 1,
                Count = 0
            };
        }
        return propByID;
    }

    public int GetRebornCount() => 
        this.userInfo.reborncount;

    public long GetResource() => 
        this.userInfo.Resource;

    public int GetScore() => 
        this.userInfo.Score;

    public ulong GetServerUserID() => 
        this.userInfo.ServerUserID;

    public string GetServerUserIDSub()
    {
        string str = string.Empty;
        try
        {
            byte[] bytes = BitConverter.GetBytes(this.userInfo.ServerUserID);
            bytes[5] = 0;
            bytes[6] = 0;
            bytes[7] = 0;
            ulong num = BitConverter.ToUInt64(bytes, 0);
            object[] args = new object[] { bytes[7], num };
            str = Utils.FormatString("{0}{1}", args);
        }
        catch
        {
        }
        return str;
    }

    public long GetShowDiamond() => 
        this.userInfo.Show_Diamond;

    public long GetShowExp() => 
        this.userInfo.Show_Exp;

    public int GetTimeBoxCount(TimeBoxType type) => 
        this.mTimeBox.GetCount(type);

    public long GetTimeBoxTime(TimeBoxType type) => 
        this.mTimeBox?.GetTime(type);

    public string GetUserID()
    {
        if (this.userInfo == null)
        {
            return string.Empty;
        }
        if (!string.IsNullOrEmpty(this.userInfo.UserID_Temp))
        {
            return this.userInfo.UserID_Temp;
        }
        return this.userInfo.UserID;
    }

    public UserInfo GetUserInfo() => 
        this.userInfo;

    public bool GetUserInfoInit() => 
        this.userInfo.isInit;

    public bool GetUserLoginSDK() => 
        this.userInfo.bLoginedSDK;

    public string GetUserName()
    {
        if (this.userInfo == null)
        {
            return string.Empty;
        }
        if (!string.IsNullOrEmpty(this.userInfo.UserName_Temp))
        {
            return this.userInfo.UserName_Temp;
        }
        return this.userInfo.UserName;
    }

    public List<CardOne> GetWearCards()
    {
        List<CardOne> list = new List<CardOne>();
        Dictionary<int, CardOne>.Enumerator enumerator = this.GetCards().GetEnumerator();
        while (enumerator.MoveNext())
        {
            KeyValuePair<int, CardOne> current = enumerator.Current;
            if (current.Value.level > 0)
            {
                KeyValuePair<int, CardOne> pair2 = enumerator.Current;
                list.Add(pair2.Value);
            }
        }
        return list;
    }

    private void Init()
    {
        this.CreateThread();
    }

    private void InitAchieve()
    {
        this.mAchieveData = this.mSaveData.mAchieveData;
        this.mAchieveData.Init();
    }

    private void InitActive()
    {
        this.mActiveData = this.mSaveData.mActiveData;
        this.mActiveData.Init();
    }

    private void InitBoxDrop()
    {
        if (this.mBoxDropCard == null)
        {
            this.mBoxDropCard = this.mSaveData.mDropCard;
        }
        if (this.mBoxDropEquip == null)
        {
            this.mBoxDropEquip = new BoxDropEquip();
        }
    }

    private void InitCard()
    {
        this.mCardData = this.mSaveData.mCardData;
        this.mCardData.Init();
    }

    private void InitChallenge()
    {
        this.mChallengeData = this.mSaveData.mChallengeData;
        if (!this.mChallengeData.isinit)
        {
            this.mChallengeData.isinit = true;
            this.mChallengeData.Refresh();
        }
    }

    public void InitData()
    {
        this.InitBoxDrop();
        this.InitCard();
        this.InitActive();
        this.InitUserInfo();
        this.InitChallenge();
        this.InitKeyTime();
        this.InitTimeBoxTime();
        this.InitStage();
        this.InitEquips();
        this.InitAchieve();
        this.InitMail();
        this.InitHarvest();
        this.SaveExtra.Init();
    }

    private void InitEquips()
    {
        this.mEquip = FileUtils.GetXml<EquipData>("localequip.txt");
        this.mEquip.Init();
        this.mEquip.init_equipone_uniqueid();
    }

    private void InitHarvest()
    {
    }

    private void InitKeyTime()
    {
        long @long = PlayerPrefsEncrypt.GetLong("Currency_Key_Time", 0L);
        if (@long == 0L)
        {
            @long = Utils.GetTimeStamp();
            PlayerPrefsEncrypt.SetLong("Currency_Key_Time", @long);
        }
        this.mCurrencyKeyTime = @long;
    }

    private void InitMail()
    {
        this.Mail = this.mSaveData.mMail;
        this.Mail.Init();
    }

    private void InitStage()
    {
        if (this.mStage.CurrentStage == 0)
        {
            this.mStage.CurrentStage = 1;
            this.mStage.FirstIn = false;
            this.mStage.Refresh();
        }
    }

    private void InitTimeBoxTime()
    {
        this.mTimeBox = this.mSaveData.mTimeBoxData;
        this.mTimeBox.Init();
    }

    private void InitUserInfo()
    {
        this.userInfo = this.mSaveData.userInfo;
        if (!this.GetUserInfoInit())
        {
            this.userInfo.Key = GameConfig.GetMaxKeyStartCount();
            this.userInfo.Gold = 100L;
            this.userInfo.Diamond = 100L;
            this.userInfo.KeyTrustCount = GameConfig.GetKeyTrustCount();
            this.SetUserInfoInit();
        }
        this.userInfo.Show_Gold = this.userInfo.Gold;
        this.userInfo.Show_Diamond = this.userInfo.Diamond;
        this.userInfo.Show_Exp = this.userInfo.Exp;
    }

    public bool IsAdFree() => 
        false;

    public bool IsBoxOpenFree(TimeBoxType type) => 
        this.mTimeBox?.IsBoxOpenFree(type);

    public bool IsKeyMax() => 
        (this.userInfo.Key >= GameConfig.GetMaxKeyCount());

    public bool IsTimeBoxMax(TimeBoxType type) => 
        this.mTimeBox?.IsMaxCount(type);

    public void LevelUp()
    {
        if (this.GetCanLevelUp())
        {
            this.userInfo.Exp -= this.GetExpByLevel(this.userInfo.Level);
            this.userInfo.Show_Exp = this.userInfo.Exp;
            this.userInfo.Level++;
            this.userInfo.Refresh();
            Facade.Instance.SendNotification("MainUI_UpdateExp");
        }
    }

    public void Modify_Diamond(long diamond, bool updateui = true)
    {
        this.userInfo.Diamond += diamond;
        if (updateui)
        {
            this.userInfo.Show_Diamond = this.userInfo.Diamond;
            Facade.Instance.SendNotification("PUB_UI_UPDATE_CURRENCY");
        }
        this.SaveUserInfo(updateui);
    }

    public void Modify_DiamondExtraCount(TimeBoxType type, int count)
    {
        if (this.userInfo != null)
        {
            if (type == TimeBoxType.BoxChoose_DiamondNormal)
            {
                if ((this.userInfo.DiamondNormalExtraCount == 0) && (count > 0))
                {
                    Instance.mGuideData.check_diamondbox_first_open();
                }
                this.userInfo.DiamondNormalExtraCount += count;
            }
            else
            {
                this.userInfo.DiamondLargeExtraCount += count;
            }
            Facade.Instance.SendNotification("MainUI_ShopRedCountUpdate");
            this.userInfo.Refresh();
        }
    }

    public void Modify_drop(string str)
    {
        try
        {
            Drop_DropModel.DropSaveOneData dropOne = Drop_DropModel.GetDropOne(str);
            if (dropOne.type == 1)
            {
                switch (dropOne.id)
                {
                    case 1:
                        this.Modify_Gold((long) dropOne.count, true);
                        return;

                    case 2:
                        this.Modify_Diamond((long) dropOne.count, true);
                        return;

                    case 3:
                        this.Modify_Key((long) dropOne.count, true);
                        return;

                    case 4:
                        this.Modify_RebornCount(dropOne.count);
                        return;
                }
                object[] args = new object[] { str };
                SdkManager.Bugly_Report("LocalSave_UserInfo", Utils.FormatString("Modify_drop currency : {0} is not a valid currency type.", args));
            }
            else
            {
                object[] args = new object[] { str };
                SdkManager.Bugly_Report("LocalSave_UserInfo", Utils.FormatString("Modify_drop other : {0} is not a valid type.", args));
            }
        }
        catch
        {
            object[] args = new object[] { str };
            SdkManager.Bugly_Report("LocalSave_UserInfo", Utils.FormatString("Modify_drop : {0} is error.", args));
        }
    }

    public void Modify_drop(string[] strs)
    {
        int index = 0;
        int length = strs.Length;
        while (index < length)
        {
            this.Modify_drop(strs[index]);
            index++;
        }
    }

    public void Modify_Exp(long exp, bool updateui = true)
    {
        this.userInfo.Exp += exp;
        if (updateui)
        {
            this.userInfo.Show_Exp = this.userInfo.Exp;
            Facade.Instance.SendNotification("MainUI_UpdateExp");
        }
        this.SaveUserInfo(false);
    }

    public void Modify_Gold(long gold, bool updateui = true)
    {
        this.userInfo.Gold += gold;
        if (updateui)
        {
            this.userInfo.Show_Gold = this.userInfo.Gold;
        }
        int coinCost = 200;
        Skill_slotoutcost beanById = LocalModelManager.Instance.Skill_slotoutcost.GetBeanById(1);
        if (beanById != null)
        {
            coinCost = beanById.CoinCost;
        }
        if ((this.userInfo.Gold >= coinCost) && (GameLogic.Hold.Guide.mCard != null))
        {
            GameLogic.Hold.Guide.mCard.StartGuide();
        }
        this.SaveUserInfo(updateui);
        if (GoldUpdateEvent != null)
        {
            GoldUpdateEvent(this.userInfo.Gold, gold);
        }
    }

    public void Modify_Key(long key, bool over = true)
    {
        int maxKeyCount = GameConfig.GetMaxKeyCount();
        if (key < 0L)
        {
            if ((this.userInfo.Key >= maxKeyCount) && ((this.userInfo.Key + key) < maxKeyCount))
            {
                this.SetKeyTime(Utils.GetTimeStamp());
            }
            this.userInfo.Key += (int) key;
        }
        else if ((this.userInfo.Key + key) > maxKeyCount)
        {
            if (over)
            {
                this.userInfo.Key += (int) key;
            }
            else
            {
                this.userInfo.Key = maxKeyCount;
            }
        }
        else
        {
            this.userInfo.Key += (int) key;
        }
        Facade.Instance.SendNotification("PUB_UI_UPDATE_CURRENCY");
        this.SaveUserInfo(true);
    }

    public void Modify_RebornCount(int value)
    {
        this.userInfo.reborncount += value;
        this.userInfo.Refresh();
    }

    public void Modify_Resource(long resource, bool updateui = true)
    {
        this.userInfo.Resource += resource;
        this.SaveUserInfo(updateui);
    }

    public void Modify_ShowDiamond(long diamond)
    {
        this.userInfo.Show_Diamond += diamond;
        Facade.Instance.SendNotification("PUB_UI_UPDATE_CURRENCY");
    }

    public void Modify_ShowExp(long exp)
    {
        this.userInfo.Show_Exp += exp;
        Facade.Instance.SendNotification("MainUI_UpdateExp");
    }

    public void Modify_ShowGold(long gold)
    {
        this.userInfo.Show_Gold += gold;
        Facade.Instance.SendNotification("PUB_UI_UPDATE_CURRENCY");
    }

    public void Modify_TimeBoxCount(TimeBoxType type, int count, bool over = false)
    {
        if (this.mTimeBox != null)
        {
            if (this.mTimeBox.IsMaxCount(type) && (count < 0))
            {
                this.SetTimeBoxTime(type, Utils.GetTimeStamp());
            }
            this.mTimeBox.UpdateCount(type, count, over);
        }
    }

    public void RefreshUserIDFromTemp()
    {
        this.userInfo.UserID = this.userInfo.UserID_Temp;
        this.userInfo.UserName = this.userInfo.UserName_Temp;
        this.userInfo.UserID_Temp = string.Empty;
        this.userInfo.UserName_Temp = string.Empty;
    }

    private void SaveUserInfo(bool updateui = true)
    {
        this.userInfo.Refresh();
        if (updateui)
        {
            Facade.Instance.SendNotification("PUB_UI_UPDATE_CURRENCY");
        }
    }

    public void Set_Gold(long gold, bool updateui = true)
    {
        this.userInfo.Gold = gold;
        if (updateui)
        {
            this.userInfo.Show_Gold = gold;
            Facade.Instance.SendNotification("PUB_UI_UPDATE_CURRENCY");
        }
    }

    public void SetDiamondBoxGuide()
    {
        this.userInfo.guide_diamondbox = true;
        this.userInfo.Refresh();
    }

    public void SetDiamondExtraCount(TimeBoxType type, int count)
    {
        if (this.userInfo != null)
        {
            if (type == TimeBoxType.BoxChoose_DiamondNormal)
            {
                this.userInfo.DiamondNormalExtraCount = count;
            }
            else
            {
                this.userInfo.DiamondLargeExtraCount = count;
            }
            Facade.Instance.SendNotification("MainUI_ShopRedCountUpdate");
            this.userInfo.Refresh();
        }
    }

    public void SetEquips(CRespItemPacket data)
    {
        if ((data != null) && (data.m_arrayEquipItems != null))
        {
            this.mEquip.SetEquips(data.m_arrayEquipItems);
        }
    }

    public void SetExp(int exp)
    {
        this.userInfo.Exp = exp;
        this.SaveUserInfo(false);
    }

    public void SetKeyTime(long time)
    {
        if (this.mCurrencyKeyTime < time)
        {
            this.UserInfo_SetKeyTime(time);
        }
    }

    public void SetLevel(int level)
    {
        this.userInfo.Level = level;
        this.SaveUserInfo(false);
    }

    public void SetServerUserID(ulong id)
    {
        this.userInfo.ServerUserID = id;
        if (this.userInfo.ServerUserID != 0L)
        {
            SdkManager.ShuShu_Login(this.GetServerUserIDSub());
        }
        this.userInfo.Refresh();
    }

    public void SetTimeBoxTime(TimeBoxType type, long time = 0L)
    {
        if (this.mTimeBox != null)
        {
            if (time == 0L)
            {
                time = Utils.GetTimeStamp();
            }
            Debugger.Log(string.Concat(new object[] { "SetTimeBoxTime ", type, " -> ", time }));
            this.mTimeBox.SetTime(type, time);
        }
    }

    public void SetUserID(LoginType type, string id, string name)
    {
        this.userInfo.loginType = type;
        this.userInfo.UserID = id;
        this.userInfo.UserName = name;
        this.SaveUserInfo(false);
    }

    public void SetUserInfoInit()
    {
        this.userInfo.isInit = true;
        this.Modify_Gold(0L, true);
        this.Modify_TimeBoxCount(TimeBoxType.BoxChoose_DiamondNormal, 0, true);
        this.SaveUserInfo(false);
    }

    public void SetUserLoginSDK(bool value)
    {
        this.userInfo.bLoginedSDK = value;
    }

    public void SetUserName(string name)
    {
        this.userInfo.UserName = name;
        this.SaveUserInfo(false);
    }

    public void SetUserName_Temp(string name)
    {
        if (this.userInfo != null)
        {
            this.userInfo.UserName_Temp = name;
        }
    }

    public void SetUserTemp(string id, string name)
    {
        if (this.userInfo != null)
        {
            this.userInfo.UserID_Temp = id;
            this.userInfo.UserName_Temp = name;
        }
    }

    public void Stage_CheckUnlockNext(int roomID)
    {
        int num = GameLogic.Hold.BattleData.Level_CurrentStage;
        int maxChapter = LocalModelManager.Instance.Stage_Level_stagechapter.GetMaxChapter();
        if ((num < maxChapter) && (num == this.Stage_GetStage()))
        {
            Stage_Level_stagechapter beanByChapter = LocalModelManager.Instance.Stage_Level_stagechapter.GetBeanByChapter(num + 1);
            int argsOpen = beanByChapter.ArgsOpen;
            if ((beanByChapter != null) && (roomID >= argsOpen))
            {
                Stage mStage = this.mStage;
                mStage.CurrentStage++;
                this.mStage.FirstIn = true;
                this.mStage.Refresh();
                GameLogic.Hold.BattleData.Level_CurrentStage = this.mStage.CurrentStage;
            }
        }
    }

    public bool Stage_GetFirstIn()
    {
        if (this.mStage != null)
        {
            return this.mStage.FirstIn;
        }
        FileUtils.WriteError("Stage_GetFirstIn stage is null.");
        return false;
    }

    public void Stage_GetNextEnd()
    {
        if (this.mStage != null)
        {
            Stage mStage = this.mStage;
            mStage.BoxLayerID++;
            this.mStage.Refresh();
        }
    }

    public int Stage_GetNextID()
    {
        if (this.mStage != null)
        {
            return (this.mStage.BoxLayerID + 1);
        }
        return 1;
    }

    public int Stage_GetStage()
    {
        if (this.mStage != null)
        {
            return this.mStage.CurrentStage;
        }
        FileUtils.WriteError("Stage_GetStage stage is null.");
        return 1;
    }

    public void Stage_InitNextID(int id)
    {
        if (this.mStage != null)
        {
            this.mStage.BoxLayerID = id;
            this.mStage.Refresh();
        }
    }

    public void Stage_SetFirstIn()
    {
        if (this.mStage != null)
        {
            this.mStage.FirstIn = false;
            this.mStage.Refresh();
        }
    }

    public void Stage_SetStage(int stageid)
    {
        if (this.mStage != null)
        {
            this.mStage.CurrentStage = stageid;
        }
    }

    public int StageDiscount_GetCurrentID()
    {
        if (this.mStageDiscount != null)
        {
            return this.mStageDiscount.Get_CurrentID();
        }
        return 0x270f;
    }

    public int StageDiscount_GetLastID()
    {
        if (this.mStageDiscount != null)
        {
            return this.mStageDiscount.Get_LastID();
        }
        return 100;
    }

    public List<Drop_DropModel.DropData> StageDiscount_GetList()
    {
        if (this.mStageDiscount != null)
        {
            return this.mStageDiscount.GetList();
        }
        return null;
    }

    public string StageDiscount_GetProductID()
    {
        if ((this.mStageDiscount != null) && (this.mStageDiscount.current_purchase != null))
        {
            return this.mStageDiscount.current_purchase.product_id;
        }
        return string.Empty;
    }

    public void StageDiscount_Init(string data)
    {
        Debugger.Log("StageDiscount_Init " + data);
        bool flag = true;
        if (string.IsNullOrEmpty(data))
        {
            if (this.mStageDiscount == null)
            {
                flag = false;
            }
            this.mStageDiscount = null;
        }
        else
        {
            try
            {
                this.mStageDiscount = JsonConvert.DeserializeObject<StageDiscountBody>(data);
                Debugger.Log(this.mStageDiscount.ToString());
            }
            catch
            {
                this.mStageDiscount = null;
                Debugger.Log("StageDiscount_Init init failed! ::: " + data);
            }
        }
        if (flag)
        {
            Facade.Instance.SendNotification("ShopUI_Update");
        }
    }

    public bool StageDiscount_IsValid() => 
        ((this.mStageDiscount != null) && this.mStageDiscount.IsValid);

    public void StageDiscount_Send(Action<string> callback)
    {
        <StageDiscount_Send>c__AnonStorey0 storey = new <StageDiscount_Send>c__AnonStorey0 {
            callback = callback
        };
        CQueryFirstRewardInfoPacket packet = new CQueryFirstRewardInfoPacket();
        NetManager.SendInternal<CQueryFirstRewardInfoPacket>(packet, SendType.eLoop, new Action<NetResponse>(storey.<>m__0));
    }

    public void TrustCount_Refresh()
    {
        this.userInfo.KeyTrustCount = GameConfig.GetKeyTrustCount();
        this.userInfo.Refresh();
    }

    public bool TrustCount_Use(short count)
    {
        if (this.userInfo.KeyTrustCount >= count)
        {
            this.userInfo.KeyTrustCount = (short) (this.userInfo.KeyTrustCount - count);
            this.userInfo.Refresh();
            return true;
        }
        return false;
    }

    public void TryLogin(Action<bool, CRespUserLoginPacket> callback)
    {
        <TryLogin>c__AnonStorey3 storey = new <TryLogin>c__AnonStorey3 {
            callback = callback,
            $this = this
        };
        Action<LoginType, SdkManager.LoginData> action = new Action<LoginType, SdkManager.LoginData>(storey.<>m__0);
        SdkManager.TryLogin(action);
    }

    private void TryLoginServer(Action<bool, CRespUserLoginPacket> callback)
    {
        <TryLoginServer>c__AnonStorey4 storey = new <TryLoginServer>c__AnonStorey4 {
            callback = callback
        };
        CUserLoginPacket packet = new CUserLoginPacket();
        NetManager.SendInternal<CUserLoginPacket>(packet, SendType.eUDP, new Action<NetResponse>(storey.<>m__0));
    }

    public void UpdateEquip(int position, int uniqueid, EquipOne equip)
    {
        this.mEquip.UpdateEquip(equip);
    }

    private void UpdateNetID()
    {
        if (this.userInfo != null)
        {
            this.userInfo.NetID += 1L;
            this.userInfo.Refresh();
        }
    }

    public bool Use_Gold(long gold)
    {
        if (this.userInfo.Gold >= gold)
        {
            this.userInfo.Gold -= gold;
            this.SaveUserInfo(true);
            return true;
        }
        return false;
    }

    public void UseActiveCount(ActiveOne one)
    {
        one.Count--;
        this.mAchieveData.Refresh();
    }

    public int UserInfo_GetAdKeyCount()
    {
        if (this.userInfo != null)
        {
            return this.userInfo.AdKeyCount;
        }
        return 0;
    }

    public void UserInfo_Set(int gold, int diamond, int exp, int key, int level, int diamondnormal, int diamondlarge)
    {
        this.userInfo.Gold = gold;
        this.userInfo.Show_Gold = gold;
        this.userInfo.Diamond = diamond;
        this.userInfo.Show_Diamond = diamond;
        this.userInfo.Exp = exp;
        this.userInfo.Show_Exp = exp;
        this.userInfo.Key = key;
        this.userInfo.Level = level;
        this.userInfo.DiamondNormalExtraCount = diamondnormal;
        this.userInfo.DiamondLargeExtraCount = diamondlarge;
        if (this.userInfo.Key > 0)
        {
            this.TrustCount_Refresh();
        }
        this.userInfo.Refresh();
        Facade.Instance.SendNotification("PUB_UI_UPDATE_CURRENCY");
    }

    public void UserInfo_SetAdKeyCount(int count)
    {
        this.userInfo.AdKeyCount = count;
        this.userInfo.Refresh();
    }

    public void UserInfo_SetBestScore(int score)
    {
        if (this.userInfo.Score < score)
        {
            this.userInfo.Score = score;
            Facade.Instance.SendNotification("MainUI_LayerUpdate");
        }
    }

    public void UserInfo_SetDiamond(int diamond)
    {
        this.userInfo.Diamond = diamond;
        this.userInfo.Show_Diamond = diamond;
        Facade.Instance.SendNotification("PUB_UI_UPDATE_CURRENCY");
    }

    public void UserInfo_SetGold(int gold)
    {
        this.userInfo.Gold = gold;
        this.userInfo.Show_Gold = gold;
        this.Modify_Gold(0L, true);
        Facade.Instance.SendNotification("PUB_UI_UPDATE_CURRENCY");
    }

    public void UserInfo_SetKey(int key)
    {
        this.userInfo.Key = key;
        Facade.Instance.SendNotification("PUB_UI_UPDATE_CURRENCY");
    }

    public void UserInfo_SetKeyTime(long time)
    {
        this.mCurrencyKeyTime = time;
        PlayerPrefsEncrypt.SetLong("Currency_Key_Time", time);
    }

    public void UserInfo_SetRebornCount(int count)
    {
        this.userInfo.reborncount = count;
        this.userInfo.Refresh();
    }

    public bool UserInfo_UseAdKeyCount()
    {
        if ((this.userInfo != null) && (this.userInfo.AdKeyCount > 0))
        {
            this.userInfo.AdKeyCount--;
            this.userInfo.Refresh();
            return true;
        }
        return false;
    }

    public void UsserInfo_SetTimeBoxCount(TimeBoxType type, int count)
    {
        if (this.mTimeBox != null)
        {
            this.mTimeBox.SetCount(type, count);
        }
    }

    public static LocalSave Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new LocalSave();
                _instance.Init();
            }
            return _instance;
        }
    }

    public bool BattleIn_In { get; private set; }

    public BattleInBase BattleIn =>
        this.mBattleIn;

    public EquipData mEquip { get; private set; }

    public LocalSaveExtra SaveExtra
    {
        get
        {
            if (this._saveExtra == null)
            {
                this._saveExtra = this.mSaveData.mExtra;
            }
            return this._saveExtra;
        }
    }

    public FakeStageDrop mFakeStageDrop
    {
        get
        {
            if (this._fakestagedrop == null)
            {
                this._fakestagedrop = this.mSaveData.mFakeStage;
            }
            return this._fakestagedrop;
        }
    }

    public FakeCardCost mFakeCardCost
    {
        get
        {
            if (this._fakecardcost == null)
            {
                this._fakecardcost = this.mSaveData.mFakeCardCost;
            }
            return this._fakecardcost;
        }
    }

    public GuideData mGuideData
    {
        get
        {
            if (this._guidedata == null)
            {
                this._guidedata = this.mSaveData.mGuideData;
            }
            return this._guidedata;
        }
    }

    public HarvestData mHarvest
    {
        get
        {
            if (this._harvest == null)
            {
                this._harvest = this.mSaveData.mHarvest;
                this._harvest.Init();
            }
            return this._harvest;
        }
        set => 
            (this._harvest = value);
    }

    public LocalMail Mail { get; private set; }

    public PurchaseData mPurchase
    {
        get
        {
            if (this._purchase == null)
            {
                this._purchase = this.mSaveData.mPurchase;
            }
            return this._purchase;
        }
    }

    public ShopLocal mShop
    {
        get
        {
            if (this._shop == null)
            {
                this._shop = this.mSaveData.mShopLocal;
                this._shop.Init();
            }
            return this._shop;
        }
    }

    public Stage mStage
    {
        get
        {
            if (this._stage == null)
            {
                this._stage = this.mSaveData.mStage;
            }
            return this._stage;
        }
        set => 
            (this._stage = value);
    }

    [CompilerGenerated]
    private sealed class <DoLogin_Start>c__AnonStorey1
    {
        internal Action callback;
        internal LocalSave $this;

        internal void <>m__0(SdkManager.LoginData response)
        {
            this.$this.DoLogin(SendType.eLoop, delegate {
                Debugger.Log("====DoLogin_Start DoLogin callback");
                if (this.callback != null)
                {
                    this.callback();
                }
            });
        }

        internal void <>m__1()
        {
            Debugger.Log("====DoLogin_Start DoLogin callback");
            if (this.callback != null)
            {
                this.callback();
            }
        }
    }

    [CompilerGenerated]
    private sealed class <DoLogin>c__AnonStorey2
    {
        internal Action callback;
        internal SendType sendType;
        internal LocalSave $this;

        internal void <>m__0(NetResponse response)
        {
            if ((response != null) && (response.error != null))
            {
                if (response.error.m_nStatusCode == 0x10)
                {
                    WindowUI.ShowWindow(WindowID.WindowID_ForceUpdate);
                }
            }
            else if ((response.IsSuccess && (response.data != null)) && (response.data is CRespUserLoginPacket))
            {
                CRespUserLoginPacket data = response.data as CRespUserLoginPacket;
                if ((LocalSave.Instance.GetServerUserID() != 0L) && (LocalSave.Instance.GetServerUserID() != data.m_nUserRawId))
                {
                    this.$this.DoLoginCallBack(data, delegate {
                        if (this.callback != null)
                        {
                            this.callback();
                        }
                        WindowUI.ReOpenMain();
                    });
                }
                else
                {
                    this.$this.DoLoginCallBack(data, this.callback);
                }
                if (data == null)
                {
                    this.$this.DoLogin(this.sendType, this.callback);
                }
            }
            else
            {
                this.$this.DoLoginCallBack(null, this.callback);
            }
        }

        internal void <>m__1()
        {
            if (this.callback != null)
            {
                this.callback();
            }
            WindowUI.ReOpenMain();
        }
    }

    [CompilerGenerated]
    private sealed class <StageDiscount_Send>c__AnonStorey0
    {
        internal Action<string> callback;

        internal void <>m__0(NetResponse response)
        {
            if (response.error != null)
            {
                CCommonRespMsg error = response.error;
                if (error != null)
                {
                    if (this.callback != null)
                    {
                        this.callback(error.m_strInfo);
                    }
                }
                else
                {
                    SdkManager.Bugly_Report("LocalSave_StageDiscount", "OnLoginCallback_back response is not a CRespFirstRewardInfo Type");
                }
            }
        }
    }

    [CompilerGenerated]
    private sealed class <TryLogin>c__AnonStorey3
    {
        internal Action<bool, CRespUserLoginPacket> callback;
        internal LocalSave $this;

        internal void <>m__0(LoginType logintype, SdkManager.LoginData userdata)
        {
            LocalSave.Instance.SetUserTemp(userdata.userid, userdata.username);
            this.$this.TryLoginServer(this.callback);
        }
    }

    [CompilerGenerated]
    private sealed class <TryLoginServer>c__AnonStorey4
    {
        internal Action<bool, CRespUserLoginPacket> callback;

        internal void <>m__0(NetResponse response)
        {
            if (response.IsSuccess && (response.data != null))
            {
                CRespUserLoginPacket data = response.data as CRespUserLoginPacket;
                if (LocalSave.Instance.GetServerUserID() != data.m_nUserRawId)
                {
                    if (this.callback != null)
                    {
                        this.callback(false, data);
                    }
                }
                else if (this.callback != null)
                {
                    this.callback(true, null);
                }
            }
        }
    }

    [Serializable]
    public class AchieveData : LocalSaveBase
    {
        public Dictionary<int, LocalSave.AchieveDataOne> list = new Dictionary<int, LocalSave.AchieveDataOne>();

        public void AddProgress(int id, int count)
        {
            this.InitID(id);
            LocalSave.AchieveDataOne local1 = this.list[id];
            local1.currentcount += count;
            base.Refresh();
        }

        public LocalSave.AchieveDataOne Get(int id)
        {
            this.InitID(id);
            return this.list[id];
        }

        public void Init()
        {
        }

        private void InitID(int id)
        {
            if (!this.list.ContainsKey(id))
            {
                LocalSave.AchieveDataOne one = new LocalSave.AchieveDataOne {
                    achieveid = id
                };
                this.list.Add(id, one);
                base.Refresh();
            }
        }

        public bool IsFinish(int id)
        {
            this.InitID(id);
            return this.list[id].isfinish;
        }

        public bool Isgot(int id)
        {
            this.InitID(id);
            return this.list[id].isgot;
        }

        protected override void OnRefresh()
        {
            FileUtils.WriteXml<LocalSave.AchieveData>("File_Achieve1", this);
        }
    }

    [Serializable]
    public class AchieveDataOne
    {
        public int achieveid;
        public int currentcount;
        public bool isgot;
        [JsonIgnore]
        private Achieve_Achieve _data;
        private int _maxcount = -1;
        [JsonIgnore]
        private AchieveConditionBase _condition;

        [JsonIgnore]
        public Achieve_Achieve mData
        {
            get
            {
                if (this._data == null)
                {
                    this._data = LocalModelManager.Instance.Achieve_Achieve.GetBeanById(this.achieveid);
                }
                return this._data;
            }
        }

        [JsonIgnore]
        public int maxcount
        {
            get
            {
                if (this._maxcount < 0)
                {
                    if (this.mData.CondTypeArgs.Length > 0)
                    {
                        int.TryParse(this.mData.CondTypeArgs[0], out this._maxcount);
                    }
                    else
                    {
                        object[] args = new object[] { this.achieveid };
                        SdkManager.Bugly_Report("LocalSave_Achieve", Utils.FormatString(" id:{0}  CondTypeArgs.Length == 0", args));
                    }
                }
                return this._maxcount;
            }
        }

        [JsonIgnore]
        public bool isfinish =>
            (this.currentcount >= this.maxcount);

        [JsonIgnore]
        public AchieveConditionBase mCondition
        {
            get
            {
                if (this._condition == null)
                {
                    int condType = this.mData.CondType;
                    object[] args = new object[] { "AchieveCondition", condType };
                    Type type = Type.GetType(Utils.GetString(args));
                    object[] objArray2 = new object[] { "AchieveCondition", condType };
                    this._condition = type.Assembly.CreateInstance(Utils.GetString(objArray2)) as AchieveConditionBase;
                    this._condition.Init(this);
                }
                return this._condition;
            }
        }
    }

    [Serializable]
    public class ActiveData : LocalSaveBase
    {
        public List<LocalSave.ActiveOne> list = new List<LocalSave.ActiveOne>();

        public void Init()
        {
            if (this.list.Count == 0)
            {
                List<Stage_Level_activityModel.ActivityTypeData> difficults = LocalModelManager.Instance.Stage_Level_activity.GetDifficults();
                int num = 0;
                int count = difficults.Count;
                while (num < count)
                {
                    int num3 = difficults[num].GetCount(0);
                    LocalSave.ActiveOne item = new LocalSave.ActiveOne {
                        Index = num,
                        Count = num3
                    };
                    this.list.Add(item);
                    num++;
                }
                base.Refresh();
            }
        }

        protected override void OnRefresh()
        {
            FileUtils.WriteXml<LocalSave.ActiveData>("File_Active", this);
        }
    }

    [Serializable]
    public class ActiveOne
    {
        public int Index;
        public int Count;
    }

    [Serializable]
    public class BattleInBase : LocalSaveBase
    {
        public bool bHaveBattle;
        public ulong serveruserid;
        public int level;
        public float exp;
        public float gold;
        public List<int> skillids = new List<int>();
        public List<int> goodids = new List<int>();
        public List<LocalSave.EquipOne> equips = new List<LocalSave.EquipOne>();
        public long hp;
        public int RoomID;
        public int ResourcesID;
        public string TmxID;
        public int reborn_skill_count;
        public int reborn_ui_count;
        public int leveluptype;
        public List<int> levelupskills = new List<int>();
        public List<int> learnskills = new List<int>();
        public bool bGoldTurn;
        public int stage;
        public List<bool> firstshopbuy;
        public List<int> potions;
        private Sequence seq;

        public void AddEquip(LocalSave.EquipOne one)
        {
            this.equips.Add(one);
            base.Refresh();
        }

        public void AddPotion(int id)
        {
            if (this.potions == null)
            {
                this.potions = new List<int>();
            }
            this.potions.Add(id);
        }

        public void AddRebornSkill()
        {
            this.reborn_skill_count++;
            base.Refresh();
        }

        public void AddRebornUI()
        {
            this.reborn_ui_count++;
            base.Refresh();
        }

        public void CheckDifferentID()
        {
            if (this.serveruserid != LocalSave.Instance.GetServerUserID())
            {
                this.DeInit();
            }
        }

        public void DeInit()
        {
            this.bGoldTurn = false;
            this.SetHaveBattle(false);
            this.reborn_skill_count = 0;
            this.reborn_ui_count = 0;
            this.level = 1;
            this.exp = 0f;
            this.hp = 0L;
            this.RoomID = 0;
            this.ResourcesID = 1;
            this.TmxID = string.Empty;
            this.levelupskills = null;
            this.equips.Clear();
            this.learnskills.Clear();
            this.skillids.Clear();
            this.goodids.Clear();
            this.LevelInit();
            List<bool> list = new List<bool> { 
                false,
                false
            };
            this.firstshopbuy = this.firstshopbuy = list;
            this.potions.Clear();
            this.OnDeInit();
            base.Refresh();
        }

        public static LocalSave.BattleInBase Get()
        {
            LocalSave.BattleInBase battleIn = FileUtils.GetBattleIn();
            if (!battleIn.GetHaveBattle())
            {
                battleIn.DeInit();
            }
            return battleIn;
        }

        public static string GetFileName(ulong serveruserid)
        {
            object[] args = new object[] { serveruserid };
            return Utils.FormatString("{0}-battle.txt", args);
        }

        public bool GetHaveBattle() => 
            ((this.serveruserid == LocalSave.Instance.GetServerUserID()) && this.bHaveBattle);

        public void LevelInit()
        {
            if ((this.firstshopbuy == null) || (this.firstshopbuy.Count < 2))
            {
                List<bool> list = new List<bool> { 
                    false,
                    false
                };
                this.firstshopbuy = list;
            }
            if (this.potions == null)
            {
                this.potions = new List<int>();
            }
        }

        protected virtual void OnDeInit()
        {
        }

        protected override void OnRefresh()
        {
            if (this.seq != null)
            {
                TweenExtensions.Kill(this.seq, false);
                this.seq = null;
            }
            this.seq = TweenSettingsExtensions.SetUpdate<Sequence>(TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(DOTween.Sequence(), 0.1f), new TweenCallback(this, this.<OnRefresh>m__0)), true);
        }

        public void SetHaveBattle(bool value)
        {
            this.bHaveBattle = value;
            base.Refresh();
        }

        public override string ToString()
        {
            if (this.GetHaveBattle())
            {
                object[] args = new object[] { this.level, this.exp, this.hp };
                return Utils.FormatString("Level:{1}, Exp:{2}, HP:{3}", args);
            }
            return Utils.FormatString("BattleInData don't in battle.", Array.Empty<object>());
        }
    }

    [Serializable]
    public class BoxDropEquip : LocalSaveBase
    {
        public int stage = -1;
        public int count;
        public int dropid;

        public Drop_DropModel.DropData GetRandom()
        {
            this.UpdateStage();
            Drop_FakeDrop beanById = LocalModelManager.Instance.Drop_FakeDrop.GetBeanById(this.dropid);
            this.count++;
            if (this.count >= beanById.RandNum)
            {
                this.count = 0;
                this.dropid = beanById.JumpDrop;
            }
            base.Refresh();
            return LocalModelManager.Instance.Drop_Drop.GetDropList(beanById.DropID)[0];
        }

        protected int OnGetDropID(int currentstage) => 
            LocalModelManager.Instance.Box_SilverBox.GetBeanById(currentstage).SingleDrop;

        protected override void OnRefresh()
        {
            FileUtils.WriteXml<LocalSave.BoxDropEquip>("File_BoxDrop", this);
        }

        private void UpdateStage()
        {
            int currentstage = LocalSave.Instance.Stage_GetStage();
            if (currentstage != this.stage)
            {
                this.stage = currentstage;
                this.count = 0;
                this.dropid = this.OnGetDropID(currentstage);
                base.Refresh();
            }
        }
    }

    [Serializable]
    public class CardData : LocalSaveBase
    {
        public Dictionary<int, LocalSave.CardOne> mList = new Dictionary<int, LocalSave.CardOne>();

        public LocalSave.CardOne AddOne(int cardid, int count)
        {
            if (this.mList.TryGetValue(cardid, out LocalSave.CardOne one))
            {
                one.level++;
            }
            else
            {
                one = new LocalSave.CardOne(cardid, 1, count) {
                    CardID = cardid,
                    HaveCount = count,
                    level = 1
                };
                this.mList.Add(cardid, one);
            }
            base.Refresh();
            if (LocalSave.CardUpdateEvent != null)
            {
                LocalSave.CardUpdateEvent();
            }
            return one;
        }

        public void Clear()
        {
            this.mList.Clear();
            this.Init();
        }

        public bool GetAllMax()
        {
            Dictionary<int, LocalSave.CardOne>.Enumerator enumerator = this.mList.GetEnumerator();
            while (enumerator.MoveNext())
            {
                KeyValuePair<int, LocalSave.CardOne> current = enumerator.Current;
                if (!current.Value.IsMaxLevel)
                {
                    return false;
                }
            }
            return true;
        }

        public LocalSave.CardOne GetCardByID(int id)
        {
            LocalSave.CardOne one = null;
            this.mList.TryGetValue(id, out one);
            return one;
        }

        public Dictionary<int, LocalSave.CardOne> GetCards() => 
            this.mList;

        public int GetCount() => 
            this.mList.Count;

        private int GetIndex(LocalSave.CardOne one)
        {
            int num = 0;
            Dictionary<int, LocalSave.CardOne>.Enumerator enumerator = this.mList.GetEnumerator();
            while (enumerator.MoveNext())
            {
                KeyValuePair<int, LocalSave.CardOne> current = enumerator.Current;
                if (current.Value.CardID == one.CardID)
                {
                    return num;
                }
                num++;
            }
            object[] args = new object[] { one.CardID };
            SdkManager.Bugly_Report("LocalSave_Card.cs", Utils.FormatString("CardData.GetIndex {0} is dont have!", args));
            return 0;
        }

        public bool HaveCard(int id)
        {
            LocalSave.CardOne one = null;
            this.mList.TryGetValue(id, out one);
            if (one == null)
            {
                return false;
            }
            return (one.level > 0);
        }

        public void Init()
        {
            if (this.IsEmpty())
            {
                IList<Skill_slotout> allBeans = LocalModelManager.Instance.Skill_slotout.GetAllBeans();
                for (int i = 0; i < allBeans.Count; i++)
                {
                    Skill_slotout _slotout = allBeans[i];
                    if (_slotout.GroupID > 100)
                    {
                        LocalSave.CardOne one = new LocalSave.CardOne(_slotout.GroupID, 1, 0) {
                            CardID = _slotout.GroupID,
                            HaveCount = 0,
                            level = 0
                        };
                        this.mList.Add(_slotout.GroupID, one);
                    }
                }
                base.Refresh();
            }
        }

        private bool IsEmpty() => 
            (this.mList.Count == 0);

        protected override void OnRefresh()
        {
            FileUtils.WriteXml<LocalSave.CardData>("File_Card", this);
        }

        public void SetOne(int cardid, int level)
        {
            if (this.mList.TryGetValue(cardid, out LocalSave.CardOne one))
            {
                one.level = level;
                base.Refresh();
            }
        }
    }

    [Serializable]
    public class CardOne
    {
        public int CardID;
        public int level;
        public int HaveCount;
        [JsonIgnore]
        private Skill_slotout _data;

        public CardOne()
        {
        }

        public CardOne(int cardid, int level, int count)
        {
            this.CardID = cardid;
            this.level = 1;
            this.HaveCount = count;
        }

        private string GetAttribute(int index, int addlevel)
        {
            Goods_goods.GoodData goodData = Goods_goods.GetGoodData(this.data.BaseAttributes[index]);
            if (goodData.percent)
            {
                if (addlevel > 0)
                {
                    float num = this.data.AddAttributes[index] * 100f;
                    goodData.value += (addlevel - 1) * ((long) num);
                }
                object[] args = new object[] { ((float) goodData.value) / 100f };
                return Utils.FormatString("{0}%", args);
            }
            if (addlevel > 0)
            {
                float num2 = this.data.AddAttributes[index];
                goodData.value += (addlevel - 1) * ((long) num2);
            }
            return goodData.value.ToString();
        }

        public string GetCurrentAttribute(int index) => 
            this.GetAttribute(index, this.level - 1);

        public string GetNextAttribute(int index) => 
            this.GetAttribute(index, this.level);

        public string GetTypeName(int index)
        {
            Goods_goods.GoodData goodData = Goods_goods.GetGoodData(this.data.BaseAttributes[index]);
            object[] args = new object[] { goodData.goodType };
            return GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("Up_{0}", args), Array.Empty<object>());
        }

        public string GetValue(string value)
        {
            string str = value.Substring(value.Length - 1, 1);
            if (str == null)
            {
                return value;
            }
            if (str != "f")
            {
                return value;
            }
            value = value.Substring(0, value.Length - 1);
            int.TryParse(value, out int num);
            object[] args = new object[] { ((float) num) / 1000f };
            return Utils.FormatString("{0}", args);
        }

        public override string ToString()
        {
            object[] args = new object[] { this.CardID };
            return Utils.FormatString("ID:{0}", args);
        }

        [JsonIgnore]
        public Skill_slotout data
        {
            get
            {
                if (this._data == null)
                {
                    this._data = LocalModelManager.Instance.Skill_slotout.GetBeanById(this.CardID);
                }
                return this._data;
            }
        }

        [JsonIgnore]
        public bool Unlock =>
            (this.level > 0);

        [JsonIgnore]
        public bool IsMaxLevel =>
            (this.level >= this.data.LevelLimit);
    }

    [Serializable]
    public class ChallengeData : LocalSaveBase
    {
        public int ChallengeID = 0x835;
        public bool bFirstIn;
        public bool isinit;

        protected override void OnRefresh()
        {
            FileUtils.WriteXml<LocalSave.ChallengeData>("File_Challenge", this);
        }
    }

    [Serializable]
    public class DropCard : LocalSaveBase
    {
        public int count;
        public int dropid = 0x3e9;

        public Drop_DropModel.DropData GetRandom()
        {
            Drop_FakeDrop beanById = LocalModelManager.Instance.Drop_FakeDrop.GetBeanById(this.dropid);
            List<Drop_DropModel.DropData> dropList = null;
            dropList = LocalModelManager.Instance.Drop_Drop.GetDropList(beanById.DropID);
            if (dropList.Count == 0)
            {
                object[] args = new object[] { beanById.DropID };
                SdkManager.Bugly_Report("LocalSave_BoxDrop", Utils.FormatString("Drop_Drop[{0}] get null.", args));
            }
            if (LocalSave.Instance.GetCardByID(dropList[0].id).IsMaxLevel)
            {
                return this.GetRandom();
            }
            return dropList[0];
        }

        public void GetSucceed()
        {
            this.count++;
            Drop_FakeDrop beanById = LocalModelManager.Instance.Drop_FakeDrop.GetBeanById(this.dropid);
            if (this.count >= beanById.RandNum)
            {
                this.dropid = beanById.JumpDrop;
                this.count = 0;
            }
        }

        public void InitCount(int allcount)
        {
            int num = allcount;
            this.dropid = 0x3e9;
            this.count = 0;
            while (allcount > 0)
            {
                Drop_FakeDrop beanById = LocalModelManager.Instance.Drop_FakeDrop.GetBeanById(this.dropid);
                int randNum = beanById.RandNum;
                allcount -= randNum;
                if (allcount >= 0)
                {
                    this.dropid = beanById.JumpDrop;
                }
                else
                {
                    this.count = randNum + allcount;
                }
            }
        }

        protected override void OnRefresh()
        {
            FileUtils.WriteXml<LocalSave.DropCard>("File_CardDrop", this);
        }
    }

    [Serializable]
    public class EquipData : LocalSaveBase
    {
        public Dictionary<string, LocalSave.EquipOne> list = new Dictionary<string, LocalSave.EquipOne>();
        public List<string> wears = new List<string>();
        [JsonIgnore]
        public List<string> invalids = new List<string>();
        [JsonIgnore]
        public bool bRefresh;
        [JsonIgnore]
        private bool bInitWear;
        [JsonIgnore]
        public Action mUpdateAction;
        [JsonIgnore]
        private int equipidd = 0xf69b5;
        [JsonIgnore]
        public List<int> mEquipExpCanDropList = new List<int>();
        [JsonIgnore]
        private Dictionary<int, int> mCombines = new Dictionary<int, int>();

        public void AddEquipInternal(LocalSave.EquipOne data)
        {
            if (data.UniqueID.Length == 0)
            {
                object[] args = new object[] { data.RowID };
                SdkManager.Bugly_Report("AddEquipInternal", Utils.FormatString("m_nRowID:{0} uuid is null", args));
                data.UniqueID = Utils.GenerateUUID();
            }
            if (data.Overlying)
            {
                data.bNew = false;
                LocalSave.EquipOne propByID = this.GetPropByID(data.EquipID);
                if (propByID != null)
                {
                    propByID.Count += data.Count;
                }
                else
                {
                    this.list.Add(data.UniqueID, data);
                }
            }
            else
            {
                bool flag = false;
                if (data.RowID != 0L)
                {
                    LocalSave.EquipOne one3 = this.get_by_rowid(data.RowID);
                    if (one3 != null)
                    {
                        one3.RowID = data.RowID;
                        one3.EquipID = data.EquipID;
                        one3.Level = data.Level;
                        one3.Count = data.Count;
                        flag = true;
                    }
                }
                if (!flag)
                {
                    if (!this.list.TryGetValue(data.UniqueID, out LocalSave.EquipOne one))
                    {
                        Debugger.LogEquipGet(string.Concat(new object[] { "UniqueID = ", data.UniqueID, " rowid ", data.RowID, " equipid ", data.EquipID, " dont in list" }));
                        this.list.Add(data.UniqueID, data);
                    }
                    else
                    {
                        one.RowID = data.RowID;
                        one.EquipID = data.EquipID;
                        one.Level = data.Level;
                        one.Count = data.Count;
                        Debugger.LogEquipGet(string.Concat(new object[] { "UniqueID = ", data.UniqueID, " rowid ", data.RowID, " equipid ", data.EquipID, " is in list" }));
                    }
                }
            }
            GameLogic.Hold.Guide.mEquip.StartGuide();
            this.Refresh_EquipExp_CanDrop(data);
            this.combine_refresh();
            Facade.Instance.SendNotification("MainUI_EquipRedCountUpdate");
            base.Refresh();
        }

        public void check_invalid()
        {
            this.check_invalid_internal(false);
        }

        private void check_invalid_internal(bool force = false)
        {
            int num = 0;
            int count = this.invalids.Count;
            while (num < count)
            {
                if (this.list.TryGetValue(this.invalids[num], out LocalSave.EquipOne one) && ((one.RowID == 0L) || force))
                {
                    this.RemoveEquip(one.UniqueID);
                }
                num++;
            }
            this.invalids.Clear();
            this.CheckWears();
            this.combine_refresh();
        }

        private void check_rowid_same()
        {
        }

        private void CheckWears()
        {
            int num = 0;
            int num2 = this.wears.Count - 1;
            while (num < num2)
            {
                string str = this.wears[num];
                if (!string.IsNullOrEmpty(str))
                {
                    int num3 = num + 1;
                    int num4 = this.wears.Count;
                    while (num3 < num4)
                    {
                        string str2 = this.wears[num3];
                        if (str == str2)
                        {
                            this.wears[num3] = null;
                        }
                        num3++;
                    }
                }
                num++;
            }
            int num5 = 0;
            int count = this.wears.Count;
            while (num5 < count)
            {
                string str3 = this.wears[num5];
                if (!string.IsNullOrEmpty(str3) && !this.list.ContainsKey(str3))
                {
                    this.wears[num5] = null;
                }
                num5++;
            }
            int num7 = 0;
            int num8 = this.wears.Count;
            while (num7 < num8)
            {
                string str4 = this.wears[num7];
                if (!string.IsNullOrEmpty(str4))
                {
                    LocalSave.EquipOne one = null;
                    if (this.list.TryGetValue(str4, out one))
                    {
                        one.WearIndex = num7;
                    }
                }
                num7++;
            }
        }

        public bool combine_can(LocalSave.EquipOne one)
        {
            int num = 0;
            return (this.mCombines.TryGetValue(one.EquipID, out num) && (num >= one.BreakNeed));
        }

        public int combine_can_count()
        {
            int num = 0;
            Dictionary<int, int>.Enumerator enumerator = this.mCombines.GetEnumerator();
            while (enumerator.MoveNext())
            {
                KeyValuePair<int, int> current = enumerator.Current;
                int breakNeed = LocalModelManager.Instance.Equip_equip.GetBeanById(current.Key).BreakNeed;
                KeyValuePair<int, int> pair2 = enumerator.Current;
                if (pair2.Value >= breakNeed)
                {
                    num++;
                }
            }
            return num;
        }

        private void combine_refresh()
        {
            this.mCombines.Clear();
            Dictionary<string, LocalSave.EquipOne>.Enumerator enumerator = this.list.GetEnumerator();
            while (enumerator.MoveNext())
            {
                KeyValuePair<string, LocalSave.EquipOne> current = enumerator.Current;
                if (!current.Value.Overlying)
                {
                    Dictionary<int, int> dictionary;
                    int num2;
                    KeyValuePair<string, LocalSave.EquipOne> pair2 = enumerator.Current;
                    int equipID = pair2.Value.EquipID;
                    if (!this.mCombines.ContainsKey(equipID))
                    {
                        this.mCombines.Add(equipID, 0);
                    }
                    (dictionary = this.mCombines)[num2 = equipID] = dictionary[num2] + 1;
                }
            }
        }

        public void DebugLog()
        {
        }

        public void EquipLevelUp(LocalSave.EquipOne one)
        {
            LocalSave.EquipOne equipByUniqueID = this.GetEquipByUniqueID(one.UniqueID);
            if (equipByUniqueID != null)
            {
                equipByUniqueID.Level++;
                base.Refresh();
                this.UpdateCallBack();
            }
        }

        public void EquipUnwear(string uniqueid)
        {
            if (this.list.TryGetValue(uniqueid, out LocalSave.EquipOne one))
            {
                if (((one.WearIndex >= 0) && (one.WearIndex < this.wears.Count)) && ((one.WearIndex >= 0) && (one.WearIndex < this.wears.Count)))
                {
                    this.wears[one.WearIndex] = null;
                }
                one.WearIndex = -1;
                base.Refresh();
                this.UpdateCallBack();
                Facade.Instance.SendNotification("MainUI_EquipRedCountUpdate");
            }
        }

        public void EquipWear(LocalSave.EquipOne data, int index)
        {
            if (this.list.TryGetValue(data.UniqueID, out LocalSave.EquipOne one))
            {
                if (!string.IsNullOrEmpty(this.wears[index]))
                {
                    this.EquipUnwear(this.wears[index]);
                }
                one.WearIndex = index;
                this.wears[index] = data.UniqueID;
                base.Refresh();
                this.UpdateCallBack();
                Facade.Instance.SendNotification("MainUI_EquipRedCountUpdate");
            }
            else
            {
                object[] args = new object[] { data.UniqueID, data.EquipID };
                SdkManager.Bugly_Report("LocalSave_Equip", Utils.FormatString("UniqueID:{0} EquipID:{1} don't int bags.", args));
            }
        }

        private LocalSave.EquipOne get_by_row_id(ulong rowid)
        {
            Dictionary<string, LocalSave.EquipOne>.Enumerator enumerator = this.list.GetEnumerator();
            while (enumerator.MoveNext())
            {
                KeyValuePair<string, LocalSave.EquipOne> current = enumerator.Current;
                if (current.Value.RowID == rowid)
                {
                    KeyValuePair<string, LocalSave.EquipOne> pair2 = enumerator.Current;
                    return pair2.Value;
                }
            }
            return null;
        }

        private LocalSave.EquipOne get_by_rowid(ulong rowid)
        {
            Dictionary<string, LocalSave.EquipOne>.Enumerator enumerator = this.list.GetEnumerator();
            while (enumerator.MoveNext())
            {
                KeyValuePair<string, LocalSave.EquipOne> current = enumerator.Current;
                if (current.Value.RowID == rowid)
                {
                    KeyValuePair<string, LocalSave.EquipOne> pair2 = enumerator.Current;
                    return pair2.Value;
                }
            }
            return null;
        }

        public bool Get_EquipExp_CanDrop(LocalSave.EquipOne one) => 
            ((((one != null) && (one.data != null)) && (one.NeedMatID > 0)) && this.Get_EquipExp_CanDrop(one.NeedMatID));

        public bool Get_EquipExp_CanDrop(int equipexpid) => 
            this.mEquipExpCanDropList.Contains(equipexpid);

        public int GetCanUpCount()
        {
            int num = 0;
            int num2 = 0;
            int count = this.wears.Count;
            while (num2 < count)
            {
                LocalSave.EquipOne equipByUniqueID = this.GetEquipByUniqueID(this.wears[num2]);
                if ((equipByUniqueID != null) && equipByUniqueID.CanLevelUp)
                {
                    num++;
                }
                num2++;
            }
            return num;
        }

        public int GetCanWearCount()
        {
            int num = 0;
            List<LocalSave.EquipOne> haveEquips = this.GetHaveEquips(false);
            int num2 = 0;
            int count = haveEquips.Count;
            while (num2 < count)
            {
                if (LocalSave.Instance.Equip_GetIsEmpty(haveEquips[num2]))
                {
                    num++;
                }
                num2++;
            }
            return num;
        }

        public LocalSave.EquipOne GetEquipByUniqueID(string uniqueid)
        {
            if (!string.IsNullOrEmpty(uniqueid) && this.list.TryGetValue(uniqueid, out LocalSave.EquipOne one))
            {
                return one;
            }
            return null;
        }

        public List<LocalSave.EquipOne> GetHaveEquips(bool havewear)
        {
            List<LocalSave.EquipOne> list = new List<LocalSave.EquipOne>();
            Dictionary<string, LocalSave.EquipOne>.Enumerator enumerator = this.list.GetEnumerator();
            while (enumerator.MoveNext())
            {
                KeyValuePair<string, LocalSave.EquipOne> current = enumerator.Current;
                LocalSave.EquipOne item = current.Value;
                if ((item.PropType == EquipType.eEquip) && (((item.WearIndex < 0) && !havewear) || havewear))
                {
                    list.Add(item);
                }
            }
            return list;
        }

        public int GetNewCount()
        {
            int num = 0;
            Dictionary<string, LocalSave.EquipOne>.Enumerator enumerator = this.list.GetEnumerator();
            while (enumerator.MoveNext())
            {
                KeyValuePair<string, LocalSave.EquipOne> current = enumerator.Current;
                if (current.Value.bNew)
                {
                    num++;
                }
            }
            return num;
        }

        public LocalSave.EquipOne GetPropByID(int equipid)
        {
            Dictionary<string, LocalSave.EquipOne>.Enumerator enumerator = this.list.GetEnumerator();
            while (enumerator.MoveNext())
            {
                KeyValuePair<string, LocalSave.EquipOne> current = enumerator.Current;
                if (current.Value.EquipID == equipid)
                {
                    KeyValuePair<string, LocalSave.EquipOne> pair2 = enumerator.Current;
                    return pair2.Value;
                }
            }
            return null;
        }

        public List<LocalSave.EquipOne> GetProps(EquipType type, bool havewear)
        {
            List<LocalSave.EquipOne> list = new List<LocalSave.EquipOne>();
            Dictionary<string, LocalSave.EquipOne>.Enumerator enumerator = this.list.GetEnumerator();
            while (enumerator.MoveNext())
            {
                KeyValuePair<string, LocalSave.EquipOne> current = enumerator.Current;
                LocalSave.EquipOne item = current.Value;
                if ((item.PropType == type) || (type == EquipType.eAll))
                {
                    if (item.PropType == EquipType.eEquip)
                    {
                        if (((item.WearIndex < 0) && !havewear) || havewear)
                        {
                            list.Add(item);
                        }
                    }
                    else if (item.Count > 0)
                    {
                        list.Add(item);
                    }
                }
            }
            return list;
        }

        public void Init()
        {
            this.bRefresh = true;
            if (this.wears.Count == 0)
            {
                for (int i = 0; i < 6; i++)
                {
                    this.wears.Add(null);
                }
                LocalSave.EquipOne one = new LocalSave.EquipOne {
                    EquipID = this.equipidd,
                    Level = 1,
                    Count = 1,
                    bNew = false
                };
                one.UniqueID = Utils.GenerateUUID();
                this.list.Add(one.UniqueID, one);
                this.EquipWear(one, 0);
            }
            else
            {
                this.bInitWear = true;
            }
            this.CheckWears();
            this.combine_refresh();
            this.Refresh_EquipExp_CanDrop();
            this.UpdateCallBack();
            Facade.Instance.SendNotification("MainUI_EquipRedCountUpdate");
        }

        public void Init(List<CEquipmentItem> equips)
        {
            if (equips != null)
            {
                LocalSave.EquipOne one;
                CEquipmentItem item;
                this.invalids.Clear();
                this.bRefresh = true;
                this.check_rowid_same();
                string uniqueid = null;
                int num = 0;
                int count = equips.Count;
                while (num < count)
                {
                    item = equips[num];
                    one = this.get_by_row_id(item.m_nRowID);
                    if (one != null)
                    {
                        one.EquipID = (int) item.m_nEquipID;
                        one.Count = (int) item.m_nFragment;
                        one.Level = (int) item.m_nLevel;
                    }
                    else
                    {
                        Equip_equip beanById = LocalModelManager.Instance.Equip_equip.GetBeanById((int) item.m_nEquipID);
                        if (beanById != null)
                        {
                            bool flag = true;
                            if (beanById.Overlying == 1)
                            {
                                LocalSave.EquipOne propByID = this.GetPropByID(beanById.Id);
                                if (propByID != null)
                                {
                                    propByID.RowID = item.m_nRowID;
                                    propByID.Count = (int) item.m_nFragment;
                                    flag = false;
                                }
                            }
                            if (flag)
                            {
                                one = new LocalSave.EquipOne {
                                    UniqueID = Utils.GenerateUUID(),
                                    RowID = item.m_nRowID,
                                    EquipID = (int) item.m_nEquipID,
                                    Level = (int) item.m_nLevel,
                                    Count = (int) item.m_nFragment,
                                    bNew = false
                                };
                                this.list.Add(one.UniqueID, one);
                            }
                        }
                    }
                    num++;
                }
                Dictionary<string, LocalSave.EquipOne>.Enumerator enumerator = this.list.GetEnumerator();
                bool flag2 = false;
                while (enumerator.MoveNext())
                {
                    flag2 = false;
                    KeyValuePair<string, LocalSave.EquipOne> current = enumerator.Current;
                    one = current.Value;
                    int num3 = 0;
                    int num4 = equips.Count;
                    while (num3 < num4)
                    {
                        item = equips[num3];
                        if (one.RowID == item.m_nRowID)
                        {
                            flag2 = true;
                            break;
                        }
                        num3++;
                    }
                    if (!flag2)
                    {
                        this.invalids.Add(one.UniqueID);
                    }
                    else if (one != null)
                    {
                        uniqueid = one.UniqueID;
                    }
                }
                if (this.wears.Count == 0)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        this.wears.Add(null);
                    }
                }
                bool flag3 = !string.IsNullOrEmpty(this.wears[0]);
                if (NetManager.mNetCache.IsEmpty)
                {
                    this.check_invalid_internal(true);
                }
                if (equips.Count == 1)
                {
                    this.check_invalid_internal(true);
                    if (flag3)
                    {
                        LocalSave.EquipOne equipByUniqueID = this.GetEquipByUniqueID(uniqueid);
                        if (equipByUniqueID != null)
                        {
                            int index = -1;
                            if (LocalSave.Instance.Equip_GetCanWearIndex(equipByUniqueID, out index))
                            {
                                this.EquipWear(equipByUniqueID, index);
                            }
                        }
                    }
                }
                if (!this.bInitWear)
                {
                    this.bInitWear = true;
                }
                else
                {
                    this.CheckWears();
                }
                this.combine_refresh();
                this.Refresh_EquipExp_CanDrop();
                base.Refresh();
                Facade.Instance.SendNotification("MainUI_EquipRedCountUpdate");
            }
        }

        public void init_equipone_uniqueid()
        {
            Dictionary<string, LocalSave.EquipOne>.Enumerator enumerator = this.list.GetEnumerator();
            while (enumerator.MoveNext())
            {
                KeyValuePair<string, LocalSave.EquipOne> current = enumerator.Current;
                KeyValuePair<string, LocalSave.EquipOne> pair2 = enumerator.Current;
                current.Value.UniqueID = pair2.Key;
            }
        }

        public bool IsEmpty() => 
            (this.list.Count == 0);

        protected override void OnRefresh()
        {
            this.bRefresh = true;
            FileUtils.WriteEquip<LocalSave.EquipData>("localequip.txt", this);
        }

        public void Refresh_EquipExp_CanDrop()
        {
            Dictionary<string, LocalSave.EquipOne>.Enumerator enumerator = this.list.GetEnumerator();
            while (enumerator.MoveNext())
            {
                KeyValuePair<string, LocalSave.EquipOne> current = enumerator.Current;
                int needMatID = current.Value.NeedMatID;
                this.Refresh_EquipExp_CanDrop(needMatID);
            }
        }

        public void Refresh_EquipExp_CanDrop(LocalSave.EquipOne one)
        {
            if (((one != null) && (one.data != null)) && (one.NeedMatID > 0))
            {
                this.Refresh_EquipExp_CanDrop(one.NeedMatID);
            }
        }

        public void Refresh_EquipExp_CanDrop(int equipexpid)
        {
            if ((equipexpid > 0) && !this.mEquipExpCanDropList.Contains(equipexpid))
            {
                this.mEquipExpCanDropList.Add(equipexpid);
            }
        }

        public void RemoveEquip(string uniqueid)
        {
            LocalSave.EquipOne one = null;
            if (this.list.TryGetValue(uniqueid, out one))
            {
                if (one != null)
                {
                    if (!one.Overlying)
                    {
                        this.EquipUnwear(uniqueid);
                        this.list.Remove(uniqueid);
                    }
                    else
                    {
                        one.Count = 0;
                    }
                }
                this.combine_refresh();
                base.Refresh();
            }
        }

        public void SetEquips(CEquipmentItem[] data)
        {
            int index = 0;
            int length = data.Length;
            while (index < length)
            {
                CEquipmentItem item = data[index];
                if (item != null)
                {
                    Equip_equip beanById = LocalModelManager.Instance.Equip_equip.GetBeanById((int) item.m_nEquipID);
                    if ((beanById != null) && (beanById.Overlying == 0))
                    {
                        LocalSave.EquipOne one = new LocalSave.EquipOne {
                            UniqueID = item.m_nUniqueID,
                            RowID = item.m_nRowID,
                            EquipID = (int) item.m_nEquipID,
                            Level = (int) item.m_nLevel,
                            Count = (int) item.m_nFragment
                        };
                        this.AddEquipInternal(one);
                    }
                }
                index++;
            }
            Facade.Instance.SendNotification("MainUI_EquipRedCountUpdate");
            this.combine_refresh();
            base.Refresh();
        }

        public void SetNew(string uniqueid)
        {
            if (this.list.TryGetValue(uniqueid, out LocalSave.EquipOne one))
            {
                one.bNew = false;
                base.Refresh();
                Facade.Instance.SendNotification("MainUI_EquipRedCountUpdate");
            }
        }

        public void SetWears(bool value)
        {
            this.bInitWear = value;
        }

        private void UpdateCallBack()
        {
            if (this.mUpdateAction != null)
            {
                this.mUpdateAction();
            }
        }

        public void UpdateEquip(LocalSave.EquipOne data)
        {
            if (this.list.ContainsKey(data.UniqueID))
            {
                this.list[data.UniqueID] = data;
                base.Refresh();
            }
        }

        [JsonIgnore]
        public bool wear_enable =>
            ((this.wears != null) && (this.wears.Count >= 6));
    }

    [Serializable]
    public class EquipOne
    {
        [JsonIgnore]
        public string UniqueID;
        public ulong RowID;
        public int EquipID;
        public int Level = 1;
        public int Count;
        public int WearIndex = -1;
        public bool bNew = true;
        [JsonIgnore]
        private int _qualityUpState = -1;
        [JsonIgnore]
        private Equip_equip _data;

        public void Clear()
        {
            this._data = null;
        }

        public void CombineReturn(List<Drop_DropModel.DropData> list)
        {
            if (this.Level > 1)
            {
                LocalSave.EquipOne propByID = LocalSave.Instance.GetPropByID(this.data.UpgradeNeed);
                if (propByID != null)
                {
                    for (int i = 1; i < this.Level; i++)
                    {
                        Drop_DropModel.DropData data2;
                        int upCoins = LocalModelManager.Instance.Equip_Upgrade.GetBeanById(i).UpCoins;
                        int upMaterials = LocalModelManager.Instance.Equip_Upgrade.GetBeanById(i).UpMaterials;
                        bool flag = false;
                        for (int j = 0; j < list.Count; j++)
                        {
                            Drop_DropModel.DropData data = list[j];
                            if ((data.type == PropType.eCurrency) && (data.id == 1))
                            {
                                data.count += upCoins;
                                flag = true;
                            }
                        }
                        if (!flag)
                        {
                            data2 = new Drop_DropModel.DropData {
                                type = PropType.eCurrency,
                                id = 1,
                                count = upCoins
                            };
                            list.Add(data2);
                        }
                        bool flag2 = false;
                        for (int k = 0; k < list.Count; k++)
                        {
                            Drop_DropModel.DropData data3 = list[k];
                            if ((data3.type == PropType.eEquip) && (data3.id == propByID.EquipID))
                            {
                                data3.count += upMaterials;
                                flag2 = true;
                            }
                        }
                        if (!flag2)
                        {
                            data2 = new Drop_DropModel.DropData {
                                type = PropType.eEquip,
                                id = propByID.EquipID,
                                count = upMaterials
                            };
                            list.Add(data2);
                        }
                    }
                }
            }
        }

        public void EquipWear(SelfAttributeData data)
        {
            List<Goods_goods.GoodData> equipAttributes = LocalModelManager.Instance.Equip_equip.GetEquipAttributes(this);
            int num = 0;
            int count = equipAttributes.Count;
            while (num < count)
            {
                equipAttributes[num].value = (long) (equipAttributes[num].value * (1f + data.GetUpPercent(this.Position)));
                data.attribute.Excute(equipAttributes[num]);
                num++;
            }
            List<string> equipAddAttributes = LocalModelManager.Instance.Equip_equip.GetEquipAddAttributes(this);
            int num3 = 0;
            int num4 = equipAddAttributes.Count;
            while (num3 < num4)
            {
                data.attribute.Excute(equipAddAttributes[num3]);
                num3++;
            }
        }

        public string GetAttName(int index)
        {
            if ((index < 0) || (index >= this.data.Attributes.Length))
            {
                return string.Empty;
            }
            Goods_goods.GoodData goodData = Goods_goods.GetGoodData(this.data.Attributes[index]);
            object[] args = new object[] { goodData.goodType };
            return GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("Attr_{0}", args), Array.Empty<object>());
        }

        public List<Goods_goods.GoodData> GetBabyAttributes()
        {
            List<Goods_goods.GoodData> list = new List<Goods_goods.GoodData>();
            int index = 0;
            int length = this.data.Attributes.Length;
            while (index < length)
            {
                string str = this.data.Attributes[index];
                if (str.Contains("EquipBaby:"))
                {
                    Goods_goods.GoodData goodData = Goods_goods.GetGoodData(str.Replace("EquipBaby:", string.Empty));
                    if (goodData.percent)
                    {
                        goodData.value += ((this.Level - 1) * this.data.AttributesUp[index]) * 100;
                    }
                    else
                    {
                        goodData.value += (this.Level - 1) * this.data.AttributesUp[index];
                    }
                    list.Add(goodData);
                }
                index++;
            }
            int num3 = 0;
            int num4 = this.data.AdditionSkills.Length;
            while (num3 < num4)
            {
                string s = this.data.AdditionSkills[num3];
                if (!int.TryParse(s, out _) && s.Contains("EquipBaby:"))
                {
                    Goods_goods.GoodData goodData = Goods_goods.GetGoodData(s.Replace("EquipBaby:", string.Empty));
                    list.Add(goodData);
                }
                num3++;
            }
            return list;
        }

        public List<int> GetBabySkills()
        {
            List<int> list = new List<int>();
            int index = 0;
            int length = this.data.AdditionSkills.Length;
            while (index < length)
            {
                string s = this.data.AdditionSkills[index];
                if (int.TryParse(s, out int num3))
                {
                    list.Add(num3);
                }
                index++;
            }
            return list;
        }

        public string GetCurrentAttributeString(int index)
        {
            if ((index < 0) || (index >= this.data.Attributes.Length))
            {
                return string.Empty;
            }
            Goods_goods.GoodData goodData = Goods_goods.GetGoodData(this.data.Attributes[index]);
            if (goodData.percent)
            {
                goodData.value += (this.data.AttributesUp[index] * (this.Level - 1)) * 0x2710;
                object[] objArray1 = new object[] { MathDxx.GetSymbolString(goodData.value), ((float) goodData.value) / 10000f };
                return Utils.FormatString("{0}{1}%", objArray1);
            }
            goodData.value += this.data.AttributesUp[index] * (this.Level - 1);
            object[] args = new object[] { MathDxx.GetSymbolString(goodData.value), goodData.value };
            return Utils.FormatString("{0}{1}", args);
        }

        public List<int> GetSkills() => 
            LocalModelManager.Instance.Equip_equip.GetSkills(this);

        public void QualityUp()
        {
            this.EquipID++;
            this._qualityUpState = -1;
            this._data = null;
        }

        public override string ToString()
        {
            string str = string.Empty;
            List<Goods_goods.GoodData> equipAttributes = LocalModelManager.Instance.Equip_equip.GetEquipAttributes(this);
            int num = 0;
            int count = equipAttributes.Count;
            while (num < count)
            {
                str = str + equipAttributes[num] + "|";
                num++;
            }
            object[] args = new object[] { this.UniqueID, this.EquipID, this.Level };
            return Utils.FormatString("UniqueID:{0} EquipID:{1} Level:{2}", args);
        }

        [JsonIgnore]
        public int Position =>
            this.data.Position;

        [JsonIgnore]
        public int Quality =>
            this.data.Quality;

        [JsonIgnore]
        public bool QualityCanUp
        {
            get
            {
                if (this._qualityUpState < 0)
                {
                    if (LocalModelManager.Instance.Equip_equip.GetBeanById(this.EquipID + 1) == null)
                    {
                        this._qualityUpState = 2;
                    }
                    else
                    {
                        this._qualityUpState = 1;
                    }
                }
                return (this._qualityUpState == 1);
            }
        }

        [JsonIgnore]
        public Color qualityColor =>
            LocalSave.QualityColors[this.Quality];

        [JsonIgnore]
        public int IdBase
        {
            get
            {
                int id = this.data.Id;
                if (!this.Overlying)
                {
                    id = ((this.data.Id / 100) * 100) + 1;
                }
                return id;
            }
        }

        [JsonIgnore]
        public int IconBase
        {
            get
            {
                int equipIcon = this.data.EquipIcon;
                if (!this.Overlying)
                {
                    equipIcon = ((this.data.EquipIcon / 100) * 100) + 1;
                }
                return equipIcon;
            }
        }

        [JsonIgnore]
        public bool IsWear =>
            (this.WearIndex >= 0);

        [JsonIgnore]
        public Equip_equip data
        {
            get
            {
                if ((this._data == null) || (this._data.Id != this.EquipID))
                {
                    this._data = LocalModelManager.Instance.Equip_equip.GetBeanById(this.EquipID);
                }
                return this._data;
            }
        }

        [JsonIgnore]
        public bool IsBaby =>
            (this.Position == 6);

        [JsonIgnore]
        public Sprite TypeIcon
        {
            get
            {
                if (this.Overlying)
                {
                    return null;
                }
                object[] args = new object[] { this.Position };
                return SpriteManager.GetCharUI(Utils.FormatString("equip_type_{0}", args));
            }
        }

        [JsonIgnore]
        public bool ShowQualityGoldImage
        {
            get
            {
                if (this.Overlying)
                {
                    return false;
                }
                return (this.Quality == 5);
            }
        }

        [JsonIgnore]
        public int CurrentMaxLevel =>
            this.data.MaxLevel;

        [JsonIgnore]
        public bool CanLevelUp =>
            ((!this.Overlying && !this.IsMax) && (this.HaveMatCount >= this.NeedMatCount));

        [JsonIgnore]
        public bool CanCombine =>
            LocalSave.Instance.Equip_can_combine(this);

        [JsonIgnore]
        public int NeedMatCount
        {
            get
            {
                Equip_Upgrade beanById = LocalModelManager.Instance.Equip_Upgrade.GetBeanById(this.Level);
                if (beanById != null)
                {
                    return beanById.UpMaterials;
                }
                object[] args = new object[] { this.Level };
                SdkManager.Bugly_Report("LocalSave_Equip", Utils.FormatString("NeedMatCount level:{0} is not in excel.", args));
                return 0x270f;
            }
        }

        [JsonIgnore]
        public int NeedMatID =>
            this.data.UpgradeNeed;

        [JsonIgnore]
        public string NeedMatUniqueID
        {
            get
            {
                LocalSave.EquipOne propByID = LocalSave.Instance.GetPropByID(this.data.UpgradeNeed);
                if (propByID != null)
                {
                    return propByID.UniqueID;
                }
                return null;
            }
        }

        [JsonIgnore]
        public int HaveMatCount
        {
            get
            {
                LocalSave.EquipOne propByID = LocalSave.Instance.GetPropByID(this.data.UpgradeNeed);
                if (propByID != null)
                {
                    return propByID.Count;
                }
                return 0;
            }
        }

        [JsonIgnore]
        public int BreakNeed =>
            this.data.BreakNeed;

        [JsonIgnore]
        public EquipType PropType
        {
            get
            {
                if (this.data == null)
                {
                    object[] args = new object[] { this.UniqueID, this.EquipID, this.Level };
                    Debug.Log(Utils.FormatString("UniqueID:{0} EquipID:{1} Level:{2}", args));
                }
                return (EquipType) this.data.PropType;
            }
        }

        [JsonIgnore]
        public bool Overlying =>
            (this.data?.Overlying != 0);

        [JsonIgnore]
        public int NeedGold
        {
            get
            {
                Equip_Upgrade beanById = LocalModelManager.Instance.Equip_Upgrade.GetBeanById(this.Level);
                if (beanById != null)
                {
                    return beanById.UpCoins;
                }
                object[] args = new object[] { this.Level };
                SdkManager.Bugly_Report("LocalSave_Equip", Utils.FormatString("NeedGold level:{0} is not in excel.", args));
                return 0;
            }
        }

        [JsonIgnore]
        public bool GoldEnough =>
            (LocalSave.Instance.GetGold() >= this.NeedGold);

        [JsonIgnore]
        public bool IsMax =>
            (this.Level >= this.CurrentMaxLevel);

        [JsonIgnore]
        public bool CountEnough =>
            (this.HaveMatCount >= this.NeedMatCount);

        [JsonIgnore]
        public string NameString
        {
            get
            {
                if (this.Overlying)
                {
                    return this.NameOnlyString;
                }
                object[] args = new object[] { this.IdBase };
                string languageByTID = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("装备名称{0}", args), Array.Empty<object>());
                object[] objArray2 = new object[] { this.QualityString, languageByTID };
                return Utils.FormatString("{0}\x00b7{1}", objArray2);
            }
        }

        [JsonIgnore]
        public string QualityString
        {
            get
            {
                if (this.Quality > 0)
                {
                    object[] args = new object[] { this.Quality };
                    return GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("equip_quality_{0}", args), Array.Empty<object>());
                }
                return string.Empty;
            }
        }

        [JsonIgnore]
        public string NameOnlyString
        {
            get
            {
                object[] args = new object[] { this.IdBase };
                return GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("装备名称{0}", args), Array.Empty<object>());
            }
        }

        [JsonIgnore]
        public string InfoString
        {
            get
            {
                object[] args = new object[] { this.IdBase };
                return GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("装备描述{0}", args), Array.Empty<object>());
            }
        }

        [JsonIgnore]
        public string SpecialInfoString =>
            GameLogic.Hold.Language.GetEquipSpecialInfo(this.IdBase);

        [JsonIgnore]
        public Sprite Icon =>
            SpriteManager.GetEquip(this.data.EquipIcon);
    }

    public enum EThreadWriteType
    {
        eBattle,
        eEquip,
        eNet,
        eLocal
    }

    [Serializable]
    public class FakeCardCost : LocalSaveBase
    {
        public int count;
        public int fakerid = 1;

        public void AddCount()
        {
            this.count++;
            Skill_slotoutcost beanById = LocalModelManager.Instance.Skill_slotoutcost.GetBeanById(this.fakerid);
            if (this.count >= beanById.LowerLimit)
            {
                this.fakerid++;
                beanById = LocalModelManager.Instance.Skill_slotoutcost.GetBeanById(this.fakerid);
            }
            base.Refresh();
        }

        public int GetCost() => 
            LocalModelManager.Instance.Skill_slotoutcost.GetBeanById(this.fakerid).CoinCost;

        public int GetNeedLevel() => 
            LocalModelManager.Instance.Skill_slotoutcost.GetBeanById(this.fakerid).NeedLevel;

        public void InitCount(int count)
        {
            this.count = count;
            this.fakerid = count + 1;
            base.Refresh();
            Facade.Instance.SendNotification("MainUI_CardRedCountUpdate");
        }

        protected override void OnRefresh()
        {
            FileUtils.WriteXml<LocalSave.FakeCardCost>("File_FakerCardDrop", this);
        }
    }

    [Serializable]
    public class FakeStageDrop : LocalSaveBase
    {
        public int stage = -1;
        public int count;
        public int fakerid = -1;

        public List<Drop_DropModel.DropData> GetDropList()
        {
            int chapterid = GameLogic.Hold.BattleData.Level_CurrentStage;
            int equipDropID = LocalModelManager.Instance.Stage_Level_stagechapter.GetEquipDropID(chapterid);
            if (equipDropID == 0)
            {
                return null;
            }
            this.UpdateStage(chapterid, equipDropID);
            Drop_FakeDrop beanById = LocalModelManager.Instance.Drop_FakeDrop.GetBeanById(this.fakerid);
            this.count++;
            if (this.count >= beanById.RandNum)
            {
                this.count = 0;
                this.fakerid = beanById.JumpDrop;
            }
            base.Refresh();
            return LocalModelManager.Instance.Drop_Drop.GetDropList(beanById.DropID);
        }

        protected override void OnRefresh()
        {
            FileUtils.WriteXml<LocalSave.FakeStageDrop>("File_FakerStageDrop", this);
        }

        public void UpdateStage(int stage, int fakerid)
        {
            if (this.stage != stage)
            {
                this.stage = stage;
                this.count = 0;
                this.fakerid = fakerid;
                base.Refresh();
            }
        }
    }

    public class GuideData : LocalSaveBase
    {
        public const ushort GAME_SYSTEM_DIAMONDBOX = 1;
        public int mDiamondBox;
        [JsonIgnore]
        private GuideNoMaskCtrl mCtrl;
        public long mGameSystemMask;
        [CompilerGenerated]
        private static Action<NetResponse> <>f__am$cache0;

        public void check_diamondbox_first_open()
        {
            if (!this.is_system_open(1))
            {
                Debug.Log("发送给服务器 钻石宝箱系统开启");
                this.system_open(1);
                LocalSave.Instance.SetTimeBoxTime(LocalSave.TimeBoxType.BoxChoose_DiamondNormal, Utils.GetTimeStamp());
                LocalSave.Instance.SetTimeBoxTime(LocalSave.TimeBoxType.BoxChoose_DiamondLarge, Utils.GetTimeStamp());
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = delegate (NetResponse response) {
                    };
                }
                this.send_system_open(1, <>f__am$cache0);
            }
        }

        public bool CheckDiamondBox(RectTransform t, int index)
        {
            if (this.mDiamondBox == index)
            {
                if ((index == 0) && (LocalSave.Instance.GetDiamondBoxFreeCount(LocalSave.TimeBoxType.BoxChoose_DiamondNormal) > 0))
                {
                    this.create_mask(t);
                    return true;
                }
                if ((index == 1) && (LocalSave.Instance.GetDiamondBoxFreeCount(LocalSave.TimeBoxType.BoxChoose_DiamondNormal) > 0))
                {
                    this.create_mask(t);
                    return true;
                }
            }
            return false;
        }

        private void create_mask(RectTransform t)
        {
            this.remove();
            this.mCtrl = CInstance<UIResourceCreator>.Instance.GetGuideNoMask(t);
        }

        public bool is_system_open(int index)
        {
            string str = Convert.ToString(this.mGameSystemMask, 2);
            Debugger.Log("is_system_open " + str);
            int length = str.Length;
            if ((!string.IsNullOrEmpty(str) && (length > index)) && (index >= 0))
            {
                int result = 0;
                int.TryParse(str.Substring((length - 1) - index, 1), out result);
                return (result != 0);
            }
            return false;
        }

        protected override void OnRefresh()
        {
            FileUtils.WriteXml<LocalSave.GuideData>("File_Achieve1", this);
        }

        private void remove()
        {
            if (this.mCtrl != null)
            {
                Object.Destroy(this.mCtrl.gameObject);
            }
        }

        private void send_system_open(ushort index, Action<NetResponse> callback)
        {
            <send_system_open>c__AnonStorey0 storey = new <send_system_open>c__AnonStorey0 {
                callback = callback
            };
            CReqSyncGameSystemMask packet = new CReqSyncGameSystemMask {
                m_syncMsg = new CCommonRespMsg()
            };
            packet.m_syncMsg.m_nStatusCode = index;
            packet.m_syncMsg.m_strInfo = string.Empty;
            NetManager.SendInternal<CReqSyncGameSystemMask>(packet, SendType.eCache, new Action<NetResponse>(storey.<>m__0));
        }

        public void SetGameSystemMask(long mask)
        {
            this.mGameSystemMask = mask;
            base.Refresh();
        }

        public void SetIndex(int index)
        {
            if (this.mDiamondBox == (index - 1))
            {
                this.mDiamondBox = index;
                this.remove();
                base.Refresh();
            }
        }

        public void system_open(int index)
        {
            this.mGameSystemMask += ((int) 1) << index;
            base.Refresh();
        }

        [CompilerGenerated]
        private sealed class <send_system_open>c__AnonStorey0
        {
            internal Action<NetResponse> callback;

            internal void <>m__0(NetResponse response)
            {
                if (this.callback != null)
                {
                    this.callback(response);
                }
            }
        }
    }

    [Serializable]
    public class HarvestData : LocalSaveBase
    {
        public long beforeexcutetime = -1L;
        public long startservertime;
        public int gold;
        public Dictionary<int, Drop_DropModel.DropData> mItems = new Dictionary<int, Drop_DropModel.DropData>();
        [JsonIgnore]
        private WeightRandom mEquipExpWeight = new WeightRandom();
        [JsonIgnore]
        private long mMaxTime;

        public void AddGold(int gold)
        {
            this.gold += gold;
            base.Refresh();
        }

        public void AddItem(Drop_DropModel.DropData item)
        {
            if (this.mItems.TryGetValue(item.id, out Drop_DropModel.DropData data))
            {
                data.count += item.count;
            }
            else
            {
                this.mItems.Add(item.id, item);
            }
            base.Refresh();
        }

        public void Clear()
        {
            this.gold = 0;
            this.startservertime = Utils.GetTimeStamp();
            this.mItems.Clear();
            base.Refresh();
        }

        public bool get_can_reward()
        {
            if (!this.is_available())
            {
                return false;
            }
            int num2 = (int) (this.get_harvest_time() / 60L);
            return (num2 >= 60);
        }

        private int get_current_refresh_minutes()
        {
            if (!this.is_available())
            {
                return 0;
            }
            long timeStamp = Utils.GetTimeStamp();
            if ((timeStamp - this.startservertime) > this.mMaxTime)
            {
                timeStamp = this.startservertime + this.mMaxTime;
            }
            long num2 = timeStamp - this.beforeexcutetime;
            int num3 = (int) (num2 / 60L);
            this.beforeexcutetime += num3 * 60;
            return num3;
        }

        public long get_harvest_time() => 
            (Utils.GetTimeStamp() - this.startservertime);

        public void Get_to_pack()
        {
            if (this.gold > 0)
            {
                LocalSave.Instance.Modify_Gold((long) this.gold, false);
                CurrencyFlyCtrl.PlayGet(CurrencyType.Gold, (long) this.gold, null, null, true);
            }
            Dictionary<int, Drop_DropModel.DropData>.Enumerator enumerator = this.mItems.GetEnumerator();
            while (enumerator.MoveNext())
            {
                KeyValuePair<int, Drop_DropModel.DropData> current = enumerator.Current;
                if (current.Value.count > 0)
                {
                    KeyValuePair<int, Drop_DropModel.DropData> pair2 = enumerator.Current;
                    LocalSave.Instance.AddProp(pair2.Value);
                }
            }
            this.Clear();
            Facade.Instance.SendNotification("MainUI_HarvestUpdate");
        }

        public List<Drop_DropModel.DropData> GetList()
        {
            List<Drop_DropModel.DropData> list = new List<Drop_DropModel.DropData>();
            if (this.gold > 0)
            {
                Drop_DropModel.DropData item = new Drop_DropModel.DropData {
                    type = PropType.eCurrency,
                    id = 1,
                    count = this.gold
                };
                list.Add(item);
            }
            Dictionary<int, Drop_DropModel.DropData>.Enumerator enumerator = this.mItems.GetEnumerator();
            while (enumerator.MoveNext())
            {
                KeyValuePair<int, Drop_DropModel.DropData> current = enumerator.Current;
                if (current.Value.count > 0)
                {
                    KeyValuePair<int, Drop_DropModel.DropData> pair2 = enumerator.Current;
                    list.Add(pair2.Value);
                }
            }
            return list;
        }

        public void Init()
        {
            this.mEquipExpWeight.Add(0x7595, 10);
            this.mEquipExpWeight.Add(0x7596, 10);
            this.mEquipExpWeight.Add(0x7597, 0x11);
            this.mEquipExpWeight.Add(0x7598, 0x11);
            this.mMaxTime = 0x15180 * GameConfig.GetHarvestMaxDay();
        }

        public void init_last_time(long time)
        {
            this.startservertime = time;
            if (this.beforeexcutetime < 0L)
            {
                this.beforeexcutetime = this.startservertime;
            }
            base.Refresh();
        }

        private bool is_available() => 
            (((this.startservertime > 0L) && (this.beforeexcutetime > 0L)) && (NetManager.NetTime > 0L));

        protected override void OnRefresh()
        {
            FileUtils.WriteXml<LocalSave.HarvestData>("File_Harvest", this);
        }

        public bool refresh_rewards()
        {
            if (NetManager.NetTime == 0L)
            {
                return false;
            }
            int key = LocalSave.Instance.Card_GetHarvestLevel();
            if (key == 0)
            {
                return false;
            }
            int num2 = this.get_current_refresh_minutes();
            if (num2 <= 0)
            {
                return false;
            }
            Drop_harvest beanById = LocalModelManager.Instance.Drop_harvest.GetBeanById(key);
            if (beanById == null)
            {
                object[] args = new object[] { key };
                SdkManager.Bugly_Report("LocalSave_Harvest", Utils.FormatString("refresh_rewards Drop_harvest ID:{0} is not in excel.", args));
                return false;
            }
            float num3 = 1f;
            this.gold += MathDxx.CeilToInt((beanById.GoldDrop * num2) * num3);
            Dictionary<int, bool> dictionary = new Dictionary<int, bool>();
            for (int i = 0; i < num2; i++)
            {
                if (GameLogic.Random(0, 0x2710) < beanById.EquipExp)
                {
                    int random = this.mEquipExpWeight.GetRandom();
                    bool flag = false;
                    if (!dictionary.TryGetValue(random, out flag))
                    {
                        flag = LocalSave.Instance.Equip_can_drop_equipexp(random);
                        dictionary.Add(random, flag);
                    }
                    if (flag)
                    {
                        Drop_DropModel.DropData item = new Drop_DropModel.DropData {
                            type = PropType.eEquip,
                            id = random,
                            count = 1
                        };
                        this.AddItem(item);
                    }
                }
            }
            return true;
        }

        public void Unlock()
        {
            this.Clear();
            this.beforeexcutetime = Utils.GetTimeStamp();
            this.startservertime = this.beforeexcutetime;
            Facade.Instance.SendNotification("MainUI_HarvestUpdate");
        }
    }

    [Serializable]
    public class LocalMail : LocalSaveBase
    {
        public uint mLastMailID;
        public List<CMailInfo> list = new List<CMailInfo>();
        private float time;
        private const int interval = 600;
        [CompilerGenerated]
        private static Comparison<CMailInfo> <>f__am$cache0;

        public void AddMail(CMailInfo mail)
        {
            int num = 0;
            int count = this.list.Count;
            while (num < count)
            {
                if (this.list[num].m_nMailID == mail.m_nMailID)
                {
                    return;
                }
                num++;
            }
            this.list.Add(mail);
            this.SetMailID(mail.m_nMailID);
            this.mailListUpdate();
        }

        public bool CheckMainPop()
        {
            if (Facade.Instance.RetrieveMediator("MailInfoMediator") == null)
            {
                int num = 0;
                int count = this.list.Count;
                while (num < count)
                {
                    CMailInfo info = this.list[num];
                    if ((info.m_nMailType == 3) && !info.IsReaded)
                    {
                        MailInfoProxy.Transfer data = new MailInfoProxy.Transfer {
                            data = info,
                            poptype = MailInfoProxy.EMailPopType.eMain
                        };
                        Facade.Instance.RegisterProxy(new MailInfoProxy(data));
                        WindowUI.ShowWindow(WindowID.WindowID_MailInfo);
                        return true;
                    }
                    num++;
                }
            }
            return false;
        }

        public int GetRedCount()
        {
            int num = 0;
            int num2 = 0;
            int count = this.list.Count;
            while (num2 < count)
            {
                if (this.list[num2].IsShowRed)
                {
                    num++;
                }
                num2++;
            }
            return num;
        }

        public void Init()
        {
            this.time = Time.realtimeSinceStartup;
            TweenSettingsExtensions.SetLoops<Sequence>(TweenSettingsExtensions.SetUpdate<Sequence>(TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(DOTween.Sequence(), 1f), new TweenCallback(this, this.Update)), true), -1);
        }

        public void MailGot(CMailInfo mail)
        {
            if (!mail.IsGot)
            {
                mail.IsGot = true;
                this.mailListUpdate();
                base.Refresh();
            }
        }

        private void mailListUpdate()
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = delegate (CMailInfo a, CMailInfo b) {
                    if (a.IsShowRed && !b.IsShowRed)
                    {
                        return -1;
                    }
                    if ((a.IsShowRed || !b.IsShowRed) && (a.m_i64PubTime > b.m_i64PubTime))
                    {
                        return -1;
                    }
                    return 1;
                };
            }
            this.list.Sort(<>f__am$cache0);
            Facade.Instance.SendNotification("MailUI_MailUpdate");
            Facade.Instance.SendNotification("MainUI_MailUpdate");
        }

        public void MailReaded(CMailInfo mail)
        {
            if (!mail.IsReaded)
            {
                mail.IsReaded = true;
                this.mailListUpdate();
                base.Refresh();
            }
        }

        protected override void OnRefresh()
        {
            FileUtils.WriteXml<LocalSave.LocalMail>("mail.txt", this);
        }

        public void SendMail()
        {
            this.SendMailInternal(null);
        }

        private void SendMailInternal(Action callback)
        {
            <SendMailInternal>c__AnonStorey0 storey = new <SendMailInternal>c__AnonStorey0 {
                callback = callback,
                $this = this
            };
            CReqAnnonceMailList packet = new CReqAnnonceMailList {
                m_nLastMailID = this.mLastMailID
            };
            NetManager.SendInternal<CReqAnnonceMailList>(packet, SendType.eLoop, new Action<NetResponse>(storey.<>m__0));
        }

        public void SetMailID(uint id)
        {
            if (this.mLastMailID < id)
            {
                this.mLastMailID = id;
            }
        }

        private void Update()
        {
            if ((Time.realtimeSinceStartup - this.time) > 600f)
            {
                this.time = Time.realtimeSinceStartup;
                this.SendMailInternal(() => this.time = Time.realtimeSinceStartup - 570f);
            }
        }

        [CompilerGenerated]
        private sealed class <SendMailInternal>c__AnonStorey0
        {
            internal Action callback;
            internal LocalSave.LocalMail $this;

            internal void <>m__0(NetResponse response)
            {
                if (response.data != null)
                {
                    CRespMailList data = response.data as CRespMailList;
                    if ((data != null) && (data.mailList != null))
                    {
                        int index = 0;
                        int length = data.mailList.Length;
                        while (index < length)
                        {
                            this.$this.AddMail(data.mailList[index]);
                            index++;
                        }
                        Facade.Instance.SendNotification("MainUI_MailUpdate");
                        this.$this.Refresh();
                    }
                }
                else if (((response.error == null) || (response.error.m_nStatusCode != 2)) && (this.callback != null))
                {
                    this.callback();
                }
            }
        }
    }

    [Serializable]
    public class LocalSaveExtra : LocalSaveBase
    {
        public int stage = 1;
        public Dictionary<int, int> list = new Dictionary<int, int>();
        public int overopencount;
        public int battleinmode = 0x3e9;
        public int guideequipalllayer;
        public int guidebattleProcess;
        public long EquipDropRate;
        public uint mTransID;

        public void AddEquipAllLayer()
        {
            this.guideequipalllayer++;
            base.Refresh();
        }

        public void AddLayerCount(int stage, int layer)
        {
            this.InitData(stage, layer);
            if (this.list[layer] == 0)
            {
                Dictionary<int, int> dictionary;
                int num;
                (dictionary = this.list)[num = layer] = dictionary[num] + 1;
                base.Refresh();
            }
        }

        public bool Get_Equip_Drop() => 
            ((((LocalSave.Instance.Card_GetLevel() >= GameConfig.GetEquipUnlockTalentLevel()) || (LocalSave.Instance.GetHaveEquips(true).Count > 1)) || (LocalSave.Instance.Stage_GetStage() > 1)) || (this.guideequipalllayer >= GameConfig.GetEquipGuide_alllayer()));

        public bool Get_EquipExp_Drop() => 
            (this.Get_Equip_Drop() && (LocalSave.Instance.Card_GetLevel() >= GameConfig.GetEquipExpUnlockTalentLevel()));

        public int GetLayerCount(int stage, int layer)
        {
            this.InitData(stage, layer);
            return this.list[layer];
        }

        public uint GetTransID()
        {
            this.mTransID++;
            base.Refresh();
            return this.mTransID;
        }

        public void Init()
        {
            if (LocalSave.Instance.mStage.MaxLevel > 0)
            {
                this.SetGuideBattleProcess(2);
            }
        }

        private void InitData(int stage, int layer)
        {
            if (this.stage != stage)
            {
                this.list.Clear();
            }
            if (!this.list.ContainsKey(layer))
            {
                this.list.Add(layer, 0);
            }
        }

        public void InitTransID(uint id)
        {
            if (this.mTransID < id)
            {
                this.mTransID = id;
                base.Refresh();
            }
        }

        protected override void OnRefresh()
        {
            FileUtils.WriteXml<LocalSave.LocalSaveExtra>("File_Extra", this);
        }

        public void SetEquipDropRate(long value)
        {
            this.EquipDropRate = value;
            base.Refresh();
        }

        public void SetGuideBattleProcess(int value)
        {
            this.guidebattleProcess = value;
            base.Refresh();
        }
    }

    [Serializable]
    public class NetCache
    {
        public List<LocalSave.NetCacheBase> list = new List<LocalSave.NetCacheBase>();

        public void SendAllCache()
        {
            int num = 0;
            int count = this.list.Count;
            while (num < count)
            {
                this.SendOne(this.list[num]);
                num++;
            }
        }

        public void SendOne(LocalSave.NetCacheBase data)
        {
            switch (data.type)
            {
                case LocalSave.NetCacheType.eBattleBegin:
                    if (data is LocalSave.NetCacheBattleBegin)
                    {
                    }
                    break;

                case LocalSave.NetCacheType.eBattleEnd:
                    if (data is LocalSave.NetCacheBattleEnd)
                    {
                    }
                    break;
            }
        }
    }

    [Serializable]
    public class NetCacheBase
    {
        public LocalSave.NetCacheType type;
    }

    [Serializable]
    public class NetCacheBattleBegin : LocalSave.NetCacheBase
    {
    }

    [Serializable]
    public class NetCacheBattleEnd : LocalSave.NetCacheBase
    {
    }

    public enum NetCacheType
    {
        eBattleBegin,
        eBattleEnd
    }

    [Serializable]
    public class PurchaseData : LocalSaveBase
    {
        public List<string> mList = new List<string>();

        public void AddPurchase(string data)
        {
            data = data.ToLower();
            this.mList.Add(data);
            base.Refresh();
        }

        protected override void OnRefresh()
        {
            FileUtils.WriteXml<LocalSave.PurchaseData>(string.Empty, this);
        }

        public void RemovePurchase(string actionid)
        {
            actionid = actionid.ToLower();
            int index = 0;
            int count = this.mList.Count;
            while (index < count)
            {
                JObject obj2 = (JObject) JsonConvert.DeserializeObject(this.mList[index]);
                if (actionid.Equals(obj2.get_Item("transactionid")))
                {
                    this.mList.RemoveAt(index);
                    base.Refresh();
                    break;
                }
                index++;
            }
        }
    }

    [Serializable]
    public class SaveData
    {
        public bool bInit;
        public LocalSave.UserInfo userInfo = new LocalSave.UserInfo();
        public LocalSave.CardData mCardData = new LocalSave.CardData();
        public LocalSave.ChallengeData mChallengeData = new LocalSave.ChallengeData();
        public LocalSave.TimeBoxData mTimeBoxData = new LocalSave.TimeBoxData();
        public LocalSave.Stage mStage = new LocalSave.Stage();
        public LocalSave.AchieveData mAchieveData = new LocalSave.AchieveData();
        public Shop_MysticShopModel.MysticShopData mMysticShopData = new Shop_MysticShopModel.MysticShopData();
        public LocalSave.LocalSaveExtra mExtra = new LocalSave.LocalSaveExtra();
        public LocalSave.FakeStageDrop mFakeStage = new LocalSave.FakeStageDrop();
        public LocalSave.FakeCardCost mFakeCardCost = new LocalSave.FakeCardCost();
        public LocalSave.ShopLocal mShopLocal = new LocalSave.ShopLocal();
        public LocalSave.ActiveData mActiveData = new LocalSave.ActiveData();
        public LocalSave.LocalMail mMail = new LocalSave.LocalMail();
        public LocalSave.DropCard mDropCard = new LocalSave.DropCard();
        public LocalSave.PurchaseData mPurchase = new LocalSave.PurchaseData();
        public LocalSave.HarvestData mHarvest = new LocalSave.HarvestData();
        public LocalSave.GuideData mGuideData = new LocalSave.GuideData();
    }

    [Serializable]
    public class ShopLocal : LocalSaveBase
    {
        [JsonIgnore]
        public bool bRefresh = true;
        [JsonIgnore]
        private static int[] mTimes = new int[] { 8, 0x18, 0x48 };

        public int get_buy_golds(int index)
        {
            if ((index < 0) || (index >= mTimes.Length))
            {
                return 0;
            }
            return ((LocalSave.Instance.Card_GetHarvestGold() * 60) * mTimes[index]);
        }

        public int get_gold_time(int index)
        {
            if ((index >= 0) && (index < mTimes.Length))
            {
                return mTimes[index];
            }
            return 0;
        }

        public void Init()
        {
            this.bRefresh = true;
        }

        protected override void OnRefresh()
        {
            this.bRefresh = true;
        }
    }

    [Serializable]
    public class Stage : LocalSaveBase
    {
        public int CurrentStage;
        public bool FirstIn;
        public int MaxLevel;
        public int BoxLayerID;
        public bool bRefresh;
        [JsonIgnore]
        public bool bNewBestLevel;

        public int GetCurrentMaxLevel()
        {
            this.GetStageLayer(this.MaxLevel, out _, out int num2);
            return num2;
        }

        public void GetLayerBoxStageLayer(int currentlayer, out int stage, out int layer)
        {
            int maxChapter = LocalModelManager.Instance.Stage_Level_stagechapter.GetMaxChapter();
            stage = maxChapter;
            layer = LocalModelManager.Instance.Stage_Level_stagechapter.GetCurrentMaxLevel(maxChapter);
            for (int i = 1; i <= maxChapter; i++)
            {
                int allMaxLevel = LocalModelManager.Instance.Stage_Level_stagechapter.GetAllMaxLevel(i);
                if (currentlayer <= allMaxLevel)
                {
                    if (i == 1)
                    {
                        stage = i;
                        layer = currentlayer;
                        return;
                    }
                    stage = i;
                    layer = currentlayer - LocalModelManager.Instance.Stage_Level_stagechapter.GetAllMaxLevel(i - 1);
                    return;
                }
            }
        }

        public void GetStageBoxEnd()
        {
            this.BoxLayerID++;
            base.Refresh();
        }

        public void GetStageLayer(int currentlayer, out int stage, out int layer)
        {
            int maxChapter = LocalModelManager.Instance.Stage_Level_stagechapter.GetMaxChapter();
            stage = maxChapter;
            layer = LocalModelManager.Instance.Stage_Level_stagechapter.GetCurrentMaxLevel(maxChapter);
            for (int i = 1; i <= maxChapter; i++)
            {
                int allMaxLevel = LocalModelManager.Instance.Stage_Level_stagechapter.GetAllMaxLevel(i);
                if (currentlayer < allMaxLevel)
                {
                    if (i == 1)
                    {
                        stage = i;
                        layer = currentlayer;
                        return;
                    }
                    stage = i;
                    layer = currentlayer - LocalModelManager.Instance.Stage_Level_stagechapter.GetAllMaxLevel(i - 1);
                    return;
                }
            }
        }

        public void InitMaxLevel(int max)
        {
            this.bRefresh = true;
            this.MaxLevel = max;
            int chapterId = 1;
            int maxChapter = LocalModelManager.Instance.Stage_Level_stagechapter.GetMaxChapter();
            int allMaxLevel = LocalModelManager.Instance.Stage_Level_stagechapter.GetAllMaxLevel(chapterId);
            while ((this.MaxLevel >= allMaxLevel) && (chapterId < maxChapter))
            {
                chapterId++;
                allMaxLevel = LocalModelManager.Instance.Stage_Level_stagechapter.GetAllMaxLevel(chapterId);
            }
            this.CurrentStage = chapterId;
            LocalSave.Instance.Stage_SetStage(this.CurrentStage);
            GameLogic.Hold.BattleData.InitState();
            Debugger.Log(string.Concat(new object[] { "stage ", chapterId, " stagemax ", allMaxLevel, " currentmax ", this.MaxLevel, " current show ", GameLogic.Hold.BattleData.Level_CurrentStage }));
            base.Refresh();
            Facade.Instance.SendNotification("MainUI_LayerUpdate");
        }

        protected override void OnRefresh()
        {
            this.bRefresh = true;
            FileUtils.WriteXml<LocalSave.Stage>("File_Stage", this);
        }

        public override string ToString()
        {
            object[] args = new object[] { this.CurrentStage, this.FirstIn, this.MaxLevel };
            return Utils.FormatString("Stage:{0}, FirstIn:{1}, MaxLevel:{2}", args);
        }

        public void UpdateMaxLevel(int max)
        {
            if (this.MaxLevel < max)
            {
                this.MaxLevel = max;
                this.InitMaxLevel(this.MaxLevel);
                this.bNewBestLevel = true;
                base.Refresh();
            }
        }
    }

    [Serializable]
    public class StageDiscountBody
    {
        public LocalSave.StageDiscountInfo purchased_info;
        public LocalSave.StageDiscountCurrent current_purchase;

        public int Get_CurrentID()
        {
            int result = 0;
            if (((this.current_purchase != null) && !string.IsNullOrEmpty(this.current_purchase.product_id)) && (this.current_purchase.product_id.Length > 3))
            {
                int.TryParse(this.current_purchase.product_id.Substring(this.current_purchase.product_id.Length - 3, 3), out result);
            }
            return result;
        }

        public int Get_LastID()
        {
            int result = 0;
            if (((this.purchased_info != null) && !string.IsNullOrEmpty(this.purchased_info.product_id)) && (this.purchased_info.product_id.Length > 3))
            {
                int.TryParse(this.purchased_info.product_id.Substring(this.purchased_info.product_id.Length - 3, 3), out result);
            }
            return result;
        }

        public List<Drop_DropModel.DropData> GetList()
        {
            List<Drop_DropModel.DropData> list = new List<Drop_DropModel.DropData>();
            if (this.IsValid)
            {
                if (((this.current_purchase == null) || (this.current_purchase.reward_info == null)) || (this.current_purchase.reward_info.Length == 0))
                {
                    return list;
                }
                int index = 0;
                int length = this.current_purchase.reward_info.Length;
                while (index < length)
                {
                    string str = this.current_purchase.reward_info[index];
                    if (!string.IsNullOrEmpty(str))
                    {
                        char[] separator = new char[] { ',' };
                        string[] strArray = str.Split(separator);
                        if (strArray.Length == 3)
                        {
                            int result = 0;
                            int.TryParse(strArray[0], out result);
                            int num4 = 0;
                            int.TryParse(strArray[1], out num4);
                            int num5 = 0;
                            int.TryParse(strArray[2], out num5);
                            if (((result != 0) && (num4 != 0)) && (num5 != 0))
                            {
                                Drop_DropModel.DropData item = new Drop_DropModel.DropData {
                                    type = (PropType) result,
                                    id = num4,
                                    count = num5
                                };
                                list.Add(item);
                            }
                        }
                    }
                    index++;
                }
            }
            return list;
        }

        public override string ToString()
        {
            string str = string.Empty;
            if (this.purchased_info != null)
            {
                str = this.purchased_info.product_id;
            }
            string str2 = string.Empty;
            string str3 = string.Empty;
            if (this.current_purchase != null)
            {
                str2 = this.current_purchase.product_id;
                if (this.current_purchase.reward_info != null)
                {
                    for (int i = 0; i < this.current_purchase.reward_info.Length; i++)
                    {
                        if (i != (this.current_purchase.reward_info.Length - 1))
                        {
                            str3 = str3 + this.current_purchase.reward_info[i] + ",";
                        }
                        else
                        {
                            str3 = str3 + this.current_purchase.reward_info[i];
                        }
                    }
                }
            }
            object[] args = new object[] { str, str2, str3 };
            return Utils.FormatString("StageDiscount: boughtid:{0} nextid:{1} next rewards:{2}", args);
        }

        public bool IsValid
        {
            get
            {
                if (this.current_purchase == null)
                {
                    return false;
                }
                if (this.current_purchase.reward_info == null)
                {
                    return false;
                }
                if (string.IsNullOrEmpty(this.current_purchase.product_id))
                {
                    return false;
                }
                return (this.current_purchase.reward_info.Length > 0);
            }
        }

        public bool Is_Ad_Free =>
            (this.Get_LastID() > 0);
    }

    [Serializable]
    public class StageDiscountCurrent
    {
        public string product_id;
        public string[] reward_info;
    }

    [Serializable]
    public class StageDiscountInfo
    {
        public string product_id;
    }

    [Serializable]
    public class TimeBoxData : LocalSaveBase
    {
        public Dictionary<LocalSave.TimeBoxType, LocalSave.TimeBoxOne> list = new Dictionary<LocalSave.TimeBoxType, LocalSave.TimeBoxOne>();

        public int GetCount(LocalSave.TimeBoxType type) => 
            this.list[type].count;

        public long GetTime(LocalSave.TimeBoxType type) => 
            this.list[type].time;

        public void Init()
        {
            LocalSave.TimeBoxOne one;
            long num = 0x7fffffffffffffffL;
            if (!this.list.ContainsKey(LocalSave.TimeBoxType.BoxChoose_DiamondLarge))
            {
                one = new LocalSave.TimeBoxOne {
                    maxcount = 1,
                    count = 0,
                    time = num
                };
                this.list.Add(LocalSave.TimeBoxType.BoxChoose_DiamondLarge, one);
                base.Refresh();
            }
            if (!this.list.ContainsKey(LocalSave.TimeBoxType.BoxChoose_DiamondNormal))
            {
                one = new LocalSave.TimeBoxOne {
                    maxcount = 1,
                    count = 0,
                    time = num
                };
                this.list.Add(LocalSave.TimeBoxType.BoxChoose_DiamondNormal, one);
                base.Refresh();
            }
        }

        public bool IsBoxOpenFree(LocalSave.TimeBoxType type)
        {
            if (this.list[type].maxcount != 0)
            {
                int id = ((int) type) + 10;
                long num2 = GameConfig.GetValue<long>(id);
                long timeStamp = Utils.GetTimeStamp();
                if ((timeStamp - num2) >= this.list[type].time)
                {
                    this.SetTime(type, timeStamp);
                    return true;
                }
            }
            return false;
        }

        public bool IsMaxCount(LocalSave.TimeBoxType type) => 
            this.list[type].IsMax;

        protected override void OnRefresh()
        {
            FileUtils.WriteXml<LocalSave.TimeBoxData>("File_TimeBox", this);
        }

        public void SetCount(LocalSave.TimeBoxType type, int value)
        {
            this.list[type].SetCount(value);
            this.UpdateRedNode(type);
            base.Refresh();
        }

        public void SetTime(LocalSave.TimeBoxType type, long time)
        {
            this.list[type].time = time;
            Debugger.Log(string.Concat(new object[] { "SetTime ", type, " time -> ", time }));
            base.Refresh();
        }

        public void UpdateCount(LocalSave.TimeBoxType type, int value, bool over)
        {
            this.list[type].UpdateCount(value, over);
            this.UpdateRedNode(type);
            base.Refresh();
        }

        private void UpdateRedNode(LocalSave.TimeBoxType type)
        {
            if ((type == LocalSave.TimeBoxType.BoxChoose_DiamondNormal) || (type == LocalSave.TimeBoxType.BoxChoose_DiamondLarge))
            {
                Facade.Instance.SendNotification("MainUI_ShopRedCountUpdate");
            }
        }
    }

    [Serializable]
    public class TimeBoxOne
    {
        public int maxcount = 1;
        public int count = 1;
        public long time;

        public void SetCount(int value)
        {
            this.count = value;
        }

        public void UpdateCount(int value, bool over)
        {
            this.count += value;
            if (!over && (value > 0))
            {
                this.count = MathDxx.Clamp(this.count, 0, this.maxcount);
            }
        }

        [JsonIgnore]
        public bool IsMax =>
            (this.count >= this.maxcount);
    }

    public enum TimeBoxType
    {
        BoxChoose_DiamondLarge = 0x3ff,
        BoxChoose_DiamondNormal = 0x402
    }

    [Serializable]
    public class UserInfo : LocalSaveBase
    {
        public string UserID = string.Empty;
        [JsonIgnore]
        public string UserID_Temp = string.Empty;
        public string UserName = string.Empty;
        public string UserName_Temp = string.Empty;
        public ulong ServerUserID;
        public LoginType loginType;
        public long NetID;
        public long Gold;
        public long Diamond;
        public long Resource;
        public int Key;
        public int reborncount;
        public int StageDiscountID;
        public int Level = 1;
        public long Exp;
        public int Score;
        public bool isInit;
        public long Show_Gold = 100L;
        public long Show_Diamond = 100L;
        public long Show_Exp;
        public short KeyTrustCount;
        public int AdKeyCount;
        public int DiamondNormalExtraCount;
        public int DiamondLargeExtraCount;
        public bool guide_diamondbox;
        [JsonIgnore]
        public int BattleAdCount;
        [JsonIgnore]
        public bool bLogined;
        [JsonIgnore]
        public bool bLoginedSDK;

        protected override void OnRefresh()
        {
            FileUtils.WriteXml<LocalSave.UserInfo>("File_Currency", this);
        }
    }
}

