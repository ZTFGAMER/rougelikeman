namespace Com.Google.Android.Gms.Common.Api
{
    using Com.Google.Android.Gms.Common;
    using Google.Developers;
    using System;

    public class GoogleApiClient : JavaObjWrapper
    {
        private const string CLASS_NAME = "com/google/android/gms/common/api/GoogleApiClient";

        public GoogleApiClient() : base("com.google.android.gms.common.api.GoogleApiClient")
        {
        }

        public GoogleApiClient(IntPtr ptr) : base(ptr)
        {
        }

        public ConnectionResult blockingConnect() => 
            base.InvokeCall<ConnectionResult>("blockingConnect", "()Lcom/google/android/gms/common/ConnectionResult;", new object[0]);

        public ConnectionResult blockingConnect(long arg_long_1, object arg_object_2)
        {
            object[] args = new object[] { arg_long_1, arg_object_2 };
            return base.InvokeCall<ConnectionResult>("blockingConnect", "(JLjava/util/concurrent/TimeUnit;)Lcom/google/android/gms/common/ConnectionResult;", args);
        }

        public PendingResult<Status> clearDefaultAccountAndReconnect() => 
            base.InvokeCall<PendingResult<Status>>("clearDefaultAccountAndReconnect", "()Lcom/google/android/gms/common/api/PendingResult;", new object[0]);

        public void connect()
        {
            base.InvokeCallVoid("connect", "()V", new object[0]);
        }

        public void disconnect()
        {
            base.InvokeCallVoid("disconnect", "()V", new object[0]);
        }

        public void dump(string arg_string_1, object arg_object_2, object arg_object_3, string[] arg_string_4)
        {
            object[] args = new object[] { arg_string_1, arg_object_2, arg_object_3, arg_string_4 };
            base.InvokeCallVoid("dump", "(Ljava/lang/String;Ljava/io/FileDescriptor;Ljava/io/PrintWriter;[Ljava/lang/String;)V", args);
        }

        public ConnectionResult getConnectionResult(object arg_object_1)
        {
            object[] args = new object[] { arg_object_1 };
            return base.InvokeCall<ConnectionResult>("getConnectionResult", "(Lcom/google/android/gms/common/api/Api;)Lcom/google/android/gms/common/ConnectionResult;", args);
        }

        public object getContext() => 
            base.InvokeCall<object>("getContext", "()Landroid/content/Context;", new object[0]);

        public object getLooper() => 
            base.InvokeCall<object>("getLooper", "()Landroid/os/Looper;", new object[0]);

        public int getSessionId() => 
            base.InvokeCall<int>("getSessionId", "()I", new object[0]);

        public bool hasConnectedApi(object arg_object_1)
        {
            object[] args = new object[] { arg_object_1 };
            return base.InvokeCall<bool>("hasConnectedApi", "(Lcom/google/android/gms/common/api/Api;)Z", args);
        }

        public bool isConnected() => 
            base.InvokeCall<bool>("isConnected", "()Z", new object[0]);

        public bool isConnecting() => 
            base.InvokeCall<bool>("isConnecting", "()Z", new object[0]);

        public bool isConnectionCallbacksRegistered(object arg_object_1)
        {
            object[] args = new object[] { arg_object_1 };
            return base.InvokeCall<bool>("isConnectionCallbacksRegistered", "(Lcom/google/android/gms/common/api/GoogleApiClient$ConnectionCallbacks;)Z", args);
        }

        public bool isConnectionFailedListenerRegistered(object arg_object_1)
        {
            object[] args = new object[] { arg_object_1 };
            return base.InvokeCall<bool>("isConnectionFailedListenerRegistered", "(Lcom/google/android/gms/common/api/GoogleApiClient$OnConnectionFailedListener;)Z", args);
        }

        public void reconnect()
        {
            base.InvokeCallVoid("reconnect", "()V", new object[0]);
        }

        public void registerConnectionCallbacks(object arg_object_1)
        {
            object[] args = new object[] { arg_object_1 };
            base.InvokeCallVoid("registerConnectionCallbacks", "(Lcom/google/android/gms/common/api/GoogleApiClient$ConnectionCallbacks;)V", args);
        }

        public void registerConnectionFailedListener(object arg_object_1)
        {
            object[] args = new object[] { arg_object_1 };
            base.InvokeCallVoid("registerConnectionFailedListener", "(Lcom/google/android/gms/common/api/GoogleApiClient$OnConnectionFailedListener;)V", args);
        }

        public void stopAutoManage(object arg_object_1)
        {
            object[] args = new object[] { arg_object_1 };
            base.InvokeCallVoid("stopAutoManage", "(Landroid/support/v4/app/FragmentActivity;)V", args);
        }

        public void unregisterConnectionCallbacks(object arg_object_1)
        {
            object[] args = new object[] { arg_object_1 };
            base.InvokeCallVoid("unregisterConnectionCallbacks", "(Lcom/google/android/gms/common/api/GoogleApiClient$ConnectionCallbacks;)V", args);
        }

        public void unregisterConnectionFailedListener(object arg_object_1)
        {
            object[] args = new object[] { arg_object_1 };
            base.InvokeCallVoid("unregisterConnectionFailedListener", "(Lcom/google/android/gms/common/api/GoogleApiClient$OnConnectionFailedListener;)V", args);
        }
    }
}

