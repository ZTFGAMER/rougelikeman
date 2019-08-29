using System;
using UnityEngine;

public class Weapon5111 : WeaponBase
{
    protected override void OnAttack(object[] args)
    {
        int num = 10;
        for (int i = 0; i < num; i++)
        {
            Vector3 vector = GameLogic.Release.MapCreatorCtrl.RandomPosition();
            GameLogic.Release.Bullet.CreateBullet(base.m_Entity, base.BulletID, vector + new Vector3(0f, 1f, 0f), 0f);
        }
    }

    protected override void OnInstall()
    {
    }

    protected override void OnUnInstall()
    {
    }
}

