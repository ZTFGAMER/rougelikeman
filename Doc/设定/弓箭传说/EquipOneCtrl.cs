using DG.Tweening;
using Dxx.UI;
using Dxx.Util;
using System;
using UnityEngine;
using UnityEngine.UI;

public class EquipOneCtrl : MonoBehaviour
{
    public GameObject[] typeparent = new GameObject[2];
    public Text Text_Count;
    public ButtonCtrl mButton;
    public Text mButtonText;
    public Image Image_BG;
    public Image Image_Icon;
    public Image Image_Type;
    public GameObject levelparent;
    public Text Text_ID;
    public Text Text_Level;
    public CanvasGroup mCanvasGroup;
    public RedNodeCtrl mRedCtrl;
    public Transform child;
    public GameObject wearparent;
    public Transform upparent;
    public Action<EquipOneCtrl> OnClickEvent;
    private GrayColor[] mGrays;
    private EquipWearCtrl mWearCtrl;
    private bool bGray;
    private int bgquality = -1;
    private int iconid = -1;
    private bool bInit;
    private int mIndex;
    public LocalSave.EquipOne equipdata;
    private Tweener tweener_ani;

    private void Awake()
    {
        this.OnInit();
    }

    public CanvasGroup GetCanvasGroup() => 
        this.mCanvasGroup;

    public void Init()
    {
        this.OnInit();
        this.miss_all_type();
        this.SetBGShow(true);
        this.ShowAniEnable(true);
        if (this.Image_Type != null)
        {
            Sprite typeIcon = this.equipdata.TypeIcon;
            this.Image_Type.enabled = typeIcon != null;
            this.Image_Type.set_sprite(typeIcon);
        }
        EquipType propType = this.equipdata.PropType;
        if (propType == EquipType.eEquip)
        {
            this.ShowLevel(true);
            this.SetCountShow(false);
            this.type_show(0, true);
            object[] args = new object[] { this.equipdata.Level };
            this.Text_Level.text = Utils.FormatString("Lv.{0}", args);
        }
        else if (propType == EquipType.eMaterial)
        {
            this.type_show(1, true);
            this.ShowLevel(false);
            this.SetCountShow(true);
            object[] args = new object[] { this.equipdata.Count };
            this.Text_Count.text = Utils.FormatString("x{0}", args);
        }
        else
        {
            object[] args = new object[] { this.equipdata.EquipID, this.equipdata.PropType };
            SdkManager.Bugly_Report("EquipOneCtrl", Utils.FormatString("Init Equip[{0}]  PropType[{1}] is not achieve! ", args));
        }
        this.SetButtonEnable(true);
        this.SetBGQuality(this.equipdata.Quality);
        this.set_icon(this.equipdata.data.EquipIcon);
        if (this.Text_ID != null)
        {
            object[] args = new object[] { this.equipdata.RowID };
            this.Text_ID.text = Utils.FormatString("ID: {0}", args);
        }
        this.mRedCtrl.DestroyChild();
    }

    public void Init(LocalSave.EquipOne equip)
    {
        this.equipdata = equip;
        this.Init();
    }

    private void miss_all_type()
    {
        int index = 0;
        int length = this.typeparent.Length;
        while (index < length)
        {
            this.typeparent[index].SetActive(false);
            index++;
        }
    }

    private void OnClickButton()
    {
        if (this.OnClickEvent != null)
        {
            this.OnClickEvent(this);
        }
    }

    private void OnInit()
    {
        if (!this.bInit)
        {
            this.bInit = true;
            this.mButton.onClick = new Action(this.OnClickButton);
        }
    }

    private void set_icon(int iconid)
    {
        if (this.iconid != iconid)
        {
            this.iconid = iconid;
            this.Image_Icon.set_sprite(this.equipdata.Icon);
        }
    }

    private void SetBGQuality(int quality)
    {
        if (this.bgquality != quality)
        {
            this.bgquality = quality;
            object[] args = new object[] { quality };
            this.Image_BG.set_sprite(SpriteManager.GetCharUI(Utils.FormatString("CharUI_Quality{0}", args)));
        }
    }

    public void SetBGShow(bool value)
    {
        if (this.Image_BG != null)
        {
            this.Image_BG.gameObject.SetActive(value);
        }
    }

    public void SetButtonEnable(bool value)
    {
        this.mButton.enabled = value;
        if (this.mButtonText != null)
        {
            this.mButtonText.enabled = value;
        }
    }

    public void SetCountShow(bool value)
    {
        this.Text_Count.gameObject.SetActive(value);
    }

    public void SetRedNodeType(RedNodeType type)
    {
        this.mRedCtrl.SetType(RedNodeType.eWarning);
        this.mRedCtrl.Value = 1;
    }

    private void SetUpShow(bool value)
    {
        if (this.upparent != null)
        {
            this.upparent.gameObject.SetActive(value);
        }
        if (value && (this.upparent.childCount == 0))
        {
            CInstance<UIResourceCreator>.Instance.GetEquipOne_UP(this.upparent);
        }
        else if (!value && (this.upparent.childCount > 0))
        {
            this.upparent.DestroyChildren();
        }
    }

    public void ShowAniEnable(bool value)
    {
    }

    public void ShowLevel(bool value)
    {
        this.levelparent.SetActive(value);
    }

    private void type_show(int index, bool value)
    {
        this.typeparent[index].SetActive(value);
    }

    public void UpdateRedShow()
    {
        if (this.equipdata != null)
        {
            if (((this.equipdata.PropType == EquipType.eEquip) && !this.equipdata.IsWear) && (LocalSave.Instance.Equip_GetIsEmpty(this.equipdata) && !LocalSave.Instance.Equip_is_same_wear(this.equipdata)))
            {
                this.mRedCtrl.SetType(RedNodeType.eRedWear);
                this.mRedCtrl.Value = 1;
                LocalSave.Instance.mEquip.SetNew(this.equipdata.UniqueID);
            }
            else if (this.equipdata.bNew)
            {
                this.mRedCtrl.SetType(RedNodeType.eRedNew);
                this.mRedCtrl.Value = 1;
                LocalSave.Instance.mEquip.SetNew(this.equipdata.UniqueID);
            }
            else
            {
                this.mRedCtrl.SetType(RedNodeType.eRedCount);
                this.mRedCtrl.Value = 0;
                this.mRedCtrl.DestroyChild();
            }
        }
    }

    public void UpdateUpShow()
    {
        this.SetUpShow(this.equipdata.IsWear && this.equipdata.CanLevelUp);
    }

    public void UpdateWear()
    {
        this.wearparent.SetActive(false);
        if ((this.equipdata != null) && this.equipdata.IsWear)
        {
            this.wearparent.SetActive(true);
            if (this.mWearCtrl == null)
            {
                this.mWearCtrl = CInstance<UIResourceCreator>.Instance.GetEquipWear(this.wearparent.transform);
            }
        }
    }
}

