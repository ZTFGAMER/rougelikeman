using DG.Tweening;
using Dxx.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager
{
    private Dictionary<string, int> mEffectCounts;
    private int perCount;
    private int stCount;
    private const int RemoveTime = 190;
    private const int RemoveMoreTime = 10;
    private float checkTime;
    private Dictionary<string, Queue<GameObject>>.Enumerator iter;
    private Queue<GameObject> iterq;
    private GameObject iterobj;
    private float itertime;
    private string iterkey;
    private int itertimecount;
    private int stCountTemp;
    private Sequence seq_update;
    private Dictionary<string, Queue<GameObject>> mEffectList;
    private Dictionary<string, GameObject> mCloneList;
    private Dictionary<string, float> mTimeList;

    public EffectManager()
    {
        Dictionary<string, int> dictionary = new Dictionary<string, int> {
            { 
                "Game/Food/3001",
                20
            }
        };
        this.mEffectCounts = dictionary;
        this.perCount = 0x3e7;
        this.stCount = 5;
        this.mEffectList = new Dictionary<string, Queue<GameObject>>();
        this.mCloneList = new Dictionary<string, GameObject>();
        this.mTimeList = new Dictionary<string, float>();
        this.KillSequence();
        this.seq_update = TweenSettingsExtensions.SetLoops<Sequence>(TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(DOTween.Sequence(), 1.4f), new TweenCallback(this, this.OnUpdate)), -1);
    }

    public void Cache(GameObject o)
    {
        this.Cache(o, GameNode.m_PoolParent);
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
                if (!queue.Contains(o))
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
            }
            else
            {
                this.mEffectList.Add(name, new Queue<GameObject>());
                this.mEffectList[name].Enqueue(o);
            }
        }
    }

    public GameObject Get(string key) => 
        this.Get(key, GameNode.m_PoolParent);

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
                    this.SetLastUseTime(key);
                    obj2.SetActive(true);
                    obj2.transform.position = new Vector3(10000f, 0f, 0f);
                    return obj2;
                }
            }
        }
        GameObject clone = this.GetClone(key);
        if (clone != null)
        {
            this.SetLastUseTime(key);
            obj2 = Object.Instantiate<GameObject>(clone);
            obj2.name = key;
            obj2.transform.SetParent(parent);
            obj2.transform.position = new Vector3(10000f, 0f, 0f);
            obj2.SetActive(true);
            return obj2;
        }
        return null;
    }

    private GameObject GetClone(string key)
    {
        if (this.mCloneList.TryGetValue(key, out GameObject obj2))
        {
            return obj2;
        }
        GameObject obj3 = ResourceManager.Load<GameObject>(key);
        this.mCloneList.Add(key, obj3);
        return obj3;
    }

    private float GetLastUseTime(string key)
    {
        float num = 0f;
        if (!this.mTimeList.TryGetValue(key, out num))
        {
            this.mTimeList.Add(key, num);
        }
        return num;
    }

    private void KillSequence()
    {
        if (this.seq_update != null)
        {
            TweenExtensions.Kill(this.seq_update, false);
            this.seq_update = null;
        }
    }

    private void OnUpdate()
    {
        if ((Time.time - this.checkTime) > 3.1f)
        {
            this.checkTime = Time.time;
            this.iter = this.mEffectList.GetEnumerator();
            while (this.iter.MoveNext())
            {
                this.iterkey = this.iter.Current.Key;
                this.itertime = this.GetLastUseTime(this.iterkey);
                if (!this.mEffectCounts.TryGetValue(this.iterkey, out this.stCountTemp))
                {
                    this.stCountTemp = this.stCount;
                }
                if (this.mEffectList.TryGetValue(this.iterkey, out this.iterq) && ((Time.time - this.itertime) > 190f))
                {
                    this.RemoveMoreEffects(this.iterq, this.stCountTemp);
                    if ((Time.time - this.itertime) > 200f)
                    {
                        this.itertimecount = (int) ((((Time.time - this.itertime) - 190f) - 10f) / 10f);
                        this.itertimecount = this.stCountTemp - this.itertimecount;
                        this.itertimecount = MathDxx.Clamp(this.itertimecount, 0, this.itertimecount);
                        this.RemoveMoreEffects(this.iterq, this.itertimecount);
                    }
                }
            }
        }
    }

    public void Release()
    {
        this.KillSequence();
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
        GameNode.m_PoolParent.DestroyChildren();
        this.mEffectList.Clear();
        this.mCloneList.Clear();
        this.mTimeList.Clear();
    }

    private void RemoveMoreEffects(Queue<GameObject> iterq, int lastcount)
    {
        while (iterq.Count > lastcount)
        {
            this.iterobj = iterq.Dequeue();
            if (this.iterobj != null)
            {
                Object.Destroy(this.iterobj);
            }
        }
    }

    private void SetLastUseTime(string key)
    {
        if (this.mTimeList.ContainsKey(key))
        {
            this.mTimeList[key] = Time.time;
        }
        else
        {
            this.mTimeList.Add(key, Time.time);
        }
    }
}

