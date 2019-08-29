using DG.Tweening;
using Dxx.Util;
using System;
using UnityEngine;

public class SkillAlone1014 : SkillAloneBase
{
    private GameObject good;
    private ParticleSystem mParticle;
    private SkillAlone1014Ctrl ctrl = new SkillAlone1014Ctrl();
    private AutoDespawn mAutoDespawn;
    private Sequence seq;

    private void CreateSkillAlone()
    {
        object[] args = new object[] { "Game/SkillPrefab/SkillAlone", base.ClassID, "Effect" };
        this.good = GameLogic.EffectGet(Utils.GetString(args));
        this.good.transform.SetParent(GameNode.m_PoolParent);
        this.mAutoDespawn = this.good.GetComponent<AutoDespawn>();
        if (this.mAutoDespawn != null)
        {
            this.mAutoDespawn.enabled = false;
        }
        this.mParticle = this.good.GetComponentInChildren<ParticleSystem>();
        this.good.SetActive(false);
    }

    private void KillSequence()
    {
        if (this.seq != null)
        {
            TweenExtensions.Kill(this.seq, false);
            this.seq = null;
        }
    }

    private void OnGotoNextRoom(RoomGenerateBase.Room room)
    {
        this.good.transform.position = base.m_Entity.position;
        this.KillSequence();
        this.good.SetActive(false);
        this.mParticle.Clear();
        this.ctrl.RemoveGoods();
    }

    protected override void OnInstall()
    {
        this.CreateSkillAlone();
        this.ctrl.Init(base.m_Entity, this);
        ReleaseModeManager mode = GameLogic.Release.Mode;
        mode.OnGotoNextRoom = (Action<RoomGenerateBase.Room>) Delegate.Combine(mode.OnGotoNextRoom, new Action<RoomGenerateBase.Room>(this.OnGotoNextRoom));
        base.m_Entity.Event_PositionBy += new Action<Vector3>(this.OnPositionBy);
    }

    private void OnPositionBy(Vector3 p)
    {
        if (!this.good.activeSelf)
        {
            this.good.SetActive(true);
        }
        this.good.transform.position = base.m_Entity.position;
    }

    protected override void OnUninstall()
    {
        ReleaseModeManager mode = GameLogic.Release.Mode;
        mode.OnGotoNextRoom = (Action<RoomGenerateBase.Room>) Delegate.Remove(mode.OnGotoNextRoom, new Action<RoomGenerateBase.Room>(this.OnGotoNextRoom));
        base.m_Entity.Event_PositionBy -= new Action<Vector3>(this.OnPositionBy);
        this.ctrl.DeInit();
        this.mAutoDespawn = this.good.GetComponent<AutoDespawn>();
        if (this.mAutoDespawn == null)
        {
            this.mAutoDespawn = this.good.AddComponent<AutoDespawn>();
        }
        this.mAutoDespawn.SetDespawnTime(5f);
        this.mAutoDespawn.enabled = true;
        this.KillSequence();
    }
}

