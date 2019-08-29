namespace AudienceNetwork
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class AdChoices : MonoBehaviour
    {
        [Header("Ad Choices:"), SerializeField]
        private Image image;
        [SerializeField]
        private Text text;
        [SerializeField]
        private CanvasGroup canvasGroup;
        private string linkURL;

        public void AdChoicesTapped()
        {
            Application.OpenURL(this.linkURL);
        }

        private void Awake()
        {
            this.canvasGroup.alpha = 0f;
            this.canvasGroup.interactable = false;
        }

        public void SetNativeAd(NativeAd nativeAd)
        {
            this.image.set_sprite(nativeAd.AdChoicesImage);
            this.text.text = nativeAd.AdChoicesText;
            this.linkURL = nativeAd.AdChoicesLinkURL;
            this.canvasGroup.alpha = 1f;
            this.canvasGroup.interactable = true;
        }
    }
}

