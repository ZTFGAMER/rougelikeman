namespace TableTool
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Language_lauguage : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <TID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <CN_s>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <CN_t>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <EN>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <FR>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <DE>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <ID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <JP>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <KR>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <PT_BR>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <RU>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <ES_ES>k__BackingField;

        public Language_lauguage Copy() => 
            new Language_lauguage { 
                TID = this.TID,
                CN_s = this.CN_s,
                CN_t = this.CN_t,
                EN = this.EN,
                FR = this.FR,
                DE = this.DE,
                ID = this.ID,
                JP = this.JP,
                KR = this.KR,
                PT_BR = this.PT_BR,
                RU = this.RU,
                ES_ES = this.ES_ES
            };

        protected override bool ReadImpl()
        {
            this.TID = base.readLocalString();
            this.CN_s = base.readLocalString();
            this.CN_t = base.readLocalString();
            this.EN = base.readLocalString();
            this.FR = base.readLocalString();
            this.DE = base.readLocalString();
            this.ID = base.readLocalString();
            this.JP = base.readLocalString();
            this.KR = base.readLocalString();
            this.PT_BR = base.readLocalString();
            this.RU = base.readLocalString();
            this.ES_ES = base.readLocalString();
            return true;
        }

        public string TID { get; private set; }

        public string CN_s { get; private set; }

        public string CN_t { get; private set; }

        public string EN { get; private set; }

        public string FR { get; private set; }

        public string DE { get; private set; }

        public string ID { get; private set; }

        public string JP { get; private set; }

        public string KR { get; private set; }

        public string PT_BR { get; private set; }

        public string RU { get; private set; }

        public string ES_ES { get; private set; }
    }
}

