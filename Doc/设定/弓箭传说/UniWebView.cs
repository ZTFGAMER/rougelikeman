using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;

public class UniWebView : MonoBehaviour
{
    private string id = Guid.NewGuid().ToString();
    private UniWebViewNativeListener listener;
    private bool isPortrait;
    [SerializeField]
    private string urlOnStart;
    [SerializeField]
    private bool showOnStart;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();
    private Dictionary<string, Action<UniWebViewNativeResultPayload>> payloadActions = new Dictionary<string, Action<UniWebViewNativeResultPayload>>();
    [SerializeField]
    private bool fullScreen;
    [SerializeField]
    private Rect frame;
    [SerializeField]
    private RectTransform referenceRectTransform;
    [SerializeField]
    private bool useToolbar;
    [SerializeField]
    private UniWebViewToolbarPosition toolbarPosition;
    private bool started;
    private Canvas mCanvas;
    private Color backgroundColor = Color.white;

    [field: CompilerGenerated, DebuggerBrowsable(0)]
    public event KeyCodeReceivedDelegate OnKeyCodeReceived;

    [field: CompilerGenerated, DebuggerBrowsable(0)]
    public event MessageReceivedDelegate OnMessageReceived;

    [Obsolete("OnOreintationChanged is a typo and deprecated. Use `OnOrientationChanged` instead.", true)]
    [field: CompilerGenerated, DebuggerBrowsable(0)]
    public event OrientationChangedDelegate OnOreintationChanged;

    [field: CompilerGenerated, DebuggerBrowsable(0)]
    public event OrientationChangedDelegate OnOrientationChanged;

    [field: CompilerGenerated, DebuggerBrowsable(0)]
    public event PageErrorReceivedDelegate OnPageErrorReceived;

    [field: CompilerGenerated, DebuggerBrowsable(0)]
    public event PageFinishedDelegate OnPageFinished;

    [field: CompilerGenerated, DebuggerBrowsable(0)]
    public event PageStartedDelegate OnPageStarted;

    [field: CompilerGenerated, DebuggerBrowsable(0)]
    public event ShouldCloseDelegate OnShouldClose;

    [field: CompilerGenerated, DebuggerBrowsable(0)]
    public event OnWebContentProcessTerminatedDelegate OnWebContentProcessTerminated;

    private void _init()
    {
        Rect rect;
        GameObject obj2 = new GameObject(this.id);
        this.listener = obj2.AddComponent<UniWebViewNativeListener>();
        obj2.transform.parent = base.transform;
        this.listener.webView = this;
        UniWebViewNativeListener.AddListener(this.listener);
        if (this.fullScreen)
        {
            rect = new Rect(0f, 0f, (float) GameLogic.ScreenWidth, (float) GameLogic.ScreenHeight);
        }
        else
        {
            rect = this.NextFrameRect();
        }
        UniWebViewInterface.Init(this.listener.Name, (int) rect.x, (int) rect.y, (int) rect.width, (int) rect.height);
        this.isPortrait = GameLogic.ScreenHeight >= GameLogic.ScreenWidth;
    }

    public void AddJavaScript(string jsString, Action<UniWebViewNativeResultPayload> completionHandler = null)
    {
        string identifier = Guid.NewGuid().ToString();
        UniWebViewInterface.AddJavaScript(this.listener.Name, jsString, identifier);
        if (completionHandler != null)
        {
            this.payloadActions.Add(identifier, completionHandler);
        }
    }

    public void AddPermissionTrustDomain(string domain)
    {
        UniWebViewInterface.AddPermissionTrustDomain(this.listener.Name, domain);
    }

    public void AddSslExceptionDomain(string domain)
    {
        if (domain == null)
        {
            UniWebViewLogger.Instance.Critical("The domain should not be null.");
        }
        else if (domain.Contains("://"))
        {
            UniWebViewLogger.Instance.Critical("The domain should not include invalid characters '://'");
        }
        else
        {
            UniWebViewInterface.AddSslExceptionDomain(this.listener.Name, domain);
        }
    }

