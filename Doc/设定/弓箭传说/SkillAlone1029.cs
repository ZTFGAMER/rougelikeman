using Dxx.Util;
using System;
using TableTool;
using UnityEngine;

public class SkillAlone1029 : SkillAloneBase
{
    private Weapon_weapon weapondata;
    private float percent;

    protected override void OnInstall()
    {
        this.percent = float.Parse(base.m_SkillData.Args[0]);
        base.m_Entity.OnKillAction = (Action<EntityBase, Vector3>) Delegate.Combine(base.m_Entity.OnKillAction, new Action<EntityBase, Vector3>(this.OnKillAction));
        this.weapondata = LocalModelManager.Instance.Weapon_weapon.GetBeanById(0xbb9);
    }

    private void OnKillAction(EntityBase entity, Vector3 HittedDirection)
    {
        float num = Utils.getAngle(HittedDirection);
        for (int i = 0; i < 8; i++)
        {
            Transform transform = GameLogic.BulletGet(this.weapondata.WeaponID).transform;
            transform.SetParent(GameNode.m_PoolParent);
            transform.position = new Vector3(entity.position.x, 1f, entity.position.z);
            transform.localRotation = Quaternion.Euler(0f, num + (i * 45f), 0f);
            transform.localScale = Vector3.one;
            transform.GetComponent<BulletBase>().Init(base.m_Entity, this.weapondata.WeaponID);
            BulletBase component = transform.GetComponent<BulletBase>();
            component.AddCantHit(entity);
            component.SetBulletAttribute(new BulletTransmit(base.m_Entity, this.weapondata.WeaponID, true));
            component.mBulletTransmit.AddAttackRatio(this.percent);
        }
    }

    protected override void OnUninstall()
    {
        base.m_Entity.OnKillAction = (Action<EntityBase, Vector3>) Delegate.Remove(base.m_Entity.OnKillAction, new Action<EntityBase, Vector3>(this.OnKillAction));
    }
}

