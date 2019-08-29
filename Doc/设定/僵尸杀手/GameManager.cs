using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [HideInInspector]
    public AsyncOperation operation;
    [HideInInspector]
    public float Points;
    [HideInInspector]
    public int newSurvivorsLeft;
    [HideInInspector]
    public StartPoint[] startPoints;
    [SerializeField]
    public GameObject prefabHealingFx;
    [SerializeField]
    public GameObject prefabTakeDamageSurvivor;
    [SerializeField]
    public GameObject prefabTakeDamageZombie;
    [SerializeField]
    public GameObject prefabZombieBlood;
    [SerializeField]
    public GameObject prefabRareGlowFx;
    [SerializeField]
    public GameObject prefabBafFx;
    [SerializeField]
    public GameObject prefabFlyEndFx;
    [SerializeField]
    public TrailRenderer prefabZombieTrail;
    [SerializeField]
    public ParticleSystem prefabProtestingZombieFx;
    [SerializeField]
    public ParticleSystem prefabZombieSpawnFx;
    [SerializeField]
    public ParticleSystem prefabArmageddonFx;
    [SerializeField]
    public ParticleSystem prefabBossDeathFx;
    public ParticleSystem inGameUpgrade;
    [SerializeField]
    private Transform protectDome;
    [SerializeField]
    private AbilityButton[] abilityButtons;
    private Transform tutorialStartPoint;
    public string[] worldNames;
    private ParticleSystem killAllFx;
    private CameraTarget cameraTarget;
    [HideInInspector]
    public bool isTutorialNow;
    private bool squadSpawned = true;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private GameModes <currentGameMode>k__BackingField;
    public List<SurvivorHuman> survivors = new List<SurvivorHuman>();
    public List<ZombieHuman> zombies = new List<ZombieHuman>();
    public TextMesh floatingTextPrefab;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private float <allHeroPowers>k__BackingField;
    [Space, SerializeField]
    public int[] bossKillsForOpenWorld;
    public int currentWorldNumber = 1;
    [NonSerialized]
    public int inGameTime;
    [HideInInspector]
    public StartPoint startPoint;
    [HideInInspector]
    public float OpponentPoints;
    [HideInInspector]
    public int timeLeft;
    [HideInInspector]
    public int currentArenaMaxPoints;
    [HideInInspector]
    public int currentOpponentPoints;
    private Coroutine arenaProgress;

    [DebuggerHidden]
    private IEnumerator ArenaProgress() => 
        new <ArenaProgress>c__Iterator1 { $this = this };

    private void Awake()
    {
        instance = this;
        if (!PlayerPrefs.HasKey(StaticConstants.TutorialCompleted))
        {
            this.isTutorialNow = true;
            this.currentGameMode = GameModes.GamePlay;
            base.InvokeRepeating("IncreaseGameTime", 0f, 1f);
        }
        else
        {
            this.currentGameMode = GameModes.Idle;
        }
        this.cameraTarget = UnityEngine.Object.FindObjectOfType<CameraTarget>();
    }

    public void CalculateAllHeroPowers()
    {
        this.allHeroPowers = 0f;
        for (int i = 0; i < DataLoader.playerData.heroData.Count; i++)
        {
            SaveData.HeroData data = DataLoader.playerData.heroData[i];
            this.allHeroPowers += DataLoader.Instance.GetHeroPower(data.heroType);
        }
    }

    public int ChangeWorld(int direction)
    {
        if (!this.IsWorldOpen(this.currentWorldNumber + direction))
        {
            return (this.bossKillsForOpenWorld[(this.currentWorldNumber + direction) - 1] - DataLoader.playerData.GetZombieByType(SaveData.ZombieData.ZombieType.BOSS).totalTimesKilled);
        }
        if (DataLoader.gui != null)
        {
            DataLoader.gui.Loading(true);
        }
        SceneManager.UnloadSceneAsync("World" + this.currentWorldNumber);
        this.currentWorldNumber += direction;
        SpawnManager.instance.Reset();
        WavesManager.instance.Reset();
        this.operation = SceneManager.LoadSceneAsync("World" + this.currentWorldNumber, LoadSceneMode.Additive);
        base.StartCoroutine(this.WaitForScene(new Action(this.Reset)));
        PlayerPrefs.SetInt(StaticConstants.currentWorld, this.currentWorldNumber);
        PlayerPrefs.Save();
        return 0;
    }

    public void DailyBossComplete()
    {
        UnityEngine.Debug.Log("Daily boss complete");
        this.squadSpawned = false;
        base.CancelInvoke("IncreaseGameTime");
        DataLoader.Instance.SaveEndMatchInfo();
        DataLoader.gui.DailyBossComplete();
    }

    public void DecreaseSurvivor(SurvivorHuman survivor)
    {
        if ((this.survivors.Remove(survivor) && (this.survivors.Count <= 0)) && (UnityEngine.Object.FindObjectsOfType<Parashute>().Length <= 0))
        {
            this.GameOver();
        }
    }

    public void DecreaseZombie(ZombieHuman zombie)
    {
        if (this.zombies.Remove(zombie) && !this.isTutorialNow)
        {
            if (this.currentGameMode != GameModes.Idle)
            {
                this.Points += zombie.power;
                DataLoader.Instance.SaveDeadZombie(zombie.zombieType, zombie.power);
                UIStreak.instance.IncreaseStreak();
            }
            if (this.currentGameMode == GameModes.GamePlay)
            {
                WavesManager.instance.TrySpawnBoss();
                DataLoader.gui.RefreshProgressToBoss();
            }
            else if (this.currentGameMode == GameModes.Arena)
            {
                DataLoader.gui.RefreshArenaProgresses();
                if (this.Points >= this.currentArenaMaxPoints)
                {
                    base.StopCoroutine(this.arenaProgress);
                    Time.timeScale = 0f;
                    this.GameOver();
                }
            }
            else if ((this.currentGameMode == GameModes.DailyBoss) && (this.zombies.Count <= 0))
            {
                this.DailyBossComplete();
            }
        }
    }

    public void GameOver()
    {
        SpawnManager.instance.StopIt();
        WavesManager.instance.StopIt();
        MoneyCoinManager.instance.EndGame();
        MoneyBoxManager.instance.EndGame();
        this.squadSpawned = false;
        base.CancelInvoke("IncreaseGameTime");
        DataLoader.Instance.SaveEndMatchInfo();
        DataLoader.gui.GameOver();
        if (this.isTutorialNow)
        {
            this.isTutorialNow = false;
            PlayerPrefs.SetInt(StaticConstants.TutorialCompleted, 0);
            PlayerPrefs.Save();
        }
    }

    public int GetWorldsCount() => 
        this.bossKillsForOpenWorld.Length;

    public void Go()
    {
        this.protectDome.gameObject.SetActive(false);
        this.currentGameMode = GameModes.GamePlay;
        if (this.isTutorialNow || PlayerPrefs.HasKey(StaticConstants.AbilityTutorialCompleted))
        {
            WavesManager.instance.StartGame();
        }
        else
        {
            WavesManager.instance.StopIt();
        }
        this.inGameTime = 0;
        base.InvokeRepeating("IncreaseGameTime", 0f, 1f);
        AnalyticsManager.instance.LogEvent("GameStarted", new Dictionary<string, string>());
        SpawnManager.instance.StartGame();
        MoneyCoinManager.instance.StartGame();
        MoneyBoxManager.instance.StartGame();
        DataLoader.Instance.ResetLocalInfo();
        this.CalculateAllHeroPowers();
        UIStreak.instance.ResetStreak();
        foreach (AbilityButton button in this.abilityButtons)
        {
            button.Reset();
        }
    }

    public void GoArena()
    {
        this.currentGameMode = GameModes.Arena;
        this.cameraTarget.enabled = false;
        WavesManager.instance.StopIt();
        if (DataLoader.gui != null)
        {
            DataLoader.gui.Loading(true);
        }
        foreach (ZombieHuman human in this.zombies)
        {
            UnityEngine.Object.Destroy(human.gameObject);
        }
        this.zombies.Clear();
        foreach (SurvivorHuman human2 in this.survivors)
        {
            UnityEngine.Object.Destroy(human2.gameObject);
        }
        this.survivors.Clear();
        SceneManager.UnloadSceneAsync("World" + this.currentWorldNumber);
        SpawnManager.instance.Reset();
        WavesManager.instance.Reset();
        this.operation = SceneManager.LoadSceneAsync("Arena1", LoadSceneMode.Additive);
        base.StartCoroutine(this.WaitForScene(new Action(this.StartArena)));
    }

    public void GoDailyBoss()
    {
        this.currentWorldNumber = 0;
        this.Reset();
        this.protectDome.gameObject.SetActive(false);
        this.currentGameMode = GameModes.DailyBoss;
        WavesManager.instance.SpawnDailyBoss();
        this.inGameTime = 0;
        base.InvokeRepeating("IncreaseGameTime", 0f, 1f);
        DataLoader.Instance.ResetLocalInfo();
        foreach (AbilityButton button in this.abilityButtons)
        {
            button.Reset();
        }
    }

    public void IncreaseGameTime()
    {
        this.inGameTime++;
    }

    public bool IsTutorialCompleted() => 
        ((PlayerPrefs.HasKey(StaticConstants.TutorialCompleted) && PlayerPrefs.HasKey(StaticConstants.AbilityTutorialCompleted)) && PlayerPrefs.HasKey(StaticConstants.UpgradeTutorialCompleted));

    public bool IsWorldOpen(int worldNumber) => 
        ((this.bossKillsForOpenWorld.Length >= worldNumber) && (this.bossKillsForOpenWorld[worldNumber - 1] <= DataLoader.playerData.GetZombieByType(SaveData.ZombieData.ZombieType.BOSS).totalTimesKilled));

    public void KillAll()
    {
        for (int i = 0; i < this.zombies.Count; i++)
        {
            if (this.zombies[i].tag != "ZombieBoss")
            {
                this.zombies[i].TakeDamage(this.zombies[i].maxCountHealth + 10);
                i--;
            }
        }
        if (this.killAllFx == null)
        {
            this.killAllFx = UnityEngine.Object.Instantiate<ParticleSystem>(this.prefabArmageddonFx);
        }
        this.killAllFx.transform.position = new Vector3(this.cameraTarget.transform.position.x, this.prefabArmageddonFx.transform.position.y, this.cameraTarget.transform.position.z);
        this.killAllFx.Play();
    }

    private void OpenGame()
    {
        this.SpawnStartSquad();
    }

    public void RefreshCurrentWorldNumber()
    {
        int @int;
        if (PlayerPrefs.HasKey(StaticConstants.currentWorld))
        {
            @int = PlayerPrefs.GetInt(StaticConstants.currentWorld);
        }
        else
        {
            @int = 1;
        }
        if (!this.IsWorldOpen(@int))
        {
            @int = 1;
        }
        this.currentWorldNumber = @int;
    }

    private void RefreshStartPoints()
    {
        this.startPoints = UnityEngine.Object.FindObjectsOfType<StartPoint>();
        foreach (StartPoint point in this.startPoints)
        {
            if (point.worldNumber < 1)
            {
                this.tutorialStartPoint = point.transform;
                break;
            }
        }
    }

    public void Reset()
    {
        Time.timeScale = 1f;
        if (this.currentGameMode == GameModes.DailyBoss)
        {
            this.RefreshCurrentWorldNumber();
        }
        if (this.currentGameMode == GameModes.Arena)
        {
            if (DataLoader.gui != null)
            {
                DataLoader.gui.Loading(true);
            }
            this.currentGameMode = GameModes.Idle;
            SceneManager.UnloadSceneAsync("Arena1");
            SpawnManager.instance.Reset();
            WavesManager.instance.Reset();
            this.operation = SceneManager.LoadSceneAsync("World" + this.currentWorldNumber, LoadSceneMode.Additive);
            base.StartCoroutine(this.WaitForScene(new Action(this.Reset)));
        }
        else
        {
            this.currentGameMode = GameModes.Idle;
            SpawnManager.instance.StopIt();
            WavesManager.instance.StopIt();
            MoneyCoinManager.instance.EndGame();
            MoneyBoxManager.instance.EndGame();
            foreach (SurvivorHuman human in this.survivors)
            {
                UnityEngine.Object.Destroy(human.gameObject);
            }
            this.survivors.Clear();
            this.squadSpawned = false;
            foreach (DropPlace place in UnityEngine.Object.FindObjectsOfType<DropPlace>())
            {
                UnityEngine.Object.Destroy(place.gameObject);
            }
            foreach (NewSurvivor survivor in UnityEngine.Object.FindObjectsOfType<NewSurvivor>())
            {
                UnityEngine.Object.Destroy(survivor.gameObject);
            }
            foreach (ZombieHuman human2 in this.zombies)
            {
                if (human2 != null)
                {
                    UnityEngine.Object.Destroy(human2.gameObject);
                }
            }
            this.zombies.Clear();
            if (!this.squadSpawned)
            {
                this.SpawnStartSquad();
            }
            this.Points = 0f;
            this.newSurvivorsLeft = 0;
            WavesManager.instance.Start();
        }
    }

    private void SpawnStartSquad()
    {
        Vector3 zero = Vector3.zero;
        if (!this.isTutorialNow)
        {
            List<StartPoint> list = new List<StartPoint>();
            foreach (StartPoint point in this.startPoints)
            {
                if ((point.openAtLevel <= DataLoader.Instance.GetCurrentPlayerLevel()) && (point.worldNumber == instance.currentWorldNumber))
                {
                    list.Add(point);
                }
            }
            if (list.Count <= 0)
            {
                UnityEngine.Debug.LogError("Compatible places for SpawnStartSquad Not Found!");
            }
            else
            {
                int num2 = UnityEngine.Random.Range(0, list.Count);
                zero = list[num2].transform.position;
                this.startPoint = list[num2];
            }
        }
        else
        {
            zero = this.tutorialStartPoint.position;
        }
        if (this.isTutorialNow)
        {
            this.protectDome.gameObject.SetActive(false);
        }
        else
        {
            this.protectDome.position = zero;
            this.protectDome.gameObject.SetActive(true);
        }
        DataLoader.Instance.startSquad = new int[Enum.GetValues(typeof(SaveData.HeroData.HeroType)).Length];
        for (int i = 0; i < DataLoader.Instance.survivors.Count; i++)
        {
            if (DataLoader.playerData.IsHeroOpened(DataLoader.Instance.survivors[i].heroType))
            {
                float num4 = UnityEngine.Random.Range((float) -0.5f, (float) 0.5f);
                float num5 = UnityEngine.Random.Range((float) -0.5f, (float) 0.5f);
                Quaternion rotation = new Quaternion();
                UnityEngine.Object.Instantiate<SurvivorHuman>(DataLoader.Instance.GetSurvivorPrefab(DataLoader.Instance.survivors[i].heroType), new Vector3(zero.x + num4, zero.y, zero.z + num5), rotation, TransformParentManager.Instance.heroes);
                DataLoader.Instance.startSquad[i]++;
            }
        }
        this.cameraTarget.transform.position = zero;
        this.cameraTarget.enabled = true;
    }

    private void Start()
    {
        this.RefreshCurrentWorldNumber();
        this.operation = SceneManager.LoadSceneAsync("World" + this.currentWorldNumber, LoadSceneMode.Additive);
        base.StartCoroutine(this.WaitForScene(new Action(this.OpenGame)));
    }

    private void StartArena()
    {
        this.SpawnStartSquad();
        this.protectDome.gameObject.SetActive(false);
        this.Points = 0f;
        this.newSurvivorsLeft = 0;
        this.inGameTime = 0;
        base.InvokeRepeating("IncreaseGameTime", 0f, 1f);
        DataLoader.Instance.ResetLocalInfo();
        this.CalculateAllHeroPowers();
        UIStreak.instance.ResetStreak();
        this.currentArenaMaxPoints = 0x83;
        this.currentOpponentPoints = UnityEngine.Random.Range(0x2f, 120);
        this.OpponentPoints = 0f;
        this.timeLeft = 60;
        DataLoader.gui.RefreshArenaProgresses();
        UnityEngine.Debug.Log(this.currentOpponentPoints);
    }

    public void StartArenaTimer()
    {
        this.arenaProgress = base.StartCoroutine(this.ArenaProgress());
    }

    [DebuggerHidden]
    private IEnumerator WaitForScene(Action callback) => 
        new <WaitForScene>c__Iterator0 { 
            callback = callback,
            $this = this
        };

    public GameModes currentGameMode { get; private set; }

    public float allHeroPowers { get; private set; }

    [CompilerGenerated]
    private sealed class <ArenaProgress>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal GameManager $this;
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
                    break;

                case 1:
                    this.$this.OpponentPoints += ((this.$this.currentOpponentPoints - this.$this.OpponentPoints) * UnityEngine.Random.Range((float) 0f, (float) 2f)) / ((float) this.$this.timeLeft);
                    this.$this.timeLeft--;
                    DataLoader.gui.RefreshArenaProgresses();
                    break;

                default:
                    goto Label_00DB;
            }
            if (this.$this.timeLeft > 0)
            {
                this.$current = new WaitForSeconds(1f);
                if (!this.$disposing)
                {
                    this.$PC = 1;
                }
                return true;
            }
            Time.timeScale = 0f;
            this.$this.GameOver();
            this.$PC = -1;
        Label_00DB:
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
    private sealed class <WaitForScene>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal Action callback;
        internal GameManager $this;
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
                    if (!this.$this.operation.isDone)
                    {
                        this.$current = null;
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                    }
                    else
                    {
                        this.$current = new WaitForSeconds(0.5f);
                        if (!this.$disposing)
                        {
                            this.$PC = 2;
                        }
                    }
                    return true;

                case 2:
                    this.$this.RefreshStartPoints();
                    this.callback();
                    DataLoader.gui.Loading(false);
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

    public enum GameModes
    {
        Idle,
        GamePlay,
        Arena,
        DailyBoss
    }
}

