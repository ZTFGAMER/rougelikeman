using Dxx.Util;
using System;
using System.Collections.Generic;

public class ActionBattle
{
    protected List<ActionBasic.ActionBase> actionList = new List<ActionBasic.ActionBase>();
    protected int actionCount;
    protected int actionIndex;
    private EntityBase m_Entity;
    private ActionBasic.ActionBase update_action;

    public void ActionClear()
    {
        this.actionList.Clear();
        this.actionCount = 0;
        this.actionIndex = 0;
        this.OnActionClear();
    }

    public void AddAction(ActionBasic.ActionBase action)
    {
        this.actionList.Add(action);
        this.actionCount = this.actionList.Count;
    }

    public void AddActionDelegate(Action a)
    {
        ActionBasic.ActionDelegate action = new ActionBasic.ActionDelegate {
            action = a
        };
        this.AddAction(action);
    }

    public void AddActionWait(float waitTime)
    {
        ActionWait action = new ActionWait {
            m_Entity = this.m_Entity,
            waitTime = waitTime
        };
        this.AddAction(action);
    }

    public void AddActionWaitDelegate(float waitTime, Action a)
    {
        this.AddActionWait(waitTime);
        this.AddActionDelegate(a);
    }

    public void DeInit()
    {
        this.ActionClear();
        Updater.RemoveUpdate("ActionBattle", new Action<float>(this.OnUpdate));
        this.OnDeInit();
    }

    public int GetActionCount() => 
        this.actionCount;

    public void Init(EntityBase entity)
    {
        this.m_Entity = entity;
        Updater.AddUpdate("ActionBattle", new Action<float>(this.OnUpdate), false);
        this.OnInits();
    }

    protected virtual void OnActionClear()
    {
    }

    protected virtual void OnDeInit()
    {
    }

    protected virtual void OnInits()
    {
    }

    protected virtual void OnUpdate(float delta)
    {
        if (this.actionList.Count > 0)
        {
            this.update_action = this.actionList[0];
            this.update_action.Init();
            this.update_action.Update();
            if (this.update_action.IsEnd)
            {
                if (this.actionList.Count > 0)
                {
                    this.actionList.RemoveAt(0);
                }
                this.actionCount = this.actionList.Count;
            }
        }
    }

    public class ActionBase : ActionBasic.ActionUIBase
    {
        protected override void OnInit()
        {
        }
    }

    public class ActionWait : ActionBattle.ActionBase
    {
        private float startTime;
        public float waitTime;
        public bool ignoreTime;

        protected override void OnInit()
        {
            this.startTime = base.m_Entity.AliveTime;
        }

        protected override void OnUpdate()
        {
            if ((base.m_Entity.AliveTime - this.startTime) >= this.waitTime)
            {
                base.End();
            }
        }
    }
}

