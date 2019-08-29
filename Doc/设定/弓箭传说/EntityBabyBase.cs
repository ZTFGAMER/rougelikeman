using Dxx.Util;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TableTool;
using UnityEngine;

public class EntityBabyBase : EntityCallBase
{
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private Character_Baby <m_BabyData>k__BackingField;
    protected ActionBasic action = new ActionBasic();

    protected virtual void InitAttackControl()
    {
        base.m_AttackCtrl = new AttackControl();
    }

    protected override void OnDeInitLogic()
    {
        ReleaseModeManager mode = GameLogic.Release.Mode;
        mode.OnGotoNextRoom = (Action<RoomGenerateBase.Room>) Delegate.Remove(mode.OnGotoNextRoom, new Action<RoomGenerateBase.Room>(this.OnGotoNextRoom));
        base.OnDeInitLogic();
    }

    protected virtual void OnGotoNextRoom(RoomGenerateBase.Room room)
    {
        float angle = GameLogic.Random((float) -80f, (float) 80f);
        float x = MathDxx.Sin(angle);
        float z = MathDxx.Cos(angle);
        base.transform.position = base.m_Parent.position + (new Vector3(x, 0f, z) * GameLogic.Random((float) 0f, (float) 1f));
    }

    protected override void OnInit()
    {
        base.OnInit();
        this.m_BabyData = LocalModelManager.Instance.Character_Baby.GetBeanById(this.ClassID.ToString());
        base.m_MoveCtrl = new MoveControl();
        this.InitAttackControl();
        base.m_MoveCtrl.Init(this);
        base.m_AttackCtrl.Init(this);
        GameLogic.Release.Entity.SetBaby(this);
        this.UpdateAttributes();
        bool babyResistBullet = base.m_Parent.m_EntityData.GetBabyResistBullet();
        this.SetCollider(babyResistBullet);
        base.ShowHP(false);
        base.RemoveColliders();
        ReleaseModeManager mode = GameLogic.Release.Mode;
        mode.OnGotoNextRoom = (Action<RoomGenerateBase.Room>) Delegate.Combine(mode.OnGotoNextRoom, new Action<RoomGenerateBase.Room>(this.OnGotoNextRoom));
    }

    private void RemoveAlreadyAddAttributes()
    {
        int num = 0;
        int count = base.m_EntityData.mSelfAttributes.Count;
        while (num < count)
        {
            string str = base.m_EntityData.mSelfAttributes[num].Replace("+", "-");
            base.m_EntityData.ExcuteAttributes(str);
            num++;
        }
        base.m_EntityData.mSelfAttributes.Clear();
    }

    public override bool SetHitted(HittedData data) => 
        false;

    public void UpdateAttributes()
    {
        this.RemoveAlreadyAddAttributes();
        int num = 0;
        int count = base.m_Parent.m_EntityData.mBabyAttributes.Count;
        while (num < count)
        {
            string item = base.m_Parent.m_EntityData.mBabyAttributes[num];
            base.m_EntityData.mSelfAttributes.Add(item);
            base.m_EntityData.ExcuteAttributes(item);
            num++;
        }
    }

    protected override void UpdateFixed()
    {
        if (!base.GetIsDead())
        {
            base.m_MoveCtrl.UpdateProgress();
        }
    }

    protected override void UpdateProcess(float delta)
    {
        base.UpdateProcess(delta);
        if (!base.GetIsDead())
        {
            base.m_AttackCtrl.UpdateProgress();
        }
    }

    public void UpdateSkillIds()
    {
        int num = 0;
        int count = base.m_Parent.m_EntityData.mBabySkillIds.Count;
        while (num < count)
        {
            int skillId = base.m_Parent.m_EntityData.mBabySkillIds[num];
            base.AddSkill(skillId, Array.Empty<object>());
            num++;
        }
    }

    protected sealed override string ModelPath =>
        "Game/Baby/Baby";

    public Character_Baby m_BabyData { get; private set; }
}

