using Dxx.Util;
using System;
using UnityEngine;
using UnityEngine.UI;

public class TipsCtrl : MonoBehaviour
{
    private Text text1;
    private Text text2;
    private Animation ani;
    private float time;
    private bool bPlayMiss;
    private bool bCanShowNext;

    private void Awake()
    {
        this.text1 = base.transform.Find("Image_BG/Text1").GetComponent<Text>();
        this.text2 = base.transform.Find("Image_BG/Text2").GetComponent<Text>();
        this.ani = base.GetComponent<Animation>();
    }

    public void Init(string value1, string value2)
    {
        this.text1.text = value1;
        this.text2.text = value2;
        this.time = 0f;
        this.bPlayMiss = false;
        this.bCanShowNext = false;
        this.ani.Play("TipsOne");
        GameLogic.Hold.Sound.PlayUI(0xf4247);
    }

    private void OnDisable()
    {
        this.ani.enabled = false;
    }

    private void OnEnable()
    {
        this.ani.enabled = true;
    }

    private void Update()
    {
        this.time += Updater.delta;
        if ((this.time >= 3f) && !this.bPlayMiss)
        {
            this.bPlayMiss = true;
            this.ani.Play("TipsOne_Miss");
        }
        if (this.bPlayMiss)
        {
            if ((this.time >= 3.2f) && !this.bCanShowNext)
            {
                this.bCanShowNext = true;
                CInstance<TipsManager>.Instance.CanShowNext();
            }
            if (this.time >= 3.5f)
            {
                this.bPlayMiss = false;
                CInstance<TipsManager>.Instance.Cache(base.gameObject);
            }
        }
    }
}

