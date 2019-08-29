using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class AnimationCtrlBase
{
    public const string AttackPrev = "AttackPrev";
    public const string AttackEnd = "AttackEnd";
    public const string Call = "Call";
    public const string Dead = "Dead";
    public const string Hitted = "Hitted";
    public const string Run = "Run";
    public const string Idle = "Idle";
    public const string Skill = "Skill";
    public const string Dizzy = "Dizzy";
    public const string Continuous = "Continuous";
    public const string SkillEnd = "SkillEnd";
    public const string TouchMoveJoy = "TouchMoveJoy";
    protected Dictionary<string, AniClass> mAniStringList;
    protected Dictionary<string, bool> mAniBoolList;
    protected bool bPlayHittedAction;
    private List<string> mGlobalActList;
    private Action<string, float> mActionSpeed;
    private Action mActionHitted;
    protected AniClass PrevState;
    protected AniClass CurrentState;
    protected Animation ani;
    protected AnimatorBase mAniBase;
    protected EntityBase m_Entity;
    protected bool mAttackInterrupt;
    private bool bHittedCallback;
    private bool bInit;
    protected Dictionary<string, ActionBasic> mActionList;

    public AnimationCtrlBase()
    {
        Dictionary<string, AniClass> dictionary = new Dictionary<string, AniClass>();
        List<string> list = new List<string> { 
            "AttackEnd",
            "Run",
            "Dizzy"
        };
        dictionary.Add("AttackPrev", new AniClass("AttackPrev", "AttackPrev", list));
        list = new List<string> { 
            "Dizzy",
            "Run"
        };
        dictionary.Add("AttackEnd", new AniClass("AttackEnd", "AttackEnd", list));
        list = new List<string> { "Dizzy" };
        dictionary.Add("Call", new AniClass("Call", "Call", list));
        dictionary.Add("Dead", new AniClass("Dead", "Dead", new List<string>()));
        list = new List<string> { 
            "AttackPrev",
            "Skill",
            "Call",
            "Continuous",
            "Dizzy"
        };
        dictionary.Add("Hitted", new AniClass("Hitted", "Hitted", list));
        list = new List<string> { 
            "Idle",
            "Hitted",
            "AttackPrev",
            "Skill",
            "Call",
            "Continuous",
            "Dizzy"
        };
        dictionary.Add("Run", new AniClass("Run", "Run", list));
        list = new List<string> { 
            "Run",
            "Hitted",
            "AttackPrev",
            "Skill",
            "Call",
            "Continuous",
            "Dizzy"
        };
        dictionary.Add("Idle", new AniClass("Idle", "Idle", list));
        list = new List<string> { "Dizzy" };
        dictionary.Add("Skill", new AniClass("Skill", "Skill", list));
        list = new List<string> { "Dizzy" };
        dictionary.Add("Continuous", new AniClass("Continuous", "Continuous", list));
        dictionary.Add("Dizzy", new AniClass("Dizzy", "Dizzy", new List<string>()));
        list = new List<string> { "Skill" };
        dictionary.Add("SkillEnd", new AniClass("SkillEnd", "SkillEnd", list));
        this.mAniStringList = dictionary;
        Dictionary<string, bool> dictionary2 = new Dictionary<string, bool> {
            { 
                "TouchMoveJoy",
                false
            }
        };
        this.mAniBoolList = dictionary2;
        this.bPlayHittedAction = true;
        list = new List<string> { "Dead" };
        this.mGlobalActList = list;
        this.mActionList = new Dictionary<string, ActionBasic>();
    }

    protected virtual void AttackInterrupt()
    {
    }

    protected void ChangeState(AniClass state)
    {
        if ((state.value.Equals(string.Empty) || (this.ani.GetClip(state.value) != null)) && (this.ani[state.value] != null))
        {
            this.PrevState = this.CurrentState;
            this.CurrentState = state;
            this.CurrentState.eventCmd();
        }
    }

    public void DeInit()
    {
        this.bInit = false;
        Dictionary<string, ActionBasic>.Enumerator enumerator = this.mActionList.GetEnumerator();
        while (enumerator.MoveNext())
        {
            KeyValuePair<string, ActionBasic> current = enumerator.Current;
            current.Value.ActionClear();
            current.Value.DeInit();
        }
    }

    public void DizzyEnd()
    {
        this.UpdateTouch();
    }

    private void Event_AttackEnd()
    {
        this.mActionList["AttackEnd"].ActionClear();
        this.Event_AttackEndI(this.mAniStringList["AttackEnd"]);
    }

    protected virtual void Event_AttackEndI(AniClass a)
    {
    }

    private void Event_AttackPrev()
    {
        this.mActionList["AttackPrev"].ActionClear();
        this.Event_AttackPrevI(this.mAniStringList["AttackPrev"]);
    }

    protected virtual void Event_AttackPrevI(AniClass a)
    {
    }

    private void Event_Call()
    {
        this.mActionList["Call"].ActionClear();
        this.Event_CallI(this.mAniStringList["Call"]);
    }

    protected virtual void Event_CallI(AniClass a)
    {
    }

    private void Event_Continuous()
    {
        this.mActionList["Continuous"].ActionClear();
        this.Event_ContinuousI(this.mAniStringList["Continuous"]);
    }

    protected virtual void Event_ContinuousI(AniClass a)
    {
    }

    private void Event_Dead()
    {
        this.StopAllActions();
        this.Event_DeadI(this.mAniStringList["Dead"]);
    }

    protected virtual void Event_DeadI(AniClass a)
    {
    }

    private void Event_Dizzy()
    {
        this.StopAllActions();
        this.AttackInterrupt();
        this.Event_DizzyI(this.mAniStringList["Dizzy"]);
    }

    protected virtual void Event_DizzyI(AniClass a)
    {
    }

    private void Event_Hitted()
    {
        this.mActionList["Hitted"].ActionClear();
        this.Event_HittedZI(this.mAniStringList["Hitted"]);
    }

    protected virtual void Event_HittedZI(AniClass a)
    {
    }

    private void Event_Idle()
    {
        this.Event_IdleI(this.mAniStringList["Idle"]);
    }

    protected virtual void Event_IdleI(AniClass a)
    {
    }

    private void Event_Run()
    {
        this.mActionList["AttackPrev"].ActionClear();
        this.Event_RunI(this.mAniStringList["Run"]);
    }

    protected virtual void Event_RunI(AniClass a)
    {
    }

    private void Event_Skill()
    {
        this.mActionList["Skill"].ActionClear();
        this.Event_SkillI(this.mAniStringList["Skill"]);
    }

    private void Event_SkillEnd()
    {
        this.mActionList["SkillEnd"].ActionClear();
        this.Event_SkillEndI(this.mAniStringList["SkillEnd"]);
    }

    protected virtual void Event_SkillEndI(AniClass a)
    {
    }

    protected virtual void Event_SkillI(AniClass a)
    {
    }

    public bool GetAnimationRevert(string name) => 
        (this.mAniStringList.TryGetValue(name, out AniClass class2) && class2.revert);

    public float GetAnimationSpeed(string name)
    {
        if (this.mAniStringList.TryGetValue(name, out AniClass class2) && (this.ani != null))
        {
            return class2.GetSpeed();
        }
        return 1f;
    }

    public float GetAnimationTime(string name)
    {
        if (this.mAniStringList.TryGetValue(name, out AniClass class2) && (this.ani != null))
        {
            return (this.ani[class2.value].length / class2.Speed);
        }
        return 1f;
    }

    public string GetAnimationValue(string name)
    {
        if (this.mAniStringList.TryGetValue(name, out AniClass class2))
        {
            return class2.value;
        }
        return string.Empty;
    }

    public bool GetPlayHittedCallback() => 
        this.bHittedCallback;

    public void InitWeaponSpeed(float speed)
    {
        this.mAniStringList["AttackPrev"].UpdateSpeedWeapon(speed);
        this.mAniStringList["AttackEnd"].UpdateSpeedWeapon(speed);
    }

    public bool IsCurrentState(string state) => 
        (this.CurrentState.name == state);

    public void OnStart()
    {
        this.CurrentState = this.mAniStringList["Idle"];
        this.mAniStringList["Idle"].eventCmd = new Action(this.Event_Idle);
        this.mAniStringList["Run"].eventCmd = new Action(this.Event_Run);
        this.mAniStringList["Hitted"].eventCmd = new Action(this.Event_Hitted);
        this.mAniStringList["AttackPrev"].eventCmd = new Action(this.Event_AttackPrev);
        this.mAniStringList["AttackEnd"].eventCmd = new Action(this.Event_AttackEnd);
        this.mAniStringList["Dead"].eventCmd = new Action(this.Event_Dead);
        this.mAniStringList["Call"].eventCmd = new Action(this.Event_Call);
        this.mAniStringList["Skill"].eventCmd = new Action(this.Event_Skill);
        this.mAniStringList["Continuous"].eventCmd = new Action(this.Event_Continuous);
        this.mAniStringList["Dizzy"].eventCmd = new Action(this.Event_Dizzy);
        this.mAniStringList["SkillEnd"].eventCmd = new Action(this.Event_SkillEnd);
        this.mActionList.Add("Idle", new ActionBasic());
        this.mActionList.Add("Run", new ActionBasic());
        this.mActionList.Add("Hitted", new ActionBasic());
        this.mActionList.Add("AttackPrev", new ActionBasic());
        this.mActionList.Add("AttackEnd", new ActionBasic());
        this.mActionList.Add("Call", new ActionBasic());
        this.mActionList.Add("Skill", new ActionBasic());
        this.mActionList.Add("Dizzy", new ActionBasic());
        this.mActionList.Add("Continuous", new ActionBasic());
        this.mActionList.Add("Dead", new ActionBasic());
        this.mActionList.Add("SkillEnd", new ActionBasic());
        Dictionary<string, ActionBasic>.Enumerator enumerator = this.mActionList.GetEnumerator();
        while (enumerator.MoveNext())
        {
            KeyValuePair<string, ActionBasic> current = enumerator.Current;
            current.Value.Init(false);
        }
        this.bInit = true;
    }

    protected void PlayHittedAction(bool value)
    {
        if (this.bHittedCallback != value)
        {
            this.bHittedCallback = value;
            if (this.m_Entity.OnPlayHittedAction != null)
            {
                this.m_Entity.OnPlayHittedAction(this.bHittedCallback);
            }
        }
    }

    public void Reborn()
    {
        if (this.mAniStringList.TryGetValue("Idle", out AniClass class2))
        {
            this.ChangeState(class2);
        }
    }

    protected void ResetPrevState()
    {
        this.PrevState = this.mAniStringList["Idle"];
    }

    public void SendEvent(string eventName, bool force = false)
    {
        if (this.bPlayHittedAction || (eventName != "Hitted"))
        {
            if ((eventName == "Hitted") && (this.mActionHitted != null))
            {
                this.mActionHitted();
            }
            if (this.CurrentState.name == "Hitted")
            {
                this.PlayHittedAction(false);
            }
            if (this.mGlobalActList.Contains(eventName) && this.mAniStringList.TryGetValue(eventName, out AniClass class2))
            {
                this.ChangeState(class2);
            }
            else
            {
                if ((force && (this.CurrentState.name == "AttackPrev")) && ((eventName == "Idle") || (eventName == "Run")))
                {
                    force = false;
                }
                if (((this.CurrentState.name != "Dead") && force) && this.mAniStringList.TryGetValue(eventName, out class2))
                {
                    this.ChangeState(class2);
                }
                else if (this.CurrentState.HaveEvent(eventName) && this.mAniStringList.TryGetValue(eventName, out class2))
                {
                    this.ChangeState(class2);
                }
            }
        }
    }

    public void SetAllSpeed(float speed)
    {
        Dictionary<string, AniClass>.Enumerator enumerator = this.mAniStringList.GetEnumerator();
        while (enumerator.MoveNext())
        {
            KeyValuePair<string, AniClass> current = enumerator.Current;
            this.UpdateAnimationSpeed(current.Key, speed);
        }
        this.UpdateWeaponSpeed(speed);
    }

    public void SetAnimation(Animation ani)
    {
        this.ani = ani;
    }

    public void SetAnimationClear(string name)
    {
        if (this.mAniStringList.TryGetValue(name, out AniClass class2))
        {
            class2.value = string.Empty;
        }
    }

    public void SetAnimationRevert(string name, bool revert)
    {
        if (this.mAniStringList.TryGetValue(name, out AniClass class2))
        {
            class2.revert = revert;
        }
    }

    public void SetAnimationValue(string name, string value = "")
    {
        if (this.mAniStringList.TryGetValue(name, out AniClass class2))
        {
            class2.value = (value != string.Empty) ? value : name;
        }
    }

    public void SetAnimatorBase(AnimatorBase b)
    {
        this.mAniBase = b;
        this.m_Entity = this.mAniBase.m_Entity;
        this.mAniStringList["Idle"].InitSpeedInit(this.mAniBase.m_Entity.m_Data.ActionSpeed[0]);
        this.mAniStringList["Run"].InitSpeedInit(this.mAniBase.m_Entity.m_Data.ActionSpeed[1]);
        this.mAniStringList["Hitted"].InitSpeedInit(this.mAniBase.m_Entity.m_Data.ActionSpeed[2]);
        this.mAniStringList["AttackPrev"].InitSpeedInit(this.mAniBase.m_Entity.m_Data.ActionSpeed[3]);
        this.mAniStringList["AttackEnd"].InitSpeedInit(this.mAniBase.m_Entity.m_Data.ActionSpeed[4]);
        this.mAniStringList["Dead"].InitSpeedInit(this.mAniBase.m_Entity.m_Data.ActionSpeed[5]);
        this.mAniStringList["Call"].InitSpeedInit(this.mAniBase.m_Entity.m_Data.ActionSpeed[6]);
        this.mAniStringList["Skill"].InitSpeedInit(this.mAniBase.m_Entity.m_Data.ActionSpeed[7]);
        this.mAniStringList["Continuous"].InitSpeedInit(this.mAniBase.m_Entity.m_Data.ActionSpeed[8]);
        this.mAniStringList["Dizzy"].InitSpeedInit(this.mAniBase.m_Entity.m_Data.ActionSpeed[9]);
        this.mAniStringList["SkillEnd"].InitSpeedInit(1f);
        Dictionary<string, AniClass>.Enumerator enumerator = this.mAniStringList.GetEnumerator();
        while (enumerator.MoveNext())
        {
            KeyValuePair<string, AniClass> current = enumerator.Current;
            this.UpdateAnimationSpeed(current.Key, 0f);
        }
    }

    public void SetBool(string name, bool value)
    {
        if (this.mAniBoolList.ContainsKey(name))
        {
            this.mAniBoolList[name] = value;
        }
    }

    public void SetDontPlayHittedAction()
    {
        this.bPlayHittedAction = false;
    }

    public void SetHeroPlayMakerColtrol(HeroPlayMakerControl ctrl)
    {
    }

    public void SetHittedCallBack(Action callback)
    {
        this.mActionHitted = callback;
    }

    private void StopAllActions()
    {
        Dictionary<string, ActionBasic>.Enumerator enumerator = this.mActionList.GetEnumerator();
        while (enumerator.MoveNext())
        {
            KeyValuePair<string, ActionBasic> current = enumerator.Current;
            current.Value.ActionClear();
        }
        this.PlayHittedAction(false);
    }

    protected void UpdateAnimationSpeed(string name)
    {
        if (this.ani != null)
        {
            this.mAniStringList.TryGetValue(name, out AniClass class2);
            if (class2 != null)
            {
                AnimationState state = this.ani[class2.value];
                if (state != null)
                {
                    state.speed = class2.Speed * (!class2.revert ? ((float) 1) : ((float) (-1)));
                }
            }
        }
    }

    public void UpdateAnimationSpeed(string name, float speed)
    {
        if (this.mAniStringList.TryGetValue(name, out AniClass class2))
        {
            class2.UpdateSpeedIn(speed);
        }
    }

    public void UpdateAttackPrevSpeed(float speed)
    {
    }

    public void UpdateSpeedOut(float speed)
    {
        this.mAniStringList["AttackPrev"].UpdateSpeedOut(speed);
        this.mAniStringList["AttackEnd"].UpdateSpeedOut(speed);
    }

    protected void UpdateTouch()
    {
        if (this.mAniBoolList["TouchMoveJoy"])
        {
            this.SendEvent("Run", true);
        }
        else
        {
            this.SendEvent("Idle", true);
        }
    }

    public void UpdateWeaponSpeed(float speed)
    {
        float num = 0f;
        if (speed < 0f)
        {
            num = 1f + speed;
        }
        else if (speed > 0f)
        {
            num = 1f / (1f - speed);
        }
        this.mAniStringList["AttackPrev"].UpdateSpeedWeapon(num);
        this.mAniStringList["AttackEnd"].UpdateSpeedWeapon(num);
    }

    public class AniClass
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <name>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <value>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <revert>k__BackingField;
        private float speedinit = 1f;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <speed_out>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <Speed_Weapon>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <speed_in>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <Speed>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<string> <action_list>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Action <eventCmd>k__BackingField;

        public AniClass(string name, string value, List<string> action_list)
        {
            this.name = name;
            this.value = value;
            this.action_list = action_list;
            this.revert = false;
            this.speedinit = 1f;
            this.speed_out = 1f;
            this.speed_in = 1f;
            this.Speed_Weapon = 1f;
            this.UpdateSpeed();
        }

        public void AddAction(string action)
        {
            if (!this.action_list.Contains(action))
            {
                this.action_list.Add(action);
            }
        }

        public float GetSpeed() => 
            this.Speed;

        public bool HaveEvent(string name) => 
            this.action_list.Contains(name);

        public void InitSpeedInit(float speed)
        {
            this.speedinit = speed;
            this.UpdateSpeed();
        }

        private void UpdateSpeed()
        {
            this.Speed = ((this.speedinit * this.speed_out) * this.speed_in) * this.Speed_Weapon;
        }

        public void UpdateSpeedIn(float speed)
        {
            this.speed_in += speed;
            this.UpdateSpeed();
        }

        public void UpdateSpeedOut(float speed)
        {
            this.speed_out = speed;
            this.UpdateSpeed();
        }

        public void UpdateSpeedWeapon(float speed)
        {
            this.Speed_Weapon *= speed;
            this.UpdateSpeed();
        }

        public string name { get; private set; }

        public string value { get; set; }

        public bool revert { get; set; }

        private float speed_out { get; set; }

        public float Speed_Weapon { get; private set; }

        private float speed_in { get; set; }

        public float Speed { get; private set; }

        public List<string> action_list { get; private set; }

        public Action eventCmd { get; set; }
    }
}

