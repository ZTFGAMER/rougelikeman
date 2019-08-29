using Dxx.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GoodsCreateManager
{
    private Dictionary<int, Queue<GameObject>> mList = new Dictionary<int, Queue<GameObject>>();
    private List<GameObject> mUseList = new List<GameObject>();

    public void Cache(GameObject o)
    {
        o.SetActive(false);
        int key = int.Parse(o.name);
        o.transform.SetParent(GameNode.m_BulletParent);
        this.UseRemove(o);
        if (this.mList.TryGetValue(key, out Queue<GameObject> queue))
        {
            queue.Enqueue(o);
        }
        else
        {
            this.mList.Add(key, new Queue<GameObject>());
            this.mList[key].Enqueue(o);
        }
    }

    public GameObject Get(int id)
    {
        if (this.mList.TryGetValue(id, out Queue<GameObject> queue))
        {
            while (queue.Count > 0)
            {
                GameObject obj2 = queue.Dequeue();
                if (obj2 != null)
                {
                    obj2.SetActive(true);
                    this.UseAdd(obj2);
                    return obj2;
                }
            }
        }
        object[] args = new object[] { "Game/Goods/GoodsCreate", id };
        GameObject o = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>(Utils.GetString(args)));
        o.name = id.ToString();
        this.UseAdd(o);
        return o;
    }

    public void Release()
    {
        int num = 0;
        int count = this.mUseList.Count;
        while (num < count)
        {
            GameObject obj2 = this.mUseList[num];
            if (obj2 != null)
            {
                Object.Destroy(obj2);
            }
            num++;
        }
        this.mUseList.Clear();
        this.mList.Clear();
    }

    private void UseAdd(GameObject o)
    {
        this.mUseList.Add(o);
    }

    private void UseRemove(GameObject o)
    {
        this.mUseList.Remove(o);
    }
}