    public void AddUrlScheme(string scheme)
    {
        if (scheme == null)
        {
            UniWebViewLogger.Instance.Critical("The scheme should not be null.");
        }
        else if (scheme.Contains("://"))
        {
            UniWebViewLogger.Instance.Critical("The scheme should not include invalid characters '://'");
        }
        else
        {
            UniWebViewInterface.AddUrlScheme(this.listener.Name, scheme);
        }
    }

    public bool AnimateTo(Rect frame, float duration, float delay = 0f, Action completionHandler = null)
    {
        string identifier = Guid.NewGuid().ToString();
        bool flag = UniWebViewInterface.AnimateTo(this.listener.Name, (int) frame.x, (int) frame.y, (int) frame.width, (int) frame.height, duration, delay, identifier);
        if (flag)
        {
            this.frame = frame;
            if (completionHandler != null)
            {
                this.actions.Add(identifier, completionHandler);
            }
        }
        return flag;
    }

    public void CleanCache()
    {
        UniWebViewInterface.CleanCache(this.listener.Name);
    }

    public static void ClearCookies()
    {
        UniWebViewInterface.ClearCookies();
    }

    public static void ClearHttpAuthUsernamePassword(string host, string realm)
    {
        UniWebViewInterface.ClearHttpAuthUsernamePassword(host, realm);
    }

    public void EvaluateJavaScript(string jsString, Action<UniWebViewNativeResultPayload> completionHandler = null)
    {
        string identifier = Guid.NewGuid().ToString();
        UniWebViewInterface.EvaluateJavaScript(this.listener.Name, jsString, identifier);
        if (completionHandler != null)
        {
            this.payloadActions.Add(identifier, completionHandler);
        }
    }

    public static string GetCookie(string url, string key, bool skipEncoding = false) => 
        UniWebViewInterface.GetCookie(url, key, skipEncoding);

    public void GetHTMLContent(Action<string> handler)
    {
        <GetHTMLContent>c__AnonStorey0 storey = new <GetHTMLContent>c__AnonStorey0 {
            handler = handler
        };
        this.EvaluateJavaScript("document.documentElement.outerHTML", new Action<UniWebViewNativeResultPayload>(storey.<>m__0));
    }

    public string GetUserAgent() => 
        UniWebViewInterface.GetUserAgent(this.listener.Name);

    public void GoBack()
    {
        UniWebViewInterface.GoBack(this.listener.Name);
    }

    public void GoForward()
    {
        UniWebViewInterface.GoForward(this.listener.Name);
    }

    public bool Hide(bool fade = false, UniWebViewTransitionEdge edge = 0, float duration = 0.4f, Action completionHandler = null)
    {
        string identifier = Guid.NewGuid().ToString();
        bool flag = UniWebViewInterface.Hide(this.listener.Name, fade, (int) edge, duration, identifier);
        if (flag && (completionHandler != null))
        {
            if (fade || (edge != UniWebViewTransitionEdge.None))
            {
                this.actions.Add(identifier, completionHandler);
            }
            else
            {
                completionHandler();
            }
        }
        if (flag && this.useToolbar)
        {
            bool onTop = this.toolbarPosition == UniWebViewToolbarPosition.Top;
            this.SetShowToolbar(false, false, onTop, this.fullScreen);
        }
        return flag;
    }

    public void Init(Canvas canvas)
    {
        this.mCanvas = canvas;
        this._init();
    }

    internal void InternalOnAddJavaScriptFinished(UniWebViewNativeResultPayload payload)
    {
        string identifier = payload.identifier;
        if (this.payloadActions.TryGetValue(identifier, out Action<UniWebViewNativeResultPayload> action))
        {
            action(payload);
            this.payloadActions.Remove(identifier);
        }
    }

