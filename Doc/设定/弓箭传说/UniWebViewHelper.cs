using System;
using System.IO;
using UnityEngine;

public class UniWebViewHelper
{
    public static string PersistentDataURLForPath(string path) => 
        Path.Combine("file://" + Application.persistentDataPath, path);

    public static string StreamingAssetURLForPath(string path) => 
        Path.Combine("file:///android_asset/", path);
}

