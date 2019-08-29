namespace BestHTTP.Examples
{
    using BestHTTP;
    using System;
    using UnityEngine;

    public sealed class TextureDownloadSample : MonoBehaviour
    {
        private const string BaseURL = "https://besthttp.azurewebsites.net/Content/";
        private string[] Images = new string[] { "One.png", "Two.png", "Three.png", "Four.png", "Five.png", "Six.png", "Seven.png", "Eight.png", "Nine.png" };
        private Texture2D[] Textures = new Texture2D[9];
        private bool allDownloadedFromLocalCache;
        private int finishedCount;
        private Vector2 scrollPos;

        private void Awake()
        {
            HTTPManager.MaxConnectionPerServer = 1;
            for (int i = 0; i < this.Images.Length; i++)
            {
                this.Textures[i] = new Texture2D(100, 150);
            }
        }

        private void DownloadImages()
        {
            this.allDownloadedFromLocalCache = true;
            this.finishedCount = 0;
            for (int i = 0; i < this.Images.Length; i++)
            {
                this.Textures[i] = new Texture2D(100, 150);
                new HTTPRequest(new Uri("https://besthttp.azurewebsites.net/Content/" + this.Images[i]), new OnRequestFinishedDelegate(this.ImageDownloaded)) { Tag = this.Textures[i] }.Send();
            }
        }

        private void ImageDownloaded(HTTPRequest req, HTTPResponse resp)
        {
            this.finishedCount++;
            switch (req.State)
            {
                case HTTPRequestStates.Finished:
                    if (!resp.IsSuccess)
                    {
                        Debug.LogWarning($"Request finished Successfully, but the server sent an error. Status Code: {resp.StatusCode}-{resp.Message} Message: {resp.DataAsText}");
                        break;
                    }
                    (req.Tag as Texture2D).LoadImage(resp.Data);
                    this.allDownloadedFromLocalCache = this.allDownloadedFromLocalCache && resp.IsFromCache;
                    break;

                case HTTPRequestStates.Error:
                    Debug.LogError("Request Finished with Error! " + ((req.Exception == null) ? "No Exception" : (req.Exception.Message + "\n" + req.Exception.StackTrace)));
                    break;

                case HTTPRequestStates.Aborted:
                    Debug.LogWarning("Request Aborted!");
                    break;

                case HTTPRequestStates.ConnectionTimedOut:
                    Debug.LogError("Connection Timed Out!");
                    break;

                case HTTPRequestStates.TimedOut:
                    Debug.LogError("Processing the request Timed Out!");
                    break;
            }
        }

        private void OnDestroy()
        {
            HTTPManager.MaxConnectionPerServer = 4;
        }

        private void OnGUI()
        {
            GUIHelper.DrawArea(GUIHelper.ClientArea, true, delegate {
                this.scrollPos = GUILayout.BeginScrollView(this.scrollPos, Array.Empty<GUILayoutOption>());
                GUILayout.SelectionGrid(0, this.Textures, 3, Array.Empty<GUILayoutOption>());
                if ((this.finishedCount == this.Images.Length) && this.allDownloadedFromLocalCache)
                {
                    GUIHelper.DrawCenteredText("All images loaded from the local cache!");
                }
                GUILayout.FlexibleSpace();
                GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
                GUILayoutOption[] optionArray1 = new GUILayoutOption[] { GUILayout.Width(150f) };
                GUILayout.Label("Max Connection/Server: ", optionArray1);
                GUILayoutOption[] optionArray2 = new GUILayoutOption[] { GUILayout.Width(20f) };
                GUILayout.Label(HTTPManager.MaxConnectionPerServer.ToString(), optionArray2);
                HTTPManager.MaxConnectionPerServer = (byte) GUILayout.HorizontalSlider((float) HTTPManager.MaxConnectionPerServer, 1f, 10f, Array.Empty<GUILayoutOption>());
                GUILayout.EndHorizontal();
                if (GUILayout.Button("Start Download", Array.Empty<GUILayoutOption>()))
                {
                    this.DownloadImages();
                }
                GUILayout.EndScrollView();
            });
        }
    }
}

