using Dxx.Util;
using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class MoveControl
{
    protected EntityBase m_Entity;
    [SerializeField]
    private bool bMoveing;
    private bool bTouchMove;
    protected JoyData m_JoyData;
    private bool bRegister;
    private float TouchStartTime;
    private Vector3 LastFramePosition;
    [SerializeField]
    protected Vector3 MoveDirection = Vector3.zero;
    private float Moving_angle;
    private Vector3 Moving_dir = new Vector3();
    private Vector3 Update_Speed;

    private void AddMoveSpeedUpdate()
    {
        this.m_Entity.m_EntityData.attribute.OnMoveSpeedUpdate = (Action) Delegate.Combine(this.m_Entity.m_EntityData.attribute.OnMoveSpeedUpdate, new Action(this.OnMoveSpeedUpdate));
    }

    public void AIMoveEnd(JoyData data)
    {
        if (this.bMoveing)
        {
            this.OnMoveEnd(data);
        }
    }

    public void AIMoveStart(JoyData data)
    {
        if (!this.bMoveing)
        {
            this.OnMoveStart(data);
        }
    }

    public void AIMoving(JoyData data)
    {
        if (this.bMoveing)
        {
            this.OnMoving(data);
        }
    }

    public void DeInit()
    {
        this.RemoveJoyEvent();
    }

    public bool GetMoving() => 
        this.bMoveing;

    public void Init(EntityBase entity)
    {
        this.m_Entity = entity;
        this.AddMoveSpeedUpdate();
        this.OnInit();
    }

    public void MoveEnd()
    {
        if (this.bMoveing)
        {
            this.bMoveing = false;
            this.m_Entity.m_AniCtrl.SendEvent("Idle", false);
            this.m_Entity.m_AttackCtrl.MoveEndCallBack();
            this.ResetRigidBody();
        }
    }

    protected virtual void MoveEndVirtual()
    {
    }

    private void MoveStart(string action = "Run")
    {
        this.bMoveing = true;
        this.bTouchMove = true;
        if (this.m_Entity.OnMoveEvent != null)
        {
            this.m_Entity.OnMoveEvent(true);
        }
        this.m_Entity.m_AniCtrl.SendEvent(action, false);
        if (this.m_Entity.IsSelf)
        {
            GameLogic.Release.Game.SetRunning();
            GameLogic.Hold.Sound.PlayWalk();
        }
        this.m_Entity.m_AniCtrl.SetBool("TouchMoveJoy", true);
        this.MoveStartVirtual();
    }

    protected virtual void MoveStartVirtual()
    {
    }

    private void Moving(JoyData data)
    {
        this.m_JoyData = data;
        this.m_Entity.m_AttackCtrl.RotateHero(data.angle);
        this.OnMoveSpeedUpdate();
        this.MovingVirtual(data);
    }

    protected virtual void MovingVirtual(JoyData data)
    {
    }

    protected virtual void OnInit()
    {
    }

    public void OnMoveEnd()
    {
        this.OnMoveEnd(this.m_JoyData);
    }

    private void OnMoveEnd(JoyData data)
    {
        if (!this.m_Entity.GetIsDead() && (data.name == "MoveJoy"))
        {
            this.bTouchMove = false;
            if (this.m_Entity.OnMoveEvent != null)
            {
                this.m_Entity.OnMoveEvent(false);
            }
            this.MoveEnd();
            if (this.m_Entity.IsSelf)
            {
                GameLogic.Hold.Sound.StopWalk();
            }
            this.m_Entity.m_AniCtrl.SetBool("TouchMoveJoy", false);
            this.ResetRigidBody();
            this.MoveEndVirtual();
        }
    }

    protected virtual void OnMoveSpeedUpdate()
    {
        if (this.bTouchMove && (this.m_Entity.State == EntityState.Normal))
        {
            if (this.m_Entity.IsSelf)
            {
                this.MoveDirection = this.m_JoyData.MoveDirection * this.m_Entity.m_EntityData.GetSpeed();
            }
            else if (this.m_JoyData._moveDirection == Vector3.zero)
            {
                this.Moving_angle = this.m_Entity.m_AttackCtrl.GetCurrentAngle();
                this.Moving_dir.x = MathDxx.Sin(this.Moving_angle);
                this.Moving_dir.z = MathDxx.Cos(this.Moving_angle);
                this.MoveDirection = (Vector3) ((this.m_Entity.m_EntityData.GetSpeed() * this.Moving_dir) * this.m_JoyData.MoveDirection.magnitude);
            }
            else
            {
                this.MoveDirection = (Vector3) (this.m_Entity.m_EntityData.GetSpeed() * this.m_JoyData.MoveDirection);
            }
        }
    }

    public void OnMoveStart(JoyData data)
    {
        if (!this.m_Entity.GetIsDead() && (data.name == "MoveJoy"))
        {
            this.MoveStart(data.action);
        }
    }

    public void OnMoving(JoyData data)
    {
        if (!this.m_Entity.GetIsDead() && (data.name == "MoveJoy"))
        {
            if (!this.bMoveing && !this.m_Entity.m_AttackCtrl.GetAttacking())
            {
                this.OnMoveStart(data);
            }
            if (this.bMoveing && this.m_Entity.m_AttackCtrl.GetAttacking())
            {
                this.MoveEnd();
            }
            if (this.bMoveing)
            {
                this.Moving(data);
            }
        }
    }

    public void RegisterJoyEvent()
    {
        if (!this.bRegister && (this.m_Entity.Type == EntityType.Hero))
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

    public void ResetRigidBody()
    {
        this.MoveDirection = Vector3.zero;
    }

    public void SetMoving(bool moving)
    {
        if ((this.bMoveing && !moving) && this.bTouchMove)
        {
            this.MoveEnd();
        }
        else if ((!this.bMoveing && moving) && this.bTouchMove)
        {
            this.MoveStart("Run");
        }
    }

    public void SetMovingInternal(bool value)
    {
        this.bMoveing = value;
    }

    public void Start()
    {
        this.RegisterJoyEvent();
    }

    public void UpdateProgress()
    {
        if ((this.m_Entity != null) && !this.m_Entity.GetIsDead())
        {
            if (this.m_Entity.State == EntityState.Hitted)
            {
                this.MoveDirection = this.m_Entity.GetHittedDirection() * this.m_Entity.HittedV;
            }
            if (((GameLogic.Release != null) && (GameLogic.Release.Game != null)) && (this.MoveDirection != Vector3.zero))
            {
                this.Update_Speed = this.MoveDirection * Time.deltaTime;
                this.m_Entity.SelfMoveBy(this.Update_Speed);
                if (this.m_Entity.m_Body != null)
                {
                    this.m_Entity.m_Body.SetOrder();
                }
            }
        }
    }

    public enum EMoveDirection
    {
        Down,
        Up,
        Left,
        Right
    }
}

