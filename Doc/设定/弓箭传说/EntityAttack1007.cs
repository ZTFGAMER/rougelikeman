using Dxx.Util;
using System;
using System.Runtime.CompilerServices;

public class EntityAttack1007 : EntityAttackBase
{
    private ActionBattle action = new ActionBattle();
    private bool bInstall;
    protected float delaytime = 0.65f;
    private RedLinesCtrl mRedLinesCtrl;

    private void AttackEnd()
    {
        base.SetIsEnd(true);
        this.AttackStart();
        base.UnInstall();
    }

    private void AttackStart()
    {
        base.m_Entity.m_AttackCtrl.OnMoveStart(base.m_AttackData);
        base.m_Entity.m_Weapon.SetTarget(base.m_Entity.m_HatredTarget);
    }

    protected override void DeInit()
    {
        this.RedLineDeInit();
    }

    public override void Install()
    {
    }

    protected override void OnInit()
    {
        <OnInit>c__AnonStorey0 storey = new <OnInit>c__AnonStorey0 {
            $this = this
        };
        base.m_Entity.ChangeWeapon(base.AttackID);
        this.mRedLinesCtrl = new RedLinesCtrl();
        this.mRedLinesCtrl.Init(base.m_Entity, this.ThroughWall, this.ReboundCount, this.count, this.perangle);
        this.action.Init(base.m_Entity);
        storey.skillname = base.m_Entity.m_AniCtrl.GetAnimationValue("Skill");
        base.m_Entity.m_AniCtrl.SetString("Skill", "AttackPrevReady");
        base.m_Entity.m_AniCtrl.SendEvent("Skill", false);
        base.SetIsEnd(false);
        this.action.AddActionWait(GameLogic.Random(this.linetimemax, this.linetimemax));
        this.action.AddActionDelegate(new Action(storey.<>m__0));
        this.action.AddActionDelegate(new Action(this.AttackEnd));
        this.bInstall = true;
        base.OnUnInstall = new Action(this.OnUnInstalls);
    }

    private void OnUnInstalls()
    {
        this.action.DeInit();
        if (this.bInstall)
        {
            this.bInstall = false;
            if (base.m_Entity.m_AttackCtrl != null)
            {
                base.m_Entity.m_AttackCtrl.OnMoveEnd(base.m_AttackData);
            }
            base.m_Entity.m_AniCtrl.SendEvent("Idle", false);
            this.RedLineDeInit();
        }
    }

    private void RedLineDeInit()
    {
        if (this.mRedLinesCtrl != null)
        {
            this.mRedLinesCtrl.DeInit();
            this.mRedLinesCtrl = null;
        }
    }

    public override void SetData(object[] args)
    {
    }

    protected override void UpdateProcess(float delta)
    {
        base.UpdateProcess(delta);
        this.delaytime -= Updater.delta;
        if (this.delaytime > 0f)
        {
            base.UpdateAttackAngle();
            base.m_Entity.m_AttackCtrl.OnMoving(base.m_AttackData);
            if (this.mRedLinesCtrl != null)
            {
                this.mRedLinesCtrl.Update();
            }
        }
        else
        {
            this.RedLineDeInit();
        }
    }

    protected virtual float linetimemin =>
        1f;

    protected virtual float linetimemax =>
        1f;

    protected virtual int count =>
        1;

    protected virtual float perangle =>
        0f;

    protected virtual int ReboundCount =>
        0;

    protected virtual bool ThroughWall =>
        false;

    [CompilerGenerated]
    private sealed class <OnInit>c__AnonStorey0
    {
        internal string skillname;
        internal EntityAttack1007 $this;

        internal void <>m__0()
        {
            this.$this.m_Entity.m_AniCtrl.SetString("Skill", this.skillname);
        }
    }
}

