using Dxx.Net;
using Dxx.Util;
using Facebook.Unity;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ThinkingGame;
using Umeng;
using UnityEngine;

public class SdkManager
{
    public const string app_start = "app_start";
    public const string app_end = "app_end";
    private static bool bLogEnable;
    private const string BuglyAppID = "8c142b9f46";

    private static void Bugly_Init()
    {
        BuglyAgent.ConfigDebugMode(false);
        BuglyAgent.ConfigDefault(null, null, null, 0L);
        BuglyAgent.ConfigAutoReportLogLevel(5);
        BuglyAgent.ConfigAutoQuitApplication(false);
        BuglyAgent.RegisterLogCallback(null);
        BuglyAgent.InitWithAppId("8c142b9f46");
        BuglyAgent.EnableExceptionHandler();
    }

    public static void Bugly_Report(Exception e, string message)
    {
        BuglyAgent.ReportException(e, message);
    }

    public static void Bugly_Report(string name, string message)
    {
        Bugly_Report(name, message, string.Empty);
    }

    public static void Bugly_Report(bool excute, string name, string message)
    {
        if (!excute)
        {
            Bugly_Report(name, message);
        }
    }

    public static void Bugly_Report(string name, string message, string stackTrace)
    {
        BuglyAgent.ReportException(name, message, stackTrace);
    }

    private static void Facebook_Init()
    {
        FB.Init("250812872464647", "e3b92c19621dc2d71a0ea872d70669c3", true, true, true, false, true, null, "en_US", null, null);
    }

    public static void GameCenter_add_login_count()
    {
        int num = GameCenter_get_login_count();
        PlayerPrefsEncrypt.SetInt("gamecenter_login_count", num + 1);
    }

    public static void GameCenter_clear_login_count()
    {
        PlayerPrefsEncrypt.SetInt("gamecenter_login_count", 0);
    }

    public static int GameCenter_get_login_count() => 
        PlayerPrefsEncrypt.GetInt("gamecenter_login_count", 0);

    public static void GameCenter_Login(Action<LoginData> callback)
    {
        <GameCenter_Login>c__AnonStorey0 storey = new <GameCenter_Login>c__AnonStorey0 {
            callback = callback
        };
        Social.Active.Authenticate(Social.Active.localUser, new Action<bool, string>(storey.<>m__0));
    }

    private static void GameCenter_TryLogin(Action<LoginType, LoginData> callback)
    {
        <GameCenter_TryLogin>c__AnonStorey1 storey = new <GameCenter_TryLogin>c__AnonStorey1 {
            callback = callback
        };
        Social.Active.Authenticate(Social.Active.localUser, new Action<bool, string>(storey.<>m__0));
    }

    public static long get_first_setup_time() => 
        PlayerPrefsEncrypt.GetLong("set_first_setup_time", 0L);

    private static int GetBeforePurchaseHour()
    {
        if (!PlayerPrefsEncrypt.HasKey("before_purchase_time"))
        {
            PlayerPrefsEncrypt.SetLong("before_purchase_time", PlayerPrefsEncrypt.GetLong("set_first_setup_time", 0L));
        }
        long @long = PlayerPrefsEncrypt.GetLong("before_purchase_time", 0L);
        DateTime time = new DateTime(@long);
        DateTime time2 = DateTime.Now.ToUniversalTime();
        TimeSpan span = (TimeSpan) (time2 - time);
        PlayerPrefsEncrypt.SetLong("before_purchase_time", time2.Ticks);
        return (int) span.TotalHours;
    }

    public static int GetLoginType() => 
        1;

    public static void Google_Login(Action<LoginData> callback)
    {
        <Google_Login>c__AnonStorey2 storey = new <Google_Login>c__AnonStorey2 {
            callback = callback,
            data = new LoginData()
        };
        storey.data.bSuccess = false;
        storey.data.userid = string.Empty;
        if (!CInstance<Unity2AndroidHelper>.Instance.is_gp_avalible() || !NetManager.IsNetConnect)
        {
            storey.callback(storey.data);
        }
        else
        {
            PlayGamesClientConfiguration configuration = new PlayGamesClientConfiguration.Builder().RequestEmail().RequestServerAuthCode(false).RequestIdToken().Build();
            Debugger.Log("SdkManager_Google.Google_Login start");
            PlayGamesPlatform.InitializeInstance(configuration);
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();
            Social.Active.Authenticate(Social.Active.localUser, new Action<bool, string>(storey.<>m__0));
        }
    }

