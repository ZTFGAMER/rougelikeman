namespace Dxx
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine.Events;

    public static class DictionaryPool<K, V>
    {
        private static readonly ObjectPool<Dictionary<K, V>> m_DictPool;

        static DictionaryPool()
        {
            DictionaryPool<K, V>.m_DictPool = new ObjectPool<Dictionary<K, V>>(null, new UnityAction<Dictionary<K, V>>(DictionaryPool<K, V>.<m_DictPool>m__0));
        }

        [CompilerGenerated]
        private static void <m_DictPool>m__0(Dictionary<K, V> d)
        {
            d.Clear();
        }

        public static Dictionary<K, V> Get() => 
            DictionaryPool<K, V>.m_DictPool.Get();

        public static void Release(Dictionary<K, V> toRelease)
        {
            DictionaryPool<K, V>.m_DictPool.Release(toRelease);
        }
    }
}

