using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class WavesManager : MonoBehaviour
{
    public static WavesManager instance;
    [HideInInspector]
    public List<ZombiesSpawn> zombiesSpawns = new List<ZombiesSpawn>();
    public List<ZombieRank> zombies;
    public List<int> chances;
    [SerializeField]
    public int maxCountZombies = 40;
    [SerializeField]
    private float[] spawnDelayByLevel;
    [SerializeField]
    private float[] firstWavePowerByLevel;
    [SerializeField]
    private SFWNBL[] startFromWaveNumberByWorld;
    private float firstWavePower = 10f;
    [SerializeField]
    private float scale1 = 1f;
    [SerializeField]
    private float scale2 = 1f;
    [SerializeField]
    private Transform cameraTarget;
    [SerializeField]
    private float staticSpawnDelay = 5f;
    [Space, SerializeField]
    public Bosses[] bossesByWorld;
    [SerializeField]
    private float bossDelayMultiplier = 2f;
    [Space, SerializeField]
    private Transform dailyBossSpawnPoint;
    [Space]
    public float spawnSmooth = 0.1f;
    private int bossesSpawned;
    private float bossWavesDelayMultiplier = 1f;
    private int bossDeadAtPoints;
    [HideInInspector]
    public bool bossInDaHause;
    private float minDistance = 20f;
    [Space]
    public int currentWave = -1;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private float <maxCurrentWavePower>k__BackingField;

    private void Awake()
    {
        instance = this;
    }

    public void BossDead()
    {
        this.bossDeadAtPoints = (int) GameManager.instance.Points;
        this.bossWavesDelayMultiplier = 1f;
        this.bossInDaHause = false;
        DataLoader.gui.SetTopPanelAnimationState(this.bossInDaHause);
    }

    [DebuggerHidden]
    public IEnumerator DelayedBossSpawn(Vector3 position, int currentBossCount) => 
        new <DelayedBossSpawn>c__Iterator0 { 
            position = position,
            currentBossCount = currentBossCount,
            $this = this
        };

    private void FindBestSpawnAndDoIt(bool onlyBoss = false)
    {
        float[] numArray = new float[this.zombiesSpawns.Count];
        for (int i = 0; i < this.zombiesSpawns.Count; i++)
        {
            numArray[i] = Vector3.Distance(this.cameraTarget.position, this.zombiesSpawns[i].transform.position);
        }
        for (int j = 0; j < numArray.Length; j++)
        {
            for (int m = j; m < numArray.Length; m++)
            {
                if (numArray[m] < numArray[j])
                {
                    float num4 = numArray[m];
                    numArray[m] = numArray[j];
                    numArray[j] = num4;
                    ZombiesSpawn spawn = this.zombiesSpawns[m];
                    this.zombiesSpawns[m] = this.zombiesSpawns[j];
                    this.zombiesSpawns[j] = spawn;
                }
            }
        }
        for (int k = 0; k < (numArray.Length - 2); k++)
        {
            if ((!this.zombiesSpawns[k].isIdleSpawn && (numArray[k] > this.minDistance)) && ((this.zombiesSpawns[k].openAtLevel <= DataLoader.Instance.GetCurrentPlayerLevel()) && (this.zombiesSpawns[k].worldNumber == GameManager.instance.currentWorldNumber)))
            {
                int num6;
                do
                {
                    num6 = k + UnityEngine.Random.Range(0, 3);
                }
                while ((this.zombiesSpawns[num6].isIdleSpawn || (this.zombiesSpawns[num6].openAtLevel > DataLoader.Instance.GetCurrentPlayerLevel())) || (this.zombiesSpawns[num6].worldNumber != GameManager.instance.currentWorldNumber));
                if (onlyBoss)
                {
                    base.StartCoroutine(this.DelayedBossSpawn(new Vector3(this.zombiesSpawns[num6].transform.position.x + UnityEngine.Random.Range((float) -1f, (float) 2f), this.zombiesSpawns[num6].transform.position.y, this.zombiesSpawns[num6].transform.position.z + UnityEngine.Random.Range((float) -1f, (float) 2f)), this.bossesSpawned));
                }
                else
                {
                    this.zombiesSpawns[num6].Spawn();
                }
                break;
            }
        }
    }

    public int GetBossDeadAtPoints() => 
        this.bossDeadAtPoints;

    public int GetNextBossTargetPoints()
    {
        if (this.bossesByWorld[GameManager.instance.currentWorldNumber - 1].bosses.Length > this.bossesSpawned)
        {
            return (this.bossesByWorld[GameManager.instance.currentWorldNumber - 1].bosses[this.bossesSpawned].pointsToGo + this.bossesByWorld[GameManager.instance.currentWorldNumber - 1].startPointsToGoByLevel[DataLoader.Instance.GetCurrentPlayerLevel()]);
        }
        return 0;
    }

    public void Reset()
    {
        this.zombiesSpawns.Clear();
    }

    public void SpawnDailyBoss()
    {
        base.CancelInvoke("SpawnStaticWave");
        string str = "Boss1";
        foreach (Boss boss in this.bossesByWorld[GameManager.instance.currentWorldNumber - 1].bosses)
        {
            if (boss.prefabBoss.name == str)
            {
                UnityEngine.Object.Instantiate<BossZombie>(boss.prefabBoss, this.dailyBossSpawnPoint.position, boss.prefabBoss.transform.rotation);
                this.firstWavePower = this.firstWavePowerByLevel[DataLoader.Instance.GetCurrentPlayerLevel()];
                this.currentWave = this.startFromWaveNumberByWorld[GameManager.instance.currentWorldNumber - 1].startFromWaveNumberByLevel[DataLoader.Instance.GetCurrentPlayerLevel()] - 1;
                this.maxCurrentWavePower = this.firstWavePower;
                base.Invoke("SpawnGroup", 1f);
                this.bossesSpawned = 0;
                this.bossWavesDelayMultiplier = 1f;
                this.bossDeadAtPoints = 0;
                this.bossInDaHause = false;
                DataLoader.gui.SetTopPanelAnimationState(this.bossInDaHause);
                return;
            }
        }
        UnityEngine.Debug.LogError("No found boss prefab with name \"" + str + "\"");
    }

    private void SpawnGroup()
    {
        if (this.zombiesSpawns.Count > 0)
        {
            this.currentWave++;
            this.maxCurrentWavePower = (this.firstWavePower * Mathf.Pow(this.scale1, (float) this.currentWave)) + (this.scale2 * this.currentWave);
            this.maxCurrentWavePower *= UnityEngine.Random.Range((float) 1f, (float) 2f);
            this.FindBestSpawnAndDoIt(false);
            base.Invoke("SpawnGroup", this.spawnDelayByLevel[DataLoader.Instance.GetCurrentPlayerLevel()] * this.bossWavesDelayMultiplier);
        }
    }

    public void SpawnStaticWave()
    {
        this.maxCurrentWavePower = 0f;
        foreach (SurvivorHuman human in GameManager.instance.survivors)
        {
            this.maxCurrentWavePower += DataLoader.Instance.GetHeroPower(human.heroType);
        }
        this.maxCurrentWavePower *= UnityEngine.Random.Range((float) 0.25f, (float) 0.35f);
        GameManager.instance.startPoint.idleZombieSpawns[UnityEngine.Random.Range(0, GameManager.instance.startPoint.idleZombieSpawns.Length)].Spawn();
        base.Invoke("SpawnStaticWave", this.staticSpawnDelay);
    }

    public void SpawnTutorialWave(float wavePower, int waveNumber)
    {
        this.currentWave = 0;
        this.maxCurrentWavePower = wavePower;
        foreach (ZombiesSpawn spawn in this.zombiesSpawns)
        {
            if (spawn.worldNumber < 1)
            {
                spawn.Spawn();
                break;
            }
        }
    }

    public void Start()
    {
        if (!GameManager.instance.isTutorialNow)
        {
            base.Invoke("SpawnStaticWave", 1f);
        }
    }

    public void StartGame()
    {
        base.CancelInvoke("SpawnStaticWave");
        this.firstWavePower = this.firstWavePowerByLevel[DataLoader.Instance.GetCurrentPlayerLevel()];
        this.currentWave = this.startFromWaveNumberByWorld[GameManager.instance.currentWorldNumber - 1].startFromWaveNumberByLevel[DataLoader.Instance.GetCurrentPlayerLevel()] - 1;
        this.maxCurrentWavePower = this.firstWavePower;
        base.Invoke("SpawnGroup", 1f);
        this.bossesSpawned = 0;
        this.bossWavesDelayMultiplier = 1f;
        this.bossDeadAtPoints = 0;
        this.bossInDaHause = false;
        DataLoader.gui.SetTopPanelAnimationState(this.bossInDaHause);
    }

    public void StopIt()
    {
        base.CancelInvoke("SpawnStaticWave");
        base.CancelInvoke("SpawnGroup");
    }

    public void TrySpawnBoss()
    {
        if ((!this.bossInDaHause && (this.bossesByWorld[GameManager.instance.currentWorldNumber - 1].bosses.Length > this.bossesSpawned)) && ((this.GetNextBossTargetPoints() <= (GameManager.instance.Points - this.bossDeadAtPoints)) && !GameManager.instance.isTutorialNow))
        {
            if (BossSpawnManager.instance.GetSpawnPosition(out Vector3 vector))
            {
                base.StartCoroutine(this.DelayedBossSpawn(vector, this.bossesSpawned));
                this.bossWavesDelayMultiplier = this.bossDelayMultiplier;
                this.bossesSpawned++;
                this.bossInDaHause = true;
                DataLoader.gui.SetTopPanelAnimationState(this.bossInDaHause);
            }
            else
            {
                this.FindBestSpawnAndDoIt(true);
                this.bossWavesDelayMultiplier = this.bossDelayMultiplier;
                this.bossesSpawned++;
                this.bossInDaHause = true;
                DataLoader.gui.SetTopPanelAnimationState(this.bossInDaHause);
            }
        }
    }

    [HideInInspector]
    public float maxCurrentWavePower { get; private set; }

    [CompilerGenerated]
    private sealed class <DelayedBossSpawn>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal Vector3 position;
        internal int currentBossCount;
        internal WavesManager $this;
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
                    BossSpawnManager.instance.SpawnFx(this.position);
                    this.$current = new WaitForSeconds(0.5f);
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    return true;

                case 1:
                    UnityEngine.Object.Instantiate<BossZombie>(this.$this.bossesByWorld[GameManager.instance.currentWorldNumber - 1].bosses[this.currentBossCount].prefabBoss, this.position, Quaternion.identity, TransformParentManager.Instance.zombies);
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

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct Boss
    {
        public BossZombie prefabBoss;
        public int pointsToGo;
        public int reward;
    }

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct Bosses
    {
        public WavesManager.Boss[] bosses;
        public int[] startPointsToGoByLevel;
        public long allBossesReward;
    }

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct SFWNBL
    {
        public int[] startFromWaveNumberByLevel;
    }

    [Serializable]
    public class ZombieRank
    {
        public List<WavesManager.ZombieVariants> zombies;
        public List<int> chances;

        private int GetBalanceNum()
        {
            int max = 0;
            foreach (int num2 in this.chances)
            {
                max += num2;
            }
            int num3 = UnityEngine.Random.Range(0, max);
            int num4 = 0;
            for (int i = 0; i < this.chances.Count; i++)
            {
                num4 += this.chances[i];
                if (num3 < num4)
                {
                    return i;
                }
            }
            return 0;
        }

        public ZombieHuman GetZombie(float powerAvailable)
        {
            if (this.zombies.Count <= 0)
            {
                return null;
            }
            int balanceNum = this.GetBalanceNum();
            ZombieHuman variant = this.zombies[balanceNum].GetVariant();
            while ((this.zombies.Count > 0) && ((((GameManager.instance.currentGameMode == GameManager.GameModes.Idle) && !this.zombies[balanceNum].readyInIdle) || ((GameManager.instance.currentGameMode != GameManager.GameModes.Idle) && (this.zombies[balanceNum].minWave > WavesManager.instance.currentWave))) || (variant.power > powerAvailable)))
            {
                this.zombies.RemoveAt(balanceNum);
                this.chances.RemoveAt(balanceNum);
                if (this.zombies.Count <= 0)
                {
                    break;
                }
                balanceNum = this.GetBalanceNum();
                variant = this.zombies[balanceNum].GetVariant();
            }
            if (this.zombies.Count <= 0)
            {
                return null;
            }
            return variant;
        }
    }

    [Serializable]
    public class ZombieVariants
    {
        public List<ZombieHuman> variants;
        public int minWave;
        public bool readyInIdle = true;

        public ZombieHuman GetVariant() => 
            this.variants[UnityEngine.Random.Range(0, this.variants.Count)];
    }
}

