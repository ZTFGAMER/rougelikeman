namespace Com.Google.Android.Gms.Common.Api
{
    using Google.Developers;
    using System;

    public class Status : JavaObjWrapper, Result
    {
        private const string CLASS_NAME = "com/google/android/gms/common/api/Status";

        public Status(int arg_int_1)
        {
            object[] args = new object[] { arg_int_1 };
            base.CreateInstance("com/google/android/gms/common/api/Status", args);
        }

        public Status(IntPtr ptr) : base(ptr)
        {
        }

        public Status(int arg_int_1, string arg_string_2)
        {
            object[] args = new object[] { arg_int_1, arg_string_2 };
            base.CreateInstance("com/google/android/gms/common/api/Status", args);
        }

        public Status(int arg_int_1, string arg_string_2, object arg_object_3)
        {
            object[] args = new object[] { arg_int_1, arg_string_2, arg_object_3 };
            base.CreateInstance("com/google/android/gms/common/api/Status", args);
        }

        public int describeContents() => 
            base.InvokeCall<int>("describeContents", "()I", Array.Empty<object>());

        public bool equals(object arg_object_1)
        {
            object[] args = new object[] { arg_object_1 };
            return base.InvokeCall<bool>("equals", "(Ljava/lang/Object;)Z", args);
        }

        public object getResolution() => 
            base.InvokeCall<object>("getResolution", "()Landroid/app/PendingIntent;", Array.Empty<object>());

        public Status getStatus() => 
            base.InvokeCall<Status>("getStatus", "()Lcom/google/android/gms/common/api/Status;", Array.Empty<object>());

        public int getStatusCode() => 
            base.InvokeCall<int>("getStatusCode", "()I", Array.Empty<object>());

        public string getStatusMessage() => 
            base.InvokeCall<string>("getStatusMessage", "()Ljava/lang/String;", Array.Empty<object>());

        public int hashCode() => 
            base.InvokeCall<int>("hashCode", "()I", Array.Empty<object>());

        public bool hasResolution() => 
            base.InvokeCall<bool>("hasResolution", "()Z", Array.Empty<object>());

        public bool isCanceled() => 
            base.InvokeCall<bool>("isCanceled", "()Z", Array.Empty<object>());

        public bool isInterrupted() => 
            base.InvokeCall<bool>("isInterrupted", "()Z", Array.Empty<object>());

        public bool isSuccess() => 
            base.InvokeCall<bool>("isSuccess", "()Z", Array.Empty<object>());

        public void startResolutionForResult(object arg_object_1, int arg_int_2)
        {
            object[] args = new object[] { arg_object_1, arg_int_2 };
            base.InvokeCallVoid("startResolutionForResult", "(Landroid/app/Activity;I)V", args);
        }

        public string toString() => 
            base.InvokeCall<string>("toString", "()Ljava/lang/String;", Array.Empty<object>());

        public void writeToParcel(object arg_object_1, int arg_int_2)
        {
            object[] args = new object[] { arg_object_1, arg_int_2 };
            base.InvokeCallVoid("writeToParcel", "(Landroid/os/Parcel;I)V", args);
        }

        public static object CREATOR =>
            JavaObjWrapper.GetStaticObjectField<object>("com/google/android/gms/common/api/Status", "CREATOR", "Landroid/os/Parcelable$Creator;");

        public static string NULL =>
            JavaObjWrapper.GetStaticStringField("com/google/android/gms/common/api/Status", "NULL");

        public static int CONTENTS_FILE_DESCRIPTOR =>
            JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/api/Status", "CONTENTS_FILE_DESCRIPTOR");

        public static int PARCELABLE_WRITE_RETURN_VALUE =>
            JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/api/Status", "PARCELABLE_WRITE_RETURN_VALUE");
    }
}

