namespace Dxx
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine.Events;

    public static class ListPool<T>
    {
        private static readonly ObjectPool<List<T>> m_ListPool;

        static ListPool()
        {
            ListPool<T>.m_ListPool = new ObjectPool<List<T>>(null, new UnityAction<List<T>>(ListPool<T>.<m_ListPool>m__0));
        }

        [CompilerGenerated]
        private static void <m_ListPool>m__0(List<T> l)
        {
            l.Clear();
        }

        public static List<T> Get() => 
            ListPool<T>.m_ListPool.Get();

        public static void Release(List<T> toRelease)
        {
            ListPool<T>.m_ListPool.Release(toRelease);
        }
    }
}

