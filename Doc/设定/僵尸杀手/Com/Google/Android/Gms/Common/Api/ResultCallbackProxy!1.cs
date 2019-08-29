namespace Com.Google.Android.Gms.Common.Api
{
    using Google.Developers;
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public abstract class ResultCallbackProxy<R> : JavaInterfaceProxy, ResultCallback<R> where R: Result
    {
        private const string CLASS_NAME = "com/google/android/gms/common/api/ResultCallback";

        public ResultCallbackProxy() : base("com/google/android/gms/common/api/ResultCallback")
        {
        }

        public void onResult(R arg_Result_1)
        {
            this.OnResult(arg_Result_1);
        }

        public void onResult(AndroidJavaObject arg_Result_1)
        {
            R local;
            IntPtr rawObject = arg_Result_1.GetRawObject();
            Type[] types = new Type[] { rawObject.GetType() };
            ConstructorInfo constructor = typeof(R).GetConstructor(types);
            if (constructor != null)
            {
                object[] parameters = new object[] { rawObject };
                local = (R) constructor.Invoke(parameters);
            }
            else
            {
                local = (R) typeof(R).GetConstructor(new Type[0]).Invoke(new object[0]);
                Marshal.PtrToStructure(rawObject, local);
            }
            this.OnResult(local);
        }

        public abstract void OnResult(R arg_Result_1);
    }
}

