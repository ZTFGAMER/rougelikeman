using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AIBabyBase : AIBase
{
    public const float NearRange = 1f;
    protected EntityCallBase baby;
    protected int AttackID;
    protected EntityBase mParent;
    private float fardis = 5f;
    [CompilerGenerated]
    private static Func<bool> <>f__am$cache0;

    protected bool FindTarget()
    {
        EntityBase base2 = GameLogic.Release.Entity.GetNearEntity(base.m_Entity, 2.147484E+09f, false);
        base.m_Entity.m_HatredTarget = base2;
        return (base2 != null);
    }

    private string getactionname(string name) => 
        ("AI2012 " + name);

    protected virtual ActionBasic.ActionBase GetAILogic()
    {
        AIBase.ActionSequence sequence = new AIBase.ActionSequence {
            m_Entity = base.m_Entity,
            name = this.getactionname("seq1")
        };
        ActionBasic.ActionBase action = new AIMove1010(base.m_Entity, this.baby.GetParent(), this.fardis) {
            name = this.getactionname("move1010")
        };
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = () => GameLogic.Release.Mode.RoomGenerate.IsDoorOpen();
        }
        action.ConditionBase = <>f__am$cache0;
        sequence.AddAction(action);
        sequence.AddAction(this.GetAttackOrMove());
        return sequence;
    }

    private ActionBasic.ActionBase GetAttackOrMove()
    {
        AIBase.ActionChooseIf choose = new AIBase.ActionChooseIf {
            name = this.getactionname("chooseif2")
        };
        if ((this.AttackID > 0) && GameLogic.Hold.BattleData.Challenge_AttackEnable())
        {
            this.OnAddAttack(choose);
        }
        ActionBasic.ActionBase babyMove = this.GetBabyMove();
        choose.AddAction(babyMove);
        return choose;
    }

    protected ActionBasic.ActionBase GetBabyMove() => 
        new AIMoveBabyNormal(base.m_Entity, 0x3e8, 0x7d0, this.fardis) { name = this.getactionname("babynormal") };

    protected bool GetFar()
    {
        if (this.mParent == null)
        {
            return false;
        }
        if (!this.mParent.m_MoveCtrl.GetMoving())
        {
            return false;
        }
        return (Vector3.Distance(base.m_Entity.position, this.mParent.position) > this.fardis);
    }

    protected virtual void OnAddAttack(AIBase.ActionChooseIf choose)
    {
        AIBase.ActionSequence action = new AIBase.ActionSequence();
        this.getactionname("attack seq");
        action.m_Entity = base.m_Entity;
        action.ConditionBase = new Func<bool>(this.FindTarget);
        ActionBasic.ActionBase base2 = base.GetActionAttack(this.getactionname("attack1"), this.AttackID, true);
        base2.ConditionBase = new Func<bool>(this.FindTarget);
        action.AddAction(base2);
        action.AddAction(base2);
        action.AddAction(base2);
        AIMoveBabyMoveParent parent = new AIMoveBabyMoveParent(base.m_Entity, this.mParent, 4) {
            name = this.getactionname("moveparent")
        };
        action.AddAction(base.GetActionWait(string.Empty, 100));
        action.AddAction(parent);
        choose.AddAction(action);
    }

    protected override void OnAIDeInit()
    {
        base.OnAIDeInit();
        ReleaseModeManager mode = GameLogic.Release.Mode;
        mode.OnGotoNextRoom = (Action<RoomGenerateBase.Room>) Delegate.Remove(mode.OnGotoNextRoom, new Action<RoomGenerateBase.Room>(this.OnGoToNextRoom));
    }

    private void OnGoToNextRoom(RoomGenerateBase.Room room)
    {
    }

    protected override void OnInit()
    {
        this.baby = base.m_Entity as EntityCallBase;
        this.mParent = this.baby.GetParent();
        base.AddAction(this.GetAILogic());
    }

    protected override void OnInitOnce()
    {
        CInstance<BattleResourceCreator>.Instance.GetFootCircle(base.m_Entity.transform);
        base.IsDelayTime = false;
        this.AttackID = base.m_Entity.m_Data.WeaponID;
        base.m_Entity.ChangeWeapon(this.AttackID);
        base.OnInitOnce();
        ReleaseModeManager mode = GameLogic.Release.Mode;
        mode.OnGotoNextRoom = (Action<RoomGenerateBase.Room>) Delegate.Combine(mode.OnGotoNextRoom, new Action<RoomGenerateBase.Room>(this.OnGoToNextRoom));
    }
}

