using DG.Tweening;
using Dxx.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TableTool;
using UnityEngine;

public class EntityMonsterBase : EntityCallBase
{
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private Soldier_soldier <m_SoldierData>k__BackingField;
    private List<DropRandomData> mDropList = new List<DropRandomData>();
    private bool bDeadDown;
    private DropBase mDrop;
    private SequencePool mSequencePool = new SequencePool();
    private const float HittedMax = 30f;
    private float HittedReal;
    private int HittedBackIndex;
    private EntityBase triggerentity;
    private float triggertime;
    private bool isinTrigger;

    protected override void CollisionEnterExtra(Collision o)
    {
        this.TriggerEnter(o.gameObject);
    }

    protected override void CollisionExitExtra(Collision o)
    {
        this.TriggerExit(o.gameObject);
    }

    private void CreateDeadGoods(List<BattleDropData> list)
    {
        if (list.Count > 0)
        {
            GameLogic.Release.Mode.CreateGoods(base.position, list, this.m_SoldierData.DropRadius);
        }
    }

    private void CreateGoods()
    {
        this.OnCreateDeadGoods();
        this.CreateDeadGoods(this.OnGetGoodList());
    }

    public override void DeadCallBack()
    {
        if (base.m_Data.Divide != 0)
        {
            base.m_AniCtrl.DeadDown();
            base.DeInit();
        }
        else
        {
            base.DeadCallBack();
        }
    }

    private void ExcuteSoldierUp()
    {
        if (GameLogic.Hold.BattleData.mModeData != null)
        {
            string[] monsterTmxAttributes = GameLogic.Hold.BattleData.mModeData.GetMonsterTmxAttributes();
            int index = 0;
            int length = monsterTmxAttributes.Length;
            while (index < length)
            {
                Goods_goods.GoodData goodData = Goods_goods.GetGoodData(monsterTmxAttributes[index]);
                if (base.IsElite && (goodData.goodType == "HPMax%"))
                {
                    goodData.value *= 2L;
                    goodData.value += 0x2710L;
                }
                base.m_EntityData.ExcuteAttributes(goodData);
                index++;
            }
        }
    }

    protected override long GetBossHP() => 
        base.m_EntityData.MaxHP;

    private void HitEntity(EntityBase e)
    {
        if ((!base.GetIsDead() && ((this.triggerentity == null) || !this.triggerentity.GetIsDead())) && base.GetColliderEnable())
        {
            int num = -base.m_EntityData.GetBodyHit();
            GameLogic.SendHit_Body(e, this, (long) num, this.m_SoldierData.BodyHitSoundID);
            this.OnHitEntity(e);
        }
    }

    protected void InitDivideID()
    {
        base.DivideID = base.GetInstanceID().ToString();
    }

    protected override void OnChangeHP(EntityBase entity, long HP)
    {
        List<BattleDropData> hittedList = this.mDrop.GetHittedList(HP);
        if ((hittedList != null) && (hittedList.Count > 0))
        {
            this.CreateDeadGoods(hittedList);
        }
        if (base.GetIsDead())
        {
            if (base.Type == EntityType.Boss)
            {
                GameLogic.Hold.BattleData.AddKillBoss(base.m_Data.CharID);
            }
            else
            {
                GameLogic.Hold.BattleData.AddKillMonsters(base.m_Data.CharID);
            }
            this.TriggerEnd();
            if (!base.bCall)
            {
            }
            if ((GameLogic.Release.Mode != null) && (GameLogic.Release.Mode.RoomGenerate != null))
            {
                GameLogic.Release.Mode.RoomGenerate.CheckOpenDoor();
            }
            bool flag = false;
            EntityType type = base.Type;
            if (base.m_Data.Divide == 0)
            {
                flag = true;
            }
            if (base.DivideID != string.Empty)
            {
                GameLogic.Release.Entity.RemoveDivide(base.DivideID);
            }
            if (flag)
            {
                this.CreateGoods();
            }
            this.RemoveMove();
        }
    }

    protected virtual void OnCreateDeadGoods()
    {
    }

    protected override void OnCreateModel()
    {
        base.OnCreateModel();
        this.ExcuteSoldierUp();
    }

