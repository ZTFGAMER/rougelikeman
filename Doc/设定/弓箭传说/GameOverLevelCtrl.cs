using DG.Tweening;
using Dxx.Net;
using GameProtocol;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class GameOverLevelCtrl : GameOverModeCtrlBase
{
    public Image Image_Title;
    public RectTransform Title_Left;
    public RectTransform Title_Right;
    public Image Image_BG;
    public GameObject stageparent;
    public GameObject goldparent;
    public GameObject bestparent;
    public GameObject getparent;
    public GameObject getparents;
    public RectTransform topnode;
    public Text Text_ReachLevel;
    public Text Text_Stage;
    public Text Text_Layer;
    public Text Text_GoldName;
    public Text Text_Get;
    public Text Text_Beat;
    public Text Text_Close;
    public Image Image_NewBest;
    public GoldTextCtrl mScoreCtrl;
    public ScrollRectBase mScrollRect;
    public Image viewpoint;
    public ButtonCtrl Button_Close;
    public GameOver_NoNetCtrl mNoNetCtrl;
    public GameOverChallengeCtrl mChallengeCtrl;
    public MainUIBattleLevelCtrl mLevelCtrl;
    private const float TextStartScale = 1.5f;
    private const float playTime = 0.15f;
    private const float DropWidth = 130f;
    private const float DropHeight = 130f;
    private const float DropTop = 10f;
    private const float DropTime = 0.12f;
    private const int LineCount = 5;
    private const float EquipScale = 0.9f;
    private int gochapter;
    private int gostage;
    private int alllayer;
    private bool bNewBest;
    private int getgold;
    private int getexp;
    private float imagebgy;
    private float imagetitlex;
    private GameObject copyitem;
    private LocalUnityObjctPool mPool;
    private List<PropOneEquip> mDropList = new List<PropOneEquip>();
    private List<LocalSave.EquipOne> mEquipDatas = new List<LocalSave.EquipOne>();
    private float startscale = 0.3f;
    private int mNetBackState;
    private bool bShowGot;

    private void AddEquipOne(Sequence s, int index)
    {
        <AddEquipOne>c__AnonStorey0 storey = new <AddEquipOne>c__AnonStorey0 {
            index = index,
            $this = this,
            one = null
        };
        TweenSettingsExtensions.AppendCallback(s, new TweenCallback(storey, this.<>m__0));
        TweenSettingsExtensions.AppendInterval(s, 0.22f);
    }

    private void AddGoldOne(Sequence s)
    {
        TweenSettingsExtensions.AppendCallback(s, new TweenCallback(this, this.<AddGoldOne>m__A));
        TweenSettingsExtensions.AppendInterval(s, 0.12f);
    }

    private void AnimationEnd()
    {
        TweenSettingsExtensions.SetUpdate<Sequence>(TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(DOTween.Sequence(), 0.2f), new TweenCallback(this, this.<AnimationEnd>m__E)), true);
    }

    private void excute_reward()
    {
        TurnTableType rewardType = GameLogic.Hold.BattleData.GetRewardType();
        int num = 1;
        int num2 = 1;
        switch (rewardType)
        {
            case TurnTableType.Reward_Gold2:
                num = 2;
                break;

            case TurnTableType.Reward_Gold3:
                num = 3;
                break;

            case TurnTableType.Reward_Item2:
                num2 = 2;
                break;

            case TurnTableType.Reward_Item3:
                num2 = 3;
                break;

            case TurnTableType.Reward_All2:
                num = 2;
                num2 = 2;
                break;

            case TurnTableType.Reward_All3:
                num = 3;
                num2 = 3;
                break;
        }
        this.getgold *= num;
        List<LocalSave.EquipOne> list = new List<LocalSave.EquipOne>();
        int num3 = 0;
        int count = this.mEquipDatas.Count;
        while (num3 < count)
        {
            LocalSave.EquipOne item = this.mEquipDatas[num3];
            if (item.Overlying)
            {
                for (int i = 0; i < num2; i++)
                {
                    item.Count *= num2;
                }
                list.Add(item);
            }
            else
            {
                for (int i = 0; i < num2; i++)
                {
                    list.Add(item);
                }
            }
            num3++;
        }
        this.mEquipDatas = list;
    }

    private int get_all_layer()
    {
        int allMaxLevel = LocalModelManager.Instance.Stage_Level_stagechapter.GetAllMaxLevel(this.gochapter - 1);
        return (this.gostage + allMaxLevel);
    }

    private bool HaveReward() => 
        ((this.getgold > 0) || (this.mEquipDatas.Count > 0));

    private void InitGet()
    {
        float num5;
        this.mScrollRect.verticalNormalizedPosition = 1f;
        Sequence s = DOTween.Sequence();
        TweenSettingsExtensions.SetUpdate<Sequence>(s, true);
        if (this.getgold > 0)
        {
            this.AddGoldOne(s);
        }
        int count = this.mEquipDatas.Count;
        for (int i = 0; i < count; i++)
        {
            this.AddEquipOne(s, i);
        }
        TweenSettingsExtensions.AppendCallback(s, new TweenCallback(this, this.<InitGet>m__9));
        float y = this.mScrollRect.get_content().sizeDelta.y;
        int num4 = count;
        if (this.getgold > 0)
        {
            num4++;
        }
        if ((num4 % 5) == 0)
        {
            num5 = (num4 / 5) * 130f;
        }
        else
        {
            num5 = ((num4 / 5) * 130f) + 130f;
        }
        y = num5;
        this.mScrollRect.get_content().sizeDelta = new Vector2(this.mScrollRect.get_content().sizeDelta.x, y);
        this.viewpoint.enabled = num4 > 10;
    }

    private void InitItem()
    {
        this.copyitem = CInstance<UIResourceCreator>.Instance.GetPropOneEquip(base.transform).gameObject;
        this.copyitem.SetActive(false);
        this.mPool = LocalUnityObjctPool.Create(base.gameObject);
        this.mPool.CreateCache<PropOneEquip>(this.copyitem);
    }

    private void OnClickClose()
    {
        WindowUI.ShowLoading(delegate {
            WindowUI.ShowWindow(WindowID.WindowID_Main);
            LocalSave.Instance.Modify_Gold((long) this.getgold, false);
            this.PlayRewards();
        }, null, null, BattleLoadProxy.LoadingType.eMiss);
    }

    protected override void OnClose()
    {
        this.mPool.Collect<PropOneEquip>();
        this.mDropList.Clear();
    }

    public override object OnGetEvent(string eventName) => 
        base.OnGetEvent(eventName);

    public override void OnHandleNotification(INotification notification)
    {
    }

    protected override void OnInit()
    {
        this.imagebgy = this.Image_BG.get_rectTransform().sizeDelta.y;
        this.imagetitlex = this.Image_Title.get_rectTransform().sizeDelta.x;
        this.InitItem();
        this.Button_Close.onClick = new Action(this.OnClickClose);
    }

    public override void OnLanguageChange()
    {
        this.Text_ReachLevel.text = GameLogic.Hold.Language.GetLanguageByTID("GameOver_Reach", Array.Empty<object>());
        this.Text_GoldName.text = GameLogic.Hold.Language.GetLanguageByTID("GameOver_Gold", Array.Empty<object>());
        int layer = LocalModelManager.Instance.Stage_Level_stagechapter.GetAllMaxLevel(this.gochapter - 1) + this.gostage;
        string beat = LocalModelManager.Instance.Beat_beat.GetBeat(layer);
        object[] args = new object[] { beat };
        this.Text_Beat.text = GameLogic.Hold.Language.GetLanguageByTID("击败", args);
        this.Text_Close.text = GameLogic.Hold.Language.GetLanguageByTID("TapToClose", Array.Empty<object>());
        this.Text_Get.text = GameLogic.Hold.Language.GetLanguageByTID("GameOver_Items", Array.Empty<object>());
        this.mNoNetCtrl.OnLanguageUpdate();
    }

    protected override void OnOpen()
    {
        this.mNetBackState = 0;
        this.bShowGot = false;
        this.mChallengeCtrl.Show(false);
        if (GameLogic.Hold.BattleData.Challenge_ismainchallenge())
        {
            this.mChallengeCtrl.Show(true);
            LocalSave.Instance.Achieve_ExcuteCurrentStage();
            if (LocalSave.Instance.Achieve_IsFinish(GameLogic.Hold.BattleData.ActiveID))
            {
                this.mChallengeCtrl.SetContent("挑战成功...");
            }
            else
            {
                this.mChallengeCtrl.SetContent("挑战失败...");
            }
        }
        this.mPool.Collect<PropOneEquip>();
        this.mNoNetCtrl.SetShow(false);
        this.topnode.anchoredPosition = new Vector2(0f, -200f);
        this.Image_NewBest.gameObject.SetActive(false);
        this.Text_ReachLevel.gameObject.SetActive(false);
        this.stageparent.gameObject.SetActive(false);
        this.bestparent.gameObject.SetActive(false);
        this.getparent.gameObject.SetActive(false);
        this.getparents.gameObject.SetActive(false);
        this.Button_Close.gameObject.SetActive(false);
        this.Text_Close.gameObject.SetActive(false);
        this.Image_Title.gameObject.SetActive(false);
        this.Image_BG.gameObject.SetActive(false);
        this.Title_Left.gameObject.SetActive(false);
        this.Title_Right.gameObject.SetActive(false);
        this.mLevelCtrl.gameObject.SetActive(false);
        this.Image_NewBest.transform.localScale = Vector3.one * this.startscale;
        this.Text_ReachLevel.transform.localScale = Vector3.one * this.startscale;
        this.Image_Title.get_rectTransform().sizeDelta = new Vector2(0f, this.Image_Title.get_rectTransform().sizeDelta.y);
        this.Image_BG.get_rectTransform().sizeDelta = new Vector2(this.Image_BG.get_rectTransform().sizeDelta.x, 0f);
        this.Title_Left.transform.localScale = Vector3.one;
        this.Title_Right.transform.localScale = Vector3.one;
        this.stageparent.transform.localScale = Vector3.one * this.startscale;
        this.bestparent.transform.localScale = Vector3.one * this.startscale;
        this.getparent.transform.localScale = Vector3.one * 1.5f;
        this.getparents.transform.localScale = Vector3.one * 1.5f;
        this.mLevelCtrl.transform.localScale = Vector3.one * this.startscale;
        this.gochapter = GameLogic.Hold.BattleData.Level_CurrentStage;
        this.gostage = GameLogic.Hold.BattleData.GetLayer();
        this.getexp = LocalModelManager.Instance.Stage_Level_stagechapter.GetExp();
        this.mLevelCtrl.UpdateLevel();
        this.getgold = (int) GameLogic.Hold.BattleData.GetGold();
        this.alllayer = this.get_all_layer();
        LocalSave.Instance.Stage_CheckUnlockNext(this.gostage);
        LocalSave.Instance.mStage.UpdateMaxLevel(this.alllayer);
        this.bNewBest = LocalSave.Instance.mStage.bNewBestLevel;
        LocalSave.Instance.mStage.bNewBestLevel = false;
        this.mEquipDatas = GameLogic.Hold.BattleData.GetEquips();
        this.excute_reward();
        int val = PlayerPrefsEncrypt.GetInt("game_end_newbest", 0) + 1;
        int newbest = 0;
        if (this.bNewBest)
        {
            newbest = 0;
            PlayerPrefsEncrypt.SetInt("game_end_newbest", 0);
        }
        else
        {
            PlayerPrefsEncrypt.SetInt("game_end_newbest", val);
            newbest = val;
        }
        int num3 = GameConfig.GetRebornCount() - GameLogic.Hold.BattleData.GetRebornCount();
        int levelUpCount = LocalModelManager.Instance.Character_Level.GetLevelUpCount(this.getexp);
        int level = LocalSave.Instance.GetLevel() + levelUpCount;
        SdkManager.send_event_game_end(num3, BattleSource.eWorld, BattleEndType.EMAIN_GAMEOVER, this.getgold, this.mEquipDatas.Count, this.gochapter, this.alllayer, newbest, this.getexp, levelUpCount, level);
        this.UpdateUI();
        this.SendGameOver();
    }

    private void PlayGet()
    {
        this.PlayGetInternal();
    }

    private void PlayGetInternal()
    {
        if (!this.bShowGot)
        {
            this.bShowGot = true;
            Sequence sequence = DOTween.Sequence();
            TweenSettingsExtensions.SetUpdate<Sequence>(sequence, true);
            TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(this, this.<PlayGetInternal>m__B));
            TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetUpdate<Tweener>(ShortcutExtensions.DOScale(this.getparent.transform, Vector3.one, 0.15f), true));
            TweenSettingsExtensions.AppendInterval(sequence, 0.15f);
            TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(this, this.<PlayGetInternal>m__C));
            TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetUpdate<Tweener>(ShortcutExtensions.DOScale(this.getparents.transform, Vector3.one, 0.15f), true));
            TweenSettingsExtensions.AppendInterval(sequence, 0.15f);
            TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(this, this.<PlayGetInternal>m__D));
        }
    }

    public void PlayRewards()
    {
        if (this.getgold > 0)
        {
            Facade.Instance.SendNotification("MainUI_GetGold", this.getgold);
        }
    }

    private void SendGameOver()
    {
        Drop_DropModel.DropData data;
        if (this.mEquipDatas.Count > 0)
        {
            LocalSave.Instance.mGuideData.check_diamondbox_first_open();
        }
        int num = 0;
        int count = this.mEquipDatas.Count;
        while (num < count)
        {
            LocalSave.EquipOne one = this.mEquipDatas[num];
            SdkManager.send_event_equipment("GET", one.EquipID, one.Count, 1, EquipSource.EMain_battle, 0);
            num++;
        }
        List<Drop_DropModel.DropData> list = new List<Drop_DropModel.DropData>();
        if (this.getgold > 0)
        {
            data = new Drop_DropModel.DropData {
                type = PropType.eCurrency,
                id = 1,
                count = this.getgold
            };
            list.Add(data);
        }
        if (this.getexp > 0)
        {
            data = new Drop_DropModel.DropData {
                type = PropType.eCurrency,
                id = 0x3e9,
                count = this.getexp
            };
            list.Add(data);
        }
        int num3 = 0;
        int num4 = this.mEquipDatas.Count;
        while (num3 < num4)
        {
            data = new Drop_DropModel.DropData {
                type = PropType.eEquip,
                id = this.mEquipDatas[num3].EquipID,
                count = this.mEquipDatas[num3].Count
            };
            list.Add(data);
            num3++;
        }
        CReqItemPacket itemPacket = NetManager.GetItemPacket(list, true);
        itemPacket.m_nPacketType = 1;
        itemPacket.m_nExtraInfo = (uint) this.alllayer;
        NetManager.SendInternal<CReqItemPacket>(itemPacket, SendType.eCache, delegate (NetResponse response) {
            if (response.IsSuccess)
            {
                this.mNetBackState = 2;
            }
            else
            {
                this.mNetBackState = 1;
            }
        });
    }

    private void UpdateImageTitleLeftRight()
    {
        float x = this.Image_Title.get_rectTransform().sizeDelta.x;
        this.Title_Left.anchoredPosition = new Vector2((-x / 2f) - 20f, this.Title_Left.anchoredPosition.y);
        this.Title_Right.anchoredPosition = new Vector2((x / 2f) + 20f, this.Title_Right.anchoredPosition.y);
    }

    private void UpdateUI()
    {
        this.Image_Title.get_rectTransform().sizeDelta = new Vector2(0f, this.Image_Title.get_rectTransform().sizeDelta.y);
        this.Image_BG.get_rectTransform().sizeDelta = new Vector2(this.Image_BG.get_rectTransform().sizeDelta.x, 0f);
        this.UpdateImageTitleLeftRight();
        object[] args = new object[] { this.gochapter };
        this.Text_Stage.text = GameLogic.Hold.Language.GetLanguageByTID("ChapterIndex_x", args);
        this.Text_Layer.text = this.gostage.ToString();
        float num = 0.2f;
        Sequence seqparent = DOTween.Sequence();
        TweenSettingsExtensions.SetUpdate<Sequence>(seqparent, true);
        TweenSettingsExtensions.AppendInterval(seqparent, 0.2f);
        if (this.bNewBest)
        {
            TweenSettingsExtensions.AppendCallback(seqparent, new TweenCallback(this, this.<UpdateUI>m__1));
            TweenSettingsExtensions.SetUpdate<Sequence>(TweenSettingsExtensions.Append(seqparent, TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.SetUpdate<Tweener>(ShortcutExtensions.DOScale(this.Image_NewBest.transform, 1f, 0.3f), true), 0x1b)), true);
        }
        TweenSettingsExtensions.AppendCallback(seqparent, new TweenCallback(this, this.<UpdateUI>m__2));
        TweenSettingsExtensions.Append(seqparent, TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.SetUpdate<Tweener>(TweenSettingsExtensions.OnUpdate<Tweener>(ShortcutExtensions46.DOSizeDelta(this.Image_Title.get_rectTransform(), new Vector2(this.imagetitlex, this.Image_Title.get_rectTransform().sizeDelta.y), 0.4f, false), new TweenCallback(this, this.UpdateImageTitleLeftRight)), true), 0x1b));
        Sequence sequence2 = TweenSettingsExtensions.SetUpdate<Sequence>(TweenSettingsExtensions.Append(TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(DOTween.Sequence(), 0.25f), new TweenCallback(this, this.<UpdateUI>m__3)), TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.SetUpdate<Tweener>(ShortcutExtensions46.DOSizeDelta(this.Image_BG.get_rectTransform(), new Vector2(this.Image_BG.get_rectTransform().sizeDelta.x, this.imagebgy), 0.3f, false), true), 0x1b)), true);
        TweenSettingsExtensions.Join(seqparent, sequence2);
        TweenSettingsExtensions.AppendInterval(seqparent, 0.3f);
        TweenSettingsExtensions.AppendCallback(seqparent, new TweenCallback(this, this.<UpdateUI>m__4));
        TweenSettingsExtensions.Append(seqparent, TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.SetUpdate<Tweener>(ShortcutExtensions.DOScale(this.Text_ReachLevel.transform, 1f, num), true), 0x1b));
        TweenSettingsExtensions.AppendCallback(seqparent, new TweenCallback(this, this.<UpdateUI>m__5));
        TweenSettingsExtensions.Append(seqparent, TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.SetUpdate<Tweener>(ShortcutExtensions.DOScale(this.stageparent.transform, 1f, num), true), 0x1b));
        TweenSettingsExtensions.AppendCallback(seqparent, new TweenCallback(this, this.<UpdateUI>m__6));
        TweenSettingsExtensions.Append(seqparent, TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.SetUpdate<Tweener>(ShortcutExtensions.DOScale(this.mLevelCtrl.transform, 0.9f, num), true), 0x1b));
        TweenSettingsExtensions.AppendInterval(seqparent, 0.1f);
        if (this.getexp > 0)
        {
            this.mLevelCtrl.AddExpAnimation(this.getexp, seqparent);
        }
        TweenSettingsExtensions.AppendCallback(seqparent, new TweenCallback(this, this.<UpdateUI>m__7));
        TweenSettingsExtensions.Append(seqparent, TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.SetUpdate<Tweener>(ShortcutExtensions.DOScale(this.bestparent.transform, 1f, num), true), 0x1b));
        TweenSettingsExtensions.AppendInterval(seqparent, num * 0.5f);
        if (this.HaveReward())
        {
            TweenSettingsExtensions.Append(seqparent, TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.SetUpdate<Tweener>(ShortcutExtensions46.DOAnchorPosY(this.topnode, -30f, num, false), true), 1));
            TweenSettingsExtensions.AppendInterval(seqparent, num * 0.5f);
            TweenSettingsExtensions.AppendCallback(seqparent, new TweenCallback(this, this.<UpdateUI>m__8));
        }
        else
        {
            TweenSettingsExtensions.AppendCallback(seqparent, new TweenCallback(this, this.AnimationEnd));
        }
    }

    [CompilerGenerated]
    private sealed class <AddEquipOne>c__AnonStorey0
    {
        internal PropOneEquip one;
        internal int index;
        internal GameOverLevelCtrl $this;

        internal void <>m__0()
        {
            this.one = this.$this.mPool.DeQueue<PropOneEquip>();
            this.one.SetAlreadyGet(true);
            this.one.InitEquip(this.$this.mEquipDatas[this.index].EquipID, this.$this.mEquipDatas[this.index].Count);
            this.one.transform.SetParentNormal(this.$this.mScrollRect.get_content());
            this.one.transform.SetTop();
            this.$this.mDropList.Add(this.one);
            this.one.transform.localScale = Vector3.one * 1.5f;
            TweenSettingsExtensions.SetUpdate<Tweener>(ShortcutExtensions.DOScale(this.one.transform, Vector3.one * 0.9f, 0.12f), true);
            if (this.$this.getgold > 0)
            {
                this.index++;
            }
            float x = -260f + ((this.index % 5) * 130f);
            float y = ((this.index / 5) * -130f) - 10f;
            (this.one.transform as RectTransform).anchoredPosition = new Vector3(x, y);
        }
    }
}

