using Dxx.Net;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class ApplicationEvent : MonoBehaviour
{
    public static ApplicationEvent Instance;
    private bool isPause = true;
    private bool bFirstInGame = true;
    public static bool bQuit;
    private bool bCheckOnlyMain;
    private bool bOnlyMain;
    public static Action<SdkManager.LoginData> login_callback;
    private int gametime;
    private int currentgametime;
    private int lastgametime;

    [field: CompilerGenerated, DebuggerBrowsable(0)]
    public static  event Action<bool> OnAppPause;

    [field: CompilerGenerated, DebuggerBrowsable(0)]
    public static  event Action OnOnlyMain;

    private void Awake()
    {
        Instance = this;
        WindowUI.OnInGameWindowClose += new Action<WindowID, List<WindowID>>(this.OnWindowClose);
        WindowUI.OnInGameWindowOpen += new Action<WindowID, List<WindowID>>(this.OnWindowOpen);
    }

    public bool check_app_start()
    {
        bool flag = false;
        LocalSave.Instance.BattleIn_CheckInit();
        if (LocalSave.Instance.BattleIn_GetIn())
        {
            flag = true;
            WindowUI.ShowWindow(WindowID.WindowID_CheckBattleIn);
        }
        if (!flag)
        {
            flag = LocalSave.Instance.Mail.CheckMainPop();
        }
        if ((!flag && (NetManager.NetTime > 0L)) && (LocalSave.Instance.mHarvest.get_can_reward() && LocalSave.Instance.Card_GetHarvestAvailable()))
        {
            WindowUI.ShowWindow(WindowID.WindowID_AdHarvest);
            flag = true;
        }
        return flag;
    }

    private void check_only_main()
    {
        this.bCheckOnlyMain = false;
        bool flag = false;
        if (!flag)
        {
            flag = GameLogic.Hold.Guide.mCard.CheckGuide();
        }
        if (!flag)
        {
            flag = GameLogic.Hold.Guide.mEquip.CheckGuide();
        }
        if (!flag && (OnOnlyMain != null))
        {
            OnOnlyMain();
        }
        Facade.Instance.SendNotification("MainUI_UpdatePage");
    }

    private bool CheckNotice()
    {
        DateTime time = new DateTime(SdkManager.get_first_setup_time());
        DateTime time2 = new DateTime(DateTime.Now.ToUniversalTime().Ticks);
        TimeSpan span = (TimeSpan) (time2 - time);
        if ((span.TotalDays >= 2.0) && !PlayerPrefsEncrypt.HasKey("first_test_notice"))
        {
            PlayerPrefsEncrypt.SetInt("first_test_notice", 0);
            WindowUI.ShowWindow(WindowID.WindowID_TestNotice);
            return true;
        }
        return false;
    }

    private void LateUpdate()
    {
        if (this.bCheckOnlyMain)
        {
            this.check_only_main();
        }
        if (GameLogic.InGame)
        {
            this.gametime = CInstance<PlayerPrefsMgr>.Instance.gametime.get_value();
            this.currentgametime = (int) Time.realtimeSinceStartup;
            if (this.currentgametime != this.lastgametime)
            {
                this.gametime++;
                this.lastgametime = this.currentgametime;
                CInstance<PlayerPrefsMgr>.Instance.gametime.set_value(this.gametime);
            }
        }
    }

    public void on_gamecenter_change(string message)
    {
        Debugger.Log("授权 on_gamecenter_change = " + message);
        if ((message == "true") || (SdkManager.GameCenter_get_login_count() == 0))
        {
            WindowUI.TryLogin();
        }
        else
        {
            SdkManager.GameCenter_add_login_count();
        }
    }

    public void on_login_callback(string gamecenterid)
    {
        Debugger.Log("授权 on_login_callback = " + gamecenterid);
        if (login_callback != null)
        {
            Debugger.Log("授权 on_login_callback success!!!");
        }
    }

    private void OnApplicationFocus(bool value)
    {
    }

    private void OnApplicationPause(bool value)
    {
        if (this.bFirstInGame)
        {
            this.bFirstInGame = false;
        }
        else
        {
            this.isPause = value;
            if (OnAppPause != null)
            {
                OnAppPause(this.isPause);
            }
            if (value)
            {
                CInstance<PlayerPrefsMgr>.Instance.gametime.flush();
                SdkManager.send_app_end();
            }
            else
            {
                if ((GameLogic.InGame && (GameLogic.Hold.BattleData.GetMode() != GameMode.eMatchDefenceTime)) && (!GameLogic.Paused && !WindowUI.IsWindowOpened(WindowID.WindowID_Pause)))
                {
                    WindowUI.ShowWindow(WindowID.WindowID_Pause);
                }
                if (!GameLogic.InGame && this.bOnlyMain)
                {
                }
                SdkManager.send_event("app_start");
            }
        }
    }

    private void OnApplicationQuit()
    {
        CInstance<PlayerPrefsMgr>.Instance.gametime.flush();
        SdkManager.send_app_end();
        bQuit = true;
    }

    private void OnWindowClose(WindowID closeID, List<WindowID> holdlist)
    {
        if ((holdlist.Count == 1) && (((WindowID) holdlist[0]) == WindowID.WindowID_Main))
        {
            this.bCheckOnlyMain = true;
            this.bOnlyMain = true;
        }
    }

    private void OnWindowOpen(WindowID openID, List<WindowID> holdlist)
    {
        if (openID != WindowID.WindowID_Main)
        {
            this.bCheckOnlyMain = false;
            this.bOnlyMain = false;
        }
    }
}

