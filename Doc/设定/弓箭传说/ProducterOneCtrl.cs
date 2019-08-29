using System;
using UnityEngine;
using UnityEngine.UI;

public class ProducterOneCtrl : MonoBehaviour
{
    public Text Text_Name;

    public void Init(string name)
    {
        this.Text_Name.text = name;
    }
}

