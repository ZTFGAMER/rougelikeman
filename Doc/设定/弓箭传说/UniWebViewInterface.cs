using System;
using UnityEngine;

public class UniWebViewInterface
{
    private static readonly AndroidJavaClass plugin;
    private static bool correctPlatform = (Application.platform == RuntimePlatform.Android);

    static UniWebViewInterface()
    {
        new GameObject("UniWebViewAndroidStaticListener").AddComponent<UniWebViewAndroidStaticListener>();
        plugin = new AndroidJavaClass("com.onevcat.uniwebview.UniWebViewInterface");
        CheckPlatform();
        plugin.CallStatic("prepare", Array.Empty<object>());
    }

    public static void AddJavaScript(string name, string jsString, string identifier)
    {
        CheckPlatform();
        object[] args = new object[] { name, jsString, identifier };
        plugin.CallStatic("addJavaScript", args);
    }

    public static void AddPermissionTrustDomain(string name, string domain)
    {
        CheckPlatform();
        object[] args = new object[] { name, domain };
        plugin.CallStatic("addPermissionTrustDomain", args);
    }

    public static void AddSslExceptionDomain(string name, string domain)
    {
        CheckPlatform();
        object[] args = new object[] { name, domain };
        plugin.CallStatic("addSslExceptionDomain", args);
    }

    public static void AddUrlScheme(string name, string scheme)
    {
        CheckPlatform();
        object[] args = new object[] { name, scheme };
        plugin.CallStatic("addUrlScheme", args);
    }

    public static bool AnimateTo(string name, int x, int y, int width, int height, float duration, float delay, string identifier)
    {
        CheckPlatform();
        object[] args = new object[] { name, x, y, width, height, duration, delay, identifier };
        return plugin.CallStatic<bool>("animateTo", args);
    }

    public static bool CanGoBack(string name)
    {
        CheckPlatform();
        object[] args = new object[] { name };
        return plugin.CallStatic<bool>("canGoBack", args);
    }

    public static bool CanGoForward(string name)
    {
        CheckPlatform();
        object[] args = new object[] { name };
        return plugin.CallStatic<bool>("canGoForward", args);
    }

    public static void CheckPlatform()
    {
        if (!correctPlatform)
        {
            throw new InvalidOperationException("Method can only be performed on Android.");
        }
    }

    public static void CleanCache(string name)
    {
        CheckPlatform();
        object[] args = new object[] { name };
        plugin.CallStatic("cleanCache", args);
    }

    public static void ClearCookies()
    {
        CheckPlatform();
        plugin.CallStatic("clearCookies", Array.Empty<object>());
    }

    public static void ClearHttpAuthUsernamePassword(string host, string realm)
    {
        CheckPlatform();
        object[] args = new object[] { host, realm };
        plugin.CallStatic("clearHttpAuthUsernamePassword", args);
    }

    public static void Destroy(string name)
    {
        CheckPlatform();
        object[] args = new object[] { name };
        plugin.CallStatic("destroy", args);
    }

    public static void EvaluateJavaScript(string name, string jsString, string identifier)
    {
        CheckPlatform();
        object[] args = new object[] { name, jsString, identifier };
        plugin.CallStatic("evaluateJavaScript", args);
    }

    public static string GetCookie(string url, string key, bool skipEncoding)
    {
        CheckPlatform();
        object[] args = new object[] { url, key };
        return plugin.CallStatic<string>("getCookie", args);
    }

    public static string GetUrl(string name)
    {
        CheckPlatform();
        object[] args = new object[] { name };
        return plugin.CallStatic<string>("getUrl", args);
    }

    public static string GetUserAgent(string name)
    {
        CheckPlatform();
        object[] args = new object[] { name };
        return plugin.CallStatic<string>("getUserAgent", args);
    }

    public static float GetWebViewAlpha(string name)
    {
        CheckPlatform();
        object[] args = new object[] { name };
        return plugin.CallStatic<float>("getWebViewAlpha", args);
    }

