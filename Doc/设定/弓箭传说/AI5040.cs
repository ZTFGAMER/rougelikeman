using Dxx.Util;
using System;
using System.Runtime.CompilerServices;

public class AI5040 : AIBase
{
    private WeightRandomCount mWeightRandom = new WeightRandomCount(2, 2);
    private int randomcount;

    private void AddAttack(int attackid, float slowspeed)
    {
        <AddAttack>c__AnonStorey0 storey = new <AddAttack>c__AnonStorey0 {
            slowspeed = slowspeed,
            $this = this
        };
        base.AddAction(base.GetActionDelegate(new Action(storey.<>m__0)));
        base.AddAction(base.GetActionAttack(string.Empty, attackid, true));
        base.AddAction(base.GetActionDelegate(new Action(storey.<>m__1)));
    }

    protected override void OnAIDeInit()
    {
    }

    protected override void OnInit()
    {
        this.randomcount++;
        if (this.randomcount == 3)
        {
            this.randomcount = 0;
            this.AddAttack(0x13f7, 0f);
        }
        else
        {
            switch (this.mWeightRandom.GetRandom())
            {
                case 0:
                    this.AddAttack(0x13f8, 0f);
                    break;

                case 1:
                    this.AddAttack(0x13f9, 0f);
                    break;
            }
        }
        base.AddAction(base.GetActionWaitRandom(string.Empty, 500, 0x4b0));
        base.bReRandom = true;
    }

    protected override void OnInitOnce()
    {
        base.OnInitOnce();
    }

    [CompilerGenerated]
    private sealed class <AddAttack>c__AnonStorey0
    {
        internal float slowspeed;
        internal AI5040 $this;

        internal void <>m__0()
        {
            this.$this.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", this.slowspeed);
        }

        internal void <>m__1()
        {
            this.$this.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", -this.slowspeed);
        }
    }
}

