using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class ZombieHuman : BaseHuman
{
    private bool isActive;
    [SerializeField]
    private float damageDelay = 0.5f;
    [SerializeField]
    public ZombieSize size;
    protected NavMeshAgent navAgent;
    private bool canDamage = true;
    private float refindTargetTime = 1.5f;
    private float lastFindTime;
    [HideInInspector]
    public SaveData.ZombieData.ZombieType zombieType;
    [HideInInspector]
    public int damage;
    public float power;
    public float defaultSpeed;
    [SerializeField]
    private AudioClip attackSound;
    [SerializeField]
    private AudioClip[] roarSounds;
    private CameraTarget cameraTarget;

    private void ActivateMe()
    {
        if (!this.isActive)
        {
            this.isActive = true;
            base.StartCoroutine(this.Roar());
            base.animator.enabled = true;
        }
    }

    protected override void FixedUpdate()
    {
        if (((GameManager.instance.currentGameMode == GameManager.GameModes.Arena) && this.cameraTarget.enabled) && (Vector3.Distance(base.transform.position, this.cameraTarget.transform.position) < 20f))
        {
            this.ActivateMe();
        }
        if (this.isActive)
        {
            if ((base.targetMove == null) || ((Time.time - this.lastFindTime) > this.refindTargetTime))
            {
                foreach (SurvivorHuman human in GameManager.instance.survivors)
                {
                    if ((human != null) && ((base.targetMove == null) || (Vector3.Distance(base.transform.position, base.targetMove.position) > Vector3.Distance(base.transform.position, human.transform.position))))
                    {
                        base.targetMove = human.transform;
                    }
                }
                this.lastFindTime = Time.time;
            }
            if (base.targetMove != null)
            {
                this.navAgent.isStopped = false;
                float num = Vector3.Distance(base.transform.position, base.targetMove.position);
                if (num > 20f)
                {
                    this.navAgent.speed = this.defaultSpeed * 5f;
                }
                else
                {
                    this.navAgent.speed = this.defaultSpeed;
                }
                if (this.canDamage && (num < (3f + (base.transform.localScale.x / 2f))))
                {
                    base.animator.SetTrigger("Attack");
                    this.canDamage = false;
                    base.Invoke("TryDamageSurvivor", 0.45f);
                }
                this.navAgent.destination = base.targetMove.position;
            }
            else
            {
                this.navAgent.isStopped = true;
            }
            if (base.animator != null)
            {
                if (base.targetMove != null)
                {
                    base.animator.SetBool("Run", true);
                }
                else
                {
                    base.animator.SetBool("Run", false);
                }
            }
        }
    }

    private void ReadyToDamage()
    {
        this.canDamage = true;
    }

    [DebuggerHidden]
    private IEnumerator Roar() => 
        new <Roar>c__Iterator0 { $this = this };

    protected virtual void Start()
    {
        this.cameraTarget = UnityEngine.Object.FindObjectOfType<CameraTarget>();
        this.navAgent = base.GetComponent<NavMeshAgent>();
        if (base.takeDamageFx == null)
        {
            base.takeDamageFx = UnityEngine.Object.Instantiate<GameObject>(GameManager.instance.prefabTakeDamageZombie, base.animator.transform).GetComponent<ParticleSystem>();
        }
        if ((this.zombieType == SaveData.ZombieData.ZombieType.FLAG) || (this.zombieType == SaveData.ZombieData.ZombieType.CLOWN))
        {
            UnityEngine.Object.Instantiate<GameObject>(GameManager.instance.prefabRareGlowFx, base.transform);
        }
        if (GameManager.instance.currentGameMode == GameManager.GameModes.Idle)
        {
            this.defaultSpeed = this.navAgent.speed / 1.5f;
        }
        else
        {
            this.defaultSpeed = this.navAgent.speed;
        }
        GameManager.instance.zombies.Add(this);
        if (GameManager.instance.currentGameMode != GameManager.GameModes.Arena)
        {
            this.ActivateMe();
        }
        else
        {
            this.navAgent.isStopped = true;
            base.animator.enabled = false;
        }
    }

    public override int TakeDamage(int damage)
    {
        if (base.countHealth > 0)
        {
            base.TakeDamage(damage);
            if (base.countHealth <= 0)
            {
                GameManager.instance.DecreaseZombie(this);
                if (base.animator != null)
                {
                    base.animator.SetTrigger("Death_" + UnityEngine.Random.Range(1, 4).ToString());
                }
                if (base.deathSounds.Length > 0)
                {
                    SoundManager.Instance.PlaySound(base.deathSounds[UnityEngine.Random.Range(0, base.deathSounds.Length)], 0.25f);
                }
                base.animator.transform.parent = null;
                UnityEngine.Object.Destroy(base.animator.gameObject, UnityEngine.Random.Range((float) 2f, (float) 3f));
                UnityEngine.Object.Destroy(base.gameObject);
                UnityEngine.Object.Destroy(UnityEngine.Object.Instantiate<GameObject>(GameManager.instance.prefabZombieBlood, new Vector3(base.transform.position.x, GameManager.instance.prefabZombieBlood.transform.position.y, base.transform.position.z), GameManager.instance.prefabZombieBlood.transform.rotation, TransformParentManager.Instance.fx), 3.7f);
                if (GameManager.instance.currentGameMode == GameManager.GameModes.Idle)
                {
                    DataLoader.Instance.RefreshMoney((double) ((this.power * StaticConstants.IdleGoldConst) * DataLoader.Instance.dailyMultiplier), false);
                }
            }
        }
        return base.countHealth;
    }

    private void TryDamageSurvivor()
    {
        if (base.targetMove != null)
        {
            if (Vector3.Distance(base.transform.position, base.targetMove.position) < (2f + (base.transform.localScale.x / 2f)))
            {
                SoundManager.Instance.PlaySound(this.attackSound, -1f);
                base.targetMove.GetComponent<BaseHuman>().TakeDamage(this.damage);
                base.Invoke("ReadyToDamage", this.damageDelay);
            }
            else
            {
                this.canDamage = true;
            }
        }
        else
        {
            this.canDamage = true;
        }
    }

    [CompilerGenerated]
    private sealed class <Roar>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal ZombieHuman $this;
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
                    if ((((this.$this.roarSounds.Length > 0) && !SoundManager.Instance.soundIsMuted) && ((this.$this.targetMove == null) || (Vector3.Distance(this.$this.transform.position, this.$this.targetMove.position) < 20f))) && (UnityEngine.Random.value < 0.5f))
                    {
                        AudioSource.PlayClipAtPoint(this.$this.roarSounds[UnityEngine.Random.Range(0, this.$this.roarSounds.Length)], this.$this.transform.position, SoundManager.Instance.soundVolume);
                    }
                    break;

                default:
                    return false;
            }
            this.$current = new WaitForSeconds((float) UnityEngine.Random.Range(2, 7));
            if (!this.$disposing)
            {
                this.$PC = 1;
            }
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

    public enum ZombieSize
    {
        Small,
        Middle,
        Big
    }
}

