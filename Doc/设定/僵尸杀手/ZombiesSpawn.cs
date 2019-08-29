using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ZombiesSpawn : PointOnMap
{
    public bool isIdleSpawn;

    private int GetBalanceNum(List<int> chances)
    {
        int max = 0;
        foreach (int num2 in chances)
        {
            max += num2;
        }
        int num3 = UnityEngine.Random.Range(0, max);
        int num4 = 0;
        for (int i = 0; i < chances.Count; i++)
        {
            num4 += chances[i];
            if (num3 < num4)
            {
                return i;
            }
        }
        return 0;
    }

    public void Spawn()
    {
        if (GameManager.instance.zombies.Count < WavesManager.instance.maxCountZombies)
        {
            base.StartCoroutine(this.SpawnProcess());
        }
    }

    [DebuggerHidden]
    private IEnumerator SpawnProcess() => 
        new <SpawnProcess>c__Iterator0 { $this = this };

    private void Start()
    {
        WavesManager.instance.zombiesSpawns.Add(this);
    }

    [CompilerGenerated]
    private sealed class <SpawnProcess>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal GameManager.GameModes <startOnMode>__0;
        internal List<WavesManager.ZombieRank> <zombies>__0;
        internal List<int> <chances>__0;
        internal float <currentWavePower>__0;
        internal int <countZombiesToMax>__0;
        internal ZombieHuman <zombie>__1;
        internal ZombieHuman <zh>__1;
        internal ZombiesSpawn $this;
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
                    this.<startOnMode>__0 = GameManager.instance.currentGameMode;
                    this.<zombies>__0 = new List<WavesManager.ZombieRank>();
                    this.<chances>__0 = new List<int>(WavesManager.instance.chances);
                    for (int i = 0; i < WavesManager.instance.zombies.Count; i++)
                    {
                        this.<zombies>__0.Add(new WavesManager.ZombieRank());
                        this.<zombies>__0[i].zombies = new List<WavesManager.ZombieVariants>(WavesManager.instance.zombies[i].zombies);
                        this.<zombies>__0[i].chances = new List<int>(WavesManager.instance.zombies[i].chances);
                    }
                    this.<currentWavePower>__0 = 0f;
                    this.<countZombiesToMax>__0 = WavesManager.instance.maxCountZombies - GameManager.instance.zombies.Count;
                    break;

                case 1:
                    if (GameManager.instance.currentGameMode == this.<startOnMode>__0)
                    {
                        break;
                    }
                    goto Label_02BD;

                default:
                    goto Label_02BD;
            }
            while ((this.<currentWavePower>__0 < WavesManager.instance.maxCurrentWavePower) && (this.<countZombiesToMax>__0 > 0))
            {
                this.<zombie>__1 = null;
                do
                {
                    int balanceNum = this.$this.GetBalanceNum(this.<chances>__0);
                    this.<zombie>__1 = this.<zombies>__0[balanceNum].GetZombie(WavesManager.instance.maxCurrentWavePower - this.<currentWavePower>__0);
                    if (this.<zombie>__1 == null)
                    {
                        this.<zombies>__0.RemoveAt(balanceNum);
                        this.<chances>__0.RemoveAt(balanceNum);
                    }
                    if (this.<zombies>__0.Count <= 0)
                    {
                        goto Label_02BD;
                    }
                }
                while (this.<zombie>__1 == null);
                this.<zh>__1 = UnityEngine.Object.Instantiate<ZombieHuman>(this.<zombie>__1, new Vector3(this.$this.transform.position.x + UnityEngine.Random.Range((float) -1f, (float) 2f), this.$this.transform.position.y, this.$this.transform.position.z + UnityEngine.Random.Range((float) -1f, (float) 2f)), new Quaternion(), TransformParentManager.Instance.zombies);
                this.<currentWavePower>__0 += this.<zh>__1.power;
                this.<countZombiesToMax>__0--;
                this.$current = new WaitForSeconds(WavesManager.instance.spawnSmooth);
                if (!this.$disposing)
                {
                    this.$PC = 1;
                }
                return true;
            }
            this.$PC = -1;
        Label_02BD:
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

