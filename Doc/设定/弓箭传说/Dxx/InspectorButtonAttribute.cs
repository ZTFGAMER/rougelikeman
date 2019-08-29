namespace Dxx
{
    using System;
    using UnityEngine;

    [AttributeUsage(AttributeTargets.Field)]
    public class InspectorButtonAttribute : PropertyAttribute
    {
        public readonly string MethodName;

        public InspectorButtonAttribute(string MethodName)
        {
            this.MethodName = MethodName;
        }
    }
}

