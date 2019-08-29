using System;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyLevelCtrl : MonoBehaviour
{
    public Text Text_Level;
    public ProgressCtrl mProgressCtrl;

    public void UpdateUI()
    {
        int level = LocalSave.Instance.GetLevel();
        this.Text_Level.text = level.ToString();
        int exp = (int) LocalSave.Instance.GetExp();
        int expByLevel = LocalSave.Instance.GetExpByLevel(level);
        this.mProgressCtrl.Value = ((float) exp) / ((float) expByLevel);
    }
}

