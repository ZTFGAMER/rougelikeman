using Dxx.Util;
using System;
using UnityEngine;

public class SkillAlone1043 : SkillAloneBase
{
    private GameObject line;
    private FireLineCtrl linectrl;
    private bool bLineShow = true;
    private float attack_time;
    private float attack_delaytime;
    private float find_time = 10f;
    private float find_delaytime = 0.5f;
    private EntityBase target;
    private float hitratio;
    private float range;
    private long clockindex;

    private void CreateSkillAlone()
    {
        this.line = GameLogic.EffectGet("Game/SkillPrefab/SkillAlone1043_One");
        this.linectrl = this.line.GetComponent<FireLineCtrl>();
    }

    private void LineShow(bool show)
    {
        if (this.bLineShow != show)
        {
            this.bLineShow = show;
            this.line.SetActive(show);
            if (show && (this.target != null))
            {
                GameLogic.SendBuff(this.target, base.m_Entity, 0x3ff, Array.Empty<float>());
            }
        }
    }

    private void OnAttack()
    {
        if (this.target != null)
        {
            int num = -((int) (base.m_Entity.m_EntityData.GetAttack(0) * this.hitratio));
            GameLogic.SendHit_Skill(this.target, (long) num);
        }
    }

    protected override void OnInstall()
    {
        this.attack_delaytime = float.Parse(base.m_SkillData.Args[0]);
        this.hitratio = float.Parse(base.m_SkillData.Args[1]);
        this.range = float.Parse(base.m_SkillData.Args[2]);
        this.range = 10f;
        this.CreateSkillAlone();
        Updater.AddUpdate("SkillAlone1043", new Action<float>(this.OnUpdate), false);
    }

    protected override void OnUninstall()
    {
        GameLogic.EffectCache(this.line);
        Updater.RemoveUpdate("SkillAlone1043", new Action<float>(this.OnUpdate));
    }

    private void OnUpdate(float delta)
    {
        this.find_time += delta;
        if (this.find_time >= this.find_delaytime)
        {
            this.find_time -= this.find_delaytime;
            this.target = GameLogic.Release.Entity.GetNearEntity(base.m_Entity, this.range, false);
        }
        this.attack_time += delta;
        if (this.attack_time >= this.attack_delaytime)
        {
            this.attack_time -= this.attack_delaytime;
            this.OnAttack();
        }
        this.LineShow((this.target != null) && !this.target.GetIsDead());
        if (this.bLineShow)
        {
            this.linectrl.UpdateLine(base.m_Entity.m_Body.EffectMask.transform.position, this.target.m_Body.EffectMask.transform.position);
        }
    }
}

