using Dxx.Util;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using TableTool;
using UnityEngine;

public class AIBase : ActionBasic
{
    public EntityBase m_Entity;
    public EntityMonsterBase m_MonsterEntity;
    protected string ClassName;
    private int pClassID;
    private float actionTime;
    private EntityAttackBase mEntityAttack;
    protected float mRoomTime;
    private float mCreateNewTime;
    private float mStartTime;
    protected bool IsDelayTime = true;
    protected bool bReRandom;
    private Dictionary<int, CallData> mCallList = new Dictionary<int, CallData>();

    protected void AddActionAddCall(int entityId, int bulletid)
    {
        <AddActionAddCall>c__AnonStorey0 storey = new <AddActionAddCall>c__AnonStorey0 {
            entityId = entityId,
            $this = this
        };
        if (this.mCallList.TryGetValue(storey.entityId, out CallData data))
        {
            storey.pos = this.GetRandomCall(storey.entityId, data);
            base.AddActionDelegate(new Action(storey.<>m__0));
            if (this.m_MonsterEntity != null)
            {
                base.AddAction(this.GetActionRotateToPos(storey.pos));
            }
            base.AddAction(this.GetActionAttack(string.Empty, bulletid, false));
        }
    }

    protected void AddCallCount(int callid)
    {
        if (this.mCallList.TryGetValue(callid, out CallData data))
        {
            data.AddCall();
        }
    }

    public void Attack(int AttackID, bool bRotate)
    {
        this.RemoveMove();
        this.RemoveAttack();
        this.mEntityAttack = new EntityAttack();
        this.mEntityAttack.SetRotate(bRotate);
        this.mEntityAttack.Init(this.m_Entity, AttackID);
    }

    public void AttackSpecial(int AttackID, bool bRotate)
    {
        this.RemoveMove();
        object[] args = new object[] { "EntityAttack", AttackID };
        Type type = Type.GetType(Utils.GetString(args));
        object[] objArray2 = new object[] { "EntityAttack", AttackID };
        this.mEntityAttack = type.Assembly.CreateInstance(Utils.GetString(objArray2)) as EntityAttackBase;
        this.mEntityAttack.SetRotate(bRotate);
        this.mEntityAttack.Init(this.m_Entity, AttackID);
    }

    public void Call(ActionCall.ActionCallData data)
    {
        int callCount = this.GetCallCount(data.entityId);
        for (int i = 0; i < callCount; i++)
        {
            if (this.IsCallStand(data.entityId))
            {
                this.CallStand(data);
            }
            else
            {
                this.CallMove(data);
            }
            this.AddCallCount(data.entityId);
        }
    }

    private void CallMove(ActionCall.ActionCallData data)
    {
        if (this.mCallList.TryGetValue(data.entityId, out CallData data2))
        {
            GameLogic.Release.MapCreatorCtrl.RandomItemSides(this.m_Entity, data2.radiusmin, data2.radiusmax, out float num, out float num2);
            Vector3 pos = new Vector3(num, this.m_Entity.m_Body.EffectMask.transform.position.y, num2);
            this.CallOne(data.entityId, pos);
        }
    }

    public void CallOne(Vector3 pos)
    {
        int callID = this.m_Entity.CallID;
        this.CallOneInternal(callID, pos, true);
    }

    private void CallOne(int callid, Vector3 pos)
    {
        this.CallOneInternal(callid, pos, true);
    }

    public void CallOne(Vector3 pos, bool showeffect)
    {
        int callID = this.m_Entity.CallID;
        this.CallOneInternal(callID, pos, showeffect);
    }

    protected void CallOneInternal(int callid, Vector3 pos, bool showcalleffect)
    {
        EntityMonsterBase base2 = GameLogic.Release.MapCreatorCtrl.CreateEntityCall(callid, pos.x, pos.z);
        if (showcalleffect)
        {
            GameLogic.PlayEffect(0x2f4d68, new Vector3(pos.x, 0f, pos.z));
        }
        base2.SetParent(this.m_Entity);
    }

    private void CallStand(ActionCall.ActionCallData data)
    {
        if (this.mCallList.TryGetValue(data.entityId, out CallData data2) && GameLogic.Release.MapCreatorCtrl.RandomCallSide(this.m_Entity, data2.radiusmin, data2.radiusmax, out float num, out float num2))
        {
            Vector3 pos = new Vector3(num, this.m_Entity.m_Body.EffectMask.transform.position.y, num2);
            this.CallOne(data.entityId, pos);
        }
    }

