using System;
using System.Runtime.InteropServices;

public class BattleStruct
{
    [StructLayout(LayoutKind.Sequential)]
    public struct BuffStruct
    {
        public EntityBase entity;
        public int buffId;
        public float[] args;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DeadStruct
    {
        public EntityBase entity;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HittedAllStruct
    {
        public EntityBase entity;
    }
}

