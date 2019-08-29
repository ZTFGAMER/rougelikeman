using DG.Tweening;
using Dxx.Net;
using Dxx.Util;
using GameProtocol;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using TableTool;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;

public class PurchaseManager : MonoBehaviour, IStoreListener
{
    public Dictionary<int, string> mDataInt;
    public Dictionary<string, string> mDataString;
    public const string TransactionID = "transactionid";
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static PurchaseManager <Instance>k__BackingField;
    private Dictionary<int, string> mProductList;
    private IStoreController controller;
    private IAppleExtensions m_AppleExtensions;
    private CrossPlatformValidator validator;
    private bool m_PurchaseInProgress;
    private ShopOpenSource opensource;
    private Action<bool, CRespInAppPurchase> m_PurchaseCallback;
    private Sequence seq_purchase;
    [CompilerGenerated]
    private static Action <>f__am$cache0;

    public PurchaseManager()
    {
        Dictionary<int, string> dictionary = new Dictionary<int, string> {
            { 
                0,
                "$0.99"
            },
            { 
                1,
                "$4.99"
            },
            { 
                2,
                "$9.99"
            },
            { 
                3,
                "$19.99"
            },
            { 
                4,
                "$49.99"
            },
            { 
                5,
                "$99.99"
            }
        };
        this.mDataInt = dictionary;
        Dictionary<string, string> dictionary2 = new Dictionary<string, string> {
            { 
                "com.habby.archero_d1",
                "$0.99"
            },
            { 
                "com.habby.archero_d2",
                "$4.99"
            },
            { 
                "com.habby.archero_d3",
                "$9.99"
            },
            { 
                "com.habby.archero_d4",
                "$19.99"
            },
            { 
                "com.habby.archero_d5",
                "$49.99"
            },
            { 
                "com.habby.archero_d6",
                "$99.99"
            },
            { 
                "com.habby.archero_discount101",
                "$0.99"
            },
            { 
                "com.habby.archero_discount102",
                "$4.99"
            },
            { 
                "com.habby.archero_discount103",
                "$4.99"
            },
            { 
                "com.habby.archero_discount104",
                "$9.99"
            },
            { 
                "com.habby.archero_discount105",
                "$9.99"
            },
            { 
                "com.habby.archero_discount106",
                "$19.99"
            },
            { 
                "com.habby.archero_discount107",
                "$19.99"
            },
            { 
                "com.habby.archero_discount108",
                "$19.99"
            },
            { 
                "com.habby.archero_discount109",
                "$49.99"
            },
            { 
                "com.habby.archero_discount110",
                "$49.99"
            },
            { 
                "com.habby.archero_discount111",
                "$49.99"
            },
            { 
                "com.habby.archero_discount112",
                "$49.99"
            }
        };
        this.mDataString = dictionary2;
        this.mProductList = new Dictionary<int, string>();
    }

    public List<Drop_DropModel.DropData> GetGotList(CRespInAppPurchase data, List<Drop_DropModel.DropData> currencylist)
    {
        List<Drop_DropModel.DropData> list = new List<Drop_DropModel.DropData>();
        if (currencylist != null)
        {
            int num = 0;
            int count = currencylist.Count;
            while (num < count)
            {
                Drop_DropModel.DropData item = currencylist[num];
                if (item.is_base_currency)
                {
                    list.Add(item);
                }
                num++;
            }
        }
        if ((data != null) && (data.m_arrEquipInfo != null))
        {
            int index = 0;
            int length = data.m_arrEquipInfo.Length;
            while (index < length)
            {
                Drop_DropModel.DropData item = new Drop_DropModel.DropData {
                    type = PropType.eEquip,
                    id = (int) data.m_arrEquipInfo[index].m_nEquipID,
                    count = (int) data.m_arrEquipInfo[index].m_nFragment
                };
                list.Add(item);
                index++;
            }
        }
        return list;
    }

    public ShopOpenSource GetOpenSource() => 
        this.opensource;

