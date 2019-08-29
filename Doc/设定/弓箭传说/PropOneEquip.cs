using Dxx.Util;
using System;
using UnityEngine;

public class PropOneEquip : PropOneBase
{
    private bool bAlreadyGot = true;
    private long gold;
    private Vector2 ImageSize;

    public Vector3 GetMiddlePosition() => 
        base.Image_BG.transform.position;

    protected override void OnAwake()
    {
        this.ImageSize = base.Image_Icon.get_rectTransform().sizeDelta;
    }

    protected override void OnClicked()
    {
    }

    protected override void OnInit()
    {
        if (base.data == null)
        {
            SdkManager.Bugly_Report("PropOneEquip", Utils.FormatString("OnInit data == null.", Array.Empty<object>()));
        }
        else
        {
            base.Text_Value.fontSize = (int) ((30f * (base.transform as RectTransform).sizeDelta.x) / 150f);
            base.Text_Value.resizeTextMinSize = base.Text_Value.fontSize / 2;
            base.Text_Value.resizeTextMaxSize = base.Text_Value.fontSize;
            base.Text_Content.text = string.Empty;
            base.Image_Icon.enabled = true;
            base.Image_BG.enabled = true;
            switch (base.data.type)
            {
                case PropType.eCurrency:
                {
                    if (!(base.data.data is CurrencyData))
                    {
                        SdkManager.Bugly_Report("PropOneEquip", Utils.FormatString("OnInit data.data is not a CurrencyData.", Array.Empty<object>()));
                        return;
                    }
                    CurrencyData data = base.data.data as CurrencyData;
                    base.Image_Type.enabled = false;
                    base.Image_QualityGold.enabled = false;
                    this.gold = data.count;
                    object[] args = new object[] { this.gold };
                    base.Text_Value.text = Utils.FormatString("x{0}", args);
                    object[] objArray2 = new object[] { 0 };
                    base.Image_BG.set_sprite(SpriteManager.GetCharUI(Utils.FormatString("CharUI_Quality{0}", objArray2)));
                    base.Image_Icon.enabled = false;
                    base.Text_Content.text = string.Empty;
                    CurrencyType id = (CurrencyType) data.id;
                    if (id == CurrencyType.Reborn)
                    {
                        base.Text_Content.text = GameLogic.Hold.Language.GetLanguageByTID("currency_reborn", Array.Empty<object>());
                    }
                    else
                    {
                        base.Image_Icon.enabled = true;
                        base.Image_Icon.set_sprite(SpriteManager.GetUICommonCurrency(id));
                        base.Image_Icon.get_rectTransform().sizeDelta = new Vector2(100f, 100f);
                    }
                    break;
                }
                case PropType.eEquip:
                {
                    if (!(base.data.data is EquipData))
                    {
                        SdkManager.Bugly_Report("PropOneEquip", Utils.FormatString("OnInit data.data is not a EquipData.", Array.Empty<object>()));
                        return;
                    }
                    EquipData data2 = base.data.data as EquipData;
                    LocalSave.EquipOne one = new LocalSave.EquipOne {
                        EquipID = data2.id
                    };
                    if ((data2.count == 0) || !one.Overlying)
                    {
                        base.Text_Value.text = string.Empty;
                    }
                    else
                    {
                        object[] objArray3 = new object[] { data2.count };
                        base.Text_Value.text = Utils.FormatString("x{0}", objArray3);
                    }
                    object[] args = new object[] { one.Quality };
                    base.Image_BG.set_sprite(SpriteManager.GetCharUI(Utils.FormatString("CharUI_Quality{0}", args)));
                    base.Image_Icon.enabled = true;
                    base.Image_Icon.set_sprite(one.Icon);
                    base.Image_Icon.get_rectTransform().sizeDelta = this.ImageSize;
                    Sprite typeIcon = one.TypeIcon;
                    base.Image_Type.enabled = typeIcon != null;
                    base.Image_Type.set_sprite(typeIcon);
                    base.Image_QualityGold.enabled = one.ShowQualityGoldImage;
                    break;
                }
            }
        }
    }

    public void SetAlreadyGet(bool alreadyget)
    {
        this.bAlreadyGot = alreadyget;
    }

    public class CurrencyData
    {
        public int id;
        public long count;
    }

    public class EquipData
    {
        public int id;
        public int count;
    }

    public class Transfer
    {
        public PropType type;
        public object data;
    }
}

