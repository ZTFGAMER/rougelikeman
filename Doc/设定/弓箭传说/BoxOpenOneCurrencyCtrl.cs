using DG.Tweening;
using Dxx.Util;
using System;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class BoxOpenOneCurrencyCtrl : MonoBehaviour
{
    public Image Image_Icon;
    public Text Text_Count;
    public Text Text_Content;
    public Transform child;
    public GameObject fx_open;
    public GameObject gift;
    public Text Text_Gift;

    public void DeInit()
    {
        this.fx_open.SetActive(false);
        this.child.localScale = Vector3.zero;
    }

    public Sequence Init(Drop_DropModel.DropData data)
    {
        this.DeInit();
        this.gift.SetActive(false);
        this.Text_Gift.text = GameLogic.Hold.Language.GetLanguageByTID("Box_Gift", Array.Empty<object>());
        this.Text_Content.text = string.Empty;
        this.Image_Icon.gameObject.SetActive(false);
        object[] args = new object[] { data.count };
        this.Text_Count.text = Utils.FormatString("x{0}", args);
        CurrencyType id = (CurrencyType) data.id;
        if (id == CurrencyType.Reborn)
        {
            this.Text_Content.text = GameLogic.Hold.Language.GetLanguageByTID("currency_reborn", Array.Empty<object>());
        }
        else
        {
            this.Image_Icon.gameObject.SetActive(true);
            this.Image_Icon.set_sprite(SpriteManager.GetUICommonCurrency(id));
        }
        this.fx_open.SetActive(true);
        return TweenSettingsExtensions.Append(DOTween.Sequence(), ShortcutExtensions.DOScale(this.child, 1f, 0.3f));
    }
}