    public static void Google_TryLogin(Action<LoginType, LoginData> callback)
    {
        <Google_TryLogin>c__AnonStorey3 storey = new <Google_TryLogin>c__AnonStorey3 {
            callback = callback,
            data = new LoginData()
        };
        if (!CInstance<Unity2AndroidHelper>.Instance.is_gp_avalible())
        {
            storey.callback(LoginType.eGP, storey.data);
        }
        else
        {
            PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().RequestEmail().RequestServerAuthCode(false).RequestIdToken().Build());
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();
            Debugger.Log("SdkManager_Google googlelogin Google_TryLogin start");
            Social.Active.Authenticate(Social.Active.localUser, new Action<bool, string>(storey.<>m__0));
        }
    }

    public static void InitSdks()
    {
        ShuShu_Init();
        Bugly_Init();
        Umeng_Init();
        Facebook_Init();
    }

    private static void Log(string value)
    {
        if (bLogEnable)
        {
            object[] args = new object[] { value };
            Debug.Log(Utils.FormatString("ShuShu TGAnalysis : {0}", args));
        }
    }

    public static void Login(Action<LoginData> callback)
    {
        Google_Login(callback);
    }

    public static void login_check()
    {
        Debug.Log("SdkManager_GameCenter.login_check do nothing...");
    }

    public static void login_check_start()
    {
        Debug.Log("SdkManager_GameCenter.login_check_start do nothing...");
    }

    public static void OnLogin()
    {
        WindowUI.CloseAllWindows();
        WindowUI.ShowWindow(WindowID.WindowID_Login);
    }

    public static void send_app_end()
    {
        int realtimeSinceStartup = (int) Time.realtimeSinceStartup;
        int num2 = realtimeSinceStartup - CInstance<PlayerPrefsMgr>.Instance.apptime.get_value();
        CInstance<PlayerPrefsMgr>.Instance.apptime.set_value(realtimeSinceStartup);
        object[] args = new object[] { num2 };
        Log(Utils.FormatString("shushu send_app_end: duration:{0}", args));
        Dictionary<string, object> dictionary = new Dictionary<string, object> {
            { 
                "duration",
                num2
            }
        };
        TGAnalytics.TG.track("app_end", dictionary);
    }

    public static void send_deadlayer(int layer)
    {
    }

    public static void send_event(string eventId)
    {
        object[] args = new object[] { eventId };
        Log(Utils.FormatString("shushu send_event: eventId:{0}", args));
        TGAnalytics.TG.track(eventId);
    }

    public static void send_event_ad(ADSource source, string step, int coins, int gems, string result, string reason)
    {
        object[] args = new object[] { source, step, coins, gems, result, reason };
        Log(Utils.FormatString("shushu send_event_ad_key: source:{0} step:{1} coins:{2} gems:{3} result:{4} reason:{5}", args));
        Dictionary<string, object> dictionary = new Dictionary<string, object> {
            { 
                "source",
                source.ToString().ToUpper()
            },
            { 
                "step",
                step.ToUpper()
            },
            { 
                "coins",
                coins
            },
            { 
                "gems",
                gems
            },
            { 
                "result",
                result
            },
            { 
                "reason",
                reason
            }
        };
        TGAnalytics.TG.track("ad", dictionary);
    }

    public static void send_event_equip_combine(string step, int equipment, string result, string reason)
    {
        object[] args = new object[] { step, equipment, result, reason };
        Log(Utils.FormatString("shushu send_event_equip_combine: step:{0} equipment:{1} result:{2} reason:{3}", args));
        Dictionary<string, object> dictionary = new Dictionary<string, object> {
            { 
                "step",
                step
            },
            { 
                "equipment",
                equipment
            },
            { 
                "result",
                result
            },
            { 
                "reason",
                reason
            }
        };
        TGAnalytics.TG.track("equip_combine", dictionary);
    }

    public static void send_event_equipment(string step, int equipment, int count, int target_level, EquipSource source, int coins)
    {
        object[] args = new object[] { step, equipment, count, target_level, source, coins };
        Log(Utils.FormatString("shushu send_event_equipment: step:{0} equipment:{1} count:{2} target_level:{3} source:{4} coins:{5}", args));
        Dictionary<string, object> dictionary = new Dictionary<string, object> {
            { 
                "step",
                step
            },
            { 
                "equipment",
                equipment
            },
            { 
                "count",
                count
            },
            { 
                "target_level",
                target_level
            },
            { 
                "source",
                source.ToString().ToUpper()
            },
            { 
                "coins",
                coins
            }
        };
        TGAnalytics.TG.track("equipment", dictionary);
    }

    public static void send_event_exchange(CoinExchangeSource source, int coins, int gems)
    {
        object[] args = new object[] { source, coins, gems };
        Log(Utils.FormatString("shushu send_event_exchange: source:{0} coins:{1} gems:{2}", args));
        Dictionary<string, object> dictionary = new Dictionary<string, object> {
            { 
                "source",
                source.ToString().ToUpper()
            },
            { 
                "coins",
                coins
            },
            { 
                "gems",
                gems
            }
        };
        TGAnalytics.TG.track("exchange", dictionary);
    }

    public static void send_event_game_end(int survive_times, BattleSource source, BattleEndType end_type, int coins, int equipment, int end_stage, int end_layer, int newbest, int exp, int levelupcount, int level)
    {
        int num = CInstance<PlayerPrefsMgr>.Instance.gametime.get_value();
        object[] args = new object[] { survive_times, source, end_type, coins, equipment, end_stage, newbest, num };
        Log(Utils.FormatString("shushu : game_end survive_times:{0} source:{1} end_type:{2} coin:{3} equipment_number:{4}, last_level:{5} newbest:{6} duration:{7}", args));
        Dictionary<string, object> dictionary = new Dictionary<string, object> {
            { 
                "survive_times",
                survive_times
            },
            { 
                "source",
                source.ToString().ToUpper()
            },
            { 
                "end_type",
                end_type.ToString().ToUpper()
            },
            { 
                "coins",
                coins
            },
            { 
                "equipment",
                equipment
            },
            { 
                "stage",
                end_stage
            },
            { 
                "layer",
                end_layer
            },
            { 
                "attempts_from_last_new_best",
                newbest
            },
            { 
                "exp",
                exp
            },
            { 
                "levelupcount",
                levelupcount
            },
            { 
                "level",
                level
            },
            { 
                "duration",
                num
            }
        };
        TGAnalytics.TG.track("game_end", dictionary);
    }

    public static void send_event_game_start(BattleSource source, int chapter)
    {
        object[] args = new object[] { source, chapter };
        Log(Utils.FormatString("shushu send_event_game_start: source:{0} chapter:{1}", args));
        Dictionary<string, object> dictionary = new Dictionary<string, object> {
            { 
                "source",
                source.ToString().ToUpper()
            },
            { 
                "chapter",
                chapter
            }
        };
        TGAnalytics.TG.track("game_start", dictionary);
    }

    public static void send_event_harvest(string step, string result, string reason, int coins, int gems)
    {
        object[] args = new object[] { step, result, reason, coins, gems };
        Log(Utils.FormatString("shushu send_event_harvest: step:{0} result:{1} reason:{2} coins:{3} gems:{4}", args));
        Dictionary<string, object> dictionary = new Dictionary<string, object> {
            { 
                "step",
                step
            },
            { 
                "result",
                result
            },
            { 
                "reason",
                reason
            },
            { 
                "coins",
                coins
            },
            { 
                "gems",
                gems
            }
        };
        TGAnalytics.TG.track("harvest", dictionary);
    }

    public static void send_event_http(string step)
    {
        Dictionary<string, object> dictionary = new Dictionary<string, object> {
            { 
                "step",
                step
            }
        };
        TGAnalytics.TG.track("http", dictionary);
    }

    public static void send_event_iap(string step, ShopOpenSource source, string product, string result, string reason)
    {
        object[] args = new object[] { step, source, product, result, reason };
        Log(Utils.FormatString("shushu send_event_iap: step:{0} source:{1} product:{2} result:{3} reason:{4}", args));
        Dictionary<string, object> dictionary = new Dictionary<string, object> {
            { 
                "step",
                step
            },
            { 
                "source",
                source.ToString().ToUpper()
            },
            { 
                "product",
                product.ToUpper()
            },
            { 
                "result",
                result
            },
            { 
                "reason",
                reason
            }
        };
        TGAnalytics.TG.track("iap", dictionary);
    }

    public static void send_event_mysteries(string step, int shoptype, int index, int equipment, int equipment1, int equipment2, int equipment3, int equipment4, int equipment5, int equipment6, int equipment7, int equipment8, int coins, int gems, string result, string reason)
    {
        if (equipment > 0)
        {
            switch (index)
            {
                case 0:
                    equipment1 = equipment;
                    break;

                case 1:
                    equipment2 = equipment;
                    break;

                case 2:
                    equipment3 = equipment;
                    break;

                case 3:
                    equipment4 = equipment;
                    break;

                case 4:
                    equipment5 = equipment;
                    break;

                case 5:
                    equipment6 = equipment;
                    break;

                case 6:
                    equipment7 = equipment;
                    break;

                case 7:
                    equipment8 = equipment;
                    break;
            }
            equipment = 0;
        }
        object[] args = new object[] { step, equipment, equipment1, equipment2, equipment3, equipment4, equipment5, equipment6, equipment7, equipment8, coins, gems, result, reason, shoptype };
        Log(Utils.FormatString("shushu send_event_mysteries: step:{0} shoptype:{14} equipment:{1} equipment1:{2} equipment2:{3} equipment3:{4} equipment4:{5} equipment5:{6} equipment6:{7} equipment7:{8} equipment8:{9} coins:{10} gems:{11} result:{12} reason:{13}", args));
        Dictionary<string, object> dictionary = new Dictionary<string, object> {
            { 
                "step",
                step
            },
            { 
                "shoptype",
                shoptype
            },
            { 
                "equipment",
                equipment
            },
            { 
                "equipment_1",
                equipment1
            },
            { 
                "equipment_2",
                equipment2
            },
            { 
                "equipment_3",
                equipment3
            },
            { 
                "equipment_4",
                equipment4
            },
            { 
                "equipment_5",
                equipment5
            },
            { 
                "equipment_6",
                equipment6
            },
            { 
                "equipment_7",
                equipment7
            },
            { 
                "equipment_8",
                equipment8
            },
            { 
                "coins",
                coins
            },
            { 
                "gems",
                gems
            },
            { 
                "result",
                result
            },
            { 
                "reason",
                reason
            }
        };
        TGAnalytics.TG.track("mysteries", dictionary);
    }

    public static void send_event_page_show(WindowID windowid, string step)
    {
        object[] args = new object[] { windowid, step };
        Log(Utils.FormatString("shushu send_event_page_show: page_name:{0} step:{1}", args));
        Dictionary<string, object> dictionary = new Dictionary<string, object> {
            { 
                "page_name",
                windowid.ToString().ToUpper()
            },
            { 
                "step",
                step
            }
        };
        TGAnalytics.TG.track("page", dictionary);
    }

    public static void send_event_revival(string result, int stage, int layer, int gems)
    {
        object[] args = new object[] { result, stage, layer, gems };
        Log(Utils.FormatString("shushu send_event_revival: result:{0} stage:{1} layer:{2} gems:{3}", args));
        Dictionary<string, object> dictionary = new Dictionary<string, object> {
            { 
                "result",
                result
            },
            { 
                "stage",
                stage
            },
            { 
                "layer",
                layer
            },
            { 
                "gems",
                gems
            }
        };
        TGAnalytics.TG.track("revival", dictionary);
    }

    public static void send_event_shop(string purchase, int coins, int gems, int equipment, int continue_count)
    {
        int beforePurchaseHour = GetBeforePurchaseHour();
        object[] args = new object[] { purchase, beforePurchaseHour, coins, gems, equipment };
        Log(Utils.FormatString("shushu send_event_shop: purchase:{0} hour:{1} coins:{2} gems:{3} equipment:{4}", args));
        Dictionary<string, object> dictionary = new Dictionary<string, object> {
            { 
                "purchase",
                purchase.ToUpper()
            },
            { 
                "time_since_last_purchase",
                beforePurchaseHour
            },
            { 
                "coins",
                coins
            },
            { 
                "gems",
                gems
            },
            { 
                "equipment",
                equipment
            },
            { 
                "count",
                continue_count
            }
        };
        TGAnalytics.TG.track("shop", dictionary);
    }

    public static void send_event_strength(string step, KeyBuySource source, string result, string reason, int gems)
    {
        object[] args = new object[] { step, source, result, reason, gems };
        Log(Utils.FormatString("shushu send_event_strength: step:{0} source:{1} result:{2} reason:{3} gems:{4}", args));
        Dictionary<string, object> dictionary = new Dictionary<string, object> {
            { 
                "step",
                step
            },
            { 
                "source",
                source.ToString().ToUpper()
            },
            { 
                "result",
                result
            },
            { 
                "reason",
                reason
            },
            { 
                "gems",
                gems
            }
        };
        TGAnalytics.TG.track("strength", dictionary);
    }

    public static void send_event_talent(string step, int talent, int target_level, int coins)
    {
        object[] args = new object[] { step, talent, target_level, coins };
        Log(Utils.FormatString("shushu send_event_talent: step:{0} talentid:{1} target_level:{2} coins:{3}", args));
        Dictionary<string, object> dictionary = new Dictionary<string, object> {
            { 
                "step",
                step
            },
            { 
                "talent",
                talent
            },
            { 
                "target_level",
                target_level
            },
            { 
                "coins",
                coins
            }
        };
        TGAnalytics.TG.track("talent", dictionary);
    }

    public static void set_first_setup_time()
    {
        if (!PlayerPrefsEncrypt.HasKey("set_first_setup_time"))
        {
            PlayerPrefsEncrypt.SetLong("set_first_setup_time", DateTime.Now.ToUniversalTime().Ticks);
        }
    }

    private static void ShuShu_Init()
    {
    }

    public static void ShuShu_Login(string name)
    {
        TGAnalytics.TG.login(name);
    }

    public static void TryLogin(Action<LoginType, LoginData> callback)
    {
        Google_TryLogin(callback);
    }

    private static void Umeng_Init()
    {
        Analytics.Start();
        Analytics.SetLogEnabled(false);
    }

    [CompilerGenerated]
    private sealed class <GameCenter_Login>c__AnonStorey0
    {
        internal Action<SdkManager.LoginData> callback;

        internal void <>m__0(bool success, string error_msg)
        {
            string id = Social.Active.localUser.id;
            string userName = Social.Active.localUser.userName;
            id = id.Replace(":", string.Empty);
            Debugger.Log(string.Concat(new object[] { "GameCenter Login Callback ", success, " userid = ", id }));
            SdkManager.LoginData data = new SdkManager.LoginData {
                username = userName
            };
            if (success && !string.IsNullOrEmpty(id))
            {
                string userID = LocalSave.Instance.GetUserID();
                if (!string.IsNullOrEmpty(userID) && id.Equals(userID))
                {
                    LocalSave.Instance.SetUserLoginSDK(true);
                    data.bSuccess = true;
                    data.userid = id;
                    data.username = userName;
                    Debugger.Log("GameCenter Login Callback userid = " + data.userid);
                    LocalSave.Instance.SetUserID(LoginType.eGameCenter, data.userid, data.username);
                }
                else
                {
                    WindowUI.TryLogin();
                }
            }
            this.callback(data);
        }
    }

    [CompilerGenerated]
    private sealed class <GameCenter_TryLogin>c__AnonStorey1
    {
        internal Action<LoginType, SdkManager.LoginData> callback;

        internal void <>m__0(bool success, string error_msg)
        {
            SdkManager.LoginData data = new SdkManager.LoginData();
            string id = Social.Active.localUser.id;
            string userName = Social.Active.localUser.userName;
            id = id.Replace(":", string.Empty);
            if (success && !string.IsNullOrEmpty(id))
            {
                data.userid = id;
                data.username = userName;
                LocalSave.Instance.SetUserLoginSDK(true);
            }
            this.callback(LoginType.eGameCenter, data);
        }
    }

    [CompilerGenerated]
    private sealed class <Google_Login>c__AnonStorey2
    {
        internal SdkManager.LoginData data;
        internal Action<SdkManager.LoginData> callback;

        internal void <>m__0(bool success, string error_msg)
        {
            if (success)
            {
                PlayGamesLocalUser localUser = (PlayGamesLocalUser) Social.Active.localUser;
                string email = localUser.Email;
                string idToken = localUser.GetIdToken();
                this.data.userid = localUser.id;
                this.data.username = localUser.userName;
                Debugger.Log("SdkManager_Google login google userName:" + localUser.userName);
                Debugger.Log("SdkManager_Google login google id:" + localUser.id);
                Debugger.Log("SdkManager_Google login google 登录成功");
                LocalSave.Instance.SetUserLoginSDK(true);
                LocalSave.Instance.SetUserID(LoginType.eGP, this.data.userid, this.data.username);
                this.data.bSuccess = true;
                this.data.userid = localUser.id;
            }
            else
            {
                Debug.Log("SdkManager_Google google Init return false.");
            }
            this.callback(this.data);
        }
    }

    [CompilerGenerated]
    private sealed class <Google_TryLogin>c__AnonStorey3
    {
        internal SdkManager.LoginData data;
        internal Action<LoginType, SdkManager.LoginData> callback;

        internal void <>m__0(bool success, string error_msg)
        {
            Debugger.Log("SdkManager_Google googlelogin Google_TryLogin start callback " + success);
            if (success)
            {
                Debugger.Log("SdkManager_Google Google_TryLogin success ");
                PlayGamesLocalUser localUser = (PlayGamesLocalUser) Social.Active.localUser;
                this.data.userid = localUser.id;
                this.data.username = localUser.userName;
            }
            else
            {
                Debugger.Log("SdkManager_Google Google_TryLogin fail ");
            }
            this.callback(LoginType.eGP, this.data);
        }
    }

    public class GAEventType
    {
        public static string DEAD_LAYER = "dead_layer";
    }

    public class LoginData
    {
        public bool bSuccess;
        public string userid;
        public string username;
    }
}

