using DG.Tweening;
using Dxx.Util;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using UnityEngine;
using UnityEngine.UI;

public class NetDoingUICtrl : MediatorCtrlBase
{
    public GameObject window;
    public Transform RotatingParent;
    public Image Image_Rotate;
    public Text Text_Count;
    public Text Text_Code;
    public Text Text_Loading;
    public CanvasGroup mCanvasGroup;
    private Sequence seq_load;
    private Sequence seq_delay;
    private int loadingindex;
    private RectTransform t;
    private string m_sCode = string.Empty;
    private NetDoingProxy.Transfer mTransfer;

    protected override void OnClose()
    {
        if (this.seq_load != null)
        {
            TweenExtensions.Kill(this.seq_load, false);
            this.seq_load = null;
        }
        if (this.seq_delay != null)
        {
            TweenExtensions.Kill(this.seq_delay, false);
            this.seq_delay = null;
        }
        Updater.RemoveUpdate("netdoing", new Action<float>(this.OnUpdate));
    }

    public override object OnGetEvent(string eventName) => 
        null;

    public override void OnHandleNotification(INotification notification)
    {
    }

    protected override void OnInit()
    {
        this.t = base.transform as RectTransform;
        this.t.sizeDelta = GameLogic.ScreenSize;
    }

    public override void OnLanguageChange()
    {
    }

    protected override void OnOpen()
    {
        IProxy proxy = Facade.Instance.RetrieveProxy("NetDoingProxy");
        this.mTransfer = proxy.Data as NetDoingProxy.Transfer;
        this.mCanvasGroup.alpha = 0f;
        this.seq_delay = TweenSettingsExtensions.SetUpdate<Sequence>(TweenSettingsExtensions.Append(TweenSettingsExtensions.AppendInterval(DOTween.Sequence(), 0.6f), ShortcutExtensions46.DOFade(this.mCanvasGroup, 0.6666667f, 1f)), true);
        this.SetLoading(0);
        this.seq_load = TweenSettingsExtensions.SetUpdate<Sequence>(TweenSettingsExtensions.SetLoops<Sequence>(TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(DOTween.Sequence(), 0.5f), new TweenCallback(this, this.<OnOpen>m__0)), -1), true);
        Updater.AddUpdate("netdoing", new Action<float>(this.OnUpdate), true);
    }

    private void OnUpdate(float delta)
    {
    }

    private void SetLoading(int index)
    {
        string languageByTID = GameLogic.Hold.Language.GetLanguageByTID(this.mTransfer.type.ToString(), Array.Empty<object>());
        string str2 = string.Empty;
        for (int i = 0; i < index; i++)
        {
            str2 = str2 + ".";
        }
        object[] args = new object[] { languageByTID, str2 };
        this.Text_Loading.text = Utils.FormatString("{0}{1}", args);
    }
}

