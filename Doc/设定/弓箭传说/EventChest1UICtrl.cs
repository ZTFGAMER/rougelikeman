using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using TableTool;

public class EventChest1UICtrl : MediatorCtrlBase
{
    public ButtonCtrl Button_Start;
    public EventChest1TurnCtrl mTurnCtrl;
    private TurnTableType resultType;
    private string[] args;

    private void InitUI()
    {
        int result = 0;
        int num2 = 0;
        if (int.TryParse(this.args[0], out result) && int.TryParse(this.args[1], out num2))
        {
            Drop_DropModel.DropData[] equips = new Drop_DropModel.DropData[2];
            List<Drop_DropModel.DropData> dropList = LocalModelManager.Instance.Drop_Drop.GetDropList(result);
            if (dropList.Count > 0)
            {
                equips[0] = dropList[0];
            }
            List<Drop_DropModel.DropData> list2 = LocalModelManager.Instance.Drop_Drop.GetDropList(num2);
            if (list2.Count > 0)
            {
                equips[1] = list2[0];
            }
            this.mTurnCtrl.InitGood(equips);
        }
        else
        {
            SdkManager.Bugly_Report("EventChest1UICtrl", "OnOpen proxy.Data is not all int.");
        }
    }

    protected override void OnClose()
    {
        GameLogic.SetPause(false);
        RoomGenerateBase.EventCloseTransfer data = new RoomGenerateBase.EventCloseTransfer {
            windowid = WindowID.WindowID_EventChect1,
            data = this.resultType
        };
        GameLogic.Release.Mode.RoomGenerate.EventClose(data);
    }

    public override object OnGetEvent(string eventName) => 
        null;

    public override void OnHandleNotification(INotification notification)
    {
    }

    protected override void OnInit()
    {
        this.mTurnCtrl.TurnEnd = delegate (TurnTableType type) {
            this.resultType = type;
            WindowUI.CloseWindow(WindowID.WindowID_EventChect1);
        };
    }

    public override void OnLanguageChange()
    {
    }

    protected override void OnOpen()
    {
        IProxy proxy = Facade.Instance.RetrieveProxy("EventChest1Proxy");
        if (proxy == null)
        {
            SdkManager.Bugly_Report("EventChest1UICtrl", "OnOpen proxy is null.");
        }
        if (proxy.Data == null)
        {
            SdkManager.Bugly_Report("EventChest1UICtrl", "OnOpen proxy.Data is null.");
        }
        if (!(proxy.Data is string[]))
        {
            SdkManager.Bugly_Report("EventChest1UICtrl", "OnOpen proxy.Data is not string[].");
        }
        this.args = proxy.Data as string[];
        if (this.args.Length != 2)
        {
            SdkManager.Bugly_Report("EventChest1UICtrl", "OnOpen proxy.Data.Length != 2.");
        }
        GameLogic.SetPause(true);
        this.Button_Start.onClick = delegate {
            this.Button_Start.gameObject.SetActive(false);
            this.mTurnCtrl.Init();
        };
        this.Button_Start.gameObject.SetActive(true);
        this.InitUI();
    }
}

