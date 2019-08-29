using DG.Tweening;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class EquipBGCtrl : MonoBehaviour
{
    private LocalSave.EquipOne _equipdata;
    private int index;
    private GameObject addparent;
    private Animation addani;
    private Text Text_Add;
    private Image Image_BG;
    private GameObject equipparent;
    private EquipOneCtrl _ctrl;
    private GameObject buttonObj;
    private ButtonCtrl button;
    private Text buttonText;
    private Action<int> mClick;
    private bool bInit;

    private void Awake()
    {
        this.index = int.Parse(base.name.Substring(base.name.Length - 1, 1));
        this.buttonObj = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>("UIPanel/CharUI/EquipBG"));
        this.button = this.buttonObj.GetComponent<ButtonCtrl>();
        this.button.onClick = new Action(this.OnClick);
        this.buttonText = this.button.GetComponent<Text>();
        Transform child = this.buttonObj.transform;
        child.SetParentNormal(base.transform);
        this.addani = child.Find("fg/Image_Add").GetComponent<Animation>();
        this.Image_BG = child.Find("fg/Image_BG").GetComponent<Image>();
        this.equipparent = this.addani.transform.Find("equipparent").gameObject;
        string str = string.Empty;
        switch (this.index)
        {
            case 0:
                str = "EquipUI_BG_Weapon";
                break;

            case 1:
                str = "EquipUI_BG_Cloth";
                break;

            case 2:
            case 3:
                str = "EquipUI_BG_Ornament";
                break;

            case 4:
            case 5:
                str = "EquipUI_BG_Pet";
                break;
        }
        this.Image_BG.set_sprite(SpriteManager.GetCharUI(str));
        this.addparent = this.addani.transform.Find("addparent").gameObject;
        this.Text_Add = this.addparent.transform.Find("Image_Shadow/Text_Add").GetComponent<Text>();
        this.equipdata = null;
        this.UpdateBGShow();
        this.ShowAdd(false);
    }

    public void DoWear()
    {
        this.PlayRotate();
    }

    public bool GetIsWear() => 
        (this.equipdata != null);

    public void Init(LocalSave.EquipOne equipdata)
    {
        this.equipdata = equipdata;
        if (this.equipdata == null)
        {
            this.ctrl.gameObject.SetActive(false);
        }
        else
        {
            this.ctrl.gameObject.SetActive(true);
            this.ctrl.Init(equipdata);
            this.ctrl.SetButtonEnable(false);
        }
        this.UpdateBGShow();
        this.UpdateRedNode();
        RectTransform transform = this.ctrl.transform as RectTransform;
        transform.localScale = Vector3.one * 1.1f;
        transform.anchoredPosition = Vector2.zero;
        this.StopRotate();
    }

    public void MissAdd()
    {
        this.ShowAdd(false);
    }

    private void OnClick()
    {
        if (this.mClick != null)
        {
            this.mClick(this.index);
        }
    }

    private void PlayRotate()
    {
        if (this.addani != null)
        {
            this.ShowAdd(true);
        }
    }

    public void SetButtonEnable(bool value)
    {
        this.button.enabled = value;
        this.buttonText.enabled = value;
    }

    public void SetClick(Action<int> click)
    {
        this.mClick = click;
    }

    private void ShowAdd(bool value)
    {
        this.addparent.SetActive(value);
        if (value)
        {
            if ((this.equipdata != null) && (this.ctrl != null))
            {
                this.Text_Add.text = GameLogic.Hold.Language.GetLanguageByTID("EquipUI_ClickChange", Array.Empty<object>());
            }
            else
            {
                this.Text_Add.text = GameLogic.Hold.Language.GetLanguageByTID("EquipUI_ClickWear", Array.Empty<object>());
            }
        }
    }

    private void Start()
    {
    }

    private void StopRotate()
    {
        if (this.addani != null)
        {
        }
    }

    public void Unwear(Vector3 endpos, Action<LocalSave.EquipOne> onFinish = null)
    {
        <Unwear>c__AnonStorey0 storey = new <Unwear>c__AnonStorey0 {
            onFinish = onFinish,
            $this = this
        };
        if (this.equipdata != null)
        {
            Sequence sequence = DOTween.Sequence();
            TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.OnComplete<Tweener>(ShortcutExtensions.DOMove(this.ctrl.transform, endpos, 0.3f, false), new TweenCallback(storey, this.<>m__0)));
            TweenSettingsExtensions.Join(sequence, TweenSettingsExtensions.Append(TweenSettingsExtensions.AppendInterval(DOTween.Sequence(), 0.1f), ShortcutExtensions46.DOFade(this.ctrl.GetCanvasGroup(), 0f, 0.1f)));
        }
    }

    private void UpdateBGShow()
    {
        this.Image_BG.gameObject.SetActive(this.equipdata == null);
    }

    public void UpdateButtonEnable()
    {
        this.SetButtonEnable(this._equipdata != null);
    }

    public void UpdateRedNode()
    {
        if (((this.equipdata != null) && (this.ctrl != null)) && (this.ctrl.equipdata != null))
        {
            this.ctrl.UpdateRedShow();
            this.ctrl.UpdateUpShow();
        }
    }

    public void WearOver()
    {
        this.StopRotate();
    }

    public LocalSave.EquipOne equipdata
    {
        get => 
            this._equipdata;
        set
        {
            this._equipdata = value;
            this.UpdateButtonEnable();
        }
    }

    public EquipOneCtrl ctrl
    {
        get
        {
            if (this._ctrl == null)
            {
                this._ctrl = CInstance<UIResourceCreator>.Instance.GetEquip(this.equipparent.transform);
                this._ctrl.SetButtonEnable(false);
                this._ctrl.ShowAniEnable(false);
            }
            return this._ctrl;
        }
    }

    [CompilerGenerated]
    private sealed class <Unwear>c__AnonStorey0
    {
        internal Action<LocalSave.EquipOne> onFinish;
        internal EquipBGCtrl $this;

        internal void <>m__0()
        {
            this.$this.ctrl.transform.localPosition = Vector3.zero;
            this.$this.ctrl.GetCanvasGroup().alpha = 1f;
            this.$this.ctrl.gameObject.SetActive(false);
            if (this.onFinish != null)
            {
                this.onFinish(this.$this.equipdata);
            }
            this.$this.equipdata = null;
            this.$this.UpdateBGShow();
        }
    }
}

