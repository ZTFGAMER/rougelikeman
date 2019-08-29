using System;
using UnityEngine;

public class UIWidthScaler : MonoBehaviour
{
    private void Start()
    {
        base.transform.localScale = Vector3.one * GameLogic.WidthScaleAll;
        base.enabled = false;
    }
}

