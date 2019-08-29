using EPPZ.Cloud;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using UnityEngine;

public class IOSCloudSave : MonoBehaviour
{
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static IOSCloudSave <instance>k__BackingField;
    [HideInInspector]
    public bool loadingCompleted;

    private void Awake()
    {
        instance = this;
    }

    public void CheckLoadPrefsKey(string key)
    {
        if (EPPZ.Cloud.Cloud.IntForKey(key) == 1)
        {
            PlayerPrefs.SetInt(key, 1);
        }
    }

    public void CheckSavePrefsKey(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            EPPZ.Cloud.Cloud.SetIntForKey(1, key);
        }
        else
        {
            EPPZ.Cloud.Cloud.SetIntForKey(0, key);
        }
    }

    [DebuggerHidden]
    private IEnumerator DelayedBossLoading() => 
        new <DelayedBossLoading>c__Iterator1 { $this = this };

    public void ForceSave()
    {
    }

    private void LoadAllBossesRewarded()
    {
        for (int i = 0; i < WavesManager.instance.bossesByWorld.Length; i++)
        {
            if (EPPZ.Cloud.Cloud.IntForKey(StaticConstants.allBossesRewardedkey + i) == 1)
            {
                PlayerPrefs.SetInt(StaticConstants.allBossesRewardedkey + i, 1);
            }
        }
    }

    private List<SaveData.BoostersData> LoadBoosterData()
    {
        List<SaveData.BoostersData> list = new List<SaveData.BoostersData>();
        for (int i = 0; i < Enum.GetValues(typeof(SaveData.BoostersData.BoosterType)).Length; i++)
        {
            SaveData.BoostersData item = new SaveData.BoostersData {
                type = (SaveData.BoostersData.BoosterType) i,
                count = EPPZ.Cloud.Cloud.IntForKey(i + "boostercount")
            };
            list.Add(item);
        }
        return list;
    }

    private void LoadFirstDate()
    {
        try
        {
            DataLoader.playerData.firstEnterDate = DateTime.Parse(EPPZ.Cloud.Cloud.StringForKey("FirstEnterDate"), CultureInfo.InvariantCulture);
        }
        catch (Exception)
        {
            UnityEngine.Debug.Log("Cant load first date");
            base.StartCoroutine(this.WaitForTimeManger());
        }
    }

    private List<SaveData.HeroData> LoadHeroData()
    {
        List<SaveData.HeroData> list = new List<SaveData.HeroData>();
        for (int i = 0; i < Enum.GetValues(typeof(SaveData.HeroData.HeroType)).Length; i++)
        {
            int num2 = EPPZ.Cloud.Cloud.IntForKey(i + "level");
            if (num2 < 1)
            {
                num2 = 1;
            }
            SaveData.HeroData item = new SaveData.HeroData {
                heroType = (SaveData.HeroData.HeroType) i,
                currentLevel = num2,
                pickedUpCount = EPPZ.Cloud.Cloud.IntForKey(i + "pickedUP"),
                diedCount = EPPZ.Cloud.Cloud.IntForKey(i + "died"),
                isOpened = EPPZ.Cloud.Cloud.BoolForKey(i + "isopened")
            };
            list.Add(item);
        }
        return list;
    }

    public void LoadPlayerPrefs()
    {
        this.CheckLoadPrefsKey(StaticConstants.starterPackPurchased);
        this.CheckLoadPrefsKey(StaticConstants.interstitialAdsKey);
        this.CheckLoadPrefsKey(StaticConstants.infinityMultiplierPurchased);
    }

    private void LoadRewardedKilledBosses()
    {
        DataLoader.playerData.killedBosses = new List<KilledBosses>();
        for (int i = 0; i < WavesManager.instance.bossesByWorld.Length; i++)
        {
            for (int j = 0; j < WavesManager.instance.bossesByWorld[i].bosses.Length; j++)
            {
                int num3 = EPPZ.Cloud.Cloud.IntForKey(WavesManager.instance.bossesByWorld[i].bosses[j].prefabBoss.myNameIs + "Stage");
                if (num3 > 0)
                {
                    KilledBosses item = new KilledBosses {
                        name = WavesManager.instance.bossesByWorld[i].bosses[j].prefabBoss.myNameIs,
                        count = 0,
                        rewardedStage = num3
                    };
                    DataLoader.playerData.killedBosses.Add(item);
                }
            }
        }
    }

    private List<SaveData.ZombieData> LoadZombieData()
    {
        List<SaveData.ZombieData> list = new List<SaveData.ZombieData>();
        for (int i = 0; i < Enum.GetValues(typeof(SaveData.ZombieData.ZombieType)).Length; i++)
        {
            SaveData.ZombieData item = new SaveData.ZombieData {
                zombieType = (SaveData.ZombieData.ZombieType) i,
                totalTimesKilled = EPPZ.Cloud.Cloud.IntForKey(i + "killedtotal"),
                killedByCapsule = EPPZ.Cloud.Cloud.IntForKey(i + "killedcapsule")
            };
            list.Add(item);
        }
        return list;
    }

    public void SaveAll()
    {
    }

    private void SaveAllBossesRewarded()
    {
        for (int i = 0; i < WavesManager.instance.bossesByWorld.Length; i++)
        {
            EPPZ.Cloud.Cloud.SetIntForKey(!PlayerPrefs.HasKey(StaticConstants.allBossesRewardedkey + i) ? 0 : 1, StaticConstants.allBossesRewardedkey + i);
        }
    }

    private void SaveBoosterData(List<SaveData.BoostersData> boosters)
    {
        for (int i = 0; i < boosters.Count; i++)
        {
            SaveData.BoostersData data = boosters[i];
            SaveData.BoostersData data2 = boosters[i];
            EPPZ.Cloud.Cloud.SetIntForKey(data.count, ((int) data2.type) + "boostercount");
        }
    }

    public void SaveFirstDate()
    {
        EPPZ.Cloud.Cloud.SetStringForKey(DataLoader.playerData.firstEnterDate.ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture), "FirstEnterDate");
    }

    private void SaveHeroData(List<SaveData.HeroData> heroData)
    {
        for (int i = 0; i < heroData.Count; i++)
        {
            SaveData.HeroData data = heroData[i];
            SaveData.HeroData data2 = heroData[i];
            EPPZ.Cloud.Cloud.SetIntForKey(data.currentLevel, ((int) data2.heroType) + "level");
            SaveData.HeroData data3 = heroData[i];
            SaveData.HeroData data4 = heroData[i];
            EPPZ.Cloud.Cloud.SetIntForKey(data3.pickedUpCount, ((int) data4.heroType) + "pickedUP");
            SaveData.HeroData data5 = heroData[i];
            SaveData.HeroData data6 = heroData[i];
            EPPZ.Cloud.Cloud.SetIntForKey(data5.diedCount, ((int) data6.heroType) + "died");
            SaveData.HeroData data7 = heroData[i];
            SaveData.HeroData data8 = heroData[i];
            EPPZ.Cloud.Cloud.SetBoolForKey(data7.isOpened, ((int) data8.heroType) + "isopened");
        }
    }

    public void SavePlayerPrefs()
    {
        this.CheckSavePrefsKey(StaticConstants.starterPackPurchased);
        this.CheckSavePrefsKey(StaticConstants.interstitialAdsKey);
        this.CheckSavePrefsKey(StaticConstants.infinityMultiplierPurchased);
    }

    private void SaveRewardedKilledBosses()
    {
        for (int i = 0; i < DataLoader.playerData.killedBosses.Count; i++)
        {
            EPPZ.Cloud.Cloud.SetIntForKey(DataLoader.playerData.killedBosses[i].rewardedStage, DataLoader.playerData.killedBosses[i].name + "Stage");
        }
    }

    public void SaveSimple()
    {
        EPPZ.Cloud.Cloud.SetFloatForKey((float) DataLoader.playerData.experience, "experience");
        EPPZ.Cloud.Cloud.SetFloatForKey((float) DataLoader.playerData.money, "money");
        EPPZ.Cloud.Cloud.SetIntForKey(DataLoader.playerData.gamesPlayed, "gamesplayed");
        EPPZ.Cloud.Cloud.SetFloatForKey(DataLoader.playerData.totalDamage, "totalDamage");
        EPPZ.Cloud.Cloud.SetFloatForKey((float) DataLoader.playerData.bestScore, "bestscore");
        EPPZ.Cloud.Cloud.SetIntForKey(DataLoader.playerData.arenaRating, "arenaRating");
    }

    private void SaveZombieData(List<SaveData.ZombieData> zombieData)
    {
        for (int i = 0; i < zombieData.Count; i++)
        {
            SaveData.ZombieData data = zombieData[i];
            SaveData.ZombieData data2 = zombieData[i];
            EPPZ.Cloud.Cloud.SetIntForKey(data.totalTimesKilled, ((int) data2.zombieType) + "killedtotalZombies");
            SaveData.ZombieData data3 = zombieData[i];
            SaveData.ZombieData data4 = zombieData[i];
            EPPZ.Cloud.Cloud.SetIntForKey(data3.killedByCapsule, ((int) data4.zombieType) + "killedcapsuleZombies");
        }
    }

    public void SetAbilityTutorialCompleted()
    {
        if (DataLoader.playerData.gamesPlayed > 1)
        {
            PlayerPrefs.SetInt(StaticConstants.AbilityTutorialCompleted, 1);
        }
    }

    public void SetTutorialsCompleted()
    {
        PlayerPrefs.SetInt(StaticConstants.TutorialCompleted, 1);
        PlayerPrefs.SetInt(StaticConstants.UpgradeTutorialCompleted, 1);
    }

    public bool TryToLoad()
    {
        if (this.loadingCompleted)
        {
            return true;
        }
        this.loadingCompleted = true;
        return false;
    }

    [DebuggerHidden]
    private IEnumerator WaitForTimeManger() => 
        new <WaitForTimeManger>c__Iterator0 { $this = this };

    public static IOSCloudSave instance
    {
        [CompilerGenerated]
        get => 
            <instance>k__BackingField;
        [CompilerGenerated]
        private set => 
            (<instance>k__BackingField = value);
    }

    [CompilerGenerated]
    private sealed class <DelayedBossLoading>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal IOSCloudSave $this;
        internal object $current;
        internal bool $disposing;
        internal int $PC;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$disposing = true;
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                case 1:
                    if ((WavesManager.instance == null) && !DataLoader.initialized)
                    {
                        this.$current = null;
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;
                    }
                    this.$this.LoadRewardedKilledBosses();
                    this.$this.LoadAllBossesRewarded();
                    DataLoader.gui.achievementsPanel.MarkAchievementsCompleted();
                    DataLoader.gui.UpdateMenuContent();
                    UnityEngine.Debug.Log("Delayed boss Loading completed");
                    this.$PC = -1;
                    break;
            }
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current =>
            this.$current;

        object IEnumerator.Current =>
            this.$current;
    }

    [CompilerGenerated]
    private sealed class <WaitForTimeManger>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal IOSCloudSave $this;
        internal object $current;
        internal bool $disposing;
        internal int $PC;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$disposing = true;
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                case 1:
                    if (!TimeManager.gotDateTime)
                    {
                        this.$current = null;
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                    }
                    else
                    {
                        this.$current = null;
                        if (!this.$disposing)
                        {
                            this.$PC = 2;
                        }
                    }
                    return true;

                case 2:
                    this.$this.SaveFirstDate();
                    this.$PC = -1;
                    break;
            }
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current =>
            this.$current;

        object IEnumerator.Current =>
            this.$current;
    }
}

