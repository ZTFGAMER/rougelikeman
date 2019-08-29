using DG.Tweening;
using DG.Tweening.Core;
using PureMVC.Patterns;
using System;
using System.Runtime.CompilerServices;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class MainUIBattleLevelCtrl : MonoBehaviour
{
    public Text Text_Level;
    public ProgressCtrl mProgress;
    private float progresstime = 1f;

    public float AddExpAnimation(int addexp, Sequence seqparent)
    {
        <AddExpAnimation>c__AnonStorey0 storey = new <AddExpAnimation>c__AnonStorey0 {
            seqparent = seqparent,
            $this = this
        };
        int level = LocalSave.Instance.GetLevel();
        if (level < 1)
        {
            level = 1;
        }
        int maxLevel = LocalModelManager.Instance.Character_Level.GetMaxLevel();
        if (level >= maxLevel)
        {
            return 0f;
        }
        Sequence sequence = null;
        if (storey.seqparent != null)
        {
            sequence = DOTween.Sequence();
            TweenSettingsExtensions.SetUpdate<Sequence>(sequence, true);
        }
        int exp = (int) LocalSave.Instance.GetExp();
        int num4 = LocalModelManager.Instance.Character_Level.GetExp(level);
        float num5 = 0f;
        float num6 = 0f;
        float num7 = 0f;
        float num8 = 0f;
        while ((exp + addexp) >= num4)
        {
            num6 = 1f;
            num5 = ((float) exp) / ((float) num4);
            level++;
            num8 = (num6 - num5) * this.progresstime;
            num7 += num8;
            if (sequence != null)
            {
                <AddExpAnimation>c__AnonStorey1 storey2 = new <AddExpAnimation>c__AnonStorey1 {
                    <>f__ref$0 = storey,
                    levelup = level
                };
                TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetUpdate<TweenerCore<float, float, FloatOptions>>(DOTween.To(new DOGetter<float>(storey2, this.<>m__0), new DOSetter<float>(storey2, this.<>m__1), 1f, num8), true));
                TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(storey2, this.<>m__2));
                TweenSettingsExtensions.AppendInterval(sequence, 0.03f);
                TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(storey2, this.<>m__3));
                TweenSettingsExtensions.AppendInterval(sequence, 0.03f);
            }
            addexp -= num4 - exp;
            exp = 0;
            if (level >= maxLevel)
            {
                break;
            }
            num4 = LocalModelManager.Instance.Character_Level.GetExp(level);
        }
        if (level >= maxLevel)
        {
            this.mProgress.Value = 1f;
        }
        else
        {
            num5 = ((float) exp) / ((float) num4);
            num6 = ((float) (exp + addexp)) / ((float) num4);
            num8 = (num6 - num5) * this.progresstime;
            num7 += num8;
            if (sequence != null)
            {
                TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetUpdate<TweenerCore<float, float, FloatOptions>>(DOTween.To(new DOGetter<float>(storey, this.<>m__0), new DOSetter<float>(storey, this.<>m__1), num6, num8), true));
                TweenSettingsExtensions.Append(storey.seqparent, sequence);
            }
        }
        LocalSave.Instance.SetLevel(level);
        LocalSave.Instance.SetExp(exp + addexp);
        return num7;
    }

    public void SetExp(int current, int max)
    {
        this.mProgress.Value = ((float) current) / ((float) max);
    }

    public void SetLevel(int level)
    {
        this.Text_Level.text = level.ToString();
    }

    public void UpdateLevel()
    {
        this.SetLevel(LocalSave.Instance.GetLevel());
        int exp = (int) LocalSave.Instance.GetExp();
        int max = LocalModelManager.Instance.Character_Level.GetExp(LocalSave.Instance.GetLevel());
        this.SetExp(exp, max);
    }

    [CompilerGenerated]
    private sealed class <AddExpAnimation>c__AnonStorey0
    {
        internal Sequence seqparent;
        internal MainUIBattleLevelCtrl $this;

        internal float <>m__0() => 
            this.$this.mProgress.Value;

        internal void <>m__1(float x)
        {
            this.$this.mProgress.Value = x;
        }
    }

    [CompilerGenerated]
    private sealed class <AddExpAnimation>c__AnonStorey1
    {
        internal int levelup;
        internal MainUIBattleLevelCtrl.<AddExpAnimation>c__AnonStorey0 <>f__ref$0;

        internal float <>m__0() => 
            this.<>f__ref$0.$this.mProgress.Value;

        internal void <>m__1(float x)
        {
            this.<>f__ref$0.$this.mProgress.Value = x;
        }

        internal void <>m__2()
        {
            TweenExtensions.Pause<Sequence>(this.<>f__ref$0.seqparent);
            LevelUpProxy.Transfer data = new LevelUpProxy.Transfer {
                level = this.levelup,
                onclose = () => TweenExtensions.Play<Sequence>(this.<>f__ref$0.seqparent)
            };
            Facade.Instance.RegisterProxy(new LevelUpProxy(data));
            WindowUI.ShowWindow(WindowID.WindowID_LevelUp);
        }

        internal void <>m__3()
        {
            this.<>f__ref$0.$this.SetLevel(this.levelup);
            this.<>f__ref$0.$this.mProgress.Value = 0f;
        }

        internal void <>m__4()
        {
            TweenExtensions.Play<Sequence>(this.<>f__ref$0.seqparent);
        }
    }
}

