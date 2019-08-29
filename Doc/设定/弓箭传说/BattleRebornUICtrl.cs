using DG.Tweening;
using Dxx.Net;
using Dxx.Util;
using GameProtocol;
using PureMVC.Interfaces;
using System;
using UnityEngine;
using UnityEngine.UI;

public class BattleRebornUICtrl : MediatorCtrlBase
{
    public Text Text_Content;
    public Text Text_Time;
    public Text Text_Count;
    public GoldTextCtrl mDiamondCtrl;
    public Text Text_Free;
    public Text Text_FreeCount;
    public GameObject FreeParent;
    public ButtonCtrl Button_Buy;
    public ButtonCtrl Button_Close;
    public ButtonCtrl Button_Shadow;
    private const int Max_Second = 5;
    private bool bFree;
    private long needdiamond;
    private bool bStart;
    private float starttime;
    private int second;
    private bool bDealed;
    private Sequence seq;

    private void CloseWindow()
    {
        if (!this.bDealed)
        {
            this.bDealed = true;
            this.CloseWindowInternal();
            GameLogic.Self.Reborn_DeadEnd();
            SdkManager.send_event_revival("NO", GameLogic.Hold.BattleData.Level_CurrentStage, GameLogic.Release.Mode.RoomGenerate.GetCurrentRoomID(), 0);
        }
    }

    private void CloseWindowInternal()
    {
        WindowUI.CloseWindow(WindowID.WindowID_BattleReborn);
        this.bStart = false;
    }

    private void DoReborn()
    {
        if (!this.bDealed)
        {
            LocalSave.Instance.BattleIn_AddRebornUI();
            this.bDealed = true;
            this.CloseWindowInternal();
            GameLogic.Self.DoReborn();
            SdkManager.send_event_revival("YES", GameLogic.Hold.BattleData.Level_CurrentStage, GameLogic.Release.Mode.RoomGenerate.GetCurrentRoomID(), (int) this.needdiamond);
        }
    }

    private void InitUI()
    {
        if (this.Button_Shadow != null)
        {
            this.Button_Shadow.gameObject.SetActive(false);
        }
        this.KillSequence();
        this.seq = TweenSettingsExtensions.SetUpdate<Sequence>(TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(DOTween.Sequence(), 0.5f), new TweenCallback(this, this.<InitUI>m__1)), true);
        this.starttime = 5f;
        this.bStart = true;
        this.needdiamond = GameConfig.GetRebornDiamond();
        GameLogic.Hold.Sound.PlayUI(SoundUIType.eRebornSecond);
        string languageByTID = GameLogic.Hold.Language.GetLanguageByTID("rebornui_count", Array.Empty<object>());
        object[] args = new object[] { languageByTID, GameLogic.Hold.BattleData.GetRebornCount().ToString() };
        this.Text_Count.text = Utils.FormatString("{0}:{1}", args);
        this.Text_Time.text = 5.ToString();
        this.second = 5;
        this.mDiamondCtrl.SetCurrencyType(CurrencyType.Diamond);
        this.mDiamondCtrl.UseTextRed();
        this.mDiamondCtrl.SetValue((int) this.needdiamond);
        this.UpdateButton();
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
        WindowUI.CloseCurrency();
        GameLogic.SetPause(false);
    }

    private void OnDiamondShopClose()
    {
        this.bStart = true;
    }

    public override object OnGetEvent(string eventName) => 
        null;

    public override void OnHandleNotification(INotification notification)
    {
        string name = notification.Name;
        object body = notification.Body;
        if ((name != null) && (name == "PUB_UI_UPDATE_CURRENCY"))
        {
            this.mDiamondCtrl.UseTextRed();
        }
    }

    protected override void OnInit()
    {
        this.Button_Close.onClick = new Action(this.CloseWindow);
        this.Button_Shadow.onClick = this.Button_Close.onClick;
        this.Button_Buy.onClick = delegate {
            if ((LocalSave.Instance.GetDiamond() < this.needdiamond) && !this.bFree)
            {
                this.bStart = false;
                this.second = 5;
                this.starttime = 5f;
                this.Update();
                PurchaseManager.Instance.SetOpenSource(ShopOpenSource.EREBORN);
                WindowUI.ShowShopSingle(ShopSingleProxy.SingleType.eDiamond, new Action(this.OnDiamondShopClose));
            }
            else
            {
                this.bStart = false;
                CLifeTransPacket packet = new CLifeTransPacket {
                    m_nTransID = LocalSave.Instance.SaveExtra.GetTransID(),
                    m_nType = 4,
                    m_nMaterial = (ushort) this.needdiamond
                };
                NetManager.SendInternal<CLifeTransPacket>(packet, SendType.eForceOnce, delegate (NetResponse response) {
                    if (response.IsSuccess)
                    {
                        if (!this.bFree)
                        {
                            LocalSave.Instance.Modify_Diamond(-this.needdiamond, true);
                        }
                        if (LocalSave.Instance.GetRebornCount() > 0)
                        {
                            LocalSave.Instance.Modify_RebornCount(-1);
                        }
                        this.DoReborn();
                    }
                    else
                    {
                        this.bStart = true;
                        if (response.error != null)
                        {
                            CInstance<TipsUIManager>.Instance.ShowCode(response.error.m_nStatusCode, 2);
                        }
                    }
                });
            }
        };
    }

    public override void OnLanguageChange()
    {
        this.Text_Free.text = GameLogic.Hold.Language.GetLanguageByTID("BoxChoose_Free", Array.Empty<object>());
        this.Text_Content.text = GameLogic.Hold.Language.GetLanguageByTID("rebornui_title", Array.Empty<object>());
    }

    protected override void OnOpen()
    {
        WindowUI.ShowCurrency(WindowID.WindowID_Currency);
        GameLogic.SetPause(true);
        this.bDealed = false;
        this.InitUI();
    }

    private void Update()
    {
        if (this.bStart)
        {
            this.starttime -= Updater.deltaIgnoreTime;
            if (this.second != MathDxx.CeilToInt(this.starttime))
            {
                this.second = MathDxx.CeilToInt(this.starttime);
                if (this.second >= 0)
                {
                    GameLogic.Hold.Sound.PlayUI(SoundUIType.eRebornSecond);
                }
                this.Text_Time.text = this.second.ToString();
            }
            if (this.second < 0)
            {
                this.bStart = false;
                this.CloseWindow();
            }
        }
    }

    private void UpdateButton()
    {
        int rebornCount = LocalSave.Instance.GetRebornCount();
        this.bFree = rebornCount > 0;
        this.mDiamondCtrl.gameObject.SetActive(!this.bFree);
        this.FreeParent.SetActive(this.bFree);
        object[] args = new object[] { rebornCount };
        this.Text_FreeCount.text = Utils.FormatString("x{0}", args);
    }
}

