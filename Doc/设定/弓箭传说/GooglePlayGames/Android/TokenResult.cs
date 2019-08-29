namespace GooglePlayGames.Android
{
    using Com.Google.Android.Gms.Common.Api;
    using Google.Developers;
    using System;

    internal class TokenResult : JavaObjWrapper, Result
    {
        public TokenResult(IntPtr ptr) : base(ptr)
        {
        }

        public string getAuthCode() => 
            base.InvokeCall<string>("getAuthCode", "()Ljava/lang/String;", Array.Empty<object>());

        public string getEmail() => 
            base.InvokeCall<string>("getEmail", "()Ljava/lang/String;", Array.Empty<object>());

        public string getIdToken() => 
            base.InvokeCall<string>("getIdToken", "()Ljava/lang/String;", Array.Empty<object>());

        public Status getStatus() => 
            new Status(base.InvokeCall<IntPtr>("getStatus", "()Lcom/google/android/gms/common/api/Status;", Array.Empty<object>()));

        public int getStatusCode() => 
            base.InvokeCall<int>("getStatusCode", "()I", Array.Empty<object>());
    }
}

