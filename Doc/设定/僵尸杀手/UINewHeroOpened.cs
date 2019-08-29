using System;
using UnityEngine;

public class UINewHeroOpened : MonoBehaviour
{
    public void OnDisable()
    {
        DataLoader.gui.survivorUpgradePanel.AnimateLastheroOpened();
    }
}

