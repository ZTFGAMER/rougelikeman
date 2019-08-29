using Dxx.Util;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

public class FileUtils
{
    public const string File_Equip = "localequip.txt";
    public const string File_Card = "File_Card";
    public const string File_Active = "File_Active";
    public const string File_Currency = "File_Currency";
    public const string File_Challenge = "File_Challenge";
    public const string File_TimeBox = "File_TimeBox";
    public const string File_Stage = "File_Stage";
    public const string File_Achieve = "File_Achieve1";
    public const string File_MysticShop = "File_MysticShop";
    public const string File_Extra = "File_Extra";
    public const string File_BoxDrop = "File_BoxDrop";
    public const string File_CardDrop = "File_CardDrop";
    public const string File_FakerStageDrop = "File_FakerStageDrop";
    public const string File_FakerEquipDrop = "File_FakerEquipDrop";
    public const string File_FakerCardDrop = "File_FakerCardDrop";
    public const string File_Shop = "File_Shop";
    public const string File_Mail = "mail.txt";
    public const string File_Harvest = "File_Harvest";
    public const string File_LocalSave = "localsave.txt";
    public static string EncryptKey = "4ptjerlkgjlk34jylkej4rgklj4klyj";
    private static string _localpath = string.Empty;
    private static string _FilesDir;
    private static string _CacheDir;
    private static string _ExternalFilesDir;
    private static string _ExternalCacheDir;

    public static void ClearFile(string name)
    {
        StreamWriter writer = new FileInfo(GetDataFolder() + "/" + name).CreateText();
        writer.Close();
        writer.Dispose();
    }

    public static void CreateFile(string path, string name, string info)
    {
        StreamWriter writer;
        FileInfo info2 = new FileInfo(path + "/" + name);
        if (!info2.Exists)
        {
            writer = info2.CreateText();
        }
        else
        {
            writer = info2.AppendText();
        }
        writer.Write(info + "\r\n");
        writer.Close();
        writer.Dispose();
    }

    public static void CreateFileOverride(string name, byte[] info)
    {
        string str = name;
        string str2 = name;
        char[] separator = new char[] { '/' };
        string[] strArray = name.Split(separator);
        name = string.Empty;
        int index = 0;
        int length = strArray.Length;
        while (index < length)
        {
            strArray[index] = Encrypt(strArray[index]);
            if (index < (length - 1))
            {
                object[] objArray1 = new object[] { strArray[index] };
                name = name + Utils.FormatStringThread("{0}/", objArray1);
            }
            else
            {
                name = name + strArray[index];
            }
            index++;
        }
        if (strArray.Length > 1)
        {
            int num3 = 0;
            int num4 = strArray.Length - 1;
            while (num3 < num4)
            {
                string dataFolder = GetDataFolder();
                int num5 = 0;
                int num6 = num3;
                while (num5 <= num6)
                {
                    object[] objArray2 = new object[] { dataFolder, strArray[num5] };
                    dataFolder = Utils.FormatStringThread("{0}/{1}", objArray2);
                    if (!Directory.Exists(dataFolder))
                    {
                        Directory.CreateDirectory(dataFolder);
                    }
                    num5++;
                }
                num3++;
            }
        }
        object[] args = new object[] { GetDataFolder(), name };
        FileStream stream = new FileStream(Utils.FormatStringThread("{0}/{1}", args), FileMode.Create);
        info = Convert.FromBase64String(Encrypt(Convert.ToBase64String(info)));
        stream.Write(info, 0, info.Length);
        stream.Close();
    }

    public static void CreateFileOverride(string dir, string name, string info)
    {
        object[] args = new object[] { GetDataFolder(), dir };
        string path = Utils.FormatStringThread("{0}/{1}", args);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        object[] objArray2 = new object[] { path, name };
        using (StreamWriter writer = new StreamWriter(Utils.FormatStringThread("{0}/{1}", objArray2), false))
        {
            writer.Write(info);
        }
    }

    public static void CreateWriteFile(string path, string info, bool isRelace = true)
    {
        StreamWriter writer;
        FileInfo info2 = new FileInfo(path);
        if (info2.Exists && isRelace)
        {
            File.Delete(path);
        }
        if (!info2.Exists)
        {
            writer = info2.CreateText();
        }
        else
        {
            writer = info2.AppendText();
        }
        writer.WriteLine(info);
        writer.Close();
        writer.Dispose();
    }

    private static void CreateWriteXML<T>(string path, T t)
    {
        string text = SerializeObject<T>(t);
        string s = string.Empty;
        if (!isEncrypt())
        {
            s = text;
        }
        else
        {
            s = NetEncrypt.Encrypt_UTF8(text, EncryptKey);
        }
        using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
        {
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            stream.Write(bytes, 0, bytes.Length);
            stream.Flush();
            stream.Close();
            stream.Dispose();
        }
    }

