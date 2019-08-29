using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[Serializable]
public class Survivors
{
    public LanguageKeysEnum name;
    public LanguageKeysEnum fullname;
    public SaveData.HeroData.HeroType heroType;
    public Sprite icon;
    public HeroOpenType heroOpenType;
    public int requiredLevelToOpen = 1;
    public int daysToOpen;
    public LanguageKeysEnum heroStory;
    public LanguageKeysEnum shortDescriptionKey;
    [HideInInspector]
    public List<SurvivorLevels> levels;
    public SurvivorHuman survivorPrefab;

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct SurvivorLevels
    {
        public float damage;
        public int cost;
        public float power;
    }
}

