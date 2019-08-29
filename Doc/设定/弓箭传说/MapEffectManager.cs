using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class MapEffectManager
{
    private int perCount = 15;
    private Dictionary<string, Queue<GameObject>> mEffectList = new Dictionary<string, Queue<GameObject>>();
    private Dictionary<string, GameObject> mCloneList = new Dictionary<string, GameObject>();
    private Dictionary<string, List<GameObject>> mUseList = new Dictionary<string, List<GameObject>>();

    public void Cache(GameObject o)
    {
        this.Cache(o, GameNode.m_PoolMapParent, true);
    }

    public void Cache(GameObject o, Transform parent, bool useremove = true)
    {
        if (o != null)
        {
            string name = o.name;
            o.transform.SetParent(parent);
            o.SetActive(false);
            if (useremove)
            {
                this.UseRemove(o);
            }
            if (this.mEffectList.TryGetValue(name, out Queue<GameObject> queue))
            {
                if (queue.Count < this.perCount)
                {
                    queue.Enqueue(o);
                }
                else
                {
                    Object.Destroy(o);
                }
            }
            else
            {
                this.mEffectList.Add(name, new Queue<GameObject>());
                this.mEffectList[name].Enqueue(o);
            }
        }
    }

    public bool check_is_map_effect(GameObject o)
    {
        if (o == null)
        {
            return false;
        }
        return (this.mUseList.TryGetValue(o.name, out List<GameObject> list) && list.Contains(o));
    }

    public void Clear()
    {
        this.MapCache();
    }

    public GameObject Get(string key) => 
        this.Get(key, GameNode.m_PoolMapParent);

    public GameObject Get(string key, Transform parent)
    {
        GameObject obj2;
        if (this.mEffectList.TryGetValue(key, out Queue<GameObject> queue))
        {
            while (queue.Count > 0)
            {
                obj2 = queue.Dequeue();
                if (obj2 != null)
                {
                    obj2.SetActive(true);
                    this.UseSet(key, obj2);
                    return obj2;
                }
            }
        }
        GameObject clone = this.GetClone(key);
        if (clone != null)
        {
            clone.SetActive(true);
            obj2 = Object.Instantiate<GameObject>(clone);
            obj2.name = key;
            obj2.transform.SetParent(parent);
            obj2.name = key;
            this.UseSet(key, obj2);
            clone.SetActive(false);
            return obj2;
        }
        return null;
    }

    private GameObject GetClone(string key)
    {
        if (!this.mCloneList.TryGetValue(key, out GameObject obj2))
        {
            GameObject original = ResourceManager.Load<GameObject>(key);
            if (original != null)
            {
                obj2 = Object.Instantiate<GameObject>(original);
                obj2.name = key;
                obj2.transform.SetParent(GameNode.m_PoolMapParent);
                obj2.transform.position = new Vector3(10000f, 0f, 0f);
                obj2.name = key;
                this.mCloneList.Add(key, obj2);
            }
        }
        return obj2;
    }

    public void MapCache()
    {
        Dictionary<string, List<GameObject>>.Enumerator enumerator = this.mUseList.GetEnumerator();
        while (enumerator.MoveNext())
        {
            KeyValuePair<string, List<GameObject>> current = enumerator.Current;
            List<GameObject> list = current.Value;
            int num = 0;
            int count = list.Count;
            while (num < count)
            {
                GameObject o = list[num];
                if (o != null)
                {
                    this.Cache(o, GameNode.m_PoolMapParent, false);
                }
                num++;
            }
        }
    }

    public void Release()
    {
        this.MapCache();
        Dictionary<string, Queue<GameObject>>.Enumerator enumerator = this.mEffectList.GetEnumerator();
        while (enumerator.MoveNext())
        {
            KeyValuePair<string, Queue<GameObject>> current = enumerator.Current;
            Queue<GameObject> queue = current.Value;
            while (queue.Count > 0)
            {
                GameObject obj2 = queue.Dequeue();
                if (obj2 != null)
                {
                    Object.Destroy(obj2);
                }
            }
        }
        GameNode.m_PoolMapParent.DestroyChildren();
        this.mUseList.Clear();
        this.mEffectList.Clear();
        this.mCloneList.Clear();
    }

    private void UseRemove(GameObject o)
    {
        string name = o.name;
        if (this.mUseList.TryGetValue(name, out List<GameObject> list) && list.Contains(o))
        {
            list.Remove(o);
        }
    }

    private void UseSet(string key, GameObject o)
    {
        string name = o.name;
        if (this.mUseList.TryGetValue(key, out List<GameObject> list))
        {
            list.Add(o);
        }
        else
        {
            this.mUseList.Add(key, new List<GameObject>());
            this.mUseList[key].Add(o);
        }
    }
}

