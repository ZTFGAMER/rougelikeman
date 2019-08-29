using DG.Tweening;
using Dxx.Util;
using System;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class CardLevelUpAtt2Ctrl : MonoBehaviour
{
    [SerializeField]
    private Text Text_Value;
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
        if (this.mData.data.BaseAttributes[0].Contains("Global_HarvestLevel"))
        {
            this.Text_Value.text = typeName;
        }
        else
        {
            string currentAttribute = this.mData.GetCurrentAttribute(index);
            string str3 = (Goods_goods.GetGoodData(this.mData.data.BaseAttributes[index]).value <= 0L) ? "-" : "+";
            object[] args = new object[] { typeName, str3, currentAttribute };
            this.Text_Value.text = Utils.FormatString("{0} {1} {2}", args);
        }
    }
}

