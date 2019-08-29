using DG.Tweening;
using Dxx.Util;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TableTool;
using UnityEngine;

public class EntityHero : EntityBase
{
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private GameObject <FootDirection>k__BackingField;
    public Transform Coin_Absorb;
    private int mAbsorb = 1;
    private List<int> equipskills;
    private static Dictionary<int, int> AbsorbDic;
    private Dictionary<int, float> mAbsorbTimes = new Dictionary<int, float>();
    private float mAbsorbInterval = 0.3f;
    public List<Sequence> mSequenceList = new List<Sequence>();
    public List<EventMoveStartData> mMoveStartList = new List<EventMoveStartData>();
    public List<EventMovingData> mMovingList = new List<EventMovingData>();
    public List<EventMoveEndData> mMoveEndList = new List<EventMoveEndData>();
    private List<LevelUpData> mLevelUps = new List<LevelUpData>();
    private float mBubbleDistance;
    private bool bFrontShield;
    private List<Skill_slotin> mSkillList = new List<Skill_slotin>();
    private List<int> mLearnSkillList = new List<int>();
    private List<int> mExtraLearnSkillList = new List<int>();
    private int WeightAll;
    private SuperSkillBase mSuperSkill;
    [CompilerGenerated]
    private static Action <>f__am$cache0;
    [CompilerGenerated]
    private static Action <>f__am$cache1;
    [CompilerGenerated]
    private static Action <>f__am$cache2;

    static EntityHero()
    {
        Dictionary<int, int> dictionary = new Dictionary<int, int> {
            { 
                0xbb9,
                0x124f81
            },
            { 
                0xbba,
                0x124f81
            },
            { 
                0x7d1,
                0x124f84
            },
            { 
                0x7d2,
                0x124f84
            },
            { 
                0x7d3,
                0x124f84
            },
            { 
                0x7d4,
                0x124f84
            },
            { 
                0x3e9,
                0x124f82
            }
        };
        AbsorbDic = dictionary;
    }

    public void AbsorbEquips(EquipBase good)
    {
        GameObject obj2 = GameLogic.EffectGet("Effect/AbsorbExp");
        obj2.transform.SetParent(base.m_Body.EffectMask.transform);
        obj2.transform.localPosition = Vector3.zero;
        obj2.transform.localScale = Vector3.one;
        good.GetGoods(this);
        if (good.m_Data.GetSound != 0)
        {
            GameLogic.Hold.Sound.PlayGetGoods(good.m_Data.GetSound, base.transform.position);
        }
    }

    public void AbsorbFoods(FoodBase good)
    {
        int foodID;
        int num = 0x124f81;
        if (good is FoodEquipBase)
        {
            foodID = 0x270f;
            GameLogic.Hold.BattleData.AddEquip(good.GetData() as LocalSave.EquipOne);
            num = 0x124f83;
        }
        else
        {
            foodID = good.FoodID;
            AbsorbDic.TryGetValue(foodID, out num);
            good.GetGoods(this);
            if (((Time.time - this.GetAbsorbTime(foodID)) > this.mAbsorbInterval) && (good.m_Data.GetSound != 0))
            {
                GameLogic.Hold.Sound.PlayGetGoods(good.m_Data.GetSound, base.transform.position);
            }
        }
        if ((Time.time - this.GetAbsorbTime(foodID)) > this.mAbsorbInterval)
        {
            base.PlayEffect(num);
            this.UpdateAbsorbTime(foodID);
        }
    }

    private void Action_OngotoNextRoom()
    {
        int num = 0;
        int count = this.mSequenceList.Count;
        while (num < count)
        {
            Sequence sequence = this.mSequenceList[num];
            if (sequence != null)
            {
                TweenExtensions.Kill(sequence, false);
                sequence = null;
            }
            num++;
        }
        this.mSequenceList.Clear();
    }

    public void AddMoveEnd(EventMoveEndData data)
    {
        if (!this.mMoveEndList.Contains(data))
        {
            this.mMoveEndList.Add(data);
        }
    }

    public void AddMoveStart(EventMoveStartData data)
    {
        if (!this.mMoveStartList.Contains(data))
        {
            this.mMoveStartList.Add(data);
        }
    }

    public void AddMoving(EventMovingData data)
    {
        if (!this.mMovingList.Contains(data))
        {
            this.mMovingList.Add(data);
        }
    }

