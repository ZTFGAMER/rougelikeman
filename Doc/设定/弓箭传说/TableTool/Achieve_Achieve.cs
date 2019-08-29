namespace TableTool
{
    using Dxx.Util;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Achieve_Achieve : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <ID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Index>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Stage>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <GlobalType>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <UnlockType>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <ShowTypeArgs>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <CondType>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <CondTypeArgs>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <Rewards>k__BackingField;
        private List<Drop_DropModel.DropData> rewardlist = new List<Drop_DropModel.DropData>();

        public Achieve_Achieve Copy() => 
            new Achieve_Achieve { 
                ID = this.ID,
                Index = this.Index,
                Stage = this.Stage,
                GlobalType = this.GlobalType,
                UnlockType = this.UnlockType,
                ShowTypeArgs = this.ShowTypeArgs,
                CondType = this.CondType,
                CondTypeArgs = this.CondTypeArgs,
                Rewards = this.Rewards
            };

        public List<Drop_DropModel.DropData> GetRewards()
        {
            if (this.rewardlist.Count == 0)
            {
                for (int i = 0; i < this.Rewards.Length; i++)
                {
                    char[] separator = new char[] { ':' };
                    string[] strArray = this.Rewards[i].Split(separator);
                    if (strArray.Length != 2)
                    {
                        object[] args = new object[] { this.ID };
                        SdkManager.Bugly_Report("LocalSave_Achieve.Achieve_Achieve", Utils.FormatString("achieveid:[{0}] Rewards.Length != 2 !!!", args));
                    }
                    if (!int.TryParse(strArray[0], out int num2))
                    {
                        object[] args = new object[] { this.ID };
                        SdkManager.Bugly_Report("LocalSave_Achieve.Achieve_Achieve", Utils.FormatString("achieveid:[{0}] id is not a int type!!! ", args));
                    }
                    if (!int.TryParse(strArray[1], out int num3))
                    {
                        object[] args = new object[] { this.ID };
                        SdkManager.Bugly_Report("LocalSave_Achieve.Achieve_Achieve", Utils.FormatString("achieveid:[{0}] count is not a int type!!! ", args));
                    }
                    Drop_DropModel.DropData item = new Drop_DropModel.DropData {
                        type = PropType.eCurrency,
                        id = num2,
                        count = num3
                    };
                    this.rewardlist.Add(item);
                }
            }
            return this.rewardlist;
        }

        protected override bool ReadImpl()
        {
            this.ID = base.readInt();
            this.Index = base.readInt();
            this.Stage = base.readInt();
            this.GlobalType = base.readInt();
            this.UnlockType = base.readInt();
            this.ShowTypeArgs = base.readInt();
            this.CondType = base.readInt();
            this.CondTypeArgs = base.readArraystring();
            this.Rewards = base.readArraystring();
            return true;
        }

        public int ID { get; private set; }

        public int Index { get; private set; }

        public int Stage { get; private set; }

        public int GlobalType { get; private set; }

        public int UnlockType { get; private set; }

        public int ShowTypeArgs { get; private set; }

        public int CondType { get; private set; }

        public string[] CondTypeArgs { get; private set; }

        public string[] Rewards { get; private set; }

        public bool IsGlobal =>
            (this.GlobalType == 1);
    }
}

