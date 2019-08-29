using Dxx.Util;
using System;
using System.Collections.Generic;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class AchieveOneCtrl : MonoBehaviour
{
    public GameObject unlockobj;
    public GameObject lockobj;
    public Text Text_Name;
    public Text Text_Info;
    public Text Text_Get;
    public ButtonCtrl Button_Get;
    public ProgressTextCtrl mProgressCtrl;
    public CanvasGroup mCanvasGroup;
    public Text Text_LockContent;
    public Transform rewardparent;
    public GameObject copyreward;
    public GameObject gotparent;
    public Action<int, AchieveOneCtrl> OnClickButton;
    [NonSerialized]
    public LocalSave.AchieveDataOne mData;
    private LocalUnityObjctPool mPool;
    private int mAchieveID;
    private int mIndex;

    private void Awake()
    {
        this.mPool = LocalUnityObjctPool.Create(base.gameObject);
        this.mPool.CreateCache<GoldTextCtrl>(this.copyreward);
        this.copyreward.SetActive(false);
        this.Button_Get.onClick = delegate {
            if (this.OnClickButton != null)
            {
                this.OnClickButton(this.mIndex, this);
            }
        };
    }

    public void GetReward()
    {
    }

    public void Init(int index, int achieveid)
    {
        this.mCanvasGroup.alpha = 1f;
        this.mAchieveID = achieveid;
        this.mIndex = index;
        this.mData = LocalSave.Instance.Achieve_Get(achieveid);
        this.Refresh();
    }

    private void init_rewards()
    {
        this.mPool.Collect<GoldTextCtrl>();
        if (!this.mData.isgot)
        {
            List<Drop_DropModel.DropData> rewards = this.mData.mData.GetRewards();
            float x = 0f;
            for (int i = rewards.Count - 1; (i >= 0) && (i < rewards.Count); i--)
            {
                GoldTextCtrl ctrl = this.mPool.DeQueue<GoldTextCtrl>();
                Drop_DropModel.DropData data = rewards[i];
                ctrl.SetCurrencyType(data.id);
                ctrl.SetValue(data.count);
                ctrl.gameObject.SetParentNormal(this.rewardparent);
                RectTransform transform = ctrl.transform as RectTransform;
                transform.anchoredPosition = new Vector2(x, 0f);
                x -= ctrl.GetWidth();
            }
        }
    }

    public void Refresh()
    {
        this.Text_Get.text = GameLogic.Hold.Language.GetLanguageByTID("成就_领取", Array.Empty<object>());
        this.gotparent.SetActive(this.mData.isgot);
        this.rewardparent.gameObject.SetActive(!this.mData.isgot);
        object[] args = new object[] { this.mData.mData.Index };
        this.Text_Name.text = Utils.FormatString("成就{0}", args);
        if (this.mData.mData.IsGlobal)
        {
            this.Text_Info.text = this.mData.mCondition.GetConditionString();
        }
        else
        {
            object[] objArray2 = new object[] { this.mData.mData.Index };
            this.Text_Info.text = Utils.FormatString("完成挑战{0}", objArray2);
        }
        bool isfinish = this.mData.isfinish;
        this.Button_Get.gameObject.SetActive(!this.mData.isgot && isfinish);
        this.mProgressCtrl.gameObject.SetActive(!this.mData.isgot && !isfinish);
        if (!isfinish)
        {
            this.mProgressCtrl.max = this.mData.mCondition.GetMax();
            this.mProgressCtrl.current = this.mData.mCondition.GetCurrent();
        }
        this.init_rewards();
    }
}

