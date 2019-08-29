namespace MagicArsenal
{
    using System;
    using UnityEngine;

    public class MagicRotation : MonoBehaviour
    {
        [Header("Rotate axises by degrees per second")]
        public Vector3 rotateVector = Vector3.zero;
        public spaceEnum rotateSpace;

        private void Start()
        {
        }

        private void Update()
        {
            if (this.rotateSpace == spaceEnum.Local)
            {
                base.transform.Rotate(this.rotateVector * Time.deltaTime);
            }
            if (this.rotateSpace == spaceEnum.World)
            {
                base.transform.Rotate(this.rotateVector * Time.deltaTime, Space.World);
            }
        }

        public enum spaceEnum
        {
            Local,
            World
        }
    }
}

