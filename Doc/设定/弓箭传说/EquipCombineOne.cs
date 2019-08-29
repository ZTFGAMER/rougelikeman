using DG.Tweening;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EquipCombineOne : MonoBehaviour
{
    public DOTweenAnimation child_ani;
    public ButtonCtrl mButton;
    public GameObject equiparent;
    public GameObject mLock;
    public GameObject mChoose_First;
    public GameObject mChoose_Second;
    public Action<EquipCombineOne> OnButtonClick;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private EquipCombineChooseOne <mChoose>k__BackingField;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private int <mIndex>k__BackingField;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private LocalSave.EquipOne <mData>k__BackingField;
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

    public void Init(int index, LocalSave.EquipOne one)
    {
        this.mIndex = index;
        this.mData = one;
        if (this.mEquip == null)
        {
            this.mEquip = CInstance<UIResourceCreator>.Instance.GetEquip(this.equiparent.transform);
            this.mEquip.ShowAniEnable(false);
        }
        this.SetButtonEnable(true);
        this.mEquip.Init(one);
        this.mEquip.UpdateWear();
        if (one.CanCombine)
        {
            this.mEquip.SetRedNodeType(RedNodeType.eWarning);
        }
        this.mEquip.SetButtonEnable(false);
        this.SetChoose(null);
        this.SetLock(false);
    }

    public void PlayAni(bool value)
    {
        if (value)
        {
            this.child_ani.DOPlay();
        }
        else
        {
            this.child_ani.DOPause();
            this.child_ani.transform.localPosition = Vector3.zero;
            this.child_ani.transform.localScale = Vector3.one;
        }
    }

    public void SetButtonEnable(bool value)
    {
        this.mButton.enabled = value;
    }

    public void SetChoose(EquipCombineChooseOne one)
    {
        this.mChoose = one;
        if (this.mChoose == null)
        {
            this.mChoose_First.SetActive(false);
            this.mChoose_Second.SetActive(false);
        }
        else
        {
            this.mChoose_First.SetActive(one.mIndex == 0);
            this.mChoose_Second.SetActive(one.mIndex > 0);
            this.PlayAni(false);
        }
    }

    public void SetLock(bool value)
    {
        if (this.mEquip != null)
        {
            this.mLock.SetActive(value);
        }
    }

    public EquipCombineChooseOne mChoose { get; private set; }

    public int mIndex { get; private set; }

    public LocalSave.EquipOne mData { get; private set; }
}

