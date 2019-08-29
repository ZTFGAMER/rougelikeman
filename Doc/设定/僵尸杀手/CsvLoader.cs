using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using UnityEngine;

public static class CsvLoader
{
    private static bool CustomTryParse<T>(string input, out T output)
    {
        TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
        if ((converter != null) && converter.IsValid(input))
        {
            output = (T) converter.ConvertFromString(input);
            return true;
        }
        output = default(T);
        return false;
    }

    private static string[] SplitLines(TextAsset textAsset)
    {
        char[] separator = new char[] { '\n' };
        return textAsset.text.Split(separator);
    }

    public static void SplitText<T>(TextAsset textAsset, char separator, ref T[,] array)
    {
        string[] strArray = SplitLines(textAsset);
        for (int i = 0; i < strArray.GetLength(0); i++)
        {
            char[] chArray1 = new char[] { separator };
            string[] strArray2 = strArray[i].Split(chArray1);
            for (int j = 0; j < strArray2.Length; j++)
            {
                CustomTryParse<T>(strArray2[j], out array[i, j]);
            }
        }
    }
}

