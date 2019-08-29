using System;
using System.Collections.Generic;
using UnityEngine;

public class UniWebViewNativeListener : MonoBehaviour
{
    private static Dictionary<string, UniWebViewNativeListener> listeners = new Dictionary<string, UniWebViewNativeListener>();
    [HideInInspector]
    public UniWebView webView;

    public void AddJavaScriptFinished(string result)
    {
        UniWebViewLogger.Instance.Info("Add JavaScript Finished Event. Result: " + result);
        UniWebViewNativeResultPayload payload = JsonUtility.FromJson<UniWebViewNativeResultPayload>(result);
        this.webView.InternalOnAddJavaScriptFinished(payload);
    }

    public static void AddListener(UniWebViewNativeListener target)
    {
        listeners.Add(target.Name, target);
    }

    public void AnimateToFinished(string identifer)
    {
        UniWebViewLogger.Instance.Info("Animate To Finished Event. Identifier: " + identifer);
        this.webView.InternalOnAnimateToFinished(identifer);
    }

    public void EvalJavaScriptFinished(string result)
    {
        UniWebViewLogger.Instance.Info("Eval JavaScript Finished Event. Result: " + result);
        UniWebViewNativeResultPayload payload = JsonUtility.FromJson<UniWebViewNativeResultPayload>(result);
        this.webView.InternalOnEvalJavaScriptFinished(payload);
    }

    public static UniWebViewNativeListener GetListener(string name)
    {
        UniWebViewNativeListener listener = null;
        if (listeners.TryGetValue(name, out listener))
        {
            return listener;
        }
        return null;
    }

    public void HideTransitionFinished(string identifer)
    {
        UniWebViewLogger.Instance.Info("Hide Transition Finished Event. Identifier: " + identifer);
        this.webView.InternalOnHideTransitionFinished(identifer);
    }

    public void MessageReceived(string result)
    {
        UniWebViewLogger.Instance.Info("Message Received Event. Result: " + result);
        this.webView.InternalOnMessageReceived(result);
    }

    public void PageErrorReceived(string result)
    {
        UniWebViewLogger.Instance.Info("Page Error Received Event. Result: " + result);
        UniWebViewNativeResultPayload payload = JsonUtility.FromJson<UniWebViewNativeResultPayload>(result);
        this.webView.InternalOnPageErrorReceived(payload);
    }

    public void PageFinished(string result)
    {
        UniWebViewLogger.Instance.Info("Page Finished Event. Url: " + result);
        UniWebViewNativeResultPayload payload = JsonUtility.FromJson<UniWebViewNativeResultPayload>(result);
        this.webView.InternalOnPageFinished(payload);
    }

    public void PageStarted(string url)
    {
        UniWebViewLogger.Instance.Info("Page Started Event. Url: " + url);
        this.webView.InternalOnPageStarted(url);
    }

    public static void RemoveListener(string name)
    {
        listeners.Remove(name);
    }

    public void ShowTransitionFinished(string identifer)
    {
        UniWebViewLogger.Instance.Info("Show Transition Finished Event. Identifier: " + identifer);
        this.webView.InternalOnShowTransitionFinished(identifer);
    }

    public void WebContentProcessDidTerminate(string param)
    {
        UniWebViewLogger.Instance.Info("Web Content Process Terminate Event.");
        this.webView.InternalWebContentProcessDidTerminate();
    }

    public void WebViewDone(string param)
    {
        UniWebViewLogger.Instance.Info("Web View Done Event.");
        this.webView.InternalOnShouldClose();
    }

    public void WebViewKeyDown(string keyCode)
    {
        UniWebViewLogger.Instance.Info("Web View Key Down: " + keyCode);
        if (int.TryParse(keyCode, out int num))
        {
            this.webView.InternalOnWebViewKeyDown(num);
        }
        else
        {
            UniWebViewLogger.Instance.Critical("Failed in converting key code: " + keyCode);
        }
    }

    public string Name =>
        base.gameObject.name;
}

