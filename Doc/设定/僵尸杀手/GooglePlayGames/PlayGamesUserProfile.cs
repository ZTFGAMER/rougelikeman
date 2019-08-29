namespace GooglePlayGames
{
    using GooglePlayGames.OurUtils;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.SocialPlatforms;

    public class PlayGamesUserProfile : IUserProfile
    {
        private string mDisplayName;
        private string mPlayerId;
        private string mAvatarUrl;
        private volatile bool mImageLoading;
        private Texture2D mImage;

        internal PlayGamesUserProfile(string displayName, string playerId, string avatarUrl)
        {
            this.mDisplayName = displayName;
            this.mPlayerId = playerId;
            this.mAvatarUrl = avatarUrl;
            this.mImageLoading = false;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }
            PlayGamesUserProfile profile = obj as PlayGamesUserProfile;
            if (profile == null)
            {
                return false;
            }
            return StringComparer.Ordinal.Equals(this.mPlayerId, profile.mPlayerId);
        }

        public override int GetHashCode() => 
            (typeof(PlayGamesUserProfile).GetHashCode() ^ this.mPlayerId.GetHashCode());

        [DebuggerHidden]
        internal IEnumerator LoadImage() => 
            new <LoadImage>c__Iterator0 { $this = this };

        protected void ResetIdentity(string displayName, string playerId, string avatarUrl)
        {
            this.mDisplayName = displayName;
            this.mPlayerId = playerId;
            if (this.mAvatarUrl != avatarUrl)
            {
                this.mImage = null;
                this.mAvatarUrl = avatarUrl;
            }
            this.mImageLoading = false;
        }

        public override string ToString() => 
            $"[Player: '{this.mDisplayName}' (id {this.mPlayerId})]";

        public string userName =>
            this.mDisplayName;

        public string id =>
            this.mPlayerId;

        public bool isFriend =>
            true;

        public UserState state =>
            UserState.Online;

        public Texture2D image
        {
            get
            {
                if ((!this.mImageLoading && (this.mImage == null)) && !string.IsNullOrEmpty(this.AvatarURL))
                {
                    UnityEngine.Debug.Log("Starting to load image: " + this.AvatarURL);
                    this.mImageLoading = true;
                    PlayGamesHelperObject.RunCoroutine(this.LoadImage());
                }
                return this.mImage;
            }
        }

        public string AvatarURL =>
            this.mAvatarUrl;

        [CompilerGenerated]
        private sealed class <LoadImage>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal WWW <www>__1;
            internal PlayGamesUserProfile $this;
            internal object $current;
            internal bool $disposing;
            internal int $PC;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$disposing = true;
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        if (string.IsNullOrEmpty(this.$this.AvatarURL))
                        {
                            UnityEngine.Debug.Log("No URL found.");
                            this.$this.mImage = Texture2D.blackTexture;
                            this.$this.mImageLoading = false;
                            goto Label_010C;
                        }
                        this.<www>__1 = new WWW(this.$this.AvatarURL);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_0113;
                }
                if (!this.<www>__1.isDone)
                {
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    return true;
                }
                if (this.<www>__1.error == null)
                {
                    this.$this.mImage = this.<www>__1.texture;
                }
                else
                {
                    this.$this.mImage = Texture2D.blackTexture;
                    UnityEngine.Debug.Log("Error downloading image: " + this.<www>__1.error);
                }
                this.$this.mImageLoading = false;
            Label_010C:
                this.$PC = -1;
            Label_0113:
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current =>
                this.$current;

            object IEnumerator.Current =>
                this.$current;
        }
    }
}

