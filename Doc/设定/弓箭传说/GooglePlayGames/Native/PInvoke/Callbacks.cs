namespace GooglePlayGames.Native.PInvoke
{
    using AOT;
    using GooglePlayGames.Native.Cwrapper;
    using GooglePlayGames.OurUtils;
    using System;
    using System.Collections;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    internal static class Callbacks
    {
        internal static readonly Action<CommonErrorStatus.UIStatus> NoopUICallback = new Action<CommonErrorStatus.UIStatus>(Callbacks.<NoopUICallback>m__0);

        [CompilerGenerated]
        private static void <NoopUICallback>m__0(CommonErrorStatus.UIStatus status)
        {
            Logger.d("Received UI callback: " + status);
        }

        internal static void AsCoroutine(IEnumerator routine)
        {
            PlayGamesHelperObject.RunCoroutine(routine);
        }

        internal static Action<T> AsOnGameThreadCallback<T>(Action<T> toInvokeOnGameThread)
        {
            <AsOnGameThreadCallback>c__AnonStorey2<T> storey = new <AsOnGameThreadCallback>c__AnonStorey2<T> {
                toInvokeOnGameThread = toInvokeOnGameThread
            };
            return new Action<T>(storey.<>m__0);
        }

        internal static Action<T1, T2> AsOnGameThreadCallback<T1, T2>(Action<T1, T2> toInvokeOnGameThread)
        {
            <AsOnGameThreadCallback>c__AnonStorey4<T1, T2> storey = new <AsOnGameThreadCallback>c__AnonStorey4<T1, T2> {
                toInvokeOnGameThread = toInvokeOnGameThread
            };
            return new Action<T1, T2>(storey.<>m__0);
        }

        [MonoPInvokeCallback(typeof(ShowUICallbackInternal))]
        internal static void InternalShowUICallback(CommonErrorStatus.UIStatus status, IntPtr data)
        {
            Logger.d("Showing UI Internal callback: " + status);
            Action<CommonErrorStatus.UIStatus> action = IntPtrToTempCallback<Action<CommonErrorStatus.UIStatus>>(data);
            try
            {
                action(status);
            }
            catch (Exception exception)
            {
                Logger.e("Error encountered executing InternalShowAllUICallback. Smothering to avoid passing exception into Native: " + exception);
            }
        }

        internal static byte[] IntPtrAndSizeToByteArray(IntPtr data, UIntPtr dataLength)
        {
            if (dataLength.ToUInt64() == 0L)
            {
                return null;
            }
            byte[] destination = new byte[dataLength.ToUInt32()];
            Marshal.Copy(data, destination, 0, (int) dataLength.ToUInt32());
            return destination;
        }

        private static T IntPtrToCallback<T>(IntPtr handle, bool unpinHandle) where T: class
        {
            T target;
            if (PInvokeUtilities.IsNull(handle))
            {
                return null;
            }
            GCHandle handle2 = GCHandle.FromIntPtr(handle);
            try
            {
                target = (T) handle2.Target;
            }
            catch (InvalidCastException exception)
            {
                Logger.e(string.Concat(new object[] { "GC Handle pointed to unexpected type: ", handle2.Target.ToString(), ". Expected ", typeof(T) }));
                throw exception;
            }
            finally
            {
                if (unpinHandle)
                {
                    handle2.Free();
                }
            }
            return target;
        }

        internal static T IntPtrToPermanentCallback<T>(IntPtr handle) where T: class => 
            IntPtrToCallback<T>(handle, false);

        internal static T IntPtrToTempCallback<T>(IntPtr handle) where T: class => 
            IntPtrToCallback<T>(handle, true);

        internal static void PerformInternalCallback(string callbackName, Type callbackType, IntPtr response, IntPtr userData)
        {
            Logger.d("Entering internal callback for " + callbackName);
            Action<IntPtr> action = (callbackType != Type.Permanent) ? IntPtrToTempCallback<Action<IntPtr>>(userData) : IntPtrToPermanentCallback<Action<IntPtr>>(userData);
            if (action != null)
            {
                try
                {
                    action(response);
                }
                catch (Exception exception)
                {
                    Logger.e(string.Concat(new object[] { "Error encountered executing ", callbackName, ". Smothering to avoid passing exception into Native: ", exception }));
                }
            }
        }

        internal static void PerformInternalCallback<T>(string callbackName, Type callbackType, T param1, IntPtr param2, IntPtr userData)
        {
            Logger.d("Entering internal callback for " + callbackName);
            Action<T, IntPtr> action = null;
            try
            {
                action = (callbackType != Type.Permanent) ? IntPtrToTempCallback<Action<T, IntPtr>>(userData) : IntPtrToPermanentCallback<Action<T, IntPtr>>(userData);
            }
            catch (Exception exception)
            {
                Logger.e(string.Concat(new object[] { "Error encountered converting ", callbackName, ". Smothering to avoid passing exception into Native: ", exception }));
                return;
            }
            Logger.d("Internal Callback converted to action");
            if (action != null)
            {
                try
                {
                    action(param1, param2);
                }
                catch (Exception exception2)
                {
                    Logger.e(string.Concat(new object[] { "Error encountered executing ", callbackName, ". Smothering to avoid passing exception into Native: ", exception2 }));
                }
            }
        }

        internal static IntPtr ToIntPtr(Delegate callback)
        {
            if (callback == null)
            {
                return IntPtr.Zero;
            }
            return GCHandle.ToIntPtr(GCHandle.Alloc(callback));
        }

        internal static IntPtr ToIntPtr<T>(Action<T> callback, Func<IntPtr, T> conversionFunction) where T: BaseReferenceHolder
        {
            <ToIntPtr>c__AnonStorey0<T> storey = new <ToIntPtr>c__AnonStorey0<T> {
                conversionFunction = conversionFunction,
                callback = callback
            };
            Action<IntPtr> action = new Action<IntPtr>(storey.<>m__0);
            return ToIntPtr(action);
        }

        internal static IntPtr ToIntPtr<T, P>(Action<T, P> callback, Func<IntPtr, P> conversionFunction) where P: BaseReferenceHolder
        {
            <ToIntPtr>c__AnonStorey1<T, P> storey = new <ToIntPtr>c__AnonStorey1<T, P> {
                conversionFunction = conversionFunction,
                callback = callback
            };
            Action<T, IntPtr> action = new Action<T, IntPtr>(storey.<>m__0);
            return ToIntPtr(action);
        }

        [CompilerGenerated]
        private sealed class <AsOnGameThreadCallback>c__AnonStorey2<T>
        {
            internal Action<T> toInvokeOnGameThread;

            internal void <>m__0(T result)
            {
                <AsOnGameThreadCallback>c__AnonStorey3<T> storey = new <AsOnGameThreadCallback>c__AnonStorey3<T> {
                    <>f__ref$2 = (Callbacks.<AsOnGameThreadCallback>c__AnonStorey2<T>) this,
                    result = result
                };
                if (this.toInvokeOnGameThread != null)
                {
                    PlayGamesHelperObject.RunOnGameThread(new Action(storey.<>m__0));
                }
            }

            private sealed class <AsOnGameThreadCallback>c__AnonStorey3
            {
                internal T result;
                internal Callbacks.<AsOnGameThreadCallback>c__AnonStorey2<T> <>f__ref$2;

                internal void <>m__0()
                {
                    this.<>f__ref$2.toInvokeOnGameThread(this.result);
                }
            }
        }

        [CompilerGenerated]
        private sealed class <AsOnGameThreadCallback>c__AnonStorey4<T1, T2>
        {
            internal Action<T1, T2> toInvokeOnGameThread;

            internal void <>m__0(T1 result1, T2 result2)
            {
                <AsOnGameThreadCallback>c__AnonStorey5<T1, T2> storey = new <AsOnGameThreadCallback>c__AnonStorey5<T1, T2> {
                    <>f__ref$4 = (Callbacks.<AsOnGameThreadCallback>c__AnonStorey4<T1, T2>) this,
                    result1 = result1,
                    result2 = result2
                };
                if (this.toInvokeOnGameThread != null)
                {
                    PlayGamesHelperObject.RunOnGameThread(new Action(storey.<>m__0));
                }
            }

            private sealed class <AsOnGameThreadCallback>c__AnonStorey5
            {
                internal T1 result1;
                internal T2 result2;
                internal Callbacks.<AsOnGameThreadCallback>c__AnonStorey4<T1, T2> <>f__ref$4;

                internal void <>m__0()
                {
                    this.<>f__ref$4.toInvokeOnGameThread(this.result1, this.result2);
                }
            }
        }

        [CompilerGenerated]
        private sealed class <ToIntPtr>c__AnonStorey0<T> where T: BaseReferenceHolder
        {
            internal Func<IntPtr, T> conversionFunction;
            internal Action<T> callback;

            internal void <>m__0(IntPtr result)
            {
                using (T local = this.conversionFunction(result))
                {
                    if (this.callback != null)
                    {
                        this.callback(local);
                    }
                }
            }
        }

        [CompilerGenerated]
        private sealed class <ToIntPtr>c__AnonStorey1<T, P> where P: BaseReferenceHolder
        {
            internal Func<IntPtr, P> conversionFunction;
            internal Action<T, P> callback;

            internal void <>m__0(T param1, IntPtr param2)
            {
                using (P local = this.conversionFunction(param2))
                {
                    if (this.callback != null)
                    {
                        this.callback(param1, local);
                    }
                }
            }
        }

        internal delegate void ShowUICallbackInternal(CommonErrorStatus.UIStatus status, IntPtr data);

        internal enum Type
        {
            Permanent,
            Temporary
        }
    }
}

