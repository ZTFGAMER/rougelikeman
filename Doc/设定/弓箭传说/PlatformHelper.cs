using System;
using UnityEngine;

public class PlatformHelper
{
    public const string IOS_APPID = "1453651052";
    private const float FringeHeight = -55f;
    private const float BottomHeight = 30f;

    public static ushort GetAppVersionCode() => 
        2;

    public static string GetAppVersionName() => 
        "1.0.3";

    public static float GetBottomHeight()
    {
        if (IsFringe())
        {
            return 30f;
        }
        return 0f;
    }

    public static bool GetFlagShip() => 
        false;

    public static float GetFringeHeight()
    {
        if (IsFringe())
        {
            return -55f;
        }
        return 0f;
    }

    public static int GetPlatformID() => 
        1;

    public static string GetUUID() => 
        SystemInfo.deviceUniqueIdentifier;

    public static bool IsAndroid() => 
        true;

    public static bool IsEditor() => 
        false;

    private static bool IsFringe() => 
        false;

    public static bool IsIOS() => 
        false;
}

