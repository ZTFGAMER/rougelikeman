using System;
using UnityEngine;

public class BattleBossHPCtrl : MonoBehaviour
{
    private const string BossAnimationName = "BossHPShow";
    public GameObject child;
    public RectTransform BossHP_FG;
    public RectTransform BossHP_FGReduce;
    public RectTransform BossHP_FGReduce1;
    public Animation Ani_Boss;
    private BattleUIBossHPCtrl mBossHPCtrl;
    private int BossHPWidth;
    private bool bShow = true;

    private void Awake()
    {
        if (this.BossHP_FG != null)
        {
            this.BossHPWidth = (int) this.BossHP_FG.sizeDelta.x;
        }
    }

    public void DeInit()
    {
        if (this.mBossHPCtrl != null)
        {
            this.mBossHPCtrl.DeInit();
        }
    }

    public void Init()
    {
        if (this.mBossHPCtrl == null)
        {
            this.mBossHPCtrl = new BattleUIBossHPCtrl();
            this.mBossHPCtrl.Init(this.BossHP_FGReduce1, this.BossHP_FGReduce, this.BossHPWidth);
        }
    }

    public bool IsShow() => 
        this.bShow;

    public void Show(bool show)
    {
        if (this.bShow != show)
        {
            this.bShow = show;
            if (show)
            {
                this.child.SetActive(true);
                this.Ani_Boss.Play("BossHPShow");
            }
            else if (!show)
            {
                this.child.SetActive(false);
            }
        }
    }

    public void UpdateBossHP(float value)
    {
        this.Init();
        if (value > 0f)
        {
            float x = this.BossHPWidth * value;
            Vector2 sizeDelta = this.BossHP_FG.sizeDelta;
            this.BossHP_FG.sizeDelta = new Vector2(x, sizeDelta.y);
            this.mBossHPCtrl.Reduce(x);
        }
        else
        {
            this.Show(false);
        }
    }
}

