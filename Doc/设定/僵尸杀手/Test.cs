using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Test : MonoBehaviour
{
    [SerializeField]
    private Transform cameraTarget;

    private void Update()
    {
        if (Application.isEditor)
        {
            base.transform.position = this.cameraTarget.position + (new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")) * 10f);
        }
        else
        {
            base.transform.position = this.cameraTarget.position + (new Vector3(CrossPlatformInputManager.GetAxis("Horizontal"), 0f, CrossPlatformInputManager.GetAxis("Vertical")) * 10f);
        }
    }
}

