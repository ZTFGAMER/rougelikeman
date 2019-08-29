using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class SecretContent : MonoBehaviour
{
    public RectTransform rect;
    [Header("Active"), SerializeField]
    private GameObject active;
    [SerializeField]
    private Text activeCoinsText;
    [SerializeField]
    private Text activeLevelText;
    [SerializeField]
    private Image activeHatImage;
    [SerializeField]
    private Text activeDescriptionText;
    [Header("Inactive"), SerializeField]
    private GameObject inactive;
    [SerializeField]
    private Text inactiveCoinsText;
    [Header("Next"), SerializeField]
    private GameObject next;
    [SerializeField]
    private Text nextCoinsText;
    [SerializeField]
    private Text nextLevelText;
    [SerializeField]
    private Image nextHatImage;

    public void SetActive(DailyContentType type)
    {
        this.active.SetActive(type == DailyContentType.Active);
        this.inactive.SetActive(type == DailyContentType.Inactive);
        this.next.SetActive(type == DailyContentType.Next);
    }

    public void SetContent(int level)
    {
        this.activeCoinsText.text = DataLoader.Instance.moneyBoxGold[level].ToString();
        this.inactiveCoinsText.text = this.activeCoinsText.text;
        this.nextCoinsText.text = this.activeCoinsText.text;
        this.activeLevelText.text = LanguageManager.instance.GetLocalizedText(LanguageKeysEnum.Level) + " " + (level + 1);
        this.activeLevelText.set_font(LanguageManager.instance.currentLanguage.font);
        this.nextLevelText.text = this.activeLevelText.text;
        this.activeHatImage.set_sprite(DataLoader.gui.multiplyImages.secretHat[level % 5]);
        this.nextHatImage.set_sprite(this.activeHatImage.get_sprite());
        this.activeDescriptionText.text = MoneyBoxManager.instance.currentHelpText;
        this.activeDescriptionText.set_font(LanguageManager.instance.currentLanguage.font);
    }

    public void SetTimeText()
    {
        if (base.gameObject.activeInHierarchy)
        {
            base.StartCoroutine(this.TimeText());
        }
        else
        {
            this.nextLevelText.text = this.activeLevelText.text;
        }
    }

    [DebuggerHidden]
    private IEnumerator TimeText() => 
        new <TimeText>c__Iterator0 { $this = this };

    [CompilerGenerated]
    private sealed class <TimeText>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal DateTime <d>__0;
        internal TimeSpan <t>__0;
        internal SecretContent $this;
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
                    this.<d>__0 = TimeManager.CurrentDateTime;
                    this.<t>__0 = this.<d>__0.AddDays(1.0).Date.Subtract(this.<d>__0);
                    break;

                case 1:
                    this.<t>__0 = this.<t>__0.Add(TimeSpan.FromSeconds(-1.0));
                    break;

                default:
                    return false;
            }
            this.$this.nextLevelText.text = $"{this.<t>__0.Hours:D2}:{this.<t>__0.Minutes:D2}:{this.<t>__0.Seconds:D2}";
            this.$current = new WaitForSeconds(1f);
            if (!this.$disposing)
            {
                this.$PC = 1;
            }
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
}

