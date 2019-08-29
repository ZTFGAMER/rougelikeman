using Dxx.Util;
using System;
using System.Runtime.CompilerServices;

public class RoomGenerateLevelGuide : RoomGenerateBase
{
    [CompilerGenerated]
    private static Action <>f__am$cache0;
    [CompilerGenerated]
    private static Action <>f__am$cache1;

    protected override void OnEnd()
    {
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = delegate {
            };
        }
        if (<>f__am$cache1 == null)
        {
            <>f__am$cache1 = delegate {
            };
        }
        WindowUI.ShowLoading(() => base.mGuidEndAction(), <>f__am$cache0, <>f__am$cache1, BattleLoadProxy.LoadingType.eMiss);
    }

    protected override void OnEnterDoorAfter()
    {
        if (base.currentRoomID < base.maxRoomID)
        {
            base.roomCtrl.SetText(string.Empty);
            base.roomCtrl.LayerShow(false);
        }
        else
        {
            base.roomCtrl.LayerShow(true);
            base.roomCtrl.SetText("1");
        }
    }

    protected override void OnEnterDoorBefore()
    {
    }

    protected override string OnGetTmxID(int roomid)
    {
        object[] args = new object[] { roomid };
        return Utils.FormatString("Level_M_0_990{0}", args);
    }

    protected override void OnInit()
    {
    }

    protected override void OnOpenDoor()
    {
    }

    protected override void OnStartGame()
    {
        base.maxRoomID = 2;
    }

    protected override void OnStartGameEnd()
    {
    }
}

