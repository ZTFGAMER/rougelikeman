using DG.Tweening;
using Dxx.Util;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CardLevelUpAttCtrl : MonoBehaviour
{
    [SerializeField]
    private Text Text_Name;
    [SerializeField]
    private Text Text_Before;
    [SerializeField]
    private Text Text_After;
    [SerializeField]
    private Image Image_Arrow;
    private LocalSave.CardOne mData;

    public Sequence GetTweener()
    {
        Sequence sequence = DOTween.Sequence();
        TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(base.transform, Vector3.one * 1.3f, 0.2f));
        TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(base.transform, Vector3.one, 0.1f));
        return sequence;
    }

    public void UpdateUI(LocalSave.CardOne data, int index)
    {
        this.mData = data;
        string typeName = this.mData.GetTypeName(index);
        this.Text_Name.text = typeName;
        if (this.mData.data.BaseAttributes[0].Contains("Global_HarvestLevel"))
        {
            this.Image_Arrow.gameObject.SetActive(false);
            this.Text_Before.text = string.Empty;
            this.Text_After.text = string.Empty;
        }
        else
        {
            this.Image_Arrow.gameObject.SetActive(true);
            string currentAttribute = this.mData.GetCurrentAttribute(index);
            string nextAttribute = this.mData.GetNextAttribute(index);
            object[] args = new object[] { currentAttribute };
            this.Text_Before.text = Utils.FormatString("+{0}", args);
            object[] objArray2 = new object[] { nextAttribute };
            this.Text_After.text = Utils.FormatString("+{0}", objArray2);
        }
    }
}

