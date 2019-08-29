namespace TableTool
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Stage_Level_stagechapter : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <ID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Notes>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <TiledID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <GameType>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int[] <GameArgs>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <StyleSequence>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <StageLevel>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <OpenCondition>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <ArgsOpen>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <GoldRate>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <EquipDropID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <EquipProb>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <IntegralRate>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <ExpBase>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <ExpAdd>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <GoldTurn>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int[] <DropAddCond>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <DropAddProb>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <AdProb>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <AdTurn>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <ScrollRate>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <ScrollRateBoss>k__BackingField;

        public Stage_Level_stagechapter Copy() => 
            new Stage_Level_stagechapter { 
                ID = this.ID,
                Notes = this.Notes,
                TiledID = this.TiledID,
                GameType = this.GameType,
                GameArgs = this.GameArgs,
                StyleSequence = this.StyleSequence,
                StageLevel = this.StageLevel,
                OpenCondition = this.OpenCondition,
                ArgsOpen = this.ArgsOpen,
                GoldRate = this.GoldRate,
                EquipDropID = this.EquipDropID,
                EquipProb = this.EquipProb,
                IntegralRate = this.IntegralRate,
                ExpBase = this.ExpBase,
                ExpAdd = this.ExpAdd,
                GoldTurn = this.GoldTurn,
                DropAddCond = this.DropAddCond,
                DropAddProb = this.DropAddProb,
                AdProb = this.AdProb,
                AdTurn = this.AdTurn,
                ScrollRate = this.ScrollRate,
                ScrollRateBoss = this.ScrollRateBoss
            };

        protected override bool ReadImpl()
        {
            this.ID = base.readInt();
            this.Notes = base.readLocalString();
            this.TiledID = base.readInt();
            this.GameType = base.readInt();
            this.GameArgs = base.readArrayint();
            this.StyleSequence = base.readArraystring();
            this.StageLevel = base.readLocalString();
            this.OpenCondition = base.readLocalString();
            this.ArgsOpen = base.readInt();
            this.GoldRate = base.readFloat();
            this.EquipDropID = base.readInt();
            this.EquipProb = base.readInt();
            this.IntegralRate = base.readFloat();
            this.ExpBase = base.readInt();
            this.ExpAdd = base.readInt();
            this.GoldTurn = base.readArraystring();
            this.DropAddCond = base.readArrayint();
            this.DropAddProb = base.readInt();
            this.AdProb = base.readInt();
            this.AdTurn = base.readArraystring();
            this.ScrollRate = base.readArraystring();
            this.ScrollRateBoss = base.readArraystring();
            return true;
        }

        public int ID { get; private set; }

        public string Notes { get; private set; }

        public int TiledID { get; private set; }

        public int GameType { get; private set; }

        public int[] GameArgs { get; private set; }

        public string[] StyleSequence { get; private set; }

        public string StageLevel { get; private set; }

        public string OpenCondition { get; private set; }

        public int ArgsOpen { get; private set; }

        public float GoldRate { get; private set; }

        public int EquipDropID { get; private set; }

        public int EquipProb { get; private set; }

        public float IntegralRate { get; private set; }

        public int ExpBase { get; private set; }

        public int ExpAdd { get; private set; }

        public string[] GoldTurn { get; private set; }

        public int[] DropAddCond { get; private set; }

        public int DropAddProb { get; private set; }

        public int AdProb { get; private set; }

        public string[] AdTurn { get; private set; }

        public string[] ScrollRate { get; private set; }

        public string[] ScrollRateBoss { get; private set; }
    }
}

