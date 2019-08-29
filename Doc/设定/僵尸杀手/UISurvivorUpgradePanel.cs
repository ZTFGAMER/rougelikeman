using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UISurvivorUpgradePanel : UIBaseScrollPanel
{
    [HideInInspector]
    public SurviviorContent[] surviviorContent;
    [HideInInspector]
    public SaveData.HeroData.HeroType lastOpenedHeroType;
    [HideInInspector]
    public bool animationCompleted = true;
    public RenderTexture renderTextureSurvivorPrefab;
    public Camera heroCamPrefab;

    public void ActivateHeroCams(bool active)
    {
        for (int i = 0; i < this.surviviorContent.Length; i++)
        {
            this.surviviorContent[i].ActivateCamera(active);
        }
    }

    public void AnimateLastheroOpened()
    {
        base.StartCoroutine(this.DelayedAnimation());
    }

    public override void CreateCells()
    {
        RectTransform[] transformArray = UIController.instance.CreareScrollContent(base.cellPrefab, base.scrollRect, DataLoader.Instance.survivors.Count, 0f, base.distanceBetweenCells, true);
        this.surviviorContent = new SurviviorContent[transformArray.Length];
        DataLoader.Instance.UpdateHeroesIsOpened();
        for (int i = 0; i < transformArray.Length; i++)
        {
            this.surviviorContent[i] = transformArray[i].GetComponent<SurviviorContent>();
            this.surviviorContent[i].SetContent(i);
        }
        base.cellPrefab.gameObject.SetActive(false);
    }

    [DebuggerHidden]
    public IEnumerator DelayedAnimation() => 
        new <DelayedAnimation>c__Iterator1 { $this = this };

    public int GetHeroIndex(SaveData.HeroData.HeroType _type)
    {
        for (int i = 0; i < this.surviviorContent.Length; i++)
        {
            if (this.surviviorContent[i].heroData.heroType == _type)
            {
                return i;
            }
        }
        return 0;
    }

    public bool IsVideoUpgradeAvailable()
    {
        for (int i = 0; i < this.surviviorContent.Length; i++)
        {
            if (this.surviviorContent[i].IsVideoAvailable())
            {
                base.StartCoroutine(this.ScrollToHero(i));
                Dictionary<string, string> eventParameters = new Dictionary<string, string> {
                    { 
                        "Hero",
                        this.surviviorContent[i].heroData.heroType.ToString()
                    }
                };
                AnalyticsManager.instance.LogEvent("AddedVideoButton", eventParameters);
                return true;
            }
        }
        return false;
    }

    [DebuggerHidden]
    private IEnumerator ScrollToHero(int heroIndex) => 
        new <ScrollToHero>c__Iterator0 { 
            heroIndex = heroIndex,
            $this = this
        };

    public void SetOpenedheroIcon(SaveData.HeroData.HeroType _type)
    {
        this.lastOpenedHeroType = _type;
        this.animationCompleted = false;
        DataLoader.gui.popUpsPanel.heroIcon.set_texture(this.surviviorContent[this.GetHeroIndex(_type)].rawImage.get_texture());
    }

    public void SetRandomVideo()
    {
        if (!this.IsVideoUpgradeAvailable())
        {
            List<int> list = new List<int>();
            for (int i = 0; i < this.surviviorContent.Length; i++)
            {
                this.surviviorContent[i].SetVideoButton(false);
                list.Add(i);
            }
            if (GameManager.instance.IsTutorialCompleted() && (DataLoader.Instance.GetCurrentPlayerLevel() > 2))
            {
                do
                {
                    int num2 = UnityEngine.Random.Range(0, list.Count);
                    if (this.surviviorContent[list[num2]].IsVideoAvailable() || this.surviviorContent[list[num2]].SetVideoButton(true))
                    {
                        base.StartCoroutine(this.ScrollToHero(list[num2]));
                        Dictionary<string, string> eventParameters = new Dictionary<string, string> {
                            { 
                                "Hero",
                                this.surviviorContent[list[num2]].heroData.heroType.ToString()
                            }
                        };
                        AnalyticsManager.instance.LogEvent("AddedVideoButton", eventParameters);
                        break;
                    }
                    list.Remove(list[num2]);
                }
                while (list.Count > 0);
            }
        }
    }

    public override void UpdateAllContent()
    {
        for (int i = 0; i < this.surviviorContent.Length; i++)
        {
            this.surviviorContent[i].UpdateContent();
        }
    }

    public void UpdateInactiveButton()
    {
        for (int i = 0; i < this.surviviorContent.Length; i++)
        {
            this.surviviorContent[i].UpdateInactiveButton();
        }
    }

    [CompilerGenerated]
    private sealed class <DelayedAnimation>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal int <index>__0;
        internal UISurvivorUpgradePanel $this;
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
                    this.<index>__0 = this.$this.GetHeroIndex(this.$this.lastOpenedHeroType);
                    this.$current = this.$this.StartCoroutine(this.$this.ScrollToHero(this.<index>__0));
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    goto Label_010A;

                case 1:
                    this.$this.surviviorContent[this.<index>__0].unlockFX.Play();
                    SoundManager.Instance.PlaySound(SoundManager.Instance.newHeroOpened, -1f);
                    this.$current = new WaitForSeconds(0.1f);
                    if (!this.$disposing)
                    {
                        this.$PC = 2;
                    }
                    goto Label_010A;

                case 2:
                    DataLoader.gui.UpdateMenuContent();
                    this.$this.animationCompleted = true;
                    if (this.$this.lastOpenedHeroType == SaveData.HeroData.HeroType.SNIPER)
                    {
                        UIRateUs.instance.Show();
                    }
                    this.$PC = -1;
                    break;
            }
            return false;
        Label_010A:
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
    private sealed class <ScrollToHero>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal int heroIndex;
        internal RectTransform <rect>__0;
        internal float <visibleHeroes>__0;
        internal Vector2 <pos>__0;
        internal UISurvivorUpgradePanel $this;
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
                    this.heroIndex++;
                    this.$this.scrollRect.get_content().anchoredPosition = new Vector2(0f, this.$this.scrollRect.get_content().anchoredPosition.y);
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    goto Label_01EE;

                case 1:
                    this.<rect>__0 = DataLoader.gui.GetComponent<RectTransform>();
                    this.<visibleHeroes>__0 = this.<rect>__0.sizeDelta.x / (this.$this.cellPrefab.sizeDelta.x + this.$this.distanceBetweenCells);
                    this.<pos>__0 = new Vector2(-(this.heroIndex - this.<visibleHeroes>__0) * (this.$this.cellPrefab.sizeDelta.x + this.$this.distanceBetweenCells), this.$this.scrollRect.get_content().anchoredPosition.y);
                    break;

                case 2:
                    break;

                default:
                    goto Label_01EC;
            }
            if (this.$this.scrollRect.get_content().anchoredPosition.x > (this.<pos>__0.x + (this.$this.distanceBetweenCells * 2f)))
            {
                this.$this.scrollRect.get_content().anchoredPosition = Vector2.MoveTowards(this.$this.scrollRect.get_content().anchoredPosition, this.<pos>__0, Time.deltaTime * 2500f);
                this.$current = new WaitForSeconds(Time.deltaTime / 2f);
                if (!this.$disposing)
                {
                    this.$PC = 2;
                }
                goto Label_01EE;
            }
            this.$PC = -1;
        Label_01EC:
            return false;
        Label_01EE:
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

