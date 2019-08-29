using Dxx;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : SingletonMono<GameObjectPool>
{
    protected static Dictionary<string, PoolCache> cacheDict = new Dictionary<string, PoolCache>();

    public static void Clear()
    {
        Dictionary<string, PoolCache>.Enumerator enumerator = cacheDict.GetEnumerator();
        List<string> toRelease = ListPool<string>.Get();
        while (enumerator.MoveNext())
        {
            KeyValuePair<string, PoolCache> current = enumerator.Current;
            toRelease.Add(current.Key);
        }
        for (int i = 0; i < toRelease.Count; i++)
        {
            DeletePool(toRelease[i]);
        }
        ListPool<string>.Release(toRelease);
    }

    public static void CreatePool(string path)
    {
        if (!cacheDict.ContainsKey(path))
        {
            cacheDict.Add(path, new ResourcePool(SingletonMono<GameObjectPool>.Instance.trans, path));
        }
    }

    public static void CreatePool(string poolKey, GameObject obj)
    {
        if (!cacheDict.ContainsKey(poolKey))
        {
            cacheDict.Add(poolKey, new CustomPool(SingletonMono<GameObjectPool>.Instance.trans, obj));
        }
    }

    public static void DeletePool(string path)
    {
        if (cacheDict.ContainsKey(path))
        {
            cacheDict[path].OnDestroy();
            cacheDict.Remove(path);
        }
    }

    public static GameObject Get(string poolKey)
    {
        if (cacheDict.ContainsKey(poolKey))
        {
            return cacheDict[poolKey].Get();
        }
        return null;
    }

    public static bool HasPool(string path) => 
        cacheDict.ContainsKey(path);

    public static GameObject Instantiate(string resPath)
    {
        if (!HasPool(resPath))
        {
            CreatePool(resPath);
        }
        return Get(resPath);
    }

    public static bool Release(string poolKey, GameObject obj)
    {
        if (cacheDict.ContainsKey(poolKey))
        {
            cacheDict[poolKey].Release(obj);
            return true;
        }
        return false;
    }

    protected class CustomPool : GameObjectPool.PoolCache
    {
        public CustomPool(Transform cacheParent, GameObject origin) : base(cacheParent)
        {
            base.origin = origin;
        }
    }

    protected class PoolCache : UnityObjectPool<GameObject>
    {
        protected GameObject origin;
        protected Transform cacheParent;

        public PoolCache(Transform cacheParent) : base(null, null, null)
        {
            this.cacheParent = cacheParent;
            base.m_actionCreate = new Func<GameObject>(this.CreateNew);
            base.m_ActionOnGet = new Action<GameObject>(this.OnGet);
            base.m_ActionOnRelease = new Action<GameObject>(this.OnRelease);
        }

        public virtual GameObject CreateNew() => 
            Object.Instantiate<GameObject>(this.origin);

        public virtual void OnDestroy()
        {
            while (base.m_Stack.Count > 0)
            {
                Object.Destroy(base.m_Stack.Pop());
            }
        }

        public virtual void OnGet(GameObject obj)
        {
            obj.SetActive(true);
            obj.transform.SetParent(null);
        }

        public virtual void OnRelease(GameObject obj)
        {
            obj.SetActive(false);
            obj.transform.SetParent(this.cacheParent);
        }
    }

    protected class ResourcePool : GameObjectPool.PoolCache
    {
        private GameObject res;

        public ResourcePool(Transform cacheParent, string resPath) : base(cacheParent)
        {
            this.res = ResourceManager.Load<GameObject>(resPath);
            base.origin = Object.Instantiate<GameObject>(this.res);
        }

        public override void OnDestroy()
        {
            if (base.origin != null)
            {
                Object.Destroy(base.origin);
                this.res = null;
            }
            base.OnDestroy();
        }
    }
}

