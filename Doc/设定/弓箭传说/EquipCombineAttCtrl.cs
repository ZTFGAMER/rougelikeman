using DG.Tweening;
using Dxx.Util;
using System;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class EquipCombineAttCtrl : MonoBehaviour
{
    [SerializeField]
    private Text Text_Name;
    [SerializeField]
    private Text Text_Before;
    [SerializeField]
    private Text Text_After;
    public GameObject baseatt;
    public GameObject skills;
    public Text Text_Down;
    private int type;
    private LocalSave.EquipOne mAfter;
    private LocalSave.EquipOne mBefore;

    public float GetHeight()
    {
        if (this.type == 0)
        {
            return 100f;
        }
        return 80f;
    }

    public Sequence GetTweener()
    {
        Sequence sequence = DOTween.Sequence();
        TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(base.transform, Vector3.one * 1.3f, 0.2f));
        TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(base.transform, Vector3.one, 0.1f));
        return sequence;
    }

    public void UpdateMaxLevel(LocalSave.EquipOne mBefore, LocalSave.EquipOne mAfter)
    {
        this.type = 0;
        this.baseatt.SetActive(true);
        this.skills.SetActive(false);
        string languageByTID = GameLogic.Hold.Language.GetLanguageByTID("EquipUI_MaxLevel", Array.Empty<object>());
        this.Text_Name.text = languageByTID;
        object[] args = new object[] { mBefore.CurrentMaxLevel };
        this.Text_Before.text = Utils.FormatString("{0}", args);
        object[] objArray2 = new object[] { mAfter.CurrentMaxLevel };
        this.Text_After.text = Utils.FormatString("{0}", objArray2);
    }

    public void UpdateUI(LocalSave.EquipOne mBefore, LocalSave.EquipOne mAfter, int index)
    {
        this.mAfter = mAfter;
        this.mBefore = mBefore;
        this.baseatt.SetActive(false);
        this.skills.SetActive(false);
        if (index < mBefore.data.Attributes.Length)
        {
            this.type = 0;
            this.baseatt.SetActive(true);
            string attName = mAfter.GetAttName(index);
            string currentAttributeString = mBefore.GetCurrentAttributeString(index);
            string str3 = mAfter.GetCurrentAttributeString(index);
            this.Text_Name.text = attName;
            object[] args = new object[] { currentAttributeString };
            this.Text_Before.text = Utils.FormatString("{0}", args);
            object[] objArray2 = new object[] { str3 };
            this.Text_After.text = Utils.FormatString("{0}", objArray2);
        }
        else if (mAfter.data.AdditionSkills.Length > mBefore.data.AdditionSkills.Length)
        {
            this.skills.SetActive(true);
            this.type = 1;
            this.Text_Before.text = string.Empty;
            this.Text_After.text = string.Empty;
            string s = mAfter.data.AdditionSkills[mAfter.data.AdditionSkills.Length - 1];
            this.Text_Name.text = GameLogic.Hold.Language.GetLanguageByTID("equip_combine_unlock_newatt", Array.Empty<object>());
            if (!int.TryParse(s, out int num))
            {
                this.Text_Down.text = Goods_goods.GetGoodShowData(s).ToString();
            }
            else
            {
                object[] args = new object[] { num };
                string languageByTID = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("技能描述{0}", args), Array.Empty<object>());
                this.Text_Down.text = languageByTID;
            }
        }
    }
}