    public void DeadBefore()
    {
        this.OnDeadBefore();
    }

    public void Divide(int entityid, int count)
    {
        this.m_Entity.PlayEffect(0x2f4d69, this.m_Entity.m_Body.EffectMask.transform.position);
        GameLogic.Release.Entity.Remove(this.m_Entity);
        float x = this.m_Entity.position.x;
        float z = this.m_Entity.position.z;
        if (count <= 1)
        {
            GameLogic.Release.MapCreatorCtrl.CreateDivideEntity(this.m_Entity, entityid, x, z).DivideID = this.m_Entity.DivideID;
        }
        else
        {
            float num3 = 1f;
            float num4 = 360f / ((float) count);
            float angle = -180f - num4;
            for (int i = 0; i < count; i++)
            {
                angle += num4;
                float num7 = MathDxx.Cos(angle) * num3;
                float num8 = MathDxx.Sin(angle) * num3;
                EntityBase base3 = GameLogic.Release.MapCreatorCtrl.CreateDivideEntity(this.m_Entity, entityid, x, z);
                base3.DivideID = this.m_Entity.DivideID;
                base3.DivideAction(num7, num8);
            }
        }
    }

    protected ActionAttack GetActionAttack(string name, int attackId, bool rotate = true) => 
        new ActionAttack { 
            name = name,
            attackId = attackId,
            bAttackSpecial = false,
            bRotate = rotate,
            m_AIBase = this,
            m_Entity = this.m_Entity
        };

    protected ActionAttack GetActionAttackSpecial(string name, int attackId, bool rotate = true) => 
        new ActionAttack { 
            name = name,
            attackId = attackId,
            bAttackSpecial = true,
            bRotate = rotate,
            m_AIBase = this,
            m_Entity = this.m_Entity
        };

    protected ActionSequence GetActionAttackWait(int attackID, int waittime, int waitmaxtime = -1)
    {
        ActionSequence sequence = new ActionSequence {
            m_Entity = this.m_Entity
        };
        sequence.AddAction(this.GetActionAttack(string.Empty, attackID, true));
        if (waitmaxtime == -1)
        {
            waitmaxtime = waittime;
        }
        sequence.AddAction(this.GetActionWaitRandom(string.Empty, waittime, waitmaxtime));
        return sequence;
    }

    protected ActionBasic.ActionBase GetActionCall(int entityId) => 
        this.GetActionCallInternal(entityId, new Action<ActionCall.ActionCallData>(this.Call));

    protected ActionBasic.ActionBase GetActionCallInternal(int entityId, Action<ActionCall.ActionCallData> call)
    {
        ActionSequence sequence = new ActionSequence {
            ConditionBase = new Func<bool>(this.GetIsAlive)
        };
        ActionBasic.ActionDelegate action = new ActionBasic.ActionDelegate {
            action = delegate {
                this.m_Entity.m_AniCtrl.SendEvent("Call", false);
            }
        };
        sequence.AddAction(action);
        float animationTime = this.m_Entity.mAniCtrlBase.GetAnimationTime("Call");
        ActionBasic.ActionWait wait = new ActionBasic.ActionWait {
            waitTime = animationTime * 0.4f
        };
        sequence.AddAction(wait);
        ActionCall call2 = new ActionCall();
        call2.InitData(entityId);
        call2.action = call;
        sequence.AddAction(call2);
        wait = new ActionBasic.ActionWait {
            waitTime = animationTime * 0.6f
        };
        sequence.AddAction(wait);
        return sequence;
    }

    protected ActionBasic.ActionDelegate GetActionDelegate(Action action) => 
        new ActionBasic.ActionDelegate { action = action };

    protected ActionDivide GetActionDivide(string name, int entityId, int count) => 
        new ActionDivide { 
            name = name,
            entityId = entityId,
            count = count,
            action = new Action<int, int>(this.Divide),
            m_Entity = this.m_Entity
        };

    protected ActionBasic.ActionDelegate GetActionRemoveAttack() => 
        this.GetActionDelegate(delegate {
            this.RemoveAttack();
        });

