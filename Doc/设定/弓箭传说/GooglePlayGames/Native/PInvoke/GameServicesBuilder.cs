namespace GooglePlayGames.Native.PInvoke
{
    using AOT;
    using GooglePlayGames.Native.Cwrapper;
    using GooglePlayGames.OurUtils;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    internal class GameServicesBuilder : BaseReferenceHolder
    {
        [CompilerGenerated]
        private static Builder.OnAuthActionFinishedCallback <>f__mg$cache0;
        [CompilerGenerated]
        private static Builder.OnAuthActionStartedCallback <>f__mg$cache1;
        [CompilerGenerated]
        private static Builder.OnTurnBasedMatchEventCallback <>f__mg$cache2;
        [CompilerGenerated]
        private static Builder.OnMultiplayerInvitationEventCallback <>f__mg$cache3;

        private GameServicesBuilder(IntPtr selfPointer) : base(selfPointer)
        {
            InternalHooks.InternalHooks_ConfigureForUnityPlugin(base.SelfPtr(), "0.9.55");
        }

        internal void AddOauthScope(string scope)
        {
            Builder.GameServices_Builder_AddOauthScope(base.SelfPtr(), scope);
        }

        internal GameServices Build(PlatformConfiguration configRef)
        {
            IntPtr selfPointer = Builder.GameServices_Builder_Create(base.SelfPtr(), HandleRef.ToIntPtr(configRef.AsHandle()));
            if (selfPointer.Equals(IntPtr.Zero))
            {
                throw new InvalidOperationException("There was an error creating a GameServices object. Check for log errors from GamesNativeSDK");
            }
            return new GameServices(selfPointer);
        }

        protected override void CallDispose(HandleRef selfPointer)
        {
            Builder.GameServices_Builder_Dispose(selfPointer);
        }

        internal static GameServicesBuilder Create() => 
            new GameServicesBuilder(Builder.GameServices_Builder_Construct());

        internal void EnableSnapshots()
        {
            Builder.GameServices_Builder_EnableSnapshots(base.SelfPtr());
        }

        [MonoPInvokeCallback(typeof(Builder.OnAuthActionFinishedCallback))]
        private static void InternalAuthFinishedCallback(Types.AuthOperation op, CommonErrorStatus.AuthStatus status, IntPtr data)
        {
            AuthFinishedCallback callback = Callbacks.IntPtrToPermanentCallback<AuthFinishedCallback>(data);
            if (callback != null)
            {
                try
                {
                    callback(op, status);
                }
                catch (Exception exception)
                {
                    Logger.e("Error encountered executing InternalAuthFinishedCallback. Smothering to avoid passing exception into Native: " + exception);
                }
            }
        }

        [MonoPInvokeCallback(typeof(Builder.OnAuthActionStartedCallback))]
        private static void InternalAuthStartedCallback(Types.AuthOperation op, IntPtr data)
        {
            AuthStartedCallback callback = Callbacks.IntPtrToPermanentCallback<AuthStartedCallback>(data);
            try
            {
                if (callback != null)
                {
                    callback(op);
                }
            }
            catch (Exception exception)
            {
                Logger.e("Error encountered executing InternalAuthStartedCallback. Smothering to avoid passing exception into Native: " + exception);
            }
        }

        [MonoPInvokeCallback(typeof(Builder.OnMultiplayerInvitationEventCallback))]
        private static void InternalOnMultiplayerInvitationEventCallback(Types.MultiplayerEvent eventType, string matchId, IntPtr match, IntPtr userData)
        {
            Action<Types.MultiplayerEvent, string, MultiplayerInvitation> action = Callbacks.IntPtrToPermanentCallback<Action<Types.MultiplayerEvent, string, MultiplayerInvitation>>(userData);
            MultiplayerInvitation invitation = MultiplayerInvitation.FromPointer(match);
            try
            {
                if (action != null)
                {
                    action(eventType, matchId, invitation);
                }
            }
            catch (Exception exception)
            {
                Logger.e("Error encountered executing InternalOnMultiplayerInvitationEventCallback. Smothering to avoid passing exception into Native: " + exception);
            }
            finally
            {
                if (invitation != null)
                {
                    invitation.Dispose();
                }
            }
        }

        [MonoPInvokeCallback(typeof(Builder.OnTurnBasedMatchEventCallback))]
        private static void InternalOnTurnBasedMatchEventCallback(Types.MultiplayerEvent eventType, string matchId, IntPtr match, IntPtr userData)
        {
            Action<Types.MultiplayerEvent, string, NativeTurnBasedMatch> action = Callbacks.IntPtrToPermanentCallback<Action<Types.MultiplayerEvent, string, NativeTurnBasedMatch>>(userData);
            NativeTurnBasedMatch match2 = NativeTurnBasedMatch.FromPointer(match);
            try
            {
                if (action != null)
                {
                    action(eventType, matchId, match2);
                }
            }
            catch (Exception exception)
            {
                Logger.e("Error encountered executing InternalOnTurnBasedMatchEventCallback. Smothering to avoid passing exception into Native: " + exception);
            }
            finally
            {
                if (match2 != null)
                {
                    match2.Dispose();
                }
            }
        }

        internal void SetOnAuthFinishedCallback(AuthFinishedCallback callback)
        {
            if (<>f__mg$cache0 == null)
            {
                <>f__mg$cache0 = new Builder.OnAuthActionFinishedCallback(GameServicesBuilder.InternalAuthFinishedCallback);
            }
            Builder.GameServices_Builder_SetOnAuthActionFinished(base.SelfPtr(), <>f__mg$cache0, Callbacks.ToIntPtr(callback));
        }

        internal void SetOnAuthStartedCallback(AuthStartedCallback callback)
        {
            if (<>f__mg$cache1 == null)
            {
                <>f__mg$cache1 = new Builder.OnAuthActionStartedCallback(GameServicesBuilder.InternalAuthStartedCallback);
            }
            Builder.GameServices_Builder_SetOnAuthActionStarted(base.SelfPtr(), <>f__mg$cache1, Callbacks.ToIntPtr(callback));
        }

        internal void SetOnMultiplayerInvitationEventCallback(Action<Types.MultiplayerEvent, string, MultiplayerInvitation> callback)
        {
            IntPtr ptr = Callbacks.ToIntPtr(callback);
            if (<>f__mg$cache3 == null)
            {
                <>f__mg$cache3 = new Builder.OnMultiplayerInvitationEventCallback(GameServicesBuilder.InternalOnMultiplayerInvitationEventCallback);
            }
            Builder.GameServices_Builder_SetOnMultiplayerInvitationEvent(base.SelfPtr(), <>f__mg$cache3, ptr);
        }

        internal void SetOnTurnBasedMatchEventCallback(Action<Types.MultiplayerEvent, string, NativeTurnBasedMatch> callback)
        {
            IntPtr ptr = Callbacks.ToIntPtr(callback);
            if (<>f__mg$cache2 == null)
            {
                <>f__mg$cache2 = new Builder.OnTurnBasedMatchEventCallback(GameServicesBuilder.InternalOnTurnBasedMatchEventCallback);
            }
            Builder.GameServices_Builder_SetOnTurnBasedMatchEvent(base.SelfPtr(), <>f__mg$cache2, ptr);
        }

        internal void SetShowConnectingPopup(bool flag)
        {
            Builder.GameServices_Builder_SetShowConnectingPopup(base.SelfPtr(), flag);
        }

        internal delegate void AuthFinishedCallback(Types.AuthOperation operation, CommonErrorStatus.AuthStatus status);

        internal delegate void AuthStartedCallback(Types.AuthOperation operation);
    }
}

