using DG.Tweening;
using Dxx.Util;
using System;
using UnityEngine;

public class AI5041 : AIBase
{
    private WeightRandomCount mWeightRandom = new WeightRandomCount(1, 2);
    private SequencePool mSeqPool = new SequencePool();
    private float yellow_prev_scale = 1f;
    private int yellow_count = 8;
    private float yellow_delay = 0.1f;
    private int blue_index;
    private float blue_startangle;
    private int red_count = 11;
    private GameObject effect_color;
    private ThunderContinueMgr.ThunderContinueReceive receive;
    private Entity3097BaseCtrl mBaseCtrl;

    private void change_color(string value)
    {
        if (this.effect_color != null)
        {
            GameLogic.EffectCache(this.effect_color);
            this.effect_color = null;
        }
        object[] args = new object[] { value };
        this.effect_color = GameLogic.EffectGet(Utils.FormatString("Effect/Monster/{0}", args));
        this.effect_color.SetParentNormal(base.m_Entity.m_Body.Body);
        this.mBaseCtrl.SetTexture(value);
        base.m_Entity.m_Body.SetTexture(value);
    }

    private bool Conditions() => 
        (base.GetIsAlive() && (base.m_Entity.m_HatredTarget != null));

    private void deinit_receive()
    {
        if (this.receive != null)
        {
            this.receive.Deinit();
            this.receive = null;
        }
    }

    protected override void OnAIDeInit()
    {
        this.deinit_receive();
        this.mSeqPool.Clear();
    }

    protected override void OnInit()
    {
        base.AddAction(base.GetActionWait("actionwaitr1", 100));
        switch (this.mWeightRandom.GetRandom())
        {
            case 0:
            {
                int num2 = 3;
                for (int i = 0; i < 3; i++)
                {
                    base.AddActionDelegate(delegate {
                        this.change_color("3097_blue");
                        this.blue_startangle = GameLogic.Random((float) 0f, (float) 360f);
                        TweenSettingsExtensions.SetLoops<Sequence>(TweenSettingsExtensions.AppendInterval(TweenSettingsExtensions.AppendCallback(this.mSeqPool.Get(), new TweenCallback(this, this.<OnInit>m__3)), 0.1f), 10);
                    });
                    if (i < (num2 - 1))
                    {
                        base.AddAction(base.GetActionWait("actionwaitr1", 0x3e8));
                    }
                }
                base.AddAction(base.GetActionWait("actionwaitr1", 500));
                break;
            }
            case 1:
            {
                int num4 = 4;
                for (int i = 0; i < num4; i++)
                {
                    base.AddAction(base.GetActionDelegate(delegate {
                        this.change_color("3097_red");
                        float num = GameLogic.Random((float) 0f, (float) 360f);
                        for (int j = 0; j < this.red_count; j++)
                        {
                            int num3 = j;
                            GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 0x438, base.m_Entity.m_Body.LeftBullet.transform.position, ((num3 * 360f) / ((float) this.red_count)) + num);
                        }
                    }));
                    if (i < (num4 - 1))
                    {
                        base.AddAction(base.GetActionWait("actionwaitr1", 600));
                    }
                }
                base.AddAction(base.GetActionWait("actionwaitr1", 500));
                break;
            }
        }
        base.bReRandom = true;
    }

    protected override void OnInitOnce()
    {
        this.mBaseCtrl = CInstance<BattleResourceCreator>.Instance.Get3097Base(base.m_Entity.m_Body.ZeroMask.transform);
        this.change_color("3097_blue");
        TweenSettingsExtensions.SetLoops<Sequence>(TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(this.mSeqPool.Get(), 3f), new TweenCallback(this, this.<OnInitOnce>m__0)), -1);
    }
}

