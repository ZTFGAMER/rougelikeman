using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class ClickSoundButton : MonoBehaviour
{
    [CompilerGenerated]
    private static UnityAction <>f__am$cache0;

    private void Start()
    {
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = () => SoundManager.Instance.PlayClickSound();
        }
        base.GetComponent<Button>().onClick.AddListener(<>f__am$cache0);
    }
}

