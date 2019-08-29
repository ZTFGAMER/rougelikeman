namespace Facebook.Unity.Example
{
    using Facebook.Unity;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    internal sealed class MainMenu : MenuBase
    {
        private void CallFBLogin()
        {
            List<string> permissions = new List<string> { 
                "public_profile",
                "email",
                "user_friends"
            };
            FB.LogInWithReadPermissions(permissions, new FacebookDelegate<ILoginResult>(this.HandleResult));
        }

        private void CallFBLoginForPublish()
        {
            List<string> permissions = new List<string> { "publish_actions" };
            FB.LogInWithPublishPermissions(permissions, new FacebookDelegate<ILoginResult>(this.HandleResult));
        }

        private void CallFBLogout()
        {
            FB.LogOut();
        }

        protected override void GetGui()
        {
            GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
            bool flag = GUI.get_enabled();
            if (base.Button("FB.Init"))
            {
                FB.Init(new InitDelegate(this.OnInitComplete), new HideUnityDelegate(this.OnHideUnity), null);
                base.Status = "FB.Init() called with " + FB.AppId;
            }
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUI.set_enabled(flag && FB.IsInitialized);
            if (base.Button("Login"))
            {
                this.CallFBLogin();
                base.Status = "Login called";
            }
            GUI.set_enabled(FB.IsLoggedIn);
            if (base.Button("Get publish_actions"))
            {
                this.CallFBLoginForPublish();
                base.Status = "Login (for publish_actions) called";
            }
            GUILayoutOption[] optionArray1 = new GUILayoutOption[] { GUILayout.MinWidth((float) ConsoleBase.MarginFix) };
            GUILayout.Label(GUIContent.none, optionArray1);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayoutOption[] optionArray2 = new GUILayoutOption[] { GUILayout.MinWidth((float) ConsoleBase.MarginFix) };
            GUILayout.Label(GUIContent.none, optionArray2);
            GUILayout.EndHorizontal();
            if (base.Button("Logout"))
            {
                this.CallFBLogout();
                base.Status = "Logout called";
            }
            GUI.set_enabled(flag && FB.IsInitialized);
            if (base.Button("Share Dialog"))
            {
                base.SwitchMenu(typeof(DialogShare));
            }
            if (base.Button("App Requests"))
            {
                base.SwitchMenu(typeof(AppRequests));
            }
            if (base.Button("Graph Request"))
            {
                base.SwitchMenu(typeof(GraphRequest));
            }
            if (Constants.IsWeb && base.Button("Pay"))
            {
                base.SwitchMenu(typeof(Pay));
            }
            if (base.Button("App Events"))
            {
                base.SwitchMenu(typeof(AppEvents));
            }
            if (base.Button("App Links"))
            {
                base.SwitchMenu(typeof(AppLinks));
            }
            if (Constants.IsMobile && base.Button("App Invites"))
            {
                base.SwitchMenu(typeof(AppInvites));
            }
            if (Constants.IsMobile && base.Button("Access Token"))
            {
                base.SwitchMenu(typeof(AccessTokenMenu));
            }
            GUILayout.EndVertical();
            GUI.set_enabled(flag);
        }

        private void OnHideUnity(bool isGameShown)
        {
            base.Status = "Success - Check log for details";
            base.LastResponse = $"Success Response: OnHideUnity Called {isGameShown}
";
            LogView.AddLog("Is game shown: " + isGameShown);
        }

        private void OnInitComplete()
        {
            base.Status = "Success - Check log for details";
            base.LastResponse = "Success Response: OnInitComplete Called\n";
            LogView.AddLog($"OnInitCompleteCalled IsLoggedIn='{FB.IsLoggedIn}' IsInitialized='{FB.IsInitialized}'");
            if (AccessToken.CurrentAccessToken != null)
            {
                LogView.AddLog(AccessToken.CurrentAccessToken.ToString());
            }
        }

        protected override bool ShowBackButton() => 
            false;
    }
}