    public string GetProduct_localpricestring(int index)
    {
        if (this.mProductList.ContainsKey(index))
        {
            return this.GetProduct_localpricestring(this.mProductList[index]);
        }
        if (this.mDataInt.ContainsKey(index))
        {
            return this.mDataInt[index];
        }
        return index.ToString();
    }

    public string GetProduct_localpricestring(string id)
    {
        ProductMetadata productMetadata = this.GetProductMetadata(id);
        if ((productMetadata != null) && !string.IsNullOrEmpty(productMetadata.localizedPriceString))
        {
            return productMetadata.localizedPriceString;
        }
        if (this.mDataString.ContainsKey(id))
        {
            return this.mDataString[id];
        }
        return id;
    }

    public string GetProductID(int index)
    {
        if (this.mProductList.ContainsKey(index))
        {
            return this.mProductList[index];
        }
        return string.Empty;
    }

    public ProductMetadata GetProductMetadata(int index)
    {
        if (!this.mProductList.ContainsKey(index))
        {
            return null;
        }
        return this.GetProductMetadata(this.mProductList[index]);
    }

    public ProductMetadata GetProductMetadata(string id)
    {
        if ((this.controller != null) && (this.controller.products != null))
        {
            Product product = this.controller.products.WithID(id);
            if (product != null)
            {
                return product.metadata;
            }
        }
        return null;
    }

    private void init()
    {
        StandardPurchasingModule first = StandardPurchasingModule.Instance();
        first.useFakeStoreUIMode = FakeStoreUIMode.StandardUser;
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(first, Array.Empty<IPurchasingModule>());
        builder.useCatalogProvider = true;
        builder.Configure<IMoolahConfiguration>().appKey = "ea6cb49d4d909aa31691e0472c6044f4";
        builder.Configure<IMoolahConfiguration>().hashKey = "cc";
        builder.Configure<IUnityChannelConfiguration>().fetchReceiptPayloadOnPurchase = false;
        ProductCatalog catalog = ProductCatalog.LoadDefaultCatalog();
        int key = 0;
        foreach (ProductCatalogItem item in catalog.allValidProducts)
        {
            if (item.allStoreIDs.Count > 0)
            {
                IDs storeIDs = new IDs();
                foreach (StoreID eid in item.allStoreIDs)
                {
                    string[] stores = new string[] { eid.store };
                    storeIDs.Add(eid.id, stores);
                }
                this.mProductList.Add(key, item.id);
                key++;
                builder.AddProduct(item.id, item.type, storeIDs);
            }
            else
            {
                this.mProductList.Add(key, item.id);
                key++;
                builder.AddProduct(item.id, item.type);
            }
        }
        builder.Configure<IMoolahConfiguration>().SetMode(CloudMoolahMode.AlwaysSucceed);
        this.validator = new CrossPlatformValidator(GooglePlayTangle.Data(), AppleTangle.Data(), Application.identifier);
        UnityPurchasing.Initialize(this, builder);
    }

    private bool is_only_one_currency(string id)
    {
        bool flag = false;
        if (((!id.Equals("com.habby.archero_d1") && !id.Equals("com.habby.archero_d2")) && (!id.Equals("com.habby.archero_d3") && !id.Equals("com.habby.archero_d4"))) && (!id.Equals("com.habby.archero_d5") && !id.Equals("com.habby.archero_d6")))
        {
            return flag;
        }
        return true;
    }

    public bool IsValid() => 
        (this.controller != null);

    private void KillSequence()
    {
        if (this.seq_purchase != null)
        {
            TweenExtensions.Kill(this.seq_purchase, false);
            this.seq_purchase = null;
        }
    }

    private void OnDeferred(Product item)
    {
        this.SetProgress(false);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debugger.Log("支付 OnInitialized success");
        this.controller = controller;
        this.m_AppleExtensions = extensions.GetExtension<IAppleExtensions>();
        this.m_AppleExtensions.RegisterPurchaseDeferredListener(new Action<Product>(this.OnDeferred));
        Facade.Instance.SendNotification("ShopUI_Update");
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debugger.Log("支付 OnInitializedFailed " + error.ToString());
    }

