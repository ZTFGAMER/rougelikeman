using Dxx.Util;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ActionBasic
{
    protected List<ActionBase> actionList = new List<ActionBase>();
    protected int actionCount;
    protected int actionIndex;
    private bool mIgnoreTimeScale;
    private ActionBase update_action;

    public void ActionClear()
    {
        this.actionList.Clear();
        this.actionCount = 0;
        this.actionIndex = 0;
        this.OnActionClear();
    }

    public void AddAction(ActionBase action)
    {
        this.actionList.Add(action);
        this.actionCount = this.actionList.Count;
    }

    public void AddActionDelegate(Action a)
    {
        ActionDelegate action = new ActionDelegate {
            action = a
        };
        this.AddAction(action);
    }

    public void AddActionIgnoreWait(float waitTime)
    {
        ActionWaitIgnoreTime action = new ActionWaitIgnoreTime {
            waitTime = waitTime
        };
        this.AddAction(action);
    }

    public void AddActionIgnoreWaitDelegate(float waitTime, Action a)
    {
        this.AddActionIgnoreWait(waitTime);
        this.AddActionDelegate(a);
    }

    public void AddActionWait(float waitTime)
    {
        ActionWait action = new ActionWait {
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
        this.OnDeInit();
        this.ActionClear();
        Updater.RemoveUpdate("ActionBasic", new Action<float>(this.OnUpdate));
    }

    public int GetActionCount() => 
        this.actionCount;

    public void Init(bool IgnoreTimeScale = false)
    {
        this.mIgnoreTimeScale = IgnoreTimeScale;
        Updater.AddUpdate("ActionBasic", new Action<float>(this.OnUpdate), IgnoreTimeScale);
        this.OnInit1();
    }

    protected virtual void OnActionClear()
    {
    }

    protected virtual void OnDeInit()
    {
    }

    protected virtual void OnInit1()
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

    public abstract class ActionBase
    {
        public string name = string.Empty;
        public EntityBase m_Entity;
        public Func<bool> ConditionBase;
        public object ConditionBase1Data;
        public Func<object, bool> ConditionBase1;
        public Func<bool> ConditionUpdate;
        public Func<bool> ConditionContinue;
        private int mEndFrame;
        private bool mIsEnd;
        private bool isInit;

        protected ActionBase()
        {
        }

        protected void End()
        {
            this.mEndFrame = Time.frameCount;
            this.isInit = false;
            this.IsEnd = true;
            this.OnEnd();
            this.OnEnd1();
        }

        public void ForceEnd()
        {
            if (!this.IsEnd)
            {
                this.OnForceEnd();
                this.End();
            }
        }

        public void Init()
        {
            if (((this.mEndFrame != Time.frameCount) || !this.IsEnd) && !this.isInit)
            {
                this.IsEnd = false;
                this.OnInit();
                if (!this.IsEnd)
                {
                    this.isInit = true;
                    if ((this.ConditionBase != null) && !this.ConditionBase())
                    {
                        this.End();
                    }
                    if ((this.ConditionBase1 != null) && !this.ConditionBase1(this.ConditionBase1Data))
                    {
                        this.End();
                    }
                }
            }
        }

        protected virtual void OnEnd()
        {
        }

        protected virtual void OnEnd1()
        {
        }

        protected abstract void OnForceEnd();
        protected abstract void OnInit();
        protected virtual void OnUpdate()
        {
        }

        public void Reset()
        {
            this.IsEnd = false;
            this.isInit = false;
        }

        public void Update()
        {
            if (!this.IsEnd)
            {
                if ((this.ConditionUpdate != null) && this.ConditionUpdate())
                {
                    this.End();
                }
                else if ((this.ConditionContinue == null) || this.ConditionContinue())
                {
                    this.OnUpdate();
                }
            }
        }

        public bool IsEnd
        {
            get => 
                this.mIsEnd;
            private set => 
                (this.mIsEnd = value);
        }
    }

    public class ActionDelegate : ActionBasic.ActionBase
    {
        public Action action;
        public Action<bool> actionbool;
        public bool resultbool;
        public Action<int> actionint;
        public int resultint;
        public Action<string> actionstring;
        public string resultstring;

        protected override void OnForceEnd()
        {
        }

        protected override void OnInit()
        {
            if (this.action != null)
            {
                this.action();
            }
            if (this.actionbool != null)
            {
                this.actionbool(this.resultbool);
            }
            if (this.actionint != null)
            {
                this.actionint(this.resultint);
            }
            if (this.actionstring != null)
            {
                this.actionstring(this.resultstring);
            }
            base.End();
        }
    }

    public class ActionParallel : ActionBasic.ActionBase
    {
        public List<ActionBasic.ActionBase> list;
        private int endCount;

        public void Add(ActionBasic.ActionBase a)
        {
            if (this.list == null)
            {
                this.list = new List<ActionBasic.ActionBase>();
            }
            this.list.Add(a);
        }

        protected override void OnForceEnd()
        {
            int num = 0;
            int count = this.list.Count;
            while (num < count)
            {
                this.list[num].ForceEnd();
                num++;
            }
        }

        protected override void OnInit()
        {
            this.endCount = 0;
        }

        protected override void OnUpdate()
        {
            int num = 0;
            int count = this.list.Count;
            while (num < count)
            {
                ActionBasic.ActionBase base2 = this.list[num];
                if (!base2.IsEnd)
                {
                    base2.Init();
                    base2.Update();
                    if (base2.IsEnd)
                    {
                        this.endCount++;
                        if (this.endCount == count)
                        {
                            base.End();
                        }
                    }
                }
                num++;
            }
        }
    }

    public class ActionShowMaskUI : ActionBasic.ActionBase
    {
        public bool show;

        protected override void OnForceEnd()
        {
        }

        protected override void OnInit()
        {
            if (this.show)
            {
                WindowUI.ShowMask(true);
            }
            else
            {
                WindowUI.ShowMask(false);
            }
            base.End();
        }
    }

    public class ActionUIBase : ActionBasic.ActionBase
    {
        protected override void OnForceEnd()
        {
        }

        protected override void OnInit()
        {
        }
    }

    public class ActionWait : ActionBasic.ActionBase
    {
        private float startTime;
        public float waitTime;
        public bool ignoreTime;

        protected override void OnForceEnd()
        {
        }

        protected override void OnInit()
        {
            this.startTime = Updater.AliveTime;
        }

        protected override void OnUpdate()
        {
            if ((Updater.AliveTime - this.startTime) >= this.waitTime)
            {
                base.End();
            }
        }
    }

    public class ActionWaitIgnoreTime : ActionBasic.ActionBase
    {
        private float startTime;
        public float waitTime;

        protected override void OnForceEnd()
        {
        }

        protected override void OnInit()
        {
            this.startTime = Updater.unscaleAliveTime;
        }

        protected override void OnUpdate()
        {
            if ((Updater.unscaleAliveTime - this.startTime) >= this.waitTime)
            {
                base.End();
            }
        }
    }
}

