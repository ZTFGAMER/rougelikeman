using Dxx.Util;
using System;
using TableTool;
using UnityEngine;

public class WeaponBase
{
    public Action OnAttackStartStartAction;
    public Action OnAttackStartEndAction;
    public Action OnAttackEndStartAction;
    public Action OnAttackEndEndAction;
    public Action OnAttackInterruptAction;
    public Action OnBulletCache;
    public Action Event_EntityAttack_AttackEnd;
    private bool _attack_ani_end;
    private bool _attackend_actionend = true;
    private bool bInit;
    public Weapon_weapon m_Data;
    protected EntityBase m_Entity;
    protected bool pShowDirection = true;
    protected int BulletID;
    private string prevAttackPrev;
    private string prevAttackEnd;
    protected int ParabolaSize = 1;
    protected ActionBasic action;
    protected SequencePool mSeqPool = new SequencePool();
    private WaitForSeconds continue_delay = new WaitForSeconds(0.1f);
    private bool bClear;
    protected bool bDizzyRemove = true;
    protected EntityBase Target;

    public void Attack(params object[] args)
    {
        this.OnAttack(args);
    }

    public virtual void AttackJoyTouchDown()
    {
    }

    public virtual void AttackJoyTouchUp()
    {
    }

    protected Transform CreateBullet(float rota) => 
        this.CreateBullet(Vector3.zero, rota);

    protected Transform CreateBullet(Vector3 offsetpos) => 
        this.CreateBullet(offsetpos, 0f);

    protected Transform CreateBullet(Vector3 offsetpos, float rota)
    {
        Transform transform = null;
        if (this.m_Entity.IsSelf)
        {
            transform = GameLogic.Release.PlayerBullet.Get(this.BulletID).transform;
            transform.SetParent(this.m_Entity.m_Body.GetWeaponNode(this.m_Data.CreateNode, this.m_Entity.GetBulletCreateNode(this.m_Data.CreateNode)));
            offsetpos /= this.m_Entity.m_Body.GetBodyScale();
        }
        else
        {
            transform = GameLogic.BulletGet(this.BulletID).transform;
            transform.SetParent(this.m_Entity.GetBulletCreateNode(this.m_Data.CreateNode));
        }
        transform.localPosition = offsetpos;
        transform.SetParent(GameNode.m_PoolParent);
        transform.rotation = Quaternion.Euler(0f, this.m_Entity.eulerAngles.y + rota, 0f);
        transform.localScale = Vector3.one;
        BulletBase component = transform.GetComponent<BulletBase>();
        this.OnBulletCreate(component);
        component.Init(this.m_Entity, this.BulletID);
        component.SetLastBullet(this.m_Entity.m_EntityData.mLastBullet);
        this.m_Entity.m_EntityData.mLastBullet = component;
        return transform;
    }

    protected BulletBase CreateBulletOverride() => 
        this.CreateBulletOverride(Vector3.zero, 0f);

    protected BulletBase CreateBulletOverride(float rota) => 
        this.CreateBulletOverride(Vector3.zero, rota);

    protected BulletBase CreateBulletOverride(Vector3 offsetpos) => 
        this.CreateBulletOverride(offsetpos, 0f);

    protected BulletBase CreateBulletOverride(Vector3 offsetpos, float rota)
    {
        BulletBase component = this.CreateBullet(offsetpos, rota + GameLogic.Random(-this.m_Data.RandomAngle, this.m_Data.RandomAngle)).GetComponent<BulletBase>();
        component.SetBulletAttribute(new BulletTransmit(this.m_Entity, this.BulletID, this.bClear));
        component.SetTarget(this.Target, 1);
        return component;
    }

    private void CreateBullets()
    {
        this.CreateBullets_(this.m_Entity.m_EntityData.attribute.Bullet_Forward.Value, 0f);
        this.CreateBullets_(this.m_Entity.m_EntityData.attribute.Bullet_Backward.Value, 180f);
        this.CreateBullets_Side(this.m_Entity.m_EntityData.attribute.Bullet_ForSide.Value);
        this.CreateBullets_LeftRight(this.m_Entity.m_EntityData.attribute.Bullet_Side.Value);
    }

