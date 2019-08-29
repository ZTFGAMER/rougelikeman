using Dxx.Util;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FoodEquipBase : FoodBase
{
    public Transform effectparent;
    public Transform meshparent;
    public SpriteRenderer sprite;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private LocalSave.EquipOne <equipone>k__BackingField;
    private GameObject effect;
    private EquipNameCtrl mNameCtrl;

    private void CreateName()
    {
        object[] args = new object[] { "Game/UI/EquipName" };
        GameObject obj2 = GameLogic.EffectGet(Utils.GetString(args));
        obj2.transform.SetParent(GameNode.m_HP.transform);
        obj2.transform.localPosition = Vector3.zero;
        obj2.transform.localScale = Vector3.one;
        obj2.transform.localRotation = Quaternion.identity;
        EquipNameCtrl component = obj2.GetComponent<EquipNameCtrl>();
        this.mNameCtrl = component;
        this.mNameCtrl.Init(this);
    }

    private void EffectShow(bool value)
    {
        if (this.effectparent != null)
        {
            this.effectparent.gameObject.SetActive(value);
        }
        if (value && (this.equipone != null))
        {
            if (this.equipone.Overlying)
            {
                this.effect = CInstance<BattleResourceCreator>.Instance.GetFoodEquipEffect_EquipExp(this.effectparent);
            }
            else
            {
                this.effect = CInstance<BattleResourceCreator>.Instance.GetFoodEquipEffect_Equip(this.effectparent);
            }
        }
    }

    private GameObject getparent()
    {
        if (this.meshparent != null)
        {
            return this.meshparent.gameObject;
        }
        return base.gameObject;
    }

    protected override void OnAbsorbStart()
    {
        this.EffectShow(false);
    }

    protected override void OnAwakeInit()
    {
        base.flyTime = 0.4f;
        base.flyDelayTime = 0.7f;
        base.flySpeed = 0.3f;
        this.sprite = base.transform.Find("child/rotate/sprite").GetComponent<SpriteRenderer>();
        base.bFlyRotate = false;
    }

    protected override void OnDeInit()
    {
        if (this.mNameCtrl != null)
        {
            GameLogic.EffectCache(this.mNameCtrl.gameObject);
        }
    }

    protected override void OnDropEnd()
    {
        this.SetNameShow(true);
        GameLogic.Hold.Sound.PlayBattleSpecial(0x4c4b4d, base.transform.position);
    }

    protected override void OnGetGoodsEnd()
    {
        this.OnDeInit();
    }

    protected override void OnInit()
    {
        this.meshparent.DestroyChildren();
        this.effectparent.DestroyChildren();
        this.equipone = base.data as LocalSave.EquipOne;
        if (this.equipone == null)
        {
            SdkManager.Bugly_Report("FoodEquipBase.cs", "[data] is not [LocalSave.EquipOne] type.");
        }
        else
        {
            if (this.equipone.Position == 1)
            {
                object[] args = new object[] { this.equipone.EquipID };
                Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>(Utils.FormatString("Game/WeaponHand/WeaponHand{0}", args))).SetParentNormal(this.getparent());
                this.sprite.gameObject.SetActive(false);
                base.transform.localRotation = Quaternion.Euler(new Vector3(0f, 45f, 0f));
            }
            else
            {
                this.sprite.gameObject.SetActive(true);
                this.sprite.sprite = this.equipone.Icon;
                base.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
            }
            this.CreateName();
            this.SetNameShow(false);
        }
    }

    private void SetNameShow(bool value)
    {
        if (this.mNameCtrl != null)
        {
            this.mNameCtrl.gameObject.SetActive(value);
        }
        this.EffectShow(value);
    }

    public LocalSave.EquipOne equipone { get; private set; }
}

