namespace Google.Developers
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class JavaObjWrapper
    {
        private IntPtr raw;
        private IntPtr cachedRawClass;

        protected JavaObjWrapper()
        {
            this.cachedRawClass = IntPtr.Zero;
        }

        public JavaObjWrapper(IntPtr rawObject)
        {
            this.cachedRawClass = IntPtr.Zero;
            this.raw = rawObject;
        }

        public JavaObjWrapper(string clazzName)
        {
            this.cachedRawClass = IntPtr.Zero;
            this.raw = AndroidJNI.AllocObject(AndroidJNI.FindClass(clazzName));
        }

        protected static jvalue[] ConstructArgArray(object[] theArgs)
        {
            object[] args = new object[theArgs.Length];
            for (int i = 0; i < theArgs.Length; i++)
            {
                if (theArgs[i] is JavaObjWrapper)
                {
                    args[i] = ((JavaObjWrapper) theArgs[i]).raw;
                }
                else
                {
                    args[i] = theArgs[i];
                }
            }
            jvalue[] jvalueArray = AndroidJNIHelper.CreateJNIArgArray(args);
            for (int j = 0; j < theArgs.Length; j++)
            {
                if (theArgs[j] is JavaObjWrapper)
                {
                    jvalueArray[j].l = ((JavaObjWrapper) theArgs[j]).raw;
                }
                else if (theArgs[j] is JavaInterfaceProxy)
                {
                    IntPtr ptr = AndroidJNIHelper.CreateJavaProxy((AndroidJavaProxy) theArgs[j]);
                    jvalueArray[j].l = ptr;
                }
            }
            if (jvalueArray.Length == 1)
            {
                for (int k = 0; k < jvalueArray.Length; k++)
                {
                    Debug.Log(string.Concat(new object[] { "---- [", k, "] -- ", jvalueArray[k].l }));
                }
            }
            return jvalueArray;
        }

        public void CreateInstance(string clazzName, params object[] args)
        {
            if (this.raw != IntPtr.Zero)
            {
                throw new Exception("Java object already set");
            }
            IntPtr constructorID = AndroidJNIHelper.GetConstructorID(this.RawClass, args);
            jvalue[] jvalueArray = ConstructArgArray(args);
            try
            {
                this.raw = AndroidJNI.NewObject(this.RawClass, constructorID, jvalueArray);
            }
            finally
            {
                AndroidJNIHelper.DeleteJNIArgArray(args, jvalueArray);
            }
        }

        public static float GetStaticFloatField(string clsName, string name)
        {
            IntPtr clazz = AndroidJNI.FindClass(clsName);
            IntPtr fieldID = AndroidJNI.GetStaticFieldID(clazz, name, "F");
            return AndroidJNI.GetStaticFloatField(clazz, fieldID);
        }

        public static int GetStaticIntField(string clsName, string name)
        {
            IntPtr clazz = AndroidJNI.FindClass(clsName);
            IntPtr fieldID = AndroidJNI.GetStaticFieldID(clazz, name, "I");
            return AndroidJNI.GetStaticIntField(clazz, fieldID);
        }

        public static T GetStaticObjectField<T>(string clsName, string name, string sig)
        {
            IntPtr clazz = AndroidJNI.FindClass(clsName);
            IntPtr fieldID = AndroidJNI.GetStaticFieldID(clazz, name, sig);
            IntPtr staticObjectField = AndroidJNI.GetStaticObjectField(clazz, fieldID);
            Type[] types = new Type[] { staticObjectField.GetType() };
            ConstructorInfo constructor = typeof(T).GetConstructor(types);
            if (constructor != null)
            {
                object[] parameters = new object[] { staticObjectField };
                return (T) constructor.Invoke(parameters);
            }
            Type structureType = typeof(T);
            return (T) Marshal.PtrToStructure(staticObjectField, structureType);
        }

        public static string GetStaticStringField(string clsName, string name)
        {
            IntPtr clazz = AndroidJNI.FindClass(clsName);
            IntPtr fieldID = AndroidJNI.GetStaticFieldID(clazz, name, "Ljava/lang/String;");
            return AndroidJNI.GetStaticStringField(clazz, fieldID);
        }

        public T InvokeCall<T>(string name, string sig, params object[] args)
        {
            T local;
            Type type = typeof(T);
            IntPtr methodID = AndroidJNI.GetMethodID(this.RawClass, name, sig);
            if (methodID == IntPtr.Zero)
            {
                Debug.LogError("Cannot get method for " + name);
                throw new Exception("Cannot get method for " + name);
            }
            jvalue[] jvalueArray = ConstructArgArray(args);
            try
            {
                if (type == typeof(bool))
                {
                    return (T) AndroidJNI.CallBooleanMethod(this.raw, methodID, jvalueArray);
                }
                if (type == typeof(string))
                {
                    return (T) AndroidJNI.CallStringMethod(this.raw, methodID, jvalueArray);
                }
                if (type == typeof(int))
                {
                    return (T) AndroidJNI.CallIntMethod(this.raw, methodID, jvalueArray);
                }
                if (type == typeof(float))
                {
                    return (T) AndroidJNI.CallFloatMethod(this.raw, methodID, jvalueArray);
                }
                if (type == typeof(double))
                {
                    return (T) AndroidJNI.CallDoubleMethod(this.raw, methodID, jvalueArray);
                }
                if (type == typeof(byte))
                {
                    return (T) AndroidJNI.CallByteMethod(this.raw, methodID, jvalueArray);
                }
                if (type == typeof(char))
                {
                    return (T) AndroidJNI.CallCharMethod(this.raw, methodID, jvalueArray);
                }
                if (type == typeof(long))
                {
                    return (T) AndroidJNI.CallLongMethod(this.raw, methodID, jvalueArray);
                }
                if (type == typeof(short))
                {
                    return (T) AndroidJNI.CallShortMethod(this.raw, methodID, jvalueArray);
                }
                local = this.InvokeObjectCall<T>(name, sig, args);
            }
            finally
            {
                AndroidJNIHelper.DeleteJNIArgArray(args, jvalueArray);
            }
            return local;
        }

        public void InvokeCallVoid(string name, string sig, params object[] args)
        {
            IntPtr methodID = AndroidJNI.GetMethodID(this.RawClass, name, sig);
            jvalue[] jvalueArray = ConstructArgArray(args);
            try
            {
                AndroidJNI.CallVoidMethod(this.raw, methodID, jvalueArray);
            }
            finally
            {
                AndroidJNIHelper.DeleteJNIArgArray(args, jvalueArray);
            }
        }

        public T InvokeObjectCall<T>(string name, string sig, params object[] theArgs)
        {
            T local2;
            IntPtr methodID = AndroidJNI.GetMethodID(this.RawClass, name, sig);
            jvalue[] args = ConstructArgArray(theArgs);
            try
            {
                IntPtr ptr = AndroidJNI.CallObjectMethod(this.raw, methodID, args);
                if (ptr.Equals(IntPtr.Zero))
                {
                    return default(T);
                }
                Type[] types = new Type[] { ptr.GetType() };
                ConstructorInfo constructor = typeof(T).GetConstructor(types);
                if (constructor != null)
                {
                    object[] parameters = new object[] { ptr };
                    return (T) constructor.Invoke(parameters);
                }
                Type structureType = typeof(T);
                local2 = (T) Marshal.PtrToStructure(ptr, structureType);
            }
            finally
            {
                AndroidJNIHelper.DeleteJNIArgArray(theArgs, args);
            }
            return local2;
        }

        public static T StaticInvokeCall<T>(string type, string name, string sig, params object[] args)
        {
            T local;
            Type type2 = typeof(T);
            IntPtr clazz = AndroidJNI.FindClass(type);
            IntPtr methodID = AndroidJNI.GetStaticMethodID(clazz, name, sig);
            jvalue[] jvalueArray = ConstructArgArray(args);
            try
            {
                if (type2 == typeof(bool))
                {
                    return (T) AndroidJNI.CallStaticBooleanMethod(clazz, methodID, jvalueArray);
                }
                if (type2 == typeof(string))
                {
                    return (T) AndroidJNI.CallStaticStringMethod(clazz, methodID, jvalueArray);
                }
                if (type2 == typeof(int))
                {
                    return (T) AndroidJNI.CallStaticIntMethod(clazz, methodID, jvalueArray);
                }
                if (type2 == typeof(float))
                {
                    return (T) AndroidJNI.CallStaticFloatMethod(clazz, methodID, jvalueArray);
                }
                if (type2 == typeof(double))
                {
                    return (T) AndroidJNI.CallStaticDoubleMethod(clazz, methodID, jvalueArray);
                }
                if (type2 == typeof(byte))
                {
                    return (T) AndroidJNI.CallStaticByteMethod(clazz, methodID, jvalueArray);
                }
                if (type2 == typeof(char))
                {
                    return (T) AndroidJNI.CallStaticCharMethod(clazz, methodID, jvalueArray);
                }
                if (type2 == typeof(long))
                {
                    return (T) AndroidJNI.CallStaticLongMethod(clazz, methodID, jvalueArray);
                }
                if (type2 == typeof(short))
                {
                    return (T) AndroidJNI.CallStaticShortMethod(clazz, methodID, jvalueArray);
                }
                local = StaticInvokeObjectCall<T>(type, name, sig, args);
            }
            finally
            {
                AndroidJNIHelper.DeleteJNIArgArray(args, jvalueArray);
            }
            return local;
        }

        public static void StaticInvokeCallVoid(string type, string name, string sig, params object[] args)
        {
            IntPtr clazz = AndroidJNI.FindClass(type);
            IntPtr methodID = AndroidJNI.GetStaticMethodID(clazz, name, sig);
            jvalue[] jvalueArray = ConstructArgArray(args);
            try
            {
                AndroidJNI.CallStaticVoidMethod(clazz, methodID, jvalueArray);
            }
            finally
            {
                AndroidJNIHelper.DeleteJNIArgArray(args, jvalueArray);
            }
        }

        public static T StaticInvokeObjectCall<T>(string type, string name, string sig, params object[] args)
        {
            T local;
            IntPtr clazz = AndroidJNI.FindClass(type);
            IntPtr methodID = AndroidJNI.GetStaticMethodID(clazz, name, sig);
            jvalue[] jvalueArray = ConstructArgArray(args);
            try
            {
                IntPtr array = AndroidJNI.CallStaticObjectMethod(clazz, methodID, jvalueArray);
                Type[] types = new Type[] { array.GetType() };
                ConstructorInfo constructor = typeof(T).GetConstructor(types);
                if (constructor != null)
                {
                    object[] parameters = new object[] { array };
                    return (T) constructor.Invoke(parameters);
                }
                if (typeof(T).IsArray)
                {
                    return AndroidJNIHelper.ConvertFromJNIArray<T>(array);
                }
                Debug.Log("Trying cast....");
                Type structureType = typeof(T);
                local = (T) Marshal.PtrToStructure(array, structureType);
            }
            finally
            {
                AndroidJNIHelper.DeleteJNIArgArray(args, jvalueArray);
            }
            return local;
        }

        public IntPtr RawObject =>
            this.raw;

        public virtual IntPtr RawClass
        {
            get
            {
                if ((this.cachedRawClass == IntPtr.Zero) && (this.raw != IntPtr.Zero))
                {
                    this.cachedRawClass = AndroidJNI.GetObjectClass(this.raw);
                }
                return this.cachedRawClass;
            }
        }
    }
}

