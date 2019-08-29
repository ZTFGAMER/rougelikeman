using System;
using System.Text;
using UnityEngine;

public static class PlayerPrefsArray
{
    public static bool[] GetBoolArray(string key)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            return new bool[0];
        }
        char[] separator = new char[] { "|"[0] };
        string[] strArray = PlayerPrefs.GetString(key).Split(separator);
        bool[] flagArray = new bool[strArray.Length];
        for (int i = 0; i < strArray.Length; i++)
        {
            flagArray[i] = Convert.ToBoolean(strArray[i]);
        }
        return flagArray;
    }

    public static bool[] GetBoolArray(string key, bool defaultValue, int defaultSize)
    {
        if (PlayerPrefs.HasKey(key))
        {
            return GetBoolArray(key);
        }
        bool[] flagArray = new bool[defaultSize];
        for (int i = 0; i < defaultSize; i++)
        {
            flagArray[i] = defaultValue;
        }
        return flagArray;
    }

    public static float[] GetFloatArray(string key)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            return new float[0];
        }
        char[] separator = new char[] { "|"[0] };
        string[] strArray = PlayerPrefs.GetString(key).Split(separator);
        float[] numArray = new float[strArray.Length];
        for (int i = 0; i < strArray.Length; i++)
        {
            numArray[i] = Convert.ToSingle(strArray[i]);
        }
        return numArray;
    }

    public static float[] GetFloatArray(string key, float defaultValue, int defaultSize)
    {
        if (PlayerPrefs.HasKey(key))
        {
            return GetFloatArray(key);
        }
        float[] numArray = new float[defaultSize];
        for (int i = 0; i < defaultSize; i++)
        {
            numArray[i] = defaultValue;
        }
        return numArray;
    }

    public static int[] GetIntArray(string key)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            return new int[0];
        }
        char[] separator = new char[] { "|"[0] };
        string[] strArray = PlayerPrefs.GetString(key).Split(separator);
        int[] numArray = new int[strArray.Length];
        for (int i = 0; i < strArray.Length; i++)
        {
            numArray[i] = Convert.ToInt32(strArray[i]);
        }
        return numArray;
    }

    public static int[] GetIntArray(string key, int defaultValue, int defaultSize)
    {
        if (PlayerPrefs.HasKey(key))
        {
            return GetIntArray(key);
        }
        int[] numArray = new int[defaultSize];
        for (int i = 0; i < defaultSize; i++)
        {
            numArray[i] = defaultValue;
        }
        return numArray;
    }

    public static string[] GetStringArray(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            char[] separator = new char[] { "\n"[0] };
            return PlayerPrefs.GetString(key).Split(separator);
        }
        return new string[0];
    }

    public static string[] GetStringArray(string key, char separator)
    {
        if (PlayerPrefs.HasKey(key))
        {
            char[] chArray1 = new char[] { separator };
            return PlayerPrefs.GetString(key).Split(chArray1);
        }
        return new string[0];
    }

    public static string[] GetStringArray(string key, string defaultValue, int defaultSize) => 
        GetStringArray(key, "\n"[0], defaultValue, defaultSize);

    public static string[] GetStringArray(string key, char separator, string defaultValue, int defaultSize)
    {
        if (PlayerPrefs.HasKey(key))
        {
            char[] chArray1 = new char[] { separator };
            return PlayerPrefs.GetString(key).Split(chArray1);
        }
        string[] strArray = new string[defaultSize];
        for (int i = 0; i < defaultSize; i++)
        {
            strArray[i] = defaultValue;
        }
        return strArray;
    }

    public static Vector3 GetVector3(string key)
    {
        float[] floatArray = GetFloatArray(key);
        if (floatArray.Length < 3)
        {
            return Vector3.zero;
        }
        return new Vector3(floatArray[0], floatArray[1], floatArray[2]);
    }

    public static bool SetBoolArray(string key, params bool[] boolArray)
    {
        if (boolArray.Length == 0)
        {
            return false;
        }
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < (boolArray.Length - 1); i++)
        {
            builder.Append(boolArray[i]).Append("|");
        }
        builder.Append(boolArray[boolArray.Length - 1]);
        try
        {
            PlayerPrefs.SetString(key, builder.ToString());
        }
        catch (Exception)
        {
            return false;
        }
        return true;
    }

    public static bool SetFloatArray(string key, params float[] floatArray)
    {
        if (floatArray.Length == 0)
        {
            return false;
        }
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < (floatArray.Length - 1); i++)
        {
            builder.Append(floatArray[i]).Append("|");
        }
        builder.Append(floatArray[floatArray.Length - 1]);
        try
        {
            PlayerPrefs.SetString(key, builder.ToString());
        }
        catch (Exception)
        {
            return false;
        }
        return true;
    }

    public static bool SetIntArray(string key, params int[] intArray)
    {
        if (intArray.Length == 0)
        {
            return false;
        }
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < (intArray.Length - 1); i++)
        {
            builder.Append(intArray[i]).Append("|");
        }
        builder.Append(intArray[intArray.Length - 1]);
        try
        {
            PlayerPrefs.SetString(key, builder.ToString());
        }
        catch (Exception)
        {
            return false;
        }
        return true;
    }

    public static bool SetStringArray(string key, params string[] stringArray)
    {
        if (!SetStringArray(key, "\n"[0], stringArray))
        {
            return false;
        }
        return true;
    }

    public static bool SetStringArray(string key, char separator, params string[] stringArray)
    {
        if (stringArray.Length == 0)
        {
            return false;
        }
        try
        {
            PlayerPrefs.SetString(key, string.Join(separator.ToString(), stringArray));
        }
        catch (Exception)
        {
            return false;
        }
        return true;
    }

    public static bool SetVector3(string key, Vector3 vector)
    {
        float[] floatArray = new float[] { vector.x, vector.y, vector.z };
        return SetFloatArray(key, floatArray);
    }
}

