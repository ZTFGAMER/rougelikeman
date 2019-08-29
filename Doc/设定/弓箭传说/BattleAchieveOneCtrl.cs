using Dxx.Util;
using System;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class BattleAchieveOneCtrl : MonoBehaviour
{
    public ButtonCtrl mButton;
    public Text Text_Content;
    public Image Image_BG;
    public Image Image_Finish;
    public Achieve_Achieve mData;
    public Action<int> OnButtonClick;

    private void Awake()
    {
        this.mButton.onClick = delegate {
            if (!LocalSave.Instance.Achieve_IsFinish(this.mData.ID) && (this.OnButtonClick != null))
            {
                this.OnButtonClick(this.mData.ID);
            }
        };
    }

    public void Init(int achieveid)
    {
        this.mData = LocalModelManager.Instance.Achieve_Achieve.GetBeanById(achieveid);
        if (LocalSave.Instance.Achieve_IsFinish(achieveid))
        {
            this.Image_BG.set_sprite(SpriteManager.GetUICommon("ButtonSmall_Green"));
            this.Image_Finish.gameObject.SetActive(true);
        }
        else
        {
            this.Image_BG.set_sprite(SpriteManager.GetUICommon("ButtonSmall_Yellow"));
            this.Image_Finish.gameObject.SetActive(false);
        }
        LocalSave.AchieveDataOne one = LocalSave.Instance.Achieve_Get(achieveid);
        object[] args = new object[] { this.mData.Index, one.mCondition.GetConditionString() };
        this.Text_Content.text = Utils.FormatString("挑战{0}:{1}", args);
    }
}

