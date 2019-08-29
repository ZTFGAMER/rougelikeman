using System;
using UnityEngine;

public class Unity2AndroidHelper : CInstance<Unity2AndroidHelper>
{
    private AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    private AndroidJavaObject jo;

    public Unity2AndroidHelper()
    {
        this.jo = this.jc.GetStatic<AndroidJavaObject>("currentActivity");
    }

    public bool is_gp_avalible() => 
        this.jo.Call<bool>("is_gp_avalible", Array.Empty<object>());
}

