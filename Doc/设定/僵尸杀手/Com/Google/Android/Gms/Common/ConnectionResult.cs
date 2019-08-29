namespace Com.Google.Android.Gms.Common
{
    using Google.Developers;
    using System;

    public class ConnectionResult : JavaObjWrapper
    {
        private const string CLASS_NAME = "com/google/android/gms/common/ConnectionResult";

        public ConnectionResult(int arg_int_1)
        {
            object[] args = new object[] { arg_int_1 };
            base.CreateInstance("com/google/android/gms/common/ConnectionResult", args);
        }

        public ConnectionResult(IntPtr ptr) : base(ptr)
        {
        }

        public ConnectionResult(int arg_int_1, object arg_object_2)
        {
            object[] args = new object[] { arg_int_1, arg_object_2 };
            base.CreateInstance("com/google/android/gms/common/ConnectionResult", args);
        }

        public ConnectionResult(int arg_int_1, object arg_object_2, string arg_string_3)
        {
            object[] args = new object[] { arg_int_1, arg_object_2, arg_string_3 };
            base.CreateInstance("com/google/android/gms/common/ConnectionResult", args);
        }

        public int describeContents() => 
            base.InvokeCall<int>("describeContents", "()I", new object[0]);

        public bool equals(object arg_object_1)
        {
            object[] args = new object[] { arg_object_1 };
            return base.InvokeCall<bool>("equals", "(Ljava/lang/Object;)Z", args);
        }

        public int getErrorCode() => 
            base.InvokeCall<int>("getErrorCode", "()I", new object[0]);

        public string getErrorMessage() => 
            base.InvokeCall<string>("getErrorMessage", "()Ljava/lang/String;", new object[0]);

        public object getResolution() => 
            base.InvokeCall<object>("getResolution", "()Landroid/app/PendingIntent;", new object[0]);

        public int hashCode() => 
            base.InvokeCall<int>("hashCode", "()I", new object[0]);

        public bool hasResolution() => 
            base.InvokeCall<bool>("hasResolution", "()Z", new object[0]);

        public bool isSuccess() => 
            base.InvokeCall<bool>("isSuccess", "()Z", new object[0]);

        public void startResolutionForResult(object arg_object_1, int arg_int_2)
        {
            object[] args = new object[] { arg_object_1, arg_int_2 };
            base.InvokeCallVoid("startResolutionForResult", "(Landroid/app/Activity;I)V", args);
        }

        public string toString() => 
            base.InvokeCall<string>("toString", "()Ljava/lang/String;", new object[0]);

        public void writeToParcel(object arg_object_1, int arg_int_2)
        {
            object[] args = new object[] { arg_object_1, arg_int_2 };
            base.InvokeCallVoid("writeToParcel", "(Landroid/os/Parcel;I)V", args);
        }

        public static int SUCCESS =>
            JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "SUCCESS");

        public static int SERVICE_MISSING =>
            JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "SERVICE_MISSING");

        public static int SERVICE_VERSION_UPDATE_REQUIRED =>
            JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "SERVICE_VERSION_UPDATE_REQUIRED");

        public static int SERVICE_DISABLED =>
            JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "SERVICE_DISABLED");

        public static int SIGN_IN_REQUIRED =>
            JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "SIGN_IN_REQUIRED");

        public static int INVALID_ACCOUNT =>
            JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "INVALID_ACCOUNT");

        public static int RESOLUTION_REQUIRED =>
            JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "RESOLUTION_REQUIRED");

        public static int NETWORK_ERROR =>
            JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "NETWORK_ERROR");

        public static int INTERNAL_ERROR =>
            JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "INTERNAL_ERROR");

        public static int SERVICE_INVALID =>
            JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "SERVICE_INVALID");

        public static int DEVELOPER_ERROR =>
            JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "DEVELOPER_ERROR");

        public static int LICENSE_CHECK_FAILED =>
            JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "LICENSE_CHECK_FAILED");

        public static int CANCELED =>
            JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "CANCELED");

        public static int TIMEOUT =>
            JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "TIMEOUT");

        public static int INTERRUPTED =>
            JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "INTERRUPTED");

        public static int API_UNAVAILABLE =>
            JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "API_UNAVAILABLE");

        public static int SIGN_IN_FAILED =>
            JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "SIGN_IN_FAILED");

        public static int SERVICE_UPDATING =>
            JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "SERVICE_UPDATING");

        public static int SERVICE_MISSING_PERMISSION =>
            JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "SERVICE_MISSING_PERMISSION");

        public static int DRIVE_EXTERNAL_STORAGE_REQUIRED =>
            JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "DRIVE_EXTERNAL_STORAGE_REQUIRED");

        public static object CREATOR =>
            JavaObjWrapper.GetStaticObjectField<object>("com/google/android/gms/common/ConnectionResult", "CREATOR", "Landroid/os/Parcelable$Creator;");

        public static string NULL =>
            JavaObjWrapper.GetStaticStringField("com/google/android/gms/common/ConnectionResult", "NULL");

        public static int CONTENTS_FILE_DESCRIPTOR =>
            JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "CONTENTS_FILE_DESCRIPTOR");

        public static int PARCELABLE_WRITE_RETURN_VALUE =>
            JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "PARCELABLE_WRITE_RETURN_VALUE");
    }
}

