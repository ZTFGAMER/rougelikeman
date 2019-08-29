namespace EPPZ.Cloud.Model.Simulation
{
    using EPPZ.Cloud.Model;
    using System;
    using UnityEngine;

    [Serializable]
    public class KeyValuePair
    {
        public string key;
        public EPPZ.Cloud.Model.KeyValuePair.Type type;
        public bool isChanged;
        private string _stringValue;
        public float floatValue;
        public int intValue;
        public bool boolValue;
        [HideInInspector]
        public bool foldedOut = true;

        public virtual string stringValue
        {
            get => 
                this._stringValue;
            set => 
                (this._stringValue = value);
        }
    }
}

