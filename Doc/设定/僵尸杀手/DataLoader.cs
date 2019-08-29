using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class DataLoader : MonoBehaviour
{
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static DataLoader <Instance>k__BackingField;
    public static SaveData playerData;
    public static GUI gui;
    public static DataUpdateManager dataUpdateManager;
    public static bool initialized;
    [SerializeField]
    private TextAsset survivorLevels;
    [SerializeField]
    private TextAsset achievementData;
    [SerializeField]
    private TextAsset experienceData;
    [SerializeField]
    private TextAsset moneyBoxData;
    [SerializeField]
    private TextAsset survivorPowers;
    [SerializeField]
    private TextAsset survivorDamage;
    public List<ZombiePrefabData> zombiePrefabData;
    public List<Survivors> survivors;
    public List<Achievements> achievements;
    public List<DailyBonusData> dailyBonus;
    [HideInInspector]
    public List<DailyBossData> dailyBosses;
    [HideInInspector]
    public float[] moneyBoxGold;
    [HideInInspector]
    public double[] levelExperience;
    [HideInInspector]
    public int[] startSquad;
    [HideInInspector]
    public int dailyMultiplier = 1;
    private List<KilledZombies> killedZombies;
    private HidingObject[] hidingObjects;
    private int[] killedZombieCapsule;
    private int[] pickedSurvivors;
    [NonSerialized]
    public int currentDayInRow = -1;
    [NonSerialized]
    public bool secretPicked;
    [NonSerialized]
    public bool cloudSaveLoaded;
    [NonSerialized]
    public NotificationManager notifManger;
    private double inGameMoneyCounter;

    public void AddPickedUpCount(SaveData.HeroData.HeroType type)
    {
        this.pickedSurvivors[(int) type]++;
    }

    private void Awake()
    {
        if ((Instance != null) && (Instance != this))
        {
            UnityEngine.Object.Destroy(base.gameObject);
        }
        else
        {
            Instance = this;
            UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
            this.Initialize();
            Application.targetFrameRate = 60;
        }
    }

    public void BuyBoosters(SaveData.BoostersData.BoosterType boosterType, int amount = 1)
    {
        for (int i = 0; i < playerData.boosters.Count; i++)
        {
            SaveData.BoostersData data = playerData.boosters[i];
            if (data.type == boosterType)
            {
                data.count += amount;
                playerData.boosters[i] = data;
                this.SaveAllData();
                gui.popUpsPanel.shop.UpdateBoosters();
            }
        }
    }

    public void CheckClosedWalls()
    {
        int currentPlayerLevel = this.GetCurrentPlayerLevel();
        ClosedWall[] wallArray = UnityEngine.Object.FindObjectsOfType<ClosedWall>();
        for (int i = 0; i < wallArray.Length; i++)
        {
            wallArray[i].Check(currentPlayerLevel);
        }
        this.GetHidingObjects();
        for (int j = 0; j < this.hidingObjects.Length; j++)
        {
            this.hidingObjects[j].UpdateObject(currentPlayerLevel);
        }
    }

    private void FillAchievements()
    {
        string[] strArray = this.SplitTextAsset(this.achievementData);
        if (strArray.Length > this.achievements.Count)
        {
            UnityEngine.Debug.Log("You Can Add " + (strArray.Length - this.achievements.Count) + " More Elements To Achievements");
        }
        for (int i = 0; i < this.achievements.Count; i++)
        {
            char[] separator = new char[] { ';' };
            string[] strArray2 = strArray[i].Split(separator);
            this.achievements[i].ID = int.Parse(strArray2[0]);
            this.achievements[i].name = strArray2[1];
            this.achievements[i].description = strArray2[2];
            this.achievements[i].type = int.Parse(strArray2[3]);
            this.achievements[i].count = int.Parse(strArray2[4]);
            this.achievements[i].reward = int.Parse(strArray2[5]);
        }
    }

    private void FillSurvivorLevels()
    {
        int[,] array = new int[this.survivors.Count, playerData.survivorMaxLevel];
        float[,] numArray2 = new float[playerData.survivorMaxLevel, this.survivors.Count];
        float[,] numArray3 = new float[playerData.survivorMaxLevel, this.survivors.Count];
        CsvLoader.SplitText<int>(this.survivorLevels, ',', ref array);
        CsvLoader.SplitText<float>(this.survivorPowers, ',', ref numArray2);
        CsvLoader.SplitText<float>(this.survivorDamage, ',', ref numArray3);
        int[] numArray4 = new int[] { 0, 1, 2, 3, 4, 6, 8, 5, 7 };
        for (int i = 0; i < this.survivors.Count; i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                Survivors.SurvivorLevels item = new Survivors.SurvivorLevels {
                    damage = numArray3[j, i],
                    cost = array[i, j],
                    power = numArray2[j, i]
                };
                this.survivors[numArray4[i]].levels.Add(item);
            }
        }
    }

    public int GetBoostersCount(SaveData.BoostersData.BoosterType boosterType)
    {
        for (int i = 0; i < playerData.boosters.Count; i++)
        {
            SaveData.BoostersData data = playerData.boosters[i];
            if (data.type == boosterType)
            {
                return data.count;
            }
        }
        return 0;
    }

    public int GetCurrentPlayerLevel()
    {
        for (int i = 0; i < this.levelExperience.Length; i++)
        {
            if ((i + 1) < this.levelExperience.Length)
            {
                if ((playerData.experience >= this.levelExperience[i]) && (playerData.experience < this.levelExperience[i + 1]))
                {
                    return (i + 1);
                }
            }
            else
            {
                return this.levelExperience.Length;
            }
        }
        return 1;
    }

    public float GetHeroDamage(SaveData.HeroData.HeroType heroType)
    {
        <GetHeroDamage>c__AnonStorey1 storey = new <GetHeroDamage>c__AnonStorey1 {
            heroType = heroType
        };
        int currentLevel = Enumerable.FirstOrDefault<SaveData.HeroData>(playerData.heroData, new Func<SaveData.HeroData, bool>(storey.<>m__0)).currentLevel;
        Survivors.SurvivorLevels levels = Enumerable.FirstOrDefault<Survivors>(this.survivors, new Func<Survivors, bool>(storey.<>m__1)).levels[currentLevel - 1];
        return levels.damage;
    }

    public float GetHeroPower(SaveData.HeroData.HeroType heroType)
    {
        <GetHeroPower>c__AnonStorey2 storey = new <GetHeroPower>c__AnonStorey2 {
            heroType = heroType
        };
        if (storey.heroType == SaveData.HeroData.HeroType.MEDIC)
        {
            return 0f;
        }
        int currentLevel = Enumerable.FirstOrDefault<SaveData.HeroData>(playerData.heroData, new Func<SaveData.HeroData, bool>(storey.<>m__0)).currentLevel;
        Survivors.SurvivorLevels levels = Enumerable.FirstOrDefault<Survivors>(this.survivors, new Func<Survivors, bool>(storey.<>m__1)).levels[currentLevel - 1];
        return (levels.power / 10f);
    }

    public void GetHidingObjects()
    {
        this.hidingObjects = UnityEngine.Object.FindObjectsOfType<HidingObject>();
    }

    public TimeSpan GetMultiplierTime()
    {
        TimeSpan minValue = TimeSpan.MinValue;
        if (PlayerPrefs.HasKey(StaticConstants.DailyMultiplierTime))
        {
            minValue = new DateTime(Convert.ToInt64(PlayerPrefs.GetString(StaticConstants.DailyMultiplierTime)), DateTimeKind.Utc).Subtract(TimeManager.CurrentDateTime);
        }
        if (minValue.TotalSeconds > 0.0)
        {
            return minValue;
        }
        return new TimeSpan();
    }

    public SurvivorHuman GetSurvivorPrefab(SaveData.HeroData.HeroType type)
    {
        foreach (Survivors survivors in this.survivors)
        {
            if (survivors.heroType == type)
            {
                return survivors.survivorPrefab;
            }
        }
        return null;
    }

    public void Initialize()
    {
        initialized = false;
        if (!SaveManager.Load<SaveData>(StaticConstants.PlayerSaveDataPath, ref playerData))
        {
            if ((playerData == null) || ((playerData != null) && !playerData.CheckNewData()))
            {
                PlayerPrefs.DeleteAll();
                SaveManager.RemoveData(StaticConstants.PlayerSaveDataPath);
                playerData = new SaveData();
                playerData.Init();
                gui = UnityEngine.Object.FindObjectOfType<GUI>();
                if (gui != null)
                {
                    gui.StartTutorialAfterReset();
                }
            }
        }
        else
        {
            playerData.CheckNewData();
        }
        this.SaveAllData();
        this.FillSurvivorLevels();
        this.FillAchievements();
        this.SetExperienceLevels();
        this.SetMoneyBoxData();
        this.SetZombiePrefabData();
        dataUpdateManager = UnityEngine.Object.FindObjectOfType<DataUpdateManager>();
        this.notifManger = UnityEngine.Object.FindObjectOfType<NotificationManager>();
        this.hidingObjects = null;
        LoadingScene scene = UnityEngine.Object.FindObjectOfType<LoadingScene>();
        if (scene != null)
        {
            scene.StartLoading();
        }
    }

    public void OpenHero(SaveData.HeroData.HeroType heroType)
    {
        <OpenHero>c__AnonStorey0 storey = new <OpenHero>c__AnonStorey0 {
            heroType = heroType
        };
        int index = playerData.heroData.IndexOf(Enumerable.First<SaveData.HeroData>(playerData.heroData, new Func<SaveData.HeroData, bool>(storey.<>m__0)));
        SaveData.HeroData data = playerData.heroData[index];
        data.isOpened = true;
        playerData.heroData[index] = data;
        this.SaveAllData();
        this.UpdateHeroesIsOpened();
        gui.UpdateMenuContent();
        GameManager.instance.Reset();
        UnityEngine.Object.FindObjectOfType<HeroInfo>().SetIsLocked(false, -1);
    }

    public bool RefreshMoney(double money, bool needToSave = false)
    {
        if ((playerData.money + money) < 0.0)
        {
            return false;
        }
        playerData.money += money;
        gui.UpdateMoney();
        if (needToSave)
        {
            this.SaveAllData();
        }
        return true;
    }

    public void ResetLocalInfo()
    {
        this.UpdateIdleHeroDamage();
        this.killedZombies = new List<KilledZombies>();
        this.pickedSurvivors = new int[Enum.GetValues(typeof(SaveData.HeroData.HeroType)).Length];
        this.UpdateInGameMoneyCounter(-this.inGameMoneyCounter);
        this.CheckClosedWalls();
        this.secretPicked = false;
        gui.isnewHeroOpened = false;
    }

    public void SaveAllData()
    {
        if (playerData != null)
        {
            SaveManager.Save<SaveData>(playerData, StaticConstants.PlayerSaveDataPath);
        }
    }

    public void SaveDeadByCapsule(SaveData.ZombieData.ZombieType type)
    {
        this.killedZombieCapsule[(int) type]++;
    }

    public void SaveDeadZombie(SaveData.ZombieData.ZombieType type, float _power)
    {
        <SaveDeadZombie>c__AnonStorey3 storey = new <SaveDeadZombie>c__AnonStorey3 {
            _power = _power
        };
        if (Enumerable.Any<KilledZombies>(this.killedZombies, new Func<KilledZombies, bool>(storey.<>m__0)))
        {
            KilledZombies local1 = Enumerable.First<KilledZombies>(this.killedZombies, new Func<KilledZombies, bool>(storey.<>m__1));
            local1.count++;
        }
        else
        {
            KilledZombies item = new KilledZombies {
                id = (int) type,
                count = 1,
                power = storey._power
            };
            this.killedZombies.Add(item);
        }
        this.UpdateInGameMoneyCounter((double) ((storey._power * StaticConstants.InGameGoldConst) * this.dailyMultiplier));
    }

    public void SaveEndMatchInfo()
    {
        double a = 0.0;
        int newHaters = 0;
        <SaveEndMatchInfo>c__AnonStorey5 storey = new <SaveEndMatchInfo>c__AnonStorey5 {
            $this = this,
            i = 0
        };
        while (storey.i < this.killedZombies.Count)
        {
            SaveData.ZombieData data = Enumerable.First<SaveData.ZombieData>(playerData.zombieData, new Func<SaveData.ZombieData, bool>(storey.<>m__0));
            if (data.zombieType != SaveData.ZombieData.ZombieType.BOSS)
            {
                data.totalTimesKilled += this.killedZombies[storey.i].count;
            }
            for (int j = 0; j < playerData.zombieData.Count; j++)
            {
                SaveData.ZombieData data2 = playerData.zombieData[j];
                if (data2.zombieType == this.killedZombies[storey.i].id)
                {
                    playerData.zombieData[j] = data;
                }
            }
            playerData.totalDamage += this.zombiePrefabData[storey.i].health * this.killedZombies[storey.i].count;
            storey.i++;
        }
        a = GameManager.instance.Points * StaticConstants.InGameExpConst;
        playerData.money += this.inGameMoneyCounter;
        playerData.experience += Math.Ceiling(a);
        if (playerData.experience > this.levelExperience[this.levelExperience.Length - 1])
        {
            playerData.experience = this.levelExperience[this.levelExperience.Length - 1];
        }
        for (int i = 0; i < this.pickedSurvivors.Length; i++)
        {
            SaveData.HeroData data3 = playerData.heroData[i];
            data3.pickedUpCount += this.pickedSurvivors[i];
            newHaters += this.pickedSurvivors[i];
            data3.diedCount += this.pickedSurvivors[i] + this.startSquad[i];
            playerData.heroData[i] = data3;
        }
        long points = (long) GameManager.instance.Points;
        playerData.gamesPlayed++;
        gui.survivorUpgradePanel.SetRandomVideo();
        IOSCloudSave.instance.SaveAll();
        if (!GameManager.instance.isTutorialNow)
        {
            if (points > playerData.bestScore)
            {
                playerData.bestScore = points;
            }
            LeaderboardManager.instance.PostScoreOnLeaderBoard(points);
            gui.gameOverContent.presentController.TryToShowPresent((int) this.inGameMoneyCounter);
            gui.gameOverContent.SetContent(this.inGameMoneyCounter, 1f, newHaters, Math.Ceiling(a), points, GameManager.instance.inGameTime, "Game Over");
        }
        gui.ShowLastOpenedHero(HeroOpenType.Level);
        this.SaveAllData();
    }

    public void SaveKilledBoss(float power, string _name)
    {
        <SaveKilledBoss>c__AnonStorey4 storey = new <SaveKilledBoss>c__AnonStorey4 {
            _name = _name
        };
        SaveData.ZombieData data = playerData.zombieData[5];
        data.totalTimesKilled++;
        playerData.zombieData[5] = data;
        this.SaveDeadZombie(SaveData.ZombieData.ZombieType.BOSS, power);
        if (Enumerable.Any<KilledBosses>(playerData.killedBosses, new Func<KilledBosses, bool>(storey.<>m__0)))
        {
            KilledBosses local1 = Enumerable.First<KilledBosses>(playerData.killedBosses, new Func<KilledBosses, bool>(storey.<>m__1));
            local1.count++;
        }
        else
        {
            KilledBosses item = new KilledBosses {
                name = storey._name,
                count = 1,
                rewarded = false,
                rewardedStage = 0
            };
            playerData.killedBosses.Add(item);
        }
        this.SaveAllData();
        Dictionary<string, string> eventParameters = new Dictionary<string, string> {
            { 
                "Name",
                storey._name
            },
            { 
                "TimeFromStart",
                GameManager.instance.inGameTime.ToString()
            }
        };
        AnalyticsManager.instance.LogEvent("KilledBoss", eventParameters);
    }

    public void SavePickedMoneyBox(Vector3 position)
    {
        if (TimeManager.gotDateTime)
        {
            PlayerPrefs.SetString(StaticConstants.MoneyBoxKey, TimeManager.CurrentDateTime.Date.Ticks.ToString());
            playerData.moneyBoxPicked++;
            UnityEngine.Object.Instantiate<TextMesh>(GameManager.instance.floatingTextPrefab, position, Quaternion.identity, TransformParentManager.Instance.fx).text = this.moneyBoxGold[playerData.moneyBoxPicked - 1].ToString();
            if (playerData.moneyBoxPicked >= (this.moneyBoxGold.Length - 1))
            {
                playerData.moneyBoxPicked -= 5;
            }
            this.RefreshMoney((double) (this.moneyBoxGold[playerData.moneyBoxPicked - 1] * this.dailyMultiplier), true);
            gui.newSecret.SetActive(false);
            MoneyBoxManager.instance.TrySpawnBox();
            this.secretPicked = true;
            Dictionary<string, string> eventParameters = new Dictionary<string, string> {
                { 
                    "Level",
                    playerData.moneyBoxPicked.ToString()
                }
            };
            AnalyticsManager.instance.LogEvent("MoneyBoxPicked", eventParameters);
        }
    }

    private void SetExperienceLevels()
    {
        string[] strArray = this.SplitTextAsset(this.experienceData);
        this.levelExperience = new double[strArray.Length + 1];
        for (int i = 0; i < strArray.Length; i++)
        {
            char[] separator = new char[] { ',' };
            double.TryParse(strArray[i].Split(separator)[1], out this.levelExperience[i + 1]);
        }
    }

    public static void SetGui(GUI _gui)
    {
        gui = _gui;
        initialized = true;
    }

    private void SetMoneyBoxData()
    {
        string[] strArray = this.SplitTextAsset(this.moneyBoxData);
        this.moneyBoxGold = new float[strArray.Length];
        for (int i = 0; i < strArray.Length; i++)
        {
            char[] separator = new char[] { ',' };
            float.TryParse(strArray[i].Split(separator)[1], out this.moneyBoxGold[i]);
        }
        if (playerData.moneyBoxPicked >= (this.moneyBoxGold.Length - 1))
        {
            playerData.moneyBoxPicked -= 5;
        }
    }

    public void SetNewMultiplier(int multiplier, int durationInSeconds)
    {
        if (TimeManager.gotDateTime)
        {
            if (multiplier > this.dailyMultiplier)
            {
                PlayerPrefs.SetInt(StaticConstants.MultiplierKey, multiplier);
            }
            PlayerPrefs.SetString(StaticConstants.DailyMultiplierTime, TimeManager.CurrentDateTime.AddSeconds(durationInSeconds + this.GetMultiplierTime().TotalSeconds).Ticks.ToString());
            dataUpdateManager.UpdateDailyMultiplier();
            this.SaveAllData();
        }
    }

    public void SetPlayerLevel(int level)
    {
        playerData.experience = this.levelExperience[level - 1];
        this.SaveAllData();
    }

    public void SetTotalDays(int value, bool needToSave = false)
    {
        SaveData playerData = DataLoader.playerData;
        playerData.totalDaysInRow = value;
        DataLoader.playerData = playerData;
        if (needToSave)
        {
            this.SaveAllData();
        }
    }

    private void SetZombiePrefabData()
    {
        for (int i = 0; i < this.zombiePrefabData.Count; i++)
        {
            this.zombiePrefabData[i].power = ((float) this.zombiePrefabData[i].health) / 53f;
            this.zombiePrefabData[i].SetPrefabData();
        }
    }

    public string[] SplitTextAsset(TextAsset textAsset)
    {
        char[] separator = new char[] { '\n' };
        return textAsset.text.Split(separator);
    }

    public void UpdateClosedWallsText()
    {
        ClosedWall[] wallArray = UnityEngine.Object.FindObjectsOfType<ClosedWall>();
        for (int i = 0; i < wallArray.Length; i++)
        {
            wallArray[i].UpdateText();
        }
    }

    public bool UpdateHeroesIsOpened()
    {
        bool flag = false;
        int currentPlayerLevel = this.GetCurrentPlayerLevel();
        for (int i = 0; i < playerData.heroData.Count; i++)
        {
            SaveData.HeroData data = playerData.heroData[i];
            if (!data.isOpened)
            {
                foreach (Survivors survivors in this.survivors)
                {
                    if (survivors.heroType == data.heroType)
                    {
                        switch (survivors.heroOpenType)
                        {
                            case HeroOpenType.Level:
                                data.isOpened = currentPlayerLevel >= survivors.requiredLevelToOpen;
                                break;

                            case HeroOpenType.Day:
                                goto Label_0091;
                        }
                    }
                    continue;
                Label_0091:
                    if (playerData.GetTimeInGameCount(out TimeSpan span))
                    {
                        data.isOpened = span.TotalDays >= survivors.daysToOpen;
                    }
                }
                SaveData.HeroData data2 = playerData.heroData[i];
                if (data2.isOpened != data.isOpened)
                {
                    playerData.heroData[i] = data;
                    if (i != 0)
                    {
                        flag = true;
                    }
                }
            }
        }
        return flag;
    }

    public void UpdateIdleHero(SaveData.HeroData.HeroType type)
    {
        Transform parent = null;
        if (type == SaveData.HeroData.HeroType.MEDIC)
        {
            HealerSurvivor[] survivorArray = UnityEngine.Object.FindObjectsOfType<HealerSurvivor>();
            for (int i = 0; i < survivorArray.Length; i++)
            {
                survivorArray[i].healDelay = 1f / this.GetHeroDamage(survivorArray[i].heroType);
                parent = survivorArray[i].transform;
            }
        }
        else if (type == SaveData.HeroData.HeroType.COOK)
        {
            BafferSurvivor[] survivorArray2 = UnityEngine.Object.FindObjectsOfType<BafferSurvivor>();
            for (int i = 0; i < survivorArray2.Length; i++)
            {
                survivorArray2[i].bafDelay = 1f / this.GetHeroDamage(survivorArray2[i].heroType);
                parent = survivorArray2[i].transform;
            }
        }
        else
        {
            SurvivorHuman[] humanArray = UnityEngine.Object.FindObjectsOfType<SurvivorHuman>();
            for (int i = 0; i < humanArray.Length; i++)
            {
                if (type == humanArray[i].heroType)
                {
                    humanArray[i].heroDamage = this.GetHeroDamage(humanArray[i].heroType);
                    parent = humanArray[i].transform;
                }
            }
        }
        ParticleSystem system = UnityEngine.Object.Instantiate<ParticleSystem>(GameManager.instance.inGameUpgrade, parent);
        system.Play();
        UnityEngine.Object.Destroy(system.gameObject, system.main.duration);
    }

    public void UpdateIdleHeroDamage()
    {
        SurvivorHuman[] humanArray = UnityEngine.Object.FindObjectsOfType<SurvivorHuman>();
        for (int i = 0; i < humanArray.Length; i++)
        {
            humanArray[i].heroDamage = this.GetHeroDamage(humanArray[i].heroType);
        }
    }

    public void UpdateInGameMoneyCounter(double money)
    {
        this.inGameMoneyCounter += money;
        gui.UpdateMoney(this.inGameMoneyCounter);
    }

    public bool UseBoosters(SaveData.BoostersData.BoosterType boosterType)
    {
        for (int i = 0; i < playerData.boosters.Count; i++)
        {
            SaveData.BoostersData data = playerData.boosters[i];
            if (data.type == boosterType)
            {
                data.count--;
                playerData.boosters[i] = data;
                Dictionary<string, string> eventParameters = new Dictionary<string, string> {
                    { 
                        "Type",
                        boosterType.ToString()
                    },
                    { 
                        "Count_Remaining",
                        data.count.ToString()
                    }
                };
                AnalyticsManager.instance.LogEvent("Used_Ability", eventParameters);
                object[] args = new object[] { boosterType };
                UnityEngine.Debug.LogFormat("Used booster {0}", args);
                return true;
            }
        }
        return false;
    }

    public static DataLoader Instance
    {
        [CompilerGenerated]
        get => 
            <Instance>k__BackingField;
        [CompilerGenerated]
        private set => 
            (<Instance>k__BackingField = value);
    }

    [CompilerGenerated]
    private sealed class <GetHeroDamage>c__AnonStorey1
    {
        internal SaveData.HeroData.HeroType heroType;

        internal bool <>m__0(SaveData.HeroData hd) => 
            (hd.heroType == this.heroType);

        internal bool <>m__1(Survivors s) => 
            (s.heroType == this.heroType);
    }

    [CompilerGenerated]
    private sealed class <GetHeroPower>c__AnonStorey2
    {
        internal SaveData.HeroData.HeroType heroType;

        internal bool <>m__0(SaveData.HeroData hd) => 
            (hd.heroType == this.heroType);

        internal bool <>m__1(Survivors s) => 
            (s.heroType == this.heroType);
    }

    [CompilerGenerated]
    private sealed class <OpenHero>c__AnonStorey0
    {
        internal SaveData.HeroData.HeroType heroType;

        internal bool <>m__0(SaveData.HeroData hd) => 
            (hd.heroType == this.heroType);
    }

    [CompilerGenerated]
    private sealed class <SaveDeadZombie>c__AnonStorey3
    {
        internal float _power;

        internal bool <>m__0(KilledZombies kz) => 
            (kz.power == this._power);

        internal bool <>m__1(KilledZombies kz) => 
            (kz.power == this._power);
    }

    [CompilerGenerated]
    private sealed class <SaveEndMatchInfo>c__AnonStorey5
    {
        internal int i;
        internal DataLoader $this;

        internal bool <>m__0(SaveData.ZombieData zd) => 
            (zd.zombieType == this.$this.killedZombies[this.i].id);
    }

    [CompilerGenerated]
    private sealed class <SaveKilledBoss>c__AnonStorey4
    {
        internal string _name;

        internal bool <>m__0(KilledBosses kb) => 
            (kb.name == this._name);

        internal bool <>m__1(KilledBosses kb) => 
            (kb.name == this._name);
    }
}