    protected override void OnDeInitLogic()
    {
        this.mSequencePool.Clear();
        this.TriggerEnd();
        base.OnDeInitLogic();
    }

    protected override List<BattleDropData> OnGetGoodList() => 
        this.goodsList;

    protected virtual void OnHitEntity(EntityBase e)
    {
    }

    protected override void OnInit()
    {
        base.OnInit();
        base.m_MoveCtrl = new MoveControl();
        base.m_AttackCtrl = new AttackControl();
        this.m_SoldierData = LocalModelManager.Instance.Soldier_soldier.GetBeanById(base.ClassID);
        object[] args = new object[] { "Soldier_soldier dont have ", base.ClassID, " monster" };
        SdkManager.Bugly_Report(this.m_SoldierData != null, "EntityMonsterBase.cs", Utils.GetString(args));
        base.m_MoveCtrl.Init(this);
        base.m_AttackCtrl.Init(this);
        GameLogic.Release.Entity.Add(this);
        base.m_AttackCtrl.SetRotate(180f);
        this.SetCollider(false);
    }

    protected override void OnInitBefore()
    {
        base.OnInitBefore();
        if (base.IsElite)
        {
            base.HPSliderName = "HPSlider_Elite";
        }
        else
        {
            base.HPSliderName = "HPSlider_Monster";
        }
    }

    protected override void OnTriggerEnterExtra(Collider o)
    {
        if (base.GetColliderTrigger())
        {
            this.TriggerEnter(o.gameObject);
        }
    }

    protected override void OnTriggerExitExtra(Collider o)
    {
        if (base.GetColliderTrigger())
        {
            this.TriggerExit(o.gameObject);
        }
    }

    private void OnTriggerUpdate()
    {
        if (this.isinTrigger)
        {
            if ((base.GetIsDead() || ((this.triggerentity != null) && this.triggerentity.GetIsDead())) || !base.GetMeshShow())
            {
                this.TriggerEnd();
            }
            else if ((this.triggerentity != null) && ((Updater.AliveTime - this.triggertime) > 1.2f))
            {
                this.triggertime = Updater.AliveTime;
                if (this.triggerentity != null)
                {
                    this.HitEntity(this.triggerentity);
                }
            }
        }
    }

    public override bool SetHitted(HittedData data)
    {
        bool flag = base.SetHitted(data);
        if (flag && (data.backtatio > 0f))
        {
            this.StartHittedBack(data.backtatio);
        }
        return flag;
    }

