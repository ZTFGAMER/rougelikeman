using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class GameTools
{
    public static void DestroyChildren(this Transform t)
    {
        bool isPlaying = Application.isPlaying;
        while (t.childCount != 0)
        {
            Transform child = t.GetChild(0);
            if (isPlaying)
            {
                child.SetParent(null);
                Object.Destroy(child.gameObject);
            }
            else
            {
                Object.DestroyImmediate(child.gameObject);
            }
        }
    }

    public static List<T> GetComponentsInChildrens<T>(this GameObject t) where T: Component
    {
        List<T> list = new List<T>();
        int childCount = t.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = t.transform.GetChild(i);
            T component = child.GetComponent<T>();
            if (component != null)
            {
                list.Add(component);
            }
            List<T> componentsInChildrens = child.gameObject.GetComponentsInChildrens<T>();
            for (int j = 0; j < componentsInChildrens.Count; j++)
            {
                list.Add(componentsInChildrens[j]);
            }
        }
        return list;
    }

    public static void RandomSort<T>(this List<T> list)
    {
        int index = 0;
        int count = list.Count;
        while (index < count)
        {
            T item = list[index];
            list.RemoveAt(index);
            int num3 = GameLogic.Random(0, list.Count);
            list.Insert(num3, item);
            index++;
        }
    }

    public static void SetLeft(this GameObject o)
    {
        RectTransform t = o.transform as RectTransform;
        if (t != null)
        {
            SetLeftInternal(t);
        }
    }

    public static void SetLeft(this RectTransform t)
    {
        SetLeftInternal(t);
    }

    public static void SetLeft(this Transform t)
    {
        RectTransform transform = t as RectTransform;
        if (transform != null)
        {
            SetLeftInternal(transform);
        }
    }

    public static void SetLeftInternal(RectTransform t)
    {
        t.anchorMin = Vector2.up;
        t.anchorMax = Vector2.up;
        t.pivot = new Vector2(0f, 0.5f);
    }

    public static void SetLeftTop(this GameObject o)
    {
        RectTransform t = o.transform as RectTransform;
        if (t != null)
        {
            SetLeftTopInternal(t);
        }
    }

    public static void SetLeftTop(this RectTransform t)
    {
        SetLeftTopInternal(t);
    }

    public static void SetLeftTop(this Transform t)
    {
        RectTransform transform = t as RectTransform;
        if (transform != null)
        {
            SetLeftTopInternal(transform);
        }
    }

    public static void SetLeftTopInternal(RectTransform t)
    {
        t.anchorMin = Vector2.up;
        t.anchorMax = Vector2.up;
        t.pivot = new Vector2(0f, 1f);
    }

    public static void SetParentNormal(this GameObject child, GameObject parent)
    {
        SetParentNormalInternal(child.transform, parent.transform);
    }

    public static void SetParentNormal(this GameObject child, Transform parent)
    {
        SetParentNormalInternal(child.transform, parent);
    }

    public static void SetParentNormal(this Transform child, GameObject parent)
    {
        SetParentNormalInternal(child, parent.transform);
    }

    public static void SetParentNormal(this Transform child, Transform parent)
    {
        SetParentNormalInternal(child, parent);
    }

    private static void SetParentNormalInternal(Transform child, Transform parent)
    {
        child.SetParent(parent, false);
        RectTransform transform = child as RectTransform;
        child.localPosition = Vector3.zero;
        child.localScale = Vector3.one;
        child.localEulerAngles = Vector3.zero;
        if (transform != null)
        {
            transform.anchoredPosition = Vector2.zero;
        }
    }

    public static void SetTop(this RectTransform t)
    {
        if (t != null)
        {
            SetTopInternal(t);
        }
    }

    public static void SetTop(this Transform t)
    {
        if (t != null)
        {
            RectTransform transform = t as RectTransform;
            if (transform != null)
            {
                SetTopInternal(transform);
            }
        }
    }

    public static void SetTopInternal(RectTransform t)
    {
        t.anchorMin = new Vector2(0.5f, 1f);
        t.anchorMax = new Vector2(0.5f, 1f);
        t.pivot = new Vector2(0.5f, 1f);
    }
}

