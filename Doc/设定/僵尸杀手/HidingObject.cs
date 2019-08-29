using System;
using UnityEngine;

public class HidingObject : MonoBehaviour
{
    [SerializeField]
    private int showOnLevel = 1;

    public void UpdateObject(int playerLevel)
    {
        base.gameObject.SetActive(this.showOnLevel <= playerLevel);
    }
}