    protected ActionBasic.ActionDelegate GetActionRemoveMove() => 
        this.GetActionDelegate(delegate {
            this.RemoveMove();
        });

    protected ActionBasic.ActionBase GetActionRotate(float angle) => 
        new ActionRotate { 
            m_Entity = this.m_Entity,
            angle = angle
        };

    protected ActionBasic.ActionBase GetActionRotateToEntity(EntityBase target) => 
        new ActionRotateToEntity { 
            m_Entity = this.m_Entity,
            target = target
        };

    protected ActionBasic.ActionBase GetActionRotateToPos(Vector3 pos) => 
        new ActionRotateToPos { 
            m_Entity = this.m_Entity,
            pos = pos
        };

    protected ActionBasic.ActionWait GetActionWait(string name, int waitTime) => 
        new ActionBasic.ActionWait { 
            name = name,
            waitTime = ((float) waitTime) / 1000f,
            m_Entity = this.m_Entity
        };

    protected ActionBasic.ActionBase GetActionWaitDelegate(int time, Action action)
    {
        ActionSequence sequence = new ActionSequence {
            m_Entity = this.m_Entity
        };
        sequence.AddAction(this.GetActionWait(string.Empty, time));
        sequence.AddAction(this.GetActionDelegate(action));
        return sequence;
    }

    protected ActionWaitRandom GetActionWaitRandom(string name, int min, int max) => 
        new ActionWaitRandom { 
            name = name,
            min = min,
            max = max,
            m_Entity = this.m_Entity
        };

    protected int GetAliveCount(int callid, bool over = false)
    {
        if (this.mCallList.TryGetValue(callid, out CallData data))
        {
            return data.CurAliveCount;
        }
        return 0;
    }

    public bool GetAttackEnd()
    {
        if ((this.mEntityAttack != null) && !this.mEntityAttack.GetIsEnd())
        {
            return false;
        }
        return !this.m_Entity.m_AttackCtrl.GetAttacking();
    }

    protected int GetCallCount(int callid)
    {
        if (this.mCallList.TryGetValue(callid, out CallData data))
        {
            return data.GetCallCount();
        }
        return 0;
    }

    protected bool GetCanCall(object callid)
    {
        int key = (int) callid;
        return (this.mCallList.TryGetValue(key, out CallData data) && data.GetCanCall());
    }

    protected bool GetHaveHatred() => 
        (this.m_Entity.m_HatredTarget != null);

    protected bool GetIsAlive() => 
        ((this.m_Entity != null) && !this.m_Entity.GetIsDead());

    private Vector3 GetRandomCall(int entityid, CallData data)
    {
        Vector3 vector;
        if (this.IsCallStand(entityid))
        {
            GameLogic.Release.MapCreatorCtrl.RandomCallSide(this.m_Entity, data.radiusmin, data.radiusmax, out float num, out float num2);
            vector = new Vector3(num, 0f, num2);
        }
        else
        {
            GameLogic.Release.MapCreatorCtrl.RandomItemSides(this.m_Entity, data.radiusmin, data.radiusmax, out float num3, out float num4);
            vector = new Vector3(num3, 0f, num4);
        }
        for (Vector2Int num5 = GameLogic.Release.MapCreatorCtrl.GetRoomXY(vector); !GameLogic.Release.MapCreatorCtrl.IsEmpty(num5); num5 = GameLogic.Release.MapCreatorCtrl.GetRoomXY(vector))
        {
            if (this.IsCallStand(entityid))
            {
                GameLogic.Release.MapCreatorCtrl.RandomCallSide(this.m_Entity, data.radiusmin, data.radiusmax, out float num6, out float num7);
                vector = new Vector3(num6, 0f, num7);
            }
            else
            {
                GameLogic.Release.MapCreatorCtrl.RandomItemSides(this.m_Entity, data.radiusmin, data.radiusmax, out float num8, out float num9);
                vector = new Vector3(num8, 0f, num9);
            }
        }
        return vector;
    }

    protected void InitCallData(CallData data)
    {
        if (this.mCallList.ContainsKey(data.CallID))
        {
            this.mCallList[data.CallID] = data;
        }
        else
        {
            this.mCallList.Add(data.CallID, data);
        }
    }

