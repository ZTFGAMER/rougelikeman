namespace BestHTTP.Examples
{
    using BestHTTP;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public sealed class AssetBundleSample : MonoBehaviour
    {
        private const string URL = "https://besthttp.azurewebsites.net/Content/AssetBundle.html";
        private string status = "Waiting for user interaction";
        private AssetBundle cachedBundle;
        private Texture2D texture;
        private bool downloading;

        [DebuggerHidden]
        private IEnumerator DownloadAssetBundle() => 
            new <DownloadAssetBundle>c__Iterator0 { $this = this };

        private void OnDestroy()
        {
            this.UnloadBundle();
        }

        private void OnGUI()
        {
            GUIHelper.DrawArea(GUIHelper.ClientArea, true, delegate {
                GUILayout.Label("Status: " + this.status, Array.Empty<GUILayoutOption>());
                if (this.texture != null)
                {
                    GUILayoutOption[] optionArray1 = new GUILayoutOption[] { GUILayout.MaxHeight(256f) };
                    GUILayout.Box(this.texture, optionArray1);
                }
                if (!this.downloading && GUILayout.Button("Start Download", Array.Empty<GUILayoutOption>()))
                {
                    this.UnloadBundle();
                    base.StartCoroutine(this.DownloadAssetBundle());
                }
            });
        }

        [DebuggerHidden]
        private IEnumerator ProcessAssetBundle(AssetBundle bundle) => 
            new <ProcessAssetBundle>c__Iterator1 { 
                bundle = bundle,
                $this = this
            };

        private void UnloadBundle()
        {
            if (this.cachedBundle != null)
            {
                this.cachedBundle.Unload(true);
                this.cachedBundle = null;
            }
        }

        [CompilerGenerated]
        private sealed class <DownloadAssetBundle>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal HTTPRequest <request>__0;
            internal HTTPRequestStates $locvar0;
            internal AssetBundleCreateRequest <async>__1;
            internal AssetBundleSample $this;
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
                        this.$this.downloading = true;
                        this.<request>__0 = new HTTPRequest(new Uri("https://besthttp.azurewebsites.net/Content/AssetBundle.html")).Send();
                        this.$this.status = "Download started";
                        break;

                    case 1:
                        this.$this.status = this.$this.status + ".";
                        break;

                    case 2:
                        this.$current = this.$this.StartCoroutine(this.$this.ProcessAssetBundle(this.<async>__1.get_assetBundle()));
                        if (!this.$disposing)
                        {
                            this.$PC = 3;
                        }
                        goto Label_02FF;

                    case 3:
                        goto Label_02EA;

                    default:
                        goto Label_02FD;
                }
                if (this.<request>__0.State < HTTPRequestStates.Finished)
                {
                    this.$current = new WaitForSeconds(0.1f);
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    goto Label_02FF;
                }
                this.$locvar0 = this.<request>__0.State;
                switch (this.$locvar0)
                {
                    case HTTPRequestStates.Finished:
                        if (!this.<request>__0.Response.IsSuccess)
                        {
                            this.$this.status = $"Request finished Successfully, but the server sent an error. Status Code: {this.<request>__0.Response.StatusCode}-{this.<request>__0.Response.Message} Message: {this.<request>__0.Response.DataAsText}";
                            Debug.LogWarning(this.$this.status);
                            break;
                        }
                        this.$this.status = $"AssetBundle downloaded! Loaded from local cache: {this.<request>__0.Response.IsFromCache.ToString()}";
                        this.<async>__1 = AssetBundle.LoadFromMemoryAsync(this.<request>__0.Response.Data);
                        this.$current = this.<async>__1;
                        if (!this.$disposing)
                        {
                            this.$PC = 2;
                        }
                        goto Label_02FF;

                    case HTTPRequestStates.Error:
                        this.$this.status = "Request Finished with Error! " + ((this.<request>__0.Exception == null) ? "No Exception" : (this.<request>__0.Exception.Message + "\n" + this.<request>__0.Exception.StackTrace));
                        Debug.LogError(this.$this.status);
                        break;

                    case HTTPRequestStates.Aborted:
                        this.$this.status = "Request Aborted!";
                        Debug.LogWarning(this.$this.status);
                        break;

                    case HTTPRequestStates.ConnectionTimedOut:
                        this.$this.status = "Connection Timed Out!";
                        Debug.LogError(this.$this.status);
                        break;

                    case HTTPRequestStates.TimedOut:
                        this.$this.status = "Processing the request Timed Out!";
                        Debug.LogError(this.$this.status);
                        break;
                }
            Label_02EA:
                this.$this.downloading = false;
                this.$PC = -1;
            Label_02FD:
                return false;
            Label_02FF:
                return true;
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

        [CompilerGenerated]
        private sealed class <ProcessAssetBundle>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal AssetBundle bundle;
            internal AssetBundleRequest <asyncAsset>__0;
            internal AssetBundleSample $this;
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
                        if (this.bundle != null)
                        {
                            this.$this.cachedBundle = this.bundle;
                            this.<asyncAsset>__0 = this.$this.cachedBundle.LoadAssetAsync("9443182_orig", typeof(Texture2D));
                            this.$current = this.<asyncAsset>__0;
                            if (!this.$disposing)
                            {
                                this.$PC = 1;
                            }
                            return true;
                        }
                        break;

                    case 1:
                        this.$this.texture = this.<asyncAsset>__0.get_asset() as Texture2D;
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

