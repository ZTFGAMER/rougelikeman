using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SkillAlone1052 : SkillAloneBase
{
    private ActionBasic action = new ActionBasic();

    private void CreateBullets()
    {
        ActionBasic.ActionParallel action = new ActionBasic.ActionParallel();
        for (int i = 0; i < 8; i++)
        {
            <CreateBullets>c__AnonStorey0 storey = new <CreateBullets>c__AnonStorey0 {
                $this = this,
                index = i
            };
            AIBase.ActionSequence a = new AIBase.ActionSequence();
            ActionBasic.ActionWait wait = new ActionBasic.ActionWait {
                waitTime = GameLogic.Random((float) 0f, (float) 0.5f)
            };
            a.AddAction(wait);
            ActionBasic.ActionDelegate delegate2 = new ActionBasic.ActionDelegate {
                action = new Action(storey.<>m__0)
            };
            a.AddAction(delegate2);
            action.Add(a);
        }
        this.action.AddAction(action);
    }

    private void OnHitted(EntityBase entity, long value)
    {
        this.CreateBullets();
    }

    protected override void OnInstall()
    {
        base.m_Entity.OnHitted = (Action<EntityBase, long>) Delegate.Combine(base.m_Entity.OnHitted, new Action<EntityBase, long>(this.OnHitted));
        this.action.Init(false);
    }

    protected override void OnUninstall()
    {
        base.m_Entity.OnHitted = (Action<EntityBase, long>) Delegate.Remove(base.m_Entity.OnHitted, new Action<EntityBase, long>(this.OnHitted));
        this.action.DeInit();
    }

    [CompilerGenerated]
    private sealed class <CreateBullets>c__AnonStorey0
    {
        internal int index;
        internal SkillAlone1052 $this;

        internal void <>m__0()
        {
            GameLogic.Release.Bullet.CreateBullet(this.$this.m_Entity, 0xbc1, this.$this.m_Entity.position + new Vector3(0f, 1f, 0f), (this.index * 45f) + GameLogic.Random((float) -30f, (float) 30f));
        }
    }
}

