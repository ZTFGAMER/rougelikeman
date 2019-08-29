using System;
using UnityEngine;

public class Weapon5005 : WeaponBase
{
    protected float posoffset = 1f;
    protected int bulletcount = 20;

    protected override void OnAttack(object[] args)
    {
        float max = 90f;
        GameLogic.Hold.Sound.PlayBulletCreate(0x1e903c, base.m_Entity.position);
        for (int i = 0; i < this.bulletcount; i++)
        {
            base.CreateBulletOverride(new Vector3(GameLogic.Random(-this.posoffset, this.posoffset), 0f, GameLogic.Random(-this.posoffset, this.posoffset)), GameLogic.Random(-max, max));
        }
    }

    protected override void OnInstall()
    {
    }

    protected override void OnUnInstall()
    {
    }
}

