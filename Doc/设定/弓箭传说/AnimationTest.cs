using DG.Tweening;
using System;
using UnityEngine;

public class AnimationTest : MonoBehaviour
{
    public string AniString;
    public string AniString2;
    private Animation ani;
    private float time;

    private void Start()
    {
        this.ani = base.GetComponent<Animation>();
        float num = 0.5f;
        this.ani[this.AniString].speed = num;
        this.ani[this.AniString2].speed = num;
        this.time = this.ani[this.AniString].length / num;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            this.ani.Play(this.AniString);
            TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(DOTween.Sequence(), this.time), new TweenCallback(this, this.<Update>m__0));
        }
    }
}

