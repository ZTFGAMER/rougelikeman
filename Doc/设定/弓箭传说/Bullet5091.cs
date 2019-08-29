using System;
using System.Collections.Generic;
using UnityEngine;

public class Bullet5091 : BulletBase
{
    private GameObject lines;
    private float angle;
    private float createdis;
    private float perdis = 7f;
    private bool bCreateRedLine;
    private List<BulletBase> bullets = new List<BulletBase>();

    private void CreateBullets()
    {
        for (int i = 0; i < 4; i++)
        {
            BulletBase item = GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 0x13e4, Vector3.zero, 0f);
            item.transform.SetParentNormal(base.mTransform);
            item.transform.rotation = Quaternion.Euler(0f, (float) (i * 90), 0f);
            this.bullets.Add(item);
        }
    }

    private void DeInitBullets()
    {
        int num = 0;
        int count = this.bullets.Count;
        while (num < count)
        {
            this.bullets[num].DeInit();
            GameLogic.BulletCache(this.bullets[num].m_Data.WeaponID, this.bullets[num].gameObject);
            num++;
        }
        this.bullets.Clear();
    }

    private void LineShow(bool value)
    {
        if (this.lines != null)
        {
            this.lines.SetActive(value);
        }
    }

    protected override void OnDeInit()
    {
        this.DeInitBullets();
        base.OnDeInit();
    }

    protected override void OnInit()
    {
        if (this.lines == null)
        {
            Transform transform = base.mTransform.Find("lines");
            if (transform != null)
            {
                this.lines = transform.gameObject;
            }
        }
        if (this.lines != null)
        {
            this.lines.transform.rotation = Quaternion.identity;
        }
        base.bFlyRotate = false;
        base.OnInit();
        this.createdis = this.perdis;
        this.DeInitBullets();
        this.bCreateRedLine = false;
        this.LineShow(false);
    }

    protected override void UpdateProcess()
    {
        base.UpdateProcess();
        if ((base.CurrentDistance > (this.createdis - 3.5f)) && !this.bCreateRedLine)
        {
            this.bCreateRedLine = true;
            this.LineShow(true);
        }
        if (base.CurrentDistance > this.createdis)
        {
            this.CreateBullets();
            this.createdis += this.perdis;
            this.bCreateRedLine = false;
            this.LineShow(false);
        }
    }
}

