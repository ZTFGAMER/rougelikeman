using Dxx.Util;
using System;
using UnityEngine;
using UnityEngine.UI;

public class BoxOpenSingleRetryCtrl : MonoBehaviour
{
    public GameObject child;
    public Text Text_RetryFree;
    public Text Text_RetryExtra;
    public Text Text_RetryNotFree;
    public Image Image_Extra;
    public ButtonCtrl Button_Retry;
    public GoldTextCtrl mGoldNow;
    public GoldTextCtrl mGoldOld;
    public RedNodeCtrl mRedNodeCtrl;
    public GameObject notfreeparent;
    public GameObject freeparent;
    public GameObject extraparent;
    public Text Text_Extra;
    public Action onRetry;
    private float retry_y;
    private float now_y;
    private float old_y;
    private RectTransform rect_now;
    private RectTransform rect_old;

    private void Awake()
    {
        this.Button_Retry.SetDepondNet(true);
        this.Button_Retry.onClick = delegate {
            if (this.onRetry != null)
            {
                this.onRetry();
            }
        };
        this.rect_now = this.mGoldNow.transform as RectTransform;
        this.rect_old = this.mGoldOld.transform as RectTransform;
        this.now_y = this.rect_now.anchoredPosition.y;
        this.old_y = this.rect_old.anchoredPosition.y;
        this.retry_y = this.Text_RetryNotFree.get_rectTransform().anchoredPosition.y;
    }

    public void Init(LocalSave.TimeBoxType type, int now, int old)
    {
        CurrencyType diamondBoxNormal = CurrencyType.DiamondBoxNormal;
        if (type == LocalSave.TimeBoxType.BoxChoose_DiamondLarge)
        {
            diamondBoxNormal = CurrencyType.DiamondBoxLarge;
        }
        this.Image_Extra.set_sprite(SpriteManager.GetUICommonCurrency(diamondBoxNormal));
        int timeBoxCount = LocalSave.Instance.GetTimeBoxCount(type);
        this.mRedNodeCtrl.SetType(RedNodeType.eRedCount);
        this.mRedNodeCtrl.Value = timeBoxCount;
        this.rect_old.anchoredPosition = new Vector2(0f, this.old_y);
        this.rect_now.anchoredPosition = new Vector2(0f, this.now_y);
        this.Text_RetryNotFree.get_rectTransform().anchoredPosition = new Vector2(0f, this.retry_y);
        this.freeparent.SetActive(false);
        this.notfreeparent.SetActive(false);
        this.extraparent.SetActive(false);
        if (timeBoxCount > 0)
        {
            this.freeparent.SetActive(true);
        }
        else
        {
            int diamondExtraCount = 0;
            diamondExtraCount = LocalSave.Instance.GetDiamondExtraCount(type);
            if (diamondExtraCount > 0)
            {
                this.extraparent.SetActive(true);
                object[] args = new object[] { diamondExtraCount };
                this.Text_Extra.text = Utils.FormatString("{0}/1", args);
            }
            else
            {
                this.notfreeparent.SetActive(true);
                if (now == old)
                {
                    this.Text_RetryNotFree.get_rectTransform().anchoredPosition = new Vector2(0f, 20f);
                    this.rect_now.anchoredPosition = new Vector2(0f, this.old_y);
                    this.mGoldOld.gameObject.SetActive(false);
                }
                else
                {
                    this.mGoldNow.gameObject.SetActive(true);
                    this.mGoldOld.gameObject.SetActive(true);
                }
                this.mGoldNow.SetValue(now);
                this.mGoldOld.SetValue(old);
            }
        }
    }

    public void OnLanguageChange()
    {
        this.Text_RetryFree.text = GameLogic.Hold.Language.GetLanguageByTID("商店_免费抽取", Array.Empty<object>());
        this.Text_RetryExtra.text = GameLogic.Hold.Language.GetLanguageByTID("shopui_buy_again", Array.Empty<object>());
        this.Text_RetryNotFree.text = GameLogic.Hold.Language.GetLanguageByTID("shopui_buy_again", Array.Empty<object>());
    }

    public void Show(bool value)
    {
        this.child.SetActive(value);
    }
}

