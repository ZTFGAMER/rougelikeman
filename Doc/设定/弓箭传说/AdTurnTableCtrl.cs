using DG.Tweening;
using Dxx.Net;
using Dxx.Util;
using GameProtocol;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class AdTurnTableCtrl : MonoBehaviour, AdsRequestHelper.AdsCallback
{
    public Text Text_Title;
    public Image Image_Ad;
    public ButtonCtrl Button_Cancel;
    public ButtonCtrl Button_Ad;
    public GameTurnTableCtrl mTurnCtrl;
    public Text Text_Turn;
    public Text Text_Last;
    public Action onClickClose;
    private float Text_TurnX;
    private bool bStartTurn;
    private TurnTableType resultType;
    private int[] qualities = new int[] { 1, 1, 1, 3, 3, 4 };
    private bool bAdReward;

    public void Deinit()
    {
        AdsRequestHelper.getRewardedAdapter().RemoveCallback(this);
    }

    public void Init()
    {
        this.Text_TurnX = this.Text_Turn.get_rectTransform().anchoredPosition.x;
        this.mTurnCtrl.TurnEnd = delegate (TurnTableData data) {
            <Init>c__AnonStorey0 storey = new <Init>c__AnonStorey0 {
                data = data,
                $this = this
            };
            this.resultType = storey.data.type;
            LocalSave.Instance.BattleAd_Use();
            CReqItemPacket itemPacket = NetManager.GetItemPacket(null, false);
            itemPacket.m_nPacketType = 0x13;
            NetManager.SendInternal<CReqItemPacket>(itemPacket, SendType.eUDP, new Action<NetResponse>(storey.<>m__0));
            if (storey.data.type == TurnTableType.Diamond)
            {
                TweenSettingsExtensions.SetUpdate<Sequence>(TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(DOTween.Sequence(), 1.8f), new TweenCallback(storey, this.<>m__1)), true);
            }
            else if (this.onClickClose != null)
            {
                this.onClickClose();
            }
        };
    }

    private void InitUI()
    {
        List<TurnTableData> list = new List<TurnTableData>();
        string[] adTurn = LocalModelManager.Instance.Stage_Level_stagechapter.GetBeanByChapter(GameLogic.Hold.BattleData.Level_CurrentStage).AdTurn;
        int index = 0;
        int length = adTurn.Length;
        while ((index < length) && (index < 6))
        {
            TurnTableData item = new TurnTableData();
            char[] separator = new char[] { ',' };
            string[] strArray2 = adTurn[index].Split(separator);
            int result = 0;
            int.TryParse(strArray2[0], out result);
            long num4 = 0L;
            long.TryParse(strArray2[1], out num4);
            if (num4 > 0L)
            {
                if (result == 1)
                {
                    item.type = TurnTableType.Gold;
                }
                else
                {
                    item.type = TurnTableType.Diamond;
                }
                item.value = num4;
            }
            else
            {
                item.type = TurnTableType.Empty;
            }
            item.quality = this.qualities[index];
            list.Add(item);
            index++;
        }
        for (int i = list.Count; i < 6; i++)
        {
            TurnTableData item = new TurnTableData {
                type = TurnTableType.Empty
            };
            list.Add(item);
        }
        this.mTurnCtrl.InitGood(list);
    }

    public void onClick(AdsRequestHelper.AdsDriver sender, string networkName)
    {
        AdsRequestHelper.DebugLog("AdTurnTableCtrl onClick");
    }

    public void onClose(AdsRequestHelper.AdsDriver sender, string networkName)
    {
        AdsRequestHelper.DebugLog("AdTurnTableCtrl onClose");
        TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(DOTween.Sequence(), 0.1f), new TweenCallback(this, this.<onClose>m__3));
    }

    public void onFail(AdsRequestHelper.AdsDriver sender, string msg)
    {
        AdsRequestHelper.DebugLog("AdTurnTableCtrl onFail");
    }

    public void OnLanguageChange()
    {
        this.Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("GameTurn_AdTitle", Array.Empty<object>());
        this.Text_Turn.text = GameLogic.Hold.Language.GetLanguageByTID("event_ad_turntable_turn", Array.Empty<object>());
    }

    public void onLoad(AdsRequestHelper.AdsDriver sender, string networkName)
    {
        AdsRequestHelper.DebugLog("AdTurnTableCtrl onLoad");
    }

    public void onOpen(AdsRequestHelper.AdsDriver sender, string networkName)
    {
        AdsRequestHelper.DebugLog("AdTurnTableCtrl onOpen");
    }

    public void onRequest(AdsRequestHelper.AdsDriver sender, string networkName)
    {
        AdsRequestHelper.DebugLog("AdTurnTableCtrl onRequest");
    }

    public void onReward(AdsRequestHelper.AdsDriver sender, string networkName)
    {
        AdsRequestHelper.DebugLog("AdTurnTableCtrl onReward");
        this.show_button(false);
        this.mTurnCtrl.Init();
    }

    public void Open()
    {
        this.show_close(false);
        AdsRequestHelper.getRewardedAdapter().AddCallback(this);
        if (LocalSave.Instance.IsAdFree())
        {
            this.Image_Ad.enabled = false;
            this.Text_Turn.get_rectTransform().anchoredPosition = new Vector2(0f, this.Text_Turn.get_rectTransform().anchoredPosition.y);
        }
        else
        {
            this.Image_Ad.enabled = true;
            this.Text_Turn.get_rectTransform().anchoredPosition = new Vector2(this.Text_TurnX, this.Text_Turn.get_rectTransform().anchoredPosition.y);
        }
        object[] args = new object[] { GameLogic.Hold.Language.GetLanguageByTID("key_ad_count", Array.Empty<object>()), LocalSave.Instance.BattleAd_Get() };
        this.Text_Last.text = Utils.FormatString("{0}: {1}", args);
        this.bStartTurn = false;
        GameLogic.Hold.Sound.PlayUI(0xf4244);
        this.Button_Cancel.onClick = delegate {
            if (this.onClickClose != null)
            {
                this.onClickClose();
            }
        };
        this.Button_Ad.SetDepondNet(true);
        this.Button_Ad.onClick = delegate {
            this.bAdReward = false;
            SdkManager.send_event_ad(ADSource.eTurntable, "CLICK", 0, 0, string.Empty, string.Empty);
            if (!NetManager.IsNetConnect)
            {
                CInstance<TipsUIManager>.Instance.Show(ETips.Tips_NetError, Array.Empty<string>());
                SdkManager.send_event_ad(ADSource.eTurntable, "IMPRESSION", 0, 0, "FAIL", "NET_ERROR");
            }
            else if (LocalSave.Instance.IsAdFree())
            {
                this.show_button(false);
                this.mTurnCtrl.Init();
            }
            else if (!AdsRequestHelper.getRewardedAdapter().isLoaded())
            {
                SdkManager.send_event_ad(ADSource.eTurntable, "IMPRESSION", 0, 0, "FAIL", "AD_NOT_READY");
                WindowUI.ShowAdInsideUI(AdInsideProxy.EnterSource.eGameTurn, delegate {
                    this.show_button(false);
                    this.mTurnCtrl.Init();
                });
            }
            else
            {
                SdkManager.send_event_ad(ADSource.eTurntable, "IMPRESSION", 0, 0, "SUCCESS", string.Empty);
                AdsRequestHelper.getRewardedAdapter().Show(this);
            }
        };
        this.show_button(true);
        this.InitUI();
    }

    private void show_button(bool value)
    {
        this.Button_Ad.transform.parent.gameObject.SetActive(value);
        this.Button_Cancel.transform.gameObject.SetActive(value);
    }

    public void show_close(bool value)
    {
        this.Button_Cancel.transform.parent.gameObject.SetActive(value);
    }

    [CompilerGenerated]
    private sealed class <Init>c__AnonStorey0
    {
        internal TurnTableData data;
        internal AdTurnTableCtrl $this;

        internal void <>m__0(NetResponse response)
        {
            if (response.IsSuccess)
            {
                long num = 0L;
                long num2 = 0L;
                if (this.data.type == TurnTableType.Gold)
                {
                    num = (long) this.data.value;
                }
                else if (this.data.type == TurnTableType.Diamond)
                {
                    num2 = (long) this.data.value;
                }
                SdkManager.send_event_ad(ADSource.eTurntable, "REWARD", (int) num, (int) num2, string.Empty, string.Empty);
            }
        }

        internal void <>m__1()
        {
            if (this.$this.onClickClose != null)
            {
                this.$this.onClickClose();
            }
        }
    }
}

