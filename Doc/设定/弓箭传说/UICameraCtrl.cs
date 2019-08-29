using System;
using UnityEngine;

public class UICameraCtrl : MonoBehaviour
{
    public Camera mCamera;

    private void Start()
    {
        this.mCamera.orthographicSize = GameLogic.Height / 2;
        base.transform.localPosition = new Vector3((float) (GameLogic.Width / 2), (float) (GameLogic.Height / 2), -100f);
        Object.Destroy(this);
    }
}

