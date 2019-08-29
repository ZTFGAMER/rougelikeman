using Dxx.Util;
using PureMVC.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SettingDebugMediator : WindowMediator, IMediator, INotifier
{
    private const string DoubleClickString = "DoubleClick";
    private const string AbsorbDelayString = "AbsorbDelay";
    private const string JoyScaleBGString = "JoyScaleBG";
    private const string JoyScaleTouchString = "JoyScaleTouch";
    private const string JoyRadiusString = "JoyRadius";
    public static Action OnValueChange;
    private static Transform ContentParent;
    [CompilerGenerated]
    private static UnityAction <>f__am$cache0;

    public SettingDebugMediator() : base("SettingDebugUIPanel")
    {
    }

    private static string GetValue(string name, string defaultValue = "") => 
        PlayerPrefs.GetString(name, defaultValue);

    public override void OnHandleNotification(INotification notification)
    {
        string name = notification.Name;
        object body = notification.Body;
        if (name == null)
        {
        }
    }

    protected override void OnLanguageChange()
    {
    }

    protected override void OnRegisterEvery()
    {
        this.SetSliderFloat("DoubleClick", 0.05f, 0.5f, 0.3f);
        this.SetSliderInt("AbsorbDelay", 0, 0x3e8, 300);
        this.SetSliderInt("JoyScaleBG", 70, 130, 100);
        this.SetSliderInt("JoyScaleTouch", 40, 130, 100);
        this.SetSliderInt("JoyRadius", 50, 150, 100);
    }

    protected override void OnRegisterOnce()
    {
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = () => WindowUI.CloseWindow(WindowID.WindowID_Setting);
        }
        base._MonoView.transform.Find("Button_Close").GetComponent<Button>().onClick.AddListener(<>f__am$cache0);
        ContentParent = base._MonoView.transform.Find("Scroll View/Viewport/Content");
    }

    protected override void OnRemoveAfter()
    {
    }

    private void SetSliderFloat(string name, float min, float max, float defaultValue)
    {
        <SetSliderFloat>c__AnonStorey1 storey = new <SetSliderFloat>c__AnonStorey1 {
            max = max,
            min = min,
            name = name,
            defaultValue = defaultValue
        };
        storey.slider = ContentParent.Find(storey.name + "/Slider").GetComponent<Slider>();
        storey.text = ContentParent.Find(storey.name + "/Slider/Handle Slide Area/Handle/Text").GetComponent<Text>();
        storey.text.text = GetValue(storey.name, storey.defaultValue.ToString());
        float.TryParse(GetValue(storey.name, storey.defaultValue.ToString()), out float num);
        storey.slider.value = (num - storey.min) / (storey.max - storey.min);
        storey.slider.onValueChanged.AddListener(new UnityAction<float>(storey.<>m__0));
    }

    private void SetSliderInt(string name, int min, int max, int defaultValue)
    {
        <SetSliderInt>c__AnonStorey0 storey = new <SetSliderInt>c__AnonStorey0 {
            name = name,
            max = max,
            min = min,
            defaultValue = defaultValue
        };
        storey.slider = ContentParent.Find(storey.name + "/Slider").GetComponent<Slider>();
        storey.text = ContentParent.Find(storey.name + "/Slider/Handle Slide Area/Handle/Text").GetComponent<Text>();
        storey.text.text = GetValue(storey.name, storey.defaultValue.ToString());
        int.TryParse(GetValue(storey.name, storey.defaultValue.ToString()), out int num);
        storey.slider.value = ((float) (num - storey.min)) / ((float) (storey.max - storey.min));
        storey.slider.onValueChanged.AddListener(new UnityAction<float>(storey.<>m__0));
    }

    private static void SetValue(string name, string value)
    {
        PlayerPrefs.SetString(name, value);
    }

    public static float DoubleClick =>
        float.Parse(GetValue("DoubleClick", "0.3"));

    public static int AbsorbDelay =>
        int.Parse(GetValue("AbsorbDelay", "0"));

    public static int JoyScaleBG =>
        int.Parse(GetValue("JoyScaleBG", "100"));

    public static int JoyScaleTouch =>
        int.Parse(GetValue("JoyScaleTouch", "100"));

    public static int JoyRadius =>
        int.Parse(GetValue("JoyRadius", "100"));

    public override List<string> OnListNotificationInterests =>
        new List<string>();

    [CompilerGenerated]
    private sealed class <SetSliderFloat>c__AnonStorey1
    {
        internal Slider slider;
        internal float max;
        internal float min;
        internal string name;
        internal float defaultValue;
        internal Text text;

        internal void <>m__0(float)
        {
            float f = (this.slider.value * (this.max - this.min)) + this.min;
            SettingDebugMediator.SetValue(this.name, Utils.GetFloat2(f).ToString());
            this.text.text = SettingDebugMediator.GetValue(this.name, this.defaultValue.ToString());
            if (SettingDebugMediator.OnValueChange != null)
            {
                SettingDebugMediator.OnValueChange();
            }
        }
    }

    [CompilerGenerated]
    private sealed class <SetSliderInt>c__AnonStorey0
    {
        internal string name;
        internal Slider slider;
        internal int max;
        internal int min;
        internal int defaultValue;
        internal Text text;

        internal void <>m__0(float)
        {
            SettingDebugMediator.SetValue(this.name, (((int) (this.slider.value * (this.max - this.min))) + this.min).ToString());
            this.text.text = SettingDebugMediator.GetValue(this.name, this.defaultValue.ToString());
            if (SettingDebugMediator.OnValueChange != null)
            {
                SettingDebugMediator.OnValueChange();
            }
        }
    }
}

