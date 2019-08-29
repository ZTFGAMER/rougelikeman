using Dxx.Util;
using System;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class BoxOpenGetOne : MonoBehaviour
{
    public GameObject currencyparent;
    public GameObject equipparent;
    public CanvasGroup mCanvasGroup;
    public Image Currency_ImageIcon;
    public Image Currency_ImageBG;
    public Text Currency_TextCount;
    public GameObject Currency_Gift;
    public Text Text_Gift;
    public Image Equip_ImageIcon;
    public Image Equip_ImageBG;
    public Text Text_Count;
    public Text Text_Content;
    protected Drop_DropModel.DropData mData;

    public void Init(Drop_DropModel.DropData data)
    {
        this.mData = data;
        this.currencyparent.SetActive(false);
        this.equipparent.SetActive(false);
        this.Currency_Gift.SetActive(false);
        this.Text_Gift.text = GameLogic.Hold.Language.GetLanguageByTID("Box_Gift", Array.Empty<object>());
        switch (data.type)
        {
            case PropType.eCurrency:
            {
                this.currencyparent.SetActive(true);
                this.Currency_ImageIcon.gameObject.SetActive(false);
                this.Text_Content.text = string.Empty;
                CurrencyType id = (CurrencyType) data.id;
                if (id == CurrencyType.Reborn)
                {
                    this.Text_Content.text = GameLogic.Hold.Language.GetLanguageByTID("currency_reborn", Array.Empty<object>());
                }
                else
                {
                    this.Currency_ImageIcon.gameObject.SetActive(true);
                    this.Currency_ImageIcon.set_sprite(SpriteManager.GetUICommonCurrency(id));
                }
                object[] args = new object[] { this.mData.count };
                this.Currency_TextCount.text = Utils.FormatString("x{0}", args);
                break;
            }
            case PropType.eEquip:
            {
                Equip_equip beanById = LocalModelManager.Instance.Equip_equip.GetBeanById(this.mData.id);
                this.equipparent.SetActive(true);
                this.Equip_ImageIcon.set_sprite(SpriteManager.GetEquip(beanById.EquipIcon));
                object[] args = new object[] { beanById.Quality };
                this.Equip_ImageBG.set_sprite(SpriteManager.GetCharUI(Utils.FormatString("CharUI_Quality{0}", args)));
                object[] objArray3 = new object[] { this.mData.count };
                this.Text_Count.text = Utils.FormatString("x{0}", objArray3);
                break;
            }
        }
    }
}

