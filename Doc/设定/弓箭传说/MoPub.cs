using System;

public class MoPub : MoPubAndroid
{
    private static string _sdkName;

    public static string SdkName
    {
        get
        {
            if (_sdkName == null)
            {
            }
            return (_sdkName = MoPubAndroid.GetSdkName().Replace("+unity", string.Empty));
        }
    }
}

