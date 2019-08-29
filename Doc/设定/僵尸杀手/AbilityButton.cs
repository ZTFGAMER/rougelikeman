using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    [SerializeField]
    private SaveData.BoostersData.BoosterType abilityType;
    [SerializeField]
    private int cooldownTime = 10;
    [SerializeField]
    private Image filledImage;
    [SerializeField]
    private Text countText;
    private Button button;
    public AudioClip sound;

    private void Awake()
    {
        this.button = base.GetComponent<Button>();
    }

    public void DoIt()
    {
        if (PlayerPrefs.HasKey(StaticConstants.AbilityTutorialCompleted))
        {
            switch (this.abilityType)
            {
                case SaveData.BoostersData.BoosterType.KillAll:
                    GameManager.instance.KillAll();
                    break;

                default:
                    SpawnManager.instance.OneMoreParashute();
                    break;
            }
            DataLoader.Instance.UseBoosters(this.abilityType);
            this.Refresh();
            SoundManager.Instance.PlaySound(this.sound, -1f);
            this.button.interactable = false;
            if ((this.cooldownTime > -1) && (DataLoader.Instance.GetBoostersCount(this.abilityType) > 0))
            {
                base.StartCoroutine(this.FillBar());
            }
        }
    }

    [DebuggerHidden]
    private IEnumerator FillBar() => 
        new <FillBar>c__Iterator0 { $this = this };

    public void Refresh()
    {
        this.countText.text = DataLoader.Instance.GetBoostersCount(this.abilityType).ToString();
        if (DataLoader.Instance.GetBoostersCount(this.abilityType) > 0)
        {
            this.button.interactable = true;
        }
        else
        {
            base.StopCoroutine(this.FillBar());
            this.filledImage.transform.parent.gameObject.SetActive(false);
            this.button.interactable = false;
        }
    }

    public void Reset()
    {
        base.StopCoroutine(this.FillBar());
        this.filledImage.transform.parent.gameObject.SetActive(false);
        this.Refresh();
    }

    [CompilerGenerated]
    private sealed class <FillBar>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal float <cooldown>__0;
        internal AbilityButton $this;
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
                    this.<cooldown>__0 = this.$this.cooldownTime;
                    this.$this.filledImage.fillAmount = 0f;
                    this.$this.filledImage.transform.parent.gameObject.SetActive(true);
                    break;

                case 1:
                    break;

                default:
                    goto Label_011A;
            }
            if (this.<cooldown>__0 > 0f)
            {
                this.<cooldown>__0 -= Time.deltaTime;
                this.$this.filledImage.fillAmount += Time.deltaTime / ((float) this.$this.cooldownTime);
                this.$current = null;
                if (!this.$disposing)
                {
                    this.$PC = 1;
                }
                return true;
            }
            this.$this.filledImage.fillAmount = 1f;
            this.$this.filledImage.transform.parent.gameObject.SetActive(false);
            this.$this.Refresh();
            this.$PC = -1;
        Label_011A:
            return false;
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