    protected void InitCallData(int callid, int alivecount, int count, int percount, int radiusmin, int radiusmax)
    {
        if (this.mCallList.TryGetValue(callid, out CallData data))
        {
            data.CallID = callid;
            data.MaxAliveCount = alivecount;
            data.MaxCount = count;
            data.perCount = percount;
            data.radiusmin = radiusmin;
            data.radiusmax = radiusmax;
        }
        else
        {
            data = new CallData(callid, alivecount, count, percount, radiusmin, radiusmax);
            this.mCallList.Add(callid, data);
        }
    }

    private bool IsCallStand(int entityid) => 
        (LocalModelManager.Instance.Character_Char.GetBeanById(entityid).Speed == 0);

    protected override void OnActionClear()
    {
        base.actionIndex = 0;
    }

    protected virtual void OnAIDeInit()
    {
    }

    protected virtual void OnDeadBefore()
    {
    }

    protected sealed override void OnDeInit()
    {
        base.OnDeInit();
        this.RemoveAttack();
        this.RemoveCurrentAction();
        this.OnAIDeInit();
    }

    protected virtual void OnElite()
    {
    }

    protected virtual void OnInit()
    {
    }

    protected sealed override void OnInit1()
    {
        this.ClassName = base.GetType().ToString();
        int.TryParse(this.ClassName.Substring(this.ClassName.Length - 4, 4), out this.pClassID);
        this.actionTime = Updater.AliveTime;
        this.mRoomTime = GameLogic.Random((float) 0.5f, (float) 0.7f);
        this.mCreateNewTime = 0.3f;
        if (this.m_Entity.IsElite)
        {
            this.OnElite();
        }
        this.OnInitOnce();
        this.OnInit();
        if (this.bReRandom)
        {
            base.AddAction(this.GetActionDelegate(new Action(this.ReRandomAI)));
        }
    }

    protected virtual void OnInitOnce()
    {
    }

    protected override void OnUpdate(float delta)
    {
        if ((this.m_Entity == null) || (this.m_Entity.gameObject.activeInHierarchy && !this.m_Entity.GetIsDead()))
        {
            if (this.IsDelayTime)
            {
                if (this.m_Entity.bDivide || this.m_Entity.bCall)
                {
                    if ((Updater.AliveTime - this.actionTime) < this.mCreateNewTime)
                    {
                        return;
                    }
                }
                else if ((Updater.AliveTime - this.actionTime) < this.mRoomTime)
                {
                    return;
                }
            }
            if (((this.m_Entity == null) || !this.m_Entity.m_EntityData.IsDizzy()) && (base.actionCount > 0))
            {
                ActionBasic.ActionBase base2 = base.actionList[base.actionIndex];
                int actionIndex = base.actionIndex;
                if ((actionIndex == base.actionIndex) && base2.IsEnd)
                {
                    base.actionIndex++;
                    if (base.actionIndex >= base.actionCount)
                    {
                        for (int i = 0; i < base.actionCount; i++)
                        {
                            base.actionList[i].Reset();
                        }
                    }
                    base.actionIndex = base.actionIndex % base.actionCount;
                    base2 = base.actionList[base.actionIndex];
                    actionIndex = base.actionIndex;
                }
                base2.Init();
                base2.Update();
                if ((actionIndex == base.actionIndex) && base2.IsEnd)
                {
                    base.actionIndex++;
                    if (base.actionIndex >= base.actionCount)
                    {
                        for (int i = 0; i < base.actionCount; i++)
                        {
                            base.actionList[i].Reset();
                        }
                    }
                    base.actionIndex = base.actionIndex % base.actionCount;
                }
            }
        }
    }

    private void RemoveAttack()
    {
        if (this.mEntityAttack != null)
        {
            this.mEntityAttack.UnInstall();
            this.mEntityAttack = null;
        }
    }

    public void RemoveCall(RemoveCallData data)
    {
        this.RemoveCallCount(data.entityId);
        if (this.IsCallStand(data.entityId))
        {
            GameLogic.Release.MapCreatorCtrl.CallPositionRecover(data.deadpos);
        }
    }

    protected void RemoveCallCount(int callid)
    {
        if (this.mCallList.TryGetValue(callid, out CallData data))
        {
            data.RemoveCall();
        }
    }

    protected void RemoveCurrentAction()
    {
        if (base.actionCount > 0)
        {
            base.actionList[base.actionIndex].ForceEnd();
        }
    }

