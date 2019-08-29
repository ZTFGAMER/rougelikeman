using Dxx.Util;
using System;
using System.Collections.Generic;
using TableTool;

public class BuffCtrl : EntityCtrlBase
{
    private Dictionary<int, BuffAloneBase> mBuffs = new Dictionary<int, BuffAloneBase>();
    private List<BuffAloneBase> mOverBuffs = new List<BuffAloneBase>();
    private List<int> removeList = new List<int>();
    private BuffAloneBase data1;
    private BuffAloneBase data2;
    private int mBuffsID;

    private void AddBuff(BattleStruct.BuffStruct data)
    {
        Buff_alone beanById = LocalModelManager.Instance.Buff_alone.GetBeanById(data.buffId);
        int buffID = beanById.BuffID;
        if (beanById.OverType == 0)
        {
            if (!this.mBuffs.ContainsKey(buffID))
            {
                this.mBuffs.Add(buffID, this.getBuff(data, beanById));
            }
            else
            {
                this.mBuffs[buffID].ResetBuffTime(false);
            }
        }
        else
        {
            this.mOverBuffs.Add(this.getBuff(data, beanById));
        }
    }

    public override void ExcuteCommend(EBattleAction id, object action)
    {
        if (id != EBattleAction.EBattle_Action_Add_Buff)
        {
            if (id == EBattleAction.EBattle_Action_Remove_Buff)
            {
                this.RemoveBuff((BattleStruct.BuffStruct) action);
            }
        }
        else
        {
            this.AddBuff((BattleStruct.BuffStruct) action);
        }
    }

    private BuffAloneBase getBuff(BattleStruct.BuffStruct data, Buff_alone buff_alone)
    {
        int buffID = buff_alone.BuffID;
        BuffAloneBase base2 = null;
        object[] args = new object[] { "BuffAlone", buffID };
        Type type = Type.GetType(Utils.GetString(args));
        if (type != null)
        {
            object[] objArray2 = new object[] { "BuffAlone", buffID };
            base2 = type.Assembly.CreateInstance(Utils.GetString(objArray2)) as BuffAloneBase;
        }
        else
        {
            base2 = new BuffAloneBase();
        }
        if (base2 != null)
        {
            base2.Init(base.m_Entity, data, LocalModelManager.Instance.Buff_alone.GetBeanById(buffID));
            return base2;
        }
        object[] objArray3 = new object[] { "Buff ", buffID, " dont have" };
        SdkManager.Bugly_Report("BuffCtrl.cs", Utils.GetString(objArray3));
        return base2;
    }

    public override void OnRemove()
    {
        Dictionary<int, BuffAloneBase>.Enumerator enumerator = this.mBuffs.GetEnumerator();
        while (enumerator.MoveNext())
        {
            KeyValuePair<int, BuffAloneBase> current = enumerator.Current;
            current.Value.Remove();
        }
        this.mBuffs.Clear();
        int num = 0;
        int count = this.mOverBuffs.Count;
        while (num < count)
        {
            this.mOverBuffs[num].Remove();
            num++;
        }
        this.mOverBuffs.Clear();
    }

    public override void OnStart(List<EBattleAction> actIds)
    {
        actIds.Add(EBattleAction.EBattle_Action_Add_Buff);
        actIds.Add(EBattleAction.EBattle_Action_Remove_Buff);
        base.SetUseUpdate();
    }

    public override void OnUpdate(float delta)
    {
        this.removeList.Clear();
        List<int> list = new List<int>(this.mBuffs.Keys);
        int num = 0;
        int count = list.Count;
        while (num < count)
        {
            this.mBuffsID = list[num];
            if (this.mBuffs.TryGetValue(this.mBuffsID, out this.data1) && (this.data1 != null))
            {
                this.data1.OnUpdate(delta);
                if (this.data1.IsEnd())
                {
                    this.removeList.Add(this.mBuffsID);
                }
            }
            num++;
        }
        int num3 = 0;
        int num4 = this.removeList.Count;
        while (num3 < num4)
        {
            this.RemoveBuff(this.removeList[num3]);
            num3++;
        }
        for (int i = this.mOverBuffs.Count - 1; i >= 0; i--)
        {
            this.data2 = this.mOverBuffs[i];
            this.data2.OnUpdate(delta);
            if (this.mOverBuffs.Count == 0)
            {
                break;
            }
            if (this.data2.IsEnd())
            {
                this.data2.Remove();
                this.mOverBuffs.Remove(this.data2);
            }
        }
    }

    private void RemoveBuff(BattleStruct.BuffStruct data)
    {
        Buff_alone beanById = LocalModelManager.Instance.Buff_alone.GetBeanById(data.buffId);
        this.RemoveBuff(beanById.BuffID);
    }

    private void RemoveBuff(int buffId)
    {
        if (this.mBuffs.TryGetValue(buffId, out BuffAloneBase base2))
        {
            if (base2 != null)
            {
                base2.Remove();
            }
            this.mBuffs.Remove(buffId);
        }
    }
}