    public static void DeleteFile(string path, string name)
    {
        object[] args = new object[] { path, name };
        path = Utils.FormatStringThread("{0}/{1}", args);
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    public static string DesDecrypt(string value)
    {
        if (!isEncrypt())
        {
            return value;
        }
        string str = string.Empty;
        try
        {
            str = NetEncrypt.DesDecrypt(value, EncryptKey);
        }
        catch
        {
        }
        return str;
    }

    private static object DeserializeObject<T>(string pXmlizedString)
    {
        try
        {
            return JsonConvert.DeserializeObject<T>(pXmlizedString);
        }
        catch
        {
            object[] args = new object[] { pXmlizedString };
            SdkManager.Bugly_Report("FileUtils.DeserializeObject", Utils.FormatString("string:{0} .... error", args));
            return null;
        }
    }

    public static string Encrypt(string value)
    {
        if (!isEncrypt())
        {
            return value;
        }
        string str = string.Empty;
        try
        {
            str = NetEncrypt.Encrypt_UTF8(value, EncryptKey);
        }
        catch
        {
        }
        return str;
    }

    public static LocalSave.BattleInBase GetBattleIn() => 
        ReadXmlFile<LocalSave.BattleInBase>(GetFullPath(LocalSave.BattleInBase.GetFileName(LocalSave.Instance.GetServerUserID())));

    public static string GetConfig(string name)
    {
        byte[] fileBytes = GetFileBytes("data/config", name);
        string str = string.Empty;
        if (fileBytes == null)
        {
            return str;
        }
        try
        {
            return Encoding.Default.GetString(fileBytes);
        }
        catch
        {
            return string.Empty;
        }
    }

    public static string GetDataFolder()
    {
        if (_FilesDir == null)
        {
            try
            {
                using (AndroidJavaClass class2 = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    using (AndroidJavaObject obj2 = class2.GetStatic<AndroidJavaObject>("currentActivity"))
                    {
                        using (AndroidJavaObject obj3 = obj2.Call<AndroidJavaObject>("getFilesDir", Array.Empty<object>()))
                        {
                            _FilesDir = obj3.Call<string>("getCanonicalPath", Array.Empty<object>());
                        }
                        using (AndroidJavaObject obj4 = obj2.Call<AndroidJavaObject>("getCacheDir", Array.Empty<object>()))
                        {
                            _CacheDir = obj4.Call<string>("getCanonicalPath", Array.Empty<object>());
                        }
                        using (AndroidJavaObject obj5 = obj2.Call<AndroidJavaObject>("getExternalFilesDir", null))
                        {
                            _ExternalFilesDir = obj5.Call<string>("getCanonicalPath", Array.Empty<object>());
                        }
                        using (AndroidJavaObject obj6 = obj2.Call<AndroidJavaObject>("getExternalCacheDir", Array.Empty<object>()))
                        {
                            _ExternalCacheDir = obj6.Call<string>("getCanonicalPath", Array.Empty<object>());
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                _FilesDir = Application.persistentDataPath;
                Debug.LogException(exception);
            }
        }
        return _FilesDir;
    }

    private static string GetEncrpytPath(string path)
    {
        string str = path;
        char[] separator = new char[] { '/' };
        string[] strArray = path.Split(separator);
        path = string.Empty;
        int index = 0;
        int length = strArray.Length;
        while (index < length)
        {
            strArray[index] = Encrypt(strArray[index]);
            if (index < (length - 1))
            {
                object[] args = new object[] { strArray[index] };
                path = path + Utils.FormatStringThread("{0}/", args);
            }
            else
            {
                path = path + strArray[index];
            }
            index++;
        }
        return path;
    }

    private static string GetEncryptFullPath(string path)
    {
        object[] args = new object[] { GetDataFolder(), GetEncrpytPath(path) };
        return Utils.FormatStringThread("{0}/{1}", args);
    }

    public static byte[] GetFileBytes(string dir, string name)
    {
        byte[] buffer = null;
        try
        {
            object[] args = new object[] { dir };
            if (!Directory.Exists(GetEncryptFullPath(Utils.FormatStringThread("{0}", args))))
            {
                return null;
            }
            object[] objArray2 = new object[] { dir, name };
            string encryptFullPath = GetEncryptFullPath(Utils.FormatStringThread("{0}/{1}", objArray2));
            FileInfo info = new FileInfo(encryptFullPath);
            if (!info.Exists)
            {
                return null;
            }
            try
            {
                FileStream stream = info.OpenRead();
                buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                stream.Close();
            }
            catch (Exception)
            {
                object[] objArray3 = new object[] { encryptFullPath };
                SdkManager.Bugly_Report("FileUtils.GetFileBytes", Utils.FormatStringThread("FileUtils {0} get info failed.", objArray3));
            }
        }
        catch
        {
        }
        return Convert.FromBase64String(DesDecrypt(Convert.ToBase64String(buffer)));
    }

    public static string GetFullPath(string name)
    {
        string path = GetPath();
        try
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        catch
        {
        }
        object[] args = new object[] { path, name };
        return Utils.FormatStringThread("{0}/{1}", args);
    }

    public static string GetPath() => 
        LocalPath;

    private static string GetPathInternal()
    {
        object[] args = new object[] { GetDataFolder() };
        return Utils.FormatStringThread("{0}/Save", args);
    }

    public static T GetXml<T>(string name) where T: new() => 
        ReadXmlFile<T>(GetFullPath(name));

    public static string GetXmlFileString(string path)
    {
        path = GetFullPath(path);
        FileInfo info = new FileInfo(path);
        if (!info.Exists)
        {
            return string.Empty;
        }
        string text = string.Empty;
        try
        {
            FileStream stream = info.OpenRead();
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            stream.Close();
            text = Encoding.UTF8.GetString(buffer);
        }
        catch (Exception)
        {
            SdkManager.Bugly_Report("FileUtils.GetXmlFileString", Utils.FormatString("path : ..... get info failed." + path, Array.Empty<object>()));
        }
        string str2 = string.Empty;
        try
        {
            string str3 = text;
            if (isEncrypt())
            {
                str3 = NetEncrypt.DesDecrypt(text, EncryptKey);
            }
            str2 = str3;
        }
        catch
        {
            object[] args = new object[] { path };
            SdkManager.Bugly_Report("FileUtils.GetXmlFileString", Utils.FormatString("GetXmlFileString failed!!!!!!!!!!!!!!!!!!!!!! path:{0}", args));
        }
        return str2;
    }

    private static bool isEncrypt() => 
        true;

    private static T ReadXmlFile<T>(string path) where T: new()
    {
        FileInfo info = new FileInfo(path);
        if (!info.Exists)
        {
            return Activator.CreateInstance<T>();
        }
        string text = string.Empty;
        try
        {
            FileStream stream = info.OpenRead();
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            stream.Close();
            text = Encoding.UTF8.GetString(buffer);
        }
        catch (Exception)
        {
            object[] args = new object[] { path };
            SdkManager.Bugly_Report("FileUtils.ReadXmlFile", Utils.FormatString("FileUtils {0} get info failed.", args));
        }
        T local = default(T);
        try
        {
            string pXmlizedString = string.Empty;
            if (!isEncrypt())
            {
                pXmlizedString = text;
                try
                {
                    local = (T) DeserializeObject<T>(pXmlizedString);
                }
                catch
                {
                    Debug.LogError("DeserializeObject error " + pXmlizedString);
                }
                return local;
            }
            try
            {
                pXmlizedString = NetEncrypt.DesDecrypt(text, EncryptKey);
            }
            catch
            {
                pXmlizedString = text;
            }
            local = (T) DeserializeObject<T>(pXmlizedString);
        }
        catch (Exception)
        {
            object[] args = new object[] { path };
            SdkManager.Bugly_Report("FileUtils.ReadXmlFile", Utils.FormatString("FileUtils.ReadXmlFile : {0}      DeserializeObject  false", args));
        }
        finally
        {
            if (local == null)
            {
                local = Activator.CreateInstance<T>();
            }
        }
        return local;
    }

    private static string SerializeObject<T>(object pObject)
    {
        string str = string.Empty;
        try
        {
            str = JsonConvert.SerializeObject(pObject);
        }
        catch (Exception exception)
        {
            SdkManager.Bugly_Report("FileUtils.SerializeObject", exception.ToString());
        }
        return str;
    }

    private static byte[] StringToUTF8ByteArray(string pXmlString)
    {
        UTF8Encoding encoding = new UTF8Encoding();
        return encoding.GetBytes(pXmlString);
    }

    private static string UTF8ByteArrayToString(byte[] characters)
    {
        UTF8Encoding encoding = new UTF8Encoding();
        return encoding.GetString(characters);
    }

    public static void Write(string name, string str)
    {
        ClearFile(name);
        CreateFile(GetDataFolder(), name, str);
    }

    public static void WriteBattleIn(LocalSave.BattleInBase data)
    {
        LocalSave.Instance.DoThreadSave(LocalSave.EThreadWriteType.eBattle);
    }

    public static void WriteBattleInThread(LocalSave.BattleInBase data)
    {
        CreateWriteXML<LocalSave.BattleInBase>(GetFullPath(LocalSave.BattleInBase.GetFileName(LocalSave.Instance.GetServerUserID())), data);
    }

    public static void WriteEquip<T>(string name, T data)
    {
        LocalSave.Instance.DoThreadSave(LocalSave.EThreadWriteType.eEquip);
    }

    public static void WriteError(object str)
    {
        Write("ErrorLog.txt", str.ToString());
    }

    public static void WriteXml<T>(string name, T data)
    {
        LocalSave.Instance.DoThreadSave(LocalSave.EThreadWriteType.eLocal);
    }

    public static void WriteXmlThread<T>(string name, T data)
    {
        CreateWriteXML<T>(GetFullPath(name), data);
    }

    public static string LocalPath
    {
        get
        {
            if (_localpath == string.Empty)
            {
                _localpath = GetPathInternal();
            }
            return _localpath;
        }
    }
}

