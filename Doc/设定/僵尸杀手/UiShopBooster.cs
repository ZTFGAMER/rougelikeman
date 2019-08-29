using IAP;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class UiShopBooster : MonoBehaviour
{
    public Text lbl_price;
    public Text busterCountText;
    public Text newBoosterCountText;
    public OverlayParticle buyFx;
    public Image icon;
    [Space(20f)]
    public SaveData.BoostersData.BoosterType boosterType;
    public int amount = 1;
    private UIPopupShop uIPopupShop;
    private Vector2 iconStartSize;
    private Coroutine incIcon;
    private float speed = 450f;

    [DebuggerHidden]
    private IEnumerator IncIcon() => 
        new <IncIcon>c__Iterator0 { $this = this };

    public void OnBuyMoney()
    {
    }

    public void OnBuyReal()
    {
        InAppManager.Instance.BuyProductID(Enumerable.First<BoosterPurchaseInfo>(InAppManager.Instance.boosterPurchases, bp => bp.boosterType == this.boosterType).index);
    }

    private void OnEnable()
    {
        BoosterPurchaseInfo info = InAppManager.Instance.boosterPurchases.Find(b => b.boosterType == this.boosterType);
        this.lbl_price.text = InAppManager.Instance.GetPrice(info.purchaseName);
        this.newBoosterCountText.text = "+" + info.boosterCount.ToString();
    }

    public void OnShowAds()
    {
        AdsManager.instance.ShowRewarded(delegate {
            DataLoader.Instance.BuyBoosters(this.boosterType, this.amount);
            this.PurchaseFx();
            this.uIPopupShop.UpdateBoosters();
            Dictionary<string, string> eventParameters = new Dictionary<string, string> {
                { 
                    "Type",
                    this.boosterType.ToString()
                }
            };
            AnalyticsManager.instance.LogEvent("BuyBoosterVideo", eventParameters);
        });
    }

    public void PurchaseFx()
    {
        this.buyFx.Play();
        if (this.incIcon != null)
        {
            base.StopCoroutine(this.incIcon);
        }
        this.incIcon = base.StartCoroutine(this.IncIcon());
    }

    private void Start()
    {
        this.uIPopupShop = base.GetComponentInParent<UIPopupShop>();
        this.iconStartSize = this.icon.get_rectTransform().sizeDelta;
    }

    public void updatePrice()
    {
        this.busterCountText.text = DataLoader.Instance.GetBoostersCount(this.boosterType).ToString();
    }

    [CompilerGenerated]
    private sealed class <IncIcon>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal Vector2 <newSize>__0;
        internal UiShopBooster $this;
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
                    this.$this.icon.get_rectTransform().sizeDelta = this.$this.iconStartSize;
                    this.<newSize>__0 = this.$this.iconStartSize * 1.18f;
                    break;

                case 1:
                    break;

                case 2:
                    goto Label_0156;

                default:
                    goto Label_01A7;
            }
            if (this.$this.icon.get_rectTransform().sizeDelta != this.<newSize>__0)
            {
                this.$this.icon.get_rectTransform().sizeDelta = Vector2.MoveTowards(this.$this.icon.get_rectTransform().sizeDelta, this.<newSize>__0, this.$this.speed * Time.deltaTime);
                this.$current = null;
                if (!this.$disposing)
                {
                    this.$PC = 1;
                }
                goto Label_01A9;
            }
        Label_0156:
            while (this.$this.icon.get_rectTransform().sizeDelta != this.$this.iconStartSize)
            {
                this.$this.icon.get_rectTransform().sizeDelta = Vector2.MoveTowards(this.$this.icon.get_rectTransform().sizeDelta, this.$this.iconStartSize, this.$this.speed * Time.deltaTime);
                this.$current = null;
                if (!this.$disposing)
                {
                    this.$PC = 2;
                }
                goto Label_01A9;
            }
            this.$this.icon.get_rectTransform().sizeDelta = this.$this.iconStartSize;
            this.$PC = -1;
        Label_01A7:
            return false;
        Label_01A9:
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