    public static void GoBack(string name)
    {
        CheckPlatform();
        object[] args = new object[] { name };
        plugin.CallStatic("goBack", args);
    }

    public static void GoForward(string name)
    {
        CheckPlatform();
        object[] args = new object[] { name };
        plugin.CallStatic("goForward", args);
    }

    public static bool Hide(string name, bool fade, int edge, float duration, string identifier)
    {
        CheckPlatform();
        object[] args = new object[] { name, fade, edge, duration, identifier };
        return plugin.CallStatic<bool>("hide", args);
    }

    public static void Init(string name, int x, int y, int width, int height)
    {
        CheckPlatform();
        object[] args = new object[] { name, x, y, width, height };
        plugin.CallStatic("init", args);
    }

    public static void Load(string name, string url, bool skipEncoding)
    {
        CheckPlatform();
        object[] args = new object[] { name, url };
        plugin.CallStatic("load", args);
    }

    public static void LoadHTMLString(string name, string html, string baseUrl, bool skipEncoding)
    {
        CheckPlatform();
        object[] args = new object[] { name, html, baseUrl };
        plugin.CallStatic("loadHTMLString", args);
    }

    public static void Print(string name)
    {
        CheckPlatform();
        object[] args = new object[] { name };
        plugin.CallStatic("print", args);
    }

    public static void Reload(string name)
    {
        CheckPlatform();
        object[] args = new object[] { name };
        plugin.CallStatic("reload", args);
    }

    public static void RemovePermissionTrustDomain(string name, string domain)
    {
        CheckPlatform();
        object[] args = new object[] { name, domain };
        plugin.CallStatic("removePermissionTrustDomain", args);
    }

    public static void RemoveSslExceptionDomain(string name, string domain)
    {
        CheckPlatform();
        object[] args = new object[] { name, domain };
        plugin.CallStatic("removeSslExceptionDomain", args);
    }

    public static void RemoveUrlScheme(string name, string scheme)
    {
        CheckPlatform();
        object[] args = new object[] { name, scheme };
        plugin.CallStatic("removeUrlScheme", args);
    }

    public static void SetAllowAutoPlay(bool flag)
    {
        CheckPlatform();
        object[] args = new object[] { flag };
        plugin.CallStatic("setAllowAutoPlay", args);
    }

    public static void SetAllowJavaScriptOpenWindow(bool flag)
    {
        CheckPlatform();
        object[] args = new object[] { flag };
        plugin.CallStatic("setAllowJavaScriptOpenWindow", args);
    }

    public static void SetBackButtonEnabled(string name, bool enabled)
    {
        CheckPlatform();
        object[] args = new object[] { name, enabled };
        plugin.CallStatic("setBackButtonEnabled", args);
    }

    public static void SetBackgroundColor(string name, float r, float g, float b, float a)
    {
        CheckPlatform();
        object[] args = new object[] { name, r, g, b, a };
        plugin.CallStatic("setBackgroundColor", args);
    }

    public static void SetBouncesEnabled(string name, bool enabled)
    {
        CheckPlatform();
        object[] args = new object[] { name, enabled };
        plugin.CallStatic("setBouncesEnabled", args);
    }

    public static void SetCookie(string url, string cookie, bool skipEncoding)
    {
        CheckPlatform();
        object[] args = new object[] { url, cookie };
        plugin.CallStatic("setCookie", args);
    }

    public static void SetFrame(string name, int x, int y, int width, int height)
    {
        CheckPlatform();
        object[] args = new object[] { name, x, y, width, height };
        plugin.CallStatic("setFrame", args);
    }

    public static void SetHeaderField(string name, string key, string value)
    {
        CheckPlatform();
        object[] args = new object[] { name, key, value };
        plugin.CallStatic("setHeaderField", args);
    }

