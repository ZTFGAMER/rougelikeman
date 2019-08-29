using System;

public class EntityPartBodyBase : EntityCallBase
{
    protected ActionBasic action = new ActionBasic();
    private ConditionTime mCondition;
    public Action<int> OnRemoveEvent;
    public bool bGotoRoomRemove;

    protected override void OnDeInitLogic()
    {
        ReleaseModeManager mode = GameLogic.Release.Mode;
        mode.OnGotoNextRoom = (Action<RoomGenerateBase.Room>) Delegate.Remove(mode.OnGotoNextRoom, new Action<RoomGenerateBase.Room>(this.OnGotoNextRoom));
        if (this.OnRemoveEvent != null)
        {
            this.OnRemoveEvent(base.m_Data.CharID);
            this.OnRemoveEvent = null;
        }
        if (!this.bGotoRoomRemove)
        {
            GameLogic.PlayEffect(0x2f4d68, base.position);
        }
        base.OnDeInitLogic();
    }

    private void OnGotoNextRoom(RoomGenerateBase.Room room)
    {
        this.OnGotoNextRooms(room);
    }

    protected virtual void OnGotoNextRooms(RoomGenerateBase.Room room)
    {
        GameLogic.Release.Entity.RemovePartBody(this, true);
    }

    protected override void OnInit()
    {
        base.OnInit();
        base.m_MoveCtrl = new MoveControl();
        base.m_AttackCtrl = new AttackControl();
        base.m_MoveCtrl.Init(this);
        base.m_AttackCtrl.Init(this);
        GameLogic.Release.Entity.SetPartBody(this);
        base.m_EntityData.Modify_Invincible(true);
        this.SetCollider(false);
        ReleaseModeManager mode = GameLogic.Release.Mode;
        mode.OnGotoNextRoom = (Action<RoomGenerateBase.Room>) Delegate.Combine(mode.OnGotoNextRoom, new Action<RoomGenerateBase.Room>(this.OnGotoNextRoom));
    }

    public void SetAliveTime(float time)
    {
        ConditionTime time2 = new ConditionTime {
            time = time
        };
        this.mCondition = time2;
    }

    protected override void StartInit()
    {
        base.StartInit();
        base.ShowHP(false);
        base.PlayEffect(0x2f4d68, base.m_Body.EffectMask.transform.position);
    }

    protected override void UpdateFixed()
    {
        if (!base.GetIsDead())
        {
            base.m_MoveCtrl.UpdateProgress();
        }
    }

    protected override void UpdateProcess(float delta)
    {
        base.UpdateProcess(delta);
        if (!base.GetIsDead())
        {
            base.m_AttackCtrl.UpdateProgress();
            if ((this.mCondition != null) && this.mCondition.IsEnd())
            {
                this.mCondition = null;
                if (this.OnRemoveEvent != null)
                {
                    this.OnRemoveEvent(base.m_Data.CharID);
                    this.OnRemoveEvent = null;
                }
                GameLogic.Release.Entity.RemovePartBody(this, false);
            }
        }
    }

    protected override string ModelPath =>
        "Game/PartBody/PartBody";
}

