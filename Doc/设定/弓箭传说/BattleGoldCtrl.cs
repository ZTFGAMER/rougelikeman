using System;
using UnityEngine;
using UnityEngine.UI;

public class BattleGoldCtrl : MonoBehaviour
{
    public Text Text_Gold;

    public void SetGold(long gold)
    {
        if (this.Text_Gold != null)
        {
            this.Text_Gold.text = gold.ToString();
        }
    }
}

