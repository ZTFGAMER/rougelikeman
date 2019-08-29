using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

[Serializable]
public class SaveData
{
    public int survivorMaxLevel;
    public double money;
    public double experience;
    public float totalDamage;
    public int totalDaysInRow;
    public int moneyBoxPicked;
    public long bestScore;
    public int arenaRating;
    public int gamesPlayed;
    public DateTime firstEnterDate;
    public List<HeroData> heroData;
    public List<ZombieData> zombieData;
    public List<AchievementsCompleted> achievementsCompleted;
    public List<BoostersData> boosters;
    public List<KilledBosses> killedBosses;

    public bool CheckEnumData<T, U>(ref List<T> dataList) where T: IDataInitializer<T, U> where U: struct, IConvertible
    {
        bool flag = false;
        int length = Enum.GetValues(typeof(U)).Length;
        if (dataList.Count < length)
        {
            flag = true;
            foreach (U local in (U[]) Enum.GetValues(typeof(U)))
            {
                bool flag2 = false;
                for (int i = 0; i < dataList.Count; i++)
                {
                    T local2 = dataList[i];
                    if (local2.HasType(Convert.ToInt32(local)))
                    {
                        flag2 = true;
                        break;
                    }
                }
                if (!flag2)
                {
                    T local3 = default(T);
                    dataList.Add(local3.Initialize(Convert.ToInt32(local)));
                    Debug.Log("New Data:" + local);
                }
            }
            return flag;
        }
        if (dataList.Count > length)
        {
            flag = true;
            for (int i = 0; i < dataList.Count; i++)
            {
                T local4 = dataList[i];
                if (!Enum.IsDefined(typeof(U), local4.GetInitializedType()))
                {
                    dataList.Remove(dataList[i]);
                    Debug.Log(typeof(U) + " Data Removed");
                }
            }
        }
        return flag;
    }

    public bool CheckNewData()
    {
        bool flag = false;
        if (this.CheckEnumData<HeroData, HeroData.HeroType>(ref this.heroData))
        {
            flag = true;
        }
        if (this.CheckEnumData<ZombieData, ZombieData.ZombieType>(ref this.zombieData))
        {
            flag = true;
        }
        if (this.CheckEnumData<BoostersData, BoostersData.BoosterType>(ref this.boosters))
        {
            flag = true;
        }
        if (this.survivorMaxLevel != 100)
        {
            this.survivorMaxLevel = 100;
            flag = true;
        }
        if (this.killedBosses == null)
        {
            this.killedBosses = new List<KilledBosses>();
            flag = true;
        }
        if ((0 == 0) && !(this.firstEnterDate == DateTime.MinValue))
        {
            return flag;
        }
        TimeManager.instance.SetFirstEnterDate(this);
        return true;
    }

    public bool GetDaysInGameCount(out int days)
    {
        days = 0;
        if (!TimeManager.gotDateTime)
        {
            return false;
        }
        days = (int) TimeManager.CurrentDateTime.Date.Subtract(this.firstEnterDate).TotalDays;
        if (days < 0)
        {
            TimeManager.instance.SetFirstEnterDate(this);
            days = 0;
        }
        return true;
    }

    public bool GetTimeInGameCount(out TimeSpan timeSpan)
    {
        timeSpan = new TimeSpan();
        if (!TimeManager.gotDateTime || !(this.firstEnterDate != DateTime.MinValue))
        {
            return false;
        }
        timeSpan = TimeManager.CurrentDateTime.Subtract(this.firstEnterDate);
        if (timeSpan.TotalSeconds < 0.0)
        {
            TimeManager.instance.SetFirstEnterDate(this);
            timeSpan = TimeManager.CurrentDateTime.Date.Subtract(this.firstEnterDate);
        }
        return true;
    }

    public ZombieData GetZombieByType(ZombieData.ZombieType type)
    {
        <GetZombieByType>c__AnonStorey1 storey = new <GetZombieByType>c__AnonStorey1 {
            type = type
        };
        return Enumerable.First<ZombieData>(this.zombieData, new Func<ZombieData, bool>(storey.<>m__0));
    }

