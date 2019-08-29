namespace Spine.Unity
{
    using System;
    using UnityEngine;

    [AttributeUsage(AttributeTargets.Field, Inherited=true, AllowMultiple=false)]
    public abstract class SpineAttributeBase : PropertyAttribute
    {
        public string dataField = string.Empty;
        public string startsWith = string.Empty;
        public bool includeNone = true;
        public bool fallbackToTextField;

        protected SpineAttributeBase()
        {
        }
    }
}

