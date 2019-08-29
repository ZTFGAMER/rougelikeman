using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static UIController <instance>k__BackingField;

    public void Awake()
    {
        instance = this;
    }

    public RectTransform[] CreareScrollContent(RectTransform cellPrefab, ScrollRect scrollRect, int cellCount, float startBorder, float spaceBetweenCells, bool horizontal)
    {
        scrollRect.horizontal = horizontal;
        scrollRect.vertical = !horizontal;
        RectTransform[] transformArray = new RectTransform[cellCount];
        if (horizontal)
        {
            float x = cellPrefab.rect.x + startBorder;
            for (int j = 0; j < cellCount; j++)
            {
                RectTransform transform = UnityEngine.Object.Instantiate<RectTransform>(cellPrefab, scrollRect.get_content());
                transform.anchoredPosition = new Vector2(x, transform.anchoredPosition.y);
                x += (transform.rect.width * transform.localScale.x) + spaceBetweenCells;
                transformArray[j] = transform;
            }
            scrollRect.get_content().sizeDelta = new Vector2(((cellPrefab.sizeDelta.x + spaceBetweenCells) * cellCount) - spaceBetweenCells, scrollRect.get_content().sizeDelta.y);
            return transformArray;
        }
        float y = cellPrefab.rect.y - startBorder;
        for (int i = 0; i < cellCount; i++)
        {
            RectTransform transform2 = UnityEngine.Object.Instantiate<RectTransform>(cellPrefab, scrollRect.get_content());
            transform2.anchoredPosition = new Vector2(transform2.anchoredPosition.x, y);
            y -= (transform2.rect.height * transform2.localScale.y) + spaceBetweenCells;
            transformArray[i] = transform2;
        }
        scrollRect.get_content().sizeDelta = new Vector2(scrollRect.get_content().sizeDelta.x, ((cellPrefab.sizeDelta.y + spaceBetweenCells) * cellCount) - spaceBetweenCells);
        return transformArray;
    }

    public static UIController instance
    {
        [CompilerGenerated]
        get => 
            <instance>k__BackingField;
        [CompilerGenerated]
        private set => 
            (<instance>k__BackingField = value);
    }
}

