using DG.Tweening;
using Dxx.Util;
using PureMVC.Patterns;
using System;
using UnityEngine;
using UnityEngine.UI;

public class BattleExpCtrl : MonoBehaviour
{
    private const string ExpAnimationName = "HeroExpShow";
    private const string ExpScaleName = "HeroExpScale";
    public Text Text_Level;
    public Animation Ani_Exp;
    public RectTransform Exp_Add;
    public RectTransform Exp_Add1;
    public RectTransform Exp_BG;
    public RectTransform Exp_FG;
    private RectTransform rectTransform;
    private int ExpWidth;
    private int ExpBGWidth;
    private BattleUIBossHPCtrl mBossHPCtrl;
    private ActionUpdateCtrl mActionUpdateCtrl;
    private bool bDropExp = true;
    private SequencePool mSequencePool = new SequencePool();

    private void Awake()
    {
        this.rectTransform = base.transform as RectTransform;
        if (this.Exp_FG != null)
        {
            this.ExpWidth = (int) this.Exp_FG.sizeDelta.x;
        }
        if (this.Exp_BG != null)
        {
            this.ExpBGWidth = (int) this.Exp_BG.sizeDelta.x;
        }
    }

    public void DeInit()
    {
        if (this.mActionUpdateCtrl != null)
        {
            this.mActionUpdateCtrl.DeInit();
        }
        this.mSequencePool.Clear();
    }

    public void ExpUP(ProgressAniManager vo)
    {
        vo.SetUpdate(new Action<ProgressAniManager.ProgressTransfer>(this.update_ui));
    }

    public void Init()
    {
        Vector2 sizeDelta = this.Exp_FG.sizeDelta;
        this.Exp_FG.sizeDelta = new Vector2(0f, sizeDelta.y);
        this.Exp_Add.sizeDelta = new Vector2(0f, sizeDelta.y);
        this.Exp_Add1.sizeDelta = new Vector2(0f, sizeDelta.y);
        this.Exp_BG.sizeDelta = new Vector2(0f, this.Exp_BG.sizeDelta.y);
        this.Exp_FG.localScale = Vector3.zero;
        this.Text_Level.text = string.Empty;
        Sequence seq = TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(DOTween.Sequence(), 0.5f), new TweenCallback(this, this.<Init>m__0));
        this.mSequencePool.Add(seq);
        this.mActionUpdateCtrl = new ActionUpdateCtrl();
        this.mActionUpdateCtrl.Init(false);
    }

    private void set_progress(float value)
    {
        this.Exp_FG.sizeDelta = new Vector2(value * this.ExpWidth, this.Exp_FG.sizeDelta.y);
    }

    public void SetDropExp(bool drop)
    {
        this.bDropExp = drop;
    }

    public void SetFringe()
    {
        RectTransform transform = this.Ani_Exp.transform as RectTransform;
        transform.anchoredPosition = new Vector2(transform.anchoredPosition.x, transform.anchoredPosition.y + PlatformHelper.GetFringeHeight());
    }

    public void SetLevel(int level)
    {
        if (this.Text_Level != null)
        {
            object[] args = new object[] { level };
            this.Text_Level.text = Utils.FormatString("Lv.{0}", args);
            if (((GameLogic.Self != null) && (GameLogic.Self.m_EntityData != null)) && (level == GameLogic.Self.m_EntityData.MaxLevel))
            {
                this.Text_Level.text = "Lv.MAX";
            }
        }
    }

    public void Show(bool show)
    {
        if (!this.bDropExp)
        {
            if (this.Ani_Exp.gameObject.activeSelf)
            {
                this.Ani_Exp.gameObject.SetActive(false);
            }
        }
        else if (show)
        {
            this.Ani_Exp.gameObject.SetActive(true);
        }
        else if (!show)
        {
            this.Ani_Exp.gameObject.SetActive(false);
            this.Ani_Exp.Play("HeroExpShow");
        }
    }

    private void update_ui(ProgressAniManager.ProgressTransfer data)
    {
        this.SetLevel(data.currentlevel);
        this.set_progress(data.percent);
        if (!data.isend)
        {
            float x = (Random.Range(0, 2) * 2) - 1;
            float y = (Random.Range(0, 2) * 2) - 1;
            this.rectTransform.anchoredPosition = new Vector2(x, y) * 2f;
        }
        else
        {
            this.rectTransform.anchoredPosition = Vector2.zero;
        }
        if (data.islevelup)
        {
            Facade.Instance.SendNotification("BATTLE_LEVEL_UP");
        }
    }
}