    public void Init()
    {
        this.InitializeSimpleData();
        this.boosters = this.InitializeDataByEnum<BoostersData, BoostersData.BoosterType>();
        this.heroData = this.InitializeDataByEnum<HeroData, HeroData.HeroType>();
        this.zombieData = this.InitializeDataByEnum<ZombieData, ZombieData.ZombieType>();
        this.achievementsCompleted = new List<AchievementsCompleted>();
        this.killedBosses = new List<KilledBosses>();
        Debug.Log("Data initialized");
    }

    private List<T> InitializeDataByEnum<T, U>() where T: IDataInitializer<T, U> where U: struct, IConvertible
    {
        List<T> list = new List<T>();
        foreach (U local in (U[]) Enum.GetValues(typeof(U)))
        {
            T local2 = default(T);
            list.Add(local2.Initialize(Convert.ToInt32(local)));
        }
        return list;
    }

    private void InitializeSimpleData()
    {
        this.money = 0.0;
        this.bestScore = 0L;
        this.experience = 0.0;
        this.totalDamage = 0f;
        this.moneyBoxPicked = 0;
        this.totalDaysInRow = -1;
        this.survivorMaxLevel = 100;
        this.gamesPlayed = 0;
        this.arenaRating = 0;
    }

    public bool IsHeroOpened(HeroData.HeroType type)
    {
        <IsHeroOpened>c__AnonStorey0 storey = new <IsHeroOpened>c__AnonStorey0 {
            type = type
        };
        return Enumerable.FirstOrDefault<HeroData>(this.heroData, new Func<HeroData, bool>(storey.<>m__0)).isOpened;
    }

    [CompilerGenerated]
    private sealed class <GetZombieByType>c__AnonStorey1
    {
        internal SaveData.ZombieData.ZombieType type;

        internal bool <>m__0(SaveData.ZombieData zData) => 
            (zData.zombieType == this.type);
    }

    [CompilerGenerated]
    private sealed class <IsHeroOpened>c__AnonStorey0
    {
        internal SaveData.HeroData.HeroType type;

        internal bool <>m__0(SaveData.HeroData hData) => 
            (hData.heroType == this.type);
    }

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct AchievementsCompleted
    {
        public int typeID;
        public int localID;
    }

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct BoostersData : IDataInitializer<SaveData.BoostersData, SaveData.BoostersData.BoosterType>
    {
        public BoosterType type;
        public int count;
        public SaveData.BoostersData Initialize(int index)
        {
            this.type = (BoosterType) index;
            this.count = 3;
            return this;
        }

        public BoosterType GetInitializedType() => 
            this.type;

        public bool HasType(int index) => 
            (this.type == index);
        public enum BoosterType
        {
            NewSurvivor,
            AirStrike,
            KillAll
        }
    }

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct HeroData : IDataInitializer<SaveData.HeroData, SaveData.HeroData.HeroType>
    {
        public HeroType heroType;
        public int currentLevel;
        public int pickedUpCount;
        public int diedCount;
        public bool isOpened;
        public SaveData.HeroData Initialize(int index)
        {
            this.heroType = (HeroType) index;
            this.currentLevel = 1;
            this.pickedUpCount = 0;
            this.diedCount = 0;
            return this;
        }

        public HeroType GetInitializedType() => 
            this.heroType;

        public bool HasType(int index) => 
            (this.heroType == index);
        public enum HeroType
        {
            AUTOMATIC,
            SHOTGUN,
            MEDIC,
            SNIPER,
            COOK,
            MINER,
            RPG,
            SMG,
            PISTOL
        }
    }

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct ZombieData : IDataInitializer<SaveData.ZombieData, SaveData.ZombieData.ZombieType>
    {
        public ZombieType zombieType;
        public int killedByCapsule;
        public int totalTimesKilled;
        public SaveData.ZombieData Initialize(int index)
        {
            this.zombieType = (ZombieType) index;
            this.killedByCapsule = 0;
            this.totalTimesKilled = 0;
            return this;
        }

        public ZombieType GetInitializedType() => 
            this.zombieType;

        public bool HasType(int index) => 
            (this.zombieType == index);
        public enum ZombieType
        {
            NORMAL,
            COP,
            FLAG,
            BARBELL,
            CLOWN,
            BOSS,
            DAILYBOSS
        }
    }
}

