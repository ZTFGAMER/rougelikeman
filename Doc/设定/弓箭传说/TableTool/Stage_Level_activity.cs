namespace TableTool
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Stage_Level_activity : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <ID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Type>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Notes>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Difficult>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <StageLevel>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <MaxLayer>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <StyleSequence>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <LevelCondition>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int[] <TimeCondition>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Number>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int[] <Power>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int[] <Price>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <GoldRate>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <EquipDropID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <EquipProb>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <IntegralRate>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int[] <Reward>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <Args>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <StandardRoom>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <Integral_Ratio>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <ExpBase>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <ExpAdd>k__BackingField;

        public Stage_Level_activity Copy() => 
            new Stage_Level_activity { 
                ID = this.ID,
                Type = this.Type,
                Notes = this.Notes,
                Difficult = this.Difficult,
                StageLevel = this.StageLevel,
                MaxLayer = this.MaxLayer,
                StyleSequence = this.StyleSequence,
                LevelCondition = this.LevelCondition,
                TimeCondition = this.TimeCondition,
                Number = this.Number,
                Power = this.Power,
                Price = this.Price,
                GoldRate = this.GoldRate,
                EquipDropID = this.EquipDropID,
                EquipProb = this.EquipProb,
                IntegralRate = this.IntegralRate,
                Reward = this.Reward,
                Args = this.Args,
                StandardRoom = this.StandardRoom,
                Integral_Ratio = this.Integral_Ratio,
                ExpBase = this.ExpBase,
                ExpAdd = this.ExpAdd
            };

        public GameMode GetMode() => 
            ((GameMode) this.Type);

        protected override bool ReadImpl()
        {
            this.ID = base.readInt();
            this.Type = base.readInt();
            this.Notes = base.readLocalString();
            this.Difficult = base.readInt();
            this.StageLevel = base.readLocalString();
            this.MaxLayer = base.readInt();
            this.StyleSequence = base.readArraystring();
            this.LevelCondition = base.readInt();
            this.TimeCondition = base.readArrayint();
            this.Number = base.readInt();
            this.Power = base.readArrayint();
            this.Price = base.readArrayint();
            this.GoldRate = base.readFloat();
            this.EquipDropID = base.readInt();
            this.EquipProb = base.readInt();
            this.IntegralRate = base.readFloat();
            this.Reward = base.readArrayint();
            this.Args = base.readArraystring();
            this.StandardRoom = base.readLocalString();
            this.Integral_Ratio = base.readFloat();
            this.ExpBase = base.readInt();
            this.ExpAdd = base.readInt();
            return true;
        }

        public int ID { get; private set; }

        public int Type { get; private set; }

        public string Notes { get; private set; }

        public int Difficult { get; private set; }

        public string StageLevel { get; private set; }

        public int MaxLayer { get; private set; }

        public string[] StyleSequence { get; private set; }

        public int LevelCondition { get; private set; }

        public int[] TimeCondition { get; private set; }

        public int Number { get; private set; }

        public int[] Power { get; private set; }

        public int[] Price { get; private set; }

        public float GoldRate { get; private set; }

        public int EquipDropID { get; private set; }

        public int EquipProb { get; private set; }

        public float IntegralRate { get; private set; }

        public int[] Reward { get; private set; }

        public string[] Args { get; private set; }

        public string StandardRoom { get; private set; }

        public float Integral_Ratio { get; private set; }

        public int ExpBase { get; private set; }

        public int ExpAdd { get; private set; }

        public bool Unlock =>
            (LocalSave.Instance.GetLevel() >= this.LevelCondition);
    }
}

