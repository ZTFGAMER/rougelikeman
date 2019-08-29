using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class SingleJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler, IEventSystemHandler
{
    [Tooltip("When checked, this joystick will stay in a fixed position.")]
    public bool joystickStaysInFixedPosition;
    [Tooltip("Sets the maximum distance the handle (knob) stays away from the center of this joystick. If the joystick handle doesn't look or feel right you can change this value. Must be a whole number. Default value is 4.")]
    public int joystickHandleDistance = 4;
    private Image bgImage;
    private Image joystickKnobImage;
    private Vector3 inputVector;
    private Vector3 unNormalizedInput;
    private Vector3[] fourCornersArray = new Vector3[4];
    private Vector2 bgImageStartPosition;

    public Vector3 GetInputDirection() => 
        new Vector3(this.inputVector.x, this.inputVector.y, 0f);

    private void OnDisable()
    {
        this.OnPointerUp(null);
    }

    public virtual void OnDrag(PointerEventData ped)
    {
        Vector2 zero = Vector2.zero;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(this.bgImage.get_rectTransform(), ped.get_position(), ped.get_pressEventCamera(), out zero))
        {
            zero.x /= this.bgImage.get_rectTransform().sizeDelta.x;
            zero.y /= this.bgImage.get_rectTransform().sizeDelta.y;
            this.inputVector = new Vector3((zero.x * 2f) + 1f, (zero.y * 2f) - 1f, 0f);
            this.unNormalizedInput = this.inputVector;
            this.inputVector = (this.inputVector.magnitude <= 1f) ? this.inputVector : this.inputVector.normalized;
            this.joystickKnobImage.get_rectTransform().anchoredPosition = new Vector3(this.inputVector.x * (this.bgImage.get_rectTransform().sizeDelta.x / ((float) this.joystickHandleDistance)), this.inputVector.y * (this.bgImage.get_rectTransform().sizeDelta.y / ((float) this.joystickHandleDistance)));
            if (!this.joystickStaysInFixedPosition && (this.unNormalizedInput.magnitude > this.inputVector.magnitude))
            {
                Vector3 position = this.bgImage.get_rectTransform().position;
                position.x += ped.get_delta().x;
                position.y += ped.get_delta().y;
                position.x = Mathf.Clamp(position.x, this.bgImage.get_rectTransform().sizeDelta.x, (float) Screen.width);
                position.y = Mathf.Clamp(position.y, 0f, Screen.height - this.bgImage.get_rectTransform().sizeDelta.y);
                this.bgImage.get_rectTransform().position = position;
            }
            CrossPlatformInputManager.SetAxis("Horizontal", this.inputVector.x);
            CrossPlatformInputManager.SetAxis("Vertical", this.inputVector.y);
        }
    }

    public virtual void OnPointerDown(PointerEventData ped)
    {
        this.OnDrag(ped);
    }

    public virtual void OnPointerUp(PointerEventData ped)
    {
        this.inputVector = Vector3.zero;
        this.joystickKnobImage.get_rectTransform().anchoredPosition = Vector3.zero;
        CrossPlatformInputManager.SetAxisZero("Horizontal");
        CrossPlatformInputManager.SetAxisZero("Vertical");
    }

    private void Start()
    {
        CrossPlatformInputManager.SetAxisZero("Horizontal");
        CrossPlatformInputManager.SetAxisZero("Vertical");
        if (base.GetComponent<Image>() == null)
        {
            Debug.LogError("There is no joystick image attached to this script.");
        }
        if (base.transform.GetChild(0).GetComponent<Image>() == null)
        {
            Debug.LogError("There is no joystick handle image attached to this script.");
        }
        if ((base.GetComponent<Image>() != null) && (base.transform.GetChild(0).GetComponent<Image>() != null))
        {
            this.bgImage = base.GetComponent<Image>();
            this.joystickKnobImage = base.transform.GetChild(0).GetComponent<Image>();
            this.bgImage.get_rectTransform().SetAsLastSibling();
            this.bgImage.get_rectTransform().GetWorldCorners(this.fourCornersArray);
            this.bgImageStartPosition = this.fourCornersArray[3];
            this.bgImage.get_rectTransform().pivot = new Vector2(1f, 0f);
            this.bgImage.get_rectTransform().position = (Vector3) this.bgImageStartPosition;
        }
    }
}

