using Dxx.Util;
using System;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeInfoCtrl : MonoBehaviour
{
    public Text Text_Title;
    public Text Text_SuccessContent;
    public Text Text_Success;
    public Text Text_RewardContent;
    public Text Text_RewardCount;
    public Image Image_RewardIcon;
    public Text Text_ChallengeButton;
    public ButtonCtrl Button_Challenge;
    public ChallengeConditionUICtrl mConditionUICtrl;
    private int m_ID;

    private void Awake()
    {
        this.Button_Challenge.onClick = delegate {
            WindowUI.CloseWindow(WindowID.WindowID_Challenge);
            GameLogic.Hold.BattleData.Challenge_UpdateMode(this.m_ID);
            GameLogic.Hold.Sound.PlayUI(0xf4243);
            WindowUI.ShowWindow(WindowID.WindowID_Battle);
        };
    }

    public void Init(int id)
    {
        this.m_ID = id;
        GameLogic.Hold.BattleData.Challenge_Init(this.m_ID);
        object[] args = new object[] { (this.m_ID - 0x834).ToString() };
        this.Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("Challenge_Title", args);
        this.Text_Success.text = GameLogic.Hold.BattleData.Challenge_GetSuccessString();
        this.mConditionUICtrl.Init();
        int[] reward = GameLogic.Hold.BattleData.ActiveData.Reward;
        if (reward.Length < 3)
        {
            object[] objArray2 = new object[] { GameLogic.Hold.BattleData.ActiveData.ID };
            SdkManager.Bugly_Report("ChallengeUICtrl", Utils.FormatString("ActiveData[{0}] reward.length < 3", objArray2));
        }
        switch (((PropType) reward[0]))
        {
            case PropType.eCurrency:
            {
                FoodOneType type2 = (FoodOneType) reward[1];
                object[] objArray3 = new object[] { reward[2] };
                this.Text_RewardCount.text = Utils.FormatString("{0}", objArray3);
                break;
            }
            case PropType.eEquip:
            {
                int equipIcon = LocalModelManager.Instance.Equip_equip.GetBeanById(reward[1]).EquipIcon;
                object[] objArray4 = new object[] { reward[2] };
                this.Text_RewardCount.text = Utils.FormatString("{0}个", objArray4);
                this.Image_RewardIcon.set_sprite(SpriteManager.GetEquip(equipIcon));
                break;
            }
            case PropType.eCard:
            {
                int groupID = LocalModelManager.Instance.Skill_slotout.GetBeanById(reward[1]).GroupID;
                object[] objArray5 = new object[] { reward[2] };
                this.Text_RewardCount.text = Utils.FormatString("{0}个", objArray5);
                this.Image_RewardIcon.set_sprite(SpriteManager.GetCard(groupID));
                break;
            }
        }
        this.Text_SuccessContent.text = GameLogic.Hold.Language.GetLanguageByTID("Challenge_Success", Array.Empty<object>());
        this.Text_RewardContent.text = GameLogic.Hold.Language.GetLanguageByTID("Challenge_Reward", Array.Empty<object>());
        this.Text_ChallengeButton.text = GameLogic.Hold.Language.GetLanguageByTID("Challenge_Button", Array.Empty<object>());
    }
}