    private void CreateBullets_(long count, float rotaoffset)
    {
        float num2 = GameLogic.Random(-this.m_Data.RandomAngle, this.m_Data.RandomAngle);
        float num3 = 0.7f;
        float num4 = num3 * (count - 1L);
        for (int i = 0; i < count; i++)
        {
            float num = rotaoffset;
            Vector3 offsetpos = new Vector3((i * num3) - (num4 / 2f), 0f, 0f);
            BulletBase component = this.CreateBullet(offsetpos, num + num2).GetComponent<BulletBase>();
            component.SetBulletAttribute(new BulletTransmit(this.m_Entity, this.BulletID, this.bClear));
            component.SetTarget(this.Target, this.ParabolaSize);
        }
    }

    private void CreateBullets_LeftRight(long count)
    {
        float num = GameLogic.Random(-this.m_Data.RandomAngle, this.m_Data.RandomAngle);
        float num2 = 0.7f;
        float num3 = num2 * (count - 1L);
        for (int i = 0; i < count; i++)
        {
            Vector3 offsetpos = new Vector3(0f, 0f, (i * num2) - (num3 / 2f));
            BulletBase component = this.CreateBullet(offsetpos, num + 90f).GetComponent<BulletBase>();
            component.SetBulletAttribute(new BulletTransmit(this.m_Entity, this.BulletID, this.bClear));
            component.SetTarget(this.Target, this.ParabolaSize);
        }
        for (int j = 0; j < count; j++)
        {
            Vector3 offsetpos = new Vector3(0f, 0f, (j * num2) - (num3 / 2f));
            BulletBase component = this.CreateBullet(offsetpos, num - 90f).GetComponent<BulletBase>();
            component.SetBulletAttribute(new BulletTransmit(this.m_Entity, this.BulletID, this.bClear));
            component.SetTarget(this.Target, this.ParabolaSize);
        }
    }

    private void CreateBullets_Side(long count)
    {
        float num = 90f / ((float) (count + 1L));
        float num2 = GameLogic.Random(-this.m_Data.RandomAngle, this.m_Data.RandomAngle);
        for (int i = 0; i < count; i++)
        {
            float rota = (-num * (i + 1)) + num2;
            BulletBase component = this.CreateBullet(Vector3.zero, rota).GetComponent<BulletBase>();
            component.SetBulletAttribute(new BulletTransmit(this.m_Entity, this.BulletID, this.bClear));
            component.SetTarget(this.Target, this.ParabolaSize);
        }
        for (int j = 0; j < count; j++)
        {
            float rota = -((-num * (j + 1)) + num2);
            BulletBase component = this.CreateBullet(Vector3.zero, rota).GetComponent<BulletBase>();
            component.SetBulletAttribute(new BulletTransmit(this.m_Entity, this.BulletID, this.bClear));
            component.SetTarget(this.Target, this.ParabolaSize);
        }
    }

    protected void Event2EntityAttack()
    {
        Debugger.Log(this.m_Entity, string.Concat(new object[] { "WeaponBase,.Event2EntityAttack 2012 start _attack_ani_end ", this._attack_ani_end, " have callback ", this.Event_EntityAttack_AttackEnd != null }));
        if (this._attack_ani_end && (this.Event_EntityAttack_AttackEnd != null))
        {
            this.Event_EntityAttack_AttackEnd();
        }
    }

    public static Transform GetWeaponNode(BodyMask body, int weaponnode)
    {
        if (body == null)
        {
            return null;
        }
        if (weaponnode != 1)
        {
            return body.LeftWeapon.transform;
        }
        return body.RightWeapon.transform;
    }

    public void Init(EntityBase entity, int weaponid)
    {
        this.m_Entity = entity;
        this.BulletID = weaponid;
        this.m_Data = LocalModelManager.Instance.Weapon_weapon.GetBeanById(this.BulletID);
        this.OnInit();
        this.Install();
    }

