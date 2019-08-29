namespace Facebook.Unity.Example
{
    using Facebook.Unity;
    using System;

    internal class AccessTokenMenu : MenuBase
    {
        protected override void GetGui()
        {
            if (base.Button("Refresh Access Token"))
            {
                FB.Mobile.RefreshCurrentAccessToken(new FacebookDelegate<IAccessTokenRefreshResult>(this.HandleResult));
            }
        }
    }
}

