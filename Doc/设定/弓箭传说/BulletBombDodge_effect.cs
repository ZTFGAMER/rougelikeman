using System;
using UnityEngine;

public class BulletBombDodge_effect : MonoBehaviour
{
    public ParticleSystem effect01;
    public ParticleSystem effect02;
    public ParticleSystem effect03;
    public ParticleSystem effect04;
    private Renderer[] renderers;
    private short[] counts01;
    private ParticleSystem.MinMaxCurve curve02_init;
    private ParticleSystem.MinMaxCurve curve02;
    private ParticleSystem.MinMaxCurve curve04_init;
    private ParticleSystem.MinMaxCurve curve04;

    private void Awake()
    {
        this.counts01 = new short[this.effect01.emission.burstCount];
        for (int i = 0; i < this.counts01.GetLength(0); i++)
        {
            this.counts01[i] = this.effect01.emission.GetBurst(i).maxCount;
        }
        this.curve02 = new ParticleSystem.MinMaxCurve();
        this.curve02.constant = this.effect02.emission.rateOverTime.constant;
        this.curve02_init = new ParticleSystem.MinMaxCurve();
        this.curve02_init.constant = this.curve02.constant;
        this.curve04 = new ParticleSystem.MinMaxCurve();
        this.curve04.constant = this.effect04.emission.rateOverTime.constant;
        this.curve04_init = new ParticleSystem.MinMaxCurve();
        this.curve04_init.constant = this.curve04.constant;
        this.renderers = base.transform.GetComponentsInChildren<Renderer>(true);
    }

    private void setcount(ParticleSystem.EmissionModule emission, short[] counts, float scale)
    {
        ParticleSystem.Burst[] bursts = new ParticleSystem.Burst[emission.burstCount];
        emission.GetBursts(bursts);
        for (int i = 0; i < bursts.Length; i++)
        {
            bursts[i].maxCount = (short) (counts[i] * scale);
        }
        emission.SetBursts(bursts);
    }

    public void SetScale(float value)
    {
        int num = (int) -base.transform.position.z;
        for (int i = 0; i < this.renderers.Length; i++)
        {
            this.renderers[i].sortingLayerName = "Player";
            this.renderers[i].sortingOrder = num;
        }
        this.setcount(this.effect01.emission, this.counts01, value);
        this.curve02.constant = this.curve02_init.constant * value;
        this.effect02.emission.rateOverTime = this.curve02;
        this.curve04.constant = this.curve04_init.constant * value;
        this.effect04.emission.rateOverTime = this.curve04;
    }
}

