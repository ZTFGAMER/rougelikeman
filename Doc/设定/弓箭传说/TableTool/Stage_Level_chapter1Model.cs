namespace TableTool
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Stage_Level_chapter1Model : LocalModel<Stage_Level_chapter1, string>
    {
        private const string _Filename = "Stage_Level_chapter1";
        private List<Stage_LevelData> list = new List<Stage_LevelData>();
        private bool bInit;

        protected override string GetBeanKey(Stage_Level_chapter1 bean) => 
            bean.RoomID;

        public int GetMaxStage() => 
            (this.list.Count - 1);

        public Stage_LevelData GetStageLevel(int stage) => 
            this.list[stage];

        public string[] GetStageLevel_Attributes(int stage, int level) => 
            this.list[stage].GetAttributes(level);

        public long GetStageLevel_Defence(int stage, int level) => 
            this.list[stage].GetDefence(level);

        public string[] GetStageLevel_MapAttributes(int stage, int level) => 
            this.list[stage].GetMapAttributes(level);

        public int GetStageLevel_MaxLevel(int stage) => 
            this.GetStageLevel(stage).Count;

        public string[] GetStageLevel_RoomIds(int stage, int level, int count) => 
            this.list[stage].GetRoomIds(level, count);

        public void Init()
        {
            this.list.Add(new Stage_LevelData(0, 0));
            IList<Stage_Level_chapter1> allBeans = LocalModelManager.Instance.Stage_Level_chapter1.GetAllBeans();
            int count = allBeans.Count;
            Stage_LevelData item = new Stage_LevelData(1, count);
            for (int i = 0; i < count; i++)
            {
                item.AddAttributes(allBeans[i].Attributes, allBeans[i].MapAttributes, allBeans[i].StandardDefence, allBeans[i].RoomIDs, allBeans[i].RoomIDs1);
            }
            this.list.Add(item);
            IList<Stage_Level_chapter2> list2 = LocalModelManager.Instance.Stage_Level_chapter2.GetAllBeans();
            int num3 = list2.Count;
            Stage_LevelData data2 = new Stage_LevelData(2, num3);
            for (int j = 0; j < num3; j++)
            {
                data2.AddAttributes(list2[j].Attributes, list2[j].MapAttributes, list2[j].StandardDefence, list2[j].RoomIDs, list2[j].RoomIDs1);
            }
            this.list.Add(data2);
            IList<Stage_Level_chapter3> list3 = LocalModelManager.Instance.Stage_Level_chapter3.GetAllBeans();
            int num5 = list3.Count;
            Stage_LevelData data3 = new Stage_LevelData(3, num5);
            for (int k = 0; k < num5; k++)
            {
                data3.AddAttributes(list3[k].Attributes, list3[k].MapAttributes, list3[k].StandardDefence, list3[k].RoomIDs, list3[k].RoomIDs1);
            }
            this.list.Add(data3);
            IList<Stage_Level_chapter4> list4 = LocalModelManager.Instance.Stage_Level_chapter4.GetAllBeans();
            int num7 = list4.Count;
            Stage_LevelData data4 = new Stage_LevelData(4, num7);
            for (int m = 0; m < num7; m++)
            {
                data4.AddAttributes(list4[m].Attributes, list4[m].MapAttributes, list4[m].StandardDefence, list4[m].RoomIDs, list4[m].RoomIDs1);
            }
            this.list.Add(data4);
            IList<Stage_Level_chapter5> list5 = LocalModelManager.Instance.Stage_Level_chapter5.GetAllBeans();
            int num9 = list5.Count;
            Stage_LevelData data5 = new Stage_LevelData(5, num9);
            for (int n = 0; n < num9; n++)
            {
                data5.AddAttributes(list5[n].Attributes, list5[n].MapAttributes, list5[n].StandardDefence, list5[n].RoomIDs, list5[n].RoomIDs1);
            }
            this.list.Add(data5);
            IList<Stage_Level_chapter6> list6 = LocalModelManager.Instance.Stage_Level_chapter6.GetAllBeans();
            int num11 = list6.Count;
            Stage_LevelData data6 = new Stage_LevelData(6, num11);
            for (int num12 = 0; num12 < num11; num12++)
            {
                data6.AddAttributes(list6[num12].Attributes, list6[num12].MapAttributes, list6[num12].StandardDefence, list6[num12].RoomIDs, list6[num12].RoomIDs1);
            }
            this.list.Add(data6);
            IList<Stage_Level_chapter7> list7 = LocalModelManager.Instance.Stage_Level_chapter7.GetAllBeans();
            int num13 = list7.Count;
            Stage_LevelData data7 = new Stage_LevelData(7, num13);
            for (int num14 = 0; num14 < num13; num14++)
            {
                data7.AddAttributes(list7[num14].Attributes, list7[num14].MapAttributes, list7[num14].StandardDefence, list7[num14].RoomIDs, list7[num14].RoomIDs1);
            }
            this.list.Add(data7);
            IList<Stage_Level_chapter8> list8 = LocalModelManager.Instance.Stage_Level_chapter8.GetAllBeans();
            int num15 = list8.Count;
            Stage_LevelData data8 = new Stage_LevelData(8, num15);
            for (int num16 = 0; num16 < num15; num16++)
            {
                data8.AddAttributes(list8[num16].Attributes, list8[num16].MapAttributes, list8[num16].StandardDefence, list8[num16].RoomIDs, list8[num16].RoomIDs1);
            }
            this.list.Add(data8);
            IList<Stage_Level_chapter9> list9 = LocalModelManager.Instance.Stage_Level_chapter9.GetAllBeans();
            int num17 = list9.Count;
            Stage_LevelData data9 = new Stage_LevelData(9, num17);
            for (int num18 = 0; num18 < num17; num18++)
            {
                data9.AddAttributes(list9[num18].Attributes, list9[num18].MapAttributes, list9[num18].StandardDefence, list9[num18].RoomIDs, list9[num18].RoomIDs1);
            }
            this.list.Add(data9);
            IList<Stage_Level_chapter10> list10 = LocalModelManager.Instance.Stage_Level_chapter10.GetAllBeans();
            int num19 = list10.Count;
            Stage_LevelData data10 = new Stage_LevelData(10, num19);
            for (int num20 = 0; num20 < num19; num20++)
            {
                data10.AddAttributes(list10[num20].Attributes, list10[num20].MapAttributes, list10[num20].StandardDefence, list10[num20].RoomIDs, list10[num20].RoomIDs1);
            }
            this.list.Add(data10);
            IList<Stage_Level_chapter11> list11 = LocalModelManager.Instance.Stage_Level_chapter11.GetAllBeans();
            int num21 = list11.Count;
            Stage_LevelData data11 = new Stage_LevelData(11, num21);
            for (int num22 = 0; num22 < num21; num22++)
            {
                data11.AddAttributes(list11[num22].Attributes, list11[num22].MapAttributes, list11[num22].StandardDefence, list11[num22].RoomIDs, list11[num22].RoomIDs1);
            }
            this.list.Add(data11);
            IList<Stage_Level_chapter12> list12 = LocalModelManager.Instance.Stage_Level_chapter12.GetAllBeans();
            int num23 = list12.Count;
            Stage_LevelData data12 = new Stage_LevelData(12, num23);
            for (int num24 = 0; num24 < num23; num24++)
            {
                data12.AddAttributes(list12[num24].Attributes, list12[num24].MapAttributes, list12[num24].StandardDefence, list12[num24].RoomIDs, list12[num24].RoomIDs1);
            }
            this.list.Add(data12);
        }

        public bool IsMaxLevel(int stage, int level) => 
            (this.GetStageLevel_MaxLevel(stage) <= level);

        protected override string Filename =>
            "Stage_Level_chapter1";

        public class Stage_LevelData
        {
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private int <Stage>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private int <Count>k__BackingField;
            private List<Stage_Level_chapter1Model.Stage_LevelDataOne> mList = new List<Stage_Level_chapter1Model.Stage_LevelDataOne>();

            public Stage_LevelData(int stage, int count)
            {
                this.Stage = stage;
                this.Count = count;
                this.mList.Add(null);
            }

            public void AddAttributes(string[] value, string[] mapsatt, long defence, string[] roomids, string[] roomids1)
            {
                int index = 0;
                int length = roomids.Length;
                while (index < length)
                {
                    roomids[index] = roomids[index].Replace("\n", string.Empty);
                    roomids[index] = roomids[index].Replace(" ", string.Empty);
                    index++;
                }
                Stage_Level_chapter1Model.Stage_LevelDataOne item = new Stage_Level_chapter1Model.Stage_LevelDataOne {
                    Attriutes = value,
                    MapAttriutes = mapsatt,
                    StandardDefence = defence,
                    RoomIds = roomids,
                    RoomIds1 = roomids1
                };
                this.mList.Add(item);
            }

            public string[] GetAttributes(int level)
            {
                if ((level >= 1) && (level <= this.Count))
                {
                    return this.mList[level].Attriutes;
                }
                return new string[0];
            }

            public long GetDefence(int level)
            {
                if ((level >= 1) && (level <= this.Count))
                {
                    return this.mList[level].StandardDefence;
                }
                return 100L;
            }

            public string[] GetMapAttributes(int level)
            {
                if ((level >= 1) && (level <= this.Count))
                {
                    return this.mList[level].MapAttriutes;
                }
                return new string[0];
            }

            public string[] GetRoomIds(int level, int count) => 
                this.mList[level].GetRoomIds(count);

            public int Stage { get; private set; }

            public int Count { get; private set; }
        }

        public class Stage_LevelDataOne
        {
            public string[] Attriutes;
            public string[] MapAttriutes;
            public long StandardDefence;
            public string[] RoomIds;
            public string[] RoomIds1;

            public string[] GetRoomIds(int count)
            {
                if ((count == 0) && (this.RoomIds1.Length > 0))
                {
                    return this.RoomIds1;
                }
                return this.RoomIds;
            }
        }
    }
}

