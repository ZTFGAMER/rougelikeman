using Dxx.Util;
using System;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class ActiveDiffCtrl : MonoBehaviour
{
    public Text Text_Diff;
    public Text Text_Attack;
    public Text Text_Unlock;
    public GoldTextCtrl mKeyCtrl;
    public GameObject unlockobj;
    public GameObject lockobj;
    public ButtonCtrl mButton;
    private int mIndex;
    private Stage_Level_activity mData;

    private void Awake()
    {
        this.mButton.onClick = new Action(this.OnClickButton);
    }

    public void Init(int index, Stage_Level_activity data)
    {
        this.mIndex = index;
        this.mData = data;
        bool unlock = data.Unlock;
        this.unlockobj.SetActive(unlock);
        this.lockobj.SetActive(!unlock);
        object[] args = new object[] { data.Difficult };
        this.Text_Diff.text = Utils.FormatString("难度:{0}", args);
        object[] objArray2 = new object[] { data.LevelCondition };
        this.Text_Unlock.text = Utils.FormatString("开启等级:{0}", objArray2);
    }

    private void OnClickButton()
    {
        WindowUI.CloseWindow(WindowID.WindowID_Active);
        GameLogic.Hold.BattleData.ActiveID = this.mData.ID;
        Debugger.Log("ActiveID = " + GameLogic.Hold.BattleData.ActiveID);
        GameLogic.Hold.BattleData.SetMode(this.mData.GetMode(), BattleSource.eActivity);
        GameLogic.Hold.Sound.PlayUI(0xf4243);
        WindowUI.ShowWindow(WindowID.WindowID_Battle);
    }
}

