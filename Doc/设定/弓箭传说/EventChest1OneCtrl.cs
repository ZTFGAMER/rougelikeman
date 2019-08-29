using DG.Tweening;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class EventChest1OneCtrl : MonoBehaviour
{
    public Transform child;
    public Image Image_Icon;
    public Text Text_Value;
    private PropOneEquip _equipone;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private TurnTableData <mData>k__BackingField;

    public void Init(TurnTableData data)
    {
        this.mData = data;
        this.child.localScale = Vector3.one;
        this.Text_Value.text = string.Empty;
        this.Image_Icon.enabled = true;
        if ((data.type != TurnTableType.Get) && (this._equipone != null))
        {
            this._equipone.gameObject.SetActive(false);
        }
        switch (data.type)
        {
            case TurnTableType.BigEquip:
            case TurnTableType.SmallEquip:
            {
                this.Image_Icon.enabled = false;
                Drop_DropModel.DropData data2 = data.value as Drop_DropModel.DropData;
                this.equipone.gameObject.SetActive(true);
                this.equipone.InitEquip(data2.id, data2.count);
                this.Image_Icon.set_sprite(SpriteManager.GetEquip(LocalModelManager.Instance.Equip_equip.GetBeanById(data2.id).EquipIcon));
                break;
            }
            case TurnTableType.Boss:
                this.Image_Icon.set_sprite(SpriteManager.GetBattle("GameTurn_Monster"));
                break;

            case TurnTableType.Hitted:
                this.Image_Icon.set_sprite(SpriteManager.GetBattle("GameTurn_Hitted"));
                break;

            case TurnTableType.Get:
            {
                Sequence sequence = DOTween.Sequence();
                TweenSettingsExtensions.SetUpdate<Sequence>(sequence, true);
                TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.child, 1.3f, 0.18f));
                TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.child, 1f, 0.18f));
                TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(this, this.<Init>m__0));
                break;
            }
        }
    }

    private PropOneEquip equipone
    {
        get
        {
            if (this._equipone == null)
            {
                GameObject child = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>("UIPanel/CharUI/EquipPropOne"));
                child.SetParentNormal(this.Image_Icon.transform);
                (child.transform as RectTransform).sizeDelta = Vector3.one * 100f;
                this._equipone = child.GetComponent<PropOneEquip>();
            }
            return this._equipone;
        }
    }

    public TurnTableData mData { get; private set; }
}

