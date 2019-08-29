using DG.Tweening;
using GameProtocol;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;

public class WindowUI
{
    private static WindowID mCurrentCurrencyID = WindowID.WindowID_Invaild;
    private static WindowID mBeforeCurrencyID = WindowID.WindowID_Invaild;
    private static int mMaskCount = 0;
    private static int mNetDoingCount = 0;
    private static List<WindowID> mInGameList = new List<WindowID>();
    private static List<WindowID> mOutGameList = new List<WindowID>();
    private static List<WindowID> mAllList = new List<WindowID>();
    private const float WindowCheckTime = 180f;
    private static bool Update_bInit = false;
    [CompilerGenerated]
    private static Action<bool, CRespUserLoginPacket> <>f__am$cache0;
    [CompilerGenerated]
    private static Action <>f__am$cache1;
    [CompilerGenerated]
    private static TweenCallback <>f__mg$cache0;
    [CompilerGenerated]
    private static Action <>f__am$cache2;

    [field: CompilerGenerated, DebuggerBrowsable(0)]
    public static  event Action<WindowID, List<WindowID>> OnInGameWindowClose;

    [field: CompilerGenerated, DebuggerBrowsable(0)]
    public static  event Action<WindowID, List<WindowID>> OnInGameWindowOpen;

    public static void AddCloseWindow(WindowID id)
    {
        int state = UIResourceDefine.windowClass[id].State;
        if (state == 1)
        {
            if (mInGameList.Contains(id))
            {
                mInGameList.Remove(id);
            }
        }
        else if ((state == 0) && mOutGameList.Contains(id))
        {
            mOutGameList.Remove(id);
        }
    }

    public static void AddOpenWindow(WindowID id)
    {
        int state = UIResourceDefine.windowClass[id].State;
        switch (state)
        {
            case 1:
                mInGameList.Add(id);
                break;

            case 0:
                mOutGameList.Add(id);
                break;
        }
        if (state < 3)
        {
            mAllList.Add(id);
            if (OnInGameWindowOpen != null)
            {
                OnInGameWindowOpen(id, mAllList);
            }
        }
    }

    public static void CloseAllWindows()
    {
        CloseGameOut();
        CloseGameIn();
    }

    public static void CloseCurrency()
    {
        CloseCurrencyInternal();
        OpenBeforeCurrency();
    }

    private static void CloseCurrencyInternal()
    {
        if (mCurrentCurrencyID != WindowID.WindowID_Invaild)
        {
            CloseWindow(mCurrentCurrencyID);
            mCurrentCurrencyID = WindowID.WindowID_Invaild;
        }
    }

    private static void CloseGameIn()
    {
        int num = 0;
        int count = mInGameList.Count;
        while (num < count)
        {
            CloseWindowInternal(mInGameList[num]);
            num++;
        }
        mInGameList.Clear();
    }

    private static void CloseGameOut()
    {
        int num = 0;
        int count = mOutGameList.Count;
        while (num < count)
        {
            CloseWindowInternal(mOutGameList[num]);
            num++;
        }
        mOutGameList.Clear();
    }

    public static void CloseWindow(WindowID id)
    {
        CloseWindowInternal(id);
        AddCloseWindow(id);
    }

    private static void CloseWindowInternal(WindowID id)
    {
        string className = UIResourceDefine.windowClass[id].ClassName;
        Facade.Instance.RemoveMediator(className);
        if ((UIResourceDefine.windowClass[id].State < 3) && mAllList.Contains(id))
        {
            mAllList.Remove(id);
            if (OnInGameWindowClose != null)
            {
                OnInGameWindowClose(id, mAllList);
            }
        }
    }

    public static void GameBegin()
    {
        CloseGameOut();
    }

    public static void GameOver()
    {
        CloseGameIn();
    }

    public static bool GetOnlyMain() => 
        ((mOutGameList.Count == 1) && (((WindowID) mOutGameList[0]) == WindowID.WindowID_Main));

    private static bool GetReOpenMainClose(WindowID id)
    {
        bool flag = true;
        return ((((id != WindowID.WindowID_BattleLoad) && (id != WindowID.WindowID_Mask)) && (id != WindowID.WindowID_NetDoing)) && flag);
    }

    public static void Init()
    {
        Update_Init();
    }

    public static bool IsWindowOpened(WindowID id)
    {
        string className = UIResourceDefine.windowClass[id].ClassName;
        return (Facade.Instance.RetrieveMediator(className) != null);
    }

    private static void OnUpdate()
    {
        Dictionary<string, WindowMediator.WindowCacheData>.Enumerator enumerator = WindowMediator.mCacheUIPanel.GetEnumerator();
        while (enumerator.MoveNext())
        {
            KeyValuePair<string, WindowMediator.WindowCacheData> current = enumerator.Current;
            WindowMediator.WindowCacheData data = current.Value;
            if (Facade.Instance.RetrieveMediator(data.name) == null)
            {
                KeyValuePair<string, WindowMediator.WindowCacheData> pair2 = enumerator.Current;
                if ((Time.realtimeSinceStartup - pair2.Value.lasttime) > 180f)
                {
                    WindowMediator.RemoveCache(data.name);
                    break;
                }
            }
        }
    }

