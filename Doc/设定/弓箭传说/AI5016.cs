using Dxx.Util;
using System;
using UnityEngine;

public class AI5016 : AIBase
{
    private AIBase.CallData[] calls = new AIBase.CallData[] { new AIBase.CallData(0xbd1, 1, 0x7fffffff, 1, 2, 3), new AIBase.CallData(0xbd3, 1, 0x7fffffff, 1, 2, 3) };
    private float[] hplimit = new float[] { 0.2f, 0.5f };
    private bool[] hpused = new bool[2];
    private float recoverhp = 0.25f;
    private WeightRandomCount weight = new WeightRandomCount(1, 4);
    private int callid;
    private int attackcount;

    private void AddFirst()
    {
        base.AddAction(base.GetActionDelegate(delegate {
            base.m_Entity.m_AniCtrl.SetString("Skill", "Begin");
            base.m_Entity.m_AniCtrl.SendEvent("Skill", false);
        }));
        base.AddAction(base.GetActionWait(string.Empty, 0x708));
    }

    private void JumpAction()
    {
        base.AddAction(base.GetActionWait(string.Empty, 200));
        base.AddAction(base.GetActionDelegate(delegate {
            base.m_Entity.m_AniCtrl.SetString("Skill", "Jump");
            base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("Skill", 1f);
            base.m_Entity.m_AniCtrl.SendEvent("Skill", false);
        }));
        base.AddAction(base.GetActionWait(string.Empty, 400));
        base.AddAction(base.GetActionDelegate(delegate {
            Vector3 position = base.m_Entity.position;
            for (int i = 0; i < 4; i++)
            {
                float num = i * 90f;
                for (int j = 0; j < 3; j++)
                {
                    float angle = num + GameLogic.Random((float) 0f, (float) 90f);
                    float x = MathDxx.Sin(angle);
                    float z = MathDxx.Cos(angle);
                    GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 0x13aa, position + new Vector3(x, 0.5f, z), angle);
                }
            }
        }));
        base.AddAction(base.GetActionDelegate(() => base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("Skill", -1f)));
    }

    protected override void OnAIDeInit()
    {
    }

    protected override void OnInit()
    {
        switch (this.weight.GetRandom())
        {
            case 0:
                this.callid = this.calls[GameLogic.Random(0, 2)].CallID;
                if (base.GetCanCall(this.callid))
                {
                    base.AddAction(base.GetActionCall(this.callid));
                }
                break;

            case 1:
                base.AddAction(new AIMove1022(base.m_Entity, 15f));
                base.AddAction(base.GetActionAttackWait(0x13a8, 200, 200));
                break;

            case 2:
                base.AddAction(new AIMove1022(base.m_Entity, 12f));
                base.AddAction(base.GetActionAttackWait(0x13a9, 200, 200));
                break;

            case 3:
                this.JumpAction();
                this.JumpAction();
                this.JumpAction();
                base.AddAction(base.GetActionWait(string.Empty, 0x3e8));
                break;
        }
        this.RecoverHPAction();
        base.bReRandom = true;
    }

    protected override void OnInitOnce()
    {
        base.IsDelayTime = false;
        base.m_Entity.mAniCtrlBase.SetDontPlayHittedAction();
        for (int i = 0; i < this.calls.Length; i++)
        {
            base.InitCallData(this.calls[i]);
        }
        base.m_Entity.AddSkill(0x10c8ed, Array.Empty<object>());
        this.AddFirst();
    }

    private void RecoverHPAction()
    {
        int index = 0;
        int length = this.hplimit.Length;
        while (index < length)
        {
            if (!this.hpused[index] && (base.m_Entity.m_EntityData.GetHPPercent() <= this.hplimit[index]))
            {
                this.hpused[index] = true;
                base.AddAction(base.GetActionDelegate(delegate {
                    base.m_Entity.m_AniCtrl.SetString("Continuous", "Begin");
                    base.m_Entity.mAniCtrlBase.SetAnimationRevert("Continuous", true);
                    base.m_Entity.m_AniCtrl.SendEvent("Continuous", false);
                    base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("Continuous", 1f);
                }));
                base.AddAction(base.GetActionWait(string.Empty, 850));
                base.AddAction(base.GetActionDelegate(() => GameLogic.SendBuff(base.m_Entity, base.m_Entity, 0x412, Array.Empty<float>())));
                base.AddAction(base.GetActionWait(string.Empty, 0x7d0));
                base.AddAction(base.GetActionDelegate(delegate {
                    base.m_Entity.mAniCtrlBase.SetAnimationRevert("Continuous", false);
                    base.m_Entity.m_AniCtrl.SendEvent("Idle", true);
                    base.m_Entity.m_AniCtrl.SendEvent("Continuous", false);
                }));
                base.AddAction(base.GetActionWait(string.Empty, 850));
                base.AddAction(base.GetActionDelegate(delegate {
                    base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("Continuous", -1f);
                    base.m_Entity.m_AniCtrl.SendEvent("Idle", true);
                }));
                break;
            }
            index++;
        }
    }
}

