using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class MainChapterCtrl : MonoBehaviour
{
    public GameObject copychapter;
    public ScrollRectBase mScrollRect;
    public ButtonCtrl Button_Left;
    public ButtonCtrl Button_Right;
    public GridLayoutGroup mLayoutGroup;
    public Action OnStageUpdate;
    private LocalUnityObjctPool mPool;
    private List<MainUILevelItem> mList = new List<MainUILevelItem>();
    private int currentstage;
    [CompilerGenerated]
    private static Action<Vector2> <>f__am$cache0;

    private void Awake()
    {
        this.mPool = LocalUnityObjctPool.Create(base.gameObject);
        this.mPool.CreateCache<MainUILevelItem>(this.copychapter);
        this.copychapter.SetActive(false);
        this.Button_Left.onClick = delegate {
            if (this.currentstage > 1)
            {
                this.currentstage--;
                this.mScrollRect.SetPage(this.currentstage - 1, true, null);
                this.update_button();
            }
        };
        this.Button_Right.onClick = delegate {
            if (this.currentstage < LocalSave.Instance.mStage.CurrentStage)
            {
                this.currentstage++;
                this.mScrollRect.SetPage(this.currentstage - 1, true, null);
                this.update_button();
            }
        };
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = delegate (Vector2 value) {
            };
        }
        this.mScrollRect.ValueChanged = <>f__am$cache0;
        this.mScrollRect.EndDragItem = delegate (int stage) {
            this.currentstage = stage + 1;
            this.update_button();
        };
    }

    public void Init()
    {
        this.currentstage = GameLogic.Hold.BattleData.Level_CurrentStage;
        this.mPool.Collect<MainUILevelItem>();
        this.mList.Clear();
        this.mScrollRect.SetWhole(this.mLayoutGroup, LocalSave.Instance.mStage.CurrentStage);
        for (int i = 0; i < LocalSave.Instance.mStage.CurrentStage; i++)
        {
            MainUILevelItem item = this.mPool.DeQueue<MainUILevelItem>();
            item.OnButtonClick = new Action(this.OnClickItem);
            item.gameObject.SetParentNormal(this.mScrollRect.get_content());
            RectTransform transform = item.transform as RectTransform;
            transform.anchoredPosition = new Vector2(0f, i * transform.sizeDelta.x);
            item.Init(i + 1);
            this.mList.Add(item);
        }
        this.mScrollRect.SetPage(this.currentstage - 1, false, null);
        this.update_button();
    }

    private void OnClickItem()
    {
        WindowUI.ShowWindow(WindowID.WindowID_StageList);
    }

    private void update_button()
    {
        this.update_current();
        this.Button_Left.gameObject.SetActive(true);
        this.Button_Right.gameObject.SetActive(true);
        if (this.currentstage >= LocalSave.Instance.mStage.CurrentStage)
        {
            this.Button_Right.gameObject.SetActive(false);
        }
        if (this.currentstage <= 1)
        {
            this.Button_Left.gameObject.SetActive(false);
        }
    }

    private void update_current()
    {
        if (((this.currentstage - 1) >= 0) && ((this.currentstage - 1) < this.mList.Count))
        {
            this.mList[this.currentstage - 1].Init(this.currentstage);
            GameLogic.Hold.BattleData.Level_CurrentStage = this.currentstage;
            if (this.OnStageUpdate != null)
            {
                this.OnStageUpdate();
            }
        }
    }
}

