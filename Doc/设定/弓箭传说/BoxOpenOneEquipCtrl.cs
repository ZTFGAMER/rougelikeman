using DG.Tweening;
using Dxx.Util;
using System;
using UnityEngine;
using UnityEngine.UI;

public class BoxOpenOneEquipCtrl : MonoBehaviour
{
    public GameObject infoparent;
    public Text Text_Title;
    public Text Text_Name;
    public Text Text_Info;
    public Transform equipparent;
    public Image Image_Icon;
    public Image Image_BG;
    public GameObject mAddParent;
    public Text Text_Count;
    public Image Image_White;
    public GameObject fx_open;
    private Sequence seq;
    private LocalSave.EquipOne equipdata;

    public void DeInit()
    {
        this.fx_open.SetActive(false);
    }

    private Color GetColor(int quality) => 
        LocalSave.QualityColors[quality];

    public Sequence Init(LocalSave.EquipOne equip, int count)
    {
        this.DeInit();
        this.equipdata = equip;
        object[] args = new object[] { count };
        this.Text_Count.text = Utils.FormatString("x{0}", args);
        this.mAddParent.SetActive(false);
        this.fx_open.SetActive(false);
        this.equipparent.localScale = Vector3.zero;
        int quality = 1;
        if (count <= 5)
        {
            quality = 1;
        }
        else if (count <= 10)
        {
            quality = 2;
        }
        else
        {
            quality = 3;
        }
        this.Text_Title.set_color(this.GetColor(quality));
        this.Text_Count.set_color(this.Text_Title.get_color());
        object[] objArray2 = new object[1];
        object[] objArray3 = new object[] { quality };
        objArray2[0] = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("BoxOpenOne_EquipTitle{0}", objArray3), Array.Empty<object>());
        this.Text_Title.text = Utils.FormatString("{0}!", objArray2);
        this.Text_Name.text = equip.NameString;
        this.Text_Info.text = equip.InfoString;
        this.Text_Title.transform.localScale = Vector3.zero;
        this.Text_Name.transform.localScale = Vector3.zero;
        this.Text_Info.transform.localScale = Vector3.zero;
        object[] objArray4 = new object[] { this.equipdata.Quality };
        this.Image_BG.set_sprite(SpriteManager.GetCharUI(Utils.FormatString("CharUI_Quality{0}", objArray4)));
        this.Image_Icon.set_sprite(this.equipdata.Icon);
        this.Image_White.set_color(new Color(1f, 1f, 1f, 0.7f));
        this.seq = DOTween.Sequence();
        TweenSettingsExtensions.Append(this.seq, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.Text_Title.transform, 1f, 0.3f), 0x1b));
        TweenSettingsExtensions.AppendInterval(this.seq, 0.3f);
        TweenSettingsExtensions.Append(this.seq, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.Text_Name.transform, 1f, 0.3f), 0x1b));
        TweenSettingsExtensions.Append(this.seq, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.Text_Info.transform, 1f, 0.3f), 0x1b));
        TweenSettingsExtensions.AppendCallback(this.seq, new TweenCallback(this, this.<Init>m__0));
        TweenSettingsExtensions.Append(this.seq, ShortcutExtensions.DOScale(this.equipparent, 1f, 0.3f));
        TweenSettingsExtensions.AppendInterval(this.seq, 0.3f);
        TweenSettingsExtensions.AppendCallback(this.seq, new TweenCallback(this, this.<Init>m__1));
        TweenSettingsExtensions.AppendInterval(this.seq, 0.3f);
        return this.seq;
    }
}

