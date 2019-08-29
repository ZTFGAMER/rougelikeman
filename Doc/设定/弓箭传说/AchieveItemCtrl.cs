using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TableTool;
using UnityEngine;

public class AchieveItemCtrl : MonoBehaviour
{
    public AchieveInfinity mInfinity;
    private List<int> mList = new List<int>();
    private int mStageID;
    [CompilerGenerated]
    private static Comparison<int> <>f__am$cache0;

    public void Init(int stage)
    {
        this.mStageID = stage;
        this.mList = LocalModelManager.Instance.Achieve_Achieve.GetStageList(stage, true);
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = delegate (int a, int b) {
                bool flag = LocalSave.Instance.Achieve_IsFinish(a);
                bool flag2 = LocalSave.Instance.Achieve_IsFinish(b);
                bool flag3 = LocalSave.Instance.Achieve_Isgot(a);
                bool flag4 = LocalSave.Instance.Achieve_Isgot(b);
                if (flag3 && flag4)
                {
                    return (a >= b) ? 1 : -1;
                }
                if (flag3 || flag4)
                {
                    return !flag3 ? -1 : 1;
                }
                if (flag == flag2)
                {
                    return (a >= b) ? 1 : -1;
                }
                return !flag ? 1 : -1;
            };
        }
        this.mList.Sort(<>f__am$cache0);
        this.mInfinity.Init(this.mList.Count);
        this.mInfinity.updatecallback = new Action<int, AchieveOneCtrl>(this.UpdateChildCallBack);
        this.InitUI();
        this.OnLanguageChange();
    }

    private void InitUI()
    {
        this.mInfinity.SetItemCount(this.mList.Count);
        this.mInfinity.Refresh();
    }

    private void OnLanguageChange()
    {
    }

    private void UpdateChildCallBack(int index, AchieveOneCtrl one)
    {
        one.Init(index, this.mList[index]);
    }
}

