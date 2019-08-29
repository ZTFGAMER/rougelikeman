using System;
using System.Collections.Generic;
using TableTool;
using UnityEngine;

public class GameFormManager
{
    private Dictionary<string, List<WeightData>> weightList = new Dictionary<string, List<WeightData>>();
    public const string AngelSkill = "AngelSkill";
    public const string DemonSkill = "DemonSkill";
    public const string GameTurntable = "GameTurntable";
    public const string GreedySkillBig = "GreedySkillBig";
    public const string GreedySkillSmall = "GreedySkillSmall";
    public const string LevelDropIn = "LevelDropIn";

    public GameFormManager()
    {
        this.Init();
    }

    public List<int> GetGreedySkill(string name, int count)
    {
        List<int> list = new List<int>();
        List<WeightData> list2 = new List<WeightData>();
        int num = 0;
        int num2 = this.weightList[name].Count;
        while (num < num2)
        {
            list2.Add(this.weightList[name][num]);
            num++;
        }
        for (int i = 0; i < count; i++)
        {
            int sum = this.GetSum(list2);
            int num4 = Random.Range(0, sum);
            for (int j = 0; j < list2.Count; j++)
            {
                WeightData data = list2[j];
                if (num4 < data.Weight)
                {
                    list.Add(data.EventID);
                    sum -= data.Weight;
                    list2.RemoveAt(j);
                    break;
                }
                num4 -= data.Weight;
            }
        }
        return list;
    }

    public int GetRandomID(string name)
    {
        List<WeightData> list = this.weightList[name];
        if (list.Count == 0)
        {
            return 0;
        }
        int sum = this.GetSum(list);
        int num2 = Random.Range(0, sum);
        int num3 = 0;
        int count = list.Count;
        for (int i = 0; i < count; i++)
        {
            int weight = list[i].Weight;
            if (num2 < weight)
            {
                num3 = i;
                break;
            }
            num2 -= weight;
        }
        return list[num3].EventID;
    }

    private int GetSum(List<WeightData> list)
    {
        int num = 0;
        int count = list.Count;
        for (int i = 0; i < count; i++)
        {
            num += list[i].Weight;
        }
        return num;
    }

    private void Init()
    {
        WeightData data;
        this.weightList.Add("AngelSkill", new List<WeightData>());
        this.weightList.Add("DemonSkill", new List<WeightData>());
        this.weightList.Add("GameTurntable", new List<WeightData>());
        this.weightList.Add("GreedySkillBig", new List<WeightData>());
        this.weightList.Add("GreedySkillSmall", new List<WeightData>());
        this.weightList.Add("LevelDropIn", new List<WeightData>());
        IList<Room_eventangelskill> allBeans = LocalModelManager.Instance.Room_eventangelskill.GetAllBeans();
        int num = 0;
        int count = allBeans.Count;
        while (num < count)
        {
            data = new WeightData {
                EventID = allBeans[num].EventID,
                Weight = allBeans[num].Weight
            };
            this.weightList["AngelSkill"].Add(data);
            num++;
        }
        IList<Room_eventdemontext2skill> list2 = LocalModelManager.Instance.Room_eventdemontext2skill.GetAllBeans();
        int num3 = 0;
        int num4 = list2.Count;
        while (num3 < num4)
        {
            data = new WeightData {
                EventID = list2[num3].EventID,
                Weight = list2[num3].Weight
            };
            this.weightList["DemonSkill"].Add(data);
            num3++;
        }
        IList<Room_eventgameturn> list3 = LocalModelManager.Instance.Room_eventgameturn.GetAllBeans();
        int num5 = 0;
        int num6 = list3.Count;
        while (num5 < num6)
        {
            data = new WeightData {
                EventID = list3[num5].EventID,
                Weight = list3[num5].Weight
            };
            this.weightList["GameTurntable"].Add(data);
            num5++;
        }
        IList<Skill_greedyskill> list4 = LocalModelManager.Instance.Skill_greedyskill.GetAllBeans();
        int num7 = 0;
        int num8 = list4.Count;
        while (num7 < num8)
        {
            Skill_greedyskill _greedyskill = list4[num7];
            if (_greedyskill.Type != 2)
            {
                string str = (_greedyskill.Type != 0) ? "GreedySkillSmall" : "GreedySkillBig";
                data = new WeightData {
                    EventID = _greedyskill.SkillID,
                    Weight = _greedyskill.Weight
                };
                this.weightList[str].Add(data);
            }
            num7++;
        }
        IList<Skill_dropin> list5 = LocalModelManager.Instance.Skill_dropin.GetAllBeans();
        int num9 = 0;
        int num10 = list5.Count;
        while (num9 < num10)
        {
            Skill_dropin _dropin = list5[num9];
            data = new WeightData {
                EventID = _dropin.ID,
                Weight = _dropin.Weight
            };
            this.weightList["LevelDropIn"].Add(data);
            num9++;
        }
    }

    public void InitData()
    {
    }

    public void Release()
    {
        this.weightList.Clear();
        this.Init();
    }

    public void RemoveID(string name, int EventID)
    {
        List<WeightData> list = this.weightList[name];
        int num = 0;
        int count = list.Count;
        while (num < count)
        {
            WeightData item = list[num];
            if (item.EventID == EventID)
            {
                list.Remove(item);
                break;
            }
            num++;
        }
    }

    public class WeightData
    {
        public int EventID;
        public int Weight;
    }
}

