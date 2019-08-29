using Dxx.Util;
using PureMVC.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class EventAngelUICtrl : MediatorCtrlBase
{
    public Text texttitle;
    public Text texttitle2;
    public ButtonCtrl buttonok1;
    public ButtonCtrl buttonok2;
    public List<Text> text_content;
    public List<Image> image;
    private const int ChooseCount = 2;
    private GetData mData = new GetData();
    private int mRecoverHPId = 0x10c8e1;

    private void InitSkill()
    {
        int randomID = GameLogic.Release.Form.GetRandomID("AngelSkill");
        Room_eventangelskill beanById = LocalModelManager.Instance.Room_eventangelskill.GetBeanById(randomID);
        this.text_content[0].text = GameLogic.Hold.Language.GetSkillName(beanById.GetID);
        Sprite skillIcon = SpriteManager.GetSkillIcon(LocalModelManager.Instance.Skill_skill.GetBeanById(beanById.GetID).SkillIcon);
        if (skillIcon != null)
        {
            this.image[0].set_sprite(skillIcon);
        }
        this.mData.getid = beanById.GetID;
        this.mData.eventID = beanById.EventID;
        this.mData.formid = randomID;
    }

    private void InitUI()
    {
        GameLogic.Hold.Sound.PlayUI(0xf4244);
        GameLogic.SetPause(true);
        this.InitSkill();
        this.image[1].set_sprite(SpriteManager.GetSkillIcon(this.mRecoverHPId));
        this.text_content[1].text = GameLogic.Hold.Language.GetSkillName(this.mRecoverHPId);
    }

    private void OnClickOK1()
    {
        GameLogic.Self.AddSkill(this.mData.getid, Array.Empty<object>());
        GameLogic.Release.Form.RemoveID("AngelSkill", this.mData.formid);
        CInstance<TipsManager>.Instance.ShowSkill(this.mData.getid);
        WindowUI.CloseWindow(WindowID.WindowID_EventAngel);
    }

    private void OnClickOK2()
    {
        int num = 40;
        if (GameLogic.Random((float) 0f, (float) 1f) < GameLogic.Self.m_EntityData.attribute.AngelR2Rate.Value)
        {
            num *= 2;
        }
        object[] args = new object[] { "HPRecoverFixed%", num };
        GameLogic.Self.m_EntityData.ExcuteAttributes(Utils.FormatString("{0} + {1}", args));
        CInstance<TipsManager>.Instance.ShowSkill(this.mRecoverHPId);
        WindowUI.CloseWindow(WindowID.WindowID_EventAngel);
    }

    protected override void OnClose()
    {
        GameLogic.SetPause(false);
        RoomGenerateBase.EventCloseTransfer data = new RoomGenerateBase.EventCloseTransfer {
            windowid = WindowID.WindowID_EventAngel
        };
        GameLogic.Release.Mode.RoomGenerate.EventClose(data);
        if ((GameLogic.Self != null) && (GameLogic.Self.OnMissAngel != null))
        {
            GameLogic.Self.OnMissAngel();
        }
    }

    public override object OnGetEvent(string eventName) => 
        null;

    public override void OnHandleNotification(INotification notification)
    {
    }

    protected override void OnInit()
    {
        this.buttonok1.onClick = new Action(this.OnClickOK1);
        this.buttonok2.onClick = new Action(this.OnClickOK2);
    }

    public override void OnLanguageChange()
    {
        this.texttitle.text = GameLogic.Hold.Language.GetLanguageByTID("天使房标题", Array.Empty<object>());
        this.texttitle2.text = GameLogic.Hold.Language.GetLanguageByTID("天使房标题2", Array.Empty<object>());
    }

    protected override void OnOpen()
    {
        this.InitUI();
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct GetData
    {
        public int eventID;
        public int getid;
        public int formid;
    }
}

