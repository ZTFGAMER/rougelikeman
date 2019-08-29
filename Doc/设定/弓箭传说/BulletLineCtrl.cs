using System;
using UnityEngine;

public class BulletLineCtrl : MonoBehaviour
{
    private GameObject mBulletLineStart;
    private LineRenderer mBulletLining;
    private GameObject mBulletLineEnd;
    private ParticleSystem[] mStartP;
    private ParticleSystem[] mEndP;
    private BulletBase mBullet;
    private BulletBase mLastBullet;
    private bool bStart;
    private bool bOverDistance;
    public Action mOverDistanceEvent;
    private const float textureLengthScale = 3f;
    private const float textureScrollSpeed = 8f;

    private void Awake()
    {
        this.mBulletLineStart = base.transform.Find("BulletLineStart").gameObject;
        this.mBulletLining = base.transform.Find("BulletLining").GetComponent<LineRenderer>();
        this.mBulletLineEnd = base.transform.Find("BulletLineEnd").gameObject;
        this.mStartP = this.mBulletLineStart.GetComponentsInChildren<ParticleSystem>(true);
        this.mEndP = this.mBulletLineEnd.GetComponentsInChildren<ParticleSystem>(true);
    }

    public void Cache()
    {
        if (this.bStart)
        {
            this.bStart = false;
            this.bOverDistance = false;
            if (this.mOverDistanceEvent != null)
            {
                this.mOverDistanceEvent();
            }
            this.mBulletLining.positionCount = 0;
            GameLogic.EffectCache(base.gameObject);
        }
    }

    private bool CheckOverDistance()
    {
        Debugger.Log(string.Concat(new object[] { "CheckOverDistance ", this.mBullet.transform.position, " last ", this.mLastBullet.transform.position }));
        Vector3 vector = this.mBullet.transform.position - this.mLastBullet.transform.position;
        this.bOverDistance = vector.magnitude > 1000f;
        return this.bOverDistance;
    }

    public void Init(BulletBase bullet, BulletBase lastbullet)
    {
        this.mBullet = bullet;
        this.mLastBullet = lastbullet;
        this.bOverDistance = false;
        this.bStart = true;
        base.transform.position = Vector3.zero;
    }

    public bool IsOverDistance() => 
        this.bOverDistance;

    private void ParticleClear()
    {
        int index = 0;
        int length = this.mStartP.Length;
        while (index < length)
        {
            this.mStartP[index].Clear();
            index++;
        }
        int num3 = 0;
        int num4 = this.mEndP.Length;
        while (num3 < num4)
        {
            this.mEndP[num3].Clear();
            num3++;
        }
    }

    private void Update()
    {
        if (this.bStart)
        {
            if ((((this.mBullet != null) && this.mBullet.GetInit()) && ((this.mLastBullet != null) && this.mLastBullet.GetInit())) && !this.CheckOverDistance())
            {
                this.UpdateEffect();
            }
            else
            {
                this.Cache();
            }
        }
    }

    private void UpdateEffect()
    {
        this.mBulletLineStart.transform.position = this.mBullet.transform.position;
        this.mBulletLining.positionCount = 2;
        this.mBulletLining.SetPosition(0, this.mBulletLineStart.transform.position);
        this.mBulletLining.SetPosition(1, this.mLastBullet.transform.position);
        this.mBulletLineEnd.transform.position = this.mLastBullet.transform.position;
        this.mBulletLineStart.transform.LookAt(this.mBulletLineEnd.transform.position);
        this.mBulletLineEnd.transform.LookAt(this.mBulletLineStart.transform.position);
        float num = Vector3.Distance(this.mBulletLineStart.transform.position, this.mLastBullet.transform.position);
        this.mBulletLining.material.mainTextureScale = new Vector2(num / 3f, 1f);
        Material material = this.mBulletLining.material;
        material.mainTextureOffset -= new Vector2(Time.deltaTime * 8f, 0f);
    }
}

