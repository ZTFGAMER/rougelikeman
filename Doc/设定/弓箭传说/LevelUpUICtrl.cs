using DG.Tweening;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpUICtrl : MediatorCtrlBase
{
    public Text Text_LevelUp;
    public Transform LevelUpItem;
    public Transform LevelItem;
    public Text Text_Level;
    public UILineCtrl mLineCtrl;
    public RectTransform rewardparent;
    public TapToCloseCtrl mCloseCtrl;
    public GameObject copyitems;
    public GameObject copyreward;
    private const float ShowScale = 1.5f;
    private const float playTime = 0.3f;
    private LevelUpProxy.Transfer mTransfer;
    private LocalUnityObjctPool mPool;
    private List<GoldTextCtrl> mRewards = new List<GoldTextCtrl>();
    private int adddiamond;
    [CompilerGenerated]
    private static Action <>f__am$cache0;

    private void InitUI()
    {
        Sequence sequence = DOTween.Sequence();
        TweenSettingsExtensions.SetUpdate<Sequence>(sequence, true);
        TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(this, this.<InitUI>m__1));
        TweenSettingsExtensions.AppendInterval(sequence, 0.3f);
        TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(this, this.<InitUI>m__2));
        TweenSettingsExtensions.AppendInterval(sequence, 0.3f);
        TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(this, this.<InitUI>m__3));
        TweenSettingsExtensions.AppendInterval(sequence, 0.3f);
        int num = 0;
        int count = this.mRewards.Count;
        while (num < count)
        {
            <InitUI>c__AnonStorey0 storey = new <InitUI>c__AnonStorey0 {
                $this = this,
                index = num
            };
            TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(storey, this.<>m__0));
            TweenSettingsExtensions.AppendInterval(sequence, 0.3f);
            num++;
        }
        TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(this, this.<InitUI>m__4));
    }

    protected override void OnClose()
    {
        GameLogic.SetPause(false);
        if ((this.mTransfer != null) && (this.mTransfer.onclose != null))
        {
            this.mTransfer.onclose();
        }
    }

    public override object OnGetEvent(string eventName) => 
        null;

    public override void OnHandleNotification(INotification notification)
    {
    }

    protected override void OnInit()
    {
        this.mPool = LocalUnityObjctPool.Create(base.gameObject);
        this.mPool.CreateCache<GoldTextCtrl>(this.copyreward);
        this.copyitems.SetActive(false);
    }

    public override void OnLanguageChange()
    {
        object[] args = new object[] { this.mTransfer.level };
        this.Text_LevelUp.text = GameLogic.Hold.Language.GetLanguageByTID("levelup_title", args);
    }

    protected override void OnOpen()
    {
        GameLogic.SetPause(true);
        this.mCloseCtrl.Show(false);
        this.LevelUpItem.localScale = Vector3.zero;
        this.LevelItem.localScale = Vector3.zero;
        this.mLineCtrl.transform.localScale = Vector3.zero;
        this.mLineCtrl.SetFontSize(40);
        this.mLineCtrl.SetText(GameLogic.Hold.Language.GetLanguageByTID("levelup_rewards", Array.Empty<object>()));
        this.mPool.Collect<GoldTextCtrl>();
        this.mRewards.Clear();
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = () => WindowUI.CloseWindow(WindowID.WindowID_LevelUp);
        }
        this.mCloseCtrl.OnClose = <>f__am$cache0;
        IProxy proxy = Facade.Instance.RetrieveProxy("LevelUpProxy");
        if (proxy == null)
        {
            SdkManager.Bugly_Report("LevelUpProxy", "WindowID_LevelUp LevelUpProxy dont transfer!");
        }
        this.mTransfer = (LevelUpProxy.Transfer) proxy.Data;
        this.Text_Level.text = this.mTransfer.level.ToString();
        Character_Level beanById = LocalModelManager.Instance.Character_Level.GetBeanById(this.mTransfer.level);
        this.adddiamond = 0;
        for (int i = 0; i < beanById.Rewards.Length; i++)
        {
            string str = beanById.Rewards[i];
            Drop_DropModel.DropSaveOneData dropOne = Drop_DropModel.GetDropOne(str);
            if (dropOne.type == 1)
            {
                GoldTextCtrl item = this.mPool.DeQueue<GoldTextCtrl>();
                item.SetCurrencyType(dropOne.id);
                item.SetAdd(dropOne.count);
                RectTransform transform = item.transform as RectTransform;
                ((Transform) transform).SetParentNormal((Transform) this.rewardparent);
                transform.localScale = Vector3.zero;
                transform.anchoredPosition = new Vector2(0f, (float) (this.mRewards.Count * -70));
                this.mRewards.Add(item);
            }
        }
        LocalSave.Instance.Modify_drop(beanById.Rewards);
        this.InitUI();
    }

    [CompilerGenerated]
    private sealed class <InitUI>c__AnonStorey0
    {
        internal int index;
        internal LevelUpUICtrl $this;

        internal void <>m__0()
        {
            if ((this.index < this.$this.mRewards.Count) && (this.$this.mRewards[this.index] != null))
            {
                this.$this.mRewards[this.index].transform.localScale = Vector3.one * 1.5f;
                TweenSettingsExtensions.SetUpdate<Tweener>(ShortcutExtensions.DOScale(this.$this.mRewards[this.index].transform, Vector3.one, 0.225f), true);
            }
        }
    }
}

