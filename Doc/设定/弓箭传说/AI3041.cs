using Dxx.Util;
using System;
using System.Collections.Generic;
using TableTool;
using UnityEngine;

public class AI3041 : AIBase
{
    private List<BulletRedLineCtrl> mLines = new List<BulletRedLineCtrl>();
    private ActionBattle action = new ActionBattle();
    private int bulletid;
    private int attack;
    private float startangle;
    private static float[] angles = new float[] { 40f, 140f, 220f, 320f };

    private void ClearLines()
    {
        for (int i = 0; i < this.mLines.Count; i++)
        {
            BulletRedLineCtrl ctrl = this.mLines[i];
            if (ctrl != null)
            {
                Object.Destroy(ctrl.gameObject);
            }
        }
        this.mLines.Clear();
    }

    private void CreateBullets()
    {
        for (int i = 0; i < 4; i++)
        {
            float rota = 0f;
            if (this.startangle == 0f)
            {
                rota = (i * 90f) + this.startangle;
            }
            else
            {
                rota = angles[i];
            }
            GameLogic.Release.Bullet.CreateBullet(base.m_Entity, this.bulletid, base.m_Entity.m_Body.LeftBullet.transform.position, rota);
        }
    }

    protected override void OnAIDeInit()
    {
        this.action.DeInit();
        this.ClearLines();
    }

    protected override void OnInit()
    {
        this.bulletid = base.m_Entity.m_Data.WeaponID;
        this.attack = LocalModelManager.Instance.Weapon_weapon.GetBeanById(this.bulletid).Attack;
        this.action.Init(base.m_Entity);
        base.AddAction(base.GetActionWait(string.Empty, 0x7d0));
        base.AddAction(base.GetActionDelegate(delegate {
            this.startangle = !MathDxx.RandomBool() ? 45f : 0f;
            this.showlines(true);
        }));
        base.AddAction(base.GetActionWait(string.Empty, 0x3e8));
        base.AddAction(base.GetActionDelegate(delegate {
            this.showlines(false);
            this.CreateBullets();
        }));
        base.AddAction(base.GetActionWait(string.Empty, 0x7d0));
    }

    private void showlines(bool show)
    {
        if (this.mLines.Count == 0)
        {
            for (int i = 0; i < 4; i++)
            {
                GameObject child = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>("Game/Bullet/Bullet1102_RedLine"));
                child.SetParentNormal(base.m_Entity.m_Body.LeftBullet.transform);
                BulletRedLineCtrl component = child.GetComponent<BulletRedLineCtrl>();
                this.mLines.Add(component);
            }
        }
        if (show)
        {
            int index = 0;
            int count = this.mLines.Count;
            while (index < count)
            {
                BulletRedLineCtrl ctrl2 = this.mLines[index];
                ctrl2.gameObject.SetActive(true);
                if (this.startangle == 0f)
                {
                    ctrl2.transform.rotation = Quaternion.Euler(0f, (index * 90f) + this.startangle, 0f);
                }
                else
                {
                    ctrl2.transform.rotation = Quaternion.Euler(0f, angles[index], 0f);
                }
                ctrl2.UpdateLine(false, 0.5f);
                ctrl2.PlayLineWidth();
                index++;
            }
        }
        else
        {
            int num4 = 0;
            int count = this.mLines.Count;
            while (num4 < count)
            {
                this.mLines[num4].gameObject.SetActive(false);
                num4++;
            }
        }
    }
}

