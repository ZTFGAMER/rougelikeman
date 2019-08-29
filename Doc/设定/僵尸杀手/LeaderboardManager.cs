using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static LeaderboardManager <instance>k__BackingField;
    [HideInInspector]
    public static bool loginSuccessful;
    public static bool autoSignIn;
    private string leaderboardID;
    [SerializeField]
    private string IosLeaderboardID;
    [SerializeField]
    private string AndroidLeaderboardID;
    [CompilerGenerated]
    private static Action<bool> <>f__am$cache0;

    public void AuthenticateUser()
    {
        this.StartTryLogin();
        Social.localUser.Authenticate(delegate (bool success) {
            loginSuccessful = success;
            if (success)
            {
                UnityEngine.Debug.Log("Successfully Authenticated");
                UIOptions options = UnityEngine.Object.FindObjectOfType<UIOptions>();
                if (options != null)
                {
                    options.RefreshGooglePlayUI();
                }
                this.LoginSuccess();
                GPGSCloudSave.CloudSync(true);
            }
            else
            {
                UnityEngine.Debug.Log("Authentication failed");
                GPGSCloudSave.syncWithCloud = true;
            }
        });
    }

    private void Awake()
    {
        instance = this;
        this.leaderboardID = this.AndroidLeaderboardID;
    }

    private void LoginSuccess()
    {
        autoSignIn = true;
        PlayerPrefs.SetInt(StaticConstants.autoSignIn, Convert.ToInt32(true));
        PlayerPrefs.Save();
        UnityEngine.Debug.Log("LoginSuccess | autoSigIn - " + autoSignIn);
    }

    private void OnApplicationPause(bool pause)
    {
    }

    public void PostScoreOnLeaderBoard(long myScore)
    {
        <PostScoreOnLeaderBoard>c__AnonStorey0 storey = new <PostScoreOnLeaderBoard>c__AnonStorey0 {
            myScore = myScore,
            $this = this
        };
        if (loginSuccessful)
        {
            GPGSCloudSave.CloudSync(false);
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = delegate (bool success) {
                    if (success)
                    {
                        UnityEngine.Debug.Log("Scores successfully uploaded");
                    }
                };
            }
            Social.ReportScore(storey.myScore, this.leaderboardID, <>f__am$cache0);
        }
        else
        {
            UnityEngine.Debug.Log("PostScoreOnLeaderBoard | autoSigIn - " + autoSignIn);
            if (autoSignIn)
            {
                this.StartTryLogin();
                Social.localUser.Authenticate(new Action<bool>(storey.<>m__0));
            }
        }
    }

    public void ShowLeaderboard()
    {
        if (loginSuccessful)
        {
            PlayGamesPlatform.Instance.ShowLeaderboardUI(this.leaderboardID);
        }
        else
        {
            this.AuthenticateUser();
        }
    }

    public void SignOut()
    {
        if (PlayGamesPlatform.Instance.localUser.authenticated)
        {
            PlayGamesPlatform.Instance.SignOut();
            loginSuccessful = false;
        }
    }

    private void Start()
    {
        PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build());
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        if (!PlayerPrefs.HasKey(StaticConstants.autoSignIn))
        {
            PlayerPrefs.SetInt(StaticConstants.autoSignIn, Convert.ToInt32(true));
            PlayerPrefs.Save();
            autoSignIn = true;
        }
        else
        {
            autoSignIn = Convert.ToBoolean(PlayerPrefs.GetInt(StaticConstants.autoSignIn));
        }
        if (autoSignIn)
        {
            this.AuthenticateUser();
        }
        else
        {
            GPGSCloudSave.syncWithCloud = true;
        }
    }

    private void StartTryLogin()
    {
        autoSignIn = false;
        PlayerPrefs.SetInt(StaticConstants.autoSignIn, Convert.ToInt32(false));
        PlayerPrefs.Save();
        UnityEngine.Debug.Log("StartTryLogin | autoSigIn - " + autoSignIn);
    }

    public static LeaderboardManager instance
    {
        [CompilerGenerated]
        get => 
            <instance>k__BackingField;
        [CompilerGenerated]
        private set => 
            (<instance>k__BackingField = value);
    }

    [CompilerGenerated]
    private sealed class <PostScoreOnLeaderBoard>c__AnonStorey0
    {
        internal long myScore;
        internal LeaderboardManager $this;
        private static Action<bool> <>f__am$cache0;

        internal void <>m__0(bool success)
        {
            if (success)
            {
                LeaderboardManager.loginSuccessful = true;
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = delegate (bool successful) {
                        if (successful)
                        {
                            UnityEngine.Debug.Log("Scores successfully uploaded");
                        }
                    };
                }
                Social.ReportScore(this.myScore, this.$this.leaderboardID, <>f__am$cache0);
                this.$this.LoginSuccess();
            }
            else
            {
                UnityEngine.Debug.Log("Authentication failed");
                GPGSCloudSave.syncWithCloud = true;
            }
        }

        private static void <>m__1(bool successful)
        {
            if (successful)
            {
                UnityEngine.Debug.Log("Scores successfully uploaded");
            }
        }
    }
}

