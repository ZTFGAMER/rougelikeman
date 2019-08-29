using System;
using UnityEngine;

[Serializable]
public class Achievements
{
    [HideInInspector]
    public int ID;
    [HideInInspector]
    public string name;
    [HideInInspector]
    public string description;
    [HideInInspector]
    public int count;
    [HideInInspector]
    public int reward;
    [HideInInspector]
    public int type;
    public Sprite icon;
}

