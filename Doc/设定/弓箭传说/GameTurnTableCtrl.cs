using Dxx.Net;
using Dxx.Util;
using GameProtocol;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TableTool;
using UnityEngine;

public class GameTurnTableCtrl : MonoBehaviour
{
    private const int TableCount = 6;
    public Action<TurnTableData> TurnEnd;
    public Transform child;
    public Transform arrow;
    public List<GameTurnTableOneCtrl> mList;
    private const float Speed = -20f;
    private float speed;
    private float speedtime;
    private float starttime;
    private bool bStart;
    private bool bDelayTurnEnd;
    private float turnendstarttime;
    private float turnendupdatetime = 0.4f;
    private float offset = 5f;
    private float rotateangle;
    private SequencePool mSeqPool = new SequencePool();
    private TurnTableData resultData;
    private TurnTableData resultGet;
    private List<TurnTableData> list;
    private int playCount;
    private ActionBasic action;
    [CompilerGenerated]
    private static Action<long> <>f__am$cache0;
    [CompilerGenerated]
    private static Action<NetResponse> <>f__am$cache1;
    [CompilerGenerated]
    private static Action<long> <>f__am$cache2;
    [CompilerGenerated]
    private static Action <>f__am$cache3;

    public GameTurnTableCtrl()
    {
        TurnTableData data = new TurnTableData {
            type = TurnTableType.Get
        };
        this.resultGet = data;
        this.list = new List<TurnTableData>();
        this.action = new ActionBasic();
    }

    private void CheckResult()
    {
        GameTurnTableOneCtrl ctrl = this.mList[0];
        int num = 0;
        for (int i = 1; i < 6; i++)
        {
            GameTurnTableOneCtrl ctrl2 = this.mList[i];
            if (this.GetMinAngle(ctrl2.transform.eulerAngles.z) < this.GetMinAngle(ctrl.transform.eulerAngles.z))
            {
                num = i;
                ctrl = ctrl2;
            }
        }
        this.resultData = ctrl.mData;
        string skillName = string.Empty;
        string skillContent = string.Empty;
        switch (this.resultData.type)
        {
            case TurnTableType.ExpBig:
            case TurnTableType.ExpSmall:
                GameLogic.Release.Mode.CreateGoods(GameLogic.Self.position, GameLogic.GetExpList((int) this.resultData.value), 2);
                break;

            case TurnTableType.PlayerSkill:
            {
                int skillid = (int) this.resultData.value;
                GameLogic.Self.LearnExtraSkill(skillid);
                skillName = GameLogic.Hold.Language.GetSkillName(skillid);
                skillContent = GameLogic.Hold.Language.GetSkillContent(skillid);
                break;
            }
            case TurnTableType.EventSkill:
            {
                int getID = LocalModelManager.Instance.Room_eventgameturn.GetBeanById((int) this.resultData.value).GetID;
                GameLogic.Self.AddSkill(getID, Array.Empty<object>());
                skillName = GameLogic.Hold.Language.GetSkillName(getID);
                skillContent = GameLogic.Hold.Language.GetSkillContent(getID);
                break;
            }
            case TurnTableType.HPAdd:
            {
                int skillId = (int) this.resultData.value;
                GameLogic.Self.AddSkillAttribute(skillId, Array.Empty<object>());
                skillName = GameLogic.Hold.Language.GetSkillName(skillId);
                skillContent = GameLogic.Hold.Language.GetSkillContent(skillId);
                break;
            }
            case TurnTableType.Empty:
                skillName = GameLogic.Hold.Language.GetLanguageByTID("GameTurn_EmptyTitle", Array.Empty<object>());
                skillContent = GameLogic.Hold.Language.GetLanguageByTID("GameTurn_EmptyContent", Array.Empty<object>());
                break;

            case TurnTableType.Gold:
            {
                long allcount = (long) this.resultData.value;
                skillName = GameLogic.Hold.Language.GetLanguageByTID("GameTurn_GoldTitle", Array.Empty<object>());
                object[] args = new object[] { allcount };
                skillContent = GameLogic.Hold.Language.GetLanguageByTID("GameTurn_GoldContent", args);
                if (allcount > 0L)
                {
                    IMediator mediator = Facade.Instance.RetrieveMediator("BattleModuleMediator");
                    if (mediator != null)
                    {
                        object obj2 = mediator.GetEvent("Event_GetGoldPosition");
                        if ((obj2 == null) || !(obj2 is Vector3))
                        {
                            GameLogic.Hold.BattleData.AddGold((float) allcount);
                            break;
                        }
                        Vector3 endpos = (Vector3) obj2;
                        if (<>f__am$cache0 == null)
                        {
                            <>f__am$cache0 = onecount => GameLogic.Hold.BattleData.AddGold((float) onecount);
                        }
                        CurrencyFlyCtrl.PlayGet(CurrencyType.BattleGold, allcount, new Vector3(((float) GameLogic.Width) / 2f, ((float) GameLogic.Height) / 2f, 0f), endpos, <>f__am$cache0, null, false);
                    }
                }
                break;
            }
            case TurnTableType.Diamond:
            {
                long diamond = (long) this.resultData.value;
                skillName = GameLogic.Hold.Language.GetLanguageByTID("GameTurn_DiamondTitle", Array.Empty<object>());
                object[] args = new object[] { diamond };
                skillContent = GameLogic.Hold.Language.GetLanguageByTID("GameTurn_DiamondContent", args);
                CReqItemPacket itemPacket = NetManager.GetItemPacket(null, false);
                itemPacket.m_nPacketType = 1;
                itemPacket.m_nDiamondAmount = (uint) diamond;
                LocalSave.Instance.Modify_Diamond(diamond, false);
                if (<>f__am$cache1 == null)
                {
                    <>f__am$cache1 = delegate (NetResponse response) {
                        if (response.IsSuccess)
                        {
                        }
                    };
                }
                NetManager.SendInternal<CReqItemPacket>(itemPacket, SendType.eUDP, <>f__am$cache1);
                if (<>f__am$cache2 == null)
                {
                    <>f__am$cache2 = delegate (long currentcount) {
                    };
                }
                if (<>f__am$cache3 == null)
                {
                    <>f__am$cache3 = delegate {
                    };
                }
                CurrencyFlyCtrl.PlayGet(CurrencyType.Diamond, diamond, new Vector3(((float) GameLogic.Width) / 2f, ((float) GameLogic.Height) / 2f, 0f), <>f__am$cache2, <>f__am$cache3, false);
                break;
            }
            case TurnTableType.Reward_Gold2:
            case TurnTableType.Reward_Gold3:
            case TurnTableType.Reward_Item2:
            case TurnTableType.Reward_Item3:
            case TurnTableType.Reward_All2:
            case TurnTableType.Reward_All3:
                GameLogic.Hold.BattleData.SetRewardType(this.resultData.type);
                break;
        }
        if (skillName != string.Empty)
        {
            CInstance<TipsManager>.Instance.Show(skillName, skillContent);
        }
        this.playCount++;
        this.mList[num].Init(this.resultGet);
        if (this.playCount <= GameLogic.Self.m_EntityData.TurnTableCount)
        {
            this.bDelayTurnEnd = false;
            this.action.AddActionIgnoreWaitDelegate(0.4f, () => this.Init());
        }
    }

