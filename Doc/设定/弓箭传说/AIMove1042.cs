using Dxx.Util;
using System;
using UnityEngine;

public class AIMove1042 : AIMoveBase
{
    private Vector3 startpos;
    private Vector3 endpos;
    private int range;
    private float movetime;
    private float starttime;
    private float percent;
    private float percentby;
    private Vector3 dir;
    private float alldis;
    private float perdis;
    private float moveby;
    private float startattacktime;
    private bool bStartAttack;
    private int bulletindex;

    public AIMove1042(EntityBase entity, int range) : base(entity)
    {
        this.perdis = 1.5f;
        this.range = range;
    }

    private void Attack()
    {
        BulletBase base2 = GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 0x13b8);
        BulletTransmit bullet = new BulletTransmit(base.m_Entity, 0x13b8, true);
        base2.SetBulletAttribute(bullet);
        base2.transform.SetParent(base.m_Entity.m_Body.LeftWeapon.transform);
        base2.transform.localPosition = Vector3.zero;
        base2.transform.localRotation = Quaternion.identity;
        base2.transform.localScale = Vector3.one;
    }

    private void AttackGround()
    {
        this.bulletindex++;
        float rota = (base.m_Entity.eulerAngles.y + GameLogic.Random((float) -45f, (float) 45f)) + (((this.bulletindex % 2) != 0) ? -90f : 90f);
        GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 0x13b6, base.m_Entity.position + new Vector3(0f, 1f, 0f), rota);
    }

    protected override void OnEnd()
    {
        base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("Skill", 0.35f);
        base.m_Entity.OnSkillActionEnd = (Action) Delegate.Remove(base.m_Entity.OnSkillActionEnd, new Action(this.End));
    }

    protected override void OnInitBase()
    {
        this.bulletindex = GameLogic.Random(0, 2);
        this.bStartAttack = false;
        this.moveby = 0f;
        this.starttime = 0f;
        this.startpos = base.m_Entity.position;
        this.endpos = base.m_Entity.m_HatredTarget.position;
        Vector3 vector = this.endpos - this.startpos;
        this.dir = vector.normalized;
        this.alldis = 15f;
        this.movetime = this.alldis / 30f;
        this.endpos = this.startpos + (this.dir * this.alldis);
        base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("Skill", -0.35f);
        base.m_Entity.m_AniCtrl.SetString("Skill", "ClawAttack");
        base.m_Entity.m_AniCtrl.SendEvent("Skill", false);
        base.m_Entity.m_AttackCtrl.RotateHero(Utils.getAngle(this.endpos - this.startpos));
        base.m_Entity.OnSkillActionEnd = (Action) Delegate.Combine(base.m_Entity.OnSkillActionEnd, new Action(this.End));
    }

    protected override void OnUpdate()
    {
        if (this.starttime == 0f)
        {
            if (base.m_Entity.m_AttackCtrl.RotateOver())
            {
                this.starttime = Updater.AliveTime + 0.5f;
                this.startattacktime = this.starttime + 0.2f;
            }
        }
        else if ((this.starttime > 0f) && (Updater.AliveTime >= this.starttime))
        {
            if (!this.bStartAttack && (Updater.AliveTime >= this.startattacktime))
            {
                this.bStartAttack = true;
                this.Attack();
            }
            this.moveby += (Updater.delta / this.movetime) * this.alldis;
            if (this.moveby >= this.perdis)
            {
                this.moveby -= this.perdis;
                this.AttackGround();
            }
            this.percentby = ((Updater.AliveTime - this.starttime) / this.movetime) - this.percent;
            this.percent = (Updater.AliveTime - this.starttime) / this.movetime;
            this.percent = MathDxx.Clamp01(this.percent);
            base.m_Entity.SetPositionBy((this.endpos - this.startpos) * this.percentby);
            if (this.percent == 1f)
            {
                this.starttime = -1f;
            }
        }
    }
}

