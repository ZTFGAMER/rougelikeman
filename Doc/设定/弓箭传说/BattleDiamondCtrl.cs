using System;
using UnityEngine;
using UnityEngine.UI;

public class BattleDiamondCtrl : MonoBehaviour
{
    public RectTransform child;
    public Image Image_Diamond;
    public Text Text_Diamond;

    private float get_x(bool value) => 
        (!value ? ((float) 200) : ((float) 0));

    public Vector3 GetDiamondPosition() => 
        this.Image_Diamond.transform.position;

    public void SetValue(long value)
    {
        if (this.Text_Diamond != null)
        {
            this.Text_Diamond.text = value.ToString();
        }
    }

    public void Show(bool value, bool rightnow)
    {
    }

    public void UpdateDiamond()
    {
        this.SetValue(GameLogic.Hold.BattleData.GetDiamond());
    }
}

