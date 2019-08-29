using System;
using UnityEngine;

public class BoxCameraCtrl : MonoBehaviour
{
    private Camera mCamera;

    private void Awake()
    {
        this.mCamera = base.GetComponent<Camera>();
        this.mCamera.orthographicSize *= GameLogic.WidthScale;
    }

    private void Update()
    {
    }
}

