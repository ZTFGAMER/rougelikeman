using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Goods1151 : GoodsBase
{
    private static Door1151 mDoorData;
    private DoorState state;

    public override void ChildTriggerEnter(GameObject o)
    {
        if (((GameLogic.Self != null) && (o == GameLogic.Self.gameObject)) && (this.state == DoorState.eNormal))
        {
            DoorData.GotoDoor(this);
        }
    }

    public override void ChildTriggetExit(GameObject o)
    {
        if ((o == GameLogic.Self.gameObject) && (this.state == DoorState.eThrough))
        {
            this.state = DoorState.eNormal;
        }
    }

    protected override void Init()
    {
        base.Init();
    }

    public void SetState(DoorState state)
    {
        this.state = state;
    }

    protected override void StartInit()
    {
        base.StartInit();
        DoorData.AddDoor(this);
    }

    public static Door1151 DoorData
    {
        get
        {
            if (mDoorData == null)
            {
                mDoorData = new Door1151();
            }
            return mDoorData;
        }
        set => 
            (mDoorData = value);
    }

    public class Door1151
    {
        public List<Goods1151> Doors = new List<Goods1151>();
        private ActionBasic action = new ActionBasic();
        [CompilerGenerated]
        private static Action <>f__am$cache0;

        public Door1151()
        {
            ReleaseModeManager mode = GameLogic.Release.Mode;
            mode.OnGotoNextRoom = (Action<RoomGenerateBase.Room>) Delegate.Combine(mode.OnGotoNextRoom, new Action<RoomGenerateBase.Room>(this.OnGotoNextRoom));
        }

        public void AddDoor(Goods1151 d)
        {
            this.Doors.Add(d);
            this.action.Init(false);
        }

        public void Clear()
        {
            this.Doors.Clear();
            this.action.DeInit();
        }

        public void DeInit()
        {
            this.Clear();
            ReleaseModeManager mode = GameLogic.Release.Mode;
            mode.OnGotoNextRoom = (Action<RoomGenerateBase.Room>) Delegate.Remove(mode.OnGotoNextRoom, new Action<RoomGenerateBase.Room>(this.OnGotoNextRoom));
        }

        private Goods1151 GetOtherDoor(Goods1151 d)
        {
            int num = 0;
            int count = this.Doors.Count;
            while (num < count)
            {
                Goods1151 goods = this.Doors[num];
                if (goods != d)
                {
                    return goods;
                }
                num++;
            }
            return null;
        }

        public void GotoDoor(Goods1151 d)
        {
            <GotoDoor>c__AnonStorey0 storey = new <GotoDoor>c__AnonStorey0();
            GameLogic.PlayEffect(0x30d401, d.transform.position);
            GameLogic.Self.ShowEntity(false);
            GameLogic.Release.Game.ShowJoy(false);
            storey.other = this.GetOtherDoor(d);
            storey.other.SetState(Goods1151.DoorState.eThrough);
            this.action.AddActionWaitDelegate(0.4f, new Action(storey.<>m__0));
            this.action.AddActionWaitDelegate(0.5f, new Action(storey.<>m__1));
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = delegate {
                    GameLogic.Self.ShowEntity(true);
                    GameLogic.Release.Game.ShowJoy(true);
                };
            }
            this.action.AddActionWaitDelegate(0.5f, <>f__am$cache0);
        }

        private void OnGotoNextRoom(RoomGenerateBase.Room room)
        {
            this.Clear();
        }

        [CompilerGenerated]
        private sealed class <GotoDoor>c__AnonStorey0
        {
            internal Goods1151 other;

            internal void <>m__0()
            {
                GameLogic.Self.SetPosition(this.other.transform.position);
            }

            internal void <>m__1()
            {
                GameLogic.PlayEffect(0x30d401, this.other.transform.position);
            }
        }
    }

    public enum DoorState
    {
        eNormal,
        eThrough
    }
}