    public void OnPurchaseClicked(string productId, Action<bool, CRespInAppPurchase> callback = null)
    {
        if (!NetManager.IsNetConnect)
        {
            CInstance<TipsUIManager>.Instance.Show(ETips.Tips_NetError, Array.Empty<string>());
        }
        else if (this.m_PurchaseInProgress)
        {
            CInstance<TipsUIManager>.Instance.Show(GameLogic.Hold.Language.GetLanguageByTID("商店_正在支付", Array.Empty<object>()));
        }
        else if (this.controller != null)
        {
            this.m_PurchaseCallback = callback;
            this.controller.InitiatePurchase(productId);
            this.SetProgress(true);
            Debugger.Log("购买 " + productId);
        }
        else
        {
            CInstance<TipsUIManager>.Instance.Show(ETips.Tips_ShopNotReady, Array.Empty<string>());
            Debugger.Log("PurchaseManager.controller is null");
        }
    }

    public void OnPurchaseFailed(Product item, PurchaseFailureReason r)
    {
        <OnPurchaseFailed>c__AnonStorey1 storey = new <OnPurchaseFailed>c__AnonStorey1 {
            item = item,
            $this = this
        };
        this.SetProgress(false);
        SdkManager.send_event_iap("FINISH", this.opensource, storey.item.definition.id, "FAIL", r.ToString().ToUpper());
        if (r != PurchaseFailureReason.PurchasingUnavailable)
        {
            if (r == PurchaseFailureReason.UserCancelled)
            {
                return;
            }
        }
        else
        {
            string title = GameLogic.Hold.Language.GetLanguageByTID("shopui_purchase_unavailable_title", Array.Empty<object>());
            string str2 = GameLogic.Hold.Language.GetLanguageByTID("shopui_purchase_unavailable_content", Array.Empty<object>());
            string str3 = GameLogic.Hold.Language.GetLanguageByTID("shopui_purchase_unavailable_ok", Array.Empty<object>());
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = delegate {
                };
            }
            WindowUI.ShowPopWindowOneUI(title, str2, str3, true, <>f__am$cache0);
            return;
        }
        string languageByTID = GameLogic.Hold.Language.GetLanguageByTID("shopui_purchase_fail_title", Array.Empty<object>());
        string content = GameLogic.Hold.Language.GetLanguageByTID("shopui_purchase_fail_content", Array.Empty<object>());
        string sure = GameLogic.Hold.Language.GetLanguageByTID("shopui_purchase_fail_retry", Array.Empty<object>());
        WindowUI.ShowPopWindowOneUI(languageByTID, content, sure, true, new Action(storey.<>m__0));
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {
        this.SetProgress(false);
        this.Send(e.purchasedProduct.definition.id, e.purchasedProduct.receipt);
        return PurchaseProcessingResult.Complete;
    }

    private void Send(string id, string receipt)
    {
        <Send>c__AnonStorey0 storey = new <Send>c__AnonStorey0 {
            id = id,
            receipt = receipt,
            $this = this
        };
        SdkManager.GameCenter_clear_login_count();
        Debugger.Log(Debugger.Tag.ePurchase, "id: " + storey.id + ", Receipt: " + storey.receipt);
        LocalSave.Instance.mPurchase.AddPurchase(storey.receipt);
        CInAppPurchase packet = new CInAppPurchase {
            m_nTransID = LocalSave.Instance.SaveExtra.GetTransID(),
            m_nPlatformIndex = 1,
            m_nProductID = storey.id,
            m_strReceiptData = storey.receipt
        };
        if (this.is_only_one_currency(storey.id))
        {
            CRespInAppPurchase purchase2 = new CRespInAppPurchase();
            if (this.m_PurchaseCallback != null)
            {
                purchase2.product_id = storey.id;
                this.m_PurchaseCallback(true, purchase2);
            }
        }
        CInstance<TipsUIManager>.Instance.Show(ETips.Tips_PurchaseSuccess, Array.Empty<string>());
        Debugger.Log(Debugger.Tag.eHTTP, "purchaseandroid id = " + packet.m_nProductID + " receipt = " + packet.m_strReceiptData);
        NetManager.SendInternal<CInAppPurchase>(packet, SendType.eCache, new Action<NetResponse>(storey.<>m__0));
    }

