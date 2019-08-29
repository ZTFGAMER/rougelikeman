using Dxx.Util;
using System;
using UnityEngine;

public class BattleLevelAchieveCtrl : MonoBehaviour
{
    public GameObject child;
    private BattleConditionUIBase mCondition;
    private bool bShow = true;

    public void Show(bool value)
    {
        if (this.child != null)
        {
            this.child.SetActive(value);
        }
        this.bShow = value;
        if (this.bShow)
        {
            if (this.mCondition != null)
            {
                Object.Destroy(this.mCondition.gameObject);
            }
            LocalSave.AchieveDataOne data = LocalSave.Instance.Achieve_Get(GameLogic.Hold.BattleData.ActiveID);
            if (data == null)
            {
                object[] args = new object[] { GameLogic.Hold.BattleData.ActiveID };
                SdkManager.Bugly_Report("BattleLevelAchieveCtrl", Utils.FormatString("Achieveid[{0}]  dont in achievelist!!!", args));
            }
            else
            {
                object[] args = new object[] { data.mData.CondType };
                GameObject child = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>(Utils.FormatString("UIPanel/BattleUI/condition/condition{0}", args)));
                child.SetParentNormal(this.child);
                this.mCondition = child.GetComponent<BattleConditionUIBase>();
                this.mCondition.Init(data);
            }
        }
    }

    private void Update()
    {
        if (this.bShow && (this.mCondition != null))
        {
            this.mCondition.Refresh();
        }
    }
}

