using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseSkillUICtrl : MediatorCtrlBase
{
    public GameObject cantclickObj;
    public List<Text> skillnameList;
    public Text Text_Level;
    public Text Text_Content;
    public GameObject levelparent;
    public Animator Ani_bg;
    public Animator Ani_skill;
    public Animator Ani_level;
    public Animator Ani_content;
    public List<ButtonCtrl> skillbutton;
    public List<ChooseSkillButtonCtrl> chooseskillbutton;
    public List<ChooseSkillOneCtrl> chooseones;
    public List<ChooseSkillColumnCtrl> columns;
    private int level;
    private ChooseSkillProxy.Transfer mTransfer;

    protected void AniDisable()
    {
        this.Ani_bg.enabled = false;
        this.Ani_skill.enabled = false;
        this.Ani_level.enabled = false;
        this.Ani_content.enabled = false;
    }

    private List<int> GetSkill9()
    {
        ChooseSkillProxy.ChooseSkillType type = this.mTransfer.type;
        if (type != ChooseSkillProxy.ChooseSkillType.eLevel)
        {
            if (type == ChooseSkillProxy.ChooseSkillType.eFirst)
            {
                return GameLogic.Self.GetFirstSkill9();
            }
            return null;
        }
        return GameLogic.Self.GetSkill9();
    }

    private void InitUI()
    {
        if (GameLogic.Self != null)
        {
            this.Ani_bg.enabled = true;
            this.Ani_skill.enabled = true;
            this.Ani_level.enabled = true;
            this.Ani_content.enabled = true;
            this.level = GameLogic.Self.GetLearnSkillCount() + 2;
            GameLogic.Release.Game.JoyEnable(false);
            List<int> skills = null;
            if (LocalSave.Instance.BattleIn_GetIn())
            {
                skills = LocalSave.Instance.BattleIn_GetLevelUpSkills();
            }
            else
            {
                skills = this.GetSkill9();
                LocalSave.Instance.BattleIn_UpdateLevelUpSkills((int) this.mTransfer.type, skills);
            }
            int count = 10;
            int num2 = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int k = 0; k < 3; k++)
                {
                    this.chooseones[num2].Init(skills[(i * 3) + k], this.skillnameList[i]);
                    num2++;
                }
                this.columns[i].Init(count, this.skillnameList[i]);
                count += 5;
            }
            for (int j = 0; j < 3; j++)
            {
                this.skillnameList[j].text = string.Empty;
            }
            this.cantclickObj.SetActive(true);
        }
    }

    protected override void OnClose()
    {
        if (this.mTransfer != null)
        {
            GameLogic.SetPause(false);
            this.AniDisable();
            GameLogic.Release.Game.JoyEnable(true);
            Facade.Instance.SendNotification("BATTLE_CHOOSESKILL_TO_BATTLE_CLOSE");
            if (this.mTransfer.type == ChooseSkillProxy.ChooseSkillType.eLevel)
            {
                if ((GameLogic.Self != null) && (GameLogic.Self.OnLevelUp != null))
                {
                    GameLogic.Self.OnLevelUp(this.level);
                }
                if ((GameLogic.Release.Mode != null) && (GameLogic.Release.Mode.RoomGenerate != null))
                {
                    RoomGenerateBase.EventCloseTransfer data = new RoomGenerateBase.EventCloseTransfer {
                        windowid = WindowID.WindowID_ChooseSkill
                    };
                    GameLogic.Release.Mode.RoomGenerate.EventClose(data);
                }
            }
            this.mTransfer = null;
        }
    }

    public override object OnGetEvent(string eventName) => 
        null;

    public override void OnHandleNotification(INotification notification)
    {
        string name = notification.Name;
        object body = notification.Body;
        if (name != null)
        {
            if (name == "BATTLE_CHOOSESKILL_ACTION_END")
            {
                this.OnSkillActionEnd();
            }
            else if (name == "BATTLE_CHOOSESKILL_SKILL_CHOOSE")
            {
                LocalSave.Instance.BattleIn_UpdateLevelUpSkills(0, null);
                int skillId = (int) body;
                GameLogic.Self.AddSkill(skillId, Array.Empty<object>());
                CInstance<TipsManager>.Instance.ShowSkill(skillId);
                WindowUI.CloseWindow(WindowID.WindowID_ChooseSkill);
            }
        }
    }

    protected override void OnInit()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                ButtonCtrl ctrl = this.skillbutton[i];
                ChooseSkillButtonCtrl ctrl2 = this.chooseskillbutton[i];
                ctrl.onClick = new Action(ctrl2.OnClick);
            }
        }
    }

    public override void OnLanguageChange()
    {
        if (this.mTransfer != null)
        {
            switch (this.mTransfer.type)
            {
                case ChooseSkillProxy.ChooseSkillType.eLevel:
                    if (GameLogic.Self.m_EntityData.IsMaxLevel())
                    {
                        this.Text_Level.text = GameLogic.Hold.Language.GetLanguageByTID("LevelUpSkill_MaxLevel", Array.Empty<object>());
                    }
                    else
                    {
                        object[] args = new object[] { this.level.ToString() };
                        this.Text_Level.text = GameLogic.Hold.Language.GetLanguageByTID("LevelUpSkill_升到了", args);
                    }
                    this.Text_Content.text = GameLogic.Hold.Language.GetLanguageByTID("LevelUpSkill_选择", Array.Empty<object>());
                    break;

                case ChooseSkillProxy.ChooseSkillType.eFirst:
                    this.Text_Level.text = GameLogic.Hold.Language.GetLanguageByTID("LevelUpSkill_初始", Array.Empty<object>());
                    this.Text_Content.text = string.Empty;
                    break;
            }
        }
    }

    protected override void OnOpen()
    {
        IProxy proxy = Facade.Instance.RetrieveProxy("ChooseSkillProxy");
        if (proxy != null)
        {
            this.mTransfer = proxy.Data as ChooseSkillProxy.Transfer;
            GameLogic.SetPause(true);
            this.InitUI();
        }
    }

    private void OnSkillActionEnd()
    {
        this.cantclickObj.SetActive(false);
    }
}

