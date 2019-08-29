using System;
using System.Runtime.InteropServices;
using UnityEngine;

[StructLayout(LayoutKind.Sequential)]
public struct RemoveCallData
{
    public int entityId;
    public Vector3 deadpos;
}

