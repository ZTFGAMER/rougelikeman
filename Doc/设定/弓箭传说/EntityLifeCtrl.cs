using Dxx.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EntityLifeCtrl : EntityCtrlBase
{
    public override void ExcuteCommend(EBattleAction id, object action)
    {
        if (id != EBattleAction.EBattle_Action_Hitted_Once)
        {
            if (id == EBattleAction.EBattle_Action_Dead_Before)
            {
                this.OnEntityDeadBefore((BattleStruct.DeadStruct) action);
            }
            else if (id == EBattleAction.EBattle_Action_Dead)
            {
                this.OnEntityDead((BattleStruct.DeadStruct) action);
            }
        }
        else
        {
            this.OnEntityHittedOnce((HitStruct) action);
        }
    }

    private void OnEntityDead(BattleStruct.DeadStruct data)
    {
    }

    private void OnEntityDeadBefore(BattleStruct.DeadStruct data)
    {
        base.m_Entity.DeadBefore();
    }

    private void OnEntityHittedOnce(HitStruct data)
    {
        if ((((base.m_Entity != null) && !base.m_Entity.GetIsDead()) && (base.m_Entity.Type != EntityType.Baby)) && (!base.m_Entity.m_EntityData.GetCanShieldCount() || (data.before_hit >= 0L)))
        {
            bool bulletthrough = false;
            float bulletangle = 0f;
            if ((data.bulletdata != null) && (data.bulletdata.weapon != null))
            {
                bulletthrough = data.bulletdata.weapon.bThroughEntity;
            }
            if ((data.bulletdata != null) && (data.bulletdata.bullet != null))
            {
                bulletangle = data.bulletdata.bullet.transform.eulerAngles.y;
            }
            HittedData hittedData = base.m_Entity.GetHittedData(bulletthrough, bulletangle);
            if (data.type == HitType.Rebound)
            {
                data.real_hit = data.before_hit;
            }
            else if (data.before_hit >= 0L)
            {
                data.real_hit = data.before_hit;
            }
            else
            {
                if (!hittedData.GetCanHitted())
                {
                    return;
                }
                switch (data.sourcetype)
                {
                    case HitSourceType.eBullet:
                        if (((base.m_Entity != null) && (data.bulletdata != null)) && (data.bulletdata.weapon != null))
                        {
                            float num2 = data.before_hit;
                            num2 *= hittedData.hitratio;
                            if (data.source != null)
                            {
                                float num4 = Vector3.Distance(base.m_Entity.position, data.source.position) / ((float) data.source.m_EntityData.attribute.DistanceAttackValueDis.Value);
                                if (num4 < 1f)
                                {
                                    float num5 = (1f - num4) * data.source.m_EntityData.attribute.DistanceAttackValuePercent.Value;
                                    num2 *= 1f + num5;
                                }
                            }
                            data.before_hit = (long) num2;
                            data = base.m_Entity.m_EntityData.GetHurt(data);
                            if (data.type != HitType.Rebound)
                            {
                                bool headShot = false;
                                if (!headShot)
                                {
                                    headShot = base.m_Entity.m_EntityData.GetHeadShot();
                                }
                                if (((!headShot && (base.m_Entity.Type != EntityType.Boss)) && ((data.source != null) && (base.m_Entity.m_EntityData.GetHPPercent() < data.source.m_EntityData.attribute.KillMonsterLessHP.Value))) && (GameLogic.Random((float) 0f, (float) 1f) < data.source.m_EntityData.attribute.KillMonsterLessHPRatio.Value))
                                {
                                    headShot = true;
                                }
                                if (headShot)
                                {
                                    data.real_hit = -9223372036854775807L;
                                    data.type = HitType.HeadShot;
                                }
                                if (data.source != null)
                                {
                                    data.source.m_EntityData.ExcuteHitAdd();
                                }
                            }
                            break;
                        }
                        return;

                    case HitSourceType.eTrap:
                        data = base.m_Entity.m_EntityData.GetHurt(data);
                        break;

                    case HitSourceType.eBody:
                        data = base.m_Entity.m_EntityData.GetHurt(data);
                        break;

                    case HitSourceType.eBuff:
                        data = base.m_Entity.m_EntityData.GetHurt(data);
                        break;

                    case HitSourceType.eSkill:
                        data = base.m_Entity.m_EntityData.GetHurt(data);
                        break;
                }
            }
            if (data.real_hit == 0L)
            {
                if (data.type == HitType.Miss)
                {
                    GameLogic.CreateHPChanger(data.source, base.m_Entity, data);
                }
            }
            else
            {
                if (data.real_hit < 0f)
                {
                    if (((data.sourcetype == HitSourceType.eBullet) && (data.bulletdata != null)) && ((data.bulletdata.bullet != null) && (data.bulletdata.weapon != null)))
                    {
                        hittedData.AddBackRatio(data.bulletdata.weapon.BackRatio);
                        hittedData.AddBackRatio(base.m_Entity.m_Data.BackRatio);
                        hittedData.SetBullet(data.bulletdata.bullet);
                        hittedData.hittype = data.type;
                        base.m_Entity.SetHitted(hittedData);
                    }
                    if ((((data.sourcetype == HitSourceType.eBullet) || (data.sourcetype == HitSourceType.eTrap)) || (data.sourcetype == HitSourceType.eBody)) && (GameLogic.Hold.BattleData.Challenge_ismainchallenge() && base.m_Entity.IsSelf))
                    {
                        GameLogic.Hold.BattleData.AddHittedCount(GameLogic.Release.Mode.RoomGenerate.GetCurrentRoomID());
                    }
                    long shieldHitValue = base.m_Entity.m_EntityData.GetShieldHitValue(-data.real_hit);
                    data.real_hit += shieldHitValue;
                    if (base.m_Entity.OnHitted != null)
                    {
                        base.m_Entity.OnHitted(data.source, data.real_hit);
                    }
                    if (((data.sourcetype == HitSourceType.eBullet) || (data.sourcetype == HitSourceType.eBody)) && (((data.source != null) && (data.real_hit < 0L)) && (data.type != HitType.Rebound)))
                    {
                        int num7 = 0;
                        if (base.m_Entity.m_EntityData.attribute.ReboundHit.Value > 0L)
                        {
                            num7 += (int) base.m_Entity.m_EntityData.attribute.ReboundHit.Value;
                        }
                        if ((base.m_Entity.m_EntityData.attribute.ReboundTargetPercent.Value > 0f) && (data.source.Type != EntityType.Boss))
                        {
                            num7 += (int) (data.source.m_EntityData.MaxHP * base.m_Entity.m_EntityData.attribute.ReboundTargetPercent.Value);
                        }
                        if (num7 > 0)
                        {
                            data.before_hit = -num7;
                            GameLogic.SendHit_Rebound(data.source, base.m_Entity, data);
                        }
                    }
                }
                if ((base.m_Entity.IsSelf && (data.real_hit < 0f)) && ((base.m_Entity.m_EntityData.mDeadRecover > 0) && (base.m_Entity.m_EntityData.CurrentHP <= 10L)))
                {
                    base.m_Entity.m_EntityData.UseDeadRecover();
                }
                else
                {
                    if ((GameLogic.Hold.BattleData.Challenge_RecoverHP() && (data.real_hit > 0L)) || (data.real_hit < 0L))
                    {
                        GameLogic.CreateHPChanger(data.source, base.m_Entity, data);
                    }
                    if (((data.type == HitType.Crit) && (data.source != null)) && (data.source.OnCrit != null))
                    {
                        data.source.OnCrit(MathDxx.Abs(data.real_hit));
                    }
                    if (((data.sourcetype == HitSourceType.eBullet) || (data.sourcetype == HitSourceType.eBody)) || ((data.sourcetype == HitSourceType.eSkill) || (data.sourcetype == HitSourceType.eTrap)))
                    {
                        if (data.real_hit < 0L)
                        {
                            base.m_Entity.PlayEffect(base.m_Entity.m_Data.HittedEffectID);
                        }
                        if ((data.real_hit < 0L) && (data.bulletdata != null))
                        {
                            if ((((data.source != null) && data.source.m_EntityData.GetLight45()) && ((data.bulletdata != null) && (data.bulletdata.bullet != null))) && !data.bulletdata.bullet.GetLight45())
                            {
                                if (data.source.OnLight45 != null)
                                {
                                    data.source.OnLight45(base.m_Entity);
                                }
                            }
                            else
                            {
                                base.m_Entity.PlayEffect(data.bulletdata.weapon.HittedEffectID, base.m_Entity.m_Body.EffectMask.transform.position, Quaternion.Euler(0f, 90f - Utils.getAngle(base.m_Entity.GetHittedDirection()), 0f));
                            }
                        }
                        base.m_Entity.PlaySound(data.soundid);
                    }
                    if (data.buffid > 0)
                    {
                        base.m_Entity.ChangeHPMust(data.source, data.real_hit);
                    }
                    else
                    {
                        base.m_Entity.ChangeHP(data.source, data.real_hit);
                    }
                }
            }
        }
    }

    public override void OnStart(List<EBattleAction> actIds)
    {
        actIds.Add(EBattleAction.EBattle_Action_Hitted_Once);
        actIds.Add(EBattleAction.EBattle_Action_Dead_Before);
        actIds.Add(EBattleAction.EBattle_Action_Dead);
    }
}