    public void RemoveMove()
    {
        if (base.actionCount > 0)
        {
            ActionBasic.ActionBase base2 = base.actionList[base.actionIndex];
            if (base2 is AIMoveBase)
            {
                base2.ForceEnd();
            }
        }
    }

    protected void ReRandomAI()
    {
        base.ActionClear();
        this.OnInit();
        base.AddAction(this.GetActionDelegate(new Action(this.ReRandomAI)));
    }

    public void SetEntity(EntityBase entity)
    {
        this.m_Entity = entity;
        this.m_MonsterEntity = entity as EntityMonsterBase;
    }

    public int ClassID =>
        this.pClassID;

    [CompilerGenerated]
    private sealed class <AddActionAddCall>c__AnonStorey0
    {
        internal int entityId;
        internal Vector3 pos;
        internal AIBase $this;

        internal void <>m__0()
        {
            this.$this.AddCallCount(this.entityId);
            if (this.$this.m_MonsterEntity != null)
            {
                this.$this.m_MonsterEntity.SetCallID(this.entityId, this.pos);
            }
        }
    }

    public class ActionAttack : ActionBasic.ActionBase
    {
        public int attackId;
        public AIBase m_AIBase;
        public bool bAttackSpecial;
        public bool bRotate = true;
        private bool bPlayAttack;
        private float test_time;

        private void Attack()
        {
            if (!this.bAttackSpecial)
            {
                this.m_AIBase.Attack(this.attackId, this.bRotate);
            }
            else
            {
                this.m_AIBase.AttackSpecial(this.attackId, this.bRotate);
            }
        }

        protected override void OnForceEnd()
        {
        }

        protected override void OnInit()
        {
            this.bPlayAttack = false;
            this.m_AIBase.RemoveAttack();
        }

        protected override void OnUpdate()
        {
            if (base.m_Entity == null)
            {
                base.End();
            }
            else
            {
                if ((base.m_Entity != null) && !this.bPlayAttack)
                {
                    if (this.bRotate)
                    {
                        base.m_Entity.m_AttackCtrl.RotateUpdate(base.m_Entity.m_HatredTarget);
                    }
                    if (base.m_Entity.m_AttackCtrl.RotateOver() || !this.bRotate)
                    {
                        this.bPlayAttack = true;
                        this.Attack();
                    }
                }
                if (this.bPlayAttack)
                {
                    this.test_time += Updater.delta;
                    if (this.test_time > 1f)
                    {
                        this.test_time--;
                    }
                    if (this.m_AIBase.GetAttackEnd())
                    {
                        base.End();
                    }
                }
            }
        }
    }

    public class ActionCall : ActionBasic.ActionBase
    {
        private ActionCallData data;
        public Action<ActionCallData> action;

        public void InitData(int entityId)
        {
            ActionCallData data = new ActionCallData {
                entityId = entityId
            };
            this.data = data;
        }

        protected override void OnForceEnd()
        {
        }

        protected override void OnInit()
        {
            if (this.action != null)
            {
                this.action(this.data);
            }
            base.End();
        }

        public class ActionCallData
        {
            public int entityId;
        }
    }

    public class ActionChoose : ActionBasic.ActionBase
    {
        public Func<bool> Condition;
        public ActionBasic.ActionBase ResultTrue;
        public ActionBasic.ActionBase ResultFalse;
        private bool bResult;

        private void ExcuteResultInit()
        {
            if (this.bResult)
            {
                if (this.ResultTrue != null)
                {
                    this.ResultTrue.Init();
                    if (this.ResultTrue.IsEnd)
                    {
                        base.End();
                    }
                }
                else
                {
                    base.End();
                }
            }
            else if (this.ResultFalse != null)
            {
                this.ResultFalse.Init();
                if (this.ResultFalse.IsEnd)
                {
                    base.End();
                }
            }
            else
            {
                base.End();
            }
        }

        private void ExcuteResultUpdate()
        {
            if (this.bResult)
            {
                if (this.ResultTrue != null)
                {
                    this.ResultTrue.Update();
                    if (this.ResultTrue.IsEnd)
                    {
                        base.End();
                    }
                }
            }
            else if (this.ResultFalse != null)
            {
                this.ResultFalse.Update();
                if (this.ResultFalse.IsEnd)
                {
                    base.End();
                }
            }
        }

