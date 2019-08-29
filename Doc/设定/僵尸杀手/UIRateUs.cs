using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIRateUs : MonoBehaviour
{
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static UIRateUs <instance>k__BackingField;
    [SerializeField]
    private Button[] starButtons;
    [SerializeField]
    private Button submit;
    private int activeStarsCount;

    public UIRateUs()
    {
        instance = this;
    }

    public void OnCancel()
    {
        DataLoader.gui.popUpsPanel.gameObject.SetActive(false);
        AnalyticsManager.instance.LogEvent("RateCanceled", new Dictionary<string, string>());
    }

    public void OnSubmit()
    {
        if (this.activeStarsCount > 3)
        {
            Application.OpenURL("market://details?id=com.woodensword.zombie");
        }
        DataLoader.gui.popUpsPanel.gameObject.SetActive(false);
        Dictionary<string, string> eventParameters = new Dictionary<string, string> {
            { 
                "Stars",
                this.activeStarsCount.ToString()
            }
        };
        AnalyticsManager.instance.LogEvent("Rate", eventParameters);
    }

    public void Show()
    {
        DataLoader.gui.popUpsPanel.gameObject.SetActive(true);
        base.gameObject.SetActive(true);
    }

    private void Start()
    {
        this.submit.interactable = false;
        for (int i = 0; i < this.starButtons.Length; i++)
        {
            <Start>c__AnonStorey0 storey = new <Start>c__AnonStorey0 {
                $this = this,
                index = i
            };
            this.starButtons[i].onClick.AddListener(new UnityAction(storey.<>m__0));
        }
    }

    public static UIRateUs instance
    {
        [CompilerGenerated]
        get => 
            <instance>k__BackingField;
        [CompilerGenerated]
        private set => 
            (<instance>k__BackingField = value);
    }

    [CompilerGenerated]
    private sealed class <Start>c__AnonStorey0
    {
        internal int index;
        internal UIRateUs $this;

        internal void <>m__0()
        {
            for (int i = 0; i < this.$this.starButtons.Length; i++)
            {
                this.$this.starButtons[i].image.set_sprite((i > this.index) ? DataLoader.gui.multiplyImages.rateUsStar.inactive : DataLoader.gui.multiplyImages.rateUsStar.active);
            }
            this.$this.submit.interactable = true;
            this.$this.submit.image.set_sprite(DataLoader.gui.multiplyImages.upgrageButtons[4].active);
            this.$this.activeStarsCount = this.index + 1;
        }
    }
}

