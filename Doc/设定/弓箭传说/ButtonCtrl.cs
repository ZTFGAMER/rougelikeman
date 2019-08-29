using Dxx.Net;
using Dxx.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonCtrl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
{
    private static Material _gray;
    private string ButtonCtrl_DownString = "ButtonCtrl_Scale_Down";
    private string ButtonCtrl_UpString = "ButtonCtrl_Scale_Up";
    private string ButtonCtrl_DisableString = "ButtonCtrl_Scale_Disable";
    [SerializeField]
    private ButtonType mType;
    public Action onClick;
    public Action onDown;
    public Action onDisable;
    private bool bDown;
    private bool bEnter;
    private long scrollCount;
    private Animator ani;
    [SerializeField]
    protected SoundButtonType Button_ClickSound = SoundButtonType.eButton_Small;
    protected bool bEnable = true;
    private Image[] mImages;
    private Text[] mTexts;
    private Color[] mTextsColor;
    private bool bDepondNet;
    private string disable_tips = string.Empty;

    private void AddClip()
    {
        if (this.mType != ButtonType.ButtonCtrl_Static)
        {
            Animator component = base.GetComponent<Animator>();
            if (component != null)
            {
                this.ani = component;
                this.ani.runtimeAnimatorController = null;
            }
            else
            {
                this.ani = base.gameObject.AddComponent<Animator>();
            }
            AnimatorOverrideController controller = new AnimatorOverrideController {
                runtimeAnimatorController = ResourceManager.Load<RuntimeAnimatorController>("Game/UI/Button/ButtonPlayCtrl")
            };
            object[] args = new object[] { "Game/UI/Button/", this.mType.ToString(), "_Down" };
            string path = Utils.GetString(args);
            controller[this.ButtonCtrl_DownString] = ResourceManager.Load<AnimationClip>(path);
            this.ButtonCtrl_DownString = path;
            object[] objArray2 = new object[] { "Game/UI/Button/", this.mType.ToString(), "_Up" };
            string str2 = Utils.GetString(objArray2);
            controller[this.ButtonCtrl_UpString] = ResourceManager.Load<AnimationClip>(str2);
            this.ButtonCtrl_UpString = str2;
            controller["ButtonCtrl_Disable"] = ResourceManager.Load<AnimationClip>("Game/UI/Button/ButtonCtrl_Disable");
            object[] objArray3 = new object[] { this.mType.ToString() };
            controller.name = Utils.FormatString("ButtonPlayCtrlRunTime_{0}", objArray3);
            this.ani.runtimeAnimatorController = controller;
            this.ani.updateMode = AnimatorUpdateMode.UnscaledTime;
        }
    }

    private void Awake()
    {
        this.AddClip();
        this.mImages = base.GetComponentsInChildren<Image>(true);
        this.mTexts = base.GetComponentsInChildren<Text>(true);
        this.UpdateTextsColor();
        this.OnAwake();
    }

    private void EnableReset()
    {
        if ((this.ani != null) && this.bEnable)
        {
            this.ani.Play("ButtonCtrl_Up", -1, 1f);
        }
    }

    protected virtual void OnAwake()
    {
    }

    protected virtual void OnDownVirtual()
    {
    }

    private void OnEnable()
    {
        this.EnableReset();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        this.bDown = true;
        if (this.onDown != null)
        {
            this.onDown();
            this.OnDownVirtual();
        }
        this.PlayDown();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        this.bEnter = true;
        if (this.bDown)
        {
            this.PlayDown();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.bEnter = false;
        if (this.bDown)
        {
            this.PlayUp();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (this.bEnter && this.bDown)
        {
            this.PlayUp();
            if (this.bEnable && !eventData.dragging)
            {
                if (((this.Button_ClickSound != SoundButtonType.eNone) && (GameLogic.Hold != null)) && (GameLogic.Hold.Sound != null))
                {
                    GameLogic.Hold.Sound.PlayUI((int) this.Button_ClickSound);
                }
                if (this.onClick != null)
                {
                    base.StartCoroutine(this.startI(this.onClick));
                }
            }
        }
        this.bDown = false;
    }

    protected virtual void OnUpVirtual()
    {
    }

    private void PlayDown()
    {
        if (this.ani != null)
        {
            if (this.bEnable)
            {
                this.ani.Play("ButtonCtrl_Down");
            }
            else
            {
                if (!string.IsNullOrEmpty(this.disable_tips))
                {
                    CInstance<TipsUIManager>.Instance.Show(this.disable_tips);
                }
                this.ani.Play("ButtonCtrl_Disable");
                if (this.onDisable != null)
                {
                    this.onDisable();
                }
            }
        }
    }

    private void PlayUp()
    {
        if ((this.ani != null) && this.bEnable)
        {
            this.ani.Play("ButtonCtrl_Up");
        }
    }

    public void SetDepondNet(bool value)
    {
        this.bDepondNet = value;
    }

    public void SetDisableTips(string tips)
    {
        this.disable_tips = tips;
    }

    public virtual void SetEnable(bool value)
    {
        if (this.bEnable != value)
        {
            this.bEnable = value;
            this.SetGray(value);
        }
    }

    public void SetGray(bool value)
    {
        this.SetImageMaterial(!value ? GrayMaterial : null);
        this.SetTextsColor(value);
    }

    private void SetImageMaterial(Material mat)
    {
        int index = 0;
        int length = this.mImages.Length;
        while (index < length)
        {
            this.mImages[index].set_material(mat);
            index++;
        }
    }

    private void SetTextsColor(bool value)
    {
        int index = 0;
        int length = this.mTextsColor.Length;
        while (index < length)
        {
            this.mTexts[index].set_color(!value ? Color.white : this.mTextsColor[index]);
            index++;
        }
    }

    [DebuggerHidden]
    private IEnumerator startI(Action action) => 
        new <startI>c__Iterator0 { 
            action = action,
            $this = this
        };

    [DebuggerHidden]
    private IEnumerator startI2(Action<ButtonCtrl> action) => 
        new <startI2>c__Iterator1 { 
            action = action,
            $this = this
        };

    private void UpdateTextsColor()
    {
        this.mTextsColor = new Color[this.mTexts.Length];
        int index = 0;
        int length = this.mTexts.Length;
        while (index < length)
        {
            this.mTextsColor[index] = this.mTexts[index].get_color();
            index++;
        }
    }

    public static Material GrayMaterial
    {
        get
        {
            if (_gray == null)
            {
                _gray = ResourceManager.Load<Material>("UIMaterial/GrayMaterial");
            }
            return _gray;
        }
    }

    [CompilerGenerated]
    private sealed class <startI>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal Action action;
        internal ButtonCtrl $this;
        internal object $current;
        internal bool $disposing;
        internal int $PC;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$disposing = true;
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    if (this.action != null)
                    {
                        this.$current = null;
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        goto Label_00C2;
                    }
                    goto Label_00C0;

                case 1:
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 2;
                    }
                    goto Label_00C2;

                case 2:
                    if (!this.$this.bDepondNet)
                    {
                        this.action();
                        break;
                    }
                    if (!NetManager.IsNetConnect)
                    {
                        CInstance<TipsUIManager>.Instance.Show(ETips.Tips_NetError, Array.Empty<string>());
                        break;
                    }
                    this.action();
                    break;

                default:
                    goto Label_00C0;
            }
            this.$PC = -1;
        Label_00C0:
            return false;
        Label_00C2:
            return true;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current =>
            this.$current;

        object IEnumerator.Current =>
            this.$current;
    }

    [CompilerGenerated]
    private sealed class <startI2>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal Action<ButtonCtrl> action;
        internal ButtonCtrl $this;
        internal object $current;
        internal bool $disposing;
        internal int $PC;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$disposing = true;
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    if (this.action != null)
                    {
                        this.$current = null;
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        goto Label_0085;
                    }
                    break;

                case 1:
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 2;
                    }
                    goto Label_0085;

                case 2:
                    this.action(this.$this);
                    this.$PC = -1;
                    break;
            }
            return false;
        Label_0085:
            return true;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current =>
            this.$current;

        object IEnumerator.Current =>
            this.$current;
    }

    public enum ButtonType
    {
        ButtonCtrl_Scale = 0,
        ButtonCtrl_Down20 = 11,
        ButtonCtrl_Down15 = 12,
        ButtonCtrl_Down10 = 13,
        ButtonCtrl_Static = 100,
        ButtonCtrl_ScaleDown20 = 0x33,
        ButtonCtrl_ScaleDown15 = 0x34,
        ButtonCtrl_ScaleDown10 = 0x35
    }
}

