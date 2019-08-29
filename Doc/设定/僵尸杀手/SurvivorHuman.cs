using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SurvivorHuman : BaseHuman
{
    public SaveData.HeroData.HeroType heroType;
    private float baseShootDelay;
    [SerializeField]
    protected float shootDelay = 0.3f;
    [HideInInspector]
    public bool isBaffed;
    private ParticleSystem healingFx;
    private ParticleSystem takeBafFx;
    public float heroDamage;
    [SerializeField]
    protected AudioClip[] shotSounds;
    [SerializeField]
    private SkinnedMeshRenderer skinnedMeshRenderer;
    private Coroutine takeDamage;
    protected bool shootExists;
    [CompilerGenerated]
    private static Func<AnimatorControllerParameter, bool> <>f__am$cache0;

    private void EndBaf()
    {
        this.shootDelay = this.baseShootDelay;
        this.isBaffed = false;
        base.Invoke("HideHpbar", 2f);
        this.takeBafFx.Stop();
    }

    private void EndWakeUpFx()
    {
        this.healingFx.Stop();
    }

    public void PlayWakeUpFx()
    {
        if (this.healingFx == null)
        {
            this.healingFx = UnityEngine.Object.Instantiate<GameObject>(GameManager.instance.prefabHealingFx, base.transform).GetComponent<ParticleSystem>();
        }
        this.healingFx.Play();
        base.Invoke("EndWakeUpFx", 0.8f);
    }

    public bool ReadyToHeal()
    {
        if (base.maxCountHealth <= base.countHealth)
        {
            return false;
        }
        return true;
    }

    protected void RotateForward()
    {
        base.targetRotation = base.targetMove;
        if ((base.animator != null) && this.shootExists)
        {
            base.animator.SetBool("Shoot", false);
        }
    }

    public virtual void Start()
    {
        base.animator.SetBool("Rest", false);
        if (this.healingFx == null)
        {
            this.healingFx = UnityEngine.Object.Instantiate<GameObject>(GameManager.instance.prefabHealingFx, base.transform).GetComponent<ParticleSystem>();
        }
        if (base.takeDamageFx == null)
        {
            base.takeDamageFx = UnityEngine.Object.Instantiate<GameObject>(GameManager.instance.prefabTakeDamageSurvivor, base.animator.transform).GetComponent<ParticleSystem>();
        }
        if (this.takeBafFx == null)
        {
            this.takeBafFx = UnityEngine.Object.Instantiate<GameObject>(GameManager.instance.prefabBafFx, base.animator.transform).GetComponent<ParticleSystem>();
        }
        this.baseShootDelay = this.shootDelay;
        base.targetRotation = base.targetMove = UnityEngine.Object.FindObjectOfType<Test>().transform;
        GameManager.instance.survivors.Add(this);
        this.heroDamage = Mathf.RoundToInt(DataLoader.Instance.GetHeroDamage(this.heroType));
        if (GameManager.instance.currentGameMode == GameManager.GameModes.Arena)
        {
            this.heroDamage *= 10f;
        }
        if (GameManager.instance.isTutorialNow)
        {
            this.heroDamage *= 2.5f;
        }
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = a => a.name == "Shoot";
        }
        this.shootExists = Enumerable.Any<AnimatorControllerParameter>(base.animator.parameters, <>f__am$cache0);
    }

    private void StopHealAnim()
    {
        this.healingFx.Stop();
    }

    public void TakeBaf(float multiplier)
    {
        this.shootDelay /= multiplier;
        this.isBaffed = true;
        base.Invoke("EndBaf", 4f);
        this.takeBafFx.Play();
    }

    public override int TakeDamage(int damage)
    {
        if (base.countHealth > 0)
        {
            if (GameManager.instance.currentGameMode == GameManager.GameModes.Idle)
            {
                return base.countHealth;
            }
            if (GameManager.instance.isTutorialNow && (base.countHealth <= (((float) base.maxCountHealth) / 2f)))
            {
                return base.countHealth;
            }
            base.TakeDamage(damage);
            if ((damage > 0) && (this.takeDamage == null))
            {
                this.takeDamage = base.StartCoroutine(this.takeDamageColorFx());
            }
            if (base.countHealth <= 0)
            {
                if (base.animator != null)
                {
                    base.animator.SetTrigger("Death");
                }
                if (base.deathSounds.Length > 0)
                {
                    SoundManager.Instance.PlaySound(base.deathSounds[UnityEngine.Random.Range(0, base.deathSounds.Length)], -1f);
                }
                base.animator.transform.parent = null;
                UnityEngine.Object.Destroy(base.animator.gameObject, 2f);
                UnityEngine.Object.Destroy(base.gameObject);
                this.skinnedMeshRenderer.materials[1].color = new Color(0f, 0f, 0f, 0f);
                GameManager.instance.DecreaseSurvivor(this);
            }
            if (damage < 0)
            {
                if (!this.healingFx.isPlaying)
                {
                    this.healingFx.Play();
                }
                base.CancelInvoke("StopHealAnim");
                base.Invoke("StopHealAnim", 1f);
            }
            else
            {
                SoundManager.Instance.PlaySurvivorTakeDamage(this.heroType);
            }
            if (this.healingFx.isPlaying && (base.countHealth >= base.maxCountHealth))
            {
                this.healingFx.Stop();
            }
        }
        return base.countHealth;
    }

    [DebuggerHidden]
    private IEnumerator takeDamageColorFx() => 
        new <takeDamageColorFx>c__Iterator0 { $this = this };

    [CompilerGenerated]
    private sealed class <takeDamageColorFx>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal SurvivorHuman $this;
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
                    if (this.$this.skinnedMeshRenderer.materials[1].color.a < 0.5f)
                    {
                        Material material1 = this.$this.skinnedMeshRenderer.materials[1];
                        material1.color += new Color(0f, 0f, 0f, Time.deltaTime * 2f);
                        this.$current = null;
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        goto Label_014E;
                    }
                    break;

                case 2:
                    break;

                default:
                    goto Label_014C;
            }
            while (this.$this.skinnedMeshRenderer.materials[1].color.a > 0f)
            {
                Material material2 = this.$this.skinnedMeshRenderer.materials[1];
                material2.color -= new Color(0f, 0f, 0f, Time.deltaTime * 2f);
                this.$current = null;
                if (!this.$disposing)
                {
                    this.$PC = 2;
                }
                goto Label_014E;
            }
            this.$this.takeDamage = null;
            this.$PC = -1;
        Label_014C:
            return false;
        Label_014E:
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

