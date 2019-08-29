using System;
using UnityEngine;

public class Weapon1047 : WeaponBase
{
    private float startoffset = 0.5f;
    private float rotateoffset = 15f;

    protected override void OnAttack(object[] args)
    {
        int num = 3;
        for (int i = 0; i < num; i++)
        {
            base.CreateBulletOverride(new Vector3(GameLogic.Random(-this.startoffset, this.startoffset), 0f, GameLogic.Random(-this.startoffset, this.startoffset)), GameLogic.Random(-this.rotateoffset, this.rotateoffset));
        }
    }
}

