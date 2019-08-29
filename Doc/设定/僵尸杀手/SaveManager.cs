using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveManager
{
    private static bool debug = true;

    public static bool Load<T>(string path, ref T data)
    {
        try
        {
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate))
                {
                    data = (T) formatter.Deserialize(stream);
                    if (debug)
                    {
                        Debug.Log("Data Loaded");
                    }
                }
                return true;
            }
            return false;
        }
        catch (Exception exception)
        {
            Debug.Log(exception.Message);
            return false;
        }
    }

    public static void RemoveData(string path)
    {
        File.Delete(path);
        if (debug)
        {
            Debug.Log("Data Removed");
        }
    }

    public static void Save<T>(T data, string path)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate))
        {
            formatter.Serialize(stream, data);
            if (debug)
            {
                Debug.Log("Data Saved");
            }
        }
    }
}

