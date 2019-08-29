using System;
using UnityEngine;

public class PopupView : MonoBehaviour
{
    [Tooltip("market://details?id=BUNDLE-ID")]
    public string gameLink = "market://details?id=com.woodensword.zombie";

    public void OnDialogPopUp()
    {
        NativeReviewRequest.RequestReview();
    }

    private void OnDialogPopupComplete(MessageState state)
    {
        if (state != MessageState.YES)
        {
            if (state == MessageState.NO)
            {
                Debug.Log("No button pressed");
            }
        }
        else
        {
            Debug.Log("Yes button pressed");
        }
    }

    private void OnDisable()
    {
        AndroidRateUsPopUp.onRateUSPopupComplete -= new AndroidRateUsPopUp.OnRateUSPopupComplete(this.OnRateUSPopupComplete);
        AndroidDialog.onDialogPopupComplete -= new AndroidDialog.OnDialogPopupComplete(this.OnDialogPopupComplete);
        AndroidMessage.onMessagePopupComplete -= new AndroidMessage.OnMessagePopupComplete(this.OnMessagePopupComplete);
    }

    private void OnEnable()
    {
        AndroidRateUsPopUp.onRateUSPopupComplete += new AndroidRateUsPopUp.OnRateUSPopupComplete(this.OnRateUSPopupComplete);
        AndroidDialog.onDialogPopupComplete += new AndroidDialog.OnDialogPopupComplete(this.OnDialogPopupComplete);
        AndroidMessage.onMessagePopupComplete += new AndroidMessage.OnMessagePopupComplete(this.OnMessagePopupComplete);
    }

    public void OnMessagePopUp()
    {
        NativeMessage message = new NativeMessage("TheAppGuruz", "Welcome To TheAppGuruz");
    }

    private void OnMessagePopupComplete(MessageState state)
    {
        Debug.Log("Ok button Clicked");
    }

    public void OnRatePopUp()
    {
        NativeRateUS eus = new NativeRateUS("Like this game?", "Please rate to support future updates!");
        eus.SetAppLink(this.gameLink);
        eus.InitRateUS();
    }

    private void OnRateUSPopupComplete(MessageState state)
    {
        if (state != MessageState.RATED)
        {
            if (state == MessageState.REMIND)
            {
                Debug.Log("Remind Button pressed");
            }
            else if (state == MessageState.DECLINED)
            {
                Debug.Log("Declined Button pressed");
            }
        }
        else
        {
            Debug.Log("Rate Button pressed");
        }
    }
}

