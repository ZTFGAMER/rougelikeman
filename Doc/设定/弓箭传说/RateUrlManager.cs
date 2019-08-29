using System;
using UnityEngine;

public class RateUrlManager
{
    private static string getAdUrl() => 
        "https://play.google.com/store/apps/details?id=com.mavis.slidey";

    private static string getAppUrl() => 
        "https://play.google.com/store/apps/details?id=com.habby.archero";

    public static void OpenAdUrl()
    {
        Application.OpenURL(getAdUrl());
    }

    public static void OpenAppUrl()
    {
        Application.OpenURL(getAppUrl());
    }
}

