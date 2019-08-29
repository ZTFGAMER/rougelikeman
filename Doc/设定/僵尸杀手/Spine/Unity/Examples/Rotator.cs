namespace Spine.Unity.Examples
{
    using System;
    using UnityEngine;

    public class Rotator : MonoBehaviour
    {
        public Vector3 direction = new Vector3(0f, 0f, 1f);
        public float speed = 1f;

        private void Update()
        {
            base.transform.Rotate(this.direction * ((this.speed * Time.deltaTime) * 100f));
        }
    }
}

