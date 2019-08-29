using Dxx.Util;
using Newtonsoft.Json;
using PureMVC.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class ReportUICtrl : MediatorCtrlBase
{
    public Text Text_Title;
    public ButtonCtrl Button_Close;
    public ButtonCtrl Button_Shadow;
    public RectTransform viewparent;
    public UniWebView mView;
    private float width;
    private float height;
    private string url = "http://feedback.habby.fun/";
    [CompilerGenerated]
    private static Action <>f__am$cache0;
    [CompilerGenerated]
    private static Action<UniWebViewNativeResultPayload> <>f__am$cache1;
    [CompilerGenerated]
    private static Action<UniWebViewNativeResultPayload> <>f__am$cache2;

    private string get_player_info()
    {
        PlayerInfo info = new PlayerInfo {
            platform = PlatformHelper.GetPlatformID(),
            sdklogintype = SdkManager.GetLoginType(),
            serveruserid = LocalSave.Instance.GetServerUserID(),
            serveruseridsub = LocalSave.Instance.GetServerUserIDSub(),
            sdkloginid = LocalSave.Instance.GetUserID(),
            uuid = PlatformHelper.GetUUID(),
            source = 0
        };
        return JsonConvert.SerializeObject(info);
    }

    private void InitUI()
    {
        base.StartCoroutine("start_load");
    }

    protected override void OnClose()
    {
        this.mView.CleanCache();
        base.StopCoroutine("start_load");
    }

    public override object OnGetEvent(string eventName) => 
        null;

    public override void OnHandleNotification(INotification notification)
    {
    }

    protected override void OnInit()
    {
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = () => WindowUI.CloseWindow(WindowID.WindowID_Report);
        }
        this.Button_Close.onClick = <>f__am$cache0;
        this.Button_Shadow.onClick = this.Button_Close.onClick;
        this.mView.Init(GameNode.m_Front.GetComponent<Canvas>());
        this.mView.OnMessageReceived += new UniWebView.MessageReceivedDelegate(this.OnMessageReceived);
        this.mView.OnPageStarted += new UniWebView.PageStartedDelegate(this.OnPageStarted);
        this.mView.OnPageFinished += new UniWebView.PageFinishedDelegate(this.OnPageFinished);
        this.mView.OnKeyCodeReceived += new UniWebView.KeyCodeReceivedDelegate(this.OnKeyCodeReceived);
        this.mView.OnPageErrorReceived += new UniWebView.PageErrorReceivedDelegate(this.OnPageErrorReceived);
        this.mView.OnShouldClose += new UniWebView.ShouldCloseDelegate(this.OnShouldClose);
        this.mView.SetBackButtonEnabled(false);
        this.mView.SetHorizontalScrollBarEnabled(false);
        this.mView.SetVerticalScrollBarEnabled(true);
        this.mView.SetBouncesEnabled(false);
        UniWebView.SetJavaScriptEnabled(true);
        this.width = this.mView.Frame.width;
        this.height = this.mView.Frame.height;
    }

    private void OnKeyCodeReceived(UniWebView webView, int keyCode)
    {
        Debugger.Log("WebView", "OnKeyCodeReceived keycode:" + keyCode);
    }

    public override void OnLanguageChange()
    {
        this.Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("setting_report", Array.Empty<object>());
    }

    private void OnMessageReceived(UniWebView webView, UniWebViewMessage message)
    {
        Debugger.Log("WebView", "OnMessageReceived :" + message.RawMessage);
        if (message.Path.Equals("game"))
        {
            string s = message.Args["score"];
            string str2 = message.Args["name"];
            Debug.Log("Your final score is: " + s + "name :" + str2);
            if (this.mView.isActiveAndEnabled)
            {
                string jsString = $"openParamOne({(int.Parse(s) * 2) + int.Parse(str2)});";
                if (<>f__am$cache2 == null)
                {
                    <>f__am$cache2 = delegate (UniWebViewNativeResultPayload payload) {
                        if (payload.resultCode.Equals("0"))
                        {
                            Debug.Log("Game Started!=========>");
                        }
                        else
                        {
                            Debug.Log("Something goes wrong: " + payload.data);
                        }
                    };
                }
                this.mView.EvaluateJavaScript(jsString, <>f__am$cache2);
            }
        }
    }

    protected override void OnOpen()
    {
        this.InitUI();
    }

    private void OnPageErrorReceived(UniWebView webView, int errorCode, string errorMessage)
    {
        Debugger.Log("WebView", "OnPageErrorReceived ：" + $"errorCode:{errorCode},errorMessage{errorMessage}");
    }

    private void OnPageFinished(UniWebView webView, int statusCode, string url)
    {
        Debugger.Log("WebView", "OnPageFinished statusCode:" + $"statusCode:{statusCode},url{url}");
        object[] args = new object[] { this.get_player_info() };
        if (<>f__am$cache1 == null)
        {
            <>f__am$cache1 = delegate (UniWebViewNativeResultPayload response) {
                if (response != null)
                {
                    Debugger.Log("WebView", "EvaluateJavaScript response  identifier    : " + response.identifier);
                    Debugger.Log("WebView", "EvaluateJavaScript response  resultCode    : " + response.resultCode);
                    Debugger.Log("WebView", "EvaluateJavaScript response  data          : " + response.data);
                }
                else
                {
                    Debugger.Log("WebView", "response is null!");
                }
            };
        }
        this.mView.EvaluateJavaScript(Utils.FormatString("get_player_info('{0}');", args), <>f__am$cache1);
    }

    private void OnPageStarted(UniWebView webView, string url)
    {
        Debugger.Log("WebView", "OnPageStarted " + url);
    }

    private bool OnShouldClose(UniWebView webView)
    {
        Debugger.Log("WebView", "OnShouldClose");
        this.mView.CleanCache();
        return true;
    }

    [DebuggerHidden]
    private IEnumerator start_load() => 
        new <start_load>c__Iterator0 { $this = this };

    [CompilerGenerated]
    private sealed class <start_load>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal ReportUICtrl $this;
        internal object $current;
        internal bool $disposing;
        internal int $PC;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$disposing = true;
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    this.$this.mView.Show(false, UniWebViewTransitionEdge.None, 0.4f, null);
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    return true;

                case 1:
                    this.$this.mView.Load(this.$this.url, false);
                    this.$PC = -1;
                    break;
            }
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current =>
            this.$current;

        object IEnumerator.Current =>
            this.$current;
    }

    [Serializable]
    public class PlayerInfo
    {
        public ulong serveruserid;
        public string serveruseridsub;
        public string uuid;
        public int platform;
        public int sdklogintype;
        public string sdkloginid;
        public int source;
    }
}

