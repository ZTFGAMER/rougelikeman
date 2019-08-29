using Dxx.Util;
using System;
using UnityEngine;

public class Debugger
{
    public static bool enable = true;

    public static void Log(string value)
    {
        if (enable)
        {
            Debug.Log("Archero:" + value);
        }
    }

    public static void Log(Tag tag, string value)
    {
        object[] args = new object[] { tag.ToString(), value };
        Debug.Log(Utils.FormatString("Archero:{0} : {1}", args));
    }

    public static void Log(EntityBase entity, string value)
    {
    }

    public static void Log(string tag, string value)
    {
        object[] args = new object[] { tag, value };
        Debug.Log(Utils.FormatString("Archero {0} : {1}", args));
    }

    public static void LogEquipGet(string value)
    {
    }

    public static void LogFormat(string value, params object[] args)
    {
        if (enable)
        {
            Debug.LogFormat("Archero:" + value, args);
        }
    }

    public enum Tag
    {
        eHTTP,
        ePurchase,
        eTest
    }
}

