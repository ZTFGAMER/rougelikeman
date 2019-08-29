using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TransformParentManager : MonoBehaviour
{
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static TransformParentManager <Instance>k__BackingField;
    public Transform upgradePanelCams;
    public Transform heroes;
    public Transform moneyBox;
    public Transform zombies;
    public Transform bullets;
    public Transform fx;
    public Transform bossList;

    private void Awake()
    {
        Instance = this;
    }

    public static TransformParentManager Instance
    {
        [CompilerGenerated]
        get => 
            <Instance>k__BackingField;
        [CompilerGenerated]
        private set => 
            (<Instance>k__BackingField = value);
    }
}

