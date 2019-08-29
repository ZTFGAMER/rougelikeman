using Dxx.Util;
using System;
using UnityEngine;
using UnityEngine.UI;

public class EquipNameCtrl : MonoBehaviour
{
    public GameObject child;
    public Image Image_BG;
    public Text Text_Name;
    private FoodEquipBase equipitem;

    public void Init(FoodEquipBase equip)
    {
        this.equipitem = equip;
        if (equip.equipone.Overlying)
        {
            object[] args = new object[] { this.equipitem.equipone.NameString, equip.equipone.Count };
            this.Text_Name.text = Utils.FormatString("{0}x{1}", args);
        }
        else
        {
            this.Text_Name.text = this.equipitem.equipone.NameString;
        }
        this.Text_Name.set_color(LocalSave.QualityColors[this.equipitem.equipone.Quality]);
        this.Image_BG.get_rectTransform().sizeDelta = new Vector2(this.Text_Name.preferredWidth + 20f, this.Image_BG.get_rectTransform().sizeDelta.y);
    }

    private void LateUpdate()
    {
        if (this.equipitem != null)
        {
            Vector3 vector = Utils.World2Screen(this.equipitem.transform.position);
            float x = vector.x;
            float y = vector.y + (70f * GameLogic.HeightScale);
            base.transform.position = new Vector3(x, y, 0f);
        }
    }
}