    public static void OpenBeforeCurrency()
    {
        if (mBeforeCurrencyID != WindowID.WindowID_Invaild)
        {
            ShowCurrency(mBeforeCurrencyID);
        }
    }

    private static void Pop_MissBefore()
    {
    }

    public static void PopClose()
    {
    }

    public static void ReOpenMain()
    {
        if (<>f__am$cache1 == null)
        {
            <>f__am$cache1 = delegate {
                Dictionary<WindowID, UIResourceDefine.WindowData>.Enumerator enumerator = UIResourceDefine.windowClass.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    KeyValuePair<WindowID, UIResourceDefine.WindowData> current = enumerator.Current;
                    if (GetReOpenMainClose(current.Key))
                    {
                        KeyValuePair<WindowID, UIResourceDefine.WindowData> pair2 = enumerator.Current;
                        CloseWindowInternal(pair2.Key);
                    }
                }
                mOutGameList.Clear();
                mInGameList.Clear();
                mAllList.Clear();
                ShowWindow(WindowID.WindowID_Main);
            };
        }
        ShowLoading(<>f__am$cache1, null, null, BattleLoadProxy.LoadingType.eMiss);
    }

    public static void ShowAdInsideUI(AdInsideProxy.EnterSource source, Action callback)
    {
        AdInsideProxy.Transfer data = new AdInsideProxy.Transfer {
            source = source,
            finish_callback = callback
        };
        Facade.Instance.RegisterProxy(new AdInsideProxy(data));
        ShowWindow(WindowID.WindowID_AdInside);
    }

    public static void ShowCurrency(WindowID id)
    {
        mBeforeCurrencyID = mCurrentCurrencyID;
        if ((mCurrentCurrencyID != WindowID.WindowID_Invaild) && (mCurrentCurrencyID != id))
        {
            CloseWindow(mCurrentCurrencyID);
        }
        mCurrentCurrencyID = id;
        ShowWindow(mCurrentCurrencyID);
    }

    public static void ShowGoldBuy(CoinExchangeSource buytype, long needgold, Action<int> callback)
    {
        GoldBuyModuleProxy.Transfer data = new GoldBuyModuleProxy.Transfer {
            buytype = buytype,
            gold = needgold,
            callback = callback
        };
        Facade.Instance.RegisterProxy(new GoldBuyModuleProxy(data));
        ShowWindow(WindowID.WindowID_GoldBuy);
    }

    public static void ShowLoading(Action loading, Action end1 = null, Action end2 = null, BattleLoadProxy.LoadingType type = 0)
    {
        BattleLoadProxy.BattleLoadData data = new BattleLoadProxy.BattleLoadData {
            LoadingDo = loading,
            LoadEnd1Do = end1,
            LoadEnd2Do = end2,
            loadingType = type
        };
        Facade.Instance.RegisterProxy(new BattleLoadProxy(data));
        ShowWindow(WindowID.WindowID_BattleLoad);
    }

    public static void ShowMask(bool value)
    {
        int mMaskCount = WindowUI.mMaskCount;
        WindowUI.mMaskCount += !value ? -1 : 1;
        int num2 = WindowUI.mMaskCount;
        if ((WindowUI.mMaskCount == 1) && value)
        {
            ShowWindow(WindowID.WindowID_Mask);
        }
        else if ((WindowUI.mMaskCount == 0) && !value)
        {
            CloseWindow(WindowID.WindowID_Mask);
        }
    }

    public static void ShowNetDoing(bool value, NetDoingType type = 1)
    {
        mNetDoingCount += !value ? -1 : 1;
        if ((mNetDoingCount == 1) && value)
        {
            NetDoingProxy.Transfer data = new NetDoingProxy.Transfer {
                type = type
            };
            NetDoingProxy proxy = new NetDoingProxy(data);
            Facade.Instance.RegisterProxy(proxy);
            ShowWindow(WindowID.WindowID_NetDoing);
        }
        else if ((mNetDoingCount == 0) && !value)
        {
            CloseWindow(WindowID.WindowID_NetDoing);
        }
    }

    private static void ShowPop(WindowID id)
    {
    }

    public static void ShowPopWindowOneUI(string title, string content, string sure, bool closebuttonshow, Action callback)
    {
        PopWindowOneProxy.Transfer data = new PopWindowOneProxy.Transfer {
            title = title,
            content = content,
            sure = sure,
            callback = callback,
            showclosebutton = closebuttonshow
        };
        Facade.Instance.RegisterProxy(new PopWindowOneProxy(data));
        ShowWindow(WindowID.WindowID_PopWindowOne);
    }

    public static void ShowPopWindowUI(string title, string content, Action callback)
    {
        PopWindowProxy.Transfer data = new PopWindowProxy.Transfer {
            title = title,
            content = content,
            callback = callback
        };
        Facade.Instance.RegisterProxy(new PopWindowProxy(data));
        ShowWindow(WindowID.WindowID_PopWindow);
    }

    public static void ShowRewardSimple(List<Drop_DropModel.DropData> list)
    {
        if ((list != null) && (list.Count != 0))
        {
            RewardSimpleProxy.Transfer data = new RewardSimpleProxy.Transfer {
                list = list
            };
            Facade.Instance.RegisterProxy(new RewardSimpleProxy(data));
            ShowWindow(WindowID.WindowID_RewardSimple);
        }
    }

    public static void ShowRewardUI(List<Drop_DropModel.DropData> list)
    {
        RewardShowProxy.Transfer data = new RewardShowProxy.Transfer {
            list = list
        };
        Facade.Instance.RegisterProxy(new RewardShowProxy(data));
        ShowWindow(WindowID.WindowID_RewardShow);
    }

    public static void ShowServerAssert(long time)
    {
        ServerAssertProxy.Transfer data = new ServerAssertProxy.Transfer {
            assertendtime = time
        };
        ServerAssertProxy proxy = new ServerAssertProxy(data);
        Facade.Instance.RegisterProxy(proxy);
        ShowWindow(WindowID.WindowID_ServerAssert);
    }

    public static void ShowShopSingle(ShopSingleProxy.SingleType type, Action onclose = null)
    {
        ShopSingleProxy.Transfer data = new ShopSingleProxy.Transfer {
            type = type
        };
        ShopSingleProxy proxy = new ShopSingleProxy(data) {
            Event_Para0 = onclose
        };
        Facade.Instance.RegisterProxy(proxy);
        ShowWindow(WindowID.WindowID_ShopSingle);
    }

    public static void ShowWindow(WindowID id)
    {
        if (id != WindowID.WindowID_Invaild)
        {
            if (id == WindowID.WindowID_Main)
            {
                GameLogic.Release.Game.SetGameState(GameManager.GameState.eMain);
                GameOver();
            }
            else if (id == WindowID.WindowID_Battle)
            {
                GameLogic.Release.Game.SetGameState(GameManager.GameState.eGaming);
                GameBegin();
            }
            int state = UIResourceDefine.windowClass[id].State;
            if ((((state == 3) || (state == 2)) || ((state == 0) && (GameLogic.Release.Game.gameState == GameManager.GameState.eMain))) || ((state == 1) && (GameLogic.Release.Game.gameState == GameManager.GameState.eGaming)))
            {
                ShowWindowInternal(id);
            }
        }
    }

    private static void ShowWindowInternal(WindowID id)
    {
        string className = UIResourceDefine.windowClass[id].ClassName;
        AddOpenWindow(id);
        WindowMediator mediator = (WindowMediator) Activator.CreateInstance(Type.GetType(className));
        Facade.Instance.RegisterMediator(mediator);
    }

    public static void TryLogin()
    {
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = delegate (bool result, CRespUserLoginPacket data) {
                <TryLogin>c__AnonStorey0 storey = new <TryLogin>c__AnonStorey0 {
                    data = data
                };
                ulong serverUserID = LocalSave.Instance.GetServerUserID();
                if ((!result && (storey.data != null)) && ((serverUserID > 0L) && (serverUserID != storey.data.m_nUserRawId)))
                {
                    Action action = new Action(storey.<>m__0);
                    if (<>f__am$cache2 == null)
                    {
                        <>f__am$cache2 = () => LocalSave.Instance.SetUserTemp(string.Empty, string.Empty);
                    }
                    Action action2 = <>f__am$cache2;
                    ChangeAccountProxy.Transfer transfer = new ChangeAccountProxy.Transfer {
                        callback_sure = action,
                        callback_confirm = action2
                    };
                    ChangeAccountProxy proxy = new ChangeAccountProxy(transfer);
                    Facade.Instance.RegisterProxy(proxy);
                    ShowWindow(WindowID.WindowID_ChangeAccount);
                }
            };
        }
        LocalSave.Instance.TryLogin(<>f__am$cache0);
    }

    private static void Update_Init()
    {
        if (!Update_bInit)
        {
            Update_bInit = true;
            if (<>f__mg$cache0 == null)
            {
                <>f__mg$cache0 = new TweenCallback(null, OnUpdate);
            }
            TweenSettingsExtensions.SetUpdate<Sequence>(TweenSettingsExtensions.SetLoops<Sequence>(TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(DOTween.Sequence(), 34.5f), <>f__mg$cache0), -1), true);
        }
    }

    [CompilerGenerated]
    private sealed class <TryLogin>c__AnonStorey0
    {
        internal CRespUserLoginPacket data;
        private static Action <>f__am$cache0;

        internal void <>m__0()
        {
            LocalSave.Instance.RefreshUserIDFromTemp();
            GameLogic.Hold.BattleData.RemoveStageLocal();
            LocalSave.Instance.StageDiscount_Init(null);
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = () => WindowUI.ReOpenMain();
            }
            LocalSave.Instance.DoLoginCallBack(this.data, <>f__am$cache0);
        }

        private static void <>m__1()
        {
            WindowUI.ReOpenMain();
        }
    }
}

