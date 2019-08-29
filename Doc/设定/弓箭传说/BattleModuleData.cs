using Dxx.Util;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using TableTool;
using UnityEngine;

public class BattleModuleData
{
    private int layer;
    private long diamond;
    private int mLevel_CurrentStage;
    private int mRebornCount;
    private float gold;
    private List<LocalSave.EquipOne> mEquips = new List<LocalSave.EquipOne>();
    private int battle_ad_use_count;
    private TurnTableType mRewardType;
    private int reward_layer = -1;
    private long BossMaxHP;
    private long BossCurrentHP;
    private Dictionary<int, int> hittedcounts = new Dictionary<int, int>();
    private Dictionary<int, int> killmonsters = new Dictionary<int, int>();
    private Dictionary<int, int> killboss = new Dictionary<int, int>();
    private float game_starttime;
    private GameMode mMode = GameMode.eLevel;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private BattleSource <mEnterSource>k__BackingField;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private GameModeBase <mModeData>k__BackingField;
    private int activeid;
    private ChallengeModeBase mChallenge;
    private bool bWin = true;
    private List<bool> mFirstShopBuy = new List<bool>();

    public void AddBossMaxHP(long hp)
    {
        this.BossMaxHP += hp;
        this.BossCurrentHP += hp;
        Facade.Instance.SendNotification("BATTLE_UI_BOSSHP_UPDATE", this.GetBossHPPercent());
    }

    public void AddDiamond(long value)
    {
        this.diamond += value;
    }

    public void AddEquip(LocalSave.EquipOne one)
    {
        this.AddEquipInternal(one);
        LocalSave.Instance.BattleIn_AddEquip(one);
    }

    public void AddEquipInternal(LocalSave.EquipOne one)
    {
        Debug.Log("addequip " + one.EquipID);
        if (!one.Overlying)
        {
            this.mEquips.Add(one);
        }
        else
        {
            bool flag = false;
            int num = 0;
            int count = this.mEquips.Count;
            while (num < count)
            {
                if (this.mEquips[num].EquipID == one.EquipID)
                {
                    LocalSave.EquipOne local1 = this.mEquips[num];
                    local1.Count += one.Count;
                    flag = true;
                    break;
                }
                num++;
            }
            if (!flag)
            {
                this.mEquips.Insert(0, one);
            }
        }
    }

    public void AddGold(float value)
    {
        this.gold += value;
        Facade.Instance.SendNotification("BATTLE_GET_GOLD", value);
    }

    public void AddHittedCount(int roomid)
    {
        Dictionary<int, int> dictionary;
        int num;
        if (!this.hittedcounts.ContainsKey(roomid))
        {
            this.hittedcounts.Add(roomid, 0);
        }
        (dictionary = this.hittedcounts)[num = roomid] = dictionary[num] + 1;
    }

    public void AddKillBoss(int entityid)
    {
        Dictionary<int, int> dictionary;
        int num;
        if (!this.killboss.ContainsKey(entityid))
        {
            this.killboss.Add(entityid, 0);
        }
        (dictionary = this.killboss)[num = entityid] = dictionary[num] + 1;
    }

    public void AddKillMonsters(int entityid)
    {
        Dictionary<int, int> dictionary;
        int num;
        if (!this.killmonsters.ContainsKey(entityid))
        {
            this.killmonsters.Add(entityid, 0);
        }
        (dictionary = this.killmonsters)[num = entityid] = dictionary[num] + 1;
    }

    public void Battle_ad_use()
    {
        this.battle_ad_use_count++;
    }

    public void BossChangeHP(long hp)
    {
        this.BossCurrentHP += hp;
        Facade.Instance.SendNotification("BATTLE_UI_BOSSHP_UPDATE", this.GetBossHPPercent());
        if (this.BossCurrentHP <= 0L)
        {
            this.BossHPClear();
        }
    }

    public void BossHPClear()
    {
        this.BossMaxHP = 0L;
        this.BossCurrentHP = 0L;
    }

    public bool Challenge_AttackEnable()
    {
        if (this.mChallenge != null)
        {
            return this.mChallenge.AttackEnable;
        }
        return true;
    }

    public bool Challenge_BombermanEnable() => 
        ((this.mChallenge != null) && this.mChallenge.BombermanEnable);

    public float Challenge_BombermanTime()
    {
        if (this.mChallenge != null)
        {
            return this.mChallenge.BombermanTime;
        }
        return 0.5f;
    }

    public void Challenge_DeInit()
    {
        if (this.mChallenge != null)
        {
            this.mChallenge.DeInit();
            this.mChallenge = null;
        }
    }

    public bool Challenge_DropExp()
    {
        if (this.mChallenge != null)
        {
            return this.mChallenge.DropExp;
        }
        return true;
    }

