namespace GoogleMobileAds.Api.Mediation
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public abstract class MediationExtras
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Dictionary<string, string> <Extras>k__BackingField;

        public MediationExtras()
        {
            this.Extras = new Dictionary<string, string>();
        }

        public Dictionary<string, string> Extras { get; protected set; }

        public abstract string AndroidMediationExtraBuilderClassName { get; }

        public abstract string IOSMediationExtraBuilderClassName { get; }
    }
}

