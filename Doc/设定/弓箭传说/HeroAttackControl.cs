using Dxx.Util;
using System;
using UnityEngine;

public class HeroAttackControl : AttackControl
{
    private bool bAttackUpdate = true;
    private EntityBase m_LastTarget;
    private float attackinterval;
    private float attackEndTime = -1f;
    private int findIndex;
    private long iii;
    private bool bAddAttack;
    private bool bPause;
    private GameObject m_TargetImageP;
    private GameObject m_TargetRedP;
    private Animation _TargetAni;
    private bool showTarget = true;
    private Vector3 TargetPos;

    private void AutoAttackUpdate()
    {
        if (GameLogic.Hold.BattleData.Challenge_AttackEnable() && !base.m_EntityHero.GetIsDead())
        {
            if (base.m_EntityHero.m_MoveCtrl.GetMoving())
            {
                this.MissTargetImage();
                this.ReSearchTarget();
                this.SetCurrentTarget();
            }
            else
            {
                if (!base.GetAttacking() && ((Updater.AliveTime - this.attackEndTime) > this.attackinterval))
                {
                    this.FindTarget();
                    if (base.m_EntityHero.m_HatredTarget != null)
                    {
                        base.OnMoveStart(base.m_JoyData);
                    }
                    else
                    {
                        this.MissTargetImage();
                    }
                }
                if ((base.m_EntityHero.m_HatredTarget != null) && base.RotateOver())
                {
                    base.OnMoveEnd(base.m_JoyData);
                    this.attackEndTime = Updater.AliveTime;
                }
                this.SetCurrentTarget();
                base.RotateUpdate(base.m_EntityHero.m_HatredTarget);
            }
            this.CheckCurrentTarget();
        }
    }

    private void CheckCurrentTarget()
    {
        if ((base.m_EntityHero.m_HatredTarget == null) || !GameLogic.GetCanHit(base.m_EntityHero, base.m_EntityHero.m_HatredTarget))
        {
            this.MissTargetImage();
        }
    }

    private void CreateTarget()
    {
        if (!this.showTarget || (this.m_LastTarget != base.m_EntityHero.m_HatredTarget))
        {
            this.showTarget = true;
            if ((this.m_LastTarget != base.m_EntityHero.m_HatredTarget) && (this.TargetAni != null))
            {
                this.m_LastTarget = base.m_EntityHero.m_HatredTarget;
                this.TargetAni.Play("TargetShow");
            }
            if (this.TargetImage != null)
            {
                this.TargetImage.SetActive(true);
            }
            if (this.TargetRed != null)
            {
                this.TargetRed.SetActive(true);
            }
        }
        if (base.m_EntityHero.m_HatredTarget != null)
        {
            if ((this.TargetImage != null) && (base.m_EntityHero.m_HatredTarget.m_Body != null))
            {
                this.TargetImage.transform.SetParent(base.m_EntityHero.m_HatredTarget.m_Body.transform);
                this.TargetImage.transform.localScale = Vector3.one;
                this.TargetImage.transform.localPosition = Vector3.zero;
            }
            if ((this.TargetRed != null) && (base.m_EntityHero.m_HatredTarget.m_HPSlider != null))
            {
                this.TargetRed.transform.SetParent(base.m_EntityHero.m_HatredTarget.m_HPSlider.transform);
                this.TargetRed.transform.localScale = Vector3.one;
                this.TargetRed.transform.localPosition = Vector3.zero;
            }
        }
    }

    private void FindTarget()
    {
        if ((((base.m_EntityHero.m_HatredTarget == null) || !base.m_EntityHero.m_HatredTarget.GetIsInCamera()) || base.m_EntityHero.m_HatredTarget.GetIsDead()) || ((!GameLogic.GetCanHit(base.m_EntityHero, base.m_EntityHero.m_HatredTarget) || !base.m_EntityHero.m_HatredTarget.GetColliderEnable()) && !base.GetAttacking()))
        {
            this.ReSearchTarget();
        }
    }

