using Dxx.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class ShopOneStageDiscount : ShopOneBase
{
    private static List<Color> mLightColors;
    private const float Width = 180f;
    public Text Text_Title;
    public Text Text_New;
    public Text Text_Content;
    public Text Text_Price;
    public Text Text_Value;
    public GameObject RewardParent;
    public ButtonCtrl Button_Click;
    public GameObject itemone;
    public GameObject itemadd;
    public Image Image_BG;
    public Image Image_Light;
    private LocalUnityObjctPool mPool;
    private List<ShopOneStageDiscountOneCtrl> mList = new List<ShopOneStageDiscountOneCtrl>();
    private List<Drop_DropModel.DropData> rewards;
    private string mID;
    [CompilerGenerated]
    private static Action <>f__am$cache0;
    [CompilerGenerated]
    private static Action<string> <>f__am$cache1;
    [CompilerGenerated]
    private static Action<bool, CRespInAppPurchase> <>f__am$cache2;
    [CompilerGenerated]
    private static Action<string> <>f__am$cache3;

    static ShopOneStageDiscount()
    {
        List<Color> list = new List<Color> {
            new Color(1f, 0.8941177f, 0.1921569f, 0.7058824f),
            new Color(0.7254902f, 1f, 0.4313726f, 0.7058824f),
            new Color(0.8627451f, 0.4980392f, 1f, 0.7058824f)
        };
        mLightColors = list;
    }

    private static bool IsUnlock()
    {
        int key = LocalSave.Instance.StageDiscount_GetCurrentID();
        Box_Activity beanById = LocalModelManager.Instance.Box_Activity.GetBeanById(key);
        if ((beanById != null) && (beanById.ShowCond.Length > 0))
        {
            int result = 0;
            int.TryParse(beanById.ShowCond[0], out result);
            switch (result)
            {
                case 1:
                {
                    if (beanById.ShowCond.Length != 2)
                    {
                        break;
                    }
                    int num3 = -1;
                    int.TryParse(beanById.ShowCond[1], out num3);
                    if ((num3 < 0) || (LocalSave.Instance.mStage.CurrentStage <= num3))
                    {
                        break;
                    }
                    return true;
                }
                case 2:
                    if (beanById.ShowCond.Length == 3)
                    {
                    }
                    break;
            }
        }
        return false;
    }

    public static bool IsValid()
    {
        if (!IsUnlock())
        {
            return false;
        }
        if (!LocalSave.Instance.StageDiscount_IsValid())
        {
            return false;
        }
        return true;
    }

    protected override void OnAwake()
    {
        this.mPool = LocalUnityObjctPool.Create(base.gameObject);
        this.mPool.CreateCache<ShopOneStageDiscountOneCtrl>(this.itemone);
        this.mPool.CreateCache<RectTransform>(this.itemadd);
        this.itemone.SetActive(false);
        this.itemadd.SetActive(false);
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = delegate {
                if (<>f__am$cache1 == null)
                {
                    <>f__am$cache1 = delegate (string data) {
                        LocalSave.StageDiscountBody body = null;
                        try
                        {
                            body = JsonConvert.DeserializeObject<LocalSave.StageDiscountBody>(data);
                            if ((body.current_purchase == null) || (body.current_purchase.product_id != LocalSave.Instance.StageDiscount_GetProductID()))
                            {
                                CInstance<TipsUIManager>.Instance.Show(ETips.Tips_StageDiscountChange, Array.Empty<string>());
                                LocalSave.Instance.StageDiscount_Init(data);
                            }
                            else
                            {
                                SdkManager.send_event_iap("CLICK", PurchaseManager.Instance.GetOpenSource(), body.current_purchase.product_id, string.Empty, string.Empty);
                                if (<>f__am$cache2 == null)
                                {
                                    <>f__am$cache2 = delegate (bool success, CRespInAppPurchase resp) {
                                        WindowUI.ShowRewardSimple(PurchaseManager.Instance.GetGotList(resp, LocalSave.Instance.StageDiscount_GetList()));
                                        LocalSave.Instance.StageDiscount_Init(null);
                                        if (<>f__am$cache3 == null)
                                        {
                                            <>f__am$cache3 = d => LocalSave.Instance.StageDiscount_Init(d);
                                        }
                                        LocalSave.Instance.StageDiscount_Send(<>f__am$cache3);
                                    };
                                }
                                PurchaseManager.Instance.OnPurchaseClicked(body.current_purchase.product_id, <>f__am$cache2);
                            }
                        }
                        catch
                        {
                            Debugger.Log("StageDiscount_Init init failed! ::: " + data);
                        }
                    };
                }
                LocalSave.Instance.StageDiscount_Send(<>f__am$cache1);
            };
        }
        this.Button_Click.onClick = <>f__am$cache0;
    }

    protected override void OnDeinit()
    {
    }

    protected override void OnInit()
    {
        this.mList.Clear();
        this.mPool.Collect<ShopOneStageDiscountOneCtrl>();
        this.mPool.Collect<RectTransform>();
        this.mID = LocalSave.Instance.StageDiscount_GetProductID();
        this.rewards = LocalSave.Instance.StageDiscount_GetList();
        if (this.rewards != null)
        {
            int count = this.rewards.Count;
            float num2 = 180f * (count - 1);
            for (int i = 0; i < count; i++)
            {
                ShopOneStageDiscountOneCtrl item = this.mPool.DeQueue<ShopOneStageDiscountOneCtrl>();
                item.Init(this.rewards[i].id, this.rewards[i].count);
                RectTransform child = item.transform as RectTransform;
                child.SetParentNormal(this.RewardParent);
                child.anchoredPosition = new Vector2((-num2 / 2f) + (180f * i), 0f);
                this.mList.Add(item);
                if ((i > 0) && (i < count))
                {
                    RectTransform transform2 = this.mPool.DeQueue<RectTransform>();
                    transform2.SetParentNormal(this.RewardParent);
                    transform2.anchoredPosition = new Vector2(((this.mList[i].transform as RectTransform).anchoredPosition.x + (this.mList[i - 1].transform as RectTransform).anchoredPosition.x) / 2f, 0f);
                }
            }
        }
        this.OnLanguageChange();
    }

    public override void OnLanguageChange()
    {
        int result = 0x65;
        if ((string.IsNullOrEmpty(this.mID) || (this.mID.Length <= 3)) || int.TryParse(this.mID.Substring(this.mID.Length - 3, 3), out result))
        {
        }
        int num2 = (result - 0x65) % mLightColors.Count;
        object[] args = new object[] { num2 };
        this.Image_BG.set_sprite(SpriteManager.GetMain(Utils.FormatString("ShopUI_Discount_BG{0}", args)));
        this.Image_Light.set_color(mLightColors[num2]);
        object[] objArray2 = new object[] { result };
        this.Text_New.text = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("discount_package_{0}描述", objArray2), Array.Empty<object>());
        object[] objArray3 = new object[] { result };
        this.Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("discount_package_{0}", objArray3), Array.Empty<object>());
        this.Text_Value.text = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("discount_value", Array.Empty<object>()), Array.Empty<object>());
        string languageByTID = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("discount_buy", Array.Empty<object>()), Array.Empty<object>());
        object[] objArray4 = new object[] { languageByTID, PurchaseManager.Instance.GetProduct_localpricestring(this.mID) };
        this.Text_Price.text = Utils.FormatString("{0} {1}", objArray4);
        for (int i = 0; i < this.mList.Count; i++)
        {
            this.mList[i].OnLanguageUpdate();
        }
    }

    public override void UpdateNet()
    {
    }
}

