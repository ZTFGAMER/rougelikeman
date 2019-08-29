using Dxx.Util;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class ActiveOneCtrl : MonoBehaviour
{
    public Text Text_Name;
    public Image Image_Icon;
    public Text Text_Count;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private Stage_Level_activityModel.ActivityTypeData <activedata>k__BackingField;

    public void Init(Stage_Level_activityModel.ActivityTypeData one)
    {
        this.activedata = one;
        this.Text_Name.text = this.activedata.GetData(0).Notes;
        int activeCount = LocalSave.Instance.GetActiveCount(this.activedata.index);
        object[] args = new object[] { activeCount };
        this.Text_Count.text = Utils.FormatString("剩余次数：{0}", args);
    }

    public Stage_Level_activityModel.ActivityTypeData activedata { get; private set; }
}

