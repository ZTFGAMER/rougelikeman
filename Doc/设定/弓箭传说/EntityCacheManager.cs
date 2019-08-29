using System;
using System.Collections.Generic;
using UnityEngine;

public class EntityCacheManager
{
    private Dictionary<string, Queue<GameObject>> mEffectList = new Dictionary<string, Queue<GameObject>>();

    public void Cache(GameObject o, int maxcount)
    {
        if (o != null)
        {
            string name = o.name;
            o.transform.SetParent(GameNode.m_PoolParent);
            o.SetActive(false);
            if (this.mEffectList.TryGetValue(name, out Queue<GameObject> queue))
            {
                if (queue.Count < maxcount)
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

    public GameObject Get(string key)
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
                    obj2.transform.position = new Vector3(10000f, 0f, 0f);
                    return obj2;
                }
            }
        }
        GameObject original = ResourceManager.Load<GameObject>(key);
        if (original != null)
        {
            obj2 = Object.Instantiate<GameObject>(original);
            obj2.name = key;
            return obj2;
        }
        return null;
    }

    public void Release()
    {
        Dictionary<string, Queue<GameObject>>.Enumerator enumerator = this.mEffectList.GetEnumerator();
        while (enumerator.MoveNext())
        {
            KeyValuePair<string, Queue<GameObject>> current = enumerator.Current;
            Queue<GameObject> queue = current.Value;
            while (queue.Count > 0)
            {
                Object.Destroy(queue.Dequeue());
            }
        }
        this.mEffectList.Clear();
    }
}

