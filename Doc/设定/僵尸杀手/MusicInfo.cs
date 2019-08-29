using System;
using UnityEngine;

[Serializable]
public class MusicInfo
{
    public AudioClip clip;
    [Range(0f, 1f)]
    public float maxVolume = 1f;
}

