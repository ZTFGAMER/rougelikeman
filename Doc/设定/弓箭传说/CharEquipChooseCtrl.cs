using System;
using UnityEngine;

public class CharEquipChooseCtrl : MonoBehaviour
{
    [NonSerialized]
    public LocalSave.EquipOne equipdata;
    private int wearindex;
    private bool bShow;
    private string uniqueid;
    private int mIndex;

    public void ChangeShow()
    {
        this.Show(!this.bShow);
    }

    public int GetIndex() => 
        this.mIndex;

    public bool GetShow() => 
        this.bShow;

    public void Init(LocalSave.EquipOne equip)
    {
        if (string.IsNullOrEmpty(this.uniqueid))
        {
            this.bShow = false;
        }
        else if (!this.uniqueid.Equals(equip.UniqueID) || (this.wearindex != equip.WearIndex))
        {
            this.bShow = false;
        }
        this.uniqueid = equip.UniqueID;
        this.wearindex = equip.WearIndex;
        this.equipdata = equip;
    }

    public void Miss()
    {
    }

    public void OnLanguageChange()
    {
    }

    public void SetIndex(int index)
    {
        this.mIndex = index;
    }

    public void Show(bool show)
    {
    }

    public void UpdateNet()
    {
    }
}

