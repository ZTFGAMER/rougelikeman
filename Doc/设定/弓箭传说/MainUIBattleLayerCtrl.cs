using DG.Tweening;
using Dxx.Util;
using System;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class MainUIBattleLayerCtrl : MonoBehaviour
{
    public const string BoxAniString = "BoxChestRotating";
    public Text Text_Stage;
    public CurrencyExpCtrl mExpCtrl;
    public ButtonCtrl Button_Layer;
    public RedNodeCtrl mRedCtrl;
    public RectTransform BoxTran;
    public Animation BoxAni;
    public Text Text_StageCount;
    public Action OnLayerClick;
    private bool bEnable;
    private int mMax;

    private void Awake()
    {
        this.Button_Layer.SetDepondNet(true);
        this.Button_Layer.onClick = delegate {
            if (this.OnLayerClick != null)
            {
                this.OnLayerClick();
            }
        };
        TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(DOTween.Sequence(), GameLogic.Random((float) 0f, (float) 1f)), new TweenCallback(this, this.<Awake>m__1));
    }

    public void OnLanguageChange()
    {
        this.Text_Stage.text = GameLogic.Hold.Language.GetLanguageByTID("Main_StageChest", Array.Empty<object>());
        this.UpdateStageCount();
    }

    public void SetLayer(int current, int max)
    {
        this.mMax = max;
        this.bEnable = current >= max;
        this.UpdateStageCount();
        int openCount = LocalModelManager.Instance.Box_ChapterBox.GetOpenCount(current, LocalSave.Instance.mStage.BoxLayerID);
        this.mRedCtrl.SetType(RedNodeType.eRedCount);
        this.mRedCtrl.Value = openCount;
        this.BoxAni.enabled = openCount > 0;
        if (this.BoxAni.enabled)
        {
            this.BoxAni.Play("BoxChestRotating");
        }
        else
        {
            this.BoxAni.transform.localRotation = Quaternion.identity;
            this.BoxAni.transform.localScale = Vector3.one;
        }
        this.UpdateNet();
    }

    public void UpdateNet()
    {
    }

    private void UpdateStageCount()
    {
        string stageLayer = GameLogic.Hold.Language.GetStageLayer(this.mMax);
        LocalSave.Instance.mStage.GetLayerBoxStageLayer(this.mMax, out int num, out int num2);
        object[] args = new object[1];
        object[] objArray2 = new object[] { num, num2 };
        args[0] = Utils.FormatString("{0}-{1}", objArray2);
        this.Text_StageCount.text = GameLogic.Hold.Language.GetLanguageByTID("Main_StageCount", args);
        if (this.mMax > 0xf423f)
        {
            this.Text_StageCount.gameObject.SetActive(false);
        }
    }
}

