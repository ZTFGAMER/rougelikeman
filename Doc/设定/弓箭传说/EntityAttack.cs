using Dxx.Util;
using System;
using UnityEngine;

public class EntityAttack : EntityAttackBase
{
    private ActionBasic action = new ActionBasic();
    private bool bInstall;

    private void AttackEnd()
    {
        base.UnInstall();
    }

    private void AttackStart()
    {
        if (base.bRotate)
        {
            base.UpdateAttackAngle();
        }
        else
        {
            this.m_AttackData.angle = base.m_Entity.eulerAngles.y;
            float x = MathDxx.Sin(this.m_AttackData.angle);
            float z = MathDxx.Cos(this.m_AttackData.angle);
            this.m_AttackData.direction = new Vector3(x, 0f, z);
        }
        if (base.m_Entity.m_AttackCtrl != null)
        {
            base.m_Entity.m_AttackCtrl.OnMoveStart(base.m_AttackData);
        }
        if (base.m_Entity.m_Weapon != null)
        {
            base.m_Entity.m_Weapon.SetTarget(base.m_Entity.m_HatredTarget);
        }
    }

    protected override void DeInit()
    {
    }

    public override void Install()
    {
    }

    protected virtual void OnHatredTarget()
    {
    }

    protected override void OnInit()
    {
        this.OnHatredTarget();
        if ((base.m_Entity == null) || (base.m_Entity.m_HatredTarget == null))
        {
            base.UnInstall();
            base.AttackNotGo();
        }
        else
        {
            base.m_Entity.ChangeWeapon(base.AttackID);
            this.action.Init(false);
            this.AttackStart();
            this.bInstall = true;
            base.OnUnInstall = new Action(this.UnInstalls);
            this.AttackEnd();
        }
    }

    public override void SetData(object[] args)
    {
    }

    private void UnInstalls()
    {
        this.action.DeInit();
        if (this.bInstall)
        {
            this.bInstall = false;
            if (base.m_Entity.m_AttackCtrl != null)
            {
                base.m_Entity.m_AttackCtrl.OnMoveEnd(base.m_AttackData);
            }
        }
    }
}

