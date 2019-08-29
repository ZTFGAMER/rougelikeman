namespace Spine.Unity.Examples
{
    using System;
    using UnityEngine;

    public class ConstrainedCamera : MonoBehaviour
    {
        public Transform target;
        public Vector3 offset;
        public Vector3 min;
        public Vector3 max;
        public float smoothing = 5f;

        private void LateUpdate()
        {
            Vector3 b = this.target.position + this.offset;
            b.x = Mathf.Clamp(b.x, this.min.x, this.max.x);
            b.y = Mathf.Clamp(b.y, this.min.y, this.max.y);
            b.z = Mathf.Clamp(b.z, this.min.z, this.max.z);
            base.transform.position = Vector3.Lerp(base.transform.position, b, this.smoothing * Time.deltaTime);
        }
    }
}

