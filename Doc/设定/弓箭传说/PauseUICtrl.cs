using DG.Tweening;
using Dxx.Util;
using PureMVC.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class PauseUICtrl : MediatorCtrlBase
{
    public ButtonCtrl Button_Sound;
    public ButtonCtrl Button_Continue;
    public ButtonCtrl Button_Home;
    public Image SoundIcon;
    public ScrollRect mScrollRect;
    public RectTransform mScrollContent;
    public Text Text_Title;
    public UILineCtrl mLineCtrl;
    private Sequence seq;
    private GameObject copyitem;
    private LocalUnityObjctPool mPool;
    [CompilerGenerated]
    private static Action <>f__am$cache0;
    [CompilerGenerated]
    private static Action <>f__am$cache1;
    [CompilerGenerated]
    private static Action <>f__am$cache2;
    [CompilerGenerated]
    private static Action <>f__am$cache3;

    private void InitSkills()
    {
        <InitSkills>c__AnonStorey1 storey = new <InitSkills>c__AnonStorey1 {
            $this = this
        };
        if (GameLogic.Self != null)
        {
            this.KillSequence();
            this.mPool.Collect<PauseUISkillIconCtrl>();
            storey.skills = GameLogic.Self.GetSkillList();
            for (int i = storey.skills.Count - 1; (i >= 0) && (i < storey.skills.Count); i--)
            {
                int key = storey.skills[i];
                if (LocalModelManager.Instance.Skill_skill.GetBeanById(key).SkillIcon == 0)
                {
                    object[] args = new object[] { key };
                    SdkManager.Bugly_Report("PauseUICtrl", Utils.FormatString("Player Skill Have a error SkillID:::{0}", args));
                    storey.skills.RemoveAt(i);
                }
            }
            int count = storey.skills.Count;
            this.mScrollRect.movementType = (count <= 10) ? ScrollRect.MovementType.Clamped : ScrollRect.MovementType.Elastic;
            storey.width = 120;
            storey.height = 120;
            storey.startx = -240;
            int num4 = ((count / 5) * 120) + 10;
            if ((count % 5) > 0)
            {
                num4 += 120;
            }
            this.mScrollContent.sizeDelta = new Vector2(this.mScrollContent.sizeDelta.x, (float) num4);
            this.seq = DOTween.Sequence();
            TweenSettingsExtensions.SetUpdate<Sequence>(this.seq, true);
            storey.starty = (num4 / 2) - 0x41;
            for (int j = 0; j < count; j++)
            {
                <InitSkills>c__AnonStorey0 storey2 = new <InitSkills>c__AnonStorey0 {
                    <>f__ref$1 = storey,
                    index = j
                };
                TweenSettingsExtensions.AppendCallback(this.seq, new TweenCallback(storey2, this.<>m__0));
                TweenSettingsExtensions.AppendInterval(this.seq, 0.1f);
            }
        }
    }

    private void InitUI()
    {
        this.UpdateSound();
        this.InitSkills();
    }

    private void KillSequence()
    {
        if (this.seq != null)
        {
            TweenExtensions.Kill(this.seq, false);
            this.seq = null;
        }
    }

    protected override void OnClose()
    {
        this.KillSequence();
        GameLogic.SetPause(false);
    }

    public override object OnGetEvent(string eventName) => 
        null;

    public override void OnHandleNotification(INotification notification)
    {
    }

    protected override void OnInit()
    {
        this.copyitem = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>("UIPanel/PauseUI/SkillLearnOne"));
        this.copyitem.SetParentNormal(base.gameObject);
        this.mPool = LocalUnityObjctPool.Create(base.gameObject);
        this.mPool.CreateCache<PauseUISkillIconCtrl>(this.copyitem);
        this.copyitem.SetActive(false);
        this.Button_Sound.onClick = delegate {
            try
            {
                GameLogic.Hold.Sound.ChangeSound();
                this.UpdateSound();
            }
            catch
            {
                SdkManager.Bugly_Report("PauseUI", "Sound Button error");
            }
        };
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = delegate {
                try
                {
                    WindowUI.CloseWindow(WindowID.WindowID_Pause);
                }
                catch
                {
                    SdkManager.Bugly_Report("PauseUI", "Continue Button error");
                }
            };
        }
        this.Button_Continue.onClick = <>f__am$cache0;
        if (<>f__am$cache1 == null)
        {
            <>f__am$cache1 = delegate {
                string languageByTID = GameLogic.Hold.Language.GetLanguageByTID("popwindow_returnhome_title", Array.Empty<object>());
                string content = GameLogic.Hold.Language.GetLanguageByTID("popwindow_returnhome_content", Array.Empty<object>());
                if (<>f__am$cache2 == null)
                {
                    <>f__am$cache2 = delegate {
                        GameLogic.Hold.BattleData.SetWin(false);
                        int coins = 0;
                        try
                        {
                            coins = (int) GameLogic.Hold.BattleData.GetGold();
                        }
                        catch
                        {
                            SdkManager.Bugly_Report("PauseUI", "Home Button gold get error");
                        }
                        int equipment = 0;
                        try
                        {
                            equipment = GameLogic.Hold.BattleData.GetEquips().Count;
                        }
                        catch
                        {
                            SdkManager.Bugly_Report("PauseUI", "Home Button equipcount get error");
                        }
                        int num3 = GameLogic.Hold.BattleData.Level_CurrentStage;
                        int currentRoomID = 0;
                        try
                        {
                            currentRoomID = GameLogic.Release.Mode.GetCurrentRoomID();
                        }
                        catch
                        {
                            SdkManager.Bugly_Report("PauseUI", "Home Button roomid get error");
                        }
                        SdkManager.send_event_game_end(0, BattleSource.eWorld, BattleEndType.EPAUSE, coins, equipment, num3, currentRoomID, 0, 0, 0, 0);
                        LocalSave.Instance.BattleIn_DeInit();
                        if (<>f__am$cache3 == null)
                        {
                            <>f__am$cache3 = () => WindowUI.ShowWindow(WindowID.WindowID_Main);
                        }
                        WindowUI.ShowLoading(<>f__am$cache3, null, null, BattleLoadProxy.LoadingType.eMiss);
                    };
                }
                WindowUI.ShowPopWindowUI(languageByTID, content, <>f__am$cache2);
            };
        }
        this.Button_Home.onClick = <>f__am$cache1;
    }

    public override void OnLanguageChange()
    {
        this.mLineCtrl.SetText(GameLogic.Hold.Language.GetLanguageByTID("暂停_技能学习", Array.Empty<object>()));
        this.Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("暂停_标题", Array.Empty<object>());
    }

    protected override void OnOpen()
    {
        GameLogic.SetPause(true);
        this.InitUI();
    }

    private void UpdateSound()
    {
        bool sound = GameLogic.Hold.Sound.GetSound();
        this.SoundIcon.set_sprite(SpriteManager.GetUICommon(!sound ? "Setting_Off1" : "Setting_On1"));
    }

    [CompilerGenerated]
    private sealed class <InitSkills>c__AnonStorey0
    {
        internal int index;
        internal PauseUICtrl.<InitSkills>c__AnonStorey1 <>f__ref$1;

        internal void <>m__0()
        {
            if (this.index < this.<>f__ref$1.skills.Count)
            {
                PauseUISkillIconCtrl ctrl = this.<>f__ref$1.$this.mPool.DeQueue<PauseUISkillIconCtrl>();
                ctrl.Init(this.<>f__ref$1.skills[this.index]);
                RectTransform transform = ctrl.transform as RectTransform;
                ((Transform) transform).SetParentNormal((Transform) this.<>f__ref$1.$this.mScrollRect.get_content());
                int num = this.index % 5;
                int num2 = this.index / 5;
                transform.name = this.index.ToString();
                transform.anchoredPosition = new Vector2((float) (this.<>f__ref$1.startx + (num * this.<>f__ref$1.width)), this.<>f__ref$1.starty - (num2 * this.<>f__ref$1.height));
            }
        }
    }

    [CompilerGenerated]
    private sealed class <InitSkills>c__AnonStorey1
    {
        internal List<int> skills;
        internal int startx;
        internal int width;
        internal float starty;
        internal int height;
        internal PauseUICtrl $this;
    }
}