    public void StartCall()
    {
        base.ShowHP(false);
        Sequence seq = TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(DOTween.Sequence(), 0.3f), new TweenCallback(this, this.<StartCall>m__0));
        this.mSequencePool.Add(seq);
    }

    private void StartHittedBack(float backRatio)
    {
        if (base.m_State == EntityState.Normal)
        {
            this.HittedReal = 30f * backRatio;
            this.HittedBackIndex = this.HittedArgsLength;
            base.m_State = EntityState.Hitted;
        }
    }

    protected override void StartInit()
    {
        base.StartInit();
        this.SetCollider(true);
        switch (GameLogic.Hold.BattleData.GetMode())
        {
            case GameMode.eChallenge101:
            case GameMode.eChallenge102:
            case GameMode.eChallenge103:
            case GameMode.eChallenge104:
                this.mDrop = new DropChallenge101();
                break;

            case GameMode.eGold1:
                this.mDrop = new DropGold();
                break;

            default:
                this.mDrop = new DropDefault();
                break;
        }
        this.mDrop.Init(this.m_SoldierData, base.m_EntityData.MaxHP);
    }

    private void TriggerEnd()
    {
        this.isinTrigger = false;
    }

    private void TriggerEnter(GameObject o)
    {
        if ((o.layer == LayerManager.Player) || (o.layer == LayerManager.Fly))
        {
            EntityBase entityByChild = GameLogic.Release.Entity.GetEntityByChild(o.gameObject);
            if (!GameLogic.IsSameTeam(this, entityByChild))
            {
                this.triggerentity = entityByChild;
                this.TriggerStart();
            }
        }
    }

    private void TriggerExit(GameObject o)
    {
        if ((this.triggerentity != null) && (o == this.triggerentity.gameObject))
        {
            this.TriggerEnd();
        }
    }

    private void TriggerStart()
    {
        this.isinTrigger = true;
    }

    protected override void UpdateFixed()
    {
        this.UpdateHittedBack();
        if (!base.GetIsDead())
        {
            base.m_MoveCtrl.UpdateProgress();
        }
    }

    private void UpdateHittedBack()
    {
        if (base.m_State == EntityState.Hitted)
        {
            if (this.HittedBackIndex < 0)
            {
                base.HittedV = 0f;
                base.m_MoveCtrl.ResetRigidBody();
                base.m_State = EntityState.Normal;
            }
            else
            {
                base.HittedV = (this.HittedReal * this.HittedBackIndex) / ((float) this.HittedArgsLength);
                this.HittedBackIndex--;
            }
        }
    }

    protected override void UpdateProcess(float delta)
    {
        base.UpdateProcess(delta);
        if (!base.GetIsDead())
        {
            base.m_AttackCtrl.UpdateProgress();
            this.OnTriggerUpdate();
        }
    }

    protected override string ModelPath =>
        "Game/Soldier/Soldier";

    public Soldier_soldier m_SoldierData { get; protected set; }

    private int HittedArgsLength =>
        3;

    protected List<BattleDropData> goodsList
    {
        get
        {
            List<BattleDropData> expList = new List<BattleDropData>();
            if (GameLogic.Release.Game.RoomState == RoomState.Runing)
            {
                if (base.bCall)
                {
                    return expList;
                }
                if (GameLogic.Hold.BattleData.Challenge_DropExp())
                {
                    int exp = (int) (this.m_SoldierData.Exp * (1f + base.m_EntityData.attribute.Monster_ExpPercent.Value));
                    expList = GameLogic.GetExpList(exp);
                }
                List<BattleDropData> dropDead = this.mDrop.GetDropDead();
                expList.AddRange(dropDead);
                float num2 = 1f;
                if (!GameLogic.Hold.BattleData.Challenge_RecoverHP())
                {
                    return expList;
                }
                BattleDropData item = new BattleDropData(FoodType.eHP, FoodOneType.eHP0020, 0);
                if (GameLogic.Random((float) 0f, (float) 100f) < ((this.m_SoldierData.HPDrop1 * num2) * (1f + base.m_EntityData.attribute.Monster_HPDrop.Value)))
                {
                    expList.Add(item);
                }
                if (GameLogic.Random((float) 0f, (float) 100f) < ((this.m_SoldierData.HPDrop2 * num2) * (1f + base.m_EntityData.attribute.Monster_HPDrop.Value)))
                {
                    expList.Add(item);
                }
                if (GameLogic.Random((float) 0f, (float) 100f) < ((this.m_SoldierData.HPDrop3 * num2) * (1f + base.m_EntityData.attribute.Monster_HPDrop.Value)))
                {
                    expList.Add(item);
                }
            }
            return expList;
        }
    }

    public class DropData
    {
        public int GoodID;
        public int Weight;
    }

    public class DropRandomData
    {
        private int DropWeight;
        private List<EntityMonsterBase.DropData> mDropList = new List<EntityMonsterBase.DropData>();

        public int GetRandom()
        {
            int num = GameLogic.Random(0, this.DropWeight);
            int num2 = 0;
            int count = this.mDropList.Count;
            while (num2 < count)
            {
                EntityMonsterBase.DropData data = this.mDropList[num2];
                if (num < data.Weight)
                {
                    if (data.GoodID > 0)
                    {
                        return data.GoodID;
                    }
                    break;
                }
                num -= data.Weight;
                num2++;
            }
            return 0;
        }

        public void InitDrop(string[] s)
        {
            int index = 0;
            int length = s.Length;
            while (index < length)
            {
                string str = s[index];
                char[] separator = new char[] { ',' };
                string[] strArray = str.Split(separator);
                EntityMonsterBase.DropData item = new EntityMonsterBase.DropData {
                    GoodID = int.Parse(strArray[0]),
                    Weight = int.Parse(strArray[1])
                };
                this.mDropList.Add(item);
                this.DropWeight += item.Weight;
                index++;
            }
        }
    }
}

