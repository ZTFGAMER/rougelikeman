using System;
using UnityEngine;
using UnityEngine.UI;

public class GuideOneCtrl : MonoBehaviour
{
    private RectTransform container;
    private Transform particle;
    private ParticleSystem ring;
    private float ringStartSize;
    private RectTransform arrow;
    private RectTransform effect;
    private HighLightMask mHighLight;
    private RectTransform target;
    private float arrowy;

    private void Awake()
    {
        this.mHighLight = base.transform.GetComponentInChildren<HighLightMask>();
        this.effect = base.transform.Find("effect") as RectTransform;
        this.container = this.effect.Find("container") as RectTransform;
        this.particle = this.effect.Find("particle");
        this.ring = this.particle.Find("Ring").GetComponent<ParticleSystem>();
        this.ringStartSize = this.ring.main.startSizeMultiplier;
        this.arrow = this.container.Find("arrow") as RectTransform;
    }

    private float GetSizeMax(Vector2 size) => 
        ((size.x <= size.y) ? size.y : size.x);

    public void Init(RectTransform target)
    {
        this.target = target;
        this.mHighLight.SetTarget(target);
        this.ring.Clear();
        float sizeMax = this.GetSizeMax(target.sizeDelta);
        this.arrow.anchoredPosition = new Vector3(0f, sizeMax / 2f, 0f);
        this.container.sizeDelta = target.sizeDelta;
        this.container.anchoredPosition = Vector2.zero;
        this.ring.main.startSizeMultiplier = (this.ringStartSize * sizeMax) / 200f;
        this.effect.position = target.position;
    }

    private void Update()
    {
        if ((this.effect != null) && (this.target != null))
        {
            this.effect.position = this.target.position;
        }
    }
}

