using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class LoginVideoCtrl : MonoBehaviour
{
    public RawImage image;
    public VideoPlayer mPlayer;
    public Action OnPlayEnd;

    private void OnLoopPointReached(VideoPlayer video)
    {
        this.OnPlayEnd();
    }

    private void Start()
    {
        this.mPlayer.add_loopPointReached(new VideoPlayer.EventHandler(this, this.OnLoopPointReached));
    }

    private void Update()
    {
        if (((this.mPlayer != null) && (this.mPlayer.get_texture() != null)) && (this.image != null))
        {
            this.image.set_texture(this.mPlayer.get_texture());
        }
    }
}

