using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle)), RequireComponent(typeof(Animator))]
public class ToggleCtrl : UIBehaviour
{
    private const string ISON_TRIGGER = "IsOn";
    private bool m_UseAnimation = true;
    private Toggle m_Toggle;
    private Animator m_Animator;

    protected override void Awake()
    {
        this.animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        this.toggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnValueChange));
    }

    private void OnValueChange(bool isOn)
    {
        if (this.useAnimation)
        {
            this.animator.SetBool("IsOn", isOn);
        }
    }

    public bool useAnimation
    {
        get => 
            this.m_UseAnimation;
        set
        {
            this.animator.enabled = value;
            this.m_UseAnimation = value;
        }
    }

    public Toggle toggle
    {
        get
        {
            if (this.m_Toggle == null)
            {
                this.m_Toggle = base.GetComponent<Toggle>();
            }
            return this.m_Toggle;
        }
    }

    public Animator animator
    {
        get
        {
            if (this.m_Animator == null)
            {
                this.m_Animator = base.GetComponent<Animator>();
            }
            return this.m_Animator;
        }
    }
}

