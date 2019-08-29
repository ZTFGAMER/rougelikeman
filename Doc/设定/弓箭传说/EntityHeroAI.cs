using Dxx.Util;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class EntityHeroAI : EntityBase
{
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static EntityHeroAI <mHeroAI>k__BackingField;
    private AIBase m_AIBase;
    private float mMoveStartAngle;
    private float mMoveTime;
    private float mNextMoveMaxTime;
    private int width;
    private int height;
    private Vector2Int mPrevV = new Vector2Int();
    private float mJoyMoveAngle;
    private float mJoyTime;
    private float mJoyMoveMaxTime;
    private bool bAttack;
    private JoyData mJoyData;

    private void CheckJoyMove()
    {
        Vector2Int roomXY = GameLogic.Release.MapCreatorCtrl.GetRoomXY(base.position);
        if ((((this.mPrevV.x > 0) && (roomXY.x == 0)) || ((this.mPrevV.x < this.width) && (roomXY.x == this.width))) || (((this.mPrevV.y > 0) && (roomXY.y == 0)) || ((this.mPrevV.y < this.height) && (roomXY.y == this.height))))
        {
            this.RandomMove();
        }
        this.mPrevV = roomXY;
    }

    private void DeInitAI()
    {
        if (this.m_AIBase != null)
        {
            this.m_AIBase.DeInit();
            this.m_AIBase = null;
        }
    }

    private void FellGround()
    {
        GameLogic.Hold.Sound.PlayMonsterSkill(0x4c4b44, base.position);
    }

    protected override void InitCharacter()
    {
        base.m_EntityData.Init(this, base.m_Data.CharID);
    }

    private void OnAttackStartEnd()
    {
        base.m_MoveCtrl.OnMoveStart(this.mJoyData);
        this.RandomMove();
        this.bAttack = false;
    }

    protected override void OnChangeHP(EntityBase entity, long HP)
    {
    }

    protected override void OnDeInitLogic()
    {
        this.DeInitAI();
        base.OnDeInitLogic();
    }

    protected override void OnInit()
    {
        base.OnInit();
        mHeroAI = this;
        GameLogic.Release.Entity.Add(this);
        base.m_MoveCtrl = new MoveControl();
        base.m_AttackCtrl = new AIHeroAttackControl();
        base.m_MoveCtrl.Init(this);
        base.m_AttackCtrl.Init(this);
        base.m_EntityData.HittedInterval = 0.5f;
        base.m_AttackCtrl.SetRotate(0f);
        this.mJoyData = new JoyData();
        this.mJoyData.action = "Run";
        this.mJoyData.name = "MoveJoy";
    }

    protected override void OnInitBefore()
    {
        base.SetEntityType(EntityType.Soldier);
    }

    private void RandomAngleAndTime(out float angle, out float time)
    {
        if (GameLogic.Random(0, 100) < 60)
        {
            angle = GameLogic.Random((float) 0.5f, (float) 1.5f) * MathDxx.RandomSymbol();
            time = GameLogic.Random((float) 0f, (float) 0.2f);
        }
        else
        {
            angle = GameLogic.Random((float) 2f, (float) 4f) * MathDxx.RandomSymbol();
            time = GameLogic.Random((float) 0f, (float) 0.15f);
        }
    }

    private void RandomJoyAngle()
    {
        if ((Updater.AliveTime - this.mJoyTime) > this.mJoyMoveMaxTime)
        {
            if (GameLogic.Random(0, 100) < 70)
            {
                this.mJoyMoveAngle = 0f;
                this.mJoyMoveMaxTime = GameLogic.Random((float) 0f, (float) 0.5f);
            }
            else
            {
                this.RandomAngleAndTime(out this.mJoyMoveAngle, out this.mJoyMoveMaxTime);
            }
            this.mJoyTime = Updater.AliveTime;
        }
    }

    private void RandomMove()
    {
        float num = this.mNextMoveMaxTime - (Updater.AliveTime - this.mMoveTime);
        if (num <= 0f)
        {
            this.mMoveTime = Updater.AliveTime;
            if (GameLogic.Random(0, 100) < 80)
            {
                this.mNextMoveMaxTime = GameLogic.Random((float) 0.3f, (float) 0.6f);
            }
            else
            {
                this.mNextMoveMaxTime = GameLogic.Random((float) 0.6f, (float) 1f);
            }
        }
        Vector2Int roomXY = GameLogic.Release.MapCreatorCtrl.GetRoomXY(base.position);
        if (((roomXY.x == 0) && (roomXY.y < this.height)) && (roomXY.y > 0))
        {
            this.mMoveStartAngle = GameLogic.Random((float) 0f, (float) 180f);
        }
        else if ((roomXY.x == 0) && (roomXY.y == 0))
        {
            this.mMoveStartAngle = GameLogic.Random((float) 90f, (float) 180f);
        }
        else if ((roomXY.x == 0) && (roomXY.y == this.height))
        {
            this.mMoveStartAngle = GameLogic.Random((float) 0f, (float) 90f);
        }
        else if (((roomXY.x == this.width) && (roomXY.x < this.height)) && (roomXY.y > 0))
        {
            this.mMoveStartAngle = GameLogic.Random((float) 180f, (float) 360f);
        }
        else if ((roomXY.x == this.width) && (roomXY.y == 0))
        {
            this.mMoveStartAngle = GameLogic.Random((float) 180f, (float) 270f);
        }
        else if ((roomXY.x == this.width) && (roomXY.y == this.height))
        {
            this.mMoveStartAngle = GameLogic.Random((float) 270f, (float) 360f);
        }
        else if (((roomXY.x > 0) && (roomXY.x < this.width)) && (roomXY.y == this.height))
        {
            this.mMoveStartAngle = GameLogic.Random((float) -90f, (float) 90f);
        }
        else if (((roomXY.x > 0) && (roomXY.x < this.width)) && (roomXY.y == 0))
        {
            this.mMoveStartAngle = GameLogic.Random((float) 90f, (float) 270f);
        }
        else
        {
            this.mMoveStartAngle = GameLogic.Random((float) 0f, (float) 360f);
        }
    }

    protected override void StartInit()
    {
        this.InitWeapon(base.m_Data.WeaponID);
        this.width = GameLogic.Release.MapCreatorCtrl.width - 1;
        this.height = GameLogic.Release.MapCreatorCtrl.height - 1;
        this.RandomMove();
        base.m_Weapon.OnAttackStartEndAction = (Action) Delegate.Combine(base.m_Weapon.OnAttackStartEndAction, new Action(this.OnAttackStartEnd));
        this.m_AIBase = new AIHero();
        this.m_AIBase.SetEntity(this);
        this.m_AIBase.Init(false);
    }

    protected override void UpdateFixed()
    {
        base.m_MoveCtrl.UpdateProgress();
    }

    private void UpdateMove()
    {
        if ((Updater.AliveTime - this.mMoveTime) > this.mNextMoveMaxTime)
        {
            if (!this.bAttack)
            {
                this.bAttack = true;
                base.m_MoveCtrl.OnMoveEnd();
            }
        }
        else
        {
            this.CheckJoyMove();
            this.RandomJoyAngle();
            this.mMoveStartAngle += this.mJoyMoveAngle;
            this.mJoyData.UpdateDirectionByAngle(this.mMoveStartAngle);
            base.m_MoveCtrl.OnMoving(this.mJoyData);
        }
    }

    protected override void UpdateProcess(float delta)
    {
        base.UpdateProcess(delta);
        base.m_AttackCtrl.UpdateProgress();
        this.UpdateMove();
    }

    public static EntityHeroAI mHeroAI
    {
        [CompilerGenerated]
        get => 
            <mHeroAI>k__BackingField;
        [CompilerGenerated]
        private set => 
            (<mHeroAI>k__BackingField = value);
    }
}

