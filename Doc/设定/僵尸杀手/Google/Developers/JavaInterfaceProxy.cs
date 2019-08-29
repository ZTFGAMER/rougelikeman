namespace Google.Developers
{
    using System;
    using UnityEngine;

    public abstract class JavaInterfaceProxy : AndroidJavaProxy
    {
        public JavaInterfaceProxy(string interfaceName) : base(interfaceName)
        {
        }
    }
}

