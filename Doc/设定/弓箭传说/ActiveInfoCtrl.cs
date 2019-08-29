using System;
using TableTool;
using UnityEngine;

public class ActiveInfoCtrl : MonoBehaviour
{
    public Transform diffparent;
    public GameObject copyDiff;
    private LocalUnityObjctPool mObjPool;

    private void Awake()
    {
        this.mObjPool = LocalUnityObjctPool.Create(base.gameObject);
        this.mObjPool.CreateCache<ActiveDiffCtrl>(this.copyDiff);
    }

    public void Init(Stage_Level_activityModel.ActivityTypeData one)
    {
        this.mObjPool.Collect<ActiveDiffCtrl>();
        int count = one.mIds.Count;
        for (int i = 0; i < count; i++)
        {
            ActiveDiffCtrl ctrl = this.mObjPool.DeQueue<ActiveDiffCtrl>();
            ctrl.Init(i, one.GetData(i));
            RectTransform child = ctrl.transform as RectTransform;
            child.SetParentNormal(this.diffparent);
            child.anchoredPosition = new Vector2(-240f + (170f * i), 0f);
        }
    }
}

