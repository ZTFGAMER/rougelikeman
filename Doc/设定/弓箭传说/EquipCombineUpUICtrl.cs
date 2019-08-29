using DG.Tweening;
using Dxx.Util;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class EquipCombineUpUICtrl : MediatorCtrlBase
{
    public Text Text_Name;
    public Text Text_Quality;
    public RectTransform iconparent;
    public GameObject successparent;
    public GameObject attributeparent;
    public Text Text_Success;
    public Transform beforeparent;
    public Transform afterparent;
    public GameObject effect_thunder;
    public GameObject effect_rotate;
    public GameObject effect_bomb;
    public Transform attparent;
    public GameObject copyitems;
    public GameObject copyatt;
    public TapToCloseCtrl mCloseCtrl;
    private EquipOneCtrl mEquipBefore;
    private EquipOneCtrl mEquipAfter;
    private LocalUnityObjctPool mPool;
    private EquipCombineUpProxy.Transfer mTransfer;
    private List<EquipCombineAttCtrl> mAttList = new List<EquipCombineAttCtrl>();
    private AnimationCurve curve_move;
    private AnimationCurve curve_sin;
    [CompilerGenerated]
    private static Action <>f__am$cache0;

    private void InitUI()
    {
        <InitUI>c__AnonStorey0 storey = new <InitUI>c__AnonStorey0 {
            $this = this
        };
        this.mCloseCtrl.Show(false);
        this.effect_bomb.SetActive(false);
        this.effect_rotate.SetActive(false);
        this.effect_thunder.SetActive(true);
        this.effect_thunder.transform.localScale = Vector3.one;
        this.mAttList.Clear();
        this.mPool.Collect<EquipOneCtrl>();
        this.mPool.Collect<EquipCombineAttCtrl>();
        this.Text_Name.text = this.mTransfer.equip.NameOnlyString;
        this.Text_Name.set_color(this.mTransfer.equip.qualityColor);
        this.Text_Quality.text = this.mTransfer.equip.QualityString;
        this.Text_Quality.set_color(this.mTransfer.equip.qualityColor);
        Debug.Log(string.Concat(new object[] { " combine up success ", this.mTransfer.equip.EquipID, " quality ", this.mTransfer.equip.Quality, " color ", this.mTransfer.equip.qualityColor }));
        LocalSave.EquipOne equip = new LocalSave.EquipOne {
            EquipID = this.mTransfer.equip.EquipID - 1,
            Level = this.mTransfer.equip.Level
        };
        this.successparent.SetActive(false);
        this.attributeparent.SetActive(false);
        this.mEquipBefore.Init(equip);
        this.mEquipAfter.Init(this.mTransfer.equip);
        int num = 1;
        float y = 0f;
        EquipCombineAttCtrl item = this.mPool.DeQueue<EquipCombineAttCtrl>();
        RectTransform child = item.transform as RectTransform;
        child.SetParentNormal(this.attparent);
        item.UpdateMaxLevel(equip, this.mTransfer.equip);
        child.anchoredPosition = new Vector2(0f, y);
        y -= item.GetHeight();
        this.mAttList.Add(item);
        item.gameObject.SetActive(false);
        num += equip.data.Attributes.Length;
        if (this.mTransfer.equip.data.AdditionSkills.Length > equip.data.AdditionSkills.Length)
        {
            num++;
        }
        for (int i = 0; i < (num - 1); i++)
        {
            EquipCombineAttCtrl ctrl2 = this.mPool.DeQueue<EquipCombineAttCtrl>();
            RectTransform transform = ctrl2.transform as RectTransform;
            transform.SetParentNormal(this.attparent);
            ctrl2.UpdateUI(equip, this.mTransfer.equip, i);
            transform.anchoredPosition = new Vector2(0f, y);
            y -= ctrl2.GetHeight();
            this.mAttList.Add(ctrl2);
            ctrl2.gameObject.SetActive(false);
        }
        this.iconparent.anchoredPosition = new Vector2(0f, -380f);
        this.Text_Name.enabled = false;
        this.Text_Quality.enabled = false;
        storey.left = this.mPool.DeQueue<EquipOneCtrl>();
        storey.right = this.mPool.DeQueue<EquipOneCtrl>();
        storey.three = null;
        storey.left.Init(equip);
        storey.right.Init(equip);
        storey.left.ShowLevel(false);
        storey.right.ShowLevel(false);
        storey.left.transform.SetParentNormal(this.iconparent);
        storey.right.transform.SetParentNormal(this.iconparent);
        this.update_canvas(storey.left.gameObject, true);
        this.update_canvas(storey.right.gameObject, true);
        storey.left.transform.localPosition = new Vector3(-200f, 0f);
        storey.right.transform.localPosition = new Vector3(200f, 0f);
        storey.left.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        storey.right.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        if (equip.data.BreakNeed == 3)
        {
            storey.three = this.mPool.DeQueue<EquipOneCtrl>();
            storey.three.Init(equip);
            storey.three.ShowLevel(false);
            storey.three.transform.SetParentNormal(this.iconparent);
            this.update_canvas(storey.three.gameObject, true);
        }
        TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(TweenSettingsExtensions.AppendCallback(DOTween.Sequence(), new TweenCallback(storey, this.<>m__0)), 1f), new TweenCallback(storey, this.<>m__1));
    }

    protected override void OnClose()
    {
        if (this.mTransfer.onClose != null)
        {
            this.mTransfer.onClose();
        }
        this.effect_rotate.SetParentNormal(this.iconparent);
    }

    public override object OnGetEvent(string eventName) => 
        null;

    public override void OnHandleNotification(INotification notification)
    {
    }

    protected override void OnInit()
    {
        this.curve_move = LocalModelManager.Instance.Curve_curve.GetCurve(0x30d41);
        this.curve_sin = LocalModelManager.Instance.Curve_curve.GetSin();
        this.mPool = LocalUnityObjctPool.Create(base.gameObject);
        this.mPool.CreateCache<EquipCombineAttCtrl>(this.copyatt);
        EquipOneCtrl equip = CInstance<UIResourceCreator>.Instance.GetEquip(null);
        equip.SetButtonEnable(false);
        this.mPool.CreateCache<EquipOneCtrl>(equip.gameObject);
        equip.gameObject.SetActive(false);
        this.copyitems.SetActive(false);
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = () => WindowUI.CloseWindow(WindowID.WindowID_EquipCombineUp);
        }
        this.mCloseCtrl.OnClose = <>f__am$cache0;
        if (this.mEquipBefore == null)
        {
            this.mEquipBefore = CInstance<UIResourceCreator>.Instance.GetEquip(this.beforeparent);
        }
        if (this.mEquipAfter == null)
        {
            this.mEquipAfter = CInstance<UIResourceCreator>.Instance.GetEquip(this.afterparent);
        }
    }

    public override void OnLanguageChange()
    {
        this.Text_Success.text = GameLogic.Hold.Language.GetLanguageByTID("equip_combine_success", Array.Empty<object>());
    }

    protected override void OnOpen()
    {
        IProxy proxy = Facade.Instance.RetrieveProxy("EquipCombineUpProxy");
        if ((proxy == null) || (proxy.Data == null))
        {
            SdkManager.Bugly_Report("EquipCombineUpUICtrl", Utils.FormatString("Proxy is null.", Array.Empty<object>()));
            this.mCloseCtrl.OnClose();
        }
        else
        {
            this.mTransfer = proxy.Data as EquipCombineUpProxy.Transfer;
            this.InitUI();
        }
    }

    private void update_canvas(GameObject o, bool add)
    {
        Canvas component = o.GetComponent<Canvas>();
        if ((component == null) && add)
        {
            component = o.AddComponent<Canvas>();
            component.overrideSorting = true;
            component.sortingLayerName = "UI";
            component.sortingOrder = 0xe9;
        }
        else if (!add && (component != null))
        {
            Object.Destroy(component);
        }
    }

    [CompilerGenerated]
    private sealed class <InitUI>c__AnonStorey0
    {
        internal EquipOneCtrl left;
        internal EquipOneCtrl right;
        internal EquipOneCtrl three;
        internal EquipCombineUpUICtrl $this;

        internal void <>m__0()
        {
            TweenSettingsExtensions.SetLoops<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DORotate(this.left.transform, new Vector3(0f, 0f, 5f), 0.1f, 0), this.$this.curve_sin), 10);
            TweenSettingsExtensions.SetLoops<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DORotate(this.right.transform, new Vector3(0f, 0f, -5f), 0.1f, 0), this.$this.curve_sin), 10);
            if (this.three != null)
            {
                TweenSettingsExtensions.SetLoops<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DORotate(this.three.transform, new Vector3(0f, 0f, -5f), 0.1f, 0), this.$this.curve_sin), 10);
            }
        }

        internal void <>m__1()
        {
            TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScaleX(this.$this.effect_thunder.transform, 0f, 0.7f), this.$this.curve_move);
            TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMoveX(this.left.transform, 0f, 0.7f, false), this.$this.curve_move);
            TweenSettingsExtensions.OnComplete<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMoveX(this.right.transform, 0f, 0.7f, false), this.$this.curve_move), new TweenCallback(this, this.<>m__2));
        }

        internal void <>m__2()
        {
            this.left.gameObject.SetActive(false);
            this.right.gameObject.SetActive(false);
            if (this.three != null)
            {
                this.three.gameObject.SetActive(false);
            }
            EquipOneCtrl ctrl = this.$this.mPool.DeQueue<EquipOneCtrl>();
            this.$this.update_canvas(ctrl.gameObject, false);
            ctrl.transform.SetParentNormal(this.$this.iconparent);
            ctrl.transform.SetAsFirstSibling();
            this.$this.effect_rotate.SetActive(true);
            this.$this.effect_bomb.SetActive(true);
            this.$this.effect_rotate.SetParentNormal(ctrl.transform);
            this.$this.effect_rotate.transform.SetAsFirstSibling();
            ctrl.Init(this.$this.mTransfer.equip);
            TweenSettingsExtensions.Append(TweenSettingsExtensions.AppendInterval(DOTween.Sequence(), 0.7f), TweenSettingsExtensions.OnComplete<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMoveY(this.$this.iconparent, -120f, 0.6f, false), 1), new TweenCallback(this, this.<>m__3)));
        }

        internal void <>m__3()
        {
            this.$this.successparent.SetActive(true);
            this.$this.Text_Name.enabled = true;
            this.$this.Text_Quality.enabled = true;
            TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(DOTween.Sequence(), 0.3f), new TweenCallback(this, this.<>m__4));
        }

        internal void <>m__4()
        {
            this.$this.attributeparent.SetActive(true);
            Sequence sequence = DOTween.Sequence();
            int num = 0;
            int count = this.$this.mAttList.Count;
            while (num < count)
            {
                <InitUI>c__AnonStorey1 storey = new <InitUI>c__AnonStorey1 {
                    <>f__ref$0 = this,
                    index = num
                };
                TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(storey, this.<>m__0));
                TweenSettingsExtensions.AppendInterval(sequence, 0.3f);
                num++;
            }
            TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(this, this.<>m__5));
        }

        internal void <>m__5()
        {
            List<Drop_DropModel.DropData> list = new List<Drop_DropModel.DropData>();
            for (int i = 0; i < this.$this.mTransfer.mats.Count; i++)
            {
                LocalSave.EquipOne equipByUniqueID = LocalSave.Instance.GetEquipByUniqueID(this.$this.mTransfer.mats[i]);
                if (equipByUniqueID != null)
                {
                    equipByUniqueID.CombineReturn(list);
                }
                LocalSave.Instance.Equip_Remove(this.$this.mTransfer.mats[i]);
            }
            WindowUI.ShowRewardSimple(list);
            LocalSave.Instance.AddProps(list);
            this.$this.mCloseCtrl.Show(true);
        }

        private sealed class <InitUI>c__AnonStorey1
        {
            internal int index;
            internal EquipCombineUpUICtrl.<InitUI>c__AnonStorey0 <>f__ref$0;

            internal void <>m__0()
            {
                EquipCombineAttCtrl ctrl = this.<>f__ref$0.$this.mAttList[this.index];
                ctrl.gameObject.SetActive(true);
                ctrl.transform.localScale = Vector3.one * 0.3f;
                ShortcutExtensions.DOScale(ctrl.transform, 1f, 0.3f);
            }
        }
    }
}

