namespace Facebook.Unity.Example
{
    using Facebook.Unity;
    using System;

    internal class AppLinks : MenuBase
    {
        protected override void GetGui()
        {
            if (base.Button("Get App Link"))
            {
                FB.GetAppLink(new FacebookDelegate<IAppLinkResult>(this.HandleResult));
            }
            if (Constants.IsMobile && base.Button("Fetch Deferred App Link"))
            {
                FB.Mobile.FetchDeferredAppLinkData(new FacebookDelegate<IAppLinkResult>(this.HandleResult));
            }
        }
    }
}