    private void MissTargetImage()
    {
        if ((base.m_EntityHero.m_HatredTarget == null) || (((base.m_EntityHero.m_HatredTarget != null) && (base.m_EntityHero.m_HatredTarget.GetIsDead() || !base.m_EntityHero.m_HatredTarget.gameObject.activeInHierarchy)) && ((this.TargetImage != null) && (this.TargetRed != null))))
        {
            this.TargetImage.SetActive(false);
            this.TargetRed.SetActive(false);
        }
        if ((this.TargetImage != null) && this.showTarget)
        {
            this.showTarget = false;
        }
    }

    public override void MoveEndCallBack()
    {
        base.MoveEndCallBack();
        if (base.m_EntityHero.m_HatredTarget != null)
        {
            this.m_LastTarget = base.m_EntityHero.m_HatredTarget;
        }
        base.m_EntityHero.m_HatredTarget = null;
    }

    protected override void OnDestroys()
    {
        base.OnDestroys();
        ReleaseModeManager mode = GameLogic.Release.Mode;
        mode.OnGotoNextRoom = (Action<RoomGenerateBase.Room>) Delegate.Remove(mode.OnGotoNextRoom, new Action<RoomGenerateBase.Room>(this.OnGotoNextRoom));
        if (this.TargetImage != null)
        {
            Object.Destroy(this.TargetImage);
        }
    }

    private void OnGotoNextRoom(RoomGenerateBase.Room room)
    {
        base.m_EntityHero.m_HatredTarget = null;
    }

    protected override void OnStart()
    {
        ReleaseModeManager mode = GameLogic.Release.Mode;
        mode.OnGotoNextRoom = (Action<RoomGenerateBase.Room>) Delegate.Combine(mode.OnGotoNextRoom, new Action<RoomGenerateBase.Room>(this.OnGotoNextRoom));
        this.TargetImage.SetActive(false);
        this.TargetRed.SetActive(false);
    }

    public void ReSearchTarget()
    {
        if (base.m_EntityHero.m_HatredTarget != null)
        {
            this.m_LastTarget = base.m_EntityHero.m_HatredTarget;
        }
        base.m_EntityHero.m_HatredTarget = GameLogic.Release.Entity.FindTargetInCamera();
        if (base.m_EntityHero.m_HatredTarget != null)
        {
            base.m_EntityHero.m_HatredTarget.m_Body.SetTarget(true);
        }
        if ((base.m_EntityHero.m_HatredTarget != this.m_LastTarget) && (this.m_LastTarget != null))
        {
            this.m_LastTarget.m_Body.SetTarget(false);
        }
    }

    public override void Reset()
    {
    }

    private void SetCurrentTarget()
    {
        if (GameLogic.GetCanHit(base.m_EntityHero, base.m_EntityHero.m_HatredTarget))
        {
            this.CreateTarget();
        }
        else
        {
            this.MissTargetImage();
        }
    }

    public override void UpdateProgress()
    {
        base.UpdateProgress();
        this.AutoAttackUpdate();
    }

    public GameObject TargetImage
    {
        get
        {
            if (this.m_TargetImageP == null)
            {
                this.m_TargetImageP = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>("Effect/Target"));
                this.m_TargetImageP.transform.SetParent(GameNode.m_Root);
                this.m_TargetImageP.transform.localScale = Vector3.one;
                this.m_TargetImageP.transform.localRotation = Quaternion.identity;
                this.m_TargetImageP.transform.localPosition = new Vector3(10000f, 0f, 0f);
            }
            return this.m_TargetImageP;
        }
    }

    private GameObject TargetRed
    {
        get
        {
            if (this.m_TargetRedP == null)
            {
                this.m_TargetRedP = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>("Game/UI/Target_Red"));
                this.m_TargetRedP.transform.SetParent(GameNode.m_Root);
                this.m_TargetRedP.transform.localScale = Vector3.one;
                this.m_TargetRedP.transform.localRotation = Quaternion.identity;
                this.m_TargetRedP.transform.localPosition = new Vector3(10000f, 0f, 0f);
                this.m_TargetRedP.name = "TargetRed";
            }
            return this.m_TargetRedP;
        }
    }

    public Animation TargetAni
    {
        get
        {
            if (this._TargetAni == null)
            {
                this._TargetAni = this.TargetImage.GetComponent<Animation>();
            }
            return this._TargetAni;
        }
    }
}

