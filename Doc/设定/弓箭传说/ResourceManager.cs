using DG.Tweening;
using Dxx;
using Dxx.Util;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

public class ResourceManager
{
    private const string ATLAS_PATH = "Atlas";
    private const float remove_time = 300f;
    private static Dictionary<string, ResourceData> mAtlasList = new Dictionary<string, ResourceData>();
    private static StringBuilder strTemp = new StringBuilder();
    private static Dictionary<string, AssetBundle> bundleDict = new Dictionary<string, AssetBundle>();
    [CompilerGenerated]
    private static TweenCallback <>f__am$cache0;

    protected static void ClearPool()
    {
        GameObjectPool.Clear();
    }

    private static AssetBundle GetAssetBundle(string bundlePath)
    {
        if (!bundleDict.ContainsKey(bundlePath))
        {
            AssetBundle bundle = AssetBundle.LoadFromFile(bundlePath);
            bundleDict.Add(bundlePath, bundle);
        }
        return bundleDict[bundlePath];
    }

    private static DxxSpriteAtlas GetAtlas(string name)
    {
        if (mAtlasList.TryGetValue(name, out ResourceData data))
        {
            if (data.atlas != null)
            {
                data.time = Time.realtimeSinceStartup;
            }
            else
            {
                data.atlas = Load<DxxSpriteAtlas>(name);
                data.time = Time.realtimeSinceStartup;
            }
        }
        else
        {
            data = new ResourceData {
                atlas = Load<DxxSpriteAtlas>(name),
                time = Time.realtimeSinceStartup
            };
            mAtlasList.Add(name, data);
        }
        return data.atlas;
    }

    public static Sprite GetSprite(string atlasName, string spriteName)
    {
        atlasName = atlasName.ToLower();
        strTemp.Clear();
        strTemp.AppendFormat("{0}/{1}", "Atlas", atlasName);
        string name = strTemp.ToString();
        DxxSpriteAtlas atlas = GetAtlas(name);
        if (atlas == null)
        {
            object[] args = new object[] { name };
            SdkManager.Bugly_Report("ResourceManager.GetSprite", Utils.FormatString("GetAtlas[{0}] is not found!", args));
        }
        Sprite sprite = null;
        try
        {
            return atlas.GetSprite(spriteName);
        }
        catch
        {
            object[] args = new object[] { name, spriteName };
            SdkManager.Bugly_Report("ResourceManager.GetSprite", Utils.FormatString("GetAtlas[{0}] try sprite:{1} is not found!", args));
        }
        if (sprite == null)
        {
            bool flag = true;
            if ((spriteName != null) && (spriteName == "element1201"))
            {
                flag = false;
            }
            if (flag)
            {
                object[] args = new object[] { name, spriteName };
                SdkManager.Bugly_Report("ResourceManager.GetSprite", Utils.FormatString("GetAtlas[{0}] sprite:{1} is not found!", args));
            }
        }
        return sprite;
    }

    public static void Init()
    {
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = new TweenCallback(null, <Init>m__0);
        }
        TweenSettingsExtensions.SetUpdate<Sequence>(TweenSettingsExtensions.SetLoops<Sequence>(TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(DOTween.Sequence(), 33f), <>f__am$cache0), -1), true);
    }

    public static T Load<T>(string path) where T: Object => 
        Resources.Load<T>(path);

    public static void LoadAnsyc<T>(string path, Action<T> onLoadComplete) where T: Object
    {
        object[] args = new object[] { path };
        GameObject obj2 = new GameObject(Utils.FormatString("ResourceManager.LoadAnsyc[{0}]", args));
    }

    public static void Release(Object assetToUnload)
    {
        Resources.UnloadAsset(assetToUnload);
    }

    public static bool TryLoad(string path) => 
        (Resources.Load(path) != null);

    public static bool TryLoad<T>(string path, out T t) where T: Object
    {
        T local = Resources.Load<T>(path);
        if (local != null)
        {
            t = local;
            return true;
        }
        t = null;
        return false;
    }

    public static void UnloadBundleAsset(Object assetToUnload)
    {
    }

    public static void UnloadUnused()
    {
        ClearPool();
        Resources.UnloadUnusedAssets();
    }

    private class ResourceData
    {
        public DxxSpriteAtlas atlas;
        public float time;
    }
}