        protected override void OnForceEnd()
        {
            if (this.bResult)
            {
                if (this.ResultTrue != null)
                {
                    this.ResultTrue.ForceEnd();
                }
            }
            else if (this.ResultFalse != null)
            {
                this.ResultFalse.ForceEnd();
            }
        }

        protected override void OnInit()
        {
            this.bResult = this.Condition();
            this.ExcuteResultInit();
        }

        protected override void OnUpdate()
        {
            this.ExcuteResultUpdate();
        }
    }

    public class ActionChooseIf : ActionBasic.ActionBase
    {
        private List<ActionBasic.ActionBase> list = new List<ActionBasic.ActionBase>();
        private int count;
        private int index;

        public void AddAction(ActionBasic.ActionBase action)
        {
            this.list.Add(action);
            this.count++;
        }

        protected override void OnForceEnd()
        {
            if ((this.index >= 0) && (this.index < this.count))
            {
                this.list[this.index].ForceEnd();
            }
        }

        protected override void OnInit()
        {
            this.index = this.count;
            for (int i = 0; i < this.count; i++)
            {
                ActionBasic.ActionBase base2 = this.list[i];
                if ((base2.ConditionBase == null) || base2.ConditionBase())
                {
                    this.index = i;
                    break;
                }
            }
        }

        protected override void OnUpdate()
        {
            if (this.index < this.count)
            {
                ActionBasic.ActionBase base2 = this.list[this.index];
                base2.Init();
                base2.Update();
                if (base2.IsEnd)
                {
                    base.End();
                }
            }
            else
            {
                base.End();
            }
        }
    }

    public class ActionChooseRandom : ActionBasic.ActionBase
    {
        private List<ActionBasic.ActionBase> actionList = new List<ActionBasic.ActionBase>();
        private List<int> weightList = new List<int>();
        private int allWeight;
        private int actionCount;
        private int currentIndex = -1;

        public void AddAction(int weight, ActionBasic.ActionBase action)
        {
            this.weightList.Add(weight);
            this.allWeight += weight;
            this.actionList.Add(action);
            this.actionCount++;
        }

        private int GetRandomWeight()
        {
            int num = GameLogic.Random(0, this.allWeight);
            int num2 = 0;
            for (int i = 0; i < this.actionCount; i++)
            {
                num2 += this.weightList[i];
                if (num < num2)
                {
                    return i;
                }
            }
            return 0;
        }

        protected override void OnForceEnd()
        {
            if (this.currentIndex >= 0)
            {
                this.actionList[this.currentIndex].ForceEnd();
            }
        }

        protected override void OnInit()
        {
        }

        protected override void OnUpdate()
        {
            if (this.currentIndex == -1)
            {
                this.currentIndex = this.GetRandomWeight();
            }
            ActionBasic.ActionBase base2 = this.actionList[this.currentIndex];
            base2.Init();
            base2.Update();
            if (base2.IsEnd)
            {
                base.End();
                this.currentIndex = -1;
            }
        }
    }

    public class ActionDivide : ActionBasic.ActionBase
    {
        public int entityId;
        public int count;
        public Action<int, int> action;

        protected override void OnForceEnd()
        {
        }

        protected override void OnInit()
        {
            if (this.action != null)
            {
                this.action(this.entityId, this.count);
            }
            base.End();
        }
    }

    public class ActionMove : ActionBasic.ActionBase
    {
        public int moveId;
        public Action<int> action;

        protected override void OnForceEnd()
        {
        }

        protected override void OnInit()
        {
            if (this.action != null)
            {
                this.action(this.moveId);
            }
            base.End();
        }
    }

    public class ActionRotate : ActionBasic.ActionBase
    {
        public float angle;
        private bool bRotate;

        protected override void OnForceEnd()
        {
        }

        protected override void OnInit()
        {
            this.bRotate = true;
        }

        protected override void OnUpdate()
        {
            if (this.bRotate)
            {
                base.m_Entity.m_AttackCtrl.RotateHero(this.angle);
                this.bRotate = false;
            }
            if (base.m_Entity.m_AttackCtrl.RotateOver())
            {
                base.End();
            }
        }
    }

    public class ActionRotateTime : ActionBasic.ActionBase
    {
        public EntityBase target;
        public float time;
        private float mTime;

        protected override void OnForceEnd()
        {
        }

