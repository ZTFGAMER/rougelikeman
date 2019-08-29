namespace Facebook.Unity.Example
{
    using Facebook.Unity;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    internal class GraphRequest : MenuBase
    {
        private string apiQuery = string.Empty;
        private Texture2D profilePic;

        protected override void GetGui()
        {
            bool flag = GUI.get_enabled();
            GUI.set_enabled(flag && FB.IsLoggedIn);
            if (base.Button("Basic Request - Me"))
            {
                FB.API("/me", HttpMethod.GET, new FacebookDelegate<IGraphResult>(this.HandleResult), (IDictionary<string, string>) null);
            }
            if (base.Button("Retrieve Profile Photo"))
            {
                FB.API("/me/picture", HttpMethod.GET, new FacebookDelegate<IGraphResult>(this.ProfilePhotoCallback), (IDictionary<string, string>) null);
            }
            if (base.Button("Take and Upload screenshot"))
            {
                base.StartCoroutine(this.TakeScreenshot());
            }
            base.LabelAndTextField("Request", ref this.apiQuery);
            if (base.Button("Custom Request"))
            {
                FB.API(this.apiQuery, HttpMethod.GET, new FacebookDelegate<IGraphResult>(this.HandleResult), (IDictionary<string, string>) null);
            }
            if (this.profilePic != null)
            {
                GUILayout.Box(this.profilePic, Array.Empty<GUILayoutOption>());
            }
            GUI.set_enabled(flag);
        }

        private void ProfilePhotoCallback(IGraphResult result)
        {
            if (string.IsNullOrEmpty(result.Error) && (result.get_Texture() != null))
            {
                this.profilePic = result.get_Texture();
            }
            base.HandleResult(result);
        }

        [DebuggerHidden]
        private IEnumerator TakeScreenshot() => 
            new <TakeScreenshot>c__Iterator0 { $this = this };

        [CompilerGenerated]
        private sealed class <TakeScreenshot>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal int <width>__0;
            internal int <height>__0;
            internal Texture2D <tex>__0;
            internal byte[] <screenshot>__0;
            internal WWWForm <wwwForm>__0;
            internal GraphRequest $this;
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
                        this.$current = new WaitForEndOfFrame();
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;

                    case 1:
                        this.<width>__0 = Screen.width;
                        this.<height>__0 = Screen.height;
                        this.<tex>__0 = new Texture2D(this.<width>__0, this.<height>__0, TextureFormat.RGB24, false);
                        this.<tex>__0.ReadPixels(new Rect(0f, 0f, (float) this.<width>__0, (float) this.<height>__0), 0, 0);
                        this.<tex>__0.Apply();
                        this.<screenshot>__0 = this.<tex>__0.EncodeToPNG();
                        this.<wwwForm>__0 = new WWWForm();
                        this.<wwwForm>__0.AddBinaryData("image", this.<screenshot>__0, "InteractiveConsole.png");
                        this.<wwwForm>__0.AddField("message", "herp derp.  I did a thing!  Did I do this right?");
                        FB.API("me/photos", HttpMethod.POST, new FacebookDelegate<IGraphResult>(this.$this.HandleResult), this.<wwwForm>__0);
                        this.$PC = -1;
                        break;
                }
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

