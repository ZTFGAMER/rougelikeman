using Dxx.Util;
using System;
using UnityEngine;
using UnityEngine.UI;

public class EquipInfoNeedCtrl : MonoBehaviour
{
    public Text Text_Title;
    public Transform itemparent;
    public Text Text_Info;
    private LocalSave.EquipOne mMaterialData;
    private EquipOneCtrl mMaterial;

    public void Init(LocalSave.EquipOne one)
    {
        LocalSave.EquipOne propShowByID = LocalSave.Instance.GetPropShowByID(one.data.UpgradeNeed);
        this.mMaterialData = propShowByID;
        this.init_equip();
        int count = 0;
        if (propShowByID != null)
        {
            count = propShowByID.Count;
        }
        int needMatCount = one.NeedMatCount;
        object[] args = new object[] { this.mMaterialData.NameString, count, needMatCount };
        this.Text_Info.text = Utils.FormatString("{0}: {1}/{2}", args);
        if (count >= needMatCount)
        {
            object[] objArray2 = new object[] { this.mMaterialData.NameString, count, needMatCount };
            this.Text_Info.text = Utils.FormatString("{0}: {1}/{2}", objArray2);
        }
        else
        {
            object[] objArray3 = new object[] { this.mMaterialData.NameString, count, needMatCount };
            this.Text_Info.text = Utils.FormatString("{0}: <color=#ff0000ff>{1}/{2}</color>", objArray3);
        }
    }

    private void init_equip()
    {
        if (this.mMaterial == null)
        {
            this.mMaterial = CInstance<UIResourceCreator>.Instance.GetEquip(this.itemparent);
        }
        this.mMaterial.Init(this.mMaterialData);
        this.mMaterial.SetCountShow(false);
        this.mMaterial.ShowAniEnable(false);
    }

    public void OnLanguageChange()
    {
        this.Text_Title.text = "升级材料1";
    }
}