    public static void SetHorizontalScrollBarEnabled(string name, bool enabled)
    {
        CheckPlatform();
        object[] args = new object[] { name, enabled };
        plugin.CallStatic("setHorizontalScrollBarEnabled", args);
    }

    public static void SetImmersiveModeEnabled(string name, bool enabled)
    {
        CheckPlatform();
        object[] args = new object[] { name, enabled };
        plugin.CallStatic("setImmersiveModeEnabled", args);
    }

    public static void SetJavaScriptEnabled(bool enabled)
    {
        CheckPlatform();
        object[] args = new object[] { enabled };
        plugin.CallStatic("setJavaScriptEnabled", args);
    }

    public static void SetLoadWithOverviewMode(string name, bool overview)
    {
        CheckPlatform();
        object[] args = new object[] { name, overview };
        plugin.CallStatic("setLoadWithOverviewMode", args);
    }

    public static void SetLogLevel(int level)
    {
        CheckPlatform();
        object[] args = new object[] { level };
        plugin.CallStatic("setLogLevel", args);
    }

    public static void SetOpenLinksInExternalBrowser(string name, bool flag)
    {
        CheckPlatform();
        object[] args = new object[] { name, flag };
        plugin.CallStatic("setOpenLinksInExternalBrowser", args);
    }

    public static void SetPosition(string name, int x, int y)
    {
        CheckPlatform();
        object[] args = new object[] { name, x, y };
        plugin.CallStatic("setPosition", args);
    }

    public static void SetShowSpinnerWhileLoading(string name, bool show)
    {
        CheckPlatform();
        object[] args = new object[] { name, show };
        plugin.CallStatic("setShowSpinnerWhileLoading", args);
    }

    public static void SetSize(string name, int width, int height)
    {
        CheckPlatform();
        object[] args = new object[] { name, width, height };
        plugin.CallStatic("setSize", args);
    }

    public static void SetSpinnerText(string name, string text)
    {
        CheckPlatform();
        object[] args = new object[] { name, text };
        plugin.CallStatic("setSpinnerText", args);
    }

    public static void SetUserAgent(string name, string userAgent)
    {
        CheckPlatform();
        object[] args = new object[] { name, userAgent };
        plugin.CallStatic("setUserAgent", args);
    }

    public static void SetUseWideViewPort(string name, bool use)
    {
        CheckPlatform();
        object[] args = new object[] { name, use };
        plugin.CallStatic("setUseWideViewPort", args);
    }

    public static void SetVerticalScrollBarEnabled(string name, bool enabled)
    {
        CheckPlatform();
        object[] args = new object[] { name, enabled };
        plugin.CallStatic("setVerticalScrollBarEnabled", args);
    }

    public static void SetWebContentsDebuggingEnabled(bool enabled)
    {
        CheckPlatform();
        object[] args = new object[] { enabled };
        plugin.CallStatic("setWebContentsDebuggingEnabled", args);
    }

    public static void SetWebViewAlpha(string name, float alpha)
    {
        CheckPlatform();
        object[] args = new object[] { name, alpha };
        plugin.CallStatic("setWebViewAlpha", args);
    }

    public static void SetZoomEnabled(string name, bool enabled)
    {
        CheckPlatform();
        object[] args = new object[] { name, enabled };
        plugin.CallStatic("setZoomEnabled", args);
    }

    public static bool Show(string name, bool fade, int edge, float duration, string identifier)
    {
        CheckPlatform();
        object[] args = new object[] { name, fade, edge, duration, identifier };
        return plugin.CallStatic<bool>("show", args);
    }

    public static void ShowWebViewDialog(string name, bool show)
    {
        CheckPlatform();
        object[] args = new object[] { name, show };
        plugin.CallStatic("showWebViewDialog", args);
    }

    public static void Stop(string name)
    {
        CheckPlatform();
        object[] args = new object[] { name };
        plugin.CallStatic("stop", args);
    }
}

