using System;
using System.Collections.Generic;
using UnityEngine;

public class MutiCachePool<T> where T: Component
{
    private LocalUnityObjctPool mPool;
    private T tempitem;
    private List<T> mUsed;
    private List<T> mCached;

    public MutiCachePool()
    {
        this.mUsed = new List<T>();
        this.mCached = new List<T>();
    }

    public void cache(T one)
    {
        this.mCached.Add(one);
        one.gameObject.SetActive(false);
    }

    public void clear()
    {
        Vector3 zero = Vector3.zero;
        int num = 0;
        int count = this.mUsed.Count;
        while (num < count)
        {
            zero.x = GameLogic.Random(-5000, -10000);
            zero.y = GameLogic.Random(-50000, 0xc350);
            T local = this.mUsed[num];
            local.transform.localPosition = zero;
            this.mCached.Add(this.mUsed[num]);
            num++;
        }
        this.mUsed.Clear();
    }

    public T get()
    {
        if (this.mCached.Count > 0)
        {
            this.tempitem = this.mCached[0];
            this.tempitem.gameObject.SetActive(true);
            this.mCached.RemoveAt(0);
        }
        else
        {
            this.tempitem = this.mPool.DeQueue<T>();
        }
        this.mUsed.Add(this.tempitem);
        return this.tempitem;
    }

    public void hold(int count)
    {
        int num = count;
        int num2 = this.mCached.Count;
        while (num < num2)
        {
            T local = this.mCached[num];
            local.gameObject.SetActive(false);
            num++;
        }
    }

    public void Init(GameObject obj, GameObject copyitem)
    {
        this.mPool = LocalUnityObjctPool.Create(obj);
        this.mPool.CreateCache<T>(copyitem);
    }
}

