using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EquipCombineChooseOne : MonoBehaviour
{
    public ButtonCtrl mButton;
    public GameObject child;
    public GameObject mMask;
    public Action<EquipCombineChooseOne> OnButtonClick;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private EquipCombineOne <mEquipChoose>k__BackingField;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private LocalSave.EquipOne <mEquipData>k__BackingField;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private int <mIndex>k__BackingField;
    private bool bMask;
    private EquipOneCtrl mEquip;

    private void Awake()
    {
        this.mButton.onClick = delegate {
            if (this.OnButtonClick != null)
            {
                this.OnButtonClick(this);
            }
        };
    }

    public void Clear()
    {
        if (this.mEquipChoose != null)
        {
            this.mEquipChoose.SetChoose(null);
            this.mEquipChoose = null;
            this.ShowMask(true);
        }
    }

    public void Init(int index, LocalSave.EquipOne one)
    {
        this.mIndex = index;
        this.mEquipData = one;
        if (this.mEquip == null)
        {
            this.mEquip = CInstance<UIResourceCreator>.Instance.GetEquip(this.child.transform);
            this.mEquip.ShowAniEnable(false);
        }
        this.bMask = true;
        this.ShowMask(false);
        this.mEquip.Init(this.mEquipData);
        this.mEquip.SetButtonEnable(false);
    }

    public void Set_Choose_Equip(EquipCombineOne one)
    {
        this.mEquipData = one.mData;
        this.mEquipChoose = one;
        this.mEquip.Init(one.mData);
        this.mEquip.SetButtonEnable(false);
        this.ShowMask(false);
    }

    public void ShowMask(bool show)
    {
        if (this.bMask != show)
        {
            this.bMask = show;
            this.mMask.SetActive(show);
            this.mEquip.ShowLevel(!show);
        }
    }

    public EquipCombineOne mEquipChoose { get; private set; }

    public LocalSave.EquipOne mEquipData { get; private set; }

    public int mIndex { get; private set; }
}

