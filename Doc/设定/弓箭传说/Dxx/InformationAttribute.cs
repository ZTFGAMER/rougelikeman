namespace Dxx
{
    using System;
    using UnityEngine;

    public class InformationAttribute : PropertyAttribute
    {
        public InformationAttribute(string message, InformationType type, bool messageAfterProperty)
        {
        }

        public enum InformationType
        {
            Error,
            Info,
            None,
            Warning
        }
    }
}

