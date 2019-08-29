using System;
using UnityEngine;

public class Bullet1042 : Bullet1031
{
    private void OnBulletCaches()
    {
        Vector3 pos = base.mTransform.position + (base.moveDirection * base.centerz);
        GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 0xbbf, pos, base.bulletAngle + 90f);
        GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 0xbbf, pos, base.bulletAngle - 90f);
    }

    protected override void OnInit()
    {
        base.OnInit();
        base.OnBulletCache = new Action(this.OnBulletCaches);
    }
}

