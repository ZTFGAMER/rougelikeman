using Dxx.Util;
using System;
using System.Collections.Generic;
using TableTool;
using UnityEngine;

public class EventChest1TurnCtrl : MonoBehaviour
{
    private const int TableCount = 6;
    public Action<TurnTableType> TurnEnd;
    public Transform child;
    public Transform arrow;
    public List<EventChest1OneCtrl> mList;
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
    private TurnTableData resultData;
    private List<TurnTableData> list = new List<TurnTableData>();
    private int playCount;
    private ActionBasic action = new ActionBasic();
    private Drop_DropModel.DropData[] equips;

    private void CheckResult()
    {
        EventChest1OneCtrl ctrl = this.mList[0];
        int num = 0;
        for (int i = 1; i < 6; i++)
        {
            EventChest1OneCtrl ctrl2 = this.mList[i];
            if (this.GetMinAngle(ctrl2.transform.eulerAngles.z) < this.GetMinAngle(ctrl.transform.eulerAngles.z))
            {
                num = i;
                ctrl = ctrl2;
            }
        }
        this.resultData = ctrl.mData;
        object[] args = new object[] { num, this.resultData.type, (this.resultData.value != null) ? this.resultData.value.ToString() : string.Empty };
        Debugger.Log(Utils.FormatString("result : {0} {1} {2}", args));
        string str = string.Empty;
        string str2 = string.Empty;
        switch (this.resultData.type)
        {
            case TurnTableType.Boss:
                GameLogic.Release.Mode.RoomGenerate.SendEvent("Event_TurnTable_Boss", null);
                break;

            case TurnTableType.Hitted:
                if (GameLogic.Self != null)
                {
                    float maxHP = GameLogic.Self.m_EntityData.MaxHP;
                    int num4 = (int) (-maxHP * 0.3f);
                    GameLogic.Self.ChangeHP(null, (long) num4);
                }
                break;
        }
        if (str != string.Empty)
        {
            CInstance<TipsManager>.Instance.Show(str, str2);
        }
        this.playCount++;
        this.resultData.type = TurnTableType.Get;
        this.mList[num].Init(this.resultData);
        if (this.playCount <= GameLogic.Self.m_EntityData.TurnTableCount)
        {
            this.bDelayTurnEnd = false;
            this.action.AddActionIgnoreWaitDelegate(0.4f, () => this.Init());
        }
    }

    public void DeInit()
    {
        this.action.DeInit();
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
        this.speedtime = Random.Range((float) 0.9f, (float) 1.1f);
        this.child.localRotation = Quaternion.Euler(0f, 0f, 0f);
        this.offset = 5f;
        this.rotateangle = 0f;
    }

    public void InitGood(Drop_DropModel.DropData[] equips)
    {
        this.equips = equips;
        if (GameLogic.Self != null)
        {
            this.playCount = 0;
            this.list.Clear();
            this.action.Init(true);
            for (int i = 0; i < 3; i++)
            {
                TurnTableData item = new TurnTableData {
                    type = TurnTableType.Boss
                };
                this.list.Add(item);
            }
            for (int j = 0; j < 1; j++)
            {
                TurnTableData item = new TurnTableData {
                    type = TurnTableType.BigEquip,
                    value = equips[1]
                };
                this.list.Add(item);
            }
            for (int k = 0; k < 1; k++)
            {
                TurnTableData item = new TurnTableData {
                    type = TurnTableType.SmallEquip,
                    value = equips[0]
                };
                this.list.Add(item);
            }
            for (int m = 0; m < 1; m++)
            {
                TurnTableData item = new TurnTableData {
                    type = TurnTableType.Hitted
                };
                this.list.Add(item);
            }
            this.list.RandomSort<TurnTableData>();
            int num5 = 0;
            int count = this.list.Count;
            while (num5 < count)
            {
                this.mList[num5].Init(this.list[num5]);
                num5++;
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
                this.speed *= 0.97f;
                this.offset *= 0.97f;
            }
            else if (num < 3f)
            {
                this.speed *= 0.98f;
                this.offset *= 0.98f;
            }
            else
            {
                this.speed *= 0.99f;
                this.offset *= 0.99f;
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
            this.TurnEnd(this.resultData.type);
            this.bDelayTurnEnd = false;
        }
    }
}