    public void SetOpenSource(ShopOpenSource source)
    {
        this.opensource = source;
    }

    private void SetProgress(bool value)
    {
        this.m_PurchaseInProgress = value;
    }

    private void ShowUI(int purchase_state, string id, string receipt)
    {
    }

    private void Start()
    {
        Instance = this;
        TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(DOTween.Sequence(), 0.1f), new TweenCallback(this, this.<Start>m__0));
    }

    public static PurchaseManager Instance
    {
        [CompilerGenerated]
        get => 
            <Instance>k__BackingField;
        [CompilerGenerated]
        private set => 
            (<Instance>k__BackingField = value);
    }

    [CompilerGenerated]
    private sealed class <OnPurchaseFailed>c__AnonStorey1
    {
        internal Product item;
        internal PurchaseManager $this;

        internal void <>m__0()
        {
            this.$this.OnPurchaseClicked(this.item.definition.id, this.$this.m_PurchaseCallback);
        }
    }

    [CompilerGenerated]
    private sealed class <Send>c__AnonStorey0
    {
        internal string id;
        internal string receipt;
        internal PurchaseManager $this;

        internal void <>m__0(NetResponse response)
        {
            if (response.data != null)
            {
                CRespInAppPurchase data = response.data as CRespInAppPurchase;
                if (data != null)
                {
                    LocalSave.Instance.Equip_Add(data.m_arrEquipInfo);
                    LocalSave.Instance.mPurchase.RemovePurchase(data.m_strIAPTransID);
                    LocalSave.Instance.UserInfo_SetRebornCount(data.m_nBattleRebornCount);
                    if (!this.$this.is_only_one_currency(this.id))
                    {
                        LocalSave.Instance.UserInfo_SetDiamond((int) data.m_nTotalDiamonds);
                        LocalSave.Instance.UserInfo_SetGold((int) data.m_nTotalCoins);
                        LocalSave.Instance.UserInfo_SetRebornCount(data.m_nBattleRebornCount);
                        LocalSave.Instance.SetDiamondExtraCount(LocalSave.TimeBoxType.BoxChoose_DiamondNormal, data.m_nNormalDiamondItems);
                        LocalSave.Instance.SetDiamondExtraCount(LocalSave.TimeBoxType.BoxChoose_DiamondLarge, data.m_nLargeDiamondItems);
                        data.product_id = this.id;
                        if (this.$this.m_PurchaseCallback != null)
                        {
                            this.$this.m_PurchaseCallback(true, data);
                        }
                    }
                    SdkManager.send_event_iap("FINISH", this.$this.opensource, this.id, "SUCCESS", string.Empty);
                }
                else
                {
                    CInstance<TipsUIManager>.Instance.Show("InAppPurchase response is not a CRespInAppPurchase type");
                    SdkManager.send_event_iap("FINISH", this.$this.opensource, this.id, "FAIL", "TYPE_ERROR");
                }
            }
            else if (response.error != null)
            {
                object[] args = new object[] { this.id, response.error.m_nStatusCode };
                SdkManager.Bugly_Report("PurchaseManager", Utils.FormatString("ProcessPurchase id:{0} response error code:{1} ", args), this.receipt);
                object[] objArray2 = new object[] { response.error.m_nStatusCode };
                SdkManager.send_event_iap("FINISH", this.$this.opensource, this.id, "FAIL", Utils.FormatString("SERVER_ERROR_CODE_{0}", objArray2));
            }
            else
            {
                SdkManager.Bugly_Report("PurchaseManager", "response.error == null");
                SdkManager.send_event_iap("FINISH", this.$this.opensource, this.id, "FAIL", "ERROR_NULL");
            }
        }
    }
}

