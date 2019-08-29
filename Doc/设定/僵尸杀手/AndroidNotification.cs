using System;
using System.Runtime.InteropServices;
using UnityEngine;

internal class AndroidNotification
{
    private const string fullClassName = "com.ahg.uanotify.UnityNotification";
    private const string mainActivityClassName = "com.unity3d.player.UnityPlayerNativeActivity";

    public static void CancelAllNotifications()
    {
        AndroidJavaClass class2 = new AndroidJavaClass("com.ahg.uanotify.UnityNotification");
        if (class2 != null)
        {
            class2.CallStatic("CancelAll", new object[0]);
        }
    }

    public static void CancelNotification(int id)
    {
        AndroidJavaClass class2 = new AndroidJavaClass("com.ahg.uanotify.UnityNotification");
        if (class2 != null)
        {
            object[] args = new object[] { id };
            class2.CallStatic("CancelNotification", args);
        }
    }

    public static void SendNotification(int id, TimeSpan delay, string title, string message, string ticker)
    {
        SendNotification(id, (long) ((int) delay.TotalSeconds), title, message, ticker, Color.white, true, NotificationExecuteMode.Inexact, true, 2, true, 0x3e8L, 0x3e8L);
    }

    public static void SendNotification(int id, TimeSpan delay, string title, string message, string ticker, Color32 bgColor, bool lights = true, NotificationExecuteMode executeMode = 0)
    {
        SendNotification(id, (long) ((int) delay.TotalSeconds), title, message, ticker, bgColor, true, executeMode, true, 2, true, 0x3e8L, 0x3e8L);
    }

    public static void SendNotification(int id, TimeSpan delay, string title, string message, string ticker, Color32 bgColor, bool lights = true, NotificationExecuteMode executeMode = 0, bool sound = true, int soundIndex = 2)
    {
        SendNotification(id, (long) ((int) delay.TotalSeconds), title, message, ticker, bgColor, lights, executeMode, sound, soundIndex, true, 0x3e8L, 0x3e8L);
    }

    public static void SendNotification(int id, long delay, string title, string message, string ticker, Color32 bgColor, bool lights = true, NotificationExecuteMode executeMode = 0, bool sound = true, int soundIndex = 2, bool vibrate = true, long vibrateDelay = 0x3e8L, long vibrateTime = 0x3e8L)
    {
        AndroidJavaClass class2 = new AndroidJavaClass("com.ahg.uanotify.UnityNotification");
        if (class2 != null)
        {
            object[] args = new object[] { id, delay * 0x3e8L, title, message, ticker, !sound ? 0 : 1, soundIndex, !vibrate ? 0 : 1, vibrateDelay, vibrateTime, !lights ? 0 : 1, ((bgColor.r * 0x10000) + (bgColor.g * 0x100)) + bgColor.b, (int) executeMode, "com.unity3d.player.UnityPlayerNativeActivity" };
            class2.CallStatic("SetNotification", args);
        }
    }

    public enum NotificationExecuteMode
    {
        Inexact,
        Exact,
        ExactAndAllowWhileIdle
    }
}

