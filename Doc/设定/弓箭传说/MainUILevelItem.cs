using Dxx.Util;
using System;
using TableTool;
using UnityEngine;

public class MainUILevelItem : MonoBehaviour
{
    public ButtonCtrl Button_Click;
    public GameObject stageparent;
    public Action OnButtonClick;
    private int stageId = 1;
    private GameObject stageitem;
    private Stage_Level_stagechapter beanData;
    private long mCount;

    private void Awake()
    {
        this.Button_Click.onClick = delegate {
            if (this.OnButtonClick != null)
            {
                this.OnButtonClick();
            }
        };
    }

    public void Init(int stageId)
    {
        this.stageId = stageId;
        this.beanData = LocalModelManager.Instance.Stage_Level_stagechapter.GetBeanByChapter(stageId);
        this.InitStage();
    }

    private void InitStage()
    {
        this.stageparent.transform.DestroyChildren();
        if (this.stageitem != null)
        {
            Object.Destroy(this.stageitem);
        }
        try
        {
            object[] args = new object[] { this.stageId };
            string path = Utils.FormatString("UIPanel/MainUI/Stage/Stage{0:D2}", args);
            GameObject original = ResourceManager.Load<GameObject>(path);
            if (original != null)
            {
                this.stageitem = Object.Instantiate<GameObject>(original);
                this.stageitem.SetParentNormal(this.stageparent);
            }
            else
            {
                object[] objArray2 = new object[] { path };
                SdkManager.Bugly_Report("MainUILevelitem", "error1", Utils.FormatString("path:[{0}] ResourceManager.Load is null", objArray2));
            }
        }
        catch
        {
            object[] args = new object[] { this.stageId };
            SdkManager.Bugly_Report("MainUILevelitem", "error2", Utils.FormatString("Create StageItem erro stageId : {0}", args));
        }
    }

    public int StageID =>
        this.StageID;
}

