namespace EPPZ.Cloud.Scenes.Helpers
{
    using System;
    using UnityEngine;

    [ExecuteInEditMode]
    public class CanvasScaleToScreenWidthConstraint : MonoBehaviour
    {
        public AnimationCurve canvasScaleToScreenWidth;

        private void Update()
        {
            base.GetComponent<CanvasScaler>().scaleFactor = this.canvasScaleToScreenWidth.Evaluate((float) Screen.width);
        }
    }
}

