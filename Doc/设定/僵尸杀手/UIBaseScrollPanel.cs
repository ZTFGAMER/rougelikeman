using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIBaseScrollPanel : MonoBehaviour
{
    public ScrollRect scrollRect;
    [SerializeField]
    protected RectTransform cellPrefab;
    [SerializeField]
    protected float distanceBetweenCells = 10f;

    protected UIBaseScrollPanel()
    {
    }

    public abstract void CreateCells();
    public abstract void UpdateAllContent();
}

