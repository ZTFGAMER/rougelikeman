using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class SampleDescriptor
{
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private bool <IsLabel>k__BackingField;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private System.Type <Type>k__BackingField;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string <DisplayName>k__BackingField;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string <Description>k__BackingField;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string <CodeBlock>k__BackingField;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private bool <IsSelected>k__BackingField;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private GameObject <UnityObject>k__BackingField;

    public SampleDescriptor(System.Type type, string displayName, string description, string codeBlock)
    {
        this.Type = type;
        this.DisplayName = displayName;
        this.Description = description;
        this.CodeBlock = codeBlock;
    }

    public void CreateUnityObject()
    {
        if (this.UnityObject == null)
        {
            this.UnityObject = new GameObject(this.DisplayName);
            this.UnityObject.AddComponent(this.Type);
        }
    }

    public void DestroyUnityObject()
    {
        if (this.UnityObject != null)
        {
            Object.Destroy(this.UnityObject);
            this.UnityObject = null;
        }
    }

    public bool IsLabel { get; set; }

    public System.Type Type { get; set; }

    public string DisplayName { get; set; }

    public string Description { get; set; }

    public string CodeBlock { get; set; }

    public bool IsSelected { get; set; }

    public GameObject UnityObject { get; set; }

    public bool IsRunning =>
        (this.UnityObject != null);
}