    internal void InternalOnAnimateToFinished(string identifier)
    {
        if (this.actions.TryGetValue(identifier, out Action action))
        {
            action();
            this.actions.Remove(identifier);
        }
    }

    internal void InternalOnEvalJavaScriptFinished(UniWebViewNativeResultPayload payload)
    {
        string identifier = payload.identifier;
        if (this.payloadActions.TryGetValue(identifier, out Action<UniWebViewNativeResultPayload> action))
        {
            action(payload);
            this.payloadActions.Remove(identifier);
        }
    }

    internal void InternalOnHideTransitionFinished(string identifier)
    {
        if (this.actions.TryGetValue(identifier, out Action action))
        {
            action();
            this.actions.Remove(identifier);
        }
    }

    internal void InternalOnMessageReceived(string result)
    {
        UniWebViewMessage message = new UniWebViewMessage(result);
        if (this.OnMessageReceived != null)
        {
            this.OnMessageReceived(this, message);
        }
    }

    internal void InternalOnPageErrorReceived(UniWebViewNativeResultPayload payload)
    {
        if (this.OnPageErrorReceived != null)
        {
            int result = -1;
            if (int.TryParse(payload.resultCode, out result))
            {
                this.OnPageErrorReceived(this, result, payload.data);
            }
            else
            {
                UniWebViewLogger.Instance.Critical("Invalid error code received: " + payload.resultCode);
            }
        }
    }

    internal void InternalOnPageFinished(UniWebViewNativeResultPayload payload)
    {
        if (this.OnPageFinished != null)
        {
            int result = -1;
            if (int.TryParse(payload.resultCode, out result))
            {
                this.OnPageFinished(this, result, payload.data);
            }
            else
            {
                UniWebViewLogger.Instance.Critical("Invalid status code received: " + payload.resultCode);
            }
        }
    }

    internal void InternalOnPageStarted(string url)
    {
        if (this.OnPageStarted != null)
        {
            this.OnPageStarted(this, url);
        }
    }

    internal void InternalOnShouldClose()
    {
        if (this.OnShouldClose != null)
        {
            if (this.OnShouldClose(this))
            {
                Object.Destroy(this);
            }
        }
        else
        {
            Object.Destroy(this);
        }
    }

    internal void InternalOnShowTransitionFinished(string identifier)
    {
        if (this.actions.TryGetValue(identifier, out Action action))
        {
            action();
            this.actions.Remove(identifier);
        }
    }

    internal void InternalOnWebViewKeyDown(int keyCode)
    {
        if (this.OnKeyCodeReceived != null)
        {
            this.OnKeyCodeReceived(this, keyCode);
        }
    }

    internal void InternalWebContentProcessDidTerminate()
    {
        if (this.OnWebContentProcessTerminated != null)
        {
            this.OnWebContentProcessTerminated(this);
        }
    }

    public void Load(string url, bool skipEncoding = false)
    {
        UniWebViewInterface.Load(this.listener.Name, url, skipEncoding);
    }

    public void LoadHTMLString(string htmlString, string baseUrl, bool skipEncoding = false)
    {
        UniWebViewInterface.LoadHTMLString(this.listener.Name, htmlString, baseUrl, skipEncoding);
    }

