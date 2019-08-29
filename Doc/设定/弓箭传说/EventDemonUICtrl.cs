using Dxx.Util;
using PureMVC.Interfaces;
using System;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class EventDemonUICtrl : MediatorCtrlBase
{
    public Text texttitle;
    public Text texttitle2;
    public ButtonCtrl buttonok;
    public Text textok;
    public ButtonCtrl buttoncancel;
    public Text textcancel;
    public Text text_content1;
    public Text text_content2;
    public Image image_1;
    public Image image_2;
    private int mLoseid;
    private int mGetid;
    private int mFormid;

    private void InitUI()
    {
        GameLogic.Hold.Sound.PlayUI(0xf4244);
        this.buttonok.onClick = new Action(this.OnClickOK);
        this.buttoncancel.onClick = new Action(this.OnClickCanccel);
        GameLogic.SetPause(true);
        bool flag = GameLogic.Random(0, 100) < 40;
        Room_eventdemontext2lose _eventdemontextlose = null;
        int randomID = GameLogic.Release.Form.GetRandomID("DemonSkill");
        this.mFormid = randomID;
        Room_eventdemontext2skill beanById = LocalModelManager.Instance.Room_eventdemontext2skill.GetBeanById(randomID);
        int index = GameLogic.Random(0, beanById.Loses.Length);
        int num4 = beanById.Loses[index];
        _eventdemontextlose = LocalModelManager.Instance.Room_eventdemontext2lose.GetBeanById(beanById.Loses[index]);
        this.mGetid = beanById.GetID;
        object[] args = new object[] { GameLogic.Hold.Language.GetLanguageByTID("获得技能", Array.Empty<object>()), GameLogic.Hold.Language.GetSkillName(this.mGetid) };
        this.text_content2.text = Utils.FormatString("{0} : {1}", args);
        int skillIcon = LocalModelManager.Instance.Skill_skill.GetBeanById(this.mGetid).SkillIcon;
        this.image_2.set_sprite(SpriteManager.GetSkillIcon(skillIcon));
        this.image_2.GetComponent<RectTransform>().sizeDelta = new Vector2(120f, 120f);
        this.mLoseid = _eventdemontextlose.LoseID;
        Goods_goods.GoodData goodData = Goods_goods.GetGoodData(LocalModelManager.Instance.Goods_food.GetBeanById(this.mLoseid).Values[0]);
        string str = MathDxx.Abs((long) (((float) (GameLogic.Self.m_EntityData.attribute.HPValue.ValueLong * goodData.value)) / 10000f)).ToString();
        object[] objArray2 = new object[] { GameLogic.Hold.Language.GetLanguageByTID("失去", Array.Empty<object>()), str, GameLogic.Hold.Language.GetLanguageByTID(_eventdemontextlose.Content1, Array.Empty<object>()) };
        this.text_content1.text = Utils.FormatString("{0} {1} {2}", objArray2);
    }

    private void OnClickCanccel()
    {
        WindowUI.CloseWindow(WindowID.WindowID_EventDemon);
    }

    private void OnClickOK()
    {
        GameLogic.Self.AddSkill(this.mGetid, Array.Empty<object>());
        GameLogic.Release.Form.RemoveID("DemonSkill", this.mFormid);
        CInstance<TipsManager>.Instance.ShowSkill(this.mGetid);
        GameLogic.Self.GetGoods(this.mLoseid);
        WindowUI.CloseWindow(WindowID.WindowID_EventDemon);
    }

    protected override void OnClose()
    {
        GameLogic.SetPause(false);
        RoomGenerateBase.EventCloseTransfer data = new RoomGenerateBase.EventCloseTransfer {
            windowid = WindowID.WindowID_EventDemon
        };
        GameLogic.Release.Mode.RoomGenerate.EventClose(data);
        if ((GameLogic.Self != null) && (GameLogic.Self.OnMissDemon != null))
        {
            GameLogic.Self.OnMissDemon();
        }
    }

    public override object OnGetEvent(string eventName) => 
        null;

    public override void OnHandleNotification(INotification notification)
    {
    }

    protected override void OnInit()
    {
    }

    public override void OnLanguageChange()
    {
        this.texttitle.text = GameLogic.Hold.Language.GetLanguageByTID("恶魔房标题", Array.Empty<object>());
        this.texttitle2.text = GameLogic.Hold.Language.GetLanguageByTID("恶魔房标题2", Array.Empty<object>());
        this.textok.text = GameLogic.Hold.Language.GetLanguageByTID("签订", Array.Empty<object>());
        this.textcancel.text = GameLogic.Hold.Language.GetLanguageByTID("拒绝", Array.Empty<object>());
    }

    protected override void OnOpen()
    {
        this.InitUI();
    }
}

