using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class GameUtility
{
    public static void AddChildToTarget(Transform target, Transform child)
    {
        child.parent = target;
        child.localScale = Vector3.one;
        child.localPosition = Vector3.zero;
        child.localEulerAngles = Vector3.zero;
        child.ChangeChildLayer(target.gameObject.layer);
    }

    public static void ChangeChildLayer(this GameObject o, int layer)
    {
        o.transform.ChangeChildLayer(layer);
    }

    public static void ChangeChildLayer(this Transform t, int layer)
    {
        t.gameObject.layer = layer;
        for (int i = 0; i < t.childCount; i++)
        {
            Transform child = t.GetChild(i);
            child.gameObject.layer = layer;
            child.ChangeChildLayer(layer);
        }
    }

    public static Transform FindDeepChild(GameObject _target, string _childName)
    {
        Transform transform = null;
        transform = _target.transform.Find(_childName);
        if (transform == null)
        {
            IEnumerator enumerator = _target.transform.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Transform current = enumerator.Current as Transform;
                transform = FindDeepChild(current.gameObject, _childName);
                if (transform != null)
                {
                    return transform;
                }
            }
        }
        return transform;
    }

    public static T FindDeepChild<T>(GameObject _target, string _childName) where T: Component
    {
        Transform transform = FindDeepChild(_target, _childName);
        if (transform != null)
        {
            return transform.gameObject.GetComponent<T>();
        }
        return null;
    }
}

