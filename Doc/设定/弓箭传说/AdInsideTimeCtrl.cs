using System;
using UnityEngine;
using UnityEngine.UI;

public class AdInsideTimeCtrl : MonoBehaviour
{
    public Image Image_Circle;
    public Text Text_Time;
    private float maxtime;

    public void SetCurrent(float current)
    {
        this.Text_Time.text = ((int) (this.maxtime - current)).ToString();
        this.Image_Circle.fillAmount = 1f - (current / this.maxtime);
    }

    public void SetMax(float maxtime)
    {
        this.maxtime = maxtime;
    }
}