    private Rect NextFrameRect()
    {
        if (this.referenceRectTransform == null)
        {
            Debugger.Log("WebView", "referenceRectTransform == null");
            UniWebViewLogger.Instance.Info("Using Frame setting to determine web view frame.");
            return this.frame;
        }
        UniWebViewLogger.Instance.Info("Using reference RectTransform to determine web view frame.");
        Vector3[] fourCornersArray = new Vector3[4];
        this.referenceRectTransform.GetWorldCorners(fourCornersArray);
        Vector3 position = fourCornersArray[0];
        Vector3 vector2 = fourCornersArray[1];
        Vector3 vector3 = fourCornersArray[2];
        Vector3 vector4 = fourCornersArray[3];
        Canvas mCanvas = this.mCanvas;
        RenderMode renderMode = mCanvas.renderMode;
        if ((renderMode != RenderMode.ScreenSpaceOverlay) && ((renderMode == RenderMode.ScreenSpaceCamera) || (renderMode == RenderMode.WorldSpace)))
        {
            Camera worldCamera = mCanvas.worldCamera;
            if (worldCamera == null)
            {
                UniWebViewLogger.Instance.Critical("You need a render camera \r\n                        or event camera to use RectTransform to determine correct \r\n                        frame for UniWebView.");
                UniWebViewLogger.Instance.Info("No camera found. Fall back to ScreenSpaceOverlay mode.");
            }
            else
            {
                position = worldCamera.WorldToScreenPoint(position);
                vector2 = worldCamera.WorldToScreenPoint(vector2);
                vector3 = worldCamera.WorldToScreenPoint(vector3);
                vector4 = worldCamera.WorldToScreenPoint(vector4);
                if (Screen.width != GameLogic.ScreenWidth)
                {
                    float num = ((float) GameLogic.ScreenWidth) / ((float) Screen.width);
                    float num2 = ((float) GameLogic.ScreenHeight) / ((float) Screen.height);
                    float num3 = position.x * num;
                    float num4 = vector2.y * num2;
                    float num5 = (vector3.x / ((float) Screen.width)) * GameLogic.ScreenWidth;
                    float num6 = (position.y / ((float) Screen.height)) * GameLogic.ScreenHeight;
                    position = new Vector3(num3, num6, position.z);
                    vector2 = new Vector3(num3, num4, vector2.z);
                    vector3 = new Vector3(num5, num4, vector3.z);
                    vector4 = new Vector3(num5, num6, vector4.z);
                }
            }
        }
        float x = vector2.x;
        float y = GameLogic.ScreenHeight - vector2.y;
        float width = vector4.x - vector2.x;
        return new Rect(x, y, width, vector2.y - vector4.y);
    }

    private void OnApplicationPause(bool pause)
    {
        UniWebViewInterface.ShowWebViewDialog(this.listener.Name, !pause);
    }

    private void OnDestroy()
    {
        UniWebViewNativeListener.RemoveListener(this.listener.Name);
        UniWebViewInterface.Destroy(this.listener.Name);
        Object.Destroy(this.listener.gameObject);
    }

    private void OnDisable()
    {
        if (this.started)
        {
            this.Hide(false, UniWebViewTransitionEdge.None, 0.4f, null);
            UniWebViewInterface.ShowWebViewDialog(this.listener.Name, false);
        }
    }

    private void OnEnable()
    {
        if (this.started)
        {
            UniWebViewInterface.ShowWebViewDialog(this.listener.Name, true);
            this.Show(false, UniWebViewTransitionEdge.None, 0.4f, null);
        }
    }

    public void Print()
    {
        UniWebViewInterface.Print(this.listener.name);
    }

    public void Reload()
    {
        UniWebViewInterface.Reload(this.listener.Name);
    }

    public void RemovePermissionTrustDomain(string domain)
    {
        UniWebViewInterface.RemovePermissionTrustDomain(this.listener.Name, domain);
    }

    public void RemoveSslExceptionDomain(string domain)
    {
        if (domain == null)
        {
            UniWebViewLogger.Instance.Critical("The domain should not be null.");
        }
        else if (domain.Contains("://"))
        {
            UniWebViewLogger.Instance.Critical("The domain should not include invalid characters '://'");
        }
        else
        {
            UniWebViewInterface.RemoveSslExceptionDomain(this.listener.Name, domain);
        }
    }

    public void RemoveUrlScheme(string scheme)
    {
        if (scheme == null)
        {
            UniWebViewLogger.Instance.Critical("The scheme should not be null.");
        }
        else if (scheme.Contains("://"))
        {
            UniWebViewLogger.Instance.Critical("The scheme should not include invalid characters '://'");
        }
        else
        {
            UniWebViewInterface.RemoveUrlScheme(this.listener.Name, scheme);
        }
    }