    public List<string> Challenge_GetConditions()
    {
        if (this.mChallenge != null)
        {
            return this.mChallenge.GetConditions();
        }
        return new List<string>();
    }

    public object Challenge_GetEvent(string eventname)
    {
        if (this.mChallenge != null)
        {
            return this.mChallenge.GetEvent(eventname);
        }
        return null;
    }

    public void Challenge_GetRewards()
    {
        if (this.mChallenge != null)
        {
            this.mChallenge.GetRewards();
        }
    }

    public string Challenge_GetSuccessString()
    {
        if (this.mChallenge != null)
        {
            return this.mChallenge.GetSuccessString();
        }
        return string.Empty;
    }

    public void Challenge_Init(int id)
    {
        this.Challenge_DeInit();
        if (id != 0)
        {
            this.ActiveID = id;
            string[] args = this.ActiveData.Args;
            if (args.Length <= 0)
            {
                SdkManager.Bugly_Report("BattleModuleData", Utils.FormatString("InitChallengeData args length == 0.", Array.Empty<object>()));
            }
            char[] separator = new char[] { ':' };
            int num = int.Parse(args[0].Split(separator)[0]);
            object[] objArray1 = new object[] { "ChallengeMode", num };
            Type type = Type.GetType(Utils.GetString(objArray1));
            object[] objArray2 = new object[] { "ChallengeMode", num };
            this.mChallenge = type.Assembly.CreateInstance(Utils.GetString(objArray2)) as ChallengeModeBase;
            this.mChallenge.Init(this.ActiveData);
        }
    }

    public bool Challenge_ismainchallenge() => 
        ((this.mChallenge != null) && (this.mEnterSource == BattleSource.eWorld));

    public void Challenge_MainUpdateMode(int id)
    {
        this.Challenge_UpdateMode(id, BattleSource.eWorld);
    }

    public void Challenge_MonsterDead()
    {
        if (this.mChallenge != null)
        {
            this.mChallenge.MonsterDead();
        }
    }

    public bool Challenge_MonsterHide() => 
        ((this.mChallenge != null) && this.mChallenge.GetMonsterHide());

    public float Challenge_MonsterHideRange()
    {
        if (this.mChallenge != null)
        {
            return this.mChallenge.GetMonsterHideRange();
        }
        return float.MaxValue;
    }

    public bool Challenge_RecoverHP()
    {
        if (this.mChallenge != null)
        {
            return this.mChallenge.RecoverHP;
        }
        return true;
    }

    public void Challenge_SendEvent(string eventname, object body = null)
    {
        if (this.mChallenge != null)
        {
            this.mChallenge.SendEvent(eventname, body);
        }
    }

    public void Challenge_SetUIParent(Transform parent)
    {
        if (this.mChallenge != null)
        {
            this.mChallenge.SetUIParent(parent);
        }
    }

    public void Challenge_Start()
    {
        if (this.mChallenge != null)
        {
            this.mChallenge.Start();
        }
    }

    public void Challenge_UpdateMode(int id)
    {
        this.Challenge_UpdateMode(id, BattleSource.eActivity);
    }

    public void Challenge_UpdateMode(int id, BattleSource source)
    {
        this.Challenge_Init(id);
        this.SetMode(this.ActiveData.GetMode(), source);
    }

    public string GetActiveLayer()
    {
        string stageLevel = LocalModelManager.Instance.Stage_Level_activity.GetBeanById(this.activeid).StageLevel;
        return this.GetActiveLayer(stageLevel, GameLogic.Release.Mode.RoomGenerate.GetCurrentRoomID());
    }

    private string GetActiveLayer(int layerid)
    {
        string stageLevel = LocalModelManager.Instance.Stage_Level_activity.GetBeanById(this.activeid).StageLevel;
        return this.GetActiveLayer(stageLevel, layerid);
    }

    public string GetActiveLayer(string stagelevel, int layerid)
    {
        object[] args = new object[] { stagelevel, layerid };
        return Utils.FormatString("{0}{1:D3}", args);
    }

    public Stage_Level_activitylevel GetActiveLevelData(int layer)
    {
        string activeLayer = this.GetActiveLayer(layer);
        Stage_Level_activitylevel beanById = LocalModelManager.Instance.Stage_Level_activitylevel.GetBeanById(activeLayer);
        object[] args = new object[] { activeLayer };
        SdkManager.Bugly_Report(beanById != null, "BattleModuleData_Active", Utils.FormatString("ActiveLevelData [{0}] is null", args));
        return beanById;
    }

    private float GetBossHPPercent()
    {
        if (this.BossMaxHP == 0L)
        {
            return 0f;
        }
        return (((float) this.BossCurrentHP) / ((float) this.BossMaxHP));
    }

    public bool GetCanReborn() => 
        (this.mRebornCount > 0);

    public long GetDiamond() => 
        this.diamond;

