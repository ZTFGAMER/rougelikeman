namespace Spine.Unity.Examples
{
    using System;
    using UnityEngine;

    public class SpineboyBeginnerInput : MonoBehaviour
    {
        public string horizontalAxis = "Horizontal";
        public string attackButton = "Fire1";
        public string jumpButton = "Jump";
        public SpineboyBeginnerModel model;

        private void OnValidate()
        {
            if (this.model == null)
            {
                this.model = base.GetComponent<SpineboyBeginnerModel>();
            }
        }

        private void Update()
        {
            if (this.model != null)
            {
                float axisRaw = Input.GetAxisRaw(this.horizontalAxis);
                this.model.TryMove(axisRaw);
                if (Input.GetButton(this.attackButton))
                {
                    this.model.TryShoot();
                }
                if (Input.GetButtonDown(this.jumpButton))
                {
                    this.model.TryJump();
                }
            }
        }
    }
}

