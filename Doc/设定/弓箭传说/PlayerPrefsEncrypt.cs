using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class PlayerPrefsEncrypt
{
    private static string sKEY = "ZTdkNTNmNDE2NTM3MWM0NDFhNTEzNzU1";
    private static string sIV = "4rZymEMfa/PpeJ89qY4gyA==";

    private static string Decrypt(string encString)
    {
        RijndaelManaged managed = new RijndaelManaged {
            Padding = PaddingMode.Zeros,
            Mode = CipherMode.CBC,
            KeySize = 0x80,
            BlockSize = 0x80
        };
        byte[] bytes = Encoding.UTF8.GetBytes(sKEY);
        byte[] rgbIV = Convert.FromBase64String(sIV);
        ICryptoTransform transform = managed.CreateDecryptor(bytes, rgbIV);
        byte[] buffer = Convert.FromBase64String(encString);
        byte[] buffer4 = new byte[buffer.Length];
        MemoryStream stream = new MemoryStream(buffer);
        new CryptoStream(stream, transform, CryptoStreamMode.Read).Read(buffer4, 0, buffer4.Length);
        return Encoding.UTF8.GetString(buffer4).TrimEnd(new char[1]);
    }

    public static void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }

    public static void DeleteKey(string key)
    {
        PlayerPrefs.DeleteKey(GetHash(key));
    }

    private static string Encrypt(string rawString)
    {
        RijndaelManaged managed = new RijndaelManaged {
            Padding = PaddingMode.Zeros,
            Mode = CipherMode.CBC,
            KeySize = 0x80,
            BlockSize = 0x80
        };
        byte[] bytes = Encoding.UTF8.GetBytes(sKEY);
        byte[] rgbIV = Convert.FromBase64String(sIV);
        ICryptoTransform transform = managed.CreateEncryptor(bytes, rgbIV);
        MemoryStream stream = new MemoryStream();
        CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Write);
        byte[] buffer = Encoding.UTF8.GetBytes(rawString);
        stream2.Write(buffer, 0, buffer.Length);
        stream2.FlushFinalBlock();
        return Convert.ToBase64String(stream.ToArray());
    }

    public static bool GetBool(string key, bool defaultValue = false)
    {
        string str = GetString(key, defaultValue.ToString());
        bool result = defaultValue;
        bool.TryParse(str, out result);
        return result;
    }

    public static float GetFloat(string key, float defaultValue = 0f)
    {
        string s = GetString(key, defaultValue.ToString());
        float result = defaultValue;
        float.TryParse(s, out result);
        return result;
    }

    private static string GetHash(string key)
    {
        byte[] buffer = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(key));
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < buffer.Length; i++)
        {
            builder.Append(buffer[i].ToString("x2"));
        }
        return builder.ToString();
    }

    public static int GetInt(string key, int defaultValue = 0)
    {
        string s = GetString(key, defaultValue.ToString());
        int result = defaultValue;
        int.TryParse(s, out result);
        return result;
    }

    public static long GetLong(string key, long defaultValue = 0L)
    {
        string s = GetString(key, defaultValue.ToString());
        long result = defaultValue;
        long.TryParse(s, out result);
        return result;
    }

    public static string GetString(string key, string defaultValue = "")
    {
        string str = defaultValue;
        string str2 = PlayerPrefs.GetString(GetHash(key), defaultValue.ToString());
        if (!str.Equals(str2))
        {
            str = Decrypt(str2);
        }
        return str;
    }

    public static uint GetUInt(string key, uint defaultValue = 0)
    {
        string s = GetString(key, defaultValue.ToString());
        uint result = defaultValue;
        uint.TryParse(s, out result);
        return result;
    }

    public static ulong GetULong(string key, ulong defaultValue = 0L)
    {
        string s = GetString(key, defaultValue.ToString());
        ulong result = defaultValue;
        ulong.TryParse(s, out result);
        return result;
    }

    public static bool HasKey(string key) => 
        PlayerPrefs.HasKey(GetHash(key));

    public static void Save()
    {
        PlayerPrefs.Save();
    }

    public static void SetBool(string key, bool val)
    {
        PlayerPrefs.SetString(GetHash(key), Encrypt(val.ToString()));
    }

    public static void SetFloat(string key, float val)
    {
        PlayerPrefs.SetString(GetHash(key), Encrypt(val.ToString()));
    }

    public static void SetInt(string key, int val)
    {
        PlayerPrefs.SetString(GetHash(key), Encrypt(val.ToString()));
    }

    public static void SetLong(string key, long val)
    {
        PlayerPrefs.SetString(GetHash(key), Encrypt(val.ToString()));
    }

    public static void SetString(string key, string val)
    {
        PlayerPrefs.SetString(GetHash(key), Encrypt(val));
    }

    public static void SetUInt(string key, uint val)
    {
        PlayerPrefs.SetString(GetHash(key), Encrypt(val.ToString()));
    }

    public static void SetULong(string key, ulong val)
    {
        PlayerPrefs.SetString(GetHash(key), Encrypt(val.ToString()));
    }
}

