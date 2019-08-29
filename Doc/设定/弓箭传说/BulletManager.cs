using DG.Tweening;
using Dxx.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager
{
    public Transform parent;
    private Dictionary<int, Queue<GameObject>> mBulletList = new Dictionary<int, Queue<GameObject>>();
    private Dictionary<int, GameObject> mCloneList = new Dictionary<int, GameObject>();
    private Action OnCache;
    private Dictionary<int, float> mTimeList = new Dictionary<int, float>();
    private Dictionary<int, int> mBulletCounts;
    private int mType;
    private const int perCount = 0x63;
    private int stCount;
    private const int RemoveTime = 180;
    private float checkTime;
    private int stCountTemp;
    private Dictionary<int, Queue<GameObject>>.Enumerator iter;
    private Queue<GameObject> iterq;
    private GameObject iterobj;
    private float itertime;
    private int iterkey;
    private int iterindex;
    private int itermax;
    private int itertimecount;
    private Sequence seq_update;

    public BulletManager(int type)
    {
        Dictionary<int, int> dictionary = new Dictionary<int, int> {
            { 
                0x13e4,
                20
            }
        };
        this.mBulletCounts = dictionary;
        this.stCount = 5;
        this.mType = type;
        if (this.mType == 1)
        {
            this.stCount = 15;
        }
        this.KillSequence();
        this.seq_update = TweenSettingsExtensions.SetLoops<Sequence>(TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(DOTween.Sequence(), 1.3f), new TweenCallback(this, this.OnUpdate)), -1);
    }

    public void Cache(int bulletID, GameObject o)
    {
        o.SetActive(false);
        o.transform.SetParent(this.parent);
        if (this.mBulletList.TryGetValue(bulletID, out Queue<GameObject> queue))
        {
            if (queue.Contains(o))
            {
                return;
            }
            if (queue.Count >= 0x63)
            {
                this.OnCache = (Action) Delegate.Remove(this.OnCache, new Action(o.GetComponent<BulletBase>().BulletCache));
                Object.Destroy(o);
                return;
            }
            queue.Enqueue(o);
        }
        else
        {
            queue = new Queue<GameObject>();
            this.mBulletList.Add(bulletID, queue);
            queue.Enqueue(o);
        }
        this.OnCache = (Action) Delegate.Remove(this.OnCache, new Action(o.GetComponent<BulletBase>().BulletCache));
    }

    public void CacheAll()
    {
        if (this.OnCache != null)
        {
            this.OnCache();
        }
    }

    public BulletBase CreateBullet(EntityBase entity, int BulletID)
    {
        BulletBase component = GameLogic.BulletGet(BulletID).transform.GetComponent<BulletBase>();
        component.Init(entity, BulletID);
        return component;
    }

    public BulletBase CreateBullet(EntityBase entity, int BulletID, Vector3 pos, float rota) => 
        this.CreateBulletInternal(entity, BulletID, pos, rota, true);

    public BulletBase CreateBulletInternal(EntityBase entity, int BulletID, Vector3 pos, float rota, bool clear)
    {
        Transform transform = GameLogic.BulletGet(BulletID).transform;
        transform.localRotation = Quaternion.Euler(0f, rota, 0f);
        transform.SetParent(GameNode.m_PoolParent);
        transform.position = pos;
        transform.localScale = Vector3.one;
        BulletBase component = transform.GetComponent<BulletBase>();
        component.Init(entity, BulletID);
        BulletTransmit bullet = new BulletTransmit(entity, BulletID, clear);
        component.SetBulletAttribute(bullet);
        return component;
    }

    public BulletBase CreateCallBullet(EntityBase entity, int BulletID, int callid, Vector3 startpos, Vector3 endpos)
    {
        Transform transform = GameLogic.BulletGet(BulletID).transform;
        float y = Utils.getAngle(endpos - startpos);
        transform.localRotation = Quaternion.Euler(0f, y, 0f);
        transform.SetParent(GameNode.m_PoolParent);
        transform.position = startpos;
        transform.localScale = Vector3.one;
        BulletCallBase component = transform.GetComponent<BulletCallBase>();
        component.Init(entity, BulletID);
        BulletTransmit bullet = new BulletTransmit(entity, BulletID, true);
        component.SetBulletAttribute(bullet);
        component.SetTarget(null, 1);
        component.SetEndPos(endpos);
        return component;
    }

    public BulletSlopeBase CreateSlopeBullet(EntityBase entity, int BulletID, Vector3 startpos, Vector3 endpos)
    {
        BulletSlopeBase base3 = this.CreateBullet(entity, BulletID, startpos, 0f) as BulletSlopeBase;
        base3.SetEndPos(endpos);
        return base3;
    }

    public GameObject Get(int bulletID)
    {
        if (this.mBulletList.TryGetValue(bulletID, out Queue<GameObject> queue))
        {
            while (queue.Count > 0)
            {
                GameObject obj2 = queue.Dequeue();
                if (obj2 != null)
                {
                    this.SetLastUseTime(bulletID);
                    this.OnCache = (Action) Delegate.Combine(this.OnCache, new Action(obj2.GetComponent<BulletBase>().BulletCache));
                    obj2.SetActive(true);
                    return obj2;
                }
            }
        }
        this.SetLastUseTime(bulletID);
        GameObject obj4 = Object.Instantiate<GameObject>(this.GetClone(bulletID));
        this.OnCache = (Action) Delegate.Combine(this.OnCache, new Action(obj4.GetComponent<BulletBase>().BulletCache));
        return obj4;
    }

    private GameObject GetClone(int key)
    {
        if (this.mCloneList.TryGetValue(key, out GameObject obj2))
        {
            return obj2;
        }
        object[] args = new object[] { "Game/Bullet/Bullet", key };
        GameObject obj3 = ResourceManager.Load<GameObject>(Utils.GetString(args));
        this.mCloneList.Add(key, obj3);
        return obj3;
    }

    private float GetLastUseTime(int key)
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
        if ((Time.time - this.checkTime) > 3f)
        {
            this.checkTime = Time.time;
            this.iter = this.mBulletList.GetEnumerator();
            while (this.iter.MoveNext())
            {
                this.iterkey = this.iter.Current.Key;
                if (!this.mBulletCounts.TryGetValue(this.iterkey, out this.stCountTemp))
                {
                    this.stCountTemp = this.stCount;
                }
                this.itertime = this.GetLastUseTime(this.iterkey);
                this.iterq = this.iter.Current.Value;
                if ((this.iterq != null) && ((Time.time - this.itertime) > 180f))
                {
                    this.RemoveMoreBullets(this.iterq, this.stCountTemp);
                    if ((Time.time - this.itertime) > 540f)
                    {
                        this.itertimecount = (int) (((Time.time - this.itertime) - 540f) / 180f);
                        this.itertimecount = this.stCountTemp - this.itertimecount;
                        this.itertimecount = MathDxx.Clamp(this.itertimecount, 0, this.itertimecount);
                        this.RemoveMoreBullets(this.iterq, this.itertimecount);
                    }
                }
            }
        }
    }

    public void Release()
    {
        this.KillSequence();
        this.CacheAll();
        this.OnCache = null;
        Dictionary<int, Queue<GameObject>>.Enumerator enumerator = this.mBulletList.GetEnumerator();
        while (enumerator.MoveNext())
        {
            KeyValuePair<int, Queue<GameObject>> current = enumerator.Current;
            Queue<GameObject> queue = current.Value;
            while (queue.Count > 0)
            {
                Object.Destroy(queue.Dequeue());
            }
        }
        this.parent.DestroyChildren();
        this.mCloneList.Clear();
        this.mBulletList.Clear();
    }

    private void RemoveMoreBullets(Queue<GameObject> q, int holdcount)
    {
        while (q.Count > holdcount)
        {
            this.iterobj = q.Dequeue();
            if (this.iterobj != null)
            {
                BulletBase component = this.iterobj.GetComponent<BulletBase>();
                this.OnCache = (Action) Delegate.Remove(this.OnCache, new Action(component.BulletCache));
                Object.Destroy(this.iterobj);
            }
        }
    }

    private void SetLastUseTime(int key)
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

