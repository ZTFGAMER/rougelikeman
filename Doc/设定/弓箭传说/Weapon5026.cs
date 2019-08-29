using System;
using UnityEngine;

public class Weapon5026 : WeaponBase
{
    protected override void OnAttack(object[] args)
    {
        int num = 5;
        for (int i = 0; i < num; i++)
        {
            ActionBasic.ActionDelegate action = new ActionBasic.ActionDelegate {
                action = delegate {
                    GameLogic.Hold.Sound.PlayBulletCreate(0x1e903c, base.m_Entity.position);
                    for (int j = 0; j < 8; j++)
                    {
                        base.CreateBulletOverride(Vector3.zero, (j * 45f) + GameLogic.Random((float) -30f, (float) 30f));
                    }
                }
            };
            base.action.AddAction(action);
            ActionBasic.ActionWait wait = new ActionBasic.ActionWait {
                waitTime = 0.15f
            };
            base.action.AddAction(wait);
        }
    }

    protected override void OnInstall()
    {
    }

    protected override void OnUnInstall()
    {
    }
}