    public static void SetAllowAutoPlay(bool flag)
    {
        UniWebViewInterface.SetAllowAutoPlay(flag);
    }

    public void SetAllowFileAccessFromFileURLs(bool flag)
    {
    }

    public static void SetAllowInlinePlay(bool flag)
    {
    }

    public static void SetAllowJavaScriptOpenWindow(bool flag)
    {
        UniWebViewInterface.SetAllowJavaScriptOpenWindow(flag);
    }

    public void SetBackButtonEnabled(bool enabled)
    {
        UniWebViewInterface.SetBackButtonEnabled(this.listener.Name, enabled);
    }

    public void SetBouncesEnabled(bool enabled)
    {
        UniWebViewInterface.SetBouncesEnabled(this.listener.Name, enabled);
    }

    public static void SetCookie(string url, string cookie, bool skipEncoding = false)
    {
        UniWebViewInterface.SetCookie(url, cookie, skipEncoding);
    }

    public void SetHeaderField(string key, string value)
    {
        if (key == null)
        {
            UniWebViewLogger.Instance.Critical("Header key should not be null.");
        }
        else
        {
            UniWebViewInterface.SetHeaderField(this.listener.Name, key, value);
        }
    }

    public void SetHorizontalScrollBarEnabled(bool enabled)
    {
        UniWebViewInterface.SetHorizontalScrollBarEnabled(this.listener.Name, enabled);
    }

    public void SetImmersiveModeEnabled(bool enabled)
    {
        UniWebViewInterface.SetImmersiveModeEnabled(this.listener.Name, enabled);
    }

    public static void SetJavaScriptEnabled(bool enabled)
    {
        UniWebViewInterface.SetJavaScriptEnabled(enabled);
    }

    public void SetLoadWithOverviewMode(bool flag)
    {
        UniWebViewInterface.SetLoadWithOverviewMode(this.listener.Name, flag);
    }

    public void SetOpenLinksInExternalBrowser(bool flag)
    {
        UniWebViewInterface.SetOpenLinksInExternalBrowser(this.listener.Name, flag);
    }

    public void SetShowSpinnerWhileLoading(bool flag)
    {
        UniWebViewInterface.SetShowSpinnerWhileLoading(this.listener.Name, flag);
    }

    public void SetShowToolbar(bool show, bool animated = false, bool onTop = true, bool adjustInset = false)
    {
    }

    public void SetSpinnerText(string text)
    {
        UniWebViewInterface.SetSpinnerText(this.listener.Name, text);
    }

    public void SetToolbarDoneButtonText(string text)
    {
    }

    public void SetUserAgent(string agent)
    {
        UniWebViewInterface.SetUserAgent(this.listener.Name, agent);
    }

    public void SetUseWideViewPort(bool flag)
    {
        UniWebViewInterface.SetUseWideViewPort(this.listener.Name, flag);
    }

    public void SetVerticalScrollBarEnabled(bool enabled)
    {
        UniWebViewInterface.SetVerticalScrollBarEnabled(this.listener.Name, enabled);
    }

    public static void SetWebContentsDebuggingEnabled(bool enabled)
    {
        UniWebViewInterface.SetWebContentsDebuggingEnabled(enabled);
    }

    public void SetWindowUserResizeEnabled(bool enabled)
    {
    }

    public void SetZoomEnabled(bool enabled)
    {
        UniWebViewInterface.SetZoomEnabled(this.listener.Name, enabled);
    }

    public bool Show(bool fade = false, UniWebViewTransitionEdge edge = 0, float duration = 0.4f, Action completionHandler = null)
    {
        string identifier = Guid.NewGuid().ToString();
        bool flag = UniWebViewInterface.Show(this.listener.Name, fade, (int) edge, duration, identifier);
        if (flag && (completionHandler != null))
        {
            if (fade || (edge != UniWebViewTransitionEdge.None))
            {
                this.actions.Add(identifier, completionHandler);
            }
            else
            {
                completionHandler();
            }
        }
        if (flag && this.useToolbar)
        {
            bool onTop = this.toolbarPosition == UniWebViewToolbarPosition.Top;
            this.SetShowToolbar(true, false, onTop, this.fullScreen);
        }
        return flag;
    }

