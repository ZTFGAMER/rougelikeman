using Dxx.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AttackControl
{
    private static Dictionary<KeyCode, MoveControl.EMoveDirection> m_KeyDic;
    private static Dictionary<MoveControl.EMoveDirection, Vector2> m_DirDic;
    protected EntityBase m_EntityHero;
    protected EntityHero m_EntitySelf;
    protected JoyData m_JoyData = new JoyData();
    private float pAliveTime;
    private GameObject child;
    [SerializeField]
    private float RotateAngle = 270f;
    private Vector3 Direction = new Vector3(0f, 0f, -1f);
    private float updateangley;
    private bool bRegister;
    private AttackCtrl_Bomberman mBomberman;
    private bool bCanRotateP = true;
    public Func<bool> OnRotateOverEvent;

    static AttackControl()
    {
        Dictionary<KeyCode, MoveControl.EMoveDirection> dictionary = new Dictionary<KeyCode, MoveControl.EMoveDirection> {
            { 
                KeyCode.UpArrow,
                MoveControl.EMoveDirection.Up
            },
            { 
                KeyCode.DownArrow,
                MoveControl.EMoveDirection.Down
            },
            { 
                KeyCode.LeftArrow,
                MoveControl.EMoveDirection.Left
            },
            { 
                KeyCode.RightArrow,
                MoveControl.EMoveDirection.Right
            }
        };
        m_KeyDic = dictionary;
        Dictionary<MoveControl.EMoveDirection, Vector2> dictionary2 = new Dictionary<MoveControl.EMoveDirection, Vector2> {
            { 
                MoveControl.EMoveDirection.Up,
                new Vector2(0f, 1f)
            },
            { 
                MoveControl.EMoveDirection.Down,
                new Vector2(0f, -1f)
            },
            { 
                MoveControl.EMoveDirection.Left,
                new Vector2(-1f, 0f)
            },
            { 
                MoveControl.EMoveDirection.Right,
                new Vector2(1f, 0f)
            }
        };
        m_DirDic = dictionary2;
    }

    public void DeInit()
    {
        this.RemoveJoyEvent();
        this.OnDestroys();
    }

    private void FixedUpdate()
    {
    }

    public bool GetAttacking() => 
        (this.m_EntityHero.mAniCtrlBase.IsCurrentState("AttackPrev") || this.m_EntityHero.mAniCtrlBase.IsCurrentState("AttackEnd"));

    public float GetCurrentAngle() => 
        this.updateangley;

    public float GetHeroRotate() => 
        this.RotateAngle;

    private float GetHeroRotate(float currenty, float endx, float endy, float framepery)
    {
        float x = endx;
        float y = endy;
        float num3 = (Utils.getAngle(x, y) + 360f) % 360f;
        if (((MathDxx.Abs((float) (currenty - num3)) < 0.1f) || (MathDxx.Abs((float) ((currenty - num3) + 360f)) < 0.1f)) || (MathDxx.Abs((float) ((currenty - num3) - 360f)) < 0.1f))
        {
            return num3;
        }
        if (num3 < currenty)
        {
            if ((currenty - num3) < 180f)
            {
                currenty -= framepery;
                if (currenty < num3)
                {
                    currenty = num3;
                }
            }
            else
            {
                currenty += framepery;
                if (currenty >= 360f)
                {
                    currenty -= 360f;
                    if (currenty > num3)
                    {
                        currenty = num3;
                    }
                }
            }
        }
        else if ((num3 - currenty) >= 180f)
        {
            currenty -= framepery;
            if (currenty < 0f)
            {
                currenty += 360f;
                if (currenty < num3)
                {
                    currenty = num3;
                }
            }
        }
        else
        {
            currenty += framepery;
            if (currenty > num3)
            {
                currenty = num3;
            }
        }
        return Utils.GetFloat2(currenty);
    }

    public void Init(EntityBase entity)
    {
        this.m_EntityHero = entity;
        this.m_EntitySelf = this.m_EntityHero as EntityHero;
        this.m_JoyData.name = "AttackJoy";
        this.child = this.m_EntityHero.Child;
    }

    public virtual void MoveEndCallBack()
    {
    }

    protected virtual void OnDestroys()
    {
        if (this.mBomberman != null)
        {
            this.mBomberman.DeInit();
            this.mBomberman = null;
        }
    }

    public void OnMoveEnd()
    {
        this.OnMoveEnd(this.m_JoyData);
    }

    public void OnMoveEnd(JoyData data)
    {
        if ((data.name == "AttackJoy") && (this.m_EntityHero.m_Weapon != null))
        {
            this.m_EntityHero.m_Weapon.AttackJoyTouchUp();
        }
    }

    public void OnMoveStart(JoyData data)
    {
        if (data.name == "AttackJoy")
        {
            this.m_EntityHero.m_AniCtrl.SendEvent("AttackPrev", false);
            if (this.m_EntityHero.m_Weapon != null)
            {
                this.m_EntityHero.m_Weapon.SetTarget(this.m_EntityHero.m_HatredTarget);
                this.m_EntityHero.m_Weapon.AttackJoyTouchDown();
            }
            this.RotateHero(data.angle);
            this.m_EntityHero.m_MoveCtrl.SetMoving(false);
        }
    }

    public void OnMoving(JoyData data)
    {
        if (data.name == "AttackJoy")
        {
        }
    }

    protected virtual void OnStart()
    {
    }

    public void RegisterJoyEvent()
    {
        if (!this.bRegister && this.m_EntityHero.IsSelf)
        {
            ScrollCircle.On_JoyTouchStart += new ScrollCircle.JoyTouchStart(this.OnMoveStart);
            ScrollCircle.On_JoyTouching += new ScrollCircle.JoyTouching(this.OnMoving);
            ScrollCircle.On_JoyTouchEnd += new ScrollCircle.JoyTouchEnd(this.OnMoveEnd);
            this.bRegister = true;
        }
    }

    public void RemoveJoyEvent()
    {
        if (this.bRegister)
        {
            ScrollCircle.On_JoyTouchStart -= new ScrollCircle.JoyTouchStart(this.OnMoveStart);
            ScrollCircle.On_JoyTouching -= new ScrollCircle.JoyTouching(this.OnMoving);
            ScrollCircle.On_JoyTouchEnd -= new ScrollCircle.JoyTouchEnd(this.OnMoveEnd);
            this.bRegister = false;
        }
    }

    public virtual void Reset()
    {
    }

    public void RotateHero(EntityBase entity)
    {
        float angle = Utils.getAngle(entity.position - this.m_EntityHero.position);
        this.RotateHero(angle);
    }

    public void RotateHero(float angle)
    {
        float x = MathDxx.Sin(angle);
        float z = MathDxx.Cos(angle);
        Vector3 vector = new Vector3(x, 0f, z);
        this.Direction = vector.normalized;
        this.RotateAngle = angle;
        this.RotateAngle = this.RotateAngle % 360f;
    }

    public bool RotateOver()
    {
        if (this.OnRotateOverEvent != null)
        {
            return this.OnRotateOverEvent();
        }
        if (MathDxx.Abs((float) (this.updateangley - this.RotateAngle)) >= 1f)
        {
            return false;
        }
        this.updateangley = Utils.getAngle(this.Direction.x, this.Direction.z);
        if (this.m_EntityHero.Child != null)
        {
            this.m_EntityHero.Child.transform.localRotation = Quaternion.Euler(0f, this.updateangley, 0f);
        }
        return true;
    }

    public void RotateUpdate(EntityBase target)
    {
        if ((!this.m_EntityHero.m_MoveCtrl.GetMoving() && (target != null)) && !target.GetIsDead())
        {
            float x = target.transform.position.x - this.m_EntityHero.position.x;
            float y = target.transform.position.z - this.m_EntityHero.position.z;
            Vector2 vector5 = new Vector2(x, y);
            float magnitude = vector5.magnitude;
            if (magnitude == 0f)
            {
                magnitude = 1f;
            }
            x /= magnitude;
            y /= magnitude;
            this.m_JoyData.direction = new Vector3(x, 0f, y);
            this.m_JoyData.angle = Utils.getAngle(this.m_JoyData.direction);
            this.RotateHero(this.m_JoyData.angle);
        }
    }

    public void SetCanRotate(bool value)
    {
        this.bCanRotateP = value;
    }

    public void SetRotate(float angle)
    {
        float x = MathDxx.Sin(angle);
        float z = MathDxx.Cos(angle);
        this.Direction = new Vector3(x, 0f, z);
        this.RotateAngle = angle;
        this.updateangley = angle;
        if (this.m_EntityHero.Child != null)
        {
            this.m_EntityHero.Child.transform.localRotation = Quaternion.Euler(0f, this.updateangley, 0f);
        }
    }

    public void Start()
    {
        this.RegisterJoyEvent();
        if (GameLogic.Hold.BattleData.Challenge_BombermanEnable())
        {
            this.mBomberman = new AttackCtrl_Bomberman();
            this.mBomberman.Init(this.m_EntityHero);
        }
        this.OnStart();
    }

    public virtual void UpdateProgress()
    {
        if ((this.CanRotate && (((this.m_EntityHero != null) && !this.m_EntityHero.GetIsDead()) && !this.m_EntityHero.m_EntityData.IsDizzy())) && ((this.m_EntityHero.m_EntityData != null) && (this.m_EntityHero.m_EntityData.attribute != null)))
        {
            this.pAliveTime = Updater.AliveTime;
            this.updateangley = MathDxx.MoveTowardsAngle(this.updateangley, this.RotateAngle, this.m_EntityHero.m_EntityData.attribute.RotateSpeed.Value * Updater.delta);
            if (this.m_EntityHero.Child != null)
            {
                this.m_EntityHero.Child.transform.localRotation = Quaternion.Euler(0f, this.updateangley, 0f);
                this.m_EntityHero.SetEulerAngles(this.m_EntityHero.Child.transform.eulerAngles);
                if (this.m_EntitySelf != null)
                {
                    this.m_EntitySelf.Coin_Absorb.localRotation = this.m_EntityHero.Child.transform.localRotation;
                }
            }
            if (this.mBomberman != null)
            {
                this.mBomberman.Update();
            }
        }
    }

    protected float AliveTime =>
        this.pAliveTime;

    public bool CanRotate =>
        this.bCanRotateP;
}

