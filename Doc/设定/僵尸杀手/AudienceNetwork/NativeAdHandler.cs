namespace AudienceNetwork
{
    using System;
    using UnityEngine;

    [RequireComponent(typeof(RectTransform))]
    public class NativeAdHandler : AdHandler
    {
        public int minViewabilityPercentage;
        public float minAlpha;
        public int maxRotation;
        public int checkViewabilityInterval;
        public Camera camera;
        public FBNativeAdHandlerValidationCallback validationCallback;
        private float lastImpressionCheckTime;
        private bool impressionLogged;
        private bool shouldCheckImpression;

        private bool checkGameObjectViewability(Camera camera, GameObject gameObject)
        {
            if (gameObject == null)
            {
                return this.logViewability(false, "GameObject is null.");
            }
            if (camera == null)
            {
                return this.logViewability(false, "Camera is null.");
            }
            if (!gameObject.activeInHierarchy)
            {
                return this.logViewability(false, "GameObject is not active in hierarchy.");
            }
            Canvas canvas = this.getCanvas(gameObject);
            if (canvas == null)
            {
                return this.logViewability(false, "GameObject is missing a Canvas parent.");
            }
            foreach (CanvasGroup group in gameObject.GetComponents<CanvasGroup>())
            {
                if (group.alpha < this.minAlpha)
                {
                    return this.logViewability(false, "GameObject has a CanvasGroup with less than the minimum alpha required.");
                }
            }
            RectTransform transform = gameObject.transform as RectTransform;
            if ((transform.rect.width <= 0f) || (transform.rect.height <= 0f))
            {
                return this.logViewability(false, "GameObject's height/width is less than or equal to zero.");
            }
            Vector3[] fourCornersArray = new Vector3[4];
            transform.GetWorldCorners(fourCornersArray);
            Vector3 position = fourCornersArray[0];
            Vector3 vector2 = fourCornersArray[2];
            Vector3 min = (Vector3) camera.pixelRect.min;
            Vector3 max = (Vector3) camera.pixelRect.max;
            if (canvas.renderMode != RenderMode.ScreenSpaceOverlay)
            {
                position = camera.WorldToScreenPoint(position);
                vector2 = camera.WorldToScreenPoint(vector2);
            }
            if ((position.x < min.x) || (vector2.x > max.x))
            {
                return this.logViewability(false, "Less than 100% of the width of the GameObject is inside the viewport.");
            }
            int num2 = (int) ((camera.pixelRect.height * (100 - this.minViewabilityPercentage)) / 100f);
            if ((max.y - vector2.y) > num2)
            {
                return this.logViewability(false, "Less than " + this.minViewabilityPercentage + "% visible from the top.");
            }
            if ((position.y - min.y) > num2)
            {
                return this.logViewability(false, "Less than " + this.minViewabilityPercentage + "% visible from the bottom.");
            }
            Vector3 eulerAngles = transform.eulerAngles;
            int num3 = Mathf.FloorToInt(eulerAngles.x);
            int num4 = Mathf.FloorToInt(eulerAngles.y);
            int num5 = Mathf.FloorToInt(eulerAngles.z);
            int num6 = 360 - this.maxRotation;
            int maxRotation = this.maxRotation;
            if ((num3 < num6) && (num3 > maxRotation))
            {
                return this.logViewability(false, "GameObject is rotated too much. (x axis)");
            }
            if ((num4 < num6) && (num4 > maxRotation))
            {
                return this.logViewability(false, "GameObject is rotated too much. (y axis)");
            }
            if ((num5 < num6) && (num5 > maxRotation))
            {
                return this.logViewability(false, "GameObject is rotated too much. (z axis)");
            }
            return this.logViewability(true, "--------------- VALID IMPRESSION REGISTERED! ----------------------");
        }

        private bool checkImpression()
        {
            float time = Time.time;
            float num2 = time - this.lastImpressionCheckTime;
            if ((this.shouldCheckImpression && !this.impressionLogged) && (num2 > this.checkViewabilityInterval))
            {
                this.lastImpressionCheckTime = time;
                GameObject gameObject = base.gameObject;
                Camera component = this.camera;
                if (component == null)
                {
                    component = base.GetComponent<Camera>();
                }
                if (component == null)
                {
                    component = Camera.main;
                }
                while (gameObject != null)
                {
                    Canvas component = gameObject.GetComponent<Canvas>();
                    if ((component != null) && (component.renderMode == RenderMode.WorldSpace))
                    {
                        break;
                    }
                    if (!this.checkGameObjectViewability(component, gameObject))
                    {
                        if (this.validationCallback != null)
                        {
                            this.validationCallback(false);
                        }
                        return false;
                    }
                    gameObject = null;
                }
                if (this.validationCallback != null)
                {
                    this.validationCallback(true);
                }
                this.impressionLogged = true;
            }
            return this.impressionLogged;
        }

        private Canvas getCanvas(GameObject gameObject)
        {
            if (gameObject.GetComponent<Canvas>() != null)
            {
                return gameObject.GetComponent<Canvas>();
            }
            if (gameObject.transform.parent != null)
            {
                return this.getCanvas(gameObject.transform.parent.gameObject);
            }
            return null;
        }

        private bool logViewability(bool success, string message)
        {
            if (!success)
            {
                Debug.Log("Viewability validation failed: " + message);
                return success;
            }
            Debug.Log("Viewability validation success! " + message);
            return success;
        }

        private void OnGUI()
        {
            this.checkImpression();
        }

        public void startImpressionValidation()
        {
            if (!base.enabled)
            {
                base.enabled = true;
            }
            this.shouldCheckImpression = true;
        }

        public void stopImpressionValidation()
        {
            this.shouldCheckImpression = false;
        }
    }
}

