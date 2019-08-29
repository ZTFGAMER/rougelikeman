namespace TableTool
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Equip_equip : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Id>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Name>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <PropType>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Overlying>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Position>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Type>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <EquipIcon>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Quality>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <Attributes>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int[] <AttributesUp>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int[] <Skills>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <SkillsUp>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <AdditionSkills>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int[] <UnlockCondition>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <InitialPower>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <AddPower>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Powerratio>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int[] <SuperID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <BreakNeed>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <MaxLevel>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <UpgradeNeed>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Score>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <SellPrice>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <CritSellProb>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float[] <SellDiamond>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int[] <CardCost>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int[] <CoinCost>k__BackingField;
        public bool Install;
        public string primaryKey;

        public Equip_equip Copy() => 
            new Equip_equip { 
                Id = this.Id,
                Name = this.Name,
                PropType = this.PropType,
                Overlying = this.Overlying,
                Position = this.Position,
                Type = this.Type,
                EquipIcon = this.EquipIcon,
                Quality = this.Quality,
                Attributes = this.Attributes,
                AttributesUp = this.AttributesUp,
                Skills = this.Skills,
                SkillsUp = this.SkillsUp,
                AdditionSkills = this.AdditionSkills,
                UnlockCondition = this.UnlockCondition,
                InitialPower = this.InitialPower,
                AddPower = this.AddPower,
                Powerratio = this.Powerratio,
                SuperID = this.SuperID,
                BreakNeed = this.BreakNeed,
                MaxLevel = this.MaxLevel,
                UpgradeNeed = this.UpgradeNeed,
                Score = this.Score,
                SellPrice = this.SellPrice,
                CritSellProb = this.CritSellProb,
                SellDiamond = this.SellDiamond,
                CardCost = this.CardCost,
                CoinCost = this.CoinCost
            };

        protected override bool ReadImpl()
        {
            this.Id = base.readInt();
            this.Name = base.readLocalString();
            this.PropType = base.readInt();
            this.Overlying = base.readInt();
            this.Position = base.readInt();
            this.Type = base.readInt();
            this.EquipIcon = base.readInt();
            this.Quality = base.readInt();
            this.Attributes = base.readArraystring();
            this.AttributesUp = base.readArrayint();
            this.Skills = base.readArrayint();
            this.SkillsUp = base.readArraystring();
            this.AdditionSkills = base.readArraystring();
            this.UnlockCondition = base.readArrayint();
            this.InitialPower = base.readLocalString();
            this.AddPower = base.readLocalString();
            this.Powerratio = base.readLocalString();
            this.SuperID = base.readArrayint();
            this.BreakNeed = base.readInt();
            this.MaxLevel = base.readInt();
            this.UpgradeNeed = base.readInt();
            this.Score = base.readInt();
            this.SellPrice = base.readInt();
            this.CritSellProb = base.readArraystring();
            this.SellDiamond = base.readArrayfloat();
            this.CardCost = base.readArrayint();
            this.CoinCost = base.readArrayint();
            return true;
        }

        public int Id { get; private set; }

        public string Name { get; private set; }

        public int PropType { get; private set; }

        public int Overlying { get; private set; }

        public int Position { get; private set; }

        public int Type { get; private set; }

        public int EquipIcon { get; private set; }

        public int Quality { get; private set; }

        public string[] Attributes { get; private set; }

        public int[] AttributesUp { get; private set; }

        public int[] Skills { get; private set; }

        public string[] SkillsUp { get; private set; }

        public string[] AdditionSkills { get; private set; }

        public int[] UnlockCondition { get; private set; }

        public string InitialPower { get; private set; }

        public string AddPower { get; private set; }

        public string Powerratio { get; private set; }

        public int[] SuperID { get; private set; }

        public int BreakNeed { get; private set; }

        public int MaxLevel { get; private set; }

        public int UpgradeNeed { get; private set; }

        public int Score { get; private set; }

        public int SellPrice { get; private set; }

        public string[] CritSellProb { get; private set; }

        public float[] SellDiamond { get; private set; }

        public int[] CardCost { get; private set; }

        public int[] CoinCost { get; private set; }
    }
}

