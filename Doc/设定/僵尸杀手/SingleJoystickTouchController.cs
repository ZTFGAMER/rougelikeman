using System;
using UnityEngine;
using UnityEngine.UI;

public class SingleJoystickTouchController : MonoBehaviour
{
    public Image singleJoystickBackgroundImage;
    public bool singleJoyStickAlwaysVisible;
    private Image singleJoystickHandleImage;
    private SingleJoystick singleJoystick;
    private int singleSideFingerID = -1;

    private void FixedUpdate()
    {
        if (Input.touchCount > 0)
        {
            Touch[] touches = Input.touches;
            for (int i = 0; i < Input.touchCount; i++)
            {
                if ((touches[i].phase == TouchPhase.Began) && (this.singleSideFingerID == -1))
                {
                    this.singleSideFingerID = touches[i].fingerId;
                    Vector3 position = this.singleJoystickBackgroundImage.get_rectTransform().position;
                    position.x = touches[i].position.x + (this.singleJoystickBackgroundImage.get_rectTransform().sizeDelta.x / 2f);
                    position.y = touches[i].position.y - (this.singleJoystickBackgroundImage.get_rectTransform().sizeDelta.y / 2f);
                    position.x = Mathf.Clamp(position.x, this.singleJoystickBackgroundImage.get_rectTransform().sizeDelta.x, (float) Screen.width);
                    position.y = Mathf.Clamp(position.y, 0f, Screen.height - this.singleJoystickBackgroundImage.get_rectTransform().sizeDelta.y);
                    this.singleJoystickBackgroundImage.get_rectTransform().position = position;
                    this.singleJoystickBackgroundImage.enabled = true;
                    this.singleJoystickBackgroundImage.get_rectTransform().GetChild(0).GetComponent<Image>().enabled = true;
                }
                if ((touches[i].phase == TouchPhase.Ended) && (touches[i].fingerId == this.singleSideFingerID))
                {
                    this.singleJoystickBackgroundImage.enabled = this.singleJoyStickAlwaysVisible;
                    this.singleJoystickHandleImage.enabled = this.singleJoyStickAlwaysVisible;
                    this.singleSideFingerID = -1;
                }
            }
        }
    }

    private void Start()
    {
        if (this.singleJoystickBackgroundImage.GetComponent<SingleJoystick>() == null)
        {
            Debug.LogError("There is no joystick attached to this script.");
        }
        else
        {
            this.singleJoystick = this.singleJoystickBackgroundImage.GetComponent<SingleJoystick>();
            this.singleJoystickBackgroundImage.enabled = this.singleJoyStickAlwaysVisible;
        }
        if (this.singleJoystick.transform.GetChild(0).GetComponent<Image>() == null)
        {
            Debug.LogError("There is no joystick handle (knob) attached to this script.");
        }
        else
        {
            this.singleJoystickHandleImage = this.singleJoystick.transform.GetChild(0).GetComponent<Image>();
            this.singleJoystickHandleImage.enabled = this.singleJoyStickAlwaysVisible;
        }
    }

    private void Update()
    {
    }
}

