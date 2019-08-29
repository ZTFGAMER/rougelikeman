using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class UIResourceCreator : CInstance<UIResourceCreator>
{
    public T Get<T>(string path) where T: Component => 
        this.get_gameobject(path).GetComponent<T>();

    private GameObject get_gameobject(string path) => 
        Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>(path));

    public BlackItemOnectrl GetBlackShopOne(Transform parent = null)
    {
        BlackItemOnectrl onectrl = this.Get<BlackItemOnectrl>("UIPanel/CharUI/BlackItemOne");
        if (parent != null)
        {
            onectrl.transform.SetParentNormal(parent);
        }
        return onectrl;
    }

    public EquipOneCtrl GetEquip(Transform parent = null)
    {
        EquipOneCtrl ctrl = this.Get<EquipOneCtrl>("UIPanel/CharUI/EquipOne");
        if (parent != null)
        {
            ctrl.transform.SetParentNormal(parent);
        }
        return ctrl;
    }

    public GameObject GetEquipOne_UP(Transform parent = null)
    {
        GameObject child = this.get_gameobject("UIPanel/CharUI/Equip_Up");
        if (parent != null)
        {
            child.SetParentNormal(parent);
        }
        return child;
    }

    public EquipWearCtrl GetEquipWear(Transform parent = null)
    {
        EquipWearCtrl ctrl = this.Get<EquipWearCtrl>("UIPanel/CharUI/equipwear");
        if (parent != null)
        {
            ctrl.transform.SetParentNormal(parent);
        }
        return ctrl;
    }

    public GuideNoMaskCtrl GetGuideNoMask(Transform parent = null)
    {
        GuideNoMaskCtrl ctrl = this.Get<GuideNoMaskCtrl>("UIPanel/GuideUI/guide_nomask");
        if (parent != null)
        {
            ctrl.transform.SetParentNormal(parent);
        }
        return ctrl;
    }

    public PropOneEquip GetPropOneEquip(Transform parent = null)
    {
        PropOneEquip equip = this.Get<PropOneEquip>("UIPanel/CharUI/EquipPropOne");
        if (parent != null)
        {
            equip.transform.SetParentNormal(parent);
        }
        return equip;
    }

    public UILineCtrlOne GetUILineOne(Transform parent = null)
    {
        UILineCtrlOne one = this.Get<UILineCtrlOne>("UIPanel/ACommon/UILineOne");
        if (parent != null)
        {
            one.transform.SetParentNormal(parent);
        }
        return one;
    }
}

