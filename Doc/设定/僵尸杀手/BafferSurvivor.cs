using System;
using UnityEngine;

public class BafferSurvivor : SurvivorHuman
{
    [SerializeField]
    private float bafDistance = 4f;
    [SerializeField]
    private float bafValue = 2f;
    [SerializeField]
    public float bafDelay = 1f;
    [SerializeField]
    private ParticleSystem bafferFx;
    private SurvivorHuman targetSurvivor;

    private void DoIt()
    {
        if ((this.targetSurvivor == null) || this.targetSurvivor.isBaffed)
        {
            this.targetSurvivor = this.FindTargetSurvivor();
        }
        if ((this.targetSurvivor != null) && !this.targetSurvivor.isBaffed)
        {
            this.targetSurvivor.TakeBaf(this.bafValue);
            this.bafferFx.Play();
            SoundManager.Instance.PlayBuffSound();
            base.Invoke("RotateForward", 0.5f);
        }
        else
        {
            base.RotateForward();
        }
        base.Invoke("DoIt", this.bafDelay);
    }

    private SurvivorHuman FindTargetSurvivor()
    {
        SurvivorHuman human = null;
        foreach (SurvivorHuman human2 in GameManager.instance.survivors)
        {
            if ((((human2 != this) && (human2.heroType != SaveData.HeroData.HeroType.COOK)) && ((human2.heroType != SaveData.HeroData.HeroType.MEDIC) && (Vector3.Distance(human2.transform.position, base.transform.position) <= this.bafDistance))) && ((human == null) || (Vector3.Distance(human2.transform.position, base.transform.position) < Vector3.Distance(human.transform.position, base.transform.position))))
            {
                SurvivorHuman human3 = human2;
                if (!human3.isBaffed)
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
        this.bafDelay = 1f / DataLoader.Instance.GetHeroDamage(base.heroType);
        base.Invoke("DoIt", this.bafDelay);
    }
}

