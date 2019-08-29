namespace IAP
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.Purchasing;

    public class InAppManager : MonoBehaviour, IStoreListener
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static InAppManager <Instance>k__BackingField;
        private static IStoreController m_StoreController;
        private static IExtensionProvider m_StoreExtensionProvider;
        public List<CoinsPurchaseInfo> coinsPurchases;
        public List<HeroesPurchaseInfo> heroesPurchases;
        public List<BoosterPurchaseInfo> boosterPurchases;
        public List<StarterPackPurchaseInfo> packs;
        public PurchaseInfo infinityMultiplier;
        public PurchaseInfo noAds;
        public StarterPackPurchaseInfo starterPack;
        [CompilerGenerated]
        private static Action<bool> <>f__am$cache0;

        public InAppManager()
        {
            Instance = this;
        }

        private void BoosterPurchased(BoosterPurchaseInfo p)
        {
            <BoosterPurchased>c__AnonStorey0 storey = new <BoosterPurchased>c__AnonStorey0 {
                p = p
            };
            UnityEngine.Debug.LogWarning(string.Concat(new object[] { "Purchased ", storey.p.purchaseName, " Booster = ", storey.p.boosterType }));
            DataLoader.Instance.BuyBoosters(storey.p.boosterType, storey.p.boosterCount);
            Enumerable.First<UiShopBooster>(UnityEngine.Object.FindObjectsOfType<UiShopBooster>(), new Func<UiShopBooster, bool>(storey.<>m__0)).PurchaseFx();
            Dictionary<string, string> eventParameters = new Dictionary<string, string> {
                { 
                    "PurchaseName",
                    storey.p.purchaseName
                }
            };
            AnalyticsManager.instance.LogPurchaseEvent("BoosterPurchased", eventParameters, (float) m_StoreController.products.WithID(storey.p.purchaseName).metadata.localizedPrice, m_StoreController.products.WithID(storey.p.purchaseName).metadata.isoCurrencyCode);
        }

        public void BuyProductID(int productIndex)
        {
            if (this.IsInitialized())
            {
                Product product = this.GetProduct(productIndex);
                if ((product != null) && product.availableToPurchase)
                {
                    UnityEngine.Debug.Log($"Purchasing product asychronously: '{product.definition.id}'");
                    m_StoreController.InitiatePurchase(product);
                }
                else
                {
                    UnityEngine.Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                }
            }
            else
            {
                this.InitializePurchasing();
                UnityEngine.Debug.Log("BuyProductID FAIL. Not initialized.");
            }
        }

        private void coinsPurchased(CoinsPurchaseInfo p)
        {
            UnityEngine.Debug.LogWarning(string.Concat(new object[] { "Purchased ", p.purchaseName, " reward = ", p.coinsReward }));
            DataLoader.Instance.RefreshMoney((double) p.coinsReward, true);
            Dictionary<string, string> eventParameters = new Dictionary<string, string> {
                { 
                    "PurchaseName",
                    p.purchaseName
                },
                { 
                    "Amount",
                    p.coinsReward.ToString()
                }
            };
            AnalyticsManager.instance.LogPurchaseEvent("CoinsPurchased", eventParameters, (float) m_StoreController.products.WithID(p.purchaseName).metadata.localizedPrice, m_StoreController.products.WithID(p.purchaseName).metadata.isoCurrencyCode);
        }

        public string GetInfinityMultiplierPrice()
        {
            if (m_StoreController != null)
            {
                return m_StoreController.products.WithID(this.infinityMultiplier.purchaseName).metadata.localizedPriceString;
            }
            return this.GetPrice(this.infinityMultiplier.purchaseName);
        }

        public string GetPrice(string purchaseName)
        {
            if (m_StoreController != null)
            {
                return m_StoreController.products.WithID(purchaseName).metadata.localizedPriceString;
            }
            return string.Empty;
        }

        public string GetPriceCointPack(int index)
        {
            <GetPriceCointPack>c__AnonStorey1 storey = new <GetPriceCointPack>c__AnonStorey1 {
                index = index
            };
            if (m_StoreController != null)
            {
                return m_StoreController.products.WithID(Enumerable.First<CoinsPurchaseInfo>(this.coinsPurchases, new Func<CoinsPurchaseInfo, bool>(storey.<>m__0)).purchaseName).metadata.localizedPriceString;
            }
            return string.Empty;
        }

        public string GetPriceHero(int index)
        {
            <GetPriceHero>c__AnonStorey2 storey = new <GetPriceHero>c__AnonStorey2 {
                index = index
            };
            if (m_StoreController != null)
            {
                return m_StoreController.products.WithID(Enumerable.First<HeroesPurchaseInfo>(this.heroesPurchases, new Func<HeroesPurchaseInfo, bool>(storey.<>m__0)).purchaseName).metadata.localizedPriceString;
            }
            return string.Empty;
        }

        public Product GetProduct(int productIndex)
        {
            if (this.GetPurchaseInfoProduct<CoinsPurchaseInfo>(this.coinsPurchases, productIndex, out Product product))
            {
                return product;
            }
            if (this.GetPurchaseInfoProduct<HeroesPurchaseInfo>(this.heroesPurchases, productIndex, out product))
            {
                return product;
            }
            if (this.GetPurchaseInfoProduct<BoosterPurchaseInfo>(this.boosterPurchases, productIndex, out product))
            {
                return product;
            }
            if (this.GetPurchaseInfoProduct<StarterPackPurchaseInfo>(this.packs, productIndex, out product))
            {
                return product;
            }
            if (this.infinityMultiplier.index == productIndex)
            {
                return m_StoreController.products.WithID(this.infinityMultiplier.purchaseName);
            }
            if (this.starterPack.index == productIndex)
            {
                return m_StoreController.products.WithID(this.starterPack.purchaseName);
            }
            if (this.noAds.index == productIndex)
            {
                return m_StoreController.products.WithID(this.noAds.purchaseName);
            }
            return null;
        }

        public bool GetPurchaseInfoProduct<T>(List<T> purchases, int productIndex, out Product product) where T: PurchaseInfo
        {
            product = null;
            foreach (PurchaseInfo info in purchases)
            {
                if (info.index == productIndex)
                {
                    product = m_StoreController.products.WithID(info.purchaseName);
                    return true;
                }
            }
            return false;
        }

        private void HeroesPurchased(HeroesPurchaseInfo p)
        {
            UnityEngine.Debug.LogWarning(string.Concat(new object[] { "Purchased ", p.purchaseName, " Hero = ", p.heroType }));
            Dictionary<string, string> eventParameters = new Dictionary<string, string> {
                { 
                    "PurchaseName",
                    p.purchaseName
                }
            };
            AnalyticsManager.instance.LogPurchaseEvent("HeroPurchased", eventParameters, (float) m_StoreController.products.WithID(p.purchaseName).metadata.localizedPrice, m_StoreController.products.WithID(p.purchaseName).metadata.isoCurrencyCode);
            DataLoader.Instance.OpenHero(p.heroType);
        }

        private void InfinityMultiplierPurchased()
        {
            UnityEngine.Debug.LogWarning("Purchased " + this.infinityMultiplier.purchaseName);
            PlayerPrefs.SetInt(StaticConstants.infinityMultiplierPurchased, 1);
            PlayerPrefs.SetInt(StaticConstants.MultiplierKey, 2);
            DataLoader.dataUpdateManager.UpdateDailyMultiplier();
            AnalyticsManager.instance.LogPurchaseEvent("InfinityMultiplierPurchased", new Dictionary<string, string>(), (float) m_StoreController.products.WithID(this.infinityMultiplier.purchaseName).metadata.localizedPrice, m_StoreController.products.WithID(this.infinityMultiplier.purchaseName).metadata.isoCurrencyCode);
        }

        public void InitializePurchasing()
        {
            if (!this.IsInitialized())
            {
                ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance(), new IPurchasingModule[0]);
                int lastIndex = 0;
                lastIndex = this.SetPurchases<CoinsPurchaseInfo>(this.coinsPurchases, builder, lastIndex);
                lastIndex = this.SetPurchases<HeroesPurchaseInfo>(this.heroesPurchases, builder, lastIndex);
                lastIndex = this.SetPurchases<BoosterPurchaseInfo>(this.boosterPurchases, builder, lastIndex);
                lastIndex = this.SetPurchases<StarterPackPurchaseInfo>(this.packs, builder, lastIndex);
                lastIndex = this.SetPurchase(this.noAds, builder, lastIndex);
                lastIndex = this.SetPurchase(this.starterPack, builder, lastIndex);
                lastIndex = this.SetPurchase(this.infinityMultiplier, builder, lastIndex);
                UnityPurchasing.Initialize(this, builder);
            }
        }

        private bool IsInitialized() => 
            ((m_StoreController != null) && (m_StoreExtensionProvider != null));

        private void NoAdsPurchased()
        {
            UnityEngine.Debug.Log("Purchased:" + this.noAds.purchaseName);
            PlayerPrefs.SetInt(StaticConstants.interstitialAdsKey, 1);
            AnalyticsManager.instance.LogPurchaseEvent("NoAdsPurchased", new Dictionary<string, string>(), (float) m_StoreController.products.WithID(this.noAds.purchaseName).metadata.localizedPrice, m_StoreController.products.WithID(this.noAds.purchaseName).metadata.isoCurrencyCode);
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            UnityEngine.Debug.Log("OnInitialized: Completed!");
            m_StoreController = controller;
            m_StoreExtensionProvider = extensions;
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            UnityEngine.Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            UnityEngine.Debug.Log($"OnPurchaseFailed: FAIL. Product: '{product.definition.storeSpecificId}', PurchaseFailureReason: {failureReason}");
        }

        private void PackPurchased(StarterPackPurchaseInfo p)
        {
            UnityEngine.Debug.LogWarning(string.Concat(new object[] { "Purchased", p.purchaseName, "Pack Type = ", p.starterType }));
            PlayerPrefs.SetInt(StaticConstants.starterPackPurchased, (int) p.starterType);
            DataLoader.Instance.RefreshMoney((double) p.coinsReward, true);
            if (DataLoader.gui.popUpsPanel.starterPack.gameObject.activeInHierarchy)
            {
                DataLoader.gui.popUpsPanel.gameObject.SetActive(false);
            }
            DataLoader.gui.popUpsPanel.starterPack.Show(false);
            for (int i = 0; i < p.boosters.Count; i++)
            {
                StarterPackPurchaseInfo.Boosters boosters = p.boosters[i];
                StarterPackPurchaseInfo.Boosters boosters2 = this.starterPack.boosters[i];
                DataLoader.Instance.BuyBoosters(boosters.boosterType, boosters2.amount);
            }
            Dictionary<string, string> eventParameters = new Dictionary<string, string> {
                { 
                    "Type",
                    p.starterType.ToString()
                }
            };
            AnalyticsManager.instance.LogPurchaseEvent("PackPurchased", eventParameters, (float) m_StoreController.products.WithID(p.purchaseName).metadata.localizedPrice, m_StoreController.products.WithID(p.purchaseName).metadata.isoCurrencyCode);
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            UnityEngine.Debug.Log($"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
            if (this.TryToProcessPurchase<CoinsPurchaseInfo>(args, this.coinsPurchases, out int num))
            {
                this.coinsPurchased(this.coinsPurchases[num]);
            }
            else if (this.TryToProcessPurchase<HeroesPurchaseInfo>(args, this.heroesPurchases, out num))
            {
                this.HeroesPurchased(this.heroesPurchases[num]);
            }
            else if (this.TryToProcessPurchase<BoosterPurchaseInfo>(args, this.boosterPurchases, out num))
            {
                this.BoosterPurchased(this.boosterPurchases[num]);
            }
            else if (this.TryToProcessPurchase<StarterPackPurchaseInfo>(args, this.packs, out num))
            {
                this.PackPurchased(this.packs[num]);
            }
            else if (this.TryToProcessPurchase(args, this.infinityMultiplier))
            {
                this.InfinityMultiplierPurchased();
            }
            else if (this.TryToProcessPurchase(args, this.starterPack))
            {
                this.StarterPurchased();
            }
            else if (this.TryToProcessPurchase(args, this.noAds))
            {
                this.NoAdsPurchased();
            }
            IOSCloudSave.instance.ForceSave();
            GPGSCloudSave.CloudSync(false);
            NoAdsManager.instance.CheckNoAds();
            return PurchaseProcessingResult.Complete;
        }

        public void ResetAllPurchases()
        {
            int lastIndex = 0;
            lastIndex = this.ResetPurchases<CoinsPurchaseInfo>(this.coinsPurchases, lastIndex);
            lastIndex = this.ResetPurchases<HeroesPurchaseInfo>(this.heroesPurchases, lastIndex);
            lastIndex = this.ResetPurchases<BoosterPurchaseInfo>(this.boosterPurchases, lastIndex);
            lastIndex = this.ResetPurchases<StarterPackPurchaseInfo>(this.packs, lastIndex);
            lastIndex = this.ResetPurchase<PurchaseInfo>(this.noAds, lastIndex);
            lastIndex = this.ResetPurchase<StarterPackPurchaseInfo>(this.starterPack, lastIndex);
            lastIndex = this.ResetPurchase<PurchaseInfo>(this.infinityMultiplier, lastIndex);
        }

        private int ResetPurchase<T>(T purchase, int lastIndex) where T: PurchaseInfo
        {
            purchase.index = lastIndex;
            return ++lastIndex;
        }

        private int ResetPurchases<T>(List<T> purchaseList, int lastIndex) where T: PurchaseInfo
        {
            for (int i = 0; i < purchaseList.Count; i++)
            {
                purchaseList[i].index = lastIndex;
                lastIndex++;
            }
            return lastIndex;
        }

        public void RestorePurchases()
        {
            if (!this.IsInitialized())
            {
                UnityEngine.Debug.Log("RestorePurchases FAIL. Not initialized.");
            }
            else if ((Application.platform == RuntimePlatform.IPhonePlayer) || (Application.platform == RuntimePlatform.OSXPlayer))
            {
                UnityEngine.Debug.Log("RestorePurchases started ...");
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = result => UnityEngine.Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
                }
                m_StoreExtensionProvider.GetExtension<IAppleExtensions>().RestoreTransactions(<>f__am$cache0);
            }
            else
            {
                UnityEngine.Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
            }
        }

        public int SetPurchase(PurchaseInfo purchase, ConfigurationBuilder builder, int lastIndex)
        {
            purchase.index = lastIndex;
            IDs storeIDs = new IDs();
            string[] stores = new string[] { "AppleAppStore" };
            storeIDs.Add(purchase.purchaseAppStore, stores);
            string[] textArray2 = new string[] { "GooglePlay" };
            storeIDs.Add(purchase.purchaseGooglePlay, textArray2);
            builder.AddProduct(purchase.purchaseName, purchase.productType, storeIDs);
            return ++lastIndex;
        }

        public int SetPurchases<T>(List<T> purchases, ConfigurationBuilder builder, int lastIndex) where T: PurchaseInfo
        {
            for (int i = 0; i < purchases.Count; i++)
            {
                purchases[i].index = lastIndex;
                IDs storeIDs = new IDs();
                string[] stores = new string[] { "AppleAppStore" };
                storeIDs.Add(purchases[i].purchaseAppStore, stores);
                string[] textArray2 = new string[] { "GooglePlay" };
                storeIDs.Add(purchases[i].purchaseGooglePlay, textArray2);
                builder.AddProduct(purchases[i].purchaseName, purchases[i].productType, storeIDs);
                lastIndex++;
            }
            return lastIndex;
        }

        private void Start()
        {
            if (m_StoreController == null)
            {
                this.InitializePurchasing();
            }
        }

        private void StarterPurchased()
        {
            UnityEngine.Debug.LogWarning("Purchased" + this.starterPack.purchaseName);
            PlayerPrefs.SetInt(StaticConstants.starterPackPurchased, 1);
            DataLoader.Instance.RefreshMoney((double) this.starterPack.coinsReward, true);
            if (DataLoader.gui.popUpsPanel.starterPack.gameObject.activeInHierarchy)
            {
                DataLoader.gui.popUpsPanel.gameObject.SetActive(false);
            }
            DataLoader.gui.popUpsPanel.starterPack.Show(false);
            for (int i = 0; i < this.starterPack.boosters.Count; i++)
            {
                StarterPackPurchaseInfo.Boosters boosters = this.starterPack.boosters[i];
                StarterPackPurchaseInfo.Boosters boosters2 = this.starterPack.boosters[i];
                DataLoader.Instance.BuyBoosters(boosters.boosterType, boosters2.amount);
            }
            AnalyticsManager.instance.LogPurchaseEvent("StarterPackPurchased", new Dictionary<string, string>(), (float) m_StoreController.products.WithID(this.starterPack.purchaseName).metadata.localizedPrice, m_StoreController.products.WithID(this.starterPack.purchaseName).metadata.isoCurrencyCode);
        }

        public bool TryToProcessPurchase(PurchaseEventArgs args, PurchaseInfo purchase) => 
            string.Equals(args.purchasedProduct.definition.id, purchase.purchaseName, StringComparison.Ordinal);

        public bool TryToProcessPurchase<T>(PurchaseEventArgs args, List<T> purchases, out int index) where T: PurchaseInfo
        {
            index = 0;
            foreach (PurchaseInfo info in purchases)
            {
                if (string.Equals(args.purchasedProduct.definition.id, info.purchaseName, StringComparison.Ordinal))
                {
                    return true;
                }
                index++;
            }
            return false;
        }

        public static InAppManager Instance
        {
            [CompilerGenerated]
            get => 
                <Instance>k__BackingField;
            [CompilerGenerated]
            private set => 
                (<Instance>k__BackingField = value);
        }

        [CompilerGenerated]
        private sealed class <BoosterPurchased>c__AnonStorey0
        {
            internal BoosterPurchaseInfo p;

            internal bool <>m__0(UiShopBooster b) => 
                (b.boosterType == this.p.boosterType);
        }

        [CompilerGenerated]
        private sealed class <GetPriceCointPack>c__AnonStorey1
        {
            internal int index;

            internal bool <>m__0(CoinsPurchaseInfo cp) => 
                (cp.index == this.index);
        }

        [CompilerGenerated]
        private sealed class <GetPriceHero>c__AnonStorey2
        {
            internal int index;

            internal bool <>m__0(HeroesPurchaseInfo hp) => 
                (hp.index == this.index);
        }
    }
}

