using Dxx.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Weapon5113 : WeaponBase
{
    protected override void OnAttack(object[] args)
    {
        int count = 7;
        List<Vector3> list = GameLogic.Release.MapCreatorCtrl.RandomOutPositions(count);
        for (int i = 0; (i < count) && (i < list.Count); i++)
        {
            Vector3 vector = list[i];
            float rota = Utils.getAngle(base.m_Entity.position - vector);
            GameLogic.Release.Bullet.CreateBullet(base.m_Entity, base.BulletID, vector + new Vector3(0f, 1f, 0f), rota);
        }
    }

    protected override void OnInstall()
    {
    }

    protected override void OnUnInstall()
    {
    }
}

