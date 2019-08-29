namespace Dxx
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine.Events;

    public static class HashSetPool<T>
    {
        private static readonly ObjectPool<HashSet<T>> m_DictPool;

        static HashSetPool()
        {
            HashSetPool<T>.m_DictPool = new ObjectPool<HashSet<T>>(null, new UnityAction<HashSet<T>>(HashSetPool<T>.<m_DictPool>m__0));
        }

        [CompilerGenerated]
        private static void <m_DictPool>m__0(HashSet<T> d)
        {
            d.Clear();
        }

        public static HashSet<T> Get() => 
            HashSetPool<T>.m_DictPool.Get();

        public static void Release(HashSet<T> toRelease)
        {
            HashSetPool<T>.m_DictPool.Release(toRelease);
        }
    }
}

