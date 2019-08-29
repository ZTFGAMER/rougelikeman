using IAP;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupShop : MonoBehaviour
{
    public Text lbl_money;
    public Text lbl_noAdsPrice;
    private double lastShownGold;
    public UIShopCoinPack[] packs;
    public UiShopBooster[] boosters;
    [Space(10f)]
    public float scrollCoinsPosY = 840f;
    public RectTransform scrollRt;

    private void OnEnable()
    {
        this.updateMoneyCounter();
        for (int i = 0; i < this.packs.Length; i++)
        {
            this.packs[i].lbl_price.text = InAppManager.Instance.GetPriceCointPack(i);
        }
        this.UpdateBoosters();
        this.lbl_noAdsPrice.text = InAppManager.Instance.GetPrice(InAppManager.Instance.noAds.purchaseName);
    }

    public void OnPurchaseCoins(int packIndex)
    {
        InAppManager.Instance.BuyProductID(packIndex);
    }

    public void ResetScroll()
    {
        this.scrollRt.anchoredPosition = new Vector2(0f, 0f);
    }

    public void ScrollToCoins()
    {
        this.scrollRt.anchoredPosition = new Vector2(0f, this.scrollCoinsPosY);
    }

    public void ScrollToNoAds()
    {
        this.scrollRt.anchoredPosition = new Vector2(0f, this.scrollRt.sizeDelta.y);
    }

    private void Start()
    {
        for (int i = 0; i < this.packs.Length; i++)
        {
            CoinsPurchaseInfo info = InAppManager.Instance.coinsPurchases[i];
            this.packs[i].lbl_amount.text = $"{info.coinsReward:N0}";
        }
    }

    private void Update()
    {
        if (this.lastShownGold != DataLoader.playerData.money)
        {
            this.updateMoneyCounter();
        }
    }

    public void UpdateBoosters()
    {
        for (int i = 0; i < this.boosters.Length; i++)
        {
            this.boosters[i].updatePrice();
        }
    }

    private void updateMoneyCounter()
    {
        if (DataLoader.playerData != null)
        {
            this.lastShownGold = DataLoader.playerData.money;
            this.lbl_money.text = Math.Floor(DataLoader.playerData.money).ToString();
        }
    }
}

