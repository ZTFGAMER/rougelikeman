using System;
using UnityEngine;

public class AIMove1033 : AIMoveBase
{
    private EntityBase target;
    private int range;
    private float maxdis;
    private bool move2target;
    private bool bExcuteShow;
    private Vector3 endpos;
    private ActionBattle action;
    private ConditionTime mCondition;

    public AIMove1033(EntityBase entity, float maxdis, int range, bool move2target) : base(entity)
    {
        this.range = range;
        this.maxdis = maxdis;
        this.move2target = move2target;
        this.target = GameLogic.Self;
    }

    private void KillAction()
    {
        if (this.action != null)
        {
            this.action.DeInit();
            this.action = null;
        }
    }

    protected override void OnEnd()
    {
        if (this.bExcuteShow)
        {
            this.Show(true);
        }
        this.KillAction();
    }

    protected override void OnInitBase()
    {
        this.bExcuteShow = false;
        Vector3 vector = this.target.position - base.m_Entity.position;
        if (vector.magnitude < this.maxdis)
        {
            base.End();
        }
        else
        {
            base.m_Entity.m_AniCtrl.SetString("Skill", "MoveMiss");
            base.m_Entity.m_AniCtrl.SendEvent("Skill", false);
            this.KillAction();
            this.action = new ActionBattle();
            this.action.Init(base.m_Entity);
            this.action.AddActionWaitDelegate(0.5f, () => base.m_Entity.PlayEffect(0x2f4d72));
            this.action.AddActionWaitDelegate(0.4f, () => this.Show(false));
            this.action.AddActionWaitDelegate(0.6f, delegate {
                float endx = 0f;
                float endz = 0f;
                if (this.move2target)
                {
                    GameLogic.Release.MapCreatorCtrl.RandomItemSide(GameLogic.Self, this.range, out endx, out endz);
                }
                else
                {
                    Vector3 vector = GameLogic.Release.MapCreatorCtrl.RandomPosition();
                    endx = vector.x;
                    endz = vector.z;
                }
                this.endpos = new Vector3(endx, 0f, endz);
            });
            this.action.AddActionWaitDelegate(0.4f, delegate {
                base.m_Entity.SetPosition(this.endpos);
                this.Show(true);
                base.m_Entity.m_AniCtrl.SetString("Skill", "MoveShow");
                base.m_Entity.m_AniCtrl.SendEvent("Skill", false);
                base.m_Entity.PlayEffect(0x2f4d72);
            });
            ConditionTime time = new ConditionTime {
                time = 2.5f
            };
            this.mCondition = time;
        }
    }

    protected override void OnUpdate()
    {
        if ((this.mCondition != null) && this.mCondition.IsEnd())
        {
            this.mCondition = null;
            base.End();
        }
    }

    private void Show(bool show)
    {
        this.bExcuteShow = !show;
        base.m_Entity.ShowEntity(show);
    }
}

