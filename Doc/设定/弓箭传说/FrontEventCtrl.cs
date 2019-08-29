using System;
using UnityEngine;

public class FrontEventCtrl : MonoBehaviour
{
    private CanvasGroup mCanvasGroup;
    private Animator ani;
    private bool bInit;

    private void Awake()
    {
    }

    private void Init()
    {
        if (!this.bInit)
        {
            this.bInit = true;
            this.mCanvasGroup = base.GetComponent<CanvasGroup>();
            if (this.mCanvasGroup == null)
            {
                this.mCanvasGroup = base.gameObject.AddComponent<CanvasGroup>();
            }
            this.ani = base.GetComponent<Animator>();
            if (this.ani == null)
            {
                this.ani = base.gameObject.AddComponent<Animator>();
                AnimatorOverrideController controller = new AnimatorOverrideController {
                    runtimeAnimatorController = ResourceManager.Load<RuntimeAnimatorController>("UIPanel/ACommon/FrontEventCtrl"),
                    name = "FrontEventCtrlRunTime"
                };
                this.ani.runtimeAnimatorController = controller;
                this.ani.updateMode = AnimatorUpdateMode.UnscaledTime;
            }
        }
    }

    public void Play(bool value)
    {
        this.Init();
        this.ani.Play(!value ? "FrontEvent_Hide" : "FrontEvent_Show");
    }
}

