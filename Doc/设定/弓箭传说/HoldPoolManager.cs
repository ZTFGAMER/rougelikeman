using System;
using System.Collections.Generic;
using UnityEngine;

public class HoldPoolManager
{
    private Dictionary<string, Queue<GameObject>> mEffectList = new Dictionary<string, Queue<GameObject>>();

    public void Cache(GameObject o)
    {
        this.Cache(o, GameNode.SoundNode);
    }

    public void Cache(GameObject o, Transform parent)
    {
        if (o != null)
        {
            string name = o.name;
            o.transform.SetParent(parent);
            o.SetActive(false);
            if (this.mEffectList.TryGetValue(name, out Queue<GameObject> queue))
            {
                queue.Enqueue(o);
            }
            else
            {
                this.mEffectList.Add(name, new Queue<GameObject>());
                this.mEffectList[name].Enqueue(o);
            }
        }
    }

    public GameObject Get(string key) => 
        this.Get(key, GameNode.SoundNode);

    public GameObject Get(string key, Transform parent)
    {
        GameObject obj2;
        if (this.mEffectList.TryGetValue(key, out Queue<GameObject> queue) && (queue.Count > 0))
        {
            obj2 = queue.Dequeue();
            if (obj2 != null)
            {
                obj2.SetActive(true);
                return obj2;
            }
        }
        obj2 = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>(key));
        obj2.transform.SetParent(parent);
        obj2.name = key;
        return obj2;
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

