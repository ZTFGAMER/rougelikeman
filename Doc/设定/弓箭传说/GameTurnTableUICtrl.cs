using DG.Tweening;
using Dxx.Util;
using PureMVC.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class GameTurnTableUICtrl : MediatorCtrlBase
{
    public GameObject gameturnparent;
    public ButtonCtrl Button_Start;
    public GameTurnTableCtrl mTurnCtrl;
    public Text Text_Title;
    public Text Text_Start;
    public AdTurnTableCtrl mAdTurnCtrl;
    private TurnTableType resultType;
    private SequencePool mSeqPool = new SequencePool();
    private float adx;
    private bool show_currency;
    [CompilerGenerated]
    private static Action <>f__am$cache0;

    private bool ContainsArrow(string[] attrs)
    {
        int index = 0;
        int length = attrs.Length;
        while (index < length)
        {
            string goodType = Goods_goods.GetGoodData(attrs[index]).goodType;
            if ((goodType != null) && ((((goodType == "BulletForward") || (goodType == "BulletBackward")) || ((goodType == "BulletContinue") || (goodType == "BulletForSide"))) || (goodType == "BulletSide")))
            {
                return true;
            }
            index++;
        }
        return false;
    }

    private bool GetRerandom(int skillid)
    {
        if (LocalModelManager.Instance.Skill_slotin.IsWeaponSkillID(skillid))
        {
            return true;
        }
        Skill_skill beanById = LocalModelManager.Instance.Skill_skill.GetBeanById(skillid);
        return ((beanById == null) || this.ContainsArrow(beanById.Attributes));
    }

    private void InitUI()
    {
        List<TurnTableData> list = new List<TurnTableData>();
        TurnTableData item = new TurnTableData {
            type = TurnTableType.ExpBig
        };
        int exp = LocalModelManager.Instance.Exp_exp.GetBeanById(GameLogic.Self.m_EntityData.GetLevel()).Exp;
        int num2 = (int) GameLogic.Random((float) (exp * 0.4f), (float) (exp * 0.6f));
        item.value = num2;
        list.Add(item);
        for (int i = 0; i < 2; i++)
        {
            TurnTableData data2 = new TurnTableData {
                type = TurnTableType.ExpSmall
            };
            int num4 = LocalModelManager.Instance.Exp_exp.GetBeanById(GameLogic.Self.m_EntityData.GetLevel()).Exp;
            int num5 = (int) GameLogic.Random((float) (num4 * 0.1f), (float) (num4 * 0.3f));
            data2.value = num5;
            list.Add(data2);
        }
        TurnTableData data3 = new TurnTableData {
            type = TurnTableType.HPAdd
        };
        list.Add(data3);
        data3.value = 0x10c8e1;
        TurnTableData data4 = new TurnTableData {
            type = TurnTableType.PlayerSkill
        };
        int randomSkill = GameLogic.Self.GetRandomSkill();
        for (int j = 0; j < 100; j++)
        {
            if (!this.GetRerandom(randomSkill))
            {
                break;
            }
            randomSkill = GameLogic.Self.GetRandomSkill();
        }
        data4.value = randomSkill;
        list.Add(data4);
        TurnTableData data5 = new TurnTableData {
            type = TurnTableType.EventSkill,
            value = GameLogic.Release.Form.GetRandomID("GameTurntable")
        };
        list.Add(data5);
        this.mTurnCtrl.InitGood(list);
    }

    protected override void OnClose()
    {
        if (this.show_currency)
        {
            this.show_currency = false;
            WindowUI.CloseCurrency();
        }
        this.mAdTurnCtrl.Deinit();
        this.mSeqPool.Clear();
        GameLogic.SetPause(false);
        RoomGenerateBase.EventCloseTransfer data = new RoomGenerateBase.EventCloseTransfer {
            windowid = WindowID.WindowID_GameTurnTable,
            data = this.resultType
        };
        GameLogic.Release.Mode.RoomGenerate.EventClose(data);
    }

    public override object OnGetEvent(string eventName) => 
        null;

    public override void OnHandleNotification(INotification notification)
    {
    }

    protected override void OnInit()
    {
        this.adx = this.mAdTurnCtrl.transform.localPosition.x;
        this.mTurnCtrl.TurnEnd = delegate (TurnTableData data) {
            this.resultType = data.type;
            if (LocalSave.Instance.BattleAd_CanShow())
            {
                SdkManager.send_event_ad(ADSource.eTurntable, "SHOW", 0, 0, string.Empty, string.Empty);
                TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(TweenSettingsExtensions.Append(TweenSettingsExtensions.AppendInterval(TweenSettingsExtensions.SetUpdate<Sequence>(this.mSeqPool.Get(), true), 0.5f), TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMoveX(this.gameturnparent.transform, -this.adx, 0.5f, false), 0x1b)), 0.5f), new TweenCallback(this, this.<OnInit>m__3));
            }
            else
            {
                WindowUI.CloseWindow(WindowID.WindowID_GameTurnTable);
            }
        };
        RectTransform transform = base.transform as RectTransform;
        RectTransform transform2 = this.gameturnparent.transform as RectTransform;
        transform2.sizeDelta = transform.sizeDelta;
        RectTransform transform3 = this.mAdTurnCtrl.transform as RectTransform;
        transform3.sizeDelta = transform.sizeDelta;
        this.mAdTurnCtrl.Init();
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = () => WindowUI.CloseWindow(WindowID.WindowID_GameTurnTable);
        }
        this.mAdTurnCtrl.onClickClose = <>f__am$cache0;
    }

    public override void OnLanguageChange()
    {
        this.Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("GameTurn_Title", Array.Empty<object>());
        this.Text_Start.text = GameLogic.Hold.Language.GetLanguageByTID("开始", Array.Empty<object>());
        this.mAdTurnCtrl.OnLanguageChange();
    }

    protected override void OnOpen()
    {
        this.show_currency = false;
        this.gameturnparent.transform.localPosition = Vector3.zero;
        GameLogic.Hold.Sound.PlayUI(0xf4244);
        GameLogic.SetPause(true);
        this.Button_Start.onClick = delegate {
            this.Button_Start.gameObject.SetActive(false);
            this.mTurnCtrl.Init();
        };
        this.Button_Start.gameObject.SetActive(true);
        this.InitUI();
        this.mAdTurnCtrl.Open();
    }
}