    public void DeInit()
    {
        this.action.DeInit();
        this.mSeqPool.Clear();
    }

    private float GetMinAngle(float angle)
    {
        float num = Mathf.Abs(angle);
        float num2 = Mathf.Abs((float) (angle + 360f));
        if (num > num2)
        {
            num = num2;
        }
        num2 = Mathf.Abs((float) (angle - 360f));
        if (num > num2)
        {
            num = num2;
        }
        return num;
    }

    private Vector3 GetRandomOffset()
    {
        switch (Random.Range(0, 4))
        {
            case 0:
                return new Vector3(-this.offset, -this.offset, 0f);

            case 1:
                return new Vector3(-this.offset, this.offset, 0f);

            case 2:
                return new Vector3(this.offset, -this.offset, 0f);

            case 3:
                return new Vector3(this.offset, this.offset, 0f);
        }
        return Vector3.zero;
    }

    public void Init()
    {
        this.bStart = true;
        this.bDelayTurnEnd = false;
        this.starttime = Time.unscaledTime;
        this.speed = -20f;
        this.speedtime = Random.Range((float) 0.6f, (float) 0.9f);
        this.child.localRotation = Quaternion.Euler(0f, 0f, 0f);
        this.offset = 5f;
        this.rotateangle = 0f;
    }

    public void InitGood(List<TurnTableData> list)
    {
        if (GameLogic.Self != null)
        {
            this.playCount = 0;
            this.list = list;
            this.action.Init(true);
            list.RandomSort<TurnTableData>();
            int num = 0;
            int count = list.Count;
            while (num < count)
            {
                this.mList[num].Init(list[num]);
                num++;
            }
        }
    }

    private void RotateAction()
    {
        bool flag = false;
        if ((Time.unscaledTime - this.starttime) < this.speedtime)
        {
            this.child.localRotation = Quaternion.Euler(0f, 0f, this.child.localEulerAngles.z + this.speed);
            flag = true;
        }
        else
        {
            flag = true;
            float num = Mathf.Abs(this.speed);
            if (num < 0.5f)
            {
                this.speed *= 0.961f;
                this.offset *= 0.961f;
            }
            else if (num < 3f)
            {
                this.speed *= 0.971f;
                this.offset *= 0.971f;
            }
            else
            {
                this.speed *= 0.981f;
                this.offset *= 0.981f;
            }
            this.child.localRotation = Quaternion.Euler(0f, 0f, this.child.localEulerAngles.z + this.speed);
            if (Mathf.Abs(this.speed) < 0.2f)
            {
                this.bStart = false;
                flag = false;
                this.bDelayTurnEnd = true;
                this.turnendstarttime = Time.unscaledTime;
                this.CheckResult();
            }
        }
        this.rotateangle -= this.speed;
        if (this.rotateangle >= 60f)
        {
            this.rotateangle -= 60f;
            GameLogic.Hold.Sound.PlayUI(0xf4245);
        }
        if (flag)
        {
            this.child.localPosition = this.GetRandomOffset();
            this.arrow.localPosition = this.GetRandomOffset() * 0.5f;
        }
    }

    private void Update()
    {
        if (this.bStart)
        {
            this.RotateAction();
        }
        if (this.bDelayTurnEnd && ((Time.unscaledTime - this.turnendstarttime) > this.turnendupdatetime))
        {
            this.TurnEnd(this.resultData);
            this.bDelayTurnEnd = false;
        }
    }
}