    private void CreateFootDirection()
    {
        GameObject obj2 = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>("Game/Player/FootDirection"));
        obj2.transform.parent = base.transform;
        obj2.transform.localPosition = Vector3.zero;
        obj2.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        obj2.transform.localScale = Vector3.one;
        this.FootDirection = obj2;
    }

    public override void DeadCallBack()
    {
        base.m_AniCtrl.SendEvent("Dead", false);
        if (base.mDeadSeq != null)
        {
            TweenExtensions.Kill(base.mDeadSeq, false);
            base.mDeadSeq = null;
        }
        base.mDeadSeq = TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(DOTween.Sequence(), 1.5f), new TweenCallback(this, this.<DeadCallBack>m__3));
    }

    private void DeInitSuperSkill()
    {
        ScrollCircle.OnDoubleClick = null;
        this.RemoveOldSuperSkill();
    }

    public void DoMoveEnd()
    {
        int num = 0;
        int count = this.mMoveEndList.Count;
        while (num < count)
        {
            <DoMoveEnd>c__AnonStorey3 storey = new <DoMoveEnd>c__AnonStorey3 {
                $this = this,
                index = num
            };
            float delay = this.mMoveEndList[storey.index].delay;
            if (delay <= 0f)
            {
                this.mMoveEndList[storey.index].mEvent();
            }
            else
            {
                Sequence item = TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(DOTween.Sequence(), delay), new TweenCallback(storey, this.<>m__0));
                this.mSequenceList.Add(item);
            }
            num++;
        }
    }

    public void DoMoveStart()
    {
        int num = 0;
        int count = this.mMoveStartList.Count;
        while (num < count)
        {
            <DoMoveStart>c__AnonStorey0 storey = new <DoMoveStart>c__AnonStorey0 {
                $this = this,
                index = num
            };
            float delay = this.mMoveStartList[storey.index].delay;
            if (delay <= 0f)
            {
                this.mMoveStartList[storey.index].mEvent();
            }
            else
            {
                Sequence item = TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(DOTween.Sequence(), delay), new TweenCallback(storey, this.<>m__0));
                this.mSequenceList.Add(item);
            }
            num++;
        }
    }

    public void DoMoving(JoyData data)
    {
        <DoMoving>c__AnonStorey2 storey = new <DoMoving>c__AnonStorey2 {
            data = data,
            $this = this
        };
        int num = 0;
        int count = this.mMovingList.Count;
        while (num < count)
        {
            <DoMoving>c__AnonStorey1 storey2 = new <DoMoving>c__AnonStorey1 {
                <>f__ref$2 = storey,
                index = num
            };
            float delay = this.mMovingList[storey2.index].delay;
            if (delay <= 0f)
            {
                this.mMovingList[storey2.index].mEvent(storey.data);
            }
            else
            {
                Sequence item = TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(DOTween.Sequence(), delay), new TweenCallback(storey2, this.<>m__0));
                this.mSequenceList.Add(item);
            }
            num++;
        }
    }

    public void DoReborn()
    {
        if (GameLogic.Hold.BattleData.GetCanReborn())
        {
            this.DoRebornInternal();
        }
        else
        {
            this.Reborn_DeadEnd();
        }
    }

    public void DoRebornInternal()
    {
        Singleton<MatchDefenceTimeSocketCtrl>.Instance.Send(MatchMessageType.eReborn, 0);
        GameLogic.Hold.BattleData.UseReborn();
        if (this.FootDirection != null)
        {
            this.FootDirection.SetActive(true);
        }
        GameLogic.Release.Game.JoyEnable(true);
        GameLogic.Hold.Sound.PlayWalk();
        base.m_EntityData.ChangeHP(null, base.m_EntityData.MaxHP);
        base.m_AniCtrl.Reborn();
        base.m_MoveCtrl.OnMoveEnd();
        base.m_AttackCtrl.Reset();
        base.ShowHP(true);
        this.SetCollider(true);
        GameLogic.SendBuff(this, this, 0x3eb, Array.Empty<float>());
        this.SetAbsorb(true);
        LocalSave.Instance.BattleIn_SetHaveBattle(true);
    }

    public void DoRunBubble(float dis)
    {
        this.mBubbleDistance += dis;
        if (this.mBubbleDistance >= 2f)
        {
            this.mBubbleDistance -= 2f;
            this.showRunBubble();
        }
    }

    public void ExcuteLevelUpAttributes(string name, long value)
    {
        LevelUpData item = new LevelUpData {
            name = name,
            value = value
        };
        this.mLevelUps.Add(item);
    }

    private void FellGround()
    {
        GameLogic.Hold.Sound.PlayMonsterSkill(0x4c4b44, base.position);
    }

    public bool GetAbsorbEnable() => 
        (this.mAbsorb > 0);

    private float GetAbsorbTime(int foodid)
    {
        if (this.mAbsorbTimes.TryGetValue(foodid, out float num))
        {
            return num;
        }
        return 0f;
    }

    public List<int> GetFirstSkill9()
    {
        int max = 0;
        IList<Skill_slotfirst> allBeans = LocalModelManager.Instance.Skill_slotfirst.GetAllBeans();
        List<Skill_slotin> list2 = new List<Skill_slotin>();
        List<int> list3 = new List<int>();
        int num2 = 0;
        int count = allBeans.Count;
        while (num2 < count)
        {
            int skillID = allBeans[num2].SkillID;
            Skill_slotin beanById = LocalModelManager.Instance.Skill_slotin.GetBeanById(skillID);
            list2.Add(beanById);
            max += beanById.Weight;
            num2++;
        }
        for (int i = 0; i < 9; i++)
        {
            int num6 = GameLogic.Random(0, max);
            int index = 0;
            int num8 = list2.Count;
            while (index < num8)
            {
                Skill_slotin _slotin2 = list2[index];
                if (num6 < _slotin2.Weight)
                {
                    list3.Add(_slotin2.SkillID);
                    max -= _slotin2.Weight;
                    list2.RemoveAt(index);
                    break;
                }
                num6 -= _slotin2.Weight;
                index++;
            }
        }
        object[] args = new object[] { "GetFirstSkill9 count = ", list3.Count };
        SdkManager.Bugly_Report(list3.Count == 9, "EntityHero_Skill.cs", Utils.GetString(args));
        return list3;
    }

    public int GetLearnSkillCount() => 
        this.mLearnSkillList.Count;

    public int GetRandomSkill()
    {
        int num = Random.Range(0, this.mSkillList.Count);
        return this.mSkillList[num].SkillID;
    }

    public List<int> GetSkill9()
    {
        List<Skill_slotin> list = new List<Skill_slotin>();
        List<int> list2 = new List<int>();
        int weightAll = this.WeightAll;
        for (int i = 0; i < 9; i++)
        {
            int num3 = GameLogic.Random(0, weightAll);
            int item = 0;
            int weight = 0;
            int index = 0;
            int num8 = this.mSkillList.Count;
            while (index < num8)
            {
                weight = this.mSkillList[index].Weight;
                if (num3 < weight)
                {
                    item = this.mSkillList[index].SkillID;
                    list.Add(this.mSkillList[index]);
                    list2.Add(item);
                    this.mSkillList.RemoveAt(index);
                    break;
                }
                num3 -= weight;
                index++;
            }
            weightAll -= weight;
        }
        int num9 = 0;
        int count = list.Count;
        while (num9 < count)
        {
            this.mSkillList.Add(list[num9]);
            num9++;
        }
        return list2;
    }

    private void InitCard(LocalSave.CardOne one)
    {
    }

    private void InitCards()
    {
        List<LocalSave.CardOne> wearCards = LocalSave.Instance.GetWearCards();
        int num = 0;
        int count = wearCards.Count;
        while (num < count)
        {
            this.InitCard(wearCards[num]);
            num++;
        }
    }

    protected override void InitCharacter()
    {
        base.m_EntityData.Init(this, base.m_Data.CharID);
    }

    protected void InitEquipSkills()
    {
        this.equipskills = LocalSave.Instance.Equip_GetSkills();
        int num = 0;
        int count = this.equipskills.Count;
        while (num < count)
        {
            base.AddSkillOverLying(this.equipskills[num], Array.Empty<object>());
            num++;
        }
    }

    private void InitSkillList()
    {
        int num = 0;
        int key = LocalSave.Instance.Equip_GetWeapon();
        Equip_equip beanById = LocalModelManager.Instance.Equip_equip.GetBeanById(key);
        if (beanById != null)
        {
            switch (beanById.Type)
            {
                case 0x65:
                    num = 0xf4264;
                    break;

                case 0x66:
                    num = 0xf4261;
                    break;

                case 0x67:
                    num = 0xf4260;
                    break;

                case 0x68:
                    num = 0xf425f;
                    break;
            }
        }
        IList<Skill_slotin> allBeans = LocalModelManager.Instance.Skill_slotin.GetAllBeans();
        this.WeightAll = 0;
        IEnumerator<Skill_slotin> enumerator = allBeans.GetEnumerator();
        int num3 = 0;
        while (enumerator.MoveNext())
        {
            Skill_slotin current = enumerator.Current;
            if (((current.SkillID != num) && !this.equipskills.Contains(current.SkillID)) && ((current.UnlockStage <= GameLogic.Hold.BattleData.Level_CurrentStage) && (current.Weight > 0)))
            {
                this.mSkillList.Add(current);
                this.WeightAll += current.Weight;
                num3++;
            }
        }
    }

    private void InitSuperSkill(int skillid)
    {
        ScrollCircle.OnDoubleClick = new Action(this.OnDoubleClick);
        this.RemoveOldSuperSkill();
        object[] args = new object[] { skillid };
        string typeName = Utils.FormatString("SuperSkill{0}", args);
        SuperSkillBase base2 = Type.GetType(typeName).Assembly.CreateInstance(typeName) as SuperSkillBase;
        this.mSuperSkill = base2;
        this.mSuperSkill.Init(this);
    }

    public void LearnExtraSkill(int skillid)
    {
        this.mExtraLearnSkillList.Add(skillid);
        this.OnLearnSkill(skillid);
    }

    public void LearnSkill(int skillid)
    {
        if ((GameLogic.Hold.BattleData.GetMode() == GameMode.eLevel) && (this.mExtraLearnSkillList.Count < GameLogic.Self.m_EntityData.attribute.ExtraSkill.Value))
        {
            this.LearnExtraSkill(skillid);
        }
        else
        {
            this.mLearnSkillList.Add(skillid);
            this.OnLearnSkill(skillid);
        }
    }

    public void LevelUp()
    {
        GameNode.CameraShake(CameraShakeType.Crit);
        base.PlayEffect(0x2dc6cc);
    }

    protected override void OnChangeHP(EntityBase entity, long HP)
    {
        if (base.GetIsDead())
        {
            this.Reborn_Dead();
        }
        LocalSave.Instance.BattleIn_UpdateHP(base.m_EntityData.CurrentHP);
        if ((GameLogic.Release.Mode != null) && (GameLogic.Release.Mode.RoomGenerate != null))
        {
            GameLogic.Release.Mode.RoomGenerate.PlayerHitted(HP);
        }
    }

    protected override void OnCreateModel()
    {
        bool flag = true;
        if (GameLogic.Hold.BattleData.GetMode() == GameMode.eBomberman)
        {
            flag = false;
        }
        if (flag)
        {
            this.CreateFootDirection();
        }
    }

    protected override void OnDeadBefore()
    {
    }

    protected override void OnDeInit()
    {
        this.Reborn_DeadEndInternal();
    }

    protected override void OnDeInitLogic()
    {
        this.DeInitSuperSkill();
        base.OnDeInitLogic();
    }

    private void OnDoubleClick()
    {
        if ((this.mSuperSkill != null) && this.mSuperSkill.CanUseSkill)
        {
            this.mSuperSkill.UseSkill();
        }
    }

    public void OnGotoNextRoom()
    {
        this.Action_OngotoNextRoom();
    }

    protected override HittedData OnHittedData(HittedData data, bool bulletthrough, float bulletangle)
    {
        this.bFrontShield = base.m_EntityData.GetFrontShield();
        if (this.bFrontShield)
        {
            if (bulletthrough)
            {
                return data;
            }
            float y = base.eulerAngles.y;
            if (((MathDxx.Abs((float) (y - bulletangle)) < 90f) || (MathDxx.Abs((float) ((y - bulletangle) + 360f)) < 90f)) || (MathDxx.Abs((float) ((y - bulletangle) - 360f)) < 90f))
            {
                return data;
            }
            data.type = EHittedType.eDefence;
            data.hitratio = 0.4f;
        }
        return data;
    }

    protected override void OnInit()
    {
        base.OnInit();
        base.m_MoveCtrl = new MoveControlHero();
        base.m_AttackCtrl = new HeroAttackControl();
        base.m_MoveCtrl.Init(this);
        base.m_AttackCtrl.Init(this);
        base.m_EntityData.HittedInterval = 0.5f;
        base.SetSuperArmor(true);
        this.Coin_Absorb = base.transform.Find("Coin_Absorb");
        base.m_AttackCtrl.SetRotate(0f);
        base.OnLevelUp = (Action<int>) Delegate.Combine(base.OnLevelUp, new Action<int>(this.OnLevelUpEvent));
    }

    protected override void OnInitBefore()
    {
        GameLogic.Release.Entity.SetSelf(this);
    }

    private void OnLearnSkill(int skillid)
    {
        Singleton<MatchDefenceTimeSocketCtrl>.Instance.Send(MatchMessageType.eLearnSkill, skillid);
        base.AddSkillInternal(skillid, Array.Empty<object>());
        int index = 0;
        int count = this.mSkillList.Count;
        while (index < count)
        {
            if (this.mSkillList[index].SkillID == skillid)
            {
                this.WeightAll -= this.mSkillList[index].Weight;
                this.mSkillList.RemoveAt(index);
                break;
            }
            index++;
        }
    }

    public void OnLevelUpEvent(int level)
    {
        int num = 0;
        int count = this.mLevelUps.Count;
        while (num < count)
        {
            LevelUpData data = this.mLevelUps[num];
            base.m_EntityData.ExcuteAttributes(data.name, data.value);
            num++;
        }
    }

    protected override void OnSetFlying(bool fly)
    {
    }

    protected override void OnSetPositionBy(Vector3 pos)
    {
        this.DoRunBubble(pos.magnitude);
    }

    protected override void OnTriggerEnterExtra(Collider o)
    {
        int layer = o.gameObject.layer;
        string tag = o.gameObject.tag;
        if ((layer == LayerManager.Map) && (tag == "Map_Door"))
        {
            this.TriggerDoor(o.gameObject);
        }
    }

    public void Reborn_Dead()
    {
        Singleton<MatchDefenceTimeSocketCtrl>.Instance.Send(MatchMessageType.eDead, 0);
        this.SetAbsorb(false);
        base.ShowHP(false);
        LocalSave.Instance.BattleIn_SetHaveBattle(false);
        base.m_AniCtrl.SendEvent("Dead", false);
        base.mAction.ActionClear();
        base.mAction.AddActionWaitDelegate(0.5f, new Action(this.FellGround));
        base.m_AniCtrl.DeadDown();
        GameLogic.Release.Game.JoyEnable(false);
        GameLogic.Hold.Sound.StopWalk();
        if (this.FootDirection != null)
        {
            this.FootDirection.SetActive(false);
        }
        GameLogic.Release.Mode.PlayerDead();
        SdkManager.send_deadlayer(GameLogic.Release.Mode.RoomGenerate.GetCurrentRoomID());
        base.m_MoveCtrl.ResetRigidBody();
    }

    public void Reborn_DeadEnd()
    {
        this.Reborn_DeadEndInternal();
        Debug.Log("Reborn_DeadEnd Reborn_DeadEnd");
        Facade.Instance.SendNotification("BATTLE_GAMEOVER");
    }

    private void Reborn_DeadEndInternal()
    {
        base.OnDeadBefore();
        LocalSave.Instance.BattleIn_DeInit();
        GameLogic.Release.Game.RemoveJoy();
        GameLogic.Hold.BattleData.SetWin(false);
        if (base.m_HPSlider != null)
        {
            base.m_HPSlider.DeInit();
        }
        base.DeInitLogic();
        base.DeInitMesh(true);
    }

    public void RemoveMoveEnd(Action callback)
    {
        int index = 0;
        int count = this.mMoveEndList.Count;
        while (index < count)
        {
            if (this.mMoveEndList[index].mEvent == callback)
            {
                this.mMoveEndList.RemoveAt(index);
                break;
            }
            index++;
        }
    }

    public void RemoveMoveStart(Action callback)
    {
        int index = 0;
        int count = this.mMoveStartList.Count;
        while (index < count)
        {
            if (this.mMoveStartList[index].mEvent == callback)
            {
                this.mMoveStartList.RemoveAt(index);
                break;
            }
            index++;
        }
    }

    public void RemoveMoving(Action<JoyData> callback)
    {
        int index = 0;
        int count = this.mMovingList.Count;
        while (index < count)
        {
            if (this.mMovingList[index].mEvent == callback)
            {
                this.mMovingList.RemoveAt(index);
                break;
            }
            index++;
        }
    }

    private void RemoveOldSuperSkill()
    {
        if (this.mSuperSkill != null)
        {
            this.mSuperSkill.DeInit();
            this.mSuperSkill = null;
        }
    }

    public void SetAbsorb(bool enable)
    {
        this.mAbsorb += !enable ? -1 : 1;
        if (this.Coin_Absorb != null)
        {
            if ((this.mAbsorb == 1) && enable)
            {
                this.Coin_Absorb.gameObject.SetActive(true);
            }
            else if ((this.mAbsorb == 0) && !enable)
            {
                this.Coin_Absorb.gameObject.SetActive(false);
            }
        }
    }

    public void SetAbsorbRangeMax(bool value)
    {
    }

    public override void SetCollider(bool enable)
    {
        base.SetCollider(enable);
    }

    private void showRunBubble()
    {
        Transform transform = GameLogic.EffectGet("Game/Player/RunBubble").transform;
        transform.transform.SetParent(base.m_Body.FootMask.transform);
        transform.transform.localPosition = Vector3.zero;
        transform.transform.localScale = Vector3.one;
        transform.transform.localRotation = Quaternion.Euler(0f, Random.Range((float) 0f, (float) 360f), 0f);
        transform.transform.SetParent(GameNode.m_PoolParent);
    }

    protected override void StartInit()
    {
        base.m_EntityData.InitExp();
        base.m_EntityData.ExcuteAttributes("AttackSpeed%", (long) (base.m_EntityData.attribute.AttackSpeed.Value * 10000f));
        this.InitEquipSkills();
        int weaponID = LocalSave.Instance.Equip_GetWeapon();
        if (weaponID == 0)
        {
            weaponID = 0x3e8;
        }
        this.InitWeapon(weaponID);
        this.InitCards();
        this.InitSkillList();
    }

    private void TriggerDoor(GameObject o)
    {
        if (GameLogic.Release.Mode.RoomGenerate.IsBattleLoad())
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = delegate {
                    GameLogic.Release.Game.SetRoomState(RoomState.Throughing);
                    GameLogic.Release.Mode.EnterDoor();
                };
            }
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = delegate {
                    GameLogic.Release.Game.JoyEnable(false);
                    CameraControlM.Instance.SetCameraPosition(GameLogic.Self.position - new Vector3(0f, 0f, 1.2f));
                    CameraControlM.Instance.SetCameraSpeed(5f);
                };
            }
            if (<>f__am$cache2 == null)
            {
                <>f__am$cache2 = delegate {
                    GameLogic.Release.Game.JoyEnable(true);
                    CameraControlM.Instance.ResetCameraSpeed();
                    GameLogic.Release.Game.SetRoomState(RoomState.Runing);
                };
            }
            WindowUI.ShowLoading(<>f__am$cache0, <>f__am$cache1, <>f__am$cache2, BattleLoadProxy.LoadingType.eMiss);
        }
    }

    private void UpdateAbsorbTime(int foodid)
    {
        if (!this.mAbsorbTimes.ContainsKey(foodid))
        {
            this.mAbsorbTimes.Add(foodid, Time.time);
        }
        else
        {
            this.mAbsorbTimes[foodid] = Time.time;
        }
    }

    protected override void UpdateFixed()
    {
        if (base.m_MoveCtrl != null)
        {
            base.m_MoveCtrl.UpdateProgress();
        }
    }

    protected override void UpdateProcess(float delta)
    {
        base.UpdateProcess(delta);
        if (base.m_AttackCtrl != null)
        {
            base.m_AttackCtrl.UpdateProgress();
        }
    }

    public GameObject FootDirection { get; private set; }

    [CompilerGenerated]
    private sealed class <DoMoveEnd>c__AnonStorey3
    {
        internal int index;
        internal EntityHero $this;

        internal void <>m__0()
        {
            this.$this.mMoveEndList[this.index].mEvent();
        }
    }

    [CompilerGenerated]
    private sealed class <DoMoveStart>c__AnonStorey0
    {
        internal int index;
        internal EntityHero $this;

        internal void <>m__0()
        {
            this.$this.mMoveStartList[this.index].mEvent();
        }
    }

    [CompilerGenerated]
    private sealed class <DoMoving>c__AnonStorey1
    {
        internal int index;
        internal EntityHero.<DoMoving>c__AnonStorey2 <>f__ref$2;

        internal void <>m__0()
        {
            this.<>f__ref$2.$this.mMovingList[this.index].mEvent(this.<>f__ref$2.data);
        }
    }

    [CompilerGenerated]
    private sealed class <DoMoving>c__AnonStorey2
    {
        internal JoyData data;
        internal EntityHero $this;
    }

    public class EventMoveEndData
    {
        public Action mEvent;
        public float delay;
    }

    public class EventMoveStartData
    {
        public Action mEvent;
        public float delay;
    }

    public class EventMovingData
    {
        public Action<JoyData> mEvent;
        public float delay;
    }

    private class LevelUpData
    {
        public string name;
        public long value;
    }
}