    private void Start()
    {
        if (this.showOnStart)
        {
            this.Show(false, UniWebViewTransitionEdge.None, 0.4f, null);
        }
        if (!string.IsNullOrEmpty(this.urlOnStart))
        {
            this.Load(this.urlOnStart, false);
        }
        this.started = true;
        if (this.referenceRectTransform != null)
        {
            this.UpdateFrame();
        }
    }

    public void Stop()
    {
        UniWebViewInterface.Stop(this.listener.Name);
    }

    private void Update()
    {
        bool flag = GameLogic.ScreenHeight >= GameLogic.ScreenWidth;
        if (this.isPortrait != flag)
        {
            this.isPortrait = flag;
            if (this.OnOrientationChanged != null)
            {
                this.OnOrientationChanged(this, !this.isPortrait ? ScreenOrientation.LandscapeLeft : ScreenOrientation.Portrait);
            }
            if (this.referenceRectTransform != null)
            {
                this.UpdateFrame();
            }
        }
    }

    public void UpdateFrame()
    {
        Rect rect = this.NextFrameRect();
        UniWebViewInterface.SetFrame(this.listener.Name, (int) rect.x, (int) rect.y, (int) rect.width, (int) rect.height);
    }

    public Rect Frame
    {
        get => 
            this.frame;
        set
        {
            this.frame = value;
            this.UpdateFrame();
        }
    }

    public RectTransform ReferenceRectTransform
    {
        get => 
            this.referenceRectTransform;
        set
        {
            this.referenceRectTransform = value;
            this.UpdateFrame();
        }
    }

    public string Url =>
        UniWebViewInterface.GetUrl(this.listener.Name);

    public bool CanGoBack =>
        UniWebViewInterface.CanGoBack(this.listener.name);

    public bool CanGoForward =>
        UniWebViewInterface.CanGoForward(this.listener.name);

    public Color BackgroundColor
    {
        get => 
            this.backgroundColor;
        set
        {
            this.backgroundColor = value;
            UniWebViewInterface.SetBackgroundColor(this.listener.Name, value.r, value.g, value.b, value.a);
        }
    }

    public float Alpha
    {
        get => 
            UniWebViewInterface.GetWebViewAlpha(this.listener.Name);
        set => 
            UniWebViewInterface.SetWebViewAlpha(this.listener.Name, value);
    }

    [CompilerGenerated]
    private sealed class <GetHTMLContent>c__AnonStorey0
    {
        internal Action<string> handler;

        internal void <>m__0(UniWebViewNativeResultPayload payload)
        {
            if (this.handler != null)
            {
                this.handler(payload.data);
            }
        }
    }

    public delegate void KeyCodeReceivedDelegate(UniWebView webView, int keyCode);

    public delegate void MessageReceivedDelegate(UniWebView webView, UniWebViewMessage message);

    public delegate void OnWebContentProcessTerminatedDelegate(UniWebView webView);

    [Obsolete("OreintationChangedDelegate is a typo and deprecated. Use `OrientationChangedDelegate` instead.", true)]
    public delegate void OreintationChangedDelegate(UniWebView webView, ScreenOrientation orientation);

    public delegate void OrientationChangedDelegate(UniWebView webView, ScreenOrientation orientation);

    public delegate void PageErrorReceivedDelegate(UniWebView webView, int errorCode, string errorMessage);

    public delegate void PageFinishedDelegate(UniWebView webView, int statusCode, string url);

    public delegate void PageStartedDelegate(UniWebView webView, string url);

    public delegate bool ShouldCloseDelegate(UniWebView webView);
}

