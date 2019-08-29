using System;
using UnityEngine;

public class HealerSurvivor : SurvivorHuman
{
    [SerializeField]
    private float healDistance = 4f;
    [SerializeField]
    private int healValue = 1;
    [SerializeField]
    public float healDelay = 1f;
    [SerializeField]
    private ParticleSystem healFx;
    private SurvivorHuman targetSurvivor;

    private void DoIt()
    {
        if ((this.targetSurvivor == null) || !this.targetSurvivor.ReadyToHeal())
        {
            this.targetSurvivor = this.FindTargetSurvivor();
        }
        if ((this.targetSurvivor != null) && this.targetSurvivor.ReadyToHeal())
        {
            this.targetSurvivor.TakeDamage(-this.healValue);
            SoundManager.Instance.PlayHealSound();
            this.healFx.Play();
        }
        else
        {
            base.RotateForward();
        }
        base.Invoke("DoIt", this.healDelay);
    }

    private SurvivorHuman FindTargetSurvivor()
    {
        SurvivorHuman human = null;
        foreach (SurvivorHuman human2 in GameManager.instance.survivors)
        {
            if (((human2 != this) && (Vector3.Distance(human2.transform.position, base.transform.position) <= this.healDistance)) && ((human == null) || (Vector3.Distance(human2.transform.position, base.transform.position) < Vector3.Distance(human.transform.position, base.transform.position))))
            {
                SurvivorHuman human3 = human2;
                if (human3.ReadyToHeal())
                {
                    base.targetRotation = human2.transform;
                    human = human3;
                }
            }
        }
        return human;
    }

    public override void Start()
    {
        base.Start();
        base.isBaffed = true;
        base.Invoke("DoIt", this.healDelay);
        this.healDelay = 1f / DataLoader.Instance.GetHeroDamage(base.heroType);
    }
}

