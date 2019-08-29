using System;
using System.Collections.Generic;
using UnityEngine;

public class LocalUnityObjctPool : MonoBehaviour
{
    protected Dictionary<string, Cache> m_Cache = new Dictionary<string, Cache>();

    public void ClearAllCache()
    {
        foreach (Cache cache in this.m_Cache.Values)
        {
            cache.Destroy();
        }
        this.m_Cache.Clear();
    }

    public void ClearCache<T>() where T: Component
    {
        this.ClearCache(typeof(T).Name);
    }

    public void ClearCache(string cacheName)
    {
        if (this.m_Cache.ContainsKey(cacheName))
        {
            this.m_Cache[cacheName].Destroy();
        }
    }

    public void Collect<T>() where T: Component
    {
        this.Collect(typeof(T).Name);
    }

    public void Collect(string cacheName)
    {
        if (this.m_Cache.ContainsKey(cacheName))
        {
            this.m_Cache[cacheName].Collect();
        }
    }

    public static LocalUnityObjctPool Create(GameObject parent)
    {
        GameObject obj2 = new GameObject("LocalPool");
        obj2.transform.SetParent(parent.transform, false);
        LocalUnityObjctPool pool = obj2.AddComponent<LocalUnityObjctPool>();
        obj2.SetActive(false);
        return pool;
    }

    public void CreateCache<T>(GameObject copyItem) where T: Component
    {
        this.CreateCache<T>(base.gameObject, copyItem);
    }

    public void CreateCache(string cacheName, GameObject copyItem)
    {
        this.CreateCache(cacheName, base.transform, copyItem);
    }

    public void CreateCache<T>(GameObject parent, GameObject copyItem) where T: Component
    {
        this.CreateCache(typeof(T).Name, parent.transform, copyItem);
    }

    public void CreateCache(string cacheName, Transform parent, GameObject copyItem)
    {
        this.m_Cache.Add(cacheName, new Cache(parent, copyItem));
    }

    public T DeQueue<T>() where T: Component => 
        this.DeQueue<T>(typeof(T).Name);

    public T DeQueue<T>(string cacheName) where T: Component => 
        this.m_Cache[cacheName].Dequeue().GetComponent<T>();

    public T DeQueueWithName<T>(string name) where T: Component => 
        this.DeQueueWithName<T>(typeof(T).Name, name);

    public T DeQueueWithName<T>(string cacheName, string name) where T: Component => 
        this.m_Cache[cacheName].Dequeue(name).GetComponent<T>();

    public void EnQueue<T>(GameObject item) where T: Component
    {
        this.EnQueue(typeof(T).Name, item);
    }

    public void EnQueue(string cacheName, GameObject item)
    {
        if (this.m_Cache.ContainsKey(cacheName))
        {
            this.m_Cache[cacheName].EnQueue(item);
        }
        else
        {
            Debugger.Log("enqueue " + cacheName + " dont have");
        }
    }

    protected class Cache
    {
        public GameObject copyItem;
        private List<GameObject> collection = new List<GameObject>();
        private Queue<GameObject> cache = new Queue<GameObject>();
        private Transform rootParent;

        public Cache(Transform rootParent, GameObject copyItem)
        {
            this.rootParent = rootParent;
            this.copyItem = copyItem;
        }

        public void Collect()
        {
            for (int i = this.collection.Count - 1; i >= 0; i--)
            {
                this.EnQueue(this.collection[i]);
            }
            this.collection.Clear();
        }

        public GameObject Dequeue() => 
            this.Dequeue(string.Empty);

        public GameObject Dequeue(string name)
        {
            GameObject obj2;
            if (this.cache.Count > 0)
            {
                obj2 = this.cache.Dequeue();
            }
            else
            {
                obj2 = Object.Instantiate<GameObject>(this.copyItem);
            }
            this.collection.Add(obj2);
            if (!string.IsNullOrEmpty(name))
            {
                obj2.name = name;
            }
            obj2.SetActive(true);
            return obj2;
        }

        public void Destroy()
        {
            this.Collect();
            while (this.cache.Count > 0)
            {
                Object.DestroyImmediate(this.cache.Dequeue());
            }
        }

        public void EnQueue(GameObject item)
        {
            item.gameObject.SetActive(false);
            item.transform.SetParent(this.rootParent.transform);
            this.cache.Enqueue(item);
            if (this.collection.Contains(item))
            {
                this.collection.Remove(item);
            }
        }
    }
}

