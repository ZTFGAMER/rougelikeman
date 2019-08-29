using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ProtestingZombie : ZombieHuman
{
    [SerializeField]
    private float spawnDistance = 5f;
    [SerializeField]
    private float spawnDelay = 1.5f;
    [SerializeField]
    private float waitTime = 1f;
    [SerializeField]
    private int spawnCount = 3;
    private ParticleSystem protestingZombieFx;
    private bool readyToSpawn = true;

    private void FixedUpdate()
    {
        base.FixedUpdate();
        if (((base.targetMove != null) && this.readyToSpawn) && (Vector3.Distance(base.transform.position, base.targetMove.position) <= this.spawnDistance))
        {
            this.readyToSpawn = false;
            base.Invoke("ReadyToSpawn", this.spawnDelay);
            base.StartCoroutine(this.SpawnZombie());
        }
    }

    private void ReadyToSpawn()
    {
        this.readyToSpawn = true;
    }

    [DebuggerHidden]
    private IEnumerator SpawnZombie() => 
        new <SpawnZombie>c__Iterator0 { $this = this };

    [CompilerGenerated]
    private sealed class <SpawnZombie>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal float <tempSpeed>__0;
        internal WavesManager.ZombieRank <zombies>__0;
        internal ProtestingZombie $this;
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
                    if (this.$this.protestingZombieFx == null)
                    {
                        this.$this.protestingZombieFx = UnityEngine.Object.Instantiate<ParticleSystem>(GameManager.instance.prefabProtestingZombieFx, this.$this.transform);
                    }
                    this.$this.protestingZombieFx.Play();
                    this.<tempSpeed>__0 = this.$this.defaultSpeed;
                    this.$this.defaultSpeed = 0f;
                    this.$current = new WaitForSeconds(this.$this.waitTime / 2f);
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    goto Label_0228;

                case 1:
                    this.<zombies>__0 = new WavesManager.ZombieRank();
                    this.<zombies>__0.zombies = new List<WavesManager.ZombieVariants>(WavesManager.instance.zombies[0].zombies);
                    this.<zombies>__0.chances = new List<int>(WavesManager.instance.zombies[0].chances);
                    for (int i = 0; i < this.$this.spawnCount; i++)
                    {
                        UnityEngine.Object.Instantiate<ParticleSystem>(GameManager.instance.prefabZombieSpawnFx, UnityEngine.Object.Instantiate<ZombieHuman>(this.<zombies>__0.GetZombie(99999f), new Vector3(this.$this.transform.position.x + UnityEngine.Random.Range((float) -0.5f, (float) 0.6f), this.$this.transform.position.y, this.$this.transform.position.z + UnityEngine.Random.Range((float) -0.5f, (float) 0.6f)), this.$this.transform.rotation).transform).Play();
                    }
                    this.$current = new WaitForSeconds(this.$this.waitTime / 2f);
                    if (!this.$disposing)
                    {
                        this.$PC = 2;
                    }
                    goto Label_0228;

                case 2:
                    this.$this.defaultSpeed = this.<tempSpeed>__0;
                    this.$PC = -1;
                    break;
            }
            return false;
        Label_0228:
            return true;
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