    public List<LocalSave.EquipOne> GetEquips() => 
        this.mEquips;

    public bool GetFirstShopBuy(int index)
    {
        if ((index < this.mFirstShopBuy.Count) && (index >= 0))
        {
            return this.mFirstShopBuy[index];
        }
        object[] args = new object[] { index, this.mFirstShopBuy.Count };
        SdkManager.Bugly_Report("BattleModuleData_Shop", Utils.FormatString("GetFirstShopBuy index = {0} is out range. mFirstShopBuy.Count = {1}", args));
        return true;
    }

    public int GetGameTime() => 
        ((int) (Updater.AliveTime - this.game_starttime));

    public float GetGold() => 
        this.gold;

    public int GetHittedCount()
    {
        int num = 0;
        Dictionary<int, int>.Enumerator enumerator = this.hittedcounts.GetEnumerator();
        while (enumerator.MoveNext())
        {
            KeyValuePair<int, int> current = enumerator.Current;
            num += current.Value;
        }
        return num;
    }

    public int GetHittedCount(int layer)
    {
        int num = 0;
        for (int i = 1; i <= layer; i++)
        {
            int num3 = 0;
            if (this.hittedcounts.TryGetValue(i, out num3))
            {
                num += num3;
            }
        }
        return num;
    }

    public int GetKillBoss()
    {
        Dictionary<int, int>.Enumerator enumerator = this.killboss.GetEnumerator();
        int num = 0;
        while (enumerator.MoveNext())
        {
            KeyValuePair<int, int> current = enumerator.Current;
            num += current.Value;
        }
        return num;
    }

    public int GetKillBoss(int entityid)
    {
        if (this.killboss.ContainsKey(entityid))
        {
            return this.killboss[entityid];
        }
        return 0;
    }

    public int GetKillMonsters()
    {
        Dictionary<int, int>.Enumerator enumerator = this.killmonsters.GetEnumerator();
        int num = 0;
        while (enumerator.MoveNext())
        {
            KeyValuePair<int, int> current = enumerator.Current;
            num += current.Value;
        }
        return num;
    }

    public int GetKillMonsters(int entityid)
    {
        if (this.killmonsters.ContainsKey(entityid))
        {
            return this.killmonsters[entityid];
        }
        return 0;
    }

    public int GetLayer() => 
        this.layer;

    public GameMode GetMode() => 
        this.mMode;

    public int GetRebornCount() => 
        this.mRebornCount;

    public int GetRewardLayer() => 
        this.reward_layer;

    public TurnTableType GetRewardType() => 
        this.mRewardType;

    private void InitFirstShop()
    {
        this.mFirstShopBuy.Clear();
        for (int i = 0; i < 2; i++)
        {
            this.mFirstShopBuy.Add(false);
        }
    }

    private void InitShop()
    {
        this.InitFirstShop();
    }

    public void InitState()
    {
        if (!PlayerPrefs.HasKey("Level_CurrentStage_Local"))
        {
            this.Level_CurrentStage = LocalSave.Instance.Stage_GetStage();
        }
    }

    public bool isEnterSourceChallenge() => 
        (this.mEnterSource == BattleSource.eChallenge);

    public bool isEnterSourceMain() => 
        (this.mEnterSource == BattleSource.eWorld);

    public bool isEnterSourceMatch() => 
        (this.mEnterSource == BattleSource.eMatch);

    public bool IsHeroMode() => 
        this.IsHeroMode(this.Level_CurrentStage);

    public bool IsHeroMode(int stageid) => 
        (stageid > 10);

    public void RemoveStageLocal()
    {
        if (PlayerPrefs.HasKey("Level_CurrentStage_Local"))
        {
            PlayerPrefs.DeleteKey("Level_CurrentStage_Local");
        }
    }

    public void Reset()
    {
        this.battle_ad_use_count = 0;
        this.killmonsters.Clear();
        this.killboss.Clear();
        this.hittedcounts.Clear();
        this.game_starttime = Updater.AliveTime;
        this.ResetGold();
        this.reset_reward();
        this.InitShop();
        this.Challenge_Init(this.ActiveID);
        this.mEquips.Clear();
        this.BossMaxHP = 0L;
        this.BossCurrentHP = 0L;
        this.SetWin(true);
        this.diamond = 0L;
        if (this.mMode == GameMode.eLevel)
        {
            this.SetRebornCount(GameConfig.GetRebornCount() - LocalSave.Instance.BattleIn_GetRebornUI());
        }
        else
        {
            this.SetRebornCount(0);
        }
    }

