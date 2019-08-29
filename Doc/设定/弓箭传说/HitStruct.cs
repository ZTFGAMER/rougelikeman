using System;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public struct HitStruct
{
    public EntityBase source;
    public long before_hit;
    public long real_hit;
    public HitType type;
    public HitBulletStruct bulletdata;
    public HitSourceType sourcetype;
    public EElementType element;
    public int soundid;
    public int buffid;
}

