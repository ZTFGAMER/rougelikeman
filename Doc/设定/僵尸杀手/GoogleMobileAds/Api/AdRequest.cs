namespace GoogleMobileAds.Api
{
    using GoogleMobileAds.Api.Mediation;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class AdRequest
    {
        public const string Version = "3.12.0";
        public const string TestDeviceSimulator = "SIMULATOR";
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<string> <TestDevices>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private HashSet<string> <Keywords>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime? <Birthday>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private GoogleMobileAds.Api.Gender? <Gender>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool? <TagForChildDirectedTreatment>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Dictionary<string, string> <Extras>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<GoogleMobileAds.Api.Mediation.MediationExtras> <MediationExtras>k__BackingField;

        private AdRequest(Builder builder)
        {
            this.TestDevices = new List<string>(builder.TestDevices);
            this.Keywords = new HashSet<string>(builder.Keywords);
            this.Birthday = builder.Birthday;
            this.Gender = builder.Gender;
            this.TagForChildDirectedTreatment = builder.ChildDirectedTreatmentTag;
            this.Extras = new Dictionary<string, string>(builder.Extras);
            this.MediationExtras = builder.MediationExtras;
        }

        public List<string> TestDevices { get; private set; }

        public HashSet<string> Keywords { get; private set; }

        public DateTime? Birthday { get; private set; }

        public GoogleMobileAds.Api.Gender? Gender { get; private set; }

        public bool? TagForChildDirectedTreatment { get; private set; }

        public Dictionary<string, string> Extras { get; private set; }

        public List<GoogleMobileAds.Api.Mediation.MediationExtras> MediationExtras { get; private set; }

        public class Builder
        {
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private List<string> <TestDevices>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private HashSet<string> <Keywords>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private DateTime? <Birthday>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private GoogleMobileAds.Api.Gender? <Gender>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private bool? <ChildDirectedTreatmentTag>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private Dictionary<string, string> <Extras>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private List<GoogleMobileAds.Api.Mediation.MediationExtras> <MediationExtras>k__BackingField;

            public Builder()
            {
                this.TestDevices = new List<string>();
                this.Keywords = new HashSet<string>();
                this.Birthday = null;
                this.Gender = null;
                this.ChildDirectedTreatmentTag = null;
                this.Extras = new Dictionary<string, string>();
                this.MediationExtras = new List<GoogleMobileAds.Api.Mediation.MediationExtras>();
            }

            public AdRequest.Builder AddExtra(string key, string value)
            {
                this.Extras.Add(key, value);
                return this;
            }

            public AdRequest.Builder AddKeyword(string keyword)
            {
                this.Keywords.Add(keyword);
                return this;
            }

            public AdRequest.Builder AddMediationExtras(GoogleMobileAds.Api.Mediation.MediationExtras extras)
            {
                this.MediationExtras.Add(extras);
                return this;
            }

            public AdRequest.Builder AddTestDevice(string deviceId)
            {
                this.TestDevices.Add(deviceId);
                return this;
            }

            public AdRequest Build() => 
                new AdRequest(this);

            public AdRequest.Builder SetBirthday(DateTime birthday)
            {
                this.Birthday = new DateTime?(birthday);
                return this;
            }

            public AdRequest.Builder SetGender(GoogleMobileAds.Api.Gender gender)
            {
                this.Gender = new GoogleMobileAds.Api.Gender?(gender);
                return this;
            }

            public AdRequest.Builder TagForChildDirectedTreatment(bool tagForChildDirectedTreatment)
            {
                this.ChildDirectedTreatmentTag = new bool?(tagForChildDirectedTreatment);
                return this;
            }

            internal List<string> TestDevices { get; private set; }

            internal HashSet<string> Keywords { get; private set; }

            internal DateTime? Birthday { get; private set; }

            internal GoogleMobileAds.Api.Gender? Gender { get; private set; }

            internal bool? ChildDirectedTreatmentTag { get; private set; }

            internal Dictionary<string, string> Extras { get; private set; }

            internal List<GoogleMobileAds.Api.Mediation.MediationExtras> MediationExtras { get; private set; }
        }
    }
}

