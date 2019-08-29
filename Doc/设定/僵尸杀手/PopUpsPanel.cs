using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class PopUpsPanel : MonoBehaviour
{
    public Image background;
    [Header("OpenHero")]
    public GameObject openHeroPanel;
    public Text heroName;
    public RawImage heroIcon;
    [Header("Daily")]
    public GameObject dailyPanel;
    public Text dailyHeaderText;
    [Header("Secret")]
    public GameObject secretPanel;
    [Header("ClosedWorld")]
    public GameObject closedWorldPanel;
    public Text[] bossKillsRemainingTexts;
    [Space]
    public UIPopupShop shop;
    public UIStarterPack starterPack;
    public GameObject exitPanel;

    private bool CheckException()
    {
        if (this.openHeroPanel.activeInHierarchy)
        {
            DataLoader.gui.survivorUpgradePanel.AnimateLastheroOpened();
            base.gameObject.SetActive(false);
            return true;
        }
        return DataLoader.gui.noInternetPanel.activeInHierarchy;
    }

    public void DisablePopups()
    {
        for (int i = 0; i < base.transform.childCount; i++)
        {
            if (base.transform.GetChild(i).gameObject != this.background.gameObject)
            {
                base.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        if ((!this.starterPack.autoShowCompleted && this.starterPack.IsAvailable()) && GameManager.instance.IsTutorialCompleted())
        {
            this.ShowStarter();
        }
        else
        {
            base.gameObject.SetActive(false);
        }
    }

    public void DisablePopupsWithoutBg()
    {
        for (int i = 0; i < base.transform.childCount; i++)
        {
            if (base.transform.GetChild(i).gameObject != this.background.gameObject)
            {
                base.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void OnDisable()
    {
        for (int i = 0; i < base.transform.childCount; i++)
        {
            base.transform.GetChild(i).gameObject.SetActive(false);
        }
        base.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        base.StartCoroutine(this.SmoothBackGround());
    }

    public void OpenDaily()
    {
        if (TimeManager.gotDateTime)
        {
            this.OpenPopup();
            this.dailyPanel.SetActive(true);
            UnityEngine.Object.FindObjectOfType<DailyRewardManager>().UpdateDailyRewardContent();
        }
        else
        {
            DataLoader.gui.dailyAnim.SetBool("IsOpened", false);
        }
    }

    public void OpenPopup()
    {
        base.gameObject.SetActive(true);
    }

    public void OpenSecret()
    {
        if (TimeManager.gotDateTime)
        {
            this.OpenPopup();
            this.secretPanel.SetActive(true);
        }
        else
        {
            DataLoader.gui.secretAnimator.SetBool("IsOpened", false);
        }
    }

    public void ShowStarter()
    {
        this.starterPack.Show(false);
    }

    [DebuggerHidden]
    public IEnumerator SmoothBackGround() => 
        new <SmoothBackGround>c__Iterator0 { $this = this };

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Escape) && base.gameObject.activeInHierarchy) && !this.CheckException())
        {
            base.gameObject.SetActive(false);
        }
    }

    [CompilerGenerated]
    private sealed class <SmoothBackGround>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal Color <color>__0;
        internal PopUpsPanel $this;
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
                    this.$this.background.gameObject.SetActive(true);
                    this.<color>__0 = new Color(0f, 0f, 0f, 0f);
                    this.$this.background.set_color(this.<color>__0);
                    break;

                case 1:
                    break;

                default:
                    goto Label_0137;
            }
            if (this.$this.background.get_color().a < 0.8431373f)
            {
                if ((this.<color>__0.a + (Time.deltaTime * 3f)) <= 0.8431373f)
                {
                    this.<color>__0.a += Time.deltaTime * 3f;
                    this.$this.background.set_color(this.<color>__0);
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    return true;
                }
                this.$this.background.set_color(new Color(0f, 0f, 0f, 0.8431373f));
            }
            else
            {
                this.$PC = -1;
            }
        Label_0137:
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