    private void reset_reward()
    {
        this.SetRewardType(TurnTableType.eInvalid);
        this.reward_layer = -1;
        if (this.mMode == GameMode.eLevel)
        {
            Stage_Level_stagechapter beanByChapter = LocalModelManager.Instance.Stage_Level_stagechapter.GetBeanByChapter(this.Level_CurrentStage);
            if (((beanByChapter != null) && (beanByChapter.DropAddProb > 0)) && ((GameLogic.Random(0, 100) < beanByChapter.DropAddProb) && (beanByChapter.DropAddCond.Length == 2)))
            {
                int min = beanByChapter.DropAddCond[0];
                int max = beanByChapter.DropAddCond[1];
                if (min > max)
                {
                    max = min;
                }
                this.reward_layer = GameLogic.Random(min, max);
            }
        }
    }

    private void ResetGold()
    {
        if (LocalSave.Instance.BattleIn_GetIn())
        {
            this.gold = LocalSave.Instance.BattleIn_GetGold();
        }
        else
        {
            this.gold = 0f;
        }
        this.UpdateGoldUI();
    }

    public void SetFirstShopBuy(int index)
    {
        if ((index >= this.mFirstShopBuy.Count) || (index < 0))
        {
            object[] args = new object[] { index, this.mFirstShopBuy.Count };
            SdkManager.Bugly_Report("BattleModuleData_Shop", Utils.FormatString("SetFirstShopBuy index = {0} is out range. mFirstShopBuy.Count = {1}", args));
        }
        else
        {
            this.mFirstShopBuy[index] = true;
        }
    }

    public void SetLayer(int layer)
    {
        this.layer = layer;
        if (!GameLogic.Hold.Guide.GetNeedGuide() && (GameLogic.Release.Mode.GetMode() == GameMode.eLevel))
        {
            if ((GameLogic.Hold.Guide.mEquip.process == 0) && (LocalSave.Instance.GetHaveEquips(true).Count == 1))
            {
                LocalSave.Instance.SaveExtra.AddEquipAllLayer();
            }
            LocalSave.Instance.SaveExtra.AddLayerCount(this.Level_CurrentStage, this.layer);
        }
    }

    public void SetMode(GameMode mode, BattleSource source)
    {
        this.mEnterSource = source;
        this.mMode = mode;
        if (this.mMode == GameMode.eLevel)
        {
            this.mModeData = new GameModeLevel();
        }
        else if (source == BattleSource.eWorld)
        {
            this.mModeData = new GameModeLevel();
        }
        else
        {
            this.mModeData = new GameModeGold1();
        }
    }

    public void SetRebornCount(int value)
    {
        this.mRebornCount = value;
    }

    public void SetRewardType(TurnTableType type)
    {
        this.mRewardType = type;
    }

    public void SetWin(bool value)
    {
        this.bWin = value;
    }

    private void UpdateGoldUI()
    {
    }

    public void UseReborn()
    {
        this.mRebornCount--;
    }

    public int Level_CurrentStage
    {
        get
        {
            if (this.mLevel_CurrentStage == 0)
            {
                this.mLevel_CurrentStage = PlayerPrefs.GetInt("Level_CurrentStage_Local", 1);
            }
            int num = MathDxx.Clamp(LocalSave.Instance.Stage_GetStage(), 1, LocalModelManager.Instance.Stage_Level_stagechapter.GetMaxChapter());
            if (this.mLevel_CurrentStage > num)
            {
                this.mLevel_CurrentStage = num;
            }
            return this.mLevel_CurrentStage;
        }
        set
        {
            if ((value >= 1) && (value <= LocalModelManager.Instance.Stage_Level_stagechapter.GetMaxChapter()))
            {
                this.mLevel_CurrentStage = value;
                PlayerPrefs.SetInt("Level_CurrentStage_Local", value);
                Facade.Instance.SendNotification("MainUI_LayerUpdate");
            }
        }
    }

    public BattleSource mEnterSource { get; private set; }

    public GameModeBase mModeData { get; private set; }

    public int ActiveID
    {
        get => 
            this.activeid;
        set => 
            (this.activeid = value);
    }

    public Stage_Level_activity ActiveData
    {
        get
        {
            Stage_Level_activity beanById = LocalModelManager.Instance.Stage_Level_activity.GetBeanById(this.activeid);
            object[] args = new object[] { this.activeid };
            SdkManager.Bugly_Report(beanById != null, "BattleModuleData_Active", Utils.FormatString("ActiveData [{0}] is null", args));
            return beanById;
        }
    }

    public Stage_Level_activitylevel ActiveLevelData
    {
        get
        {
            string activeLayer = this.GetActiveLayer();
            Stage_Level_activitylevel beanById = LocalModelManager.Instance.Stage_Level_activitylevel.GetBeanById(activeLayer);
            object[] args = new object[] { activeLayer };
            SdkManager.Bugly_Report(beanById != null, "BattleModuleData_Active", Utils.FormatString("ActiveLevelData [{0}] is null", args));
            return beanById;
        }
    }

    public bool Win =>
        this.bWin;
}

