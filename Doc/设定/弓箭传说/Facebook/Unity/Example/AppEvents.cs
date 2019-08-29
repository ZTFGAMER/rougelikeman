namespace Facebook.Unity.Example
{
    using Facebook.Unity;
    using System;
    using System.Collections.Generic;

    internal class AppEvents : MenuBase
    {
        protected override void GetGui()
        {
            if (base.Button("Log FB App Event"))
            {
                base.Status = "Logged FB.AppEvent";
                Dictionary<string, object> parameters = new Dictionary<string, object> {
                    { 
                        "fb_description",
                        "Clicked 'Log AppEvent' button"
                    }
                };
                FB.LogAppEvent("fb_mobile_achievement_unlocked", null, parameters);
                LogView.AddLog("You may see results showing up at https://www.facebook.com/analytics/" + FB.AppId);
            }
        }
    }
}

