namespace DG.Tweening
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public static class DOTweenAnimationExtensions
    {
        public static bool IsSameOrSubclassOf<T>(this Component t) => 
            (t is T);
    }
}

