namespace LitJson
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct PropertyMetadata
    {
        public MemberInfo Info;
        public bool IsField;
        public System.Type Type;
    }
}

