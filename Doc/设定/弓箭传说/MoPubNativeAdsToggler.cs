using System;
using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
public class MoPubNativeAdsToggler : MonoBehaviour
{
    private void Awake()
    {
        if (Application.isPlaying)
        {
            base.enabled = false;
        }
    }

    private void Update()
    {
        base.gameObject.hideFlags = HideFlags.HideInInspector | HideFlags.HideInHierarchy;
        IEnumerator enumerator = base.transform.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                Transform current = (Transform) enumerator.Current;
                current.gameObject.SetActive(false);
            }
        }
        finally
        {
            if (enumerator is IDisposable disposable)
            {
                IDisposable disposable;
                disposable.Dispose();
            }
        }
    }
}

