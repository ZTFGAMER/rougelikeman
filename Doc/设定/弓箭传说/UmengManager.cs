using System;
using Umeng;
using UnityEngine;

public class UmengManager : MonoBehaviour
{
    private void Awake()
    {
        Object.DontDestroyOnLoad(base.transform.gameObject);
    }

    private void OnApplicationPause(bool isPause)
    {
        if (isPause)
        {
            Analytics.onPause();
        }
        else
        {
            Analytics.onResume();
        }
    }

    private void OnApplicationQuit()
    {
        Analytics.onKillProcess();
    }
}

