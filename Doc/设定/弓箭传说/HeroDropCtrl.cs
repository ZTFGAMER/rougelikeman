using Dxx.Util;
using PureMVC.Patterns;
using System;
using TableTool;
using UnityEngine;

public class HeroDropCtrl
{
    private ActionBasic action = new ActionBasic();
    private bool bDropStart;
    private float mDropStartTime;
    private float mDropTime = 0.32f;
    private int frameCount;
    private float starty;
    private AnimationCurve curve;

    private void CreateSmoke()
    {
        GameObject obj2 = GameLogic.EffectGet("Effect/Smoke/Smoke");
        if (obj2 != null)
        {
            obj2.transform.position = GameLogic.Self.position;
        }
        GameObject obj3 = GameLogic.EffectGet("Game/Player/wave");
        if (obj3 != null)
        {
            obj3.transform.position = GameLogic.Self.position;
        }
    }

    public void DeInit()
    {
        Updater.RemoveUpdate("HeroDropCtrl", new Action<float>(this.OnUpdate));
        this.action.DeInit();
    }

    private void HeroDroping()
    {
        if (this.bDropStart && (this.frameCount <= (Time.frameCount - 3)))
        {
            this.mDropStartTime += Updater.delta;
            float num = this.mDropStartTime / this.mDropTime;
            num = MathDxx.Clamp01(num);
            GameLogic.Self.SetPosition(new Vector3(GameLogic.Self.position.x, this.starty * this.curve.Evaluate(num), GameLogic.Self.position.z));
            if ((num == 1f) && (GameLogic.Self.position.y <= 0f))
            {
                GameLogic.Self.SetPosition(new Vector3(GameLogic.Self.position.x, 0f, GameLogic.Self.position.z));
                this.bDropStart = false;
                this.CreateSmoke();
                GameLogic.Hold.Sound.PlayBattleSpecial(0x4c4b45, Vector3.zero);
                GameNode.CameraShake(CameraShakeType.FirstDrop);
                this.action.AddActionWaitDelegate(0.6f, delegate {
                    GameLogic.Release.Game.ShowJoy(true);
                    GameLogic.Release.Game.SetRunning();
                    this.DeInit();
                    if (GameLogic.Hold.BattleData.GetMode() == GameMode.eLevel)
                    {
                        ChooseSkillProxy.Transfer transfer;
                        if (LocalSave.Instance.BattleIn_GetIn())
                        {
                            if (LocalSave.Instance.BattleIn_GetLevelUpSkills() != null)
                            {
                                transfer = new ChooseSkillProxy.Transfer {
                                    type = (ChooseSkillProxy.ChooseSkillType) LocalSave.Instance.BattleIn_GetLevelUpType()
                                };
                                Facade.Instance.RegisterProxy(new ChooseSkillProxy(transfer));
                                WindowUI.ShowWindow(WindowID.WindowID_ChooseSkill);
                            }
                        }
                        else if (GameLogic.Self.m_EntityData.attribute.ExtraSkill.Value > 0L)
                        {
                            transfer = new ChooseSkillProxy.Transfer {
                                type = ChooseSkillProxy.ChooseSkillType.eFirst
                            };
                            Facade.Instance.RegisterProxy(new ChooseSkillProxy(transfer));
                            WindowUI.ShowWindow(WindowID.WindowID_ChooseSkill);
                        }
                    }
                    LocalSave.Instance.BattleIn_UpdateIn();
                });
            }
        }
    }

    public void Init()
    {
        this.curve = LocalModelManager.Instance.Curve_curve.GetCurve(0x186a7);
        Updater.AddUpdate("HeroDropCtrl", new Action<float>(this.OnUpdate), false);
        this.action.Init(false);
    }

    private void OnUpdate(float delta)
    {
        this.HeroDroping();
    }

    public void StartDrop()
    {
        ActionBasic.ActionDelegate action = new ActionBasic.ActionDelegate {
            action = () => this.StartDrop1()
        };
        this.action.AddAction(action);
    }

    private void StartDrop1()
    {
        this.frameCount = Time.frameCount;
        this.bDropStart = true;
        this.mDropStartTime = -0.15f;
        this.starty = 32.5f;
        GameLogic.Self.SetPosition(new Vector3(GameLogic.Self.position.x, this.starty, GameLogic.Self.position.z));
    }
}

