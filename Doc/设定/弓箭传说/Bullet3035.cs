using Dxx.Util;
using System;
using UnityEngine;

public class Bullet3035 : BulletBase
{
    private BulletBombDodge_effect effectctrl;
    private Transform effect;
    private const float textureLengthScale = 3f;
    private const float textureScrollSpeed = 8f;
    private const float width = 13f;
    private int layerMask;

    public void CheckBulletLength()
    {
        float x = MathDxx.Sin(base.bulletAngle);
        float z = MathDxx.Cos(base.bulletAngle);
        Vector3 direction = new Vector3(x, 0f, z);
        RaycastHit[] hitArray = Physics.RaycastAll(base.mTransform.position, direction, 100f, this.layerMask);
        float distance = 100f;
        int index = 0;
        int length = hitArray.Length;
        while (index < length)
        {
            RaycastHit hit = hitArray[index];
            if ((((hit.collider.gameObject.layer == LayerManager.Stone) && !base.m_Data.bThroughWall) || (hit.collider.gameObject.layer == LayerManager.MapOutWall)) && (distance > hit.distance))
            {
                distance = hit.distance;
            }
            index++;
        }
        float num6 = distance / 13f;
        if (this.effect != null)
        {
            this.effect.localScale = new Vector3(num6, 1f, 1f);
        }
        this.effectctrl.SetScale(num6);
        base.boxList[0].center = new Vector3(0f, 0f, distance / 2f);
        base.boxList[0].size = new Vector3(base.boxList[0].size.x, base.boxList[0].size.y, distance);
        this.effect.gameObject.SetActive(true);
    }

    protected override void OnDeInit()
    {
        base.OnDeInit();
        this.effect.gameObject.SetActive(false);
    }

    protected override void OnInit()
    {
        base.OnInit();
        this.effect = base.mBulletModel.Find("effect");
        this.effectctrl = base.mBulletModel.GetComponent<BulletBombDodge_effect>();
        if (base.m_Data.bThroughInsideWall)
        {
            this.layerMask = ((int) 1) << LayerManager.MapOutWall;
        }
        else
        {
            this.layerMask = (((int) 1) << LayerManager.Stone) | (((int) 1) << LayerManager.MapOutWall);
        }
        this.CheckBulletLength();
    }
}