    public void Install()
    {
        if (!this.bInit)
        {
            this.bInit = true;
            this.m_Entity.OnDizzy = (Action<bool>) Delegate.Combine(this.m_Entity.OnDizzy, new Action<bool>(this.OnDizzy));
            this.m_Entity.WeaponHandUpdate();
            this.prevAttackPrev = this.m_Entity.m_AniCtrl.GetString("AttackPrev");
            this.prevAttackEnd = this.m_Entity.m_AniCtrl.GetString("AttackEnd");
            this.m_Entity.m_AniCtrl.SetString("AttackPrev", this.m_Data.AttackPrevString);
            this.m_Entity.m_AniCtrl.SetString("AttackEnd", this.m_Data.AttackEndString);
            this.OnAttackEndEndAction = (Action) Delegate.Combine(this.OnAttackEndEndAction, new Action(this.OnAttackEnd));
            this.m_Entity.mAniCtrlBase.InitWeaponSpeed(this.m_Data.AttackSpeed);
            this.action = new ActionBasic();
            this.action.Init(false);
            this.OnInstall();
        }
    }

    protected virtual void OnAttack(params object[] args)
    {
        this.CreateBullets();
        if (this.m_Entity.m_EntityData.attribute.Bullet_Continue.Value > 1L)
        {
            for (int i = 0; i < (this.m_Entity.m_EntityData.attribute.Bullet_Continue.Value - 1L); i++)
            {
                ActionBasic.ActionWait action = new ActionBasic.ActionWait {
                    waitTime = 0.1f
                };
                this.action.AddAction(action);
                ActionBasic.ActionDelegate delegate2 = new ActionBasic.ActionDelegate {
                    action = () => this.CreateBullets()
                };
                this.action.AddAction(delegate2);
            }
        }
    }

    private void OnAttackEnd()
    {
        this._attack_ani_end = true;
        if (this.bAttackEndActionEnd)
        {
            this.Event2EntityAttack();
        }
    }

    protected virtual void OnBulletCreate(BulletBase bullet)
    {
    }

    private void OnDizzy(bool value)
    {
        if (this.bDizzyRemove)
        {
            this.UnInstall();
        }
    }

    protected virtual void OnInit()
    {
    }

    protected virtual void OnInstall()
    {
    }

    protected virtual void OnUnInstall()
    {
    }

    public void SetDizzyCantRemove()
    {
        this.bDizzyRemove = false;
    }

    public void SetFlying(bool fly)
    {
        if (this.m_Entity.m_WeaponHand != null)
        {
            MeshRenderer componentInChildren = this.m_Entity.m_WeaponHand.GetComponentInChildren<MeshRenderer>();
            if (componentInChildren != null)
            {
                componentInChildren.material.renderQueue = !fly ? 0x7d0 : 0xbb8;
            }
        }
    }

    public void SetOrder(int order)
    {
    }

    public void SetTarget(EntityBase entity)
    {
        this.Target = entity;
    }

    public void UnInstall()
    {
        this.mSeqPool.Clear();
        if (this.bInit)
        {
            this.bInit = false;
            if (this.m_Entity != null)
            {
                this.m_Entity.OnDizzy = (Action<bool>) Delegate.Remove(this.m_Entity.OnDizzy, new Action<bool>(this.OnDizzy));
                if (this.m_Entity.m_AniCtrl != null)
                {
                    this.m_Entity.m_AniCtrl.SetString(this.prevAttackPrev, "AttackPrev");
                    this.m_Entity.m_AniCtrl.SetString(this.prevAttackEnd, "AttackEnd");
                    if (this.m_Entity.mAniCtrlBase != null)
                    {
                        this.m_Entity.mAniCtrlBase.InitWeaponSpeed(1f / this.m_Data.AttackSpeed);
                    }
                }
            }
            this.action.DeInit();
            this.OnUnInstall();
        }
    }

    protected bool bAttackEndActionEnd
    {
        get => 
            this._attackend_actionend;
        set
        {
            if (!this._attackend_actionend && value)
            {
                this._attackend_actionend = true;
                this.Event2EntityAttack();
            }
            else
            {
                this._attackend_actionend = value;
            }
        }
    }

    public bool ShowDirection
    {
        set => 
            (this.pShowDirection = value);
    }
}

