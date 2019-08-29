namespace Com.Google.Android.Gms.Common.Api
{
    using Google.Developers;
    using System;

    public class PendingResult<R> : JavaObjWrapper where R: Result
    {
        private const string CLASS_NAME = "com/google/android/gms/common/api/PendingResult";

        public PendingResult() : base("com.google.android.gms.common.api.PendingResult")
        {
        }

        public PendingResult(IntPtr ptr) : base(ptr)
        {
        }

        public R await() => 
            base.InvokeCall<R>("await", "()Lcom/google/android/gms/common/api/Result;", Array.Empty<object>());

        public R await(long arg_long_1, object arg_object_2)
        {
            object[] args = new object[] { arg_long_1, arg_object_2 };
            return base.InvokeCall<R>("await", "(JLjava/util/concurrent/TimeUnit;)Lcom/google/android/gms/common/api/Result;", args);
        }

        public void cancel()
        {
            base.InvokeCallVoid("cancel", "()V", Array.Empty<object>());
        }

        public bool isCanceled() => 
            base.InvokeCall<bool>("isCanceled", "()Z", Array.Empty<object>());

        public void setResultCallback(ResultCallback<R> arg_ResultCallback_1)
        {
            object[] args = new object[] { arg_ResultCallback_1 };
            base.InvokeCallVoid("setResultCallback", "(Lcom/google/android/gms/common/api/ResultCallback;)V", args);
        }

        public void setResultCallback(ResultCallback<R> arg_ResultCallback_1, long arg_long_2, object arg_object_3)
        {
            object[] args = new object[] { arg_ResultCallback_1, arg_long_2, arg_object_3 };
            base.InvokeCallVoid("setResultCallback", "(Lcom/google/android/gms/common/api/ResultCallback;JLjava/util/concurrent/TimeUnit;)V", args);
        }
    }
}

