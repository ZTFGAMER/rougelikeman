namespace Dxx
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public static class ExtensionMethods
    {
        public static bool Contains(this LayerMask mask, int layer) => 
            ((mask.value & (((int) 1) << layer)) > 0);

        public static bool Contains(this LayerMask mask, GameObject gameobject) => 
            ((mask.value & (((int) 1) << gameobject.layer)) > 0);

        public static float ToFloat(this int @this) => 
            Convert.ToSingle(@this);

        public static int ToInt(this float @this) => 
            Convert.ToInt32(@this);
    }
}