        protected override void OnInit()
        {
            this.mTime = this.time;
        }

        protected override void OnUpdate()
        {
            if (this.mTime > 0f)
            {
                base.m_Entity.m_AttackCtrl.RotateHero(Utils.getAngle(this.target.position - base.m_Entity.position));
                this.mTime -= Updater.delta;
            }
            else
            {
                base.End();
            }
        }
    }

    public class ActionRotateToEntity : ActionBasic.ActionBase
    {
        public EntityBase target;
        private bool bRotate;

        protected override void OnForceEnd()
        {
        }

        protected override void OnInit()
        {
            this.bRotate = true;
        }

        protected override void OnUpdate()
        {
            if (this.bRotate)
            {
                base.m_Entity.m_AttackCtrl.RotateHero(Utils.getAngle(this.target.position - base.m_Entity.position));
                this.bRotate = false;
            }
            if (base.m_Entity.m_AttackCtrl.RotateOver())
            {
                base.End();
            }
        }
    }

    public class ActionRotateToPos : ActionBasic.ActionBase
    {
        public Vector3 pos;
        private bool bRotate;

        protected override void OnForceEnd()
        {
        }

        protected override void OnInit()
        {
            this.bRotate = true;
        }

        protected override void OnUpdate()
        {
            if (this.bRotate)
            {
                base.m_Entity.m_AttackCtrl.RotateHero(Utils.getAngle(this.pos - base.m_Entity.position));
                this.bRotate = false;
            }
            if (base.m_Entity.m_AttackCtrl.RotateOver())
            {
                base.End();
            }
        }
    }

    public class ActionSequence : ActionBasic.ActionBase
    {
        public List<ActionBasic.ActionBase> list = new List<ActionBasic.ActionBase>();
        private int count;
        private int index;

        public void AddAction(ActionBasic.ActionBase action)
        {
            this.list.Add(action);
            this.count++;
        }

        protected override void OnForceEnd()
        {
            if (this.index < this.count)
            {
                this.list[this.index].ForceEnd();
            }
        }

        protected override void OnInit()
        {
            this.index = 0;
        }

        protected override void OnUpdate()
        {
            if (this.index < this.count)
            {
                ActionBasic.ActionBase base2 = this.list[this.index];
                base2.Init();
                base2.Update();
                if (this.list[this.index].IsEnd)
                {
                    this.index++;
                }
            }
            else
            {
                base.End();
            }
        }
    }

    public class ActionWaitRandom : ActionBasic.ActionBase
    {
        public int min;
        public int max;
        private float startTime;
        private float waitTime;

        private int GetRandomInt(int min, int max) => 
            GameLogic.Random(min, max);

        protected override void OnForceEnd()
        {
        }

        protected override void OnInit()
        {
            this.startTime = Updater.AliveTime;
            this.waitTime = ((float) this.GetRandomInt(this.min, this.max)) / 1000f;
        }

        protected override void OnUpdate()
        {
            if ((Updater.AliveTime - this.startTime) >= this.waitTime)
            {
                base.End();
            }
        }
    }

    public class CallData
    {
        public int CallID;
        public int MaxAliveCount;
        public int MaxCount;
        public int perCount;
        public int radiusmin;
        public int radiusmax;
        public int CurAliveCount;
        public int CurAllCount;

        public CallData(int callid, int alivecount, int count, int percount, int radiusmin, int radiusmax)
        {
            this.CallID = callid;
            this.MaxAliveCount = alivecount;
            this.MaxCount = count;
            this.perCount = percount;
            this.radiusmin = radiusmin;
            this.radiusmax = radiusmax;
        }

        public void AddCall()
        {
            this.CurAllCount++;
            this.CurAliveCount++;
        }

        public int GetCallCount()
        {
            int perCount = this.perCount;
            if ((this.MaxCount - this.CurAllCount) < perCount)
            {
                perCount = this.MaxCount = this.CurAllCount;
            }
            if ((this.MaxAliveCount - this.CurAliveCount) < perCount)
            {
                perCount = this.MaxAliveCount - this.CurAliveCount;
            }
            return perCount;
        }

        public bool GetCanCall() => 
            ((this.CurAliveCount < this.MaxAliveCount) && (this.CurAllCount < this.MaxCount));

        public void RemoveCall()
        {
            this.CurAliveCount--;
        }
    }
}

